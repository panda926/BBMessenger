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
using System.Windows.Shapes;

// 2014-04-05: GreenRose
// 여자인경우 카메라선택창을 리용한다.

using GGLib.NDisk.Passive;
using ESPlus.Application.CustomizeInfo;
using ESPlus.Rapid;
using ESPlus.Application.Basic;
using System.Configuration;
using OMCS.Passive;
using OMCS.Tools;
using GG.Core;
using ESBasic;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for CamSelectingWnd.xaml
    /// </summary>
    public partial class CamSelectingWnd : BaseWindow
    {
        public Main mainWnd = null;

        public CamSelectingWnd()
        {
            InitializeComponent();

            LoginOMCSServer();
            InitCameraControl();
        }
        
        private void InitCameraControl()
        {
            try
            {
                List<CameraInformation> listCameraInfos = OMCS.Tools.Camera.GetCameras();

                for (int i = 0; i < listCameraInfos.Count; i++)
                {
                    cmbCamera.Items.Add(listCameraInfos[i].Name);
                }

                if (listCameraInfos.Count == 0)
                    cmbCamera.Items.Add("没有");

                if (cmbCamera.Items.Count > 0)
                    cmbCamera.SelectedIndex = 0;
            }
            catch (Exception)
            { }
        }

        // 2014-02-26: GreenRose 
        // Connect to OMCS server
        private bool LoginOMCSServer()
        {
            try
            {
                string id = Login._UserInfo.Id;
                string pwd = Login._UserInfo.Password;

                Login.multimediaManager.ChannelMode = ChannelMode.P2PChannelFirst;
                //multimediaManager.CameraDeviceIndex = int.Parse(ConfigurationManager.AppSettings["CameraIndex"]);
                //multimediaManager.MicrophoneDeviceIndex = int.Parse(ConfigurationManager.AppSettings["MicrophoneIndex"]);
                //multimediaManager.SpeakerIndex = int.Parse(ConfigurationManager.AppSettings["SpeakerIndex"]);
                //multimediaManager.CameraEncodeQuality = 3;
                //multimediaManager.CameraVideoSize = new System.Drawing.Size(320, 240); //Size(320, 240)  


                List<CameraInformation> listCameraInfos = OMCS.Tools.Camera.GetCameras();
                if (listCameraInfos.Count > 0)
                {
                    Login.multimediaManager.CameraDeviceIndex = 0;
                    List<CameraCapability> listCameraCapability = OMCS.Tools.Camera.GetCameraCapability(0);

                    if (listCameraCapability.Count > 0)
                    {
                        //multimediaManager.CameraVideoSize = listCameraCapability[listCameraCapability.Count - 1].VideoSize;
                        //Login.multimediaManager.CameraVideoSize = listCameraCapability[listCameraCapability.Count - 1].VideoSize;
                    }
                }

                List<MicrophoneInformation> listMicInfos = OMCS.Tools.SoundDevice.GetMicrophones();
                if (listMicInfos.Count > 0)
                    Login.multimediaManager.MicrophoneDeviceIndex = 0;

                List<SpeakerInformation> listSpeakInfos = OMCS.Tools.SoundDevice.GetSpeakers();
                if (listSpeakInfos.Count > 0)
                    Login.multimediaManager.SpeakerIndex = 0;

                Login.multimediaManager.CameraEncodeQuality = 6;


                Login.multimediaManager.Initialize(id, "", ConfigurationManager.AppSettings["OmcsServerIP"], int.Parse(ConfigurationManager.AppSettings["OmcsServerPort"]));
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }

            return true;
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmbCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCamera.SelectedIndex >= 0)
            {
                Login.multimediaManager.CameraDeviceIndex = cmbCamera.SelectedIndex;
                List<CameraCapability> listCameraCapability = OMCS.Tools.Camera.GetCameraCapability(cmbCamera.SelectedIndex);

                if (listCameraCapability.Count > 0)
                {                    
                    Login.multimediaManager.CameraVideoSize = listCameraCapability[listCameraCapability.Count / 2].VideoSize;
                }
            }            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {            
            mainWnd.ShowWebCam();
        }
    }
}
