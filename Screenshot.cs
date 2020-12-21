
using System;
using System.Drawing;

namespace XDevkit
{
    public class DMScreenshot
    {
        public Xbox Con;

        internal void Screenshot()
        {
            //sends command
            //ReceiveSocketLine
            //
            Con.GetFileCommand("screenshot");
        }
    }
}
