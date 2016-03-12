using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateServer
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

        Exception_Occur
    }

    // notify
    public enum NotifyType
    {
        Notify_Socket,
        Request_UpdateCheck,
        Reply_UpdateFile,
        Reply_UpdateEnd,
        Request_GiveMeFile,
        Notify_notify
    }

    // info
    public enum InfoType
    {
        None,
        Header,
        UpdateCheck,
        UpdateFile
    }

    class Constant
    {
    }
}
