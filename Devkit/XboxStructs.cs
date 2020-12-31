﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDevkit
{
	#region Xbox Structs
	public struct XBOX_AUTOMATION_GAMEPAD
	{
		public XboxAutomationButtonFlags Buttons;

		public uint LeftTrigger;

		public uint RightTrigger;

		public int LeftThumbX;

		public int LeftThumbY;

		public int RightThumbX;

		public int RightThumbY;
	}
	public struct wChar
	{
		public string Text;
	}

	/// <summary>
	/// Xbox debug connection information.
	/// </summary>
	public struct DebugConnection
	{
		public System.Net.IPAddress IP;
		public string Name;
		public DebugConnection(System.Net.IPAddress ip, string name)
		{
			IP = ip;
			Name = name;
		}
	};
	public struct XBOX_SECTION_INFO
	{
		public string Name;

		public uint BaseAddress;

		public uint Size;

		public uint Index;

		public XboxSectionInfoFlags Flags;
	}

	public struct XBOX_THREAD_INFO
	{
		public uint ThreadId;

		public uint SuspendCount;

		public uint Priority;

		public uint TlsBase;

		public uint StartAddress;

		public uint StackBase;

		public uint StackLimit;

		public uint StackSlackSpace;

		public object CreateTime;

		public string Name;
	}

	public struct XBOX_USER
	{
		public string UserName;

		public XboxAccessFlags Access;
	}

	public struct XBOX_DUMP_SETTINGS
	{
		public XboxDumpReportFlags Flags;

		public string NetworkPath;
	}

	public struct XBOX_EVENT_INFO
	{
		public XboxDebugEventType Event;

		public short IsThreadStopped;

		//public IXboxThread Thread;

		//public IXboxModule Module;

		//public IXboxSection Section;

		public XboxExecutionState ExecState;

		public string Message;

		public uint Code;

		public uint Address;

		public XboxExceptionFlags Flags;

		public uint ParameterCount;

		public uint[] Parameters;
	}

	public struct XBOX_FUNCTION_INFO
	{
		public XboxFunctionType FunctionType;

		public uint BeginAddress;

		public uint PrologEndAddress;

		public uint FunctionEndAddress;
	}

	public struct XBOX_MODULE_INFO
	{
		public string Name;

		public string FullName;

		public uint BaseAddress;

		public uint Size;

		public uint TimeStamp;

		public uint CheckSum;

		public XboxModuleInfoFlags Flags;
	}

	public struct XBOX_PROCESS_INFO
	{
		public uint ProcessId;

		public string ProgramName;
	}
	public struct XBOX_Hardware_Info
	{
		public uint Flags;
		public byte NumberOfProcessors, PCIBridgeRevisionID;
		public byte[] ReservedBytes;
		public ushort BldrMagic, BldrFlags;
	}
	public struct XBOX_Vector2
	{
		public float x, y;
	}
	public struct XBOX_Vector3
	{
		public float x, y, z;
	}
	#endregion
}