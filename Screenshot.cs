
using System;

namespace XDevkit
{
    public class Screenshot
    {
        public static Xbox XConsole;
        public Screenshot()
        {
            
        }

        internal void GetScreenshot()
        {
            XConsole.GetFileCommand("screenshot");
        }
    }
}
