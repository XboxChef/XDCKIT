//// Decompiled with JetBrains decompiler
//// Type: XDevkit.IXboxAutomation
//// Assembly: XDevkit, Version=2.0.21256.0, Culture=neutral, PublicKeyToken=null
//// MVID: 228FB035-F174-45DE-A3EC-F1520A3BE524
//// Assembly location: C:\Program Files (x86)\Microsoft Xbox 360 SDK\bin\win32\xdevkit.dll

//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;

//namespace XDCKIT
//{
//    public class XboxAutomation
//    {
//        public struct XBOX_AUTOMATION_GAMEPAD
//        {
//            public XboxAutomationButtonFlags Buttons;
//            public uint LeftTrigger;
//            public uint RightTrigger;
//            public int LeftThumbX;
//            public int LeftThumbY;
//            public int RightThumbX;
//            public int RightThumbY;
//        }

//        [DllImport("xinput1_4.dll")]
//        static extern void GetInputProcess()
//        {
//           uint UserIndex, out bool SystemProcess
//        }
//        void BindController([In] uint UserIndex, [In] uint QueueLength)
//        {

//        }
//        void UnbindController([In] uint UserIndex)
//        {

//        }
//        void ConnectController([In] uint UserIndex)
//        {

//        }
//        void DisconnectController([In] uint UserIndex)
//        {

//        }
//        void SetGamepadState([In] uint UserIndex, [In] ref XBOX_AUTOMATION_GAMEPAD Gamepad)
//        {

//        }
//        void QueueGamepadState_cpp(
//          [In] uint UserIndex,
//          [In] ref XBOX_AUTOMATION_GAMEPAD GamepadArray,
//          [In] ref uint TimedDurationArray,
//          [In] ref uint CountDurationArray,
//          [In] uint ItemCount,
//          out uint ItemsAddedToQueue);
//        bool QueueGamepadState(
//          [In] uint UserIndex,
//          [In] ref XBOX_AUTOMATION_GAMEPAD Gamepad,
//          [In] uint TimedDuration,
//          [In] uint CountDuration);
//        void ClearGamepadQueue([In] uint UserIndex);
//        void QueryGamepadQueue(uint UserIndex,out uint QueueLength, out uint ItemsInQueue,out uint TimedDurationRemaining,out uint CountDurationRemaining);
//        void GetUserDefaultProfile(out long Xuid)
//        {

//        }
//        void SetUserDefaultProfile([In] long Xuid)
//        {

//        }
//    }
//}
