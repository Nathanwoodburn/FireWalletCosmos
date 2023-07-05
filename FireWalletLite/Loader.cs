using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using FireWallet;

namespace FireWalletLite
{
    public partial class Loader : Form
    {
        #region Constants
        MainForm mainForm;
        bool hideScreen = false;
        Process HSDProcess;
        #endregion

        public Loader()
        {
            InitializeComponent();
            mainForm = new MainForm();

            SplashScreen splashScreen = new SplashScreen(false);
            splashScreen.Show();
            Application.DoEvents();

            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 5)
            {
                Thread.Sleep(10);
                Application.DoEvents();
            }

            // Install and load node
            string dir = mainForm.dir;
            HSDProcess = new Process();
            if (!Directory.Exists(dir)) Environment.Exit(1);

            string hsdPath = dir + "hsd\\bin\\hsd.exe";
            if (!Directory.Exists(dir + "hsd"))
            {
                string repositoryUrl = "https://github.com/handshake-org/hsd.git";
                string destinationPath = dir + "hsd";
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

            splashScreen.CloseSplash();
            while (!splashScreen.IsClosed)
            {
                Thread.Sleep(10);
                Application.DoEvents();
            }
            splashScreen.Dispose();
        }

        private void Loader_Load(object sender, EventArgs e)
        {
            this.Hide();
            mainForm.ShowDialog();
            // Close HSD
            if (HSDProcess != null)
            {
                try
                {
                    HSDProcess.Kill();
                    Thread.Sleep(1000);
                }
                catch
                {
                    Environment.Exit(90);
                }
                try
                {
                    HSDProcess.Dispose();
                }
                catch
                {
                    Environment.Exit(90);
                }
            }
            Environment.Exit(0);
        }

        #region Git
        public void CloneRepository(string repositoryUrl, string destinationPath)
        {
            try
            {
                // Check if git is installed
                Process testInstalled = new Process();
                testInstalled.StartInfo.FileName = "git";
                testInstalled.StartInfo.Arguments = "-v";
                testInstalled.StartInfo.RedirectStandardOutput = true;
                testInstalled.StartInfo.UseShellExecute = false;
                testInstalled.StartInfo.CreateNoWindow = true;
                testInstalled.Start();
                string outputInstalled = testInstalled.StandardOutput.ReadToEnd();
                testInstalled.WaitForExit();

                if (!outputInstalled.Contains("git version"))
                {
                    mainForm.AddLog("Git is not installed");
                    NotifyForm notifyForm = new NotifyForm("Git is not installed\nPlease install it to install HSD dependencies", "Install", "https://git-scm.com/download/win");
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
                    NotifyForm notifyForm = new NotifyForm("Node is not installed\nPlease install it to install HSD dependencies", "Install", "https://nodejs.org/en/download");
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
                    NotifyForm notifyForm = new NotifyForm("NPM is not installed\nPlease install it to install HSD dependencies", "Install", "https://docs.npmjs.com/downloading-and-installing-node-js-and-npm");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                    Environment.Exit(23);
                    return;
                }

                mainForm.AddLog("Prerequisites installed");

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "git";
                startInfo.Arguments = $"clone {repositoryUrl} {destinationPath}";

                if (repositoryUrl == "https://github.com/handshake-org/hsd.git")
                {
                    startInfo.Arguments = $"clone --depth 1 --branch latest {repositoryUrl} {destinationPath}";
                }

                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = hideScreen;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                while (!process.HasExited)
                {
                    output += process.StandardOutput.ReadToEnd();
                }
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
                    NotifyForm notifyForm = new NotifyForm("Git needs to be installed\nCheck logs for more details");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                else if (ex.Message.Contains("to start process 'node'"))
                {
                    NotifyForm notifyForm = new NotifyForm("Node needs to be installed\nCheck logs for more details");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                else if (ex.Message.Contains("to start process 'npm'"))
                {
                    NotifyForm notifyForm = new NotifyForm("NPM needs to be installed\nCheck logs for more details");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                else
                {

                    NotifyForm notifyForm = new NotifyForm("Git/NPM Install FAILED\nCheck logs for more details");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                Environment.Exit(24);
            }
        }
        public bool CheckNodeInstalled()
        {
            try
            {
                // Create a new process to execute the 'node' command
                Process process = new Process();
                process.StartInfo.FileName = "node";
                process.StartInfo.Arguments = "--version";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                // Start the process and read the output
                process.Start();
                string output = process.StandardOutput.ReadToEnd();

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
}