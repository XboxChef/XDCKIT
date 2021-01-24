using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDevkit
{
    public partial class Xbox //Main
    {

        #region Properties
        public string Name { get; set; } = XboxClient.Connected ? SendTextCommand("dbgname") : "Error";
        [Browsable(false)]
        public XboxFileSystem File { get; set; } = new XboxFileSystem();
        public object SystemTime { get; set; }
        public static string Response;
        public XBOX_PROCESS_INFO RunningProcessInfo { get; }
        public XboxDumpMode DumpMode { get; set; } = XboxDumpMode.Smart;

        /// <summary>
        ///
        /// </summary>
        public XboxConsoleType ConsoleType { get; private set; } = XboxConsoleType.DevelopmentKit;//TODO: will make it so it actually grabs the console type
        bool MemoryCacheEnabled { get; set; }  
        public static bool IsConnected { get; } = XboxClient.XboxName.Connected;

        public static int ConnectTimeout
        {
            get => XboxClient.XboxName.SendTimeout; set => XboxClient.XboxName.SendTimeout = value;
        }
        public static int ConversationTimeout
        {
            get => XboxClient.XboxName.ReceiveTimeout; set => XboxClient.XboxName.ReceiveTimeout = value;
        }
        public string IPAddress { get; set; } = "000.000.000.000";

        public string DefaultConsole { get; set; } = "Not Set...";

        #endregion



        static Xbox()
        {

        }




    }
}
