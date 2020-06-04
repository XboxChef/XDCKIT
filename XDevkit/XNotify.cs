using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDevkit
{
	#region XNotify
	public class XNotify
	{
		public readonly static uint Int;
		public Xbox Jtag = new Xbox();
		public readonly static uint JRPCVersion;
		public readonly static uint String;
		public static bool XNotifyEnabled
		{
			get
			{
				return true;
			}
			set
			{

			}
		}
		public void Show(string Message)
		{
             Show(XNotiyLogo.FLASHING_XBOX_LOGO, Message, true);
		}

		public void Show(string Message, bool isON)
		{
			if (isON == true)
			{
				XNotifyEnabled = true;
			}
			if (XNotifyEnabled == true)
			{
				Show(XNotiyLogo.FLASHING_XBOX_LOGO, Message, isON);
			}
		}

		public void Show(XNotiyLogo Type, string Message, bool isON)
		{
			if (isON == true)
			{
				XNotifyEnabled = true;
			}
			if (XNotifyEnabled == true)
			{

				object[] jRPCVersion = new object[] { "consolefeatures ver=", JRPCVersion, " type=12 params=\"A\\0\\A\\2\\", String, "/", Message.Length, "\\", Message.ToHexString(), "\\", Int, "\\", Type, "\\\"" };
				Jtag.SendTextCommand(string.Concat(jRPCVersion));
			}
		}


	}
	#endregion
}
