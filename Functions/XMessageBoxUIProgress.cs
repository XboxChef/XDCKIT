namespace XDevkit
{
    using System;
    using System.Runtime.CompilerServices;

    public class XMessageBoxUIProgress : EventArgs
    {
        private XMessageBoxUIProgress()
        {
        }

        public XMessageBoxUIProgress(uint result, uint code)
        {
            Result = result;
            Code = code;
        }

        public uint Result { get; private set; }

        public uint Code { get; private set; }
    }
}

