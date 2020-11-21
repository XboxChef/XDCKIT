/*
	OpenSauceBox: SDK for Xbox User Modding

	See license\Xbox\Xbox for specific license information
*/
using System;

namespace XDevkit
{
    /// <summary>
    /// Xbox command status response.
    /// </summary>
    public class StatusResponse
    {
        public string Full { get; private set; }
        public ResponseType Type { get; private set; }
        public string Message { get; private set; }
        public bool Success { get { return ((int)Type & 200) == 200; } }

        public StatusResponse(string full, ResponseType type, string message)
        {
            this.Full = full;
            this.Type = type;
            this.Message = message;
        }
    };
    /// <summary>Thrown when an Yelo.Debug API function fails</summary>
    public class ApiException : Exception
    {

        public ApiException(string message) : base(message) { }
    };

    /// <summary>Represents errors that occur when there is no debug connection detected between the xbox and pc.</summary>
    public class NoConnectionException : ApiException
    {
        public NoConnectionException() : base("Requires debug connection.") { }
    };
    /// <summary>Represents errors that occur when there is no debug connection detected between the xbox and pc.</summary>
    public class FailedConnectionException : ApiException
    {
        public FailedConnectionException() : base("Failed Connection") { }
    };



}