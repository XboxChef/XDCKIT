using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDevkit;

namespace XDevkit
{

	#region XNotify
    /// <summary>
    /// Made By TeddyHammer If Copied You Must Give Credit... Do Not Delete This Comment..
    /// </summary>
	public class XNotify
	{
        public static Xbox XConsole = new Xbox();
		public static void Show(string Message)
		{
             Show(Message, true);
		}

		public static void Show(string Message, bool ISON)
		{
            if (ISON == true)
            {
                Show(Message, XNotiyLogo.FLASHING_XBOX_LOGO);
            }
            else
            {

            }
		}
        public static void Show(string Message, XNotiyLogo Logo, bool ISON)
        {
            if (ISON == true)
            {
                Show(Message, Logo);
            }
            else
            {

            }
        }

        public static void Show(string message, XNotiyLogo Logo)
		{
            int String = 2;
            int Int = 1;
            string command = "consolefeatures ver=2" + " type=12 params=\"A\\0\\A\\2\\" + String + "/" + message.Length + "\\" + Functions.ConvertStringToHex(message, Encoding.ASCII) + "\\" + Int + "\\";
            switch (Logo)
            {
                case XNotiyLogo.XBOX_LOGO:
                    command += (int)XNotiyLogo.XBOX_LOGO + "\\\"";
                    break;
                case XNotiyLogo.NEW_MESSAGE_LOGO:
                    command += (int)XNotiyLogo.NEW_MESSAGE_LOGO + "\\\"";
                    break;
                case XNotiyLogo.FRIEND_REQUEST_LOGO:
                    command += (int)XNotiyLogo.FRIEND_REQUEST_LOGO + "\\\"";
                    break;
                case XNotiyLogo.NEW_MESSAGE:
                    command += (int)XNotiyLogo.NEW_MESSAGE + "\\\"";
                    break;
                case XNotiyLogo.FLASHING_XBOX_LOGO:
                    command += (int)XNotiyLogo.FLASHING_XBOX_LOGO + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_SENT_YOU_A_MESSAGE:
                    command += (int)XNotiyLogo.GAMERTAG_SENT_YOU_A_MESSAGE + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_SINGED_OUT:
                    command += (int)XNotiyLogo.GAMERTAG_SINGED_OUT + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_SIGNEDIN:
                    command += (int)XNotiyLogo.GAMERTAG_SIGNEDIN + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_SIGNED_INTO_XBOX_LIVE:
                    command += (int)XNotiyLogo.GAMERTAG_SIGNED_INTO_XBOX_LIVE + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_SIGNED_IN_OFFLINE:
                    command += (int)XNotiyLogo.GAMERTAG_SIGNED_IN_OFFLINE + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_WANTS_TO_CHAT:
                    command += (int)XNotiyLogo.GAMERTAG_WANTS_TO_CHAT + "\\\"";
                    break;
                case XNotiyLogo.DISCONNECTED_FROM_XBOX_LIVE:
                    command += (int)XNotiyLogo.DISCONNECTED_FROM_XBOX_LIVE + "\\\"";
                    break;
                case XNotiyLogo.DOWNLOAD:
                    command += (int)XNotiyLogo.DOWNLOAD + "\\\"";
                    break;
                case XNotiyLogo.FLASHING_MUSIC_SYMBOL:
                    command += (int)XNotiyLogo.FLASHING_MUSIC_SYMBOL + "\\\"";
                    break;
                case XNotiyLogo.FLASHING_HAPPY_FACE:
                    command += (int)XNotiyLogo.FLASHING_HAPPY_FACE + "\\\"";
                    break;
                case XNotiyLogo.FLASHING_FROWNING_FACE:
                    command += (int)XNotiyLogo.FLASHING_FROWNING_FACE + "\\\"";
                    break;
                case XNotiyLogo.FLASHING_DOUBLE_SIDED_HAMMER:
                    command += (int)XNotiyLogo.GAMERTAG_WANTS_TO_CHAT + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_WANTS_TO_CHAT_2:
                    command += (int)XNotiyLogo.GAMERTAG_WANTS_TO_CHAT_2 + "\\\"";
                    break;
                case XNotiyLogo.PLEASE_REINSERT_MEMORY_UNIT:
                    command += (int)XNotiyLogo.PLEASE_REINSERT_MEMORY_UNIT + "\\\"";
                    break;
                case XNotiyLogo.PLEASE_RECONNECT_CONTROLLERM:
                    command += (int)XNotiyLogo.PLEASE_RECONNECT_CONTROLLERM + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_HAS_JOINED_CHAT:
                    command += (int)XNotiyLogo.GAMERTAG_HAS_JOINED_CHAT + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_HAS_LEFT_CHAT:
                    command += (int)XNotiyLogo.GAMERTAG_HAS_LEFT_CHAT + "\\\"";
                    break;
                case XNotiyLogo.GAME_INVITE_SENT:
                    command += (int)XNotiyLogo.GAME_INVITE_SENT + "\\\"";
                    break;
                case XNotiyLogo.FLASH_LOGO:
                    command += (int)XNotiyLogo.FLASH_LOGO + "\\\"";
                    break;
                case XNotiyLogo.PAGE_SENT_TO:
                    command += (int)XNotiyLogo.PAGE_SENT_TO + "\\\"";
                    break;
                case XNotiyLogo.FOUR_2:
                    command += (int)XNotiyLogo.FOUR_2 + "\\\"";
                    break;
                case XNotiyLogo.FOUR_3:
                    command += (int)XNotiyLogo.FOUR_3 + "\\\"";
                    break;
                case XNotiyLogo.ACHIEVEMENT_UNLOCKED:
                    command += (int)XNotiyLogo.ACHIEVEMENT_UNLOCKED + "\\\"";
                    break;
                case XNotiyLogo.FOUR_9:
                    command += (int)XNotiyLogo.FOUR_9 + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_WANTS_TO_TALK_IN_VIDEO_KINECT:
                    command += (int)XNotiyLogo.GAMERTAG_WANTS_TO_TALK_IN_VIDEO_KINECT + "\\\"";
                    break;
                case XNotiyLogo.VIDEO_CHAT_INVITE_SENT:
                    command += (int)XNotiyLogo.VIDEO_CHAT_INVITE_SENT + "\\\"";
                    break;
                case XNotiyLogo.READY_TO_PLAY:
                    command += (int)XNotiyLogo.READY_TO_PLAY + "\\\"";
                    break;
                case XNotiyLogo.CANT_DOWNLOAD_X:
                    command += (int)XNotiyLogo.CANT_DOWNLOAD_X + "\\\"";
                    break;
                case XNotiyLogo.DOWNLOAD_STOPPED_FOR_X:
                    command += (int)XNotiyLogo.DOWNLOAD_STOPPED_FOR_X + "\\\"";
                    break;
                case XNotiyLogo.FLASHING_XBOX_CONSOLE:
                    command += (int)XNotiyLogo.FLASHING_XBOX_CONSOLE + "\\\"";
                    break;
                case XNotiyLogo.X_SENT_YOU_A_GAME_MESSAGE:
                    command += (int)XNotiyLogo.X_SENT_YOU_A_GAME_MESSAGE + "\\\"";
                    break;
                case XNotiyLogo.DEVICE_FULL:
                    command += (int)XNotiyLogo.DEVICE_FULL + "\\\"";
                    break;
                case XNotiyLogo.FOUR_7:
                    command += (int)XNotiyLogo.FOUR_7 + "\\\"";
                    break;
                case XNotiyLogo.FLASHING_CHAT_ICON:
                    command += (int)XNotiyLogo.FLASHING_CHAT_ICON + "\\\"";
                    break;
                case XNotiyLogo.ACHIEVEMENTS_UNLOCKED:
                    command += (int)XNotiyLogo.ACHIEVEMENTS_UNLOCKED + "\\\"";
                    break;
                case XNotiyLogo.X_HAS_SENT_YOU_A_NUDGE:
                    command += (int)XNotiyLogo.X_HAS_SENT_YOU_A_NUDGE + "\\\"";
                    break;
                case XNotiyLogo.MESSENGER_DISCONNECTED:
                    command += (int)XNotiyLogo.MESSENGER_DISCONNECTED + "\\\"";
                    break;
                case XNotiyLogo.BLANK:
                    command += (int)XNotiyLogo.BLANK + "\\\"";
                    break;
                case XNotiyLogo.CANT_SIGN_IN_MESSENGER:
                    command += (int)XNotiyLogo.CANT_SIGN_IN_MESSENGER + "\\\"";
                    break;
                case XNotiyLogo.MISSED_MESSENGER_CONVERSATION:
                    command += (int)XNotiyLogo.MISSED_MESSENGER_CONVERSATION + "\\\"";
                    break;
                case XNotiyLogo.FAMILY_TIMER_X_TIME_REMAINING:
                    command += (int)XNotiyLogo.FAMILY_TIMER_X_TIME_REMAINING + "\\\"";
                    break;
                case XNotiyLogo.DISCONNECTED_XBOX_LIVE_11_MINUTES_REMAINING:
                    command += (int)XNotiyLogo.DISCONNECTED_XBOX_LIVE_11_MINUTES_REMAINING + "\\\"";
                    break;
                case XNotiyLogo.KINECT_HEALTH_EFFECTS:
                    command += (int)XNotiyLogo.KINECT_HEALTH_EFFECTS + "\\\"";
                    break;
                case XNotiyLogo.FOUR_5:
                    command += (int)XNotiyLogo.FOUR_5 + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_WANTS_YOU_TO_JOIN_AN_XBOX_LIVE_PARTY:
                    command += (int)XNotiyLogo.GAMERTAG_WANTS_YOU_TO_JOIN_AN_XBOX_LIVE_PARTY + "\\\"";
                    break;
                case XNotiyLogo.PARTY_INVITE_SENT:
                    command += (int)XNotiyLogo.PARTY_INVITE_SENT + "\\\"";
                    break;
                case XNotiyLogo.GAME_INVITE_SENT_TO_XBOX_LIVE_PARTY:
                    command += (int)XNotiyLogo.GAME_INVITE_SENT_TO_XBOX_LIVE_PARTY + "\\\"";
                    break;
                case XNotiyLogo.KICKED_FROM_XBOX_LIVE_PARTY:
                    command += (int)XNotiyLogo.KICKED_FROM_XBOX_LIVE_PARTY + "\\\"";
                    break;
                case XNotiyLogo.NULLED:
                    command += (int)XNotiyLogo.NULLED + "\\\"";
                    break;
                case XNotiyLogo.DISCONNECTED_XBOX_LIVE_PARTY:
                    command += (int)XNotiyLogo.DISCONNECTED_XBOX_LIVE_PARTY + "\\\"";
                    break;
                case XNotiyLogo.DOWNLOADED:
                    command += (int)XNotiyLogo.DOWNLOADED + "\\\"";
                    break;
                case XNotiyLogo.CANT_CONNECT_XBL_PARTY:
                    command += (int)XNotiyLogo.CANT_CONNECT_XBL_PARTY + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_HAS_JOINED_XBL_PARTY:
                    command += (int)XNotiyLogo.GAMERTAG_HAS_JOINED_XBL_PARTY + "\\\"";
                    break;
                case XNotiyLogo.GAMERTAG_HAS_LEFT_XBL_PARTY:
                    command += (int)XNotiyLogo.GAMERTAG_HAS_LEFT_XBL_PARTY + "\\\"";
                    break;
                case XNotiyLogo.GAMER_PICTURE_UNLOCKED:
                    command += (int)XNotiyLogo.GAMER_PICTURE_UNLOCKED + "\\\"";
                    break;
                case XNotiyLogo.AVATAR_AWARD_UNLOCKED:
                    command += (int)XNotiyLogo.AVATAR_AWARD_UNLOCKED + "\\\"";
                    break;
                case XNotiyLogo.JOINED_XBL_PARTY:
                    command += (int)XNotiyLogo.JOINED_XBL_PARTY + "\\\"";
                    break;
                case XNotiyLogo.PLEASE_REINSERT_USB_STORAGE_DEVICE:
                    command += (int)XNotiyLogo.PLEASE_REINSERT_USB_STORAGE_DEVICE + "\\\"";
                    break;
                case XNotiyLogo.PLAYER_MUTED:
                    command += (int)XNotiyLogo.PLAYER_MUTED + "\\\"";
                    break;
                case XNotiyLogo.PLAYER_UNMUTED:
                    command += (int)XNotiyLogo.PLAYER_UNMUTED + "\\\"";
                    break;
                case XNotiyLogo.FLASHING_CHAT_SYMBOL:
                    command += (int)XNotiyLogo.FLASHING_CHAT_SYMBOL + "\\\"";
                    break;
                case XNotiyLogo.UPDATING:
                    command += (int)XNotiyLogo.UPDATING + "\\\"";
                    break;
                default:
                    command += "\\\"";
                    break;
            }
            XConsole.SendTextCommand(command);
        }


	}
	#endregion
}
