using JRPC_Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using XDevkit;

namespace XRGHC
{
    public class Util
    {
        #region Misc
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static string method_8()
        {
            int num = 0;
            string str = Path.GetDirectoryName(Application.ExecutablePath) + @"\";
            if (Directory.Exists(str + @"Screenshots\"))
            {
                for (int i = 0; i < 0x270f; i++)
                {
                    string[] textArray1 = new string[] { str, @"Screenshots\", timeStamp, "-", num.ToString(), ".png" };
                    if (!File.Exists(string.Concat(textArray1)))
                    {
                        return num.ToString();
                    }
                    num++;
                }
            }
            return num.ToString();
        }

        #region Value Editors
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static byte[] NOP_4Bytes()
        {
            return new byte[] { 0x60, 0x00, 0x00, 0x00 };
        }
        #endregion

        public enum XMessageBoxIcons
        {
            XBM_NOICON = 0,
            XMB_ERRORICON = 1,
            XMB_WARNINGICON = 2,
            XMB_ALERTICON = 3
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class XMessageBoxUIProgress : EventArgs
        {

            private XMessageBoxUIProgress() { }

            public XMessageBoxUIProgress(uint result, uint code)
            {
                Result = result;
                Code = code;
            }

            public uint Code { get; private set; }
            public uint Result { get; private set; }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class ActiveXMessageBoxes
        {
            public uint Size;
            public byte[] XOverlappedBytes;

            public ActiveXMessageBoxes(uint size, byte[] xOverlappedBytes)
            {
                Size = size;
                XOverlappedBytes = xOverlappedBytes;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static class XMessageBoxTracking
        {
            public static List<ActiveXMessageBoxes> ActiveMessageBoxes = new List<ActiveXMessageBoxes>();
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class XMessageBoxUI
        {

            public ActiveXMessageBoxes CurMessageBox;
            public volatile bool IsMessageBoxOpen = false;
            public string MessageBoxMessage;

            public string MessageBoxTitle;
            public volatile uint ResultAddr;
            public int SelectedButton = 0;
            public string[] XMessageBoxButtons;
            public XMessageBoxIcons XMessageBoxIcon;
            public volatile uint XOverlappedAddr;

            public XMessageBoxUI(IXboxConsole console, string MsgBoxTitle, string MsgBoxMsg, string[] MsgBoxButtons, XMessageBoxIcons Icon, int SelectedButtonIndex)
            {
                XConsole = console;
                MessageBoxTitle = MsgBoxTitle;
                MessageBoxMessage = MsgBoxMsg;
                XMessageBoxButtons = MsgBoxButtons;
                XMessageBoxIcon = Icon;
                SelectedButton = SelectedButtonIndex;
            }

            public event EventHandler<XMessageBoxUIProgress> MessageBoxUIResult;

            public void CheckMessageBoxResult()
            {
                while (IsMessageBoxOpen)
                {
                    try
                    {
                        if (CurMessageBox == XMessageBoxTracking.ActiveMessageBoxes.Last())
                        {
                            uint code = XConsole.ReadUInt32(XOverlappedAddr);
                            uint result = 0;
                            switch (code)
                            {
                                case 0: //the user clicked one of the buttons
                                    result = XConsole.ReadUInt32(ResultAddr);
                                    IsMessageBoxOpen = false;
                                    break;
                                case 0x65b: //the user pressed b or back to close the message box ui
                                case 0x4c7: //user pressed the guide button to close the message box
                                    result = 420;
                                    IsMessageBoxOpen = false;
                                    break;
                                case 0x3e5: //the messagebox is still open
                                    break;
                                default: //unhandled case
                                    result = 710;
                                    IsMessageBoxOpen = false;
                                    break;
                            }
                            if (!IsMessageBoxOpen)
                            {
                                RemoveMessageBox(CurMessageBox);
                                MessageBoxUIResult(this, new XMessageBoxUIProgress(result, code));
                            }
                        }
                        Thread.Sleep(200);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
            //Result values:
            //0, 1, 2 (user pressed this button index)
            //420 (user exited the messagebox without choosing a value)
            //710 (unhandled case, check e.Code and add it to the case statement in the XMessageBoxUI CheckMessageBoxResult function)
            public static void MessageBoxExit(object sender, XMessageBoxUIProgress e)
            {
                XMessageBoxUI Msg = (XMessageBoxUI)sender;
                if (e.Result >= 0 && e.Result < 3)
                    MessageBox.Show("Users button \"" + Msg.XMessageBoxButtons[e.Result] + "\"");
                else
                    MessageBox.Show("XMessageBoxUIed with result " + e.Result.ToString() + " (code: 0x" + e.Code.ToString("X2") + ")");
            }

            public void RemoveMessageBox(ActiveXMessageBoxes mBox)
            {
                XMessageBoxTracking.ActiveMessageBoxes.Remove(CurMessageBox);
                if (XMessageBoxTracking.ActiveMessageBoxes.Count() > 0)
                    XConsole.SetMemory(XOverlappedAddr, XMessageBoxTracking.ActiveMessageBoxes.Last().XOverlappedBytes);
            }

            public bool Show()
            {
                if (IsMessageBoxOpen)
                {
                    MessageBox.Show("This XMessageBox is already open. Close it before opening it again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (XMessageBoxButtons.Count() > 3)
                {
                    MessageBox.Show("Number of buttons may not exceed 3.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (XMessageBoxButtons.Count() == 0)
                {
                    MessageBox.Show("Must have at least one button.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                uint FreeMemory = XConsole.ResolveFunction("xam.xex", 2601) + 0x3000;

                //XOverlapped and Result share the same address for every message box... no clue how to ger around this.
                uint XOverlapped = FreeMemory;
                uint Result = FreeMemory + 0x20; // xoverlapped is about 0x18 in size... we'll just do 0x20 to be safe

                //If we already have message boxes open, increment the new messageboxes location in memory
                foreach (ActiveXMessageBoxes mb in XMessageBoxTracking.ActiveMessageBoxes)
                    FreeMemory += mb.Size;

                //setup our title text and copy it into memory on the console
                byte[] title = JRPC.WCHAR(MessageBoxTitle);
                uint Title = Result + 0x10;
                XConsole.SetMemory(Title, title);

                //setup our message text and copy it into memory on the console
                byte[] msg = JRPC.WCHAR(MessageBoxMessage);
                uint Msg = Title + (uint)title.Length;
                XConsole.SetMemory(Msg, msg);

                //setup our button text and copy it into memory on the console
                List<byte[]> ButtonList = new List<byte[]>();
                foreach (string s in XMessageBoxButtons)
                    ButtonList.Add(JRPC.ToWCHAR(s));
                uint ButtonAddr = Msg + (uint)msg.Length;
                uint OrigButtonAddr = ButtonAddr;
                foreach (byte[] b in ButtonList)
                {
                    XConsole.SetMemory(ButtonAddr, b);
                    ButtonAddr += (uint)b.Length;
                }

                //copy pointers to button wstrs into an array
                uint EndAddr = FreeMemory;
                for (int i = 0; i < ButtonList.Count(); i++)
                {
                    EndAddr = ButtonAddr + ((uint)i * 4);
                    XConsole.WriteUInt32(EndAddr, OrigButtonAddr);
                    OrigButtonAddr += (uint)ButtonList.ElementAt(i).Length;
                }

                //finally, call XamShowMessageBoxUI
                uint LocalClientIndex = 0;

                uint addr = XConsole.ResolveFunction("xam.xex", 0x2ca);
                int ret = XConsole.Call<int>(addr, LocalClientIndex, Title, Msg, ButtonList.Count(), ButtonAddr, SelectedButton, (uint)XMessageBoxIcon, Result, XOverlapped);

                if (ret == 0x3e5) //messagebox was opened on the console successfully
                {
                    IsMessageBoxOpen = true;

                    //set our xoverlapped and result addresses for later use
                    XOverlappedAddr = XOverlapped;
                    ResultAddr = XOverlappedAddr + 0x20;

                    //grab the current xoverlapped bytes from the console and store them in an array incase we popup a messagebox on top of this one
                    byte[] XOverlappedBytes = XConsole.GetMemory(XOverlappedAddr, 0x20);

                    //grab the size of all the stuff we just set into memory on the console
                    uint Size = (EndAddr + 0x4) - FreeMemory;
                    while ((Size & 0x3) != 0) //make sure address is 4 byte aligned (don't know if this even matters)
                        Size += 0x1;

                    //add this Messagebox to the list of active Messageboxes
                    CurMessageBox = new ActiveXMessageBoxes(Size, XOverlappedBytes);
                    XMessageBoxTracking.ActiveMessageBoxes.Add(CurMessageBox);

                    //create a new thread to monitor the Messagebox
                    Thread MessageBoxThread = new Thread(CheckMessageBoxResult);
                    MessageBoxThread.Start();
                }
                else
                    IsMessageBoxOpen = false;

                return IsMessageBoxOpen;
            }

            private enum XNotiyLogo
            {
                ACHIEVEMENT_UNLOCKED = 0x1b,
                ACHIEVEMENTS_UNLOCKED = 0x27,
                AVATAR_AWARD_UNLOCKED = 0x60,
                BLANK = 0x2a,
                CANT_CONNECT_XBL_PARTY = 0x38,
                CANT_DOWNLOAD_X = 0x20,
                CANT_SIGN_IN_MESSENGER = 0x2b,
                DEVICE_FULL = 0x63,
                DISCONNECTED_FROM_XBOX_LIVE = 11,
                DISCONNECTED_XBOX_LIVE_11_MINUTES_REMAINING = 0x2e,
                DISCONNECTED_XBOX_LIVE_PARTY = 0x36,
                DOWNLOAD = 12,
                DOWNLOAD_STOPPED_FOR_X = 0x21,
                DOWNLOADED = 0x37,
                FAMILY_TIMER_X_TIME_REMAINING = 0x2d,
                FLASH_LOGO = 0x17,
                FLASHING_CHAT_ICON = 0x26,
                FLASHING_CHAT_SYMBOL = 0x41,
                FLASHING_DOUBLE_SIDED_HAMMER = 0x10,
                FLASHING_FROWNING_FACE = 0x15,
                FLASHING_HAPPY_FACE = 0x14,
                FLASHING_MUSIC_SYMBOL = 13,
                FLASHING_XBOX_CONSOLE = 0x22,
                FLASHING_XBOX_LOGO = 0x04,
                FOUR_2 = 0x19,
                FOUR_3 = 0x1a,
                FOUR_5 = 0x30,
                FOUR_7 = 0x25,
                FOUR_9 = 0x1c,
                FRIEND_REQUEST_LOGO = 2,
                GAME_INVITE_SENT = 0x16,
                GAME_INVITE_SENT_TO_XBOX_LIVE_PARTY = 0x33,
                GAMER_PICTURE_UNLOCKED = 0x3b,
                GAMERTAG_HAS_JOINED_CHAT = 20,
                GAMERTAG_HAS_JOINED_XBL_PARTY = 0x39,
                GAMERTAG_HAS_LEFT_CHAT = 0x15,
                GAMERTAG_HAS_LEFT_XBL_PARTY = 0x3a,
                GAMERTAG_SENT_YOU_A_MESSAGE = 5,
                GAMERTAG_SIGNED_IN_OFFLINE = 9,
                GAMERTAG_SIGNED_INTO_XBOX_LIVE = 8,
                GAMERTAG_SIGNED_IN = 7,
                GAMERTAG_SINGED_OUT = 6,
                GAMERTAG_WANTS_TO_CHAT = 10,
                GAMERTAG_WANTS_TO_CHAT_2 = 0x11,
                GAMERTAG_WANTS_TO_TALK_IN_VIDEO_KINECT = 0x1d,
                GAMERTAG_WANTS_YOU_TO_JOIN_AN_XBOX_LIVE_PARTY = 0x31,
                JOINED_XBL_PARTY = 0x3d,
                KICKED_FROM_XBOX_LIVE_PARTY = 0x34,
                KINECT_HEALTH_EFFECTS = 0x2f,
                MESSENGER_DISCONNECTED = 0x29,
                MISSED_MESSENGER_CONVERSATION = 0x2c,
                NEW_MESSAGE = 3,
                NEW_MESSAGE_LOGO = 1,
                NULLED = 0x35,
                PAGE_SENT_TO = 0x18,
                PARTY_INVITE_SENT = 50,
                PLAYER_MUTED = 0x3f,
                PLAYER_UNMUTED = 0x40,
                PLEASE_RECONNECT_CONTROLLERM = 0x13,
                PLEASE_REINSERT_MEMORY_UNIT = 0x12,
                PLEASE_REINSERT_USB_STORAGE_DEVICE = 0x3e,
                READY_TO_PLAY = 0x1f,
                UPDATING = 0x4c,
                VIDEO_CHAT_INVITE_SENT = 30,
                X_HAS_SENT_YOU_A_NUDGE = 40,
                X_SENT_YOU_A_GAME_MESSAGE = 0x23,
                XBOX_LOGO = 0
            }

        }

        internal static string Kernal()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Shot Callers
        public static IXboxConsole XConsole;
        public const int patch = 0x60000000;
        public static bool connected = false;
        public static bool XNotify = true;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static byte[] lvl = null;
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static byte[] byte_0 = new byte[0x10];
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static byte[] byte_1 = new byte[0x10];
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static uint cu;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string timeStamp = GetTimestamp(DateTime.Now);
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static string string_0;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string time;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("M" + "MM-" + "dd" + "-" + "yyyy");
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        private float method_9(TemperatureFlag Temperature)
        {
            XConsole.ResolveFunction("xboxkrnl.exe", 0x29);
            uint address = XConsole.ResolveFunction("xam.xex", 0xa29) + 0x3000;
            Array.Clear(byte_0, 0, byte_0.Length);
            Array.Clear(byte_1, 0, byte_1.Length);
            byte_0[0] = 7;
            object[] arguments = new object[] { byte_0, address };
            XConsole.Call<uint>("xboxkrnl.exe", 0x29, arguments);
            byte[] buffer = XConsole.GetMemory(address, 12);
            float num2 = (float)((buffer[((int)2) + 1] | (buffer[2 + 2] << 8)) / 256.0);
            return (1.8f * num2) + 32f;
        }

        #endregion
        #region Connection
        public static void Connect()
        {
            try
            {
                if (XConsole.Connect(out XConsole))
                {
                    X360Text("Connected! " + Application.ProductVersion);
                    connected = true;

                }
                else
                {
                    MessageBox.Show("Error 101 (Xbox 360 Not Found)");
                }
            }
            catch (Exception)
            {

            }
        }
        public static void IPConnection(string ip)
        {
            try
            {
                if (XConsole.Connect(out XConsole, ip))
                {
                    cu = XConsole.OpenConnection(null);
                    X360Text("Connected! " + Application.ProductVersion);
                    time = XConsole.Drives;
                    connected = true;

                }
                else
                {
                    MessageBox.Show("Error 101 (Xbox 360 Not Found)");
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion
        #region Features

        public void XMessageBox(string Title, string Message, string Button1, string Button2)
        {
            XMessageBoxUI message = new XMessageBoxUI(XConsole, Title, Message, new string[] { Button1, Button2 }, XMessageBoxIcons.XMB_ALERTICON, 0);
            if (message.Show())
                message.MessageBoxUIResult += XMessageBoxUI.MessageBoxExit;
        }
        /// <summary>
        /// Writes Text On Screen.
        /// </summary>
        public static void X360Text(string Message)
        {
            if (XNotify == true)
            {
                object[] arguments = new object[] { Type(), 0xff, 2, Message.ToWCHAR(), 1 };
                XConsole.CallVoid(ThreadType.Title, "xam.xex", 0x290, arguments);
            }
            else
            {

            }

        }
        public static void ConsoleFileCreationTime(string file, string Info_output)
        {
            IXboxFile xf = XConsole.GetFileObject(file);
            DateTime t = (DateTime)xf.CreationTime;
            t = DateTime.Parse(Info_output);
        }
        public static void ConsoleFileSize(string filename, ulong Info_output)
        {
            IXboxFile xf = XConsole.GetFileObject(filename);
            Info_output = xf.Size;

        }
        /// <summary>
        /// Cleans Consoles Cache.
        /// </summary>
        public static void Clean_Cache()
        {
            int num = 0;
            foreach (IXboxFile file in XConsole.DirectoryFiles(@"Hdd:\cache\"))
            {
                try
                {
                    XConsole.DeleteFile(file.Name);
                    num++;
                }
                catch
                {
                }
            }
            try
            {
                foreach (IXboxFile file2 in XConsole.DirectoryFiles(@"SysCache0:\"))
                {
                    try
                    {
                        XConsole.DeleteFile(file2.Name);
                        num++;
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            X360Text("Cleared " + num.ToString() + " files from Cache");
        }
        public static void Delete_KV()
        {
            
            foreach (IXboxFile file in XConsole.DirectoryFiles(@"Hdd:\"))
            {
                string num2 = "";
                string num3 = "";
                string num4 = "";
                try
                {
                    if(file.Name == "kv.bin")
                    {
                        XConsole.DeleteFile("kv.bin");
                        num2 = "kv.bin ";
                    }
                    else if (file.Name == "cpukey.txt")
                    {
                        XConsole.DeleteFile("cpukey.txt");
                        num3 = " cpukey.txt ";
                    }
                    else if (file.Name == "cpukey.bin")
                    {
                        XConsole.DeleteFile("cpukey.bin");
                        num3 = " cpukey.bin";
                    }
                }
                catch
                {
                }
                X360Text(num2 + num3+ num4 + " Were Successfully Deleted from HDD");
            }
        }
            /// <summary>
            /// Returns To Console's Home.
            /// </summary>
            public static void DashHome()
        {
            XConsole.Reboot(@"\Device\Harddisk0\SystemExtPartition\20445100\dash.xex", @"\Device\Harddisk0\SystemExtPartition\20445100\", @"\Device\Harddisk0\SystemExtPartition\20445100\", XboxRebootFlags.Title);
        }
        /// <summary>
        /// Connection method 2.
        /// </summary>
        public static void Connect2()
        {

            IXboxManager xboxMgr = new XboxManager();
            IXboxConsole X = xboxMgr.OpenConsole(xboxMgr.DefaultConsole);
            XConsole = X;
            IXboxDebugTarget XboxDebugTarget = XConsole.DebugTarget;
            XboxDebugTarget.ConnectAsDebugger("XRGHC", XboxDebugConnectFlags.Force);


            try
            {
                connected = true;
                if (connected == false)
                {
                    MessageBox.Show("Failed to connect to console!", "Error!");
                }
                else
                {
                    X360Text("Connected to console successfully!");
                }
            }
            catch (COMException ce)
            {
                Console.WriteLine("Connection failed returning {0}", xboxMgr.TranslateError(ce.ErrorCode));
            }
        }

        /// <summary>
        /// Freezes/Stops Console.
        /// </summary>
        public static void Freeze_Console(bool FalseorTrue)
        {
            if (!FalseorTrue == true)
            {
                XConsole.SendTextCommand("stop", out string_0);
                MessageBox.Show("Frozen!");
            }
            else if (FalseorTrue == false)
            {
                XConsole.SendTextCommand("go", out string_0);
                MessageBox.Show("Un-Frozen!");
            }
        }
        /// <summary>
        /// Cold Reboot Is a Gentle Booting Rebooting Method.
        /// </summary>
        public static void Cold_Reboot()
        {
            XConsole.Reboot(null, null, null, XboxRebootFlags.Cold);

        }
        /// <summary>
        /// Warm Reboot Is a Instant Booting Rebooting Method.
        /// </summary>
        public static void Warm_Reboot()
        {
            XConsole.Reboot(null, null, null, XboxRebootFlags.Warm);

        }

        /// <summary>
        /// Reboot Method As Cold Is true & Warm Is False.
        /// </summary>
        public static void Reboot(bool Warm_or_Cold)
        {
            if (Warm_or_Cold == true)
            {
                XConsole.Reboot(null, null, null, XboxRebootFlags.Cold);
            }
            if (Warm_or_Cold == false)
            {
                XConsole.Reboot(null, null, null, XboxRebootFlags.Warm);
            }
        }
        /// <summary>
        /// Turns Off Console.
        /// </summary>
        public static void Shutdown()
        {
            XConsole.ShutDownConsole();
        }
        /// <summary>
        /// Console Launches To Avatar Editor.
        /// </summary>
        public static void Avatar_Editor()
        {
            XConsole.Reboot(@"\Device\Harddisk0\SystemExtPartition\20445100\AvatarEditor.xex", @"\Device\Harddisk0\SystemExtPartition\20445100\", null, XboxRebootFlags.Title);
        }
        /// <summary>
        /// Takes A Screenshot And Sends It To Your Computer.
        /// </summary>
        public static void ScreenShot()
        {
            string str = Path.GetDirectoryName(Application.ExecutablePath) + @"\";
            string str2 = timeStamp + "-" + method_8();
            if (!Directory.Exists(str + @"Screenshots\"))
            {
                Directory.CreateDirectory(str + @"Screenshots\");
            }
            XConsole.ScreenShot(str2 + ".bmp");
            string filename = str + str2 + ".bmp";
            Image image1 = Image.FromFile(filename);
            image1.Save(str + @"Screenshots\" + str2 + ".png", ImageFormat.Png);
            image1.Dispose();
            File.Delete(filename);
            MessageBox.Show("Screenshot Saved: " + Path.Combine(str, @"Screenshots\" + str2 + ".png"));
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static int Type()
        {
            return 0x18;
        }
        public static void SetMemory(uint v1, byte[] v2)
        {
            try

            {

                if (connected == true)
                {
                    XConsole.SetMemory(v1, v2);
                }
                else
                {
                    MessageBox.Show("Not Connected");
                }

            }
            catch (Exception)
            {

            }
        }
        #endregion
        #region Console Information
        public static string TitleID()
        {
            return JRPC.XamGetCurrentTitleId(XConsole).ToString("X");
        }
        public static string Kernel()
        {
            return JRPC.GetKernalVersion(XConsole).ToString();
        }

        public static string XboxIP()
        {
            return JRPC.XboxIP(XConsole).ToString();
        }
        public static string ConsoleType()
        {
            return JRPC.ConsoleType(XConsole).ToString();
        }
        #endregion
        #region Console Tempatures
        public static string CPUTEMP()
        {
            return JRPC.GetTemperature(XConsole, TemperatureFlag.CPU) + "\x00b0C".ToString() + "%";
        }
        public static string GPUTEMP()
        {
            return JRPC.GetTemperature(XConsole, TemperatureFlag.GPU) + "\x00b0C".ToString() + "%";
        }
        public static string RamTEMP()
        {
            return JRPC.GetTemperature(XConsole, TemperatureFlag.EDRAM) + "\x00b0C".ToString() + "%";
        }
        public static string MOBOTEMP()
        {
            return JRPC.GetTemperature(XConsole, TemperatureFlag.MotherBoard) + "\x00b0C".ToString() + "%";
        }
        #endregion
    }
}

