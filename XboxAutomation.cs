using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDevkit
{
    class XboxAutomation : IXboxAutomation
    {
        public void BindController(uint UserIndex, uint QueueLength)
        {

        }

        public void ClearGamepadQueue(uint UserIndex)
        {
            
        }

        public void ConnectController(uint UserIndex)
        {
            
        }

        public void DisconnectController(uint UserIndex)
        {
            
        }

        public void GetInputProcess(uint UserIndex, out bool SystemProcess)
        {
            SystemProcess = true;
        }

        public void GetUserDefaultProfile(out long Xuid)
        {
            Xuid = 0;
        }

        public void QueryGamepadQueue(uint UserIndex, out uint QueueLength, out uint ItemsInQueue, out uint TimedDurationRemaining, out uint CountDurationRemaining)
        {
            throw new NotImplementedException();
        }

        public bool QueueGamepadState(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD Gamepad, uint TimedDuration, uint CountDuration)
        {
            throw new NotImplementedException();
        }

        public void QueueGamepadState_cpp(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD GamepadArray, ref uint TimedDurationArray, ref uint CountDurationArray, uint ItemCount, out uint ItemsAddedToQueue)
        {
            throw new NotImplementedException();
        }

        public void SetGamepadState(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD Gamepad)
        {
            
        }

        public void SetUserDefaultProfile(long Xuid)
        {
            
        }

        public void UnbindController(uint UserIndex)
        {
            
        }
    }
}
