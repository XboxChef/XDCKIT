#  Xbox Direct Connect Kit (XDCKIT)
Give A ⭐ To Support This Project.

[![GitHub Latest Release](https://img.shields.io/badge/Latest-Release-red)](https://github.com/XBM360/XDCKIT/releases)[![Join our Discord](https://img.shields.io/badge/join%20Us-discord-7289DA)](https://discord.gg/QvdmNnfQ86)


An open source library designed to Emulate or Imitate The XDevkit library extention to work exactly like the original with New added features

I Am Known As Serenity And Also TeddyHammer So If You wanna reach me Join Me On This Server
Have Any Ideas Or Suggestions? Join Us On [Discord Server](https://discord.gg/QvdmNnfQ86)!

# Features

### General Features
```markdown
1.  Get SMC Version
2.  Reboot Console Or Game Title
3.  XboxShortcuts (witch include Guide Button Press And Takes You Various Places On The Xbox Such As Friends List Etc)
4.  Get Box ID (Get's The Xbox's identification number)
5.  Set Console Color (Allows User to Turns The Console's Default Neighborhood Icon to any of the following... (black , blue , bluegray , nosidecar) 
6.  Get Console ID (Gets The Console's identification number)
7.  Get DM Version (Allows User To See The Debug Monitor Version)
8.  Get System Info (allows user see Various Console Information)
9.  Reboot Cold Or Warm (Allows user to Perform A Warm In A Very Fast Manner Or A Cold Reboot WItch Attempts To Reboot Slowly)
10. Freeze Console (Allows User Perform A Stop Or Go Witch Tempory Freezes The Console Until A Go Command Is Sent)
11. Get Console Type (Allows The User The See Witch Console Type They Own)
12. CloseConnection (Allows For The Connection From The Device To Be severed From Each Other)
13. Reconnect (Allows The User To Add A Delay So when The Console Is Ready The User Can Connect To It)
14. OpenConnection (Find's Console And Connects To the IP Found and does not set class meaning you would have to set ConsoleX TO XDCKIT)
15. Get CPU Key
16. Get Kernal Version
17. Get Temperature
18. Set LED State
19. Get Module Handle
20. Launch System DLL Thread
21. Unload Image
22. Xex Pc To File Header
23. Xam Get Current Title Id
24. ShutDown
25. Quick Sign In  (In Beta Testing Phase)
26. Fan Speed Control
27. Get Sign in State (In Beta Testing Phase)
28. Trainer Features witch allows The ability To Share Modified Files For Any Game To Be modded
```
### Debugging Features
```markdown
1.   NULL_Address (Allows The User To Add An Adress witch sets The Value To 6000000 meaning that you are making a null value)
2.   SetBreakpoint (In Development)
3.   RemoveBreakpoint (In Development)
4.   RemoveAllBreakpoints (In Development)
5.   SetInitialBreakpoint (In Development)
6.   SetDataBreakpoint (In Development)
7.   IsBreakpoint (In Development)
8.   Invalidate MemoryCache (In Development)
9.   Poke
10.  peek
11.  Find Hex Offset
12.  constant Memory Setting
13.  constant Memory Set
14.  Get Memory / Set Memory
15.  Dump Memory
16.  ResolveFunction
17.  ReverseBytes
18.  Bool {Get; Set;}
19.  String {Get; Set;}
20.  Float {Get; Set;}
21.  BinaryData {Get; Set;}
22.  Byte {Get; Set;}
23.  SByte {Get; Set;}
24.  All Int {Get; Set;}
25.  All Uint {Get; Set;}
26.  Double {Get; Set;}
27.  Long {Get; Set;}
28.  All XOR int / AND Int / OR Int {Get; Set;}
29.  WriteVector's
30.  WriteHook
31.  SendTextCommand
```
### Current FileSystem Features
```markdown
1.   ChangeTime
2.   CreationTime
3.   bool IsDirectory
4.   bool IsReadOnly
5.   GetFile Size(string directory)
6.   MakeDirectory
7.   RemoveDirectory(string path)
8.   DirectoryFiles
9.   Get FileName
10.  ReceiveBinaryData
11.  ReceiveBinaryData(int size)
12.  ReceiveBinaryData(byte[] data)
13.  ReceiveFile(string localName, string remoteName)
14.  SendBinaryData(byte[] data)
15.  SendBinaryData(byte[] data, int length)
16.  SendFile(string localName, string remoteName)
17.  RenameFile(string OldFileName, string NewFileName)
```
### XNOTIFY Features
```markdown
1.   Show (Aloows The Pgrammer To Use It as A Messagebox.Show and Contains The Ability To Turn Off Notifications at Any Moment If A Programmer Added a switch Example's Below)
2.   Show (Message , Logo , Switch)
3.   Show (Message , Switch)
4.   Show (Message , Logo)
5.   XMessage (Allow The Console To Display A MessageBox (In Development)) 
```
# So Many More Features!

# Requirements
**1. An Internet Connection**

**2. A know How Of C# Programing language Development**

**3. An Understand How The Xbox XDevkit Works**

**4. An Understanding On How To Work A Modified Xbox Console**

## Code Example

```C#
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

### Xbox 360 Plugin Requirements
xbdm.xex

### Computer Requirements
A Working Tool Using this Extention Properly
## Contributors
* [@ohhsodead](https://github.com/ohhsodead) - Help Provided Code Enhancements And Help Performance Issues
## Disclaimer
I have no liability for any damages done to your system by using this extention.
## License
This project is released under the GNU General Public License v3.
