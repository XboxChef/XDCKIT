//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it 3 years and counting...
//Thank You for looking love you guys...

using System;
using System.ComponentModel;

namespace XDevkit
{
    public partial class Xbox //Main
    {

        #region Properties
        /// <summary>
        /// Get's or Set's Console's Current Name
        /// </summary>
        public string Name
        {
            get => XboxClient.XboxName.Connected ? SendTextCommand("dbgname").Replace("200- ", string.Empty) : "Error";
            set
            {
                if(XboxClient.XboxName.Connected == true)
                {
                    SendTextCommand("dbgname name=" + value);
                }
            }
        }
        public bool IsTrayOpen { get; set; } = false;
        [Browsable(false)]
        public XboxFileSystem File
        {
            get => new XboxFileSystem();
        }
        public string SystemTime
        {
            get => XboxClient.XboxName.Connected ? SendTextCommand("systime").Replace("200- ", string.Empty) : "Error";
            set
            {
                if (XboxClient.XboxName.Connected == true)
                {
                    SendTextCommand("setsystime" + value);
                }
            }
        }
        public static string Response;
        public XBOX_PROCESS_INFO RunningProcessInfo
        {
            get;
        }
        /// <summary>
        /// Detects Console Type Information.
        /// </summary>
        public XboxConsoleType ConsoleType
        {
            get => XboxClient.XboxName.Connected ? (XboxConsoleType)Enum.Parse(typeof(XboxConsoleType), SendTextCommand("consoletype"), true) : XboxConsoleType.DevelopmentKit;
        } 
        bool MemoryCacheEnabled
        {
            get;
            set;
        }
        public static bool IsConnected
        {
            get => XboxClient.XboxName.Connected;
        }

        public static int ConnectTimeout
        {
            get => XboxClient.XboxName.SendTimeout;
            set => XboxClient.XboxName.SendTimeout = value;
        }
        public static int ConversationTimeout
        {
            get => XboxClient.XboxName.ReceiveTimeout;
            set => XboxClient.XboxName.ReceiveTimeout = value;
        }
        public string IPAddress
        {
            get;
            set;
        } = "000.000.000.000";

        public string DefaultConsole
        {
            get;
            set;
        } = "Not Set...";

        #endregion

        static Xbox()
        {

        }




    }
}
