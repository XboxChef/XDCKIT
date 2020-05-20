using System.Net.Sockets;

namespace XDevkit
{
    public interface IXboxConsole
    {
        uint ResolveFunction(string ModuleName, uint Ordinal);
        bool Connected { get; set; }
        bool Connect(string XboxNameOrIP = "defualt");
        TcpClient XboxName { get; set; }
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

    }
}