# XDCKIT
[![GitHub Latest Release](https://img.shields.io/badge/Latest-Release-red)](https://github.com/XBM360/XDCKIT/releases)[![Join our Discord](https://img.shields.io/badge/join%20Us-discord-7289DA)](https://discord.gg/QvdmNnfQ86)
An open source libery designed to Emulate XDevkit libery extention to work exactly like the original with added features

## Requirements
**An Internet Connection**
**know C# Programing language**
**Understand How Xbox XDevkit Works**
## Code Example

```markdown
# Connecting
using XDCKIT;

namespace Custom_namespace
{
    public partial class Classnamehere : Form
    {
        public static XboxConsole Console;
    
        public Classnamehere()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, ItemClickEventArgs e)
        {
            Console.Connect() or Console.Connect(Provide_IP) or Console.Connect(Provide_IP,provide_CostumePort) or
            Connect(this XboxConsole Source, out XboxConsole Client, string ConsoleNameOrIP = "default", int Port = 730)
        }
   }
}
```
[![Click Me!](https://img.shields.io/badge/Click-Me!-blue)](https://xbm360.github.io/XDCKIT/) For More Example's

## Contributors
* [ohhsodead](https://github.com/ohhsodead) - Help Provided Code Enhancements And Help Performance Issues
## Disclaimer
I have no liability for any damages done to your system by using this extention.

## License

This project is released under the GNU General Public License v3.
