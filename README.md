# XDCKIT
Give A ⭐ To Support This Project.

[![GitHub Latest Release](https://img.shields.io/badge/Latest-Release-red)](https://github.com/XBM360/XDCKIT/releases)[![Join our Discord](https://img.shields.io/badge/join%20Us-discord-7289DA)](https://discord.gg/QvdmNnfQ86)


An open source libery designed to Emulate XDevkit libery extention to work exactly like the original with added features

Have Any Ideas Or Suggestions? Join Us On [Discord Server](https://discord.gg/QvdmNnfQ86)!
I Am Known As Serenity And Also TeddyHammer So If You wanna reach me Join Me On This Server
# Features

### Debugging Features

### FileSystem Features

### Memory Features


# Requirements
**1. An Internet Connection**

**2. know C# Programing language**

**3 .Understand How Xbox XDevkit Works**

## Code Example

```markdown
# Connecting
using XDCKIT;

namespace Custom_namespace
{
    public partial class Classnamehere : Form
    {
        public static XboxConsole ConsoleX;
    
        public Classnamehere()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, ItemClickEventArgs e)
        {
            Console.Connect() //Attemps to Find Console 192.168.0.X Attempts compensate for X aka finds last digit.
            Console.Connect(Provide_IP) //User Provides Costume IP Address
            Console.Connect(Provide_IP,provide_CostumePort) //User Provides The IP Address and Port Number "Port Number Is Always 730" Regardless was added for more flexibility.
            Connect(this XboxConsole Source, out XboxConsole Client, string ConsoleNameOrIP = "default", int Port = 730) //sets the (ConsoleX) to XDCKIT class so everythin can be called like so example: ConsoleX.Screenshot() , then if IP Address is Provide then it proceeds to Connect also you can provide Port Number Witch By Again Defualt Is 730.
        }
   }
}
```
[![Click Me!](https://img.shields.io/badge/Click-Me!-blue)](https://xbm360.github.io/XDCKIT/) For More Example's
## Quick Guide

### Getting Started

You can Either Build Yourself By Downloading the Source Or Just Grabbing The Latest Dll Extention.

### Connection

Have To Be under The Same Local Network and Make Sure both Devices Are Connected To Same Wifi Name Or If Wired make sure to be under the same Router.

### Plugin Requirements

xbdm.xex

## Contributors
* [ohhsodead](https://github.com/ohhsodead) - Help Provided Code Enhancements And Help Performance Issues
## Disclaimer
I have no liability for any damages done to your system by using this extention.

## License

This project is released under the GNU General Public License v3.
