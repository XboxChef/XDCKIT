//Do Not Delete This Comment... 
//Made By Serenity on 04/22/21
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2016.
//Thank You for looking love you guys...

using System;
using System.IO;
using System.Net.Sockets;

namespace XDCKIT
{
    public class XboxFile
    {

        public XboxFile()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string ChangeTime(string directory)
        {
            return new FileInfo(directory).LastWriteTime.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public string CreationTime(string directory)
        {
            return new FileInfo(directory).CreationTime.ToString();
        }

        /// <summary>
        /// Get the specified folder path from the console.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] DirectoryFiles(string path)
        {
            return new[] { XboxConsole.SendTextCommand(string.Concat("dir name=\"{0}\"", path)) };

        }
        /// <summary>
        /// determines if it's a file or Directory
        /// </summary>
        public bool IsDirectory(string directory)
        {
            //get the file attributes for file or directory and detect whether its a directory or file
            if ((File.GetAttributes(directory) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        ///  Gets value that determines if the current file is read only.
        /// </summary>
        public bool IsReadOnly(string directory)
        {
            return new FileInfo(directory).IsReadOnly;
        }
        /// <summary>
        /// Create the specified directory on the console.
        /// </summary>
        /// <param name="path">Directory name.</param>
        public void MakeDirectory(string path)
        {
            string sdr = string.Concat("mkdir name=\"{0}\"", path);
            XboxConsole.SendTextCommand(sdr, out _);
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name(string directory)
        {
            return new FileInfo(directory).Name;
        }

        /// <summary>
        /// Receives all available binary data sent from the xbox.
        /// </summary>
        /// <returns></returns>
        public byte[] ReceiveBinaryData()
        {
            if (XboxClient.XboxName.Available > 0)
            {
                byte[] binData = new byte[XboxClient.XboxName.Available];
                XboxClient.XboxName.Client.Receive(binData, binData.Length, SocketFlags.None);
                return binData;
            }
            else return null;
        }
        /// <summary>
        /// Receives binary data of specified size sent from the xbox.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public byte[] ReceiveBinaryData(int size)
        {

            XboxConsole.Wait(size);
            byte[] binData = new byte[size];
            XboxClient.XboxName.Client.Receive(binData, binData.Length, SocketFlags.None);
            return binData;
        }

        /// <summary>
        /// Receives binary data of specified size sent from the xbox.
        /// </summary>
        /// <param name="data"></param>
        public void ReceiveBinaryData(byte[] data)
        {
            XboxConsole.Wait(data.Length);
            XboxClient.XboxName.Client.Receive(data, data.Length, SocketFlags.None);
        }
        /// <summary>
        /// Receives a file from the xbox.
        /// </summary>
        /// <param name="localName">PC file name.</param>
        /// <param name="remoteName">Xbox file name.</param>
        public void ReceiveFile(string localName, string remoteName)
        {
            XboxConsole Xbox = new XboxConsole();
            XboxConsole.SendTextCommand("getfile name=\"{0}\"" + remoteName);
            int fileSize = BitConverter.ToInt32(Xbox.ReceiveBinaryData(4), 0);
            using (var lfs = new System.IO.FileStream(localName, FileMode.Create))
            {
                byte[] fileData = new byte[XboxClient.XboxName.Client.ReceiveBufferSize];

                int mainIterations = fileSize / XboxClient.XboxName.Client.ReceiveBufferSize;
                int remainder = fileSize % XboxClient.XboxName.Client.ReceiveBufferSize;

                for (int i = 0; i < mainIterations; i++)
                {
                    fileData = Xbox.ReceiveBinaryData(fileData.Length);
                    lfs.Write(fileData, 0, fileData.Length);
                }
                fileData = Xbox.ReceiveBinaryData(remainder);
                lfs.Write(fileData, 0, remainder);
            }
        }
        /// <summary>
        /// Sets the size of a specified file on the xbox.  This method will not zero out any extra bytes that may have been created.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="size"></param>
        public void SetFileSize(string fileName, int size)
        {
            XboxConsole Xbox = new XboxConsole();
            Xbox.SendTextCommand("fileeof name=\"{0}\" size={1}", fileName, size);
        }
        public DateTime SystemTime
        {

            get
            {
                string response = SendCommand("systime");
                if (response == ResponseType.SingleResponse.ToString())
                {
                    string ticks = string.Format("0x{0}{1}",
                        response.Substring(7, 7),
                        response.Substring(21).PadLeft(8, '0')
                        );
                    return DateTime.FromFileTime(Convert.ToInt64(ticks, 16));
                }
                else
                {
                    throw new Exception("Failed to get xbox system time.");
                }
            }
            set
            {
                
                long fileTime = value.ToFileTimeUtc();
                int lo = (int)(fileTime & 0xFFFFFFFF); // *(int*)&fileTime;
                int hi = (int)(((ulong)fileTime & 0xFFFFFFFF00000000UL) >> 32);// *((int*)&fileTime + 1);
                string response = SendCommand(string.Format("setsystime clockhi=0x{0} clocklo=0x{1} tz=1", Convert.ToString(hi, 16), Convert.ToString(lo, 16)));
                if (response != ResponseType.SingleResponse.ToString())
                {

                }
                else
                {
                    throw new Exception("Failed to set xbox system time.");
                }
                    //throw new ApiException("Failed to set xbox system time.");
            }
        }

        private string SendCommand(string v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a file on the xbox.
        /// </summary>
        /// <param name="fileName">File to create.</param>
        /// <param name="createDisposition">Creation options.</param>
        public void CreateFile(string fileName, FileMode createDisposition)
        {
            XboxConsole Xbox = new XboxConsole();
            if (createDisposition == FileMode.Open) { if (!File.Exists(fileName)) throw new FileNotFoundException("File does not exist."); }
            else if (createDisposition == FileMode.Create) Xbox.SendTextCommand("fileeof name=\"" + fileName + "\" size=0 cancreate");
            else if (createDisposition == FileMode.CreateNew) Xbox.SendTextCommand("fileeof name=\"" + fileName + "\" size=0 mustcreate");
            // else throw "Unsupported FileMode.";
        }
        /// <summary>
        /// Delete the specified folder path from the console.
        /// </summary>
        /// <param name="path"></param>
        public void RemoveDirectory(string path)
        {
            string sdr = string.Concat("delete name=\"{0}\"", path);
            XboxConsole.SendTextCommand(sdr, out _);
        }

        /// <summary>
        /// Renames or moves a file on the xbox.
        /// </summary>
        /// <param name="oldFileName">Old file name.</param>
        /// <param name="newFileName">New file name.</param>
        public void RenameFile(string OldFileName, string NewFileName)
        {
            string ren = string.Concat("rename name=\"{0}\" newname=\"{1}\"", OldFileName, NewFileName);
            XboxConsole.SendTextCommand(ren);
        }
        /// <summary>
        /// Sends binary data to the xbox.
        /// </summary>
        /// <param name="data"></param>
        public void SendBinaryData(byte[] data)
        {
            XboxClient.XboxName.Client.Send(data);
        }
        /// <summary>
        /// Sends binary data of specified length to the xbox.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public void SendBinaryData(byte[] data, int length)
        {
            XboxClient.XboxName.Client.Send(data, length, SocketFlags.None);
        }
        /// <summary>
        /// Sends a file to the xbox.
        /// </summary>
        /// <param name="localName">PC file name.</param>
        /// <param name="remoteName">Xbox file name.</param>
        public void SendFile(string localName, string remoteName)
        {
            XboxConsole Xbox = new XboxConsole();
            FileStream lfs = new FileStream(localName, FileMode.Open);
            byte[] fileData = new byte[XboxClient.XboxName.Client.SendBufferSize];
            XboxConsole.SendTextCommand("sendfile name=\"{0}\" length={1}" + remoteName + lfs.Length);

            int mainIterations = (int)lfs.Length / XboxClient.XboxName.Client.SendBufferSize;
            int remainder = (int)lfs.Length % XboxClient.XboxName.Client.SendBufferSize;

            for (int i = 0; i < mainIterations; i++)
            {
                lfs.Read(fileData, 0, fileData.Length);
                Xbox.SendBinaryData(fileData);
            }
            lfs.Read(fileData, 0, remainder);
            Xbox.SendBinaryData(fileData, remainder);

            lfs.Close();
        }

        /// <summary>
        /// Size of File
        /// </summary>
        public long Size(string directory)
        {
            return new FileInfo(directory).Length;
        }

        /// <summary>
        /// Get the console drive names.
        /// </summary>
        public string Drives
        {
            get
            {
                return XboxConsole.SendTextCommand("drivelist").Replace("drivename=", ",");
            }
        }

    }
}