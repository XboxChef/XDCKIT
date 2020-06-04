using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDevkit
{
    public interface IXboxDebugTarget
    {
        void InvalidateMemoryCache(bool v, uint address, uint length);
        void GetMemory(uint address, uint length, byte[] numArray, out uint num);
        void SetMemory(uint address, uint length, byte[] data, out uint num);
    }
}
