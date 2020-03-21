using System.Collections;
using System.Net.Sockets;

namespace XDevkit
{
	public interface IXboxConsole
	{
		IXboxFile GetFileObject(string Filename);
		IXboxFiles DirectoryFiles(string Directory);
		void DeleteFile(string Filename);
		void RenameFile(string OldName, string NewName);
		void ScreenShot(string Filename);
		void CloseConnection(uint Connection);
		void FindConsole(uint Retries, uint RetryDelay);
		void GetDiskFreeSpace(ushort Drive, out ulong FreeBytesAvailableToCaller, out ulong TotalNumberOfBytes, out ulong TotalNumberOfFreeBytes);
		uint OpenConnection(string Handler);
		void Reboot(string Name, string MediaDirectory, string CmdLine, XboxRebootFlags Flags);
		void ReceiveSocketLine(uint Connection, out string Line);
		int ReceiveStatusResponse(uint Connection, out string Line);
		void SendTextCommand(string text, out string response);

		void XNotify(string message);

		uint ConnectTimeout { get; set; }

		uint ConversationTimeout { get; set; }
		IXboxDebugTarget DebugTarget { get; }
		string Drives { get; }

		uint IPAddress { get; }
		string SystemTime { get; set; }
		XBOX_PROCESS_INFO RunningProcessInfo { get; }
		IXboxAutomation XboxAutomation { get; }
		string Name { get; }

		void SendBinary(uint connectionId, byte[] callData, uint length);
		void ReceiveBinary(uint connectionId, byte[] numArray, uint length, out uint bytesReceived);
	}

public interface IXboxAutomation
	{
		void BindController(uint UserIndex, uint QueueLength);

		void ClearGamepadQueue(uint UserIndex);

		void ConnectController(uint UserIndex);

		void DisconnectController(uint UserIndex);

		void GetInputProcess(uint UserIndex, out bool SystemProcess);
		void GetUserDefaultProfile(out long Xuid);

		void QueryGamepadQueue(uint UserIndex, out uint QueueLength, out uint ItemsInQueue, out uint TimedDurationRemaining, out uint CountDurationRemaining);

		bool QueueGamepadState(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD Gamepad, uint TimedDuration, uint CountDuration);

		void QueueGamepadState_cpp(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD GamepadArray, ref uint TimedDurationArray, ref uint CountDurationArray, uint ItemCount, out uint ItemsAddedToQueue);

		void SetGamepadState(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD Gamepad);
		void SetUserDefaultProfile(long Xuid);
		void UnbindController(uint UserIndex);
	}

	public interface IXboxDebugTarget
	{


		void ConnectAsDebugger(string DebuggerName, XboxDebugConnectFlags Flags);
		void CopyEventInfo(out XBOX_EVENT_INFO EventInfoDest, ref XBOX_EVENT_INFO EventInfoSource);
		void DisconnectAsDebugger();
		void FreeEventInfo(ref XBOX_EVENT_INFO EventInfo);
		void GetMemory(uint Address, uint BytesToRead, byte[] Data, out uint BytesRead);
		void GetMemory_cpp(uint Address, uint BytesToRead, byte[] Data, out uint BytesRead);
		void Go(bool NotStopped);
		void InvalidateMemoryCache(bool ExecutablePages, uint Address, uint Size);
		void IsBreakpoint(uint Address, out XboxBreakpointType Type);
		bool IsDebuggerConnected(out string DebuggerName, out string UserName);
		void PgoSaveSnapshot(string Phase, bool Reset, uint PgoModule);
		void PgoSetAllocScale(uint PgoModule, uint BufferAllocScale);
		void PgoStartDataCollection(uint PgoModule);
		void PgoStopDataCollection(uint PgoModule);
		void RemoveAllBreakpoints();
		void RemoveBreakpoint(uint Address);
		void SetBreakpoint(uint Address);
		void SetDataBreakpoint(uint Address, XboxBreakpointType Type, uint Size);
		void SetInitialBreakpoint();
		void SetMemory(uint Address, uint BytesToWrite, byte[] Data, out uint BytesWritten);
		void SetMemory_cpp(uint Address, uint BytesToWrite, byte[]Data, out uint BytesWritten);
		void Stop(bool AlreadyStopped);
		void StopOn(XboxStopOnFlags StopOn, bool Stop);
		void WriteDump(string Filename, XboxDumpFlags Type);

		XboxConsole Console { get; }
		bool IsDump { get; }
		bool MemoryCacheEnabled { get; set; }

		IXboxMemoryRegions MemoryRegions { get; }

		IXboxModules Modules { get; }
		XBOX_PROCESS_INFO RunningProcessInfo { get; }
		IXboxThreads Threads { get; }

		XboxManager XboxManager { get; }
	}

	public interface XboxConsole : IXboxConsole
	{

	}

	public interface IXboxStackFrame
	{
		void FlushRegisterChanges();
		bool GetRegister32(XboxRegisters32 Register, out int Value);
		bool GetRegister64(XboxRegisters64 Register, out long Value);
		bool GetRegisterDouble(XboxRegistersDouble Register, out double Value);
		bool GetRegisterVector(XboxRegistersVector Register, float[] Value);
		bool GetRegisterVector_cpp(XboxRegistersVector Register, out float Value);
		void SetRegister32(XboxRegisters32 Register, int Value);
		void SetRegister64(XboxRegisters64 Register, long Value);
		void SetRegisterDouble(XboxRegistersDouble Register, double Value);
		void SetRegisterVector(XboxRegistersVector Register, float[] Value);
		void SetRegisterVector_cpp(XboxRegistersVector Register, ref float Value);

		bool Dirty { get; }
		XBOX_FUNCTION_INFO FunctionInfo { get; }
		IXboxStackFrame NextStackFrame { get; }
		uint ReturnAddress { get; }
		uint StackPointer { get; }
		bool TopOfStack { get; }
	}

	public interface IXboxThread
	{

		void Continue(bool Exception);
		void Halt();
		void Resume();
		void Suspend();

		uint CurrentProcessor { get; }
		uint LastError { get; }
		XBOX_EVENT_INFO StopEventInfo { get; }
		XBOX_THREAD_INFO ThreadInfo { get; }
		IXboxStackFrame TopOfStack { get; }
	}

	public interface IXboxThreads : IEnumerable
	{

		new IEnumerator GetEnumerator();

		int Count { get; }
	}

	public interface XboxEvents
	{
		void OnStdNotify(XboxDebugEventType EventCode, IXboxEventInfo EventInfo);

		void OnTextNotify(string Source, string Notification);
	}

	public interface IXboxManager
	{
		XboxConsole OpenConsole(string XboxName);
		string DefaultConsole { get; set; }

		int TranslateError(int errorCode);
	}

	public interface IXboxEventInfo
	{
		XBOX_EVENT_INFO xbox_event_info { get; }
	}

	public interface IXboxFile
	{

		object ChangeTime { get; set; }
		object CreationTime { get; set; }
		bool IsDirectory { get; }
		bool IsReadOnly { get; set; }
		ulong Size { get; set; }
		string Name { get; set; }
	}

	public interface IXboxModule
	{
		XBOX_MODULE_INFO xbox_module_info { get; }

		uint GetEntryPointAddress();
		void GetFunctionInfo(uint Address, out XBOX_FUNCTION_INFO FunctionInfo);

		IXboxExecutable Executable { get; }
		uint OriginalSize { get; }
		IXboxSections Sections { get; }
	}

	public interface IXboxMemoryRegions : IEnumerable
	{
		IXboxMemoryRegion xboxMemoryRegion { get; }

		new IEnumerator GetEnumerator();

		int Count { get; }
	}

	public interface IXboxFiles : IEnumerable
	{
		IXboxFile xboxFile{ get; }

		new IEnumerator GetEnumerator();

		int Count { get; }
	}

	public interface IXboxEvents
	{
		void OnStdNotify(XboxDebugEventType EventCode, IXboxEventInfo EventInfo);
		void OnTextNotify(string Source, string Notification);
	}

	public interface IXboxMemoryRegion
	{
		int BaseAddress { get; }
		XboxMemoryRegionFlags Flags { get; }
		int RegionSize { get; }
	}

	public interface IXboxExecutable
	{
		string GetPEModuleName();
	}

	public interface IXboxExecutableInfo
	{
		string BasePath { get; set; }
		string ModuleName { get; }
		string PortableExecutablePath { get; }
		bool PropGetRelativePath { get; set; }
		string PublicSymbolPath { get; }
		uint SizeOfImage { get; }
		bool StoreRelativePath { get; }
		string SymbolGuid { get; }
		string SymbolPath { get; }
		uint TimeDateStamp { get; }
		string XboxExecutablePath { get; }
	}

	public interface IXboxModules : IEnumerable
	{
		IXboxModule XboxModule { get; }

		new IEnumerator GetEnumerator();

		int Count { get; }
	}

	public interface IXboxSection
	{
		XBOX_SECTION_INFO SectionInfo { get; }
	}

	public interface IXboxSections : IEnumerable
	{
		IXboxSection XboxSection { get; }

		new IEnumerator GetEnumerator();

		int Count { get; }
	}
}
