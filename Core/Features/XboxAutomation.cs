using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XDCKIT
{
    public class XboxAutomation
    {
        static  void GetInputProcess(UserIndex Index, out bool SystemProcess)
        {
            SystemProcess = false;
            XboxConsole.SendTextCommand("autoinput user=" + Index + " process");
        }
        void BindController(UserIndex Index, uint QueueLength)
        {
            XboxConsole.SendTextCommand("autoinput user=" + Index + " bind queuelen="+ QueueLength);
        }
        void UnbindController(UserIndex Index)
        {
            XboxConsole.SendTextCommand("autoinput user=" + Index + " unbind");
        }
        void ConnectController(UserIndex Index)
        {
            XboxConsole.SendTextCommand("autoinput user=" + Index + " connect");
        }
        void DisconnectController(UserIndex Index) 
        {
            XboxConsole.SendTextCommand("autoinput user=" + Index + " disconnect");
        }
        void SetGamepadState(UserIndex Index, ref XBOX_AUTOMATION_GAMEPAD Gamepad)
        {
            XboxConsole.SendTextCommand("autoinput user=" + Index + " setpacket");
        }
        bool QueueGamepadState(UserIndex Index, ref XBOX_AUTOMATION_GAMEPAD Gamepad,uint TimedDuration,uint CountDuration)
        {
            XboxConsole.SendTextCommand("autoinput user=" + Index + " queuepackets count="+ CountDuration);
            return true;
            
        }
        void ClearGamepadQueue(UserIndex Index)
        {
            XboxConsole.SendTextCommand("autoinput user=" + Index + " clearqueue");
        }
        public void QueryGamepadQueue(UserIndex Index, out uint QueueLength, out uint ItemsInQueue, out uint TimedDurationRemaining, out uint CountDurationRemaining)
        {
            QueueLength = 0; 
            ItemsInQueue = 0; 
            TimedDurationRemaining = 0;
            CountDurationRemaining = 0;
            XboxConsole.SendTextCommand("autoinput user=" + Index + " queryqueue");
        }
        void GetUserDefaultProfile(out long Xuid)
        {
            XboxConsole.SendTextCommand("autoprof");
            Xuid = 0;
        }
        void SetUserDefaultProfile(long Xuid)
        {
            XboxConsole.SendTextCommand("autoprof xuid="+ Xuid);
        }
    }
}
