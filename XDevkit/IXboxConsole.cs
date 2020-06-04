using System.Net.Sockets;

namespace XDevkit
{
    public interface IXboxConsole
    {
        Xbox XboxConsole { get;}
        uint ResolveFunction(string ModuleName, uint Ordinal);
        bool Connected { get; set; }
        bool Connect(string XboxNameOrIP = "defualt");
        TcpClient XboxName { get; set; }
        int ConnectTimeout { get; set; }
        int ConversationTimeout { get; set; }
        bool IPAddress { get; }
        IXboxDebugTarget DebugTarget { get; }
        XBOX_PROCESS_INFO RunningProcessInfo { get; }
        string Name
        {
            get;
            set;
        }
        void Disconnect();
        void CloseConnection(uint Connection);
        string SendTextCommand(string Command);
        void SendTextCommand(string Command, out string response);
        string GetBoxID();
        string GetConsoleID();
        void SetConsoleColor(XboxColor Color);
        string GetDMVersion();
        string GetSystemInfo(Info Type);
        void CreateDirectory(string name);
        void RenameFile(string OldFileName, string NewFileName);
        void DeleteFile(string fileName);
        void Reboot(string Name, string MediaDirectory, string CmdLine, XboxRebootFlags Flags);
        void XboxShortcut(string UI);
        void XboxShortcut(XboxShortcuts UI);
        void Reboot(XboxReboot Warm_or_Cold);
        void Freeze_Console(XboxSwitch Freeze);
        string ConsoleType();
        string GetCPUKey();
        uint GetKernalVersion();
        uint GetTemperature(TemperatureFlag TemperatureType);
        void SetLeds(LEDState Top_Left, LEDState Top_Right, LEDState Bottom_Left, LEDState Bottom_Right);
        uint XamGetCurrentTitleId();
        void ShutDownConsole();
        void XNotify(string Text);
        void XNotify(string Text, uint Type);
        void constantMemorySet(uint Address, uint Value, uint TitleID);
        void constantMemorySet(uint Address, uint Value, uint IfValue, uint TitleID);
        void constantMemorySetting(uint Address, uint Value, bool useIfValue, uint IfValue, bool usetitleID, uint TitleID);
        void constantMemorySet(uint Address, uint Value);
        StatusResponse SendCommand(string command, params object[] args);
        void SetMemory(uint Address, byte[] Data);
        void SetMemory(uint Address, uint BytesToWrite, byte[] Data, out uint BytesWritten);
        byte[] GetMemory(uint Address, uint Length);
        void GetMemory(uint Address, uint BytesToRead, byte[] Data, out uint BytesRead);
        string ReceiveMultilineResponse();
        string ReceiveSocketLine();
        void Wait(int targetLength);
        void Wait(WaitType type);
        StatusResponse ReceiveStatusResponse();
        void FlushSocketBuffer();
        void FlushSocketBuffer(int size);
        bool Ping();
        bool Ping(int waitTime);
        bool SetBool(uint Address);
        void SetBool(uint Address, bool Value);
        void SetBool(uint Address, bool[] Value);
        string GetString(uint Address, uint size);
        void SetString(uint Address, string String);
        float GetFloat(uint Address);
        float[] GetFloat(uint Address, uint ArraySize);
        void SetFloat(uint Address, float Value);
        void SetFloat(uint Address, float[] Value);
        byte GetByte(uint Address);
        void SetByte(uint Address, byte Value);
        void SetByte(uint Address, byte[] Value);
        sbyte GetSByte(uint Address);
        void SetSByte(uint Address, sbyte Value);
        void SetSByte(uint Address, sbyte[] Value);
        short GetInt16(uint Address);
        short[] GetInt16(uint Address, uint ArraySize);
        void SetInt16(uint Address, short Value);
        void SetInt16(uint Address, short[] Value);
        int GetInt32(uint Address);
        int[] GetInt32(uint Address, uint ArraySize);
        void SetInt32(uint Address, int Value);
        void SetInt32(uint Address, int[] Value);
        long GetInt64(uint Address);
        long[] GetInt64(uint Address, uint ArraySize);
        void SetInt64(uint Address, long Value);
        void SetInt64(uint Address, long[] Value);
        ushort GetUInt16(uint Address);
        ushort[] GetUInt16(uint Address, uint ArraySize);
        void SetUInt16(uint Address, ushort Value);

        void SetUInt16(uint Address, ushort[] Value);
        uint GetUInt32(uint Address);

        uint[] GetUInt32(uint Address, uint ArraySize);
        void SetUInt32(uint Address, uint Value);
        void SetUInt32(uint Address, uint[] Value);
        ulong GetUInt64(uint Address);

        ulong[] GetUInt64(uint Address, uint ArraySize);

        void SetUInt64(uint Address, ulong Value);

        void SetUInt64(uint Address, ulong[] Value);



    }
}