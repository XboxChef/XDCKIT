//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it 3 years and counting...
//Thank You for looking love you guys...

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;

namespace XDevkit
{
    public class XboxFileSystem : IXboxFile
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return "";
            }
            set
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        public object CreationTime
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public object ChangeTime
        {
            get;
            set;
        }

        /// <summary>
        /// Size of File
        /// </summary>
        public ulong Size
        {
            get => (ulong)Size.ToString().Length;
            set
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDirectory
        {
            get;
            set;
        }

        [Browsable(false)]
        public static StreamReader Reader;

        /// <summary>
        /// Get the specified folder path from the console.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] DirectoryFiles(string path)
        {
            string sdr = string.Concat("dir name=\"{0}\"", path);
            return new[] { Xbox.SendTextCommand(sdr) };
        }

        /// <summary>
        /// Get the specified directory information from the console.
        /// </summary>
        /// <param name="name">Directory name.</param>
        public void GetDirectory(string path)
        {
            string sdr = string.Concat("dir name=\"{0}\"", path);
            Xbox.SendTextCommand(sdr, out Xbox.Response);
        }

        /// <summary>
        /// Get the console drive names.
        /// </summary>
        public string Drives
        {
            get
            {
                return Xbox.SendTextCommand("drivelist").Replace("drivename=", string.Empty);
            }
        }



        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        /// <summary>
        /// Create the specified directory on the console.
        /// </summary>
        /// <param name="path">Directory name.</param>
        public void MakeDirectory(string path)
        {
            string sdr = string.Concat("mkdir name=\"{0}\"", path);
            Xbox.SendTextCommand(sdr, out Xbox.Response);
        }

        /// <summary>
        /// Delete the specified folder path from the console.
        /// </summary>
        /// <param name="path"></param>
        public void RemoveDirectory(string path)
        {
            string sdr = string.Concat("delete name=\"{0}\"", path);
            Xbox.SendTextCommand(sdr, out Xbox.Response);
        }

        /// <summary>
        /// Create the specified directory path on the console.
        /// </summary>
        /// <param name="path">Directory name.</param>
        public void CreateDirectory(string path)
        {
            string sdr = string.Concat("mkdir name=\"{0}\"", path);
            Xbox.SendTextCommand(sdr, out Xbox.Response);
        }

        /// <summary>
        /// Deletes a file on the xbox.
        /// </summary>
        /// <param name="filePath">File to delete.</param>
        public void DeleteFile(string filePath)
        {
            string dre = string.Concat("delete name=\"{0}\"", filePath);
            Xbox.SendTextCommand(dre);
        }

        /// <summary>
        /// Renames or moves a file on the xbox.
        /// </summary>
        /// <param name="oldFileName">Old file name.</param>
        /// <param name="newFileName">New file name.</param>
        public void RenameFile(string OldFileName, string NewFileName)
        {
            string ren = string.Concat("rename name=\"{0}\" newname=\"{1}\"", OldFileName, NewFileName);
            Xbox.SendTextCommand(ren);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localFolder"></param>
        /// <param name="remoteFolderToSaveIn"></param>
        public void UploadDirectory(string localFolder, string remoteFolderToSaveIn)
        {
            string str4;
            string[] files = Directory.GetFiles(localFolder);
            string[] directories = Directory.GetDirectories(localFolder);
            if (localFolder[localFolder.Length - 1] == '\\')
            {
                localFolder = localFolder.Substring(0, localFolder.Length - 1);
            }
            if (remoteFolderToSaveIn[remoteFolderToSaveIn.Length - 1] == '\\')
            {
                remoteFolderToSaveIn = remoteFolderToSaveIn.Substring(0, remoteFolderToSaveIn.Length - 1);
            }
            string[] strArray3 = localFolder.Split(new char[] { '\\' });
            string str = strArray3[strArray3.Length - 1];
            string directory = remoteFolderToSaveIn + @"\" + str + @"\";
            CreateDirectory(directory);
            foreach (string str3 in files)
            {
                strArray3 = str3.Split(new char[] { '\\' });
                str4 = strArray3[strArray3.Length - 1];
                UploadFile(str3, directory + str4);
            }
            foreach (string str5 in directories)
            {
                strArray3 = str5.Split(new char[] { '\\' });
                str4 = strArray3[strArray3.Length - 1];
                UploadDirectory(str5, directory);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="remoteName"></param>
        public void UploadFile(string localName, string remoteName)
        {
            SendFile(localName, remoteName);
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
            Xbox.Wait(size);
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
            Xbox.Wait(data.Length);
            XboxClient.XboxName.Client.Receive(data, data.Length, SocketFlags.None);
        }

        /// <summary>
        /// Receives binary data of specified size sent from the xbox.
        /// </summary>
        /// <param name="data"></param>
        public void ReceiveBinaryData(byte[] data, int offset, int size)
        {
            Xbox.Wait(size);
            XboxClient.XboxName.Client.Receive(data, offset, size, SocketFlags.None);
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
            Xbox.SendTextCommand("sendfile name=\"{0}\" length={1}" + remoteName + lfs.Length);

            int mainIterations = (int)lfs.Length / XboxClient.XboxName.Client.SendBufferSize;
            int remainder = (int)lfs.Length % XboxClient.XboxName.Client.SendBufferSize;

            for (int i = 0; i < mainIterations; i++)
            {
                lfs.Read(fileData, 0, fileData.Length);
                SendBinaryData(fileData);
            }
            lfs.Read(fileData, 0, remainder);
            SendBinaryData(fileData, remainder);

            lfs.Close();
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
        /// 
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="remoteName"></param>
        public void DownloadFile(string localName, string remoteName)
        {
            ReceiveFile(localName, remoteName);
        }

        /// <summary>
        /// Receives a file from the xbox.
        /// </summary>
        /// <param name="localName">PC file name.</param>
        /// <param name="remoteName">Xbox file name.</param>
        public void ReceiveFile(string localName, string remoteName)
        {
            Xbox.SendTextCommand("getfile name=\"{0}\"" + remoteName);
            int fileSize = BitConverter.ToInt32(ReceiveBinaryData(4), 0);
            using (var lfs = new System.IO.FileStream(localName, FileMode.Create))
            {
                byte[] fileData = new byte[XboxClient.XboxName.Client.ReceiveBufferSize];

                int mainIterations = fileSize / XboxClient.XboxName.Client.ReceiveBufferSize;
                int remainder = fileSize % XboxClient.XboxName.Client.ReceiveBufferSize;

                for (int i = 0; i < mainIterations; i++)
                {
                    fileData = ReceiveBinaryData(fileData.Length);
                    lfs.Write(fileData, 0, fileData.Length);
                }
                fileData = ReceiveBinaryData(remainder);
                lfs.Write(fileData, 0, remainder);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localFolderToSaveIn"></param>
        /// <param name="remoteFolderPath"></param>
        public void DownloadDirectory(string localFolderToSaveIn, string remoteFolderPath)
        {
            string str4;
            string[] files = GetFiles(remoteFolderPath);
            string[] directories = DirectoryFiles(remoteFolderPath);
            if (remoteFolderPath[remoteFolderPath.Length - 1] == '\\')
            {
                remoteFolderPath = remoteFolderPath.Substring(0, remoteFolderPath.Length - 1);
            }
            if (localFolderToSaveIn[localFolderToSaveIn.Length - 1] == '\\')
            {
                localFolderToSaveIn = localFolderToSaveIn.Substring(0, localFolderToSaveIn.Length - 1);
            }
            string[] strArray3 = remoteFolderPath.Split(new char[] { '\\' });
            string str = strArray3[strArray3.Length - 1];
            string path = localFolderToSaveIn + @"\" + str + @"\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (string str3 in files)
            {
                strArray3 = str3.Split(new char[] { '\\' });
                str4 = strArray3[strArray3.Length - 1];
                DownloadFile(path + str4, str3);
            }
            foreach (string str5 in directories)
            {
                strArray3 = str5.Split(new char[] { '\\' });
                str4 = strArray3[strArray3.Length - 1];
                DownloadDirectory(path, str5);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <returns></returns>
        private string[] GetFiles(string FolderPath)//todo
        {
            return new[] { Xbox.SendTextCommand("DIRLIST NAME=" + FolderPath + "\"") };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public bool DirectoryExists(string FolderPath)
        {
            return File.Exists(Xbox.SendTextCommand("DIRLIST NAME=" + FolderPath + "\""));
        }

    }
}
