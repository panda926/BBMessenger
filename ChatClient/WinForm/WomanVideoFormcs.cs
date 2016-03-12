using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using ControlExs;

using System.Net;
using System.Net.Sockets;

using ChatEngine;

using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace ChatClient
{
    public partial class WomanVideoFormcs : FormEx
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

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        extern public static IntPtr GetLastErrorString();


        public NewChatRoom _newChatRoom = null;

        private string currentUserID;
        private string friendID;

        public WomanVideoFormcs(string currentID, string _friendID, string friendName, bool waitingAnswer, ChatClient.NewChatRoom _newChatRoom)
        {
            InitializeComponent();
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            Point maximizedLocation = new Point((screen.Width - 336) / 2, (screen.Height - 305) / 2);
            this.MaximizedBounds = new Rectangle(maximizedLocation, this.MaximumSize);

            this._newChatRoom = _newChatRoom;

            this.currentUserID = currentID;
            this.friendID = _friendID;
            this.Text = string.Format("正在和{0}视频会话", friendName);

            if (!waitingAnswer) //同意视频，开始连接
            {
                this.OnAgree();
            }
        }

        public void OnAgree()
        {
            //this.toolStripLabel_msg.Text = "正在连接 . . .";

            //this.qqGlassButton3.Image = MakeGrayscale3((Bitmap)(resources.GetObject("qqGlassButton3.Image")));
            //bAudioSending = false;
            int nResult = StartVideoCapture(ChatClient.Main._nCameraIndex);
            if (nResult == 0)
            {
                IntPtr result = GetLastErrorString();
                string strError = Marshal.PtrToStringAnsi(result);

                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }

            nResult = StartAudioCapture();
            if (nResult == 0)
            {
                IntPtr result = GetLastErrorString();
                string strError = Marshal.PtrToStringAnsi(result);

                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }

            SetCaptureWindow(this.panelLocal.Handle);

            nResult = StartSend();
            if (nResult == 0)
            {
                IntPtr result = GetLastErrorString();
                string strError = Marshal.PtrToStringAnsi(result);

                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }

            StringBuilder strFriendID = new StringBuilder(this.friendID);

            nResult = StartReceive(strFriendID, this.panelRemote.Handle);
            if (nResult == 0)
            {
                IntPtr result = GetLastErrorString();
                string strError = Marshal.PtrToStringAnsi(result);

                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }

            //this.toolStripLabel_msg.Visible = false;
            //this.toolStripLabel_netstate.Visible = true;
            //this.toolStripButton_hideMyself.Visible = true;
            //this.toolStripButton_camera.Visible = true;
            //this.toolStripButton_microphone.Visible = true;
            //this.toolStripButton_speaker.Visible = true;
            //this.toolStripSeparator1.Visible = true;
            //this.timer1.Start();
        }

        //#region Override

        //protected override CreateParams CreateParams
        //{E:\Working_NewTFS\HBMessenger\ChatClient\app.manifest
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        if (!DesignMode)
        //        {
        //            cp.ExStyle |= (int)WindowStyle.WS_CLIPCHILDREN;
        //        }
        //        return cp;
        //    }
        //}

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    DrawFromAlphaMainPart(this, e.Graphics);
        //}

        //#endregion

        //#region Private

        ///// <summary>
        ///// 绘制窗体主体部分白色透明层
        ///// </summary>
        ///// <param name="form"></param>
        ///// <param name="g"></param>
        //public static void DrawFromAlphaMainPart(Form form, Graphics g)
        //{
        //    Color[] colors = 
        //    {
        //        Color.FromArgb(5, Color.White),
        //        Color.FromArgb(30, Color.White),
        //        Color.FromArgb(145, Color.White),
        //        Color.FromArgb(150, Color.White),
        //        Color.FromArgb(30, Color.White),
        //        Color.FromArgb(5, Color.White)
        //    };

        //    float[] pos = 
        //    {
        //        0.0f,
        //        0.04f,
        //        0.10f,
        //        0.90f,
        //        0.97f,
        //        1.0f      
        //    };

        //    ColorBlend colorBlend = new ColorBlend(6);
        //    colorBlend.Colors = colors;
        //    colorBlend.Positions = pos;

        //    RectangleF destRect = new RectangleF(0, 0, form.Width, form.Height);
        //    using (LinearGradientBrush lBrush = new LinearGradientBrush(destRect, colors[0], colors[5], LinearGradientMode.Vertical))
        //    {
        //        lBrush.InterpolationColors = colorBlend;
        //        g.FillRectangle(lBrush, destRect);
        //    }
        //}


        //private void SetStyles()
        //{
        //    SetStyle(ControlStyles.UserPaint, true);
        //    SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        //    SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        //    SetStyle(ControlStyles.ResizeRedraw, true);
        //    SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        //    UpdateStyles();
        //}

        //#endregion

       
        private void WomanVideoFormcs_Load(object sender, EventArgs e)
        {
            if (_newChatRoom != null)
            {
                int nScreenWidth = SystemInformation.VirtualScreen.Width;
                int nScreenHeight = SystemInformation.VirtualScreen.Height;
                if (_newChatRoom.Left + _newChatRoom.Width + this.Width > nScreenWidth)
                {
                    this.Left = nScreenWidth - this.Width;
                }
                else
                    this.Left = (int)(_newChatRoom.Left + _newChatRoom.Width + 5);

                this.Top = (int)_newChatRoom.Top;
            }
        }
        
        private void WomanVideoFormcs_FormClosing(object sender, FormClosingEventArgs e)
        {
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

        bool bLocalVisible = true;
        private void qqGlassButton2_Click(object sender, EventArgs e)
        {
            if (bLocalVisible)
            {
                this.qqGlassButton2.Image = MakeGrayscale3((Bitmap)(resources.GetObject("qqGlassButton2.Image")));
                this.MaximizeBox = true;
                this.MaximumSize = new System.Drawing.Size(336, 305);       
                this.ClientSize = new System.Drawing.Size(256, 245);
                this.panelLocal.Size = new System.Drawing.Size(0, 0);
                this.panelRemote.Size = new System.Drawing.Size(240, 180);                
                StringBuilder strFriendID = new StringBuilder(friendID);                
                StartReceive(strFriendID, this.panelRemote.Handle);
                bLocalVisible = false;
                                         
            }
            else
            {
                this.qqGlassButton2.Image = ((System.Drawing.Image)(resources.GetObject("qqGlassButton2.Image")));
                this.MaximumSize = new System.Drawing.Size(800, 600);
                this.MaximizeBox = false;
                this.ClientSize = new System.Drawing.Size(256, 425);
                this.panelLocal.Size = new System.Drawing.Size(240, 180);
                this.panelRemote.Size = new System.Drawing.Size(240, 180);
                StringBuilder strFriendID = new StringBuilder(friendID);
                StartReceive(strFriendID, this.panelRemote.Handle);                
                bLocalVisible = true;                
            }
        }

        private void WomanVideoFormcs_SizeChanged(object sender, EventArgs e)
        {
            StringBuilder strFriendID = new StringBuilder(this.friendID);
            StartReceive(strFriendID, this.panelRemote.Handle);

            if (this.Size == this.MaximumSize)
            {
                this.panelLocal.Visible = false;
                this.qqGlassButton2.Enabled = false;                               
            }
            else
            {
                this.panelLocal.Visible = true;
                this.qqGlassButton2.Enabled = true;
            }
        }

        // 자기의 웹캠영상을 상대에게 보여주지 않기.
        bool bWebCamSending = true;
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WomanVideoFormcs));
        private void qqGlassButton1_Click(object sender, EventArgs e)
        {
            if (bWebCamSending)
            {
                bWebCamSending = false;
                this.qqGlassButton1.Image = MakeGrayscale3((Bitmap)(resources.GetObject("qqGlassButton1.Image")));
                StopVideoCapture();

                System.Threading.Thread.Sleep(1000);
                _newChatRoom.SendMessage(_newChatRoom.m_strVideoSendingStop);
            }
            else
            {
                bWebCamSending = true;
                this.qqGlassButton1.Image = ((System.Drawing.Image)(resources.GetObject("qqGlassButton1.Image")));
                StartVideoCapture(Main._nCameraIndex);
            }
        }

        // 자기의 음성을 상대에게 보내지 않기.
        bool bAudioSending = true;
        private void qqGlassButton3_Click(object sender, EventArgs e)
        {
            if (bAudioSending)
            {
                this.qqGlassButton3.Image = MakeGrayscale3((Bitmap)(resources.GetObject("qqGlassButton3.Image")));
                bAudioSending = false;
                StopAudioCapture();
            }
            else
            {
                //this.qqGlassButton3.Image = this.imageList1.Images[4];
                bAudioSending = true;                
                this.qqGlassButton3.Image = ((System.Drawing.Image)(resources.GetObject("qqGlassButton3.Image")));
                StartAudioCapture();
            }
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        public void ChangeRemotePanelBackground()
        {
            this.panelRemote.BackgroundImage = ChatClient.Properties.Resources.videowindow;
        }
    }
}
