/*
	OpenSauceBox: SDK for Xbox User Modding

	See license\Xbox\Xbox for specific license information
*/
using System;

namespace XDevkit
{
    /// <summary>Thrown when an Yelo.Debug API function fails</summary>
    public class ApiException : Exception
    {
        public ApiException() : base("An internal method has failed.") { }
        public ApiException(string message) : base(message) { }
        public ApiException(string message, Exception inner) : base(message, inner) { }
    };

    /// <summary>Represents errors that occur when there is no debug connection detected between the xbox and pc.</summary>
    public class NoConnectionException : ApiException
    {
        public NoConnectionException() : base("Requires debug connection.") { }
        public NoConnectionException(string message) : base(message) { }
        public NoConnectionException(string message, Exception inner) : base(message, inner) { }
    };

    /// <summary>Represents errors that occur when a script becomes unresponsive.</summary>
    public class ScriptTimeoutException : ApiException
    {
        public ScriptTimeoutException() : base("Xbox script has become unresponsive.") { }
        public ScriptTimeoutException(string message) : base(message) { }
        public ScriptTimeoutException(string message, Exception inner) : base(message, inner) { }
    };

    /// <summary>Represents errors that occur within the xbox debugging interface.</summary>
    public class XboxDebugException : ApiException
    {
        public XboxDebugException() : base("Unknown error.") { }
        public XboxDebugException(string message) : base(message) { }
        public XboxDebugException(string message, Exception inner) : base(message, inner) { }
    };

    /// <summary>Represents errors that occur when an unsupported method is called.</summary>
    public class UnsupportedException : ApiException
    {
        public UnsupportedException() : base("This method or operation is not supported.") { }
        public UnsupportedException(string message) : base(message) { }
        public UnsupportedException(string message, Exception inner) : base(message, inner) { }
    };

    /// <summary>Represents errors that occur when an xbox memory allocation fails.</summary>
    public class MemoryAllocationException : ApiException
    {
        public MemoryAllocationException() : base("Failed to allocate xbox memory.") { }
        public MemoryAllocationException(string message) : base(message) { }
        public MemoryAllocationException(string message, Exception inner) : base(message, inner) { }
    };

    /// <summary>Represents errors that occur when a corruption is detected in our history pages.</summary>
    public class HistoryCorruptionException : ApiException
    {
        public HistoryCorruptionException() : base("History corruption detected. A new one has been recreated automatically but certain information could be lost.") { }
        public HistoryCorruptionException(string message) : base(message) { }
        public HistoryCorruptionException(string message, Exception inner) : base(message, inner) { }
    };
}