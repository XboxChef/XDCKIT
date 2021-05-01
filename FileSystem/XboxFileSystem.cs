//Do Not Delete This Comment... 
//Made By TeddyHammer on 04/22/21
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2015.
//Thank You for looking love you guys...

using System;
using System.IO;

namespace XDCKIT
{
    public class XboxFileSystem
    {
        XboxConsole Xbox = new XboxConsole();
        public XboxFileSystem()
        {
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
        /// Create the specified directory on the console.
        /// </summary>
        /// <param name="path">Directory name.</param>
        public void MakeDirectory(string path)
        {
            string sdr = string.Concat("mkdir name=\"{0}\"", path);
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
        /// Delete the specified folder path from the console.
        /// </summary>
        /// <param name="path"></param>
        public void RemoveDirectory(string path)
        {
            string sdr = string.Concat("delete name=\"{0}\"", path);
            XboxConsole.SendTextCommand(sdr, out XboxConsole.Response);
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
        internal void DirectoryFiles(string directory)
        {
            throw new NotImplementedException();
        }
    }
}