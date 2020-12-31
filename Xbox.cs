//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it 3 years and counting...
//Thank You for looking love you guys...
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace XDevkit
{
    //Main Xbox Class
    /// <summary>
    /// Xbox Emulation Class
    /// Made By TeddyHammer
    /// </summary>
    public partial class Xbox
    {
        public Xbox()
        {

        }
        ~Xbox() { Dispose(); }

        public void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// Takes a screenshot of the xbox display.
        /// </summary>
        public System.Drawing.Image Screenshot()
        {
            return null;
        }

        }


}