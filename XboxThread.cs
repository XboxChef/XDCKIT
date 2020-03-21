using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDevkit
{
    class XboxThread : IXboxThread
    {
        private static IXboxConsole Console = new XboxConsoleClass().ActiveXboxConsole;
        public uint CurrentProcessor
        {
            get;
        }

        public uint LastError
        {
            get;
        }

        public XBOX_EVENT_INFO StopEventInfo
        {
            get;
        }

        public XBOX_THREAD_INFO ThreadInfo
        {
            get;
        }

        public IXboxStackFrame TopOfStack
        {
            get;
        }

        public void Continue(bool Exception)
        {
            if (Exception == true)
            {
                Console.SendTextCommand("Continue", out _);
            }

        }

        public void Halt()
        {
            Console.SendTextCommand("Halt",out _);
        }

        public void Resume()
        {
            Console.SendTextCommand("Resume", out _);
        }

        public void Suspend()
        {
            Console.SendTextCommand("Suspend", out _);
        }
    }
}
