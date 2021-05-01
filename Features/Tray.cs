using System.ComponentModel;

namespace XDCKIT
{
    public class Tray
    {
        private static readonly XboxConsole xbox = new XboxConsole();
        /// <summary>
        /// 
        /// </summary>
        public bool IsTrayOpen
        {
            get
            {
                return false;
            }
            set
            {

            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private const string XAMModule = "xam.xex";
        public void Open()
        {
            XboxExtention.CallVoid(xbox.ResolveFunction(XAMModule, (int)XboxShortcuts.Open_Tray), new object[] { 0, 0, 0, 0 });
        }
        public void Close()
        {
            XboxExtention.CallVoid(xbox.ResolveFunction(XAMModule, (int)XboxShortcuts.Close_Tray), new object[] { 0, 0, 0, 0 });
        }
        /// <summary>
        /// User Can Open/Close There Console's Disc Tray
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool Options(TrayState state)
        {

            switch (state)//works by getting the int of the UI and matches the numbers to execute things
            {
                case TrayState.Open:
                    XboxExtention.CallVoid(xbox.ResolveFunction(XAMModule, (int)XboxShortcuts.Open_Tray), new object[] { 0, 0, 0, 0 });
                    IsTrayOpen = true;
                    break;
                case TrayState.Close:
                    XboxExtention.CallVoid(xbox.ResolveFunction(XAMModule, (int)XboxShortcuts.Close_Tray), new object[] { 0, 0, 0, 0 });
                    IsTrayOpen = false;
                    break;
            }
            return IsTrayOpen;
        }
    }
}
