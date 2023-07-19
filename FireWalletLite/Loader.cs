using System.Diagnostics;
using System.Text.RegularExpressions;
using FireWallet;

namespace FireWalletLite;

public partial class Loader : Form
{
    #region Constants

    private readonly MainForm mainForm = new();
    private readonly bool hideScreen = true; // Hide screen or not (for debug)
    private readonly Process HSDProcess;

    #endregion
    public Loader()
    {
        InitializeComponent();

        var splashScreen = new SplashScreen(false);
        splashScreen.Show();
        Application.DoEvents();
        var start = DateTime.Now;
        // Install and load node
        var dir = mainForm.dir;
        HSDProcess = new Process();
        if (!Directory.Exists(dir)) Environment.Exit(1);
        var hsdPath = dir + "hsd\\bin\\hsd.exe";
        if (!Directory.Exists(dir + "hsd"))
        {
            var repositoryUrl = "https://github.com/handshake-org/hsd.git";
            var destinationPath = dir + "hsd";
            CloneRepository(repositoryUrl, destinationPath);
        }

        // Start HSD
        HSDProcess.StartInfo.RedirectStandardInput = true;
        HSDProcess.StartInfo.RedirectStandardOutput = false;
        HSDProcess.StartInfo.UseShellExecute = false;
        HSDProcess.StartInfo.FileName = "node.exe";
        HSDProcess.StartInfo.Arguments = dir + "hsd\\bin\\hsd --agent=FireWallet --spv --prefix " + dir + "\\hsd_data";
        HSDProcess.StartInfo.CreateNoWindow = hideScreen;
        if (hideScreen)
        {
            HSDProcess.StartInfo.RedirectStandardError = true;
            // Send errors to log
            HSDProcess.ErrorDataReceived += (sender, e) => mainForm.AddLog("HSD Error: " + e.Data);
        }
        else
        {
            HSDProcess.StartInfo.RedirectStandardError = false;
        }

        HSDProcess.Start();
        while ((DateTime.Now - start).TotalSeconds < 5)
        {
            Thread.Sleep(10);
            Application.DoEvents();
        }

        splashScreen.CloseSplash();
        while (!splashScreen.IsClosed)
        {
            Thread.Sleep(10);
            Application.DoEvents();
        }

        splashScreen.Dispose();
        mainForm.Show();
    }

    #region Git

    public void CloneRepository(string repositoryUrl, string destinationPath)
    {
        try
        {
            // Check if git is installed
            var testInstalled = new Process();
            testInstalled.StartInfo.FileName = "git";
            testInstalled.StartInfo.Arguments = "-v";
            testInstalled.StartInfo.RedirectStandardOutput = true;
            testInstalled.StartInfo.UseShellExecute = false;
            testInstalled.StartInfo.CreateNoWindow = true;
            testInstalled.Start();
            var outputInstalled = testInstalled.StandardOutput.ReadToEnd();
            testInstalled.WaitForExit();

            if (!outputInstalled.Contains("git version"))
            {
                mainForm.AddLog("Git is not installed");
                var notifyForm = new NotifyForm("Git is not installed\nPlease install it to install HSD dependencies",
                    "Install", "https://git-scm.com/download/win");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                Environment.Exit(21);
                return;
            }

            // Check if node installed
            testInstalled = new Process();
            testInstalled.StartInfo.FileName = "node";
            testInstalled.StartInfo.Arguments = "-v";
            testInstalled.StartInfo.RedirectStandardOutput = true;
            testInstalled.StartInfo.UseShellExecute = false;
            testInstalled.StartInfo.CreateNoWindow = true;
            testInstalled.Start();
            outputInstalled = testInstalled.StandardOutput.ReadToEnd();
            testInstalled.WaitForExit();

            if (!outputInstalled.Contains("v"))
            {
                mainForm.AddLog("Node is not installed");
                var notifyForm = new NotifyForm("Node is not installed\nPlease install it to install HSD dependencies",
                    "Install", "https://nodejs.org/en/download");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                Environment.Exit(22);
                return;
            }


            // Check if npm installed
            testInstalled = new Process();
            testInstalled.StartInfo.FileName = "cmd.exe";
            testInstalled.StartInfo.Arguments = "npm -v";
            testInstalled.StartInfo.RedirectStandardOutput = true;
            testInstalled.StartInfo.UseShellExecute = false;
            testInstalled.StartInfo.CreateNoWindow = false;
            testInstalled.Start();
            // Wait 3 seconds and then kill
            Thread.Sleep(3000);
            testInstalled.Kill();
            outputInstalled = testInstalled.StandardOutput.ReadToEnd();
            testInstalled.WaitForExit();
            if (Regex.IsMatch(outputInstalled, @"^\d+\.\d+\.\d+$"))
            {
                mainForm.AddLog("NPM is not installed");
                mainForm.AddLog(outputInstalled);
                var notifyForm = new NotifyForm("NPM is not installed\nPlease install it to install HSD dependencies",
                    "Install", "https://docs.npmjs.com/downloading-and-installing-node-js-and-npm");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                Environment.Exit(23);
                return;
            }

            mainForm.AddLog("Prerequisites installed");

            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "git";
            startInfo.Arguments = $"clone {repositoryUrl} {destinationPath}";

            if (repositoryUrl == "https://github.com/handshake-org/hsd.git")
                startInfo.Arguments = $"clone --depth 1 --branch latest {repositoryUrl} {destinationPath}";

            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = hideScreen;

            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            while (!process.HasExited) output += process.StandardOutput.ReadToEnd();
            var psiNpmRunDist = new ProcessStartInfo
            {
                FileName = "cmd",
                RedirectStandardInput = true,
                WorkingDirectory = destinationPath,
                CreateNoWindow = hideScreen
            };
            var pNpmRunDist = Process.Start(psiNpmRunDist);
            pNpmRunDist.StandardInput.WriteLine("npm install & exit");
            pNpmRunDist.WaitForExit();
        }
        catch (Exception ex)
        {
            mainForm.AddLog("Git/NPM Install FAILED");
            mainForm.AddLog(ex.Message);
            if (ex.Message.Contains("to start process 'git'"))
            {
                var notifyForm = new NotifyForm("Git is not installed\nPlease install it to install HSD dependencies",
                    "Install", "https://git-scm.com/download/win");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                Environment.Exit(21);
            }
            else if (ex.Message.Contains("to start process 'node'"))
            {
                var notifyForm = new NotifyForm("Node is not installed\nPlease install it to install HSD dependencies",
                    "Install", "https://nodejs.org/en/download");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                Environment.Exit(22);
            }
            else if (ex.Message.Contains("to start process 'npm'"))
            {
                var notifyForm = new NotifyForm("NPM is not installed\nPlease install it to install HSD dependencies",
                    "Install", "https://docs.npmjs.com/downloading-and-installing-node-js-and-npm");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                Environment.Exit(23);
            }
            else
            {
                var notifyForm = new NotifyForm("Git/NPM Install FAILED\nCheck logs for more details");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                Environment.Exit(24);
            }

        }
    }

    public bool CheckNodeInstalled()
    {
        try
        {
            // Create a new process to execute the 'node' command
            var process = new Process();
            process.StartInfo.FileName = "node";
            process.StartInfo.Arguments = "--version";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Start the process and read the output
            process.Start();
            var output = process.StandardOutput.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();

            // Check if the output contains a version number
            return !string.IsNullOrEmpty(output);
        }
        catch (Exception)
        {
            // An exception occurred, indicating that 'node' is not installed or accessible
            return false;
        }
    }

    #endregion
}