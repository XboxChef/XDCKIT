//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2015.
//Thank You for looking love you guys...

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace XDCKIT
{
    public partial class XboxConsole //Main
    {

        #region Properties
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
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

        public XboxFileSystem DirectoryFiles(string Directory)
        {
            XboxFileSystem xboxFile = new XboxFileSystem();
            xboxFile.DirectoryFiles(Directory);
            return xboxFile;
        }
        /// <summary>
        /// 
        /// </summary>
        public XboxFileSystem File = new XboxFileSystem();

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

        /// <summary>
        /// Captures a screenshot from the frame buffer of the console.
        /// </summary>
        /// <returns>A BitmapSource containing the uncompressed frame buffer captured off the current console.</returns>
        //public BitmapSource CaptureScreenshot()
        //{

        //   // IntPtr pointerToHBitmap = 
        //    BitmapSource managedBitmap = null;

        //    try
        //    {
        //        managedBitmap = Imaging.CreateBitmapSourceFromHBitmap(pointerToHBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        // Occurs when passing a IntPtr.Zero into CreateBitmapSourceFromHBitmap.
        //        // Shouldn't ever happen because the adapter checks for this.
        //        throw new ("Failed to capture screenshot.", ex);
        //    }
        //    finally
        //    {
        //        NativeMethods.DeleteObject(pointerToHBitmap);
        //    }

        //    return managedBitmap;
        //}

        /// <summary>
        /// Captures a screenshot from the frame buffer of the specified console.
        /// </summary>
        /// <param name="systemIpAddress">The IP address of the console.</param>
        /// <returns>A pointer to the location in memory of the uncompressed frame buffer captured off the console.</returns>
        //public IntPtr CaptureScreenshot(string systemIpAddress)
        //{
        //    return null;
        //}

        public void ScreenShot(string FilePath)
        {
            //try
            //{
            //    // Retrieve the image.
            //    image1 = new Bitmap(@"C:\Documents and Settings\All Users\"
            //        + @"Documents\My Music\music.bmp", true);

            //    int x, y;

            //    // Loop through the images pixels to reset color.
            //    for (x = 0; x < image1.Width; x++)
            //    {
            //        for (y = 0; y < image1.Height; y++)
            //        {
            //            Color pixelColor = image1.GetPixel(x, y);
            //            Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
            //            image1.SetPixel(x, y, newColor);
            //        }
            //    }

            //    // Set the PictureBox to display the image.
            //    PictureBox1.Image = image1;

            //    // Display the pixel format in Label1.
            //    Label1.Text = "Pixel format: " + image1.PixelFormat.ToString();
            //}
            //catch (ArgumentException)
            //{
            //    MessageBox.Show("There was an error." +
            //        "Check the path to the image file.");
            //}
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
            get => XboxClient.XboxName.SendTimeout;
            set => XboxClient.XboxName.SendTimeout = value;
        }
        /// <summary>
        /// 
        /// </summary>
        public static int ConversationTimeout
        {
            get => XboxClient.XboxName.ReceiveTimeout;
            set => XboxClient.XboxName.ReceiveTimeout = value;
        }
        /// <summary>
        /// Gets The Console IPAddress 
        /// </summary>
        public string IPAddress
        {
            get
            {
                return XboxClient.IPAddress;
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

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]//hidden not yet set
        public string DefaultConsole
        {
            get
            {
                if (Connected)
                {
                    return Name;
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

        public Tray Tray = new Tray();
        public XNotify XNotify = new XNotify();

        #endregion
        public void Connect()
        {
            XboxClient.Connect();
        }
        public void Connect(string ConsoleNameOrIP = "default", int Port = 730)
        {
            XboxClient.Connect(ConsoleNameOrIP, Port);
        }
        public void Disconnect()
        {
            XboxClient.Disconnect();
        }
        public string TranslateError(int code)
        {
            string str;
            int num = code;
            switch (num)
            {
                case 200:
                    str = "200- OK (Standard response for successful execution of a command.)";
                    break;

                case 0xc9:
                    str = "201- connected (Initial response sent after a connection is established. The client does not need to send anything to solicit this response.)";
                    break;

                case 0xca:
                    str = "202- multiline response follows - The response line is followed by one or more additional lines of data terminated by a line containing only a . (period).The client must read all available lines before sending another command.)";
                    break;

                case 0xcb:
                    str = "203- binary response follows (The response line is followed by raw binary data, the length of which is indicated in some command-specific way.The client must read all available data before sending another command.)";
                    break;

                case 0xcc:
                    str = "204- send binary data (The command is expecting additional binary data from the client.After the client sends the required number of bytes, XBDM will send another response line with the final result of the command.)";
                    break;

                case 0xcd:
                    str = "205- connection dedicated (The connection has been moved to a dedicated handler thread).";
                    break;

                default:
                    switch (num)
                    {
                        case 400:
                            str = "400- unexpected error = An internal error occurred that could not be translated to a standard error code.The message is typically more descriptive, such as 'out of memory' or 'bad parameter'.";
                            break;

                        case 0x191:
                            str = "401- max number of connections exceeded = The connection could not be established because XBDM is already serving the maximum number of clients(4).";
                            break;

                        case 0x192:
                            str = "402- file not found = An operation was attempted on a file that does not exist.";
                            break;

                        case 0x193:
                            str = "403- no such module = An operation was attempted on a module that does not exist.";
                            break;

                        case 0x194:
                            str = "404- memory not mapped = An operation was attempted on a region of memory that is not mapped in the page table.";
                            break;

                        case 0x195:
                            str = "405- no such thread = An operation was attempted on a thread that does not exist.";
                            break;

                        case 0x196:
                            str = "406- = An attempt to set the system time with the setsystime command failed. This status code is undocumented.";
                            break;

                        case 0x197:
                            str = "407- unknown command = The command is not recognized.";
                            break;

                        case 0x198:
                            str = "408- not stopped = The target thread is not stopped.";
                            break;

                        case 0x199:
                            str = "409- file must be copied = A move operation was attempted on a file that can only be copied.";
                            break;

                        case 410:
                            str = "410- file already exists = A file could not be created or moved because one already exists with the same name.";
                            break;

                        case 0x19b:
                            str = "411- directory not empty = A directory could not be deleted because it still contains files and/or directories.";
                            break;

                        case 0x19c:
                            str = "412- filename is invalid = The specified file contains invalid characters or is too long.";
                            break;

                        case 0x19d:
                            str = "413- file cannot be created = The file cannot be created for some unspecified reason.";
                            break;

                        case 0x19e:
                            str = "414- access denied = The file cannot be accessed at the connection's current privilege level (see #Security).";
                            break;

                        case 0x19f:
                            str = "415- no room on device = The target device has run out of storage space.";
                            break;

                        case 0x1a0:
                            str = "416- not debuggable = The title is not debuggable.";
                            break;

                        case 0x1a1:
                            str = "417- type invalid = The performance counter type is invalid.";
                            break;

                        case 0x1a2:
                            str = "418- data not available = The performance counter data is not available.";
                            break;

                        case 420:
                            str = "420- box not locked = The command can only be executed when security is enabled (see #Security).";
                            break;

                        case 0x1a5:
                            str = "421- key exchange required = The client must perform a key exchange with the keyxchg command (see #Security).";
                            break;

                        case 0x1a6:
                            str = "422- dedicated connection required = The command can only be executed on a dedicated connection (see #Connection dedication).";
                            break;

                        default:
                            str = "Response code you entered is either invalid or there isn't any information for it.";
                            break;
                    }
                    break;
            }
            return str;
        }
    }
}
