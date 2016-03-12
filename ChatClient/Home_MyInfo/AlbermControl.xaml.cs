using System;
using System.Collections.Generic;
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
using ChatEngine;
using System.IO;

using ControlExs;

namespace ChatClient
{
	/// <summary>
	/// AlbermControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class AlbermControl : UserControl
	{
		public AlbermControl()
		{
			this.InitializeComponent();
		}

        int nTotalCount = 0;
        public void pictureList(List<IconInfo> iconInfoList)
        {
            wrapPanel1.Children.Clear();

            nTotalCount = iconInfoList.Count;
            if (iconInfoList.Count == 0)
            {
                this.titleName.Visibility = Visibility.Visible;
                return;
            }

            for (int i = 0; i < iconInfoList.Count; i++)
            {
                //string video = iconInfoList[i].Icon;
                //char[] delimiterChars = { ':', '\\' };
                //string[] words = video.Split(delimiterChars);
                //int count = words.Length;

                //if (words[count - 2].ToString() == "Face")
                //{
                //    var faceGrid = new Grid();
                //    faceGrid.Width = 80;
                //    faceGrid.Height = 80;
                //    faceGrid.Margin = new Thickness(0);

                //    Image finalImage = new Image();
                //    finalImage.Cursor = Cursors.Hand;
                //    finalImage.Margin = new Thickness(1, 1, 1, 1);
                    
                //    finalImage.DataContext = iconInfoList[i].Icon;
                //    finalImage.Source = ImageDownloader.GetInstance().GetImage(iconInfoList[i].Icon);
                //    finalImage.MouseRightButtonUp += new MouseButtonEventHandler(imgMenu_MouseRightUp);
                //    finalImage.MouseLeftButtonUp += new MouseButtonEventHandler(finalImage_MouseLeftButtonUp);
                //    faceGrid.Children.Add(finalImage);
                //    wrapPanel1.Children.Add(faceGrid);
                //}
                
                // 2013-12-30: GreenRose                
                if (!iconInfoList[i].Icon.Contains(".avi"))
                {
                    iconInfoList[i].Icon = iconInfoList[i].Icon.Replace("\\", "/");

                    WebDownloader.GetInstance().ctrlAlbum = this;
                    progressUpdate.Visibility = Visibility.Visible;
                    WebDownloader.GetInstance().DownloadFile(iconInfoList[i].Icon, TitleDownloadComplete, this);
                }
            }
        }

        // 2013-12-30: GreenRose
        int nStep = 0;
        public void TitleDownloadComplete(string filePath)
        {
            try
            {
                string strImagePath = filePath;
                BitmapImage bi = new BitmapImage();

                bi.BeginInit();
                bi.UriSource = new Uri(strImagePath, UriKind.RelativeOrAbsolute);
                bi.EndInit();

                var faceGrid = new Grid();
                faceGrid.Width = 82;
                faceGrid.Height = 81;
                faceGrid.Margin = new Thickness(0);

                Image finalImage = new Image();
                finalImage.Cursor = Cursors.Hand;
                finalImage.Margin = new Thickness(2, 1, 2, 1);
                finalImage.DataContext = filePath;
                finalImage.Source = bi;
                finalImage.MouseLeftButtonUp += new MouseButtonEventHandler(finalImage_MouseLeftButtonUp);
                finalImage.MouseRightButtonUp += new MouseButtonEventHandler(imgMenu_MouseRightUp);
                faceGrid.Children.Add(finalImage);
                wrapPanel1.Children.Add(faceGrid);

                nStep++;

                if (nStep == nTotalCount)
                {
                    progressUpdate.Visibility = Visibility.Hidden;
                    WebDownloader.GetInstance().ctrlSelectAlbum = null;

                    nStep = 0;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        void finalImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            

            //wrapPanel1.Children.Clear();
            //callImg.IsEnabled = false;
            var getIcon = sender as Image;
            string iconName = getIcon.DataContext.ToString();

            if (Main.previewImage == null)
                Main.previewImage = new PreviewImage();
            Main.previewImage.ViewImage(iconName);
            Main.previewImage.Show();

            //var reGrid = new Grid();
            //reGrid.Width = 380;
            //reGrid.Height = 250;

            //Image reImage = new Image();
            //reImage.Source = ImageDownloader.GetInstance().GetImage(iconName);
            //reGrid.Children.Add(reImage);
            //wrapPanel1.Children.Add(reGrid);
        }

        int nTotalFileCount = 0;
        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            //// Create OpenFileDialog 
            //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //// Set filter for file extension and default file extension 
            //dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            //Nullable<bool> result = dlg.ShowDialog();
            //// Get the selected file name and display in a TextBox 
            //if (result == true)
            //{
            //    Image myImg = new Image();

            //    BitmapImage myBit = new BitmapImage();
            //    myBit.BeginInit();
            //    myBit.UriSource = new Uri(dlg.FileName, UriKind.Absolute);
            //    myBit.EndInit();
            //    /*********************************/
            //    char[] delimiterChars = { ':', '\\' };
            //    string[] words = dlg.FileName.Split(delimiterChars);
            //    int count = words.Length;
            //    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            //    encoder.Frames.Add(BitmapFrame.Create(myBit));

            //    byte[] bit = new byte[0];

            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        encoder.Save(stream);
            //        bit = stream.ToArray();
            //        stream.Close();


            //        if (bit.Length >= 120000)
            //        {
            //            //MessageBoxCommon.Show("文件过大上传失败.请上传100KB以下的文件.", MessageBoxType.Ok);
            //            TempWindowForm tempWindowForm = new TempWindowForm();
            //            QQMessageBox.Show(tempWindowForm, "文件过大上传失败.请上传100KB以下的文件.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
            //        }
            //        else
            //        {
            //            VideoInfo videoInfo = new VideoInfo();
            //            videoInfo.Data = bit;
            //            videoInfo.UserId = words[count - 1].ToString();
            //            videoInfo.ImgData = bit;
            //            videoInfo.ImgName = words[count - 1].ToString();

            //            Login._ClientEngine.Send(NotifyType.Request_IconUpload, videoInfo);
            //        }
                    
            //    }
            //    /*************************************/
               

            //}

            try
            {
                System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
                fileDialog.Multiselect = false;
                fileDialog.Filter = "ALL Files (*.*)|*.*|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
                fileDialog.Title = "请选择图片文件.";

                if (System.Windows.Forms.DialogResult.OK == fileDialog.ShowDialog())
                {
                    
                    foreach (string fileName in fileDialog.FileNames)
                    {
                        System.IO.FileInfo fileSize = new System.IO.FileInfo(fileName);
                        if (fileSize.Length > 1024 * 10 * 10 * 10 * 10 / 2)
                            continue;

                        string strUri = Login._ServerPath;
                        if (strUri[strUri.Length - 1] != '/')
                        {
                            strUri = strUri + "/";
                        }

                        strUri += "FaceUpload.php";

                        WebUploader.GetInstance().ctrlAlbum = this;
                        progressUpdate.Visibility = Visibility.Visible;
                        WebUploader.GetInstance().UploadFile(fileName, strUri, FileUploadComplete, this);
                        
                        //if (UpdateFile(fileName))
                        //{
                        //    byte[] bit = new byte[0];

                        //    VideoInfo videoInfo = new VideoInfo();
                        //    videoInfo.Data = bit;
                        //    videoInfo.UserId = Login._UserInfo.Id;
                        //    videoInfo.ImgData = bit;
                        //    int nIndex = fileName.LastIndexOf("\\");
                        //    videoInfo.ImgName = fileName.Substring(nIndex + 1);
                        //    Login._ClientEngine.Send(NotifyType.Request_IconUpload, videoInfo);
                        //}

                        nTotalFileCount++;
                    }


                    if (nTotalFileCount == 0)
                    {
                        TempWindowForm tempWindowForm = new TempWindowForm();
                        QQMessageBox.Show(tempWindowForm, "文件大小必须小于 5MB.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                    }
                }


            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        // 2013-12-31: GreenRose
        int nUploadStep = 0;
        private void FileUploadComplete(string strFileName)
        {
            byte[] bit = new byte[0];

            VideoInfo videoInfo = new VideoInfo();
            videoInfo.Data = bit;
            videoInfo.UserId = Login._UserInfo.Id;
            videoInfo.ImgData = bit;
            int nIndex = strFileName.LastIndexOf("\\");
            videoInfo.ImgName = strFileName.Substring(nIndex + 1);
            Login._ClientEngine.Send(NotifyType.Request_IconUpload, videoInfo);

            nUploadStep++;
            if (nUploadStep == nTotalCount)
            {
                progressUpdate.Visibility = Visibility.Hidden;
                WebDownloader.GetInstance().ctrlSelectAlbum = null;

                nUploadStep = 0;
            }
        }

        // 2013-12-29: GreenRose
        private bool UpdateFile(string strFileName)
        {
            try
            {
                string strUri = Login._ServerPath;
                if (strUri[strUri.Length - 1] != '/')
                {
                    strUri = strUri + "/";
                }

                strUri += "FaceUpload.php";

                System.Net.WebClient wc = new System.Net.WebClient();
                wc.Credentials = System.Net.CredentialCache.DefaultCredentials;
                wc.UploadFile(strUri, strFileName);
                wc.Dispose();
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);

                return false;
            }

            return true;
        }

        void imgMenu_MouseRightUp(object sender, MouseButtonEventArgs e)
        {
            Image selectImg = sender as Image;
            ContextMenu contextMenu = new ContextMenu();
            //MenuItem item1 = new MenuItem();
            //item1.Click += new RoutedEventHandler(item1_Click);
            //item1.Header = "设定图像";
            //item1.DataContext = selectImg.DataContext.ToString();
            MenuItem item2 = new MenuItem();
            item2.Click += new RoutedEventHandler(item2_Click);
            item2.Header = "删除图像";
            item2.DataContext = selectImg.DataContext.ToString();
            //contextMenu.Items.Add(item1);
            contextMenu.Items.Add(item2);
            contextMenu.IsOpen = true;
        }

        void item1_Click(object sender, RoutedEventArgs e)
        {
            var selectMenu = sender as MenuItem;
            string imgIcon = selectMenu.DataContext.ToString();
            ImageBrush selectIconImg = new ImageBrush();
            selectIconImg.ImageSource = ImageDownloader.GetInstance().GetImage(imgIcon);
            
            Login.myhome.myPicture.Fill = selectIconImg;
            Login._UserInfo.Icon = imgIcon;
            Login._ClientEngine.Send(NotifyType.Request_UpdateUser, Login._UserInfo);
        }
        void item2_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBoxCommon.Show("您确定要删除吗?", MessageBoxType.YesNo) == MessageBoxReply.No)
            //    return;
            TempWindowForm tempWindowForm = new TempWindowForm();
            if (QQMessageBox.Show(tempWindowForm, "您确定要删除吗?", "提示", QQMessageBoxIcon.Question, QQMessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                return;

            var selectMenu = sender as MenuItem;
            string imgIcon = selectMenu.DataContext.ToString();
            if (imgIcon == Login._UserInfo.Icon)
            {
                QQMessageBox.Show(tempWindowForm, "已设定在头像的图片不能删除.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                //MessageBoxCommon.Show("已设定在头像的图片不能删除.", MessageBoxType.Ok);
            }
            else
            {
                for(int i=0;i<Login.myhome.iconInfoList.Count;i++)
                {
                    if(imgIcon == System.Windows.Forms.Application.StartupPath + "\\" + Login.myhome.iconInfoList[i].Icon.Replace("/", "\\"))
                    {
                        Login._ClientEngine.Send(NotifyType.Request_IconRemove, Login.myhome.iconInfoList[i]);

                        break;
                    }
                }
                
            }
        }
       
	}
}