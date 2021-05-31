//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2016.
//Thank You for looking love you guys...

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;

namespace XDCKIT
{
    /// <summary>
    /// Xbox Emulation Class
    /// Made By TeddyHammer
    /// </summary>
    public partial class XboxConsole //XboxFeatures
    {
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private const string XAMModule = "xam.xex", krnlModule = "xboxkrnl.exe";

        public string GetSMCVersion()
        {
            byte[] byte_0 = GetMemory(0x81AC7C50, 4);
            object[] objArray1 = new object[] { " ", byte_0[2], ".", byte_0[3] };
            return string.Concat(objArray1);
        }



        /// <summary>
        ///
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="MediaDirectory"></param>
        /// <param name="CmdLine"></param>
        /// <param name="Flags"></param>
        public void Reboot(string Name, string MediaDirectory, string CmdLine, XboxRebootFlags Flags)
        {

            string[] lines = Name.Split("\\".ToCharArray());
            for (int i = 0; i < lines.Length - 1; i++)
                MediaDirectory += lines[i] + "\\";
            object[] Reboot = new object[] { $"magicboot title=\"{Name}\" directory=\"{MediaDirectory}\"" };//TODO:
            SendTextCommand(string.Concat(Reboot));
        }

        /// <summary>
        /// Shortcuts To Guide
        /// </summary>
        /// <param name="Color"></param>
        public void XboxShortcut(XboxShortcuts UI)
        {

            if (Connected)
                switch (UI)//works by getting the int of the UI and matches the numbers to execute things
                {
                    case XboxShortcuts.XboxHome:
                        Reboot(@"\Device\Harddisk0\SystemExtPartition\20449700\dash.xex",
                               @"\Device\Harddisk0\SystemExtPartition\20449700\dash.xex",
                               @"\Device\Harddisk0\SystemExtPartition\20445700\dash.xex", XboxRebootFlags.Title);
                        break;
                    case XboxShortcuts.AvatarEditor:
                        Reboot(@"\Device\Harddisk0\SystemExtPartition\20449700\AvatarEditor.xex",
                               @"\Device\Harddisk0\SystemExtPartition\20449700\AvatarEditor.xex",
                               @"\Device\Harddisk0\SystemExtPartition\20445700\AvatarEditor.xex", XboxRebootFlags.Title);

                        break;
                    case XboxShortcuts.DriveSelector:
                        Reboot(@"\Device\Harddisk0\SystemExtPartition\20449700\signin.xex",
                               @"\Device\Harddisk0\SystemExtPartition\20449700\signin.xex",
                               @"\Device\Harddisk0\SystemExtPartition\20445700\signin.xex", XboxRebootFlags.Title);

                        break;

                    case XboxShortcuts.Turn_Off_Console:
                        ShutDown();
                        break;
                    case XboxShortcuts.Account_Management:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Account_Management),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Achievements:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Achievements),
                                      new object[]
                            { 0, 0, 0, 0 });//achievements
                        break;
                    case XboxShortcuts.Active_Downloads:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Active_Downloads),
                                      new object[]
                            { 0, 0, 0, 0 });//XamShowMarketplaceDownloadItemsUI
                        break;
                    case XboxShortcuts.Awards:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Awards),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Beacons_And_Activiy:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Beacons_And_Activiy),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Family_Settings:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Family_Settings),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Friends:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Friends),
                                      new object[]
                            { 0, 0, 0, 0 });//friends
                        break;
                    case XboxShortcuts.Guide_Button:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Guide_Button),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Messages:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Messages), 0);//messages tab
                        break;
                    case XboxShortcuts.My_Games:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.My_Games), new object[] { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Open_Tray:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Open_Tray), new object[] { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Close_Tray:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Close_Tray),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Party:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Party), new object[] { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Preferences:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Preferences),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Private_Chat:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Private_Chat),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Profile:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Profile),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Recent:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Recent),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Redeem_Code:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Redeem_Code),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Select_Music:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Select_Music),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.System_Music_Player:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.System_Music_Player),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.System_Settings:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.System_Settings),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.System_Video_Player:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.System_Video_Player),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                    case XboxShortcuts.Windows_Media_Center:
                        XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxShortcuts.Windows_Media_Center),
                                      new object[]
                            { 0, 0, 0, 0 });
                        break;
                }
        }

        /// <summary>
        /// Get's Box Id.
        /// </summary>
        public string GetBoxID()
        {

            return SendTextCommand("BOXID").Replace("200- ", string.Empty);
        }

        /// <summary>
        /// Turns The Console's Default Neighborhood Icon to any of the following...(black , blue , bluegray , nosidecar
        /// , white) Also Changes The Type Of Console It Is.
        /// </summary>
        /// <param name="Color"></param>
        public void SetConsoleColor(XboxColor Color)
        {

            SendTextCommand("setcolor name=" + Enum.GetName(typeof(int), Color).ToLower());
        }

        /// <summary>
        /// Get's The Consoles ID.
        /// </summary>
        /// <returns></returns>
        public string GetConsoleID()
        {

            return SendTextCommand(string.Concat("getconsoleid")).Replace("200- consoleid=", string.Empty);
        }

        /// <summary>
        /// Gets the debug Monitor version Number.
        /// </summary>
        public string GetDMVersion()
        {

            return SendTextCommand("dmversion").Replace("200- ", string.Empty);

        }

        /// <summary>
        /// Get's Consoles System Information.
        /// </summary>
        /// <param name="Type"></param>
        /// <returns>Type Is The System Type Of Information you Want To Retrieve</returns>
        public string GetSystemInfo(Info Type)
        {
            if (XboxClient.XboxName == null)
            {
                Console.WriteLine("Console Is Not Connnected...");
            }
            else
            {
                Console.WriteLine("System Info Came Threw.. (Command Executed == " + Type + " )");
                switch (Type)
                {
                    case Info.HDD:
                        #region HDD
                        try
                        {

                            string[] Info = new[] { SendTextCommand(string.Concat("systeminfo")) };
                            foreach (string s in Info)
                            {
                                int Start = s.find("hdd=");
                                int End = s.IndexOf("type=");
                                return s.Substring(4, End - 4);
                            }
                        }
                        catch
                        {
                        }
                        #endregion
                        break;
                    case Info.Type:
                        #region Console Type
                        try
                        {
                            return SendTextCommand(string.Concat("consoletype")).Replace("200- ", string.Empty);
                        }
                        catch
                        {
                        }
                        #endregion
                        break;
                    case Info.Platform:
                        #region Platform
                        try
                        {

                            string[] Info = new[] { SendTextCommand(string.Concat("systeminfo")) };
                            foreach (string s in Info)
                            {
                                int Start = s.IndexOf("type=");
                                int End = s.IndexOf(" p");
                                return s.Substring(Start + 9, End - 1).Substring(Start);
                            }
                        }
                        catch
                        {
                        }
                        #endregion
                        break;
                    case Info.System:
                        #region System
                        try
                        {

                            string[] Info = new[] { SendTextCommand(string.Concat("systeminfo")) };
                            foreach (string s in Info)
                            {
                                int Start = s.IndexOf("type=");
                                int End = s.IndexOf(" p");
                                return s.Substring(Start + End + 4, End - 4).Substring(Start);
                            }
                        }
                        catch
                        {
                        }
                        #endregion
                        break;
                    case Info.BaseKrnlVersion:
                        #region BaseKrnlVersion
                        try
                        {

                            string[] Info = new[] { SendTextCommand(string.Concat("systeminfo")) };
                            foreach (string s in Info)
                            {
                                int Start = s.IndexOf(" krnl=");
                                int End = s.IndexOf(" ");
                                return s.Substring(Start - 10, End);
                            }
                        }
                        catch
                        {
                        }
                        #endregion
                        break;
                    case Info.KrnlVersion:
                        #region Kernal Version
                        try
                        {

                            string[] Info = new[] { SendTextCommand(string.Concat("systeminfo")) };
                            foreach (string s in Info)
                            {
                                int Start = s.IndexOf(" krnl=");
                                int End = s.IndexOf(" ");
                                return s.Substring(Start + 6, End);
                            }
                        }
                        catch
                        {
                        }
                        #endregion
                        break;
                    case Info.XDKVersion:
                        #region XDK Version
                        try
                        {

                            string[] Info = new[] { SendTextCommand(string.Concat("systeminfo")) };
                            foreach (string s in Info)
                            {
                                return s.Substring(s.IndexOf("xdk=") + 4, 12);
                            }
                        }
                        catch
                        {
                        }
                        #endregion
                        break;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Reboot Method flag types cold or warm reboot.
        /// </summary>
        public void Reboot(XboxReboot Warm_or_Cold)
        {

            if (Warm_or_Cold == XboxReboot.Cold)
            {
                SendTextCommand("magicboot cold");
            }
            if (Warm_or_Cold == XboxReboot.Warm)
            {
                SendTextCommand("magicboot warm");
            }
        }

        /// <summary>
        /// Freezes/Stops Console.
        /// </summary>
        public void FreezeConsole(XboxSwitch Freeze)
        {

            if (Freeze == XboxSwitch.True)
            {
                SendTextCommand("stop");
            }
            else if (Freeze == XboxSwitch.False)
            {
                SendTextCommand("go");
            }
        }

        /// <summary>
        /// Allows the User To See The Type Of Console they own.
        /// </summary>
        /// <returns></returns>
        public string GetConsoleType()
        {

            if (Connected)
            {
                string str = string.Concat("consolefeatures ver=", 2, " type=17 params=\"A\\0\\A\\0\\\"");
                string str1 = SendTextCommand(str);
                return str1.Substring(str1.find(" ") + 1);
            }
            else
            {
                return "Error Not Connected!";
            }


        }
        public void CloseConnection()
        {

            XboxClient.Disconnect();
        }
        
        /// <summary>
        /// Reconnect Feature.
        /// </summary>
        private void Reconnect(int delay)
        {
            Thread.Sleep(delay);
            XboxClient.Reconnect();
        }
        /// <summary>
        /// Find's Console And Connects To Found IP does not set class
        /// </summary>
        private void OpenConnection(string port)
        {

            XboxClient.Connect("default", XboxClient.Port);
        }


        public void Get_GameTitle()
        {

        }

        /// <summary>
        /// Retrieve's The Console's Central Processing Unit Key.
        /// </summary>
        public string GetCPUKey()
        {

            string str = string.Concat("consolefeatures ver=", 2, " type=10 params=\"A\\0\\A\\0\\\"");
            return SendTextCommand(str).Replace("200- ", string.Empty);
        }


        /// <summary>
        /// Version Of Kernal
        /// </summary>
        /// <returns></returns>
        public uint GetKernalVersion()
        {

            string str = string.Concat("consolefeatures ver=", 2, " type=13 params=\"A\\0\\A\\0\\\"");
            string str1 = SendTextCommand(str);
            return uint.Parse(str1.Substring(str1.find(" ") + 1));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="TemperatureType"></param>
        /// <returns></returns>
        public uint GetTemperature(TemperatureFlag TemperatureType)//TODO: Rework this...
        {

            string Command = "consolefeatures ver=" + (uint)2 + " type=15 params=\"A\\0\\A\\1\\" + (uint)1 + "\\" + (int)TemperatureType + "\\\"";
            string String = SendTextCommand(Command);
            return uint.Parse(String.Substring(String.find(" ")), NumberStyles.HexNumber);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Top_Left"></param>
        /// <param name="Top_Right"></param>
        /// <param name="Bottom_Left"></param>
        /// <param name="Bottom_Right"></param>
        public void SetLeds(LEDState Top_Left, LEDState Top_Right, LEDState Bottom_Left, LEDState Bottom_Right)
        {

            object[] Resolver = new object[]
            {
                "consolefeatures ver=",
                2,
                " type=14 params=\"A\\0\\A\\4\\",
                1,
                "\\",
                (uint)Top_Left,
                "\\",
                1,
                "\\",
                (uint)Top_Right,
                "\\",
                1,
                "\\",
                (uint)Bottom_Left,
                "\\",
                1,
                "\\",
                (uint)Bottom_Right,
                "\\\""
            };
            SendTextCommand(string.Concat(Resolver));
        }
        private uint GetModuleHandle(string ModuleName)
        {
            object[] arguments = new object[] { ModuleName };
            return XboxExtention.Call<uint>(ModuleName, 0x44e, arguments);
        }
        private uint LaunchSystemDLLThread(string ThreadPath)
        {
            object[] arguments = new object[] { ThreadPath, 8, 0, 0 };
            return XboxExtention.Call<uint>(krnlModule, 0x199, arguments);
        }
        private void UnloadImage(string ModuleName, bool isSysDll)
        {
            uint moduleHandle = GetModuleHandle(ModuleName);
            if (moduleHandle != 0)
            {
                if (isSysDll)
                {
                    GetInt16(moduleHandle + 0x40, 1);
                }
                object[] arguments = new object[] { moduleHandle };
                XboxExtention.CallVoid(krnlModule, 0x1a1, arguments);
            }
        }
        private uint XexPcToFileHeader(uint baseAddress)
        {
            uint num3;
            uint address = ResolveFunction(krnlModule, 0x19c);
            if (address == 0)
            {
                num3 = 0;
            }
            else
            {
                uint num2 = ResolveFunction(XAMModule, 0xa29) + 0x3000;
                object[] arguments = new object[] { baseAddress, num2 };
                XboxExtention.CallVoid(address, arguments);
                num3 = GetUInt32(num2);
            }
            return num3;
        }
        /// <summary>
        /// Gets a list of modules loaded by the xbox.
        /// </summary>
        public List<ModuleInfo> Modules { get { return modules; } }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<ModuleInfo> modules = null;
        /// <summary>
        /// Gets the notification listener registered with the xbox that listens for incoming notification session requests.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TcpListener NotificationListener { get { return notificationListener; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly TcpListener notificationListener = null;

        /// <summary>
        /// Retrieves Current Title ID
        /// </summary>
        /// <returns></returns>
        public uint GetTitleID()
        {

            string str = string.Concat("consolefeatures ver=", 2, " type=16 params=\"A\\0\\A\\0\\\"");
            string str1 = SendTextCommand(str);
            return uint.Parse(str1.Substring(str1.find(" ") + 1), NumberStyles.HexNumber);
        }

        /// <summary>
        /// Turns Off Console.
        /// </summary>
        public void ShutDown()
        {
            try
            {
                string str = string.Concat("consolefeatures ver=", 2, " type=11 params=\"A\\0\\A\\0\\\"");
                SendTextCommand(str);
            }
            catch
            {
            }
        }
        /// <summary>
        /// Signs User in
        /// </summary>
        public void QuickSignIn()
        {

            XboxExtention.CallVoid(ResolveFunction(XAMModule, (int)XboxSignIn.QuickSignin), new object[] { 0, 0, 0, 0 });

        }

        /// <summary>
        /// Controls The Fan Speed.
        /// </summary>
        /// <param name="Value_1"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool FanSpeed(int Value_1, int input)
        {
            uint uint_0 = ResolveFunction("xboxkrnl.exe", 0x29);
            byte[] byte_0 = new byte[0x10];
            byte[] byte_1 = new byte[0x10];
            Array.Clear(byte_0, 0, byte_0.Length);
            Array.Clear(byte_1, 0, byte_1.Length);
            if (Value_1 == 1)
            {
                byte_0[0] = 0x94;
            }
            else
            {
                if (Value_1 != 2)
                {
                    return false;
                }
                byte_0[0] = 0x89;
            }
            if (input > 100)
            {
                input = 100;
            }
            if (input <= 0)
            {
                input = 10;
            }
            if (input < 0x2d)
            {
                byte_0[1] = 0x7f;
            }
            else
            {
                byte_0[1] = (byte)(input | 0x80);
            }
            object[] arguments = new object[2];
            arguments[0] = byte_0;
            XboxExtention.CallVoid(uint_0, arguments);
            return true;
        }
        /// <summary>
        /// Get's The State of User's Sign In
        /// </summary>
        public void GetSigninState()
        {
            ResolveFunction("xboxkrnl.exe", 528);
        }
    }
    /// <summary>
    /// Contains all native helper methods used by XboxConsole.
    /// </summary>
    public static class NativeMethods
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
    }
}