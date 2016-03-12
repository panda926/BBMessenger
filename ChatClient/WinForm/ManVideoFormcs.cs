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
using System.Runtime.InteropServices;

using System.Drawing.Imaging;

namespace ChatClient
{
    public partial class ManVideoFormcs : FormEx
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

        private string currentUserID;
        private string friendID;

        ChatClient.Main m_mainWnd = null;

        public ManVideoFormcs(string currentID, string _friendID, string friendName, bool waitingAnswer, ChatClient.Main _mainWnd)
        {
            InitializeComponent();            
            Point maximizedLocation = new Point((SystemInformation.VirtualScreen.Width - this.MaximumSize.Width) / 2, (SystemInformation.VirtualScreen.Height - this.MaximumSize.Height) / 2);
            this.MaximizedBounds = new Rectangle(maximizedLocation, this.MaximumSize);

            this.m_mainWnd = _mainWnd;

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

            if (ChatClient.Login._UserInfo.Kind == (int)ChatEngine.UserKind.ServiceWoman)       // 여자인경우 자기카메라만 현시한다.
            {
                //this.qqGlassButton2.Image = MakeGrayscale3((Bitmap)(resources.GetObject("qqGlassButton2.Image")));
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

                SetCaptureWindow(this.panel1.Handle);

                nResult = StartSend();
                if (nResult == 0)
                {
                    IntPtr result = GetLastErrorString();
                    string strError = Marshal.PtrToStringAnsi(result);

                    ErrorCollection.GetInstance().SetErrorInfo(strError);
                }

                //this.toolStripLabel_msg.Visible = false;
                //this.toolStripLabel_netstate.Visible = true;
                //this.toolStripButton_hideMyself.Visible = false;
                //this.toolStripButton_camera.Visible = true;
                //this.toolStripButton_microphone.Visible = false;
                //this.toolStripButton_speaker.Visible = false;
                //this.toolStripSeparator1.Visible = false;

                ChatClient.Login._UserInfo.nUserState = 21;
                ChatClient.Login._ClientEngine.Send(ChatEngine.NotifyType.Request_UserState, ChatClient.Login._UserInfo);

            }
            else
            {
                this.qqGlassButton1.Enabled = false;
                this.qqGlassButton2.Enabled = false;
                StringBuilder strFriendID = new StringBuilder(this.friendID);
                int nResult = StartReceive(strFriendID, this.panel1.Handle);
                if (nResult == 0)
                {
                    IntPtr result = GetLastErrorString();
                    string strError = Marshal.PtrToStringAnsi(result);

                    ErrorCollection.GetInstance().SetErrorInfo(strError);
                }

                //this.toolStripLabel_msg.Visible = false;
                //this.toolStripLabel_netstate.Visible = true;
                //this.toolStripButton_hideMyself.Visible = false;
                //this.toolStripButton_camera.Visible = false;
                //this.toolStripButton_microphone.Visible = false;
                //this.toolStripButton_speaker.Visible = false;
                //this.toolStripSeparator1.Visible = false;

                //timer1.Start();
            }
        }

        //#region Override

        //protected override CreateParams CreateParams
        //{
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


        private void SetStyles()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        //#endregion

        private void ManVideoFormcs_Load(object sender, EventArgs e)
        {
            if (m_mainWnd != null)
            {
                int nScreenWidth = SystemInformation.VirtualScreen.Width;
                int nScreenHeight = SystemInformation.VirtualScreen.Height;
                if (m_mainWnd.Left + m_mainWnd.Width + this.Width > nScreenWidth)
                {
                    this.Left = nScreenWidth - this.Width;
                }
                else
                    this.Left = (int)(m_mainWnd.Left + m_mainWnd.Width + 5);

                this.Top = (int)m_mainWnd.Top;
            }

            if (ChatClient.Login._UserInfo.Kind == (int)ChatEngine.UserKind.ServiceWoman)
            {
                System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/Resources;component/image/video.png", UriKind.RelativeOrAbsolute);
                bi.EndInit();

                m_mainWnd.imgCamOrNotify.Source = bi;
            }
        }

        private void ManVideoFormcs_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.timer1.Stop();

            if (ChatClient.Login._UserInfo.Kind == (int)ChatEngine.UserKind.ServiceWoman)
            {
                StopSend();
                StopAudioCapture();
                StopVideoCapture();
                m_mainWnd.soloVideoForm = null;
                ChatClient.Login._UserInfo.nUserState = 22;
                ChatClient.Login._ClientEngine.Send(ChatEngine.NotifyType.Request_UserState, ChatClient.Login._UserInfo);

                System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/Resources;component/image/novideo.png", UriKind.RelativeOrAbsolute);
                bi.EndInit();

                m_mainWnd.imgCamOrNotify.Source = bi;
            }
            else
            {
                StringBuilder strFriendID = new StringBuilder(this.friendID);
                StopReceive(strFriendID);
            }
        }

        private void ManVideoFormcs_SizeChanged(object sender, EventArgs e)
        {
            if (ChatClient.Login._UserInfo.Kind == (int)ChatEngine.UserKind.ServiceWoman)       // 여자인경우 자기카메라만 현시한다.
            {
                SetCaptureWindow(this.panel1.Handle);                
            }
            else
            {
                StringBuilder strFriendID = new StringBuilder(this.friendID);
                StartReceive(strFriendID, this.panel1.Handle);
            }

            if(this.WindowState == FormWindowState.Maximized)
            {                               
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

        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManVideoFormcs));
        bool bWebCamSending = true;
        private void qqGlassButton1_Click(object sender, EventArgs e)
        {
            if (bWebCamSending)
            {
                bWebCamSending = false;
                this.qqGlassButton1.Image = MakeGrayscale3((Bitmap)(resources.GetObject("qqGlassButton1.Image")));
                StopVideoCapture();
            }
            else
            {
                bWebCamSending = true;
                this.qqGlassButton1.Image = ((System.Drawing.Image)(resources.GetObject("qqGlassButton1.Image")));
                StartVideoCapture(ChatClient.Main._nCameraIndex);
            }
        }

        bool bAudioSending = true;
        private void qqGlassButton2_Click(object sender, EventArgs e)
        {
            if (bAudioSending)
            {
                this.qqGlassButton2.Image = MakeGrayscale3((Bitmap)(resources.GetObject("qqGlassButton2.Image")));
                bAudioSending = false;
                StopAudioCapture();             
            }
            else
            {                
                bAudioSending = true;                
                this.qqGlassButton2.Image = ((System.Drawing.Image)(resources.GetObject("qqGlassButton2.Image")));
                StartAudioCapture();
            }
        }
    }
}
