//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2015.
//Thank You for looking love you guys...

namespace XDCKIT
{

    public enum EndianType
    {
        BigEndian,
        LittleEndian
    }
    public enum XMessageBoxIcons
    {
        XBM_NOICON,
        XMB_ERRORICON,
        XMB_WARNINGICON,
        XMB_ALERTICON
    }
    public enum XboxConsoleFeatures
    {
        Debugging = 1,
        SecondaryNIC = 2,
        GB_RAM = 4
    }
    public enum FriendRequestStatus
    {
        RequestSent,
        RequestReceived,
        RequestAccepted,
    }
    public enum FriendStatus
    {
        Offline = -1, // 0xFFFFFFFF
        Online = 0,
        Away = 65536, // 0x00010000
        Busy = 131072, // 0x00020000
    }
    public enum SignInState
    {
        NotSignedIn,
        SignedInLocally,
        SignedInToLive,
    }
    /// <summary>
    /// Xbox response type.
    /// </summary>
    public enum ResponseType
    {
        // Success
        SingleResponse = 200,  //OK
        Connected = 201,
        MultiResponse = 202,  //terminates with period
        BinaryResponse = 203,
        ReadyForBinary = 204,
        NowNotifySession = 205,  // Notification channel/ dedicated connection

        // Errors
        UndefinedError = 400,
        MaxConnectionsExceeded = 401,
        FileNotFound = 402,
        NoSuchModule = 403,
        MemoryNotMapped = 404,  //setzerobytes or set mem failed
        NoSuchThread = 405,
        ClockNotSet = 406,  //linetoolong or clock not set
        UnknownCommand = 407,
        NotStopped = 408,
        FileMustBeCopied = 409,
        FileAlreadyExists = 410,
        DirectoryNotEmpty = 411,
        BadFileName = 412,
        FileCannotBeCreated = 413,
        AccessDenied = 414,
        NoRoomOnDevice = 415,
        NotDebuggable = 416,
        TypeInvalid = 417,
        DataNotAvailable = 418,
        BoxIsNotLocked = 420,
        KeyExchangeRequired = 421,
        DedicatedConnectionRequired = 422,
        InvalidArgument = 423,
        ProfileNotStarted = 424,
        ProfileAlreadyStarted = 425,
        D3DDebugCommandNotImplemented = 480,
        D3DInvalidSurface = 481,
        VxTaskPending = 496,
        VxTooManySessions = 497,
    };
    /// <summary>
    /// Receive wait type.
    /// </summary>
    public enum WaitType
    {
        /// <summary>
        /// Does not wait.
        /// </summary>
        None,

        /// <summary>
        /// Waits for data to start being received.
        /// </summary>
        Partial,

        /// <summary>
        /// Waits for data to start and then stop being received.
        /// </summary>
        Full,

        /// <summary>
        /// Waits for data to stop being received.
        /// </summary>
        Idle
    };
    public enum XboxFunctionType
    {
        NoPData = -1,
        SaveMillicode = 0,
        NoHandler = 1,
        RestoreMillicode = 2,
        Handler = 3
    }
    public enum XboxSwitch
    {
        True,
        False
    }
    public enum XboxExecutionState
    {
        Pending,
        Rebooting,
        Running,
        Stopped,
        TitlePending,
        TitleRebooting,
        Unknown,

    }
    /// <summary>
    /// Used to Get Version Information And Console Type
    /// </summary>
    public enum Info
    {
        HDD,
        Type,
        Platform,
        System,
        BaseKrnlVersion,
        KrnlVersion,
        XDKVersion,
    }
    public enum TRAY_STATE
    {
        Open,
        Unknown,
        Closed,
        Opening,
        Closing
    }
    public enum TrayState
    {
        Open,
        Close
    }
    public enum XboxDrives
    {
        HDD,
        HDD0,
        INTUSB,
        USB0,
        USB1,
        USB2,
        USB3,
        USB4,
        CdRom0,
        DVD,
        GAME,
        D,
        DASHUSER,
        media,
        SysCache0,
        SysCache1,
    }
    public enum XboxChars
    {
        a = 4,
        aa = 0x2d,
        b = 5,
        bb = 0x2e,
        c = 6,
        Caps = 0x39,
        cc = 0x2f,
        d = 7,
        dd = 0x30,
        Delete = 0x4c,
        e = 8,
        ee = 0x31,
        eight = 0x25,
        f = 9,
        ff = 0x33,
        five = 0x22,
        four = 0x21,
        g = 10,
        gg = 0x34,
        h = 11,
        hh = 0x35,
        i = 12,
        ii = 0x36,
        j = 13,
        jj = 0x37,
        k = 14,
        kk = 0x38,
        l = 15,
        m = 0x10,
        n = 0x11,
        nine = 0x26,
        o = 0x12,
        one = 30,
        p = 0x13,
        q = 20,
        r = 0x15,
        s = 0x16,
        seven = 0x24,
        six = 0x23,
        Space = 0x2c,
        t = 0x17,
        three = 0x20,
        two = 0x1f,
        u = 0x18,
        v = 0x19,
        w = 0x1a,
        x = 0x1b,
        y = 0x1c,
        z = 0x1d,
        zero = 0x27
    }
    public enum XboxColor
    {
        Black,
        Blue,
        BlueGray,
        White,
    };
    public enum XboxReboot
    {
        Cold = 2,
        Warm = 4,
    }
    /// <summary>
    /// Party System
    /// </summary>
    public enum Xbox_Party_Options
    {
        CreateParty = 0xafc,
        PartySettings = 0xb08,
        InviteOnly = 1,
        Kick = 0xb02,
        OpenParty = 0,
        JoinParty = 0xb01,
        AltJoinParty = 0xb1b,
        LeaveParty = 0xafd,
        InvitePlayer = 0xb15,

    }
    public enum XboxSignIn : int
    {
        QuickSignin = 700
    }
    public enum XboxAutomationButtonFlags : int
    {
        DPadUp = 1,//0x1
        DPadDown = 2,//0x2
        DPadLeft = 4,//0x4
        DPadRight = 8,//0x8
        StartButton = 16,//0x10
        BackButton = 32,//0x20
        LeftThumbButton = 64,//0x40
        RightThumbButton = 128,//0x80
        LeftShoulderButton = 256,//0x100
        RightShoulderButton = 512,//0x200
        Xbox360_Button = 1024,//0x400
        Bind_Button = 2048,//0x800
        A_Button = 4096,//0x1000
        B_Button = 8192,//0x2000
        X_Button = 16384,//0x4000
        Y_Button = 32768//0x8000 hex values
    }
    /// <summary>
    /// Guide Shortcuts
    /// </summary>
    public enum XboxShortcuts : int
    {
        //Main Shortcut
        Recent = 0x2C8,
        Guide_Button = 0x506,
        //End Of Main Shortcut

        //Games And Apps Tab
        Achievements = 0x2D0,
        Awards = 0x03C6,
        My_Games,
        Active_Downloads = 0x02E7,
        Redeem_Code,
        //End Of Games And Apps Tab

        //Main Guide
        XboxHome,
        Friends = 0x2BF,
        Party = 0x0305,
        Messages = 0x2C0,
        Beacons_And_Activiy = 0xB39,
        Private_Chat = 0x2C2,
        Open_Tray = 0x60,
        Close_Tray = 0x62,
        //End Of Main Guide

        //Media
        System_Video_Player = 2,
        System_Music_Player = 1,
        Picture_Viewer,
        Windows_Media_Center,
        Select_Music = 0,
        //End Of Media

        //settings
        DriveSelector = 5,
        Profile = 0x2c4,
        Preferences,
        Family_Settings,
        System_Settings,
        Account_Management = 4,
        Turn_Off_Console = 0x0295,
        AvatarEditor = 0xB3A
        //End Of settings

    };

    public enum XboxEventDeferFlags
    {
        CanDeferExecutionBreak = 1,
        CanDeferDebugString = 2,
        CanDeferSingleStep = 4,
        CanDeferAssertionFailed = 8,
        CanDeferAssertionFailedEx = 16,
        CanDeferDataBreak = 32,
        CanDeferRIP = 64
    }
    public enum XboxCreateDisposition
    {
        CreateNew = 1,
        CreateAlways = 2,
        OpenExisting = 3,
        OpenAlways = 4
    }
    public enum XboxConsoleType
    {
        DevelopmentKit,
        TestKit,
        ReviewerKit,
        NotConnected
    }
    public enum XboxBreakpointType
    {
        OnWrite,
        NoBreakpoint,
        OnRead,
        OnExecuteHW,
        OnExecute
    }
    public enum XboxDumpFlags
    {
        Normal = 0,
        WithDataSegs = 1,
        WithFullMemory = 2,
        WithHandleData = 4,
        FilterMemory = 8,
        ScanMemory = 16,
        WithUnloadedModules = 32,
        WithIndirectlyReferencedMemory = 64,
        FilterModulePaths = 128,
        WithProcessThreadData = 256,
        WithPrivateReadWriteMemory = 512
    }
    public enum XboxDebugEventType
    {
        NoEvent,
        ExecutionBreak,
        DebugString,
        ExecStateChange,
        SingleStep,
        ModuleLoad,
        ModuleUnload,
        ThreadCreate,
        ThreadDestroy,
        Exception,
        AssertionFailed,
        AssertionFailedEx,
        DataBreak,
        RIP,
        SectionLoad,
        SectionUnload,
        StackTrace,
        FiberCreate,
        FiberDestroy,
        BugCheck,
        PgoModuleStartup
    }
    public enum XboxStopOnFlags
    {
        OnThreadCreate = 1,
        OnFirstChanceException = 2,
        OnDebugString = 4,
        OnStackTrace = 8,
        OnModuleLoad = 16,
        OnTitleLaunch = 32,
        OnPgoModuleStartup = 64
    }

    public enum XboxAccessFlags
    {
        Read = 1,
        Write = 2,
        Control = 4,
        Configure = 8,
        Manage = 16
    }
    public enum XboxShareMode
    {
        ShareNone = 0,
        ShareRead = 1,
        ShareWrite = 2,
        ShareDelete = 4
    }
    public enum XboxSelectConsoleFlags
    {
        NoPromptIfDefaultExists,
        NoPromptIfOnlyOne,
        FilterByAccess
    }
    public enum XboxSectionInfoFlags
    {
        Loaded = 1,
        Readable = 2,
        Writeable = 4,
        Executable = 8,
        Uninitialized = 16
    }
    public enum XboxRegistersVector
    {
        v0,
        v1,
        v2,
        v3,
        v4,
        v5,
        v6,
        v7,
        v8,
        v9,
        v10,
        v11,
        v12,
        v13,
        v14,
        v15,
        v16,
        v17,
        v18,
        v19,
        v20,
        v21,
        v22,
        v23,
        v24,
        v25,
        v26,
        v27,
        v28,
        v29,
        v30,
        v31,
        v32,
        v33,
        v34,
        v35,
        v36,
        v37,
        v38,
        v39,
        v40,
        v41,
        v42,
        v43,
        v44,
        v45,
        v46,
        v47,
        v48,
        v49,
        v50,
        v51,
        v52,
        v53,
        v54,
        v55,
        v56,
        v57,
        v58,
        v59,
        v60,
        v61,
        v62,
        v63,
        v64,
        v65,
        v66,
        v67,
        v68,
        v69,
        v70,
        v71,
        v72,
        v73,
        v74,
        v75,
        v76,
        v77,
        v78,
        v79,
        v80,
        v81,
        v82,
        v83,
        v84,
        v85,
        v86,
        v87,
        v88,
        v89,
        v90,
        v91,
        v92,
        v93,
        v94,
        v95,
        v96,
        v97,
        v98,
        v99,
        v100,
        v101,
        v102,
        v103,
        v104,
        v105,
        v106,
        v107,
        v108,
        v109,
        v110,
        v111,
        v112,
        v113,
        v114,
        v115,
        v116,
        v117,
        v118,
        v119,
        v120,
        v121,
        v122,
        v123,
        v124,
        v125,
        v126,
        v127,
        vscr
    }
    public enum XboxRegisters64
    {
        ctr,
        r0,
        r1,
        r2,
        r3,
        r4,
        r5,
        r6,
        r7,
        r8,
        r9,
        r10,
        r11,
        r12,
        r13,
        r14,
        r15,
        r16,
        r17,
        r18,
        r19,
        r20,
        r21,
        r22,
        r23,
        r24,
        r25,
        r26,
        r27,
        r28,
        r29,
        r30,
        r31
    }

    public enum XboxMemoryRegionFlags
    {
        NoAccess = 1,
        ReadOnly = 2,
        ReadWrite = 4,
        WriteCopy = 8,
        Execute = 16,
        ExecuteRead = 32,
        ExecuteReadWrite = 64,
        ExecuteWriteCopy = 128,
        Guard = 256,
        NoCache = 512,
        WriteCombine = 1024,
        UserReadOnly = 4096,
        UserReadWrite = 8192
    }
    public enum XboxRebootFlags
    {
        Title = 0,
        Wait = 1,
        Cold = 2,
        Warm = 4,
        Stop = 8
    }
    public enum XboxModuleInfoFlags
    {
        Main = 1,
        Tls = 2,
        Dll = 4
    }
    public enum XboxRegisters32
    {
        msr,
        iar,
        lr,
        cr,
        xer
    }
    public enum XboxRegistersDouble
    {
        fp0,
        fp1,
        fp2,
        fp3,
        fp4,
        fp5,
        fp6,
        fp7,
        fp8,
        fp9,
        fp10,
        fp11,
        fp12,
        fp13,
        fp14,
        fp15,
        fp16,
        fp17,
        fp18,
        fp19,
        fp20,
        fp21,
        fp22,
        fp23,
        fp24,
        fp25,
        fp26,
        fp27,
        fp28,
        fp29,
        fp30,
        fp31,
        fpscr
    }
    public enum LEDState
    {
        OFF = 0,
        RED = 8,
        GREEN = 128,
        ORANGE = 136
    }

    public enum TemperatureFlag
    {
        CPU = 0,
        GPU = 1,
        EDRAM = 2,
        MotherBoard = 3
    }
    public enum ThreadType
    {
        System,
        Title
    }
    public enum XNotiyLogo : int
    {
        XBOX_LOGO = 0,
        NEW_MESSAGE_LOGO = 1,
        FRIEND_REQUEST_LOGO = 2,
        NEW_MESSAGE = 3,
        FLASHING_XBOX_LOGO = 4,
        GAMERTAG_SENT_YOU_A_MESSAGE = 5,
        GAMERTAG_SINGED_OUT = 6,
        GAMERTAG_SIGNEDIN = 7,
        GAMERTAG_SIGNED_INTO_XBOX_LIVE = 8,
        GAMERTAG_SIGNED_IN_OFFLINE = 9,
        GAMERTAG_WANTS_TO_CHAT = 10,
        DISCONNECTED_FROM_XBOX_LIVE = 11,
        DOWNLOAD = 12,
        FLASHING_MUSIC_SYMBOL = 13,
        FLASHING_HAPPY_FACE = 14,
        FLASHING_FROWNING_FACE = 15,
        FLASHING_DOUBLE_SIDED_HAMMER = 16,
        GAMERTAG_WANTS_TO_CHAT_2 = 17,
        PLEASE_REINSERT_MEMORY_UNIT = 18,
        PLEASE_RECONNECT_CONTROLLERM = 19,
        GAMERTAG_HAS_JOINED_CHAT = 20,
        GAMERTAG_HAS_LEFT_CHAT = 21,
        GAME_INVITE_SENT = 22,
        FLASH_LOGO = 23,
        PAGE_SENT_TO = 24,
        FOUR_2 = 25,
        FOUR_3 = 26,
        ACHIEVEMENT_UNLOCKED = 27,
        FOUR_9 = 28,
        GAMERTAG_WANTS_TO_TALK_IN_VIDEO_KINECT = 29,
        VIDEO_CHAT_INVITE_SENT = 30,
        READY_TO_PLAY = 31,
        CANT_DOWNLOAD_X = 32,
        DOWNLOAD_STOPPED_FOR_X = 33,
        FLASHING_XBOX_CONSOLE = 34,
        X_SENT_YOU_A_GAME_MESSAGE = 35,
        DEVICE_FULL = 36,
        FOUR_7 = 37,
        FLASHING_CHAT_ICON = 38,
        ACHIEVEMENTS_UNLOCKED = 39,
        X_HAS_SENT_YOU_A_NUDGE = 40,
        MESSENGER_DISCONNECTED = 41,
        BLANK = 42,
        CANT_SIGN_IN_MESSENGER = 43,
        MISSED_MESSENGER_CONVERSATION = 44,
        FAMILY_TIMER_X_TIME_REMAINING = 45,
        DISCONNECTED_XBOX_LIVE_11_MINUTES_REMAINING = 46,
        KINECT_HEALTH_EFFECTS = 47,
        FOUR_5 = 48,
        GAMERTAG_WANTS_YOU_TO_JOIN_AN_XBOX_LIVE_PARTY = 49,
        PARTY_INVITE_SENT = 50,
        GAME_INVITE_SENT_TO_XBOX_LIVE_PARTY = 51,
        KICKED_FROM_XBOX_LIVE_PARTY = 52,
        NULLED = 53,
        DISCONNECTED_XBOX_LIVE_PARTY = 54,
        DOWNLOADED = 55,
        CANT_CONNECT_XBL_PARTY = 56,
        GAMERTAG_HAS_JOINED_XBL_PARTY = 57,
        GAMERTAG_HAS_LEFT_XBL_PARTY = 58,
        GAMER_PICTURE_UNLOCKED = 59,
        AVATAR_AWARD_UNLOCKED = 60,
        JOINED_XBL_PARTY = 61,
        PLEASE_REINSERT_USB_STORAGE_DEVICE = 62,
        PLAYER_MUTED = 63,
        PLAYER_UNMUTED = 64,
        FLASHING_CHAT_SYMBOL = 65,
        UPDATING = 76,
    }
    public enum XboxLiveCountry
    {
        Unknown = 0,
        UnitedArabEmirates = 1,
        Albania = 2,
        Armenia = 3,
        Argentina = 4,
        Austria = 5,
        Australia = 6,
        Azerbaijan = 7,
        Belgium = 8,
        Bulgaria = 9,
        Bahrain = 10, // 0x0000000A
        BruneiDarussalam = 11, // 0x0000000B
        Bolivia = 12, // 0x0000000C
        Brazil = 13, // 0x0000000D
        Belarus = 14, // 0x0000000E
        Belize = 15, // 0x0000000F
        Canada = 16, // 0x00000010
        Switzerland = 18, // 0x00000012
        Chile = 19, // 0x00000013
        China = 20, // 0x00000014
        Colombia = 21, // 0x00000015
        CostaRica = 22, // 0x00000016
        CzechRepublic = 23, // 0x00000017
        Germany = 24, // 0x00000018
        Denmark = 25, // 0x00000019
        DominicanRepublic = 26, // 0x0000001A
        Algeria = 27, // 0x0000001B
        Ecuador = 28, // 0x0000001C
        Estonia = 29, // 0x0000001D
        Egypt = 30, // 0x0000001E
        Spain = 31, // 0x0000001F
        Finland = 32, // 0x00000020
        FaroeIslands = 33, // 0x00000021
        France = 34, // 0x00000022
        UnitedKingdom = 35, // 0x00000023
        Georgia = 36, // 0x00000024
        Greece = 37, // 0x00000025
        Guatemala = 38, // 0x00000026
        HongKong = 39, // 0x00000027
        Honduras = 40, // 0x00000028
        Croatia = 41, // 0x00000029
        Hungary = 42, // 0x0000002A
        Indonesia = 43, // 0x0000002B
        Ireland = 44, // 0x0000002C
        Israel = 45, // 0x0000002D
        India = 46, // 0x0000002E
        Iraq = 47, // 0x0000002F
        Iran = 48, // 0x00000030
        Iceland = 49, // 0x00000031
        Italy = 50, // 0x00000032
        Jamaica = 51, // 0x00000033
        Jordan = 52, // 0x00000034
        Japan = 53, // 0x00000035
        Kenya = 54, // 0x00000036
        Kyrgyzstan = 55, // 0x00000037
        Korea = 56, // 0x00000038
        Kuwait = 57, // 0x00000039
        Kazakhstan = 58, // 0x0000003A
        Lebanon = 59, // 0x0000003B
        Liechtenstein = 60, // 0x0000003C
        Lithuania = 61, // 0x0000003D
        Luxembourg = 62, // 0x0000003E
        Latvia = 63, // 0x0000003F
        LibyanArabJamahiriya = 64, // 0x00000040
        Morocco = 65, // 0x00000041
        Monaco = 66, // 0x00000042
        Macedonia = 67, // 0x00000043
        Mongolia = 68, // 0x00000044
        Macao = 69, // 0x00000045
        Maldives = 70, // 0x00000046
        Mexico = 71, // 0x00000047
        Malaysia = 72, // 0x00000048
        Nicaragua = 73, // 0x00000049
        Netherlands = 74, // 0x0000004A
        Norway = 75, // 0x0000004B
        NewZealand = 76, // 0x0000004C
        Oman = 77, // 0x0000004D
        Panama = 78, // 0x0000004E
        Peru = 79, // 0x0000004F
        Philippines = 80, // 0x00000050
        Pakistan = 81, // 0x00000051
        Poland = 82, // 0x00000052
        PuertoRico = 83, // 0x00000053
        Portugal = 84, // 0x00000054
        Paraguay = 85, // 0x00000055
        Qatar = 86, // 0x00000056
        Romania = 87, // 0x00000057
        RussianFederation = 88, // 0x00000058
        SaudiArabia = 89, // 0x00000059
        Sweden = 90, // 0x0000005A
        Singapore = 91, // 0x0000005B
        Slovenia = 92, // 0x0000005C
        Slovakia = 93, // 0x0000005D
        ElSalvador = 95, // 0x0000005F
        SyrianArabRepublic = 96, // 0x00000060
        Thailand = 97, // 0x00000061
        Tunisia = 98, // 0x00000062
        Turkey = 99, // 0x00000063
        TrinidadAndTobago = 100, // 0x00000064
        Taiwan = 101, // 0x00000065
        Ukraine = 102, // 0x00000066
        UnitedStates = 103, // 0x00000067
        Uruguay = 104, // 0x00000068
        Uzbekistan = 105, // 0x00000069
        Venezuela = 106, // 0x0000006A
        Vietnam = 107, // 0x0000006B
        Yemen = 108, // 0x0000006C
        SouthAfrica = 109, // 0x0000006D
        Zimbabwe = 110, // 0x0000006E
    }
}
