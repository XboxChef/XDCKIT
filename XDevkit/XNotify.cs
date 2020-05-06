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
		public static Xbox console;
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
		public static void Show(string Message, bool isON)
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
		public static void Show(XNotiyLogo Type, string Message, bool isON)
		{
			if (isON == true)
			{
				XNotifyEnabled = true;
			}
			if (XNotifyEnabled == true)
			{
				object[] arguments = new object[] { 0x18, 0xff, 2, (Message).ToWCHAR(), (int)Type };
				console.CallVoid((int)ThreadType.Title, "xam.xex", 0x290, arguments);
			}
		}

	}
	#endregion
}
