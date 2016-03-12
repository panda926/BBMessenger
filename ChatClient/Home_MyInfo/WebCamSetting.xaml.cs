using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Runtime.InteropServices;

namespace ChatClient.Home_MyInfo
{
    /// <summary>
    /// Interaction logic for WebCamSetting.xaml
    /// </summary>
    public partial class WebCamSetting : UserControl
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

        public WebCamSetting()
        {
            InitializeComponent();

            CloseAllVideoWindow();

            Destroy();
            StringBuilder strID = new StringBuilder(Login._UserInfo.Id);
            StringBuilder strServerIP = new StringBuilder(Login._ServerServiceUri);

            int nResult = Init(strID, strServerIP);
            if (nResult == 0)
            {
                IntPtr result = GetLastErrorString();
                string strError = Marshal.PtrToStringAnsi(result);

                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        // 2014-04-04: GreenRose
        private void InitCameraControl()
        {
            try
            {
                int cameraCount = GetCameraCount();
                cmbCamera.Items.Clear();

                for (int i = 0; i < cameraCount; i++)
                {
                    IntPtr a = GetCameraName(i);
                    string s = Marshal.PtrToStringAnsi(a);
                    cmbCamera.Items.Add(s);
                }

                if (cameraCount == 0)
                    cmbCamera.Items.Add("没有");

                if (cmbCamera.Items.Count > 0)
                    cmbCamera.SelectedIndex = 0;
            }
            catch (Exception)
            { }
        }

        private void CloseAllVideoWindow()
        {
            CheckOpened("TempSoloVideoForm");
            CheckOpened("P2pVideoChat");
        }

        private void CheckOpened(string strTag)
        {
            System.Windows.Forms.FormCollection fc = System.Windows.Forms.Application.OpenForms;

            foreach (System.Windows.Forms.Form frm in fc)
            {
                if (frm.Tag.ToString() == strTag)
                {
                    frm.Close();
                    return;
                }
            }           
        }

        private void cmbCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StopAudioCapture();
            StopVideoCapture();
            
            CloseAllVideoWindow();

            Destroy();
            StringBuilder strID = new StringBuilder(Login._UserInfo.Id);
            StringBuilder strServerIP = new StringBuilder(Login._ServerServiceUri);

            int nResult = Init(strID, strServerIP);
            if (nResult == 0)
            {
                IntPtr result = GetLastErrorString();
                string strError = Marshal.PtrToStringAnsi(result);

                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }

            this.cameraView.BackgroundImage = ChatClient.Properties.Resources.videowindow;
            this.cameraView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            int index = 0;
            if (cmbCamera.SelectedIndex >= 0)
            {
                index = cmbCamera.SelectedIndex;
            }

            nResult = StartVideoCapture(index);
            if (nResult == 0)
            {
                IntPtr result = GetLastErrorString();
                string strError = Marshal.PtrToStringAnsi(result);

                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }

            SetCaptureWindow(cameraView.Handle);

            //if (this.cameraConnector1.Connected)
            //    this.cameraConnector1.Disconnect();

            //this.cameraConnector1.BeginConnect(Login._UserInfo.Id);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitCameraControl();            
        }

        private void bTarget_Unloaded(object sender, RoutedEventArgs e)
        {
            StopAudioCapture();
            StopVideoCapture();
        }
    }
}
