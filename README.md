# FireWalletLite
A lite wallet for Handshake.  
This is aimed to be mainly used for holding HNS and domains without sending anything.

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