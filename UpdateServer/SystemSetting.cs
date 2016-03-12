using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ANYCHATAPI;

namespace UpdateServer
{
    public delegate void TextReceivedHandler(int fromUID, int toUID, string Text, bool isserect);
    public delegate void TransBufferReceivedHandler(int userId, IntPtr buf, int len, int userValue);
    public delegate void TransFileReceivedHandler(int userId, string fileName,string filePath, int fileLength, int wParam, int lParam,int taskId, int userValue);
    

    public class SystemSetting
    {
        /// <summary>
        /// ��ʼ��AnyChat sdk
        /// ע�� callback
        /// </summary>
        public static void Init(IntPtr hWnd)
        {
            AnyChatCoreSDK.ActiveCallLog(true);
            ulong dwFuncMode = AnyChatCoreSDK.BRAC_FUNC_VIDEO_AUTODISP
                | AnyChatCoreSDK.BRAC_FUNC_AUDIO_AUTOPLAY
                | AnyChatCoreSDK.BRAC_FUNC_CHKDEPENDMODULE
                | AnyChatCoreSDK.BRAC_FUNC_AUDIO_VOLUMECALC
                | AnyChatCoreSDK.BRAC_FUNC_NET_SUPPORTUPNP
                | AnyChatCoreSDK.BRAC_FUNC_FIREWALL_OPEN
                | AnyChatCoreSDK.BRAC_FUNC_AUDIO_AUTOVOLUME
                | AnyChatCoreSDK.BRAC_FUNC_AUDIO_VOLUMECALC
                | AnyChatCoreSDK.BRAC_FUNC_AUDIO_CBDATA
                | AnyChatCoreSDK.BRAC_FUNC_CONFIG_LOCALINI;

            ///��ʼ��
            AnyChatCoreSDK.InitSDK(hWnd, dwFuncMode);

            ///ע��ص�
            ///��Ƶ
            AnyChatCoreSDK.SetVideoDataCallBack(AnyChatCoreSDK.PixelFormat.BRAC_PIX_FMT_RGB24,
                video_Callback, hWnd.ToInt32());
            //����
            AnyChatCoreSDK.SetAudioDataCallBack(audio_Callback, hWnd.ToInt32());
            //����
            AnyChatCoreSDK.SetTextMessageCallBack(text_Callback, hWnd.ToInt32());
            //͸��ͨ��
            AnyChatCoreSDK.SetTransBufferCallBack(transBuff_Callback, hWnd.ToInt32());
            //p2p�ļ�����
            AnyChatCoreSDK.SetTransFileCallBack(transFile_callback, hWnd.ToInt32());
            ///������������ݻص�
            AnyChatCoreSDK.SetSDKFilterDataCallBack(filterData_callback, hWnd.ToInt32());
            ///�ṩ����������֤
            AnyChatCoreSDK.SetServerAuthPass(new StringBuilder(""));

        }


        //�˳��ͷ���Դ
        public static void Release(int myroomID)
        {
            AnyChatCoreSDK.LeaveRoom(myroomID);
            AnyChatCoreSDK.Logout();
            AnyChatCoreSDK.Release();
        }

        static AnyChatCoreSDK.SDKFilterDataCallBack filterData_callback =
            new AnyChatCoreSDK.SDKFilterDataCallBack(SDKFilter_DataCallBack);

        static AnyChatCoreSDK.TransBufferCallBack transBuff_Callback =
            new AnyChatCoreSDK.TransBufferCallBack(TransBuffer_CallBack);

        static AnyChatCoreSDK.VideoData_CallBack video_Callback = new
            AnyChatCoreSDK.VideoData_CallBack(VideoData_CallBack);

        static AnyChatCoreSDK.AudioData_CallBack audio_Callback = new
            AnyChatCoreSDK.AudioData_CallBack(AudioData_CallBack);

        static AnyChatCoreSDK.TextMessage_CallBack text_Callback = new
            AnyChatCoreSDK.TextMessage_CallBack(TextMessage_CallBack);

        static AnyChatCoreSDK.TransFileCallBack transFile_callback = new
            AnyChatCoreSDK.TransFileCallBack(TransFile_CallBack);

        /// <summary>
        /// ����������Ϣ�ص�
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="len"></param>
        /// <param name="userValue"></param>
        private static void SDKFilter_DataCallBack(IntPtr buf, int len, int userValue)
        {


        }

        public static TransFileReceivedHandler TransFile_OnReceive = null;
        /// <summary>
        /// �ļ��ص�
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileLength"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="taskId"></param>
        /// <param name="userValue"></param>
        private static void TransFile_CallBack(int userId, string fileName,
            string filePath, int fileLength, int wParam, int lParam,
            int taskId, int userValue)
        {
            if (TransFile_OnReceive != null)
                TransFile_OnReceive(userId, fileName,filePath, fileLength,wParam, lParam,taskId, userValue);
        }

        public static TransBufferReceivedHandler TransBuffer_OnReceive = null;
        /// <summary>
        /// ͸��ͨ���ص�
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="buf"></param>
        /// <param name="len"></param>
        /// <param name="userValue"></param>
        private static void TransBuffer_CallBack(int userId, IntPtr buf, int len, int userValue)
        {
            if (TransBuffer_OnReceive != null)
                TransBuffer_OnReceive(userId, buf, len, userValue);
        }

        public static TextReceivedHandler Text_OnReceive = null;
        /// <summary>
        /// ��Ϣ�ص�
        /// </summary>
        /// <param name="fromuserId"></param>
        /// <param name="touserId"></param>
        /// <param name="isserect"></param>
        /// <param name="message"></param>
        /// <param name="len"></param>
        /// <param name="userValue"></param>
        private static void TextMessage_CallBack(int fromuserId, int touserId, bool isserect,
            string message, int len, int userValue)
        {
            if (Text_OnReceive != null)
                Text_OnReceive(fromuserId, touserId, message, isserect);
        }

        /// <summary>
        /// ��Ƶ�ص�
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="buf"></param>
        /// <param name="len"></param>
        /// <param name="format"></param>
        /// <param name="userValue"></param>
        private static void AudioData_CallBack(int userId, IntPtr buf, int len,
            AnyChatCoreSDK.WaveFormat format, int userValue)
        {

        }

        /// <summary>
        /// ��Ƶ�ص�
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="buf"></param>
        /// <param name="len"></param>
        /// <param name="bitMap"></param>
        /// <param name="userValue"></param>
        private static void VideoData_CallBack(int userId, IntPtr buf, int len,
            AnyChatCoreSDK.BITMAPINFOHEADER bitMap, int userValue)
        {

        }
    }
}
