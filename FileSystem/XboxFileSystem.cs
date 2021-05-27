//Do Not Delete This Comment... 
//Made By TeddyHammer on 04/22/21
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2015.
//Thank You for looking love you guys...

using System;
using System.IO;
using System.Net.Sockets;

namespace XDCKIT
{
    public class XboxFileSystem
    {
        private static string Directory = string.Empty;
        // Full file name   
        private static string fileName = string.Empty;
        FileInfo fi = new FileInfo(fileName);
        // get the file attributes for file or directory
        FileAttributes Path = File.GetAttributes(Directory);
        XboxConsole Xbox = new XboxConsole();

        public XboxFileSystem()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string ChangeTime(string directory)
        {
            fileName = directory;
            return fi.LastWriteTime.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreationTime(string directory)
        {
            fileName = directory;
            DateTime creationTime = fi.CreationTime;
            return creationTime.ToString();
        }

        /// <summary>
        /// Get the specified folder path from the console.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] DirectoryFiles(string path)
        {
            string sdr = string.Concat("dir name=\"{0}\"", path);
            return new[] { XboxConsole.SendTextCommand(sdr) };

        }
        /// <summary>
        /// determines if it's a file or Directory
        /// </summary>
        public bool IsDirectory(string directory)
        {
            Directory = directory;
            //detect whether its a directory or file
            if ((Path & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly(string directory)
        {
            fileName = directory;
            // File ReadOnly ?  
            return fi.IsReadOnly;
        }
        /// <summary>
        /// Create the specified directory on the console.
        /// </summary>
        /// <param name="path">Directory name.</param>
        public void MakeDirectory(string path)
        {
            string sdr = string.Concat("mkdir name=\"{0}\"", path);
            XboxConsole.SendTextCommand(sdr, out XboxConsole.Response);
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name(string directory)
        {
            fileName = directory;
            return fi.Name;
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
        /// Delete the specified folder path from the console.
        /// </summary>
        /// <param name="path"></param>
        public void RemoveDirectory(string path)
        {
            string sdr = string.Concat("delete name=\"{0}\"", path);
            XboxConsole.SendTextCommand(sdr, out XboxConsole.Response);
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
            fileName = directory;
            return fi.Length;
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