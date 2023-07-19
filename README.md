# FireWalletLite
A lite wallet for Handshake.  
This is aimed to be mainly used for holding HNS and domains without sending anything.  
For example if you want to gift a domain to someone, you can have them use this wallet to store the wallet.  
You will still need to renew the domains at least every 2 years.


## Features
First run flow:
- Create a default wallet (`Primary`)
- Show Seed phrase and ask user to write it down
- Require user to encrypt wallet with a password
- Login with password and show main page

Login only requires password as this wallet is made to only use the `primary` wallet.


This wallet supports
- Displaying balance
- Domain list and expiration date
- Renew expiring domains button
- Displaying receiving address
- Sending HNS
- Sign message 

This wallet does not (and will never) support
- Creating new wallets
- Auctions (bidding, revealing, etc)
- Multisig wallets
- Hardware wallets
- DNS management


If you want to use a wallet with more features, please use [Fire Wallet](https://firewallet.au) or [Bob Wallet](https://bobwallet.io) instead.

## Install
FireWalletLite is available for Windows only.  
You can download the latest prebuilt release from [here](https://git.woodburn.au/nathanwoodburn/FireWalletLite/releases).  
You should download the `FireWalletLite.zip` file and extract it to a folder then run the `setup.exe` file as this file will install the .net runtime if it is not already installed.  

**You can also build it yourself by cloning this repo and building it in Visual Studio.**