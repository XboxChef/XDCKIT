//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it 3 years and counting...
//Thank You for looking love you guys...

using System.ComponentModel;

namespace XDevkit
{
    public partial class Xbox //Main
    {

        #region Properties

        public string Name
        {
            get => XboxClient.XboxName.Connected ? SendTextCommand("dbgname") : "Error";
            set => SendTextCommand("dbgname = " + value);
        }
        public bool IsTrayOpen { get; set; } = false;
        [Browsable(false)]
        public XboxFileSystem File
        {
            get => new XboxFileSystem();
        }
        public string SystemTime
        {
            get;
            set;
        }
        public static string Response;
        public XBOX_PROCESS_INFO RunningProcessInfo
        {
            get;
        }
        public XboxDumpMode DumpMode
        {
            get;
            set;

        } = XboxDumpMode.Smart;

        /// <summary>
        ///
        /// </summary>
        public XboxConsoleType ConsoleType
        {
            get => XboxClient.XboxName.Connected ? XboxConsoleType.DevelopmentKit : XboxConsoleType.DevelopmentKit;
        } //TODO: make it so it actually grabs the console type
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
