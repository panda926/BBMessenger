using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.Win32;
using CCWin.Win32.Const;
using System.Diagnostics;

using System.Configuration;

using System.Runtime.InteropServices;

namespace GG
{
    public partial class VideoForm : CCSkinMain
    {
        // 2014-05-01: GreenRose
        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int Init(StringBuilder pUserID, StringBuilder pServerAddress);

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int Destroy();


        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int GetCameraCount();

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        extern public static IntPtr GetCameraName(int index);


        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StartVideoCapture(int index);

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StartAudioCapture();

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StopVideoCapture();

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StopAudioCapture();

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int SetCaptureWindow(IntPtr hWnd);


        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StartSend();

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StopSend();


        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StartReceive(StringBuilder pUserID, IntPtr hWnd);

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StopReceive(StringBuilder pUserID);    

        /// <summary>
        /// 当自己挂断视频时，触发此事件。
        /// </summary>

        private string currentUserID;
        private string friendID;

        ChatClient.NewChatRoom _newChatRoom = null;

        public VideoForm(string currentID, string _friendID, string friendName, bool waitingAnswer, ChatClient.NewChatRoom _newChatRoom)
        {
            InitializeComponent();

            this._newChatRoom = _newChatRoom;
            
            this.currentUserID = currentID;
            this.friendID = _friendID;
            this.Text = string.Format("正在和{0}视频会话" ,friendName);

            if (!waitingAnswer) //同意视频，开始连接
            {                
                this.OnAgree();
            }
        }
        

        /// <summary>
        /// 对方同意视频会话
        /// </summary>
        public void OnAgree()
        {
            this.toolStripLabel_msg.Text = "正在连接 . . .";

            StartVideoCapture(ChatClient.Main._nCameraIndex);
            SetCaptureWindow(this.skinPanel_Myself.Handle);
            StartSend();

            StringBuilder strFriendID = new StringBuilder(this.friendID);
            StartReceive(strFriendID, this.skinPanel1.Handle);

            this.toolStripLabel_msg.Visible = false;
            this.toolStripLabel_netstate.Visible = true;
            this.toolStripButton_hideMyself.Visible = true;
            this.toolStripButton_camera.Visible = true;
            this.toolStripButton_microphone.Visible = true;
            this.toolStripButton_speaker.Visible = true;
            this.toolStripSeparator1.Visible = true;
            this.timer1.Start();
        }

        /// <summary>
        /// 对方挂断视频
        /// </summary>
        public void OnHungUpVideo()
        {
            this.Close();
        }


        private int totalSeconds = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            ++this.totalSeconds;
            this.ShowVideoTime();
        }

        //显示已经会话的时间
        private void ShowVideoTime()
        {
            int min = this.totalSeconds / 60;
            int sec = this.totalSeconds % 60;
            this.toolStripLabel_time.Text = string.Format("{0}:{1}  " ,min.ToString("00") ,sec.ToString("00"));
        }

        //自己挂断视频
        private void toolStripButton1_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void VideoForm_FormClosing(object sender, FormClosingEventArgs e)
        {         
            this.timer1.Stop();

            StopSend();
            StringBuilder strFriendID = new StringBuilder(this.friendID);
            StopReceive(strFriendID);
            StopAudioCapture();
            StopVideoCapture();

            if (ChatClient.Login._UserInfo.Kind == (int)ChatEngine.UserKind.ServiceWoman)
            {
                ChatClient.Login._UserInfo.nUserState = 0;
                ChatClient.Login._ClientEngine.Send(ChatEngine.NotifyType.Request_UserState, ChatClient.Login._UserInfo);
            }

            _newChatRoom.SendMessage(_newChatRoom.m_strVideoChatEnd);
            _newChatRoom.videoForm = null;
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            this.skinPanel_Myself.Visible = !this.skinPanel_Myself.Visible;
            this.toolStripButton_hideMyself.Text =this.skinPanel_Myself.Visible ? "隐藏小窗口" :"显示小窗口";
        }

        bool bOutputVideo = true;
        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {
            if (bOutputVideo)
            {
                StopSend();
                this.toolStripButton_camera.Text = "状态：不输出视频";
                this.toolStripButton_camera.Image = this.imageList1.Images[1];
                bOutputVideo = false;
            }
            else
            {
                StartSend();
                this.toolStripButton_camera.Text = "状态：正常输出视频";
                this.toolStripButton_camera.Image = this.imageList1.Images[0];
                bOutputVideo = true;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //this.microphoneConnector1.Mute = !this.microphoneConnector1.Mute;
            //this.toolStripButton_speaker.Text = this.microphoneConnector1.Mute ? "状态：静音" : "状态：正常";
            //this.toolStripButton_speaker.Image = this.microphoneConnector1.Mute ? this.imageList1.Images[3] : this.imageList1.Images[2]; 
        }

        private void toolStripButton_microphone_Click(object sender, EventArgs e)
        {
            //this.multimediaManager.OutputAudio = !this.multimediaManager.OutputAudio;
            //this.toolStripButton_microphone.Text = this.multimediaManager.OutputAudio ? "状态：正常输出音频" : "状态：不输出音频";
            //this.toolStripButton_microphone.Image = this.multimediaManager.OutputAudio ? this.imageList1.Images[4] : this.imageList1.Images[5]; 
        }

        private void VideoForm_Load(object sender, EventArgs e)
        {
            if (_newChatRoom != null)
            {
                this.Left = (int)(_newChatRoom.Left + _newChatRoom.Width + 5);
                this.Top = (int)_newChatRoom.Top;
            }
        }
    }
}
