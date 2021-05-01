using System;

namespace XDCKIT
{
    public class Offset
    {
        public string Address { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public Offset(string offset, string type, bool getValueFromXbox)
        {
            Address = offset;
            Type = type;
            if (getValueFromXbox)
            {
                Value = getValue(Convert.ToUInt32(Address, 0x10), Type);
            }
        }

        public Offset(string offset, string type, string value)
        {
            Address = offset;
            Type = type;
            Value = value;
        }

        public string getValue(uint offset, string type)
        {
            string hex = "X";
            object rn = null;
            XboxConsole Xbox_Debug_Communicator = new XboxConsole();


                XboxMemoryStream xbms = Xbox_Debug_Communicator.ReturnXboxMemoryStream();
                //Endian IO
                EndianIO IO = new EndianIO(xbms,EndianType.BigEndian);
                IO.Open();
                IO.In.BaseStream.Position = offset;

                if (type == "String" | type == "string")
                    rn = IO.In.ReadString();
                if (type == "Float" | type == "float")
                    rn = IO.In.ReadSingle();
                if (type == "Double" | type == "double")
                    rn = IO.In.ReadDouble();
                if (type == "Short" | type == "short")
                    rn = IO.In.ReadInt16().ToString(hex);
                if (type == "Byte" | type == "byte")
                    rn = IO.In.ReadByte().ToString(hex);
                if (type == "Long" | type == "long")
                    rn = IO.In.ReadInt32().ToString(hex);
                if (type == "Quad" | type == "quad")
                    rn = IO.In.ReadInt64().ToString(hex);

                IO.Close();
                xbms.Close();
                return rn.ToString();
        }
    }
}
