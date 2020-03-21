using System;
using System.Runtime.InteropServices;

namespace XDevkit.Utils
{
    internal class XBDM
    {
        public static string Show(string Message)
        {

            return Message;
        }
        public static Exception Exception(string Message)
        {
            throw new Exception(Message);
        }
        public static string Failed_Connection(string Message)
        {

            return Exception(Message).ToString();
        }
        public static string Connected()
        {

            return "Connected!";
        }
        public const uint ThreadCount = 0x100;
        private const int XBDM_SUCCESS = 0x2da0000;
        private const int XBDM_ERROR = -2099642368;
        public const int XBDM_NOERR = 0x2da0000;
        public const int XBDM_UNDEFINED = -2099642368;
        public const int XBDM_MAXCONNECT = -2099642367;
        public const int XBDM_NOSUCHFILE = -2099642366;
        public const int XBDM_NOMODULE = -2099642365;
        public const int XBDM_MEMUNMAPPED = -2099642364;
        public const int XBDM_NOTHREAD = -2099642363;
        public const int XBDM_CLOCKNOTSET = -2099642362;
        public const int XBDM_INVALIDCMD = -2099642361;
        public const int XBDM_NOTSTOPPED = -2099642360;
        public const int XBDM_MUSTCOPY = -2099642359;
        public const int XBDM_ALREADYEXISTS = -2099642358;
        public const int XBDM_DIRNOTEMPTY = -2099642357;
        public const int XBDM_BADFILENAME = -2099642356;
        public const int XBDM_CANNOTCREATE = -2099642355;
        public const int XBDM_CANNOTACCESS = -2099642354;
        public const int XBDM_DEVICEFULL = -2099642353;
        public const int XBDM_NOTDEBUGGABLE = -2099642352;
        public const int XBDM_BADCOUNTTYPE = -2099642351;
        public const int XBDM_COUNTUNAVAILABLE = -2099642350;
        public const int XBDM_NOTLOCKED = -2099642348;
        public const int XBDM_KEYXCHG = -2099642347;
        public const int XBDM_MUSTBEDEDICATED = -2099642346;
        public const int XBDM_INVALIDARG = -2099642345;
        public const int XBDM_PROFILENOTSTARTED = -2099642344;
        public const int XBDM_PROFILEALREADYSTARTED = -2099642343;
        public const int XBDM_ALREADYSTOPPED = -2099642342;
        public const int XBDM_FASTCAPNOTENABLED = -2099642341;
        public const int XBDM_NOMEMORY = -2099642340;
        public const int XBDM_TIMEOUT = -2099642339;
        public const int XBDM_NOSUCHPATH = -2099642338;
        public const int XBDM_INVALID_SCREEN_INPUT_FORMAT = -2099642337;
        public const int XBDM_INVALID_SCREEN_OUTPUT_FORMAT = -2099642336;
        public const int XBDM_CALLCAPNOTENABLED = -2099642335;
        public const int XBDM_INVALIDCAPCFG = -2099642334;
        public const int XBDM_CAPNOTENABLED = -2099642333;
        public const int XBDM_TOOBIGJUMP = -2099642332;
        public const int XBDM_FIELDNOTPRESENT = -2099642331;
        public const int XBDM_OUTPUTBUFFERTOOSMALL = -2099642330;
        public const int XBDM_PROFILEREBOOT = -2099642329;
        public const int XBDM_MAXDURATIONEXCEEDED = -2099642327;
        public const int XBDM_INVALIDSTATE = -2099642326;
        public const int XBDM_MAXEXTENSIONS = -2099642325;
        public const int XBDM_D3D_DEBUG_COMMAND_NOT_IMPLEMENTED = -2099642288;
        public const int XBDM_D3D_INVALID_SURFACE = -2099642287;
        public const int XBDM_CANNOTCONNECT = -2099642112;
        public const int XBDM_CONNECTIONLOST = -2099642111;
        public const int XBDM_FILEERROR = -2099642109;
        public const int XBDM_ENDOFLIST = -2099642108;
        public const int XBDM_BUFFER_TOO_SMALL = -2099642107;
        public const int XBDM_NOTXBEFILE = -2099642106;
        public const int XBDM_MEMSETINCOMPLETE = -2099642105;
        public const int XBDM_NOXBOXNAME = -2099642104;
        public const int XBDM_NOERRORSTRING = -2099642103;
        public const int XBDM_INVALIDSTATUS = -2099642102;
        public const int XBDM_TASK_PENDING = -2099642032;
        public const int XBDM_CONNECTED = 0x2da0001;
        public const int XBDM_MULTIRESPONSE = 0x2da0002;
        public const int XBDM_BINRESPONSE = 0x2da0003;
        public const int XBDM_READYFORBIN = 0x2da0004;
        public const int XBDM_DEDICATED = 0x2da0005;
        public const int XBDM_PROFILERESTARTED = 0x2da0006;
        public const int XBDM_FASTCAPENABLED = 0x2da0007;
        public const int XBDM_CALLCAPENABLED = 0x2da0008;
        public const uint DM_THREADINFOEX_SIZE = 0x31;
        private const uint MM_TITLE_IMAGE_64KB_VAD_BASE = 0x82000000;
        private const uint MM_TITLE_IMAGE_64KB_VAD_END = 0x8bfeffff;
        private const uint MM_TITLE_IMAGE_VAD_BASE = 0x92000000;
        private const uint MM_TITLE_IMAGE_VAD_END = 0x9ffeffff;

        [DllImport("xbdm.dll", CharSet = CharSet.Ansi)]
        public static extern int DmScreenshot(IntPtr walkMod);
    }
    internal class XNotify
    {
        public static string Show(string Message)
        {

            return Message;
        }
    }
}