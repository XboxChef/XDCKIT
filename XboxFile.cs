using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using XDevkit;

namespace XDevkit
{
    class XboxFile : TcpClient, IXboxFile
    {
        IXboxConsole XConsole;
        public object ChangeTime
        {
            get => changeTime(string.Empty);
            set=> changeTime(value.ToString());
        }

        private string changeTime(string value)//Todo: Add the text Command....
        {
            if (value == null)
            {
                string r;
                XConsole.SendTextCommand("", out r);
                return r;
            }
            else
            {
                string r;
                XConsole.SendTextCommand("", out r);
                return null;
            }
        }
        private string creationTime(string value)//Todo: Add the text Command....
        {
            if (value == null)
            {
                string r;
                XConsole.SendTextCommand("", out r);
                return r;
            }
            else
            {
                string r;
                XConsole.SendTextCommand("", out r);
                return null;
            }
        }


        public object CreationTime
        {
            get => creationTime(string.Empty);
            set => creationTime(value.ToString());
        }

        public bool IsDirectory
        {
            get;
        }

        public bool IsReadOnly
        {
            get;
            set;
        }
        public ulong Size         {
            get;
            set;
        }
        public string Name
        { 
            get; 
            set; 
        }
    }
}
