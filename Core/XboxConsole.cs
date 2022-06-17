//Do Not Delete This Comment... 
//Made By Serenity on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2016.
//Thank You for looking love you guys...

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace XDCKIT
{
    public partial class XboxConsole //Main
    {
        public XboxConsole()
        {

        }
        #region Properties

        public static string Response;
        /// <summary>
        /// Get's or Set's Console's Current Name
        /// </summary>
        public string Name
        {
            get
            {
                if (Connected)
                {
                    return SendTextCommand("dbgname").Replace("200- ", string.Empty);
                }
                else
                {
                    return "Error";
                }
            }
            set
            {
                if (Connected)
                {
                    SendTextCommand("dbgname name=" + value);
                }
            }
        }

        public string[] DirectoryFiles(string Directory)
        {
            return new XboxFile().DirectoryFiles(Directory);
        }
        /// <summary>
        /// Object To FileSystem Class
        /// </summary>
        public XboxFile File
        {
            get
            {
                return new XboxFile();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SystemTime
        {
            get
            {
                if (Connected)
                {
                    return SendTextCommand("systime");
                }
                else
                {
                    return "Error";
                }
            }
            set
            {
                if (Connected)
                {
                    SendTextCommand("setsystime" + value);
                }
            }
        }


        public bool Connected
        {
            get
            {
                return XboxClient.Connected;
            }
            set
            {
                XboxClient.Connected = value;
            }
        }
        public void ScreenShot(string FilePath)//TODO: Work in Progress
        {
            Bitmap image1 = new Bitmap(SendTextCommand("screenshot"));
            int x, y;

            // Loop through the images pixels to reset color.
            for (x = 0; x < image1.Width; x++)
            {
                for (y = 0; y < image1.Height; y++)
                {
                    Color pixelColor = image1.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    image1.SetPixel(x, y, newColor);
                }
            }

        }

        /// <summary>
        /// Detects Console Type Information.
        /// </summary>
        public XboxConsoleType ConsoleType
        {
            get
            {
                if (Connected)
                {
                    return (XboxConsoleType)Enum.Parse(typeof(XboxConsoleType), SendTextCommand("consoletype"), true);
                }
                else
                {
                    return XboxConsoleType.NotConnected;
                }
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]//hidden not yet set
        bool MemoryCacheEnabled
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public static int ConnectTimeout
        {
            get
            {
                return XboxClient.XboxName.SendTimeout;
            }
            set
            {
                XboxClient.XboxName.SendTimeout = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static int ConversationTimeout
        {
            get
            {
                return XboxClient.XboxName.ReceiveTimeout;
            }
            set
            {
                XboxClient.XboxName.ReceiveTimeout = value;
            }
        }
        /// <summary>
        /// Xbox Console IPAddress 
        /// </summary>
        public string IPAddress
        {
            get
            {
                return XboxClient.IPAddress;
            }
            set
            {
                IPAddress = value;
            }
        }
        /// <summary>
        /// Gets the title ip address
        /// </summary>
        uint IPAddressTitle
        {
            get
            {
                if (XboxClient.Connected)
                {
                    return uint.Parse(SendTextCommand("altaddr"), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    return 0;
                }
            }
        }

        public string DefaultConsole //TODO: Add XML To Save Console name or IP
        {
            get
            {
                if (Connected)
                {
                    return "In Development";
                }
                else
                {
                    return "Error";
                }
            }
            set
            {
                DefaultConsole = value;
            }
        }
        public Tray Tray
        {
            get
            {
                return new Tray();
            }
        }
        public XNotify XNotify
        {
            get
            {
                return new XNotify();
            }
        }

        #endregion
        public void Disconnect()
        {
            XboxClient.Disconnect();
        }
        public string ResponseTranslator(int code)
        {
            switch (code)
            {
                case 200:
                    return "200- OK (Standard response for successful execution of a command.)";
                    

                case 0xc9:
                    return "201- connected (Initial response sent after a connection is established. The client does not need to send anything to solicit this response.)";
                    

                case 0xca:
                    return "202- multiline response follows - The response line is followed by one or more additional lines of data terminated by a line containing only a . (period).The client must read all available lines before sending another command.)";
                    

                case 0xcb:
                    return "203- binary response follows (The response line is followed by raw binary data, the length of which is indicated in some command-specific way.The client must read all available data before sending another command.)";
                    

                case 0xcc:
                    return "204- send binary data (The command is expecting additional binary data from the client.After the client sends the required number of bytes, XBDM will send another response line with the final result of the command.)";
                    

                case 0xcd:
                    return "205- connection dedicated (The connection has been moved to a dedicated handler thread).";
                    
                case 400:
                    return "400- unexpected error = An internal error occurred that could not be translated to a standard error code.The message is typically more descriptive, such as 'out of memory' or 'bad parameter'.";
                    

                case 0x191:
                    return "401- max number of connections exceeded = The connection could not be established because XBDM is already serving the maximum number of clients(4).";
                    

                case 0x192:
                    return "402- file not found = An operation was attempted on a file that does not exist.";
                    

                case 0x193:
                    return "403- no such module = An operation was attempted on a module that does not exist.";
                    

                case 0x194:
                    return "404- memory not mapped = An operation was attempted on a region of memory that is not mapped in the page table.";
                    

                case 0x195:
                    return "405- no such thread = An operation was attempted on a thread that does not exist.";
                    

                case 0x196:
                    return "406- = An attempt to set the system time with the setsystime command failed. This status code is undocumented.";
                    

                case 0x197:
                    return "407- unknown command = The command is not recognized.";
                    

                case 0x198:
                    return "408- not stopped = The target thread is not stopped.";
                    

                case 0x199:
                    return "409- file must be copied = A move operation was attempted on a file that can only be copied.";
                    

                case 410:
                    return "410- file already exists = A file could not be created or moved because one already exists with the same name.";
                    

                case 0x19b:
                    return "411- directory not empty = A directory could not be deleted because it still contains files and/or directories.";
                    

                case 0x19c:
                    return "412- filename is invalid = The specified file contains invalid characters or is too long.";
                    

                case 0x19d:
                    return "413- file cannot be created = The file cannot be created for some unspecified reason.";
                    

                case 0x19e:
                    return "414- access denied = The file cannot be accessed at the connection's current privilege level (see #Security).";
                    

                case 0x19f:
                    return "415- no room on device = The target device has run out of storage space.";
                    

                case 0x1a0:
                    return "416- not debuggable = The title is not debuggable.";
                    

                case 0x1a1:
                    return "417- type invalid = The performance counter type is invalid.";
                    

                case 0x1a2:
                    return "418- data not available = The performance counter data is not available.";
                    

                case 420:
                    return "420- box not locked = The command can only be executed when security is enabled (see #Security).";
                    

                case 0x1a5:
                    return "421- key exchange required = The client must perform a key exchange with the keyxchg command (see #Security).";
                    

                case 0x1a6:
                    return "422- dedicated connection required = The command can only be executed on a dedicated connection (see #Connection dedication).";
                    
                default:
                    return "Response code you entered is either invalid or there isn't any information for it.";
                    
            }
        }
    }
}
