using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatEngine
{
    // error 
    public enum ErrorType
    {
        None,

        Fail_CoonectSocket,
        Fail_WaitSocket,
        Fail_ReceiveSocket,
        Fail_SendSocket,

        Fail_OpenDB,
        Fail_GetDB,
        Fail_WriteDB,

        Duplicate_Id,
        Invalid_Id,
        Invalid_Password,
        Duplicate_login,
        Duplicate_logout,
        Unknown_User,

        Duplicate_RoomId,
        Invalid_RoomId,
        Invalid_ChargeId,
        Invalid_GameId,
        Invalid_GameSource,

        Notenough_NewID,
        Notenough_Cash,
        Notenough_Point,
        Notallow_Chat,
        Already_Chat,
        Already_Serviceman,
        Notallow_NoServiceman,
        Notallow_Previlege,

        Live_Room,
        Full_Gamer,

        Exception_Occur,
        Notrespond_Server
    }

    // notify
    public enum NotifyType
    {
        // notify
        Notify_Socket,
        Notify_Duplicate,
        Notify_Ping,
        Notify_GameTimer,

        // from client
        Request_VideoPort,

        Request_Home,
        Request_UserDetail,
        Request_PartnerDetail,
        Request_NewID,
        Request_IconUpload,
        Request_VideoUpload,
        Request_IconRemove,
        Request_UpdateUser,
        Request_UpdatePercent,
        
        Request_Login,
        Request_Phone_Login,
        Request_Logout,
        Request_UserList,
        Request_RoomList,
        Request_Message,
        Request_Evaluation,

        Request_EnterMeeting,
        Request_OutMeeting,
        Request_MakeRoom,
        Request_UpdateRoom,
        Request_DelRoom,
        Request_EnterRoom,
        Request_OutRoom,
        Request_RoomDetail,
        Request_StringChat,
        Request_VoiceChat,
        Request_VideoChat,
        Request_Give,
        
        Request_GameList,
        Request_EnterGame,
        Request_OutGame,
        Request_GameDetail,

        Request_TableList,

        Request_ChargeSiteUrl,
        Request_ApplyBanker,
        Request_CancelBanker,

        Request_PlayerEnter,
        Request_PlayerOut,
        Request_Betting,
        Request_AddScore,
        Request_GiveUp,
        Request_Ready,
        Request_OpenCard,
        
        Request_SendLetter,
        Request_DelLetter,
        Request_ReadNotice,
        Request_DelNotice,
        Request_MusiceInfo,
        Request_MusiceStateInfo,
        Request_ClassInfo,
        Request_ClassTypeInfo,
        Request_ClassPictureDetail,
        Request_RoomInfo,
        Request_RoomPrice,

        // 2013-12-17: GreenRose
        Request_PaymentInfo,

        // from server
        Reply_Error,
        Reply_VideoPort,

        Reply_Home,
        Reply_UserDetail,
        Reply_PartnerDetail,
        Reply_NewID,
        Reply_IconUpload,
        Reply_VideoUpload,
        Reply_IconRemove,
        Reply_UpdateUser,
        Reply_UserInfo,
        Reply_UpdatePercent,

        Reply_Login,
        Reply_Logout,
        Reply_UserList,
        Reply_RoomList,
        Reply_Message,
        Reply_Evaluation,

        Reply_EnterMeeting,
        Reply_OutMeeting,
        Reply_MakeRoom,
        Reply_UpdateRoom,
        Reply_DelRoom,
        Reply_EnterRoom,
        Reply_OutRoom,
        Reply_RoomDetail,
        Reply_StringChat,
        Reply_VoiceChat,
        Reply_VideoChat,
        Reply_Give,

        Reply_GameList,
        Reply_EnterGame,
        Reply_OutGame,
        Reply_GameDetail,

        Reply_ChargeSiteUrl,
        Reply_ChangeBanker,
        
        Reply_ViewerEnter,
        Reply_PlayerEnter,
        Reply_PlayerOut,
        Reply_TableDetail,
        Reply_Betting,
        Reply_AddScore,
        Reply_GiveUp,
        Reply_SendCard,
        Reply_OpenCard,

        Reply_LetterList,
        Reply_NoticeList,
        Reply_MusiceInfo,
        Reply_MusiceStateInfo,
        Reply_ClassInfo,
        Reply_ClassTypeInfo,
        Reply_ClassPictureDetail,
        Reply_RoomInfo,
        Reply_RoomPrice,

        // 2013-12-17: GreenRose
        Reply_PaymentInfo,

        // added by usc at 2014/01/08
        Request_GameChat,
        Reply_GameChat,

        // 2014-01-10: GreenRose
        Request_Reconnect,

        Request_FishSend,
        Reply_FishSend,

        // 2014-01-21: GreenRose
        Request_Download_GameFile,
        Reply_Download_GameFile,

        // 2014-02-03: GreenRose
        Request_UserState,
        Reply_UserState,        
        
        // 2014-02-13: GreenRose
        Reply_AdminNotice,
        Request_AdminNotices,

        // added by usc at 2014/02/15
        Request_GameResult,
        Reply_GameResult,

        Request_Server,
        Reply_Server,

        Notify_OldServer,


        // 2014-02-24: GreenRose
        Request_SendIP,
        Reply_SendIP,

        Request_VideoInfo,
        Reply_VideoInfo
    }

    // info
    public enum InfoType
    {
        None,
        Header,
        Result,
        Home,
        User,
        UserList,
        UserDetail,
        Room,
        RoomList,
        RoomDetail,
        String,
        Voice,
        Video,
        Present,
        Give,
        Game,
        GameList,
        GameDetail,
        ChatHistory,
        ChatHistoryList,
        ChargeHistory,
        EvalHistory,
        PointHistory,
        PresentHistory,
        GameHistory,
        AskChat,
        Board,
        Paper,
        Betting,
        Table,
        Sicbo,
        Dice, 
        DzCard,
        Horse,
        BumperCar,
        Fish,
        AddScore,
        SendCard,
        Musice,
        MusiceState,
        ClassInfo,
        ClassList,
        ClassTypeInfo,
        ClassTypeList,
        ClassPictureDetail,
        RoomValue,
        Payment,
        FishSend,
        DWGameFile,
        UserState,
        Server,
        AVMsg,
    }

    class Constant
    {
    }
}
