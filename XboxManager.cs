using System;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace XDevkit
{
    public class XboxManager : IXboxManager
    {
        public static string IPAddress = new XboxConsoleClass().IPAddress.ToString();
        private static  IXboxConsole activeXboxConsole = new XboxConsoleClass().ActiveXboxConsole;
        public XboxManager()
        {
        }
        XboxConsole IXboxManager.OpenConsole(string XboxName)
        {
            return null;
        }

        internal IXboxConsole OpenConsole(string xboxNameOrIP)
        {
            return activeXboxConsole = new XboxConsoleClass(xboxNameOrIP);
        }

        public int TranslateError(int errorCode)
        {
            return errorCode;
        }

        public string DefaultConsole
        {
            get
            {
                using (XmlReader reader = XmlReader.Create("Settings.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            //return only when you have START tag  
                            switch (reader.Name.ToString())
                            {
                                case "IP":

                                    IPAddress = reader.ReadString();
                                    break;
                            }
                        }
                    }
                }
                return "Default";
            }
            set
            {
                using (XmlWriter writer = XmlWriter.Create("Settings.xml"))
                {
                    writer.WriteStartElement("Default");
                    writer.WriteElementString("Description", "Saves Console's IP For Easy Access");
                    writer.WriteElementString("IP", value);
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
        }


    }
}
