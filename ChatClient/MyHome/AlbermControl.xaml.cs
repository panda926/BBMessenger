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

        public void pictureList(List<IconInfo> iconInfoList)
        {
            wrapPanel1.Children.Clear();

            for (int i = 0; i < iconInfoList.Count; i++)
            {
                string video = iconInfoList[i].Icon;
                char[] delimiterChars = { ':', '\\' };
                string[] words = video.Split(delimiterChars);
                int count = words.Length;

                if (words[count - 2].ToString() == "Face")
                {
                    var faceGrid = new Grid();
                    faceGrid.Width = 80;
                    faceGrid.Height = 80;
                    faceGrid.Margin = new Thickness(0);

                    Image finalImage = new Image();
                    finalImage.Cursor = Cursors.Hand;
                    finalImage.Margin = new Thickness(1, 1, 1, 1);
                    
                    finalImage.DataContext = iconInfoList[i].Icon;
                    finalImage.Source = ImageDownloader.GetInstance().GetImage(iconInfoList[i].Icon);
                    finalImage.MouseRightButtonUp += new MouseButtonEventHandler(imgMenu_MouseRightUp);
                    finalImage.MouseLeftButtonUp += new MouseButtonEventHandler(finalImage_MouseLeftButtonUp);
                    faceGrid.Children.Add(finalImage);
                    wrapPanel1.Children.Add(faceGrid);
                }
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

        
        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                Image myImg = new Image();

                BitmapImage myBit = new BitmapImage();
                myBit.BeginInit();
                myBit.UriSource = new Uri(dlg.FileName, UriKind.Absolute);
                myBit.EndInit();
                /*********************************/
                char[] delimiterChars = { ':', '\\' };
                string[] words = dlg.FileName.Split(delimiterChars);
                int count = words.Length;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(myBit));

                byte[] bit = new byte[0];

                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bit = stream.ToArray();
                    stream.Close();


                    if (bit.Length >= 120000)
                        MessageBoxCommon.Show("文件过大上传失败.请上传100KB以下的文件.", MessageBoxType.Ok);
                    else
                    {
                        VideoInfo videoInfo = new VideoInfo();
                        videoInfo.Data = bit;
                        videoInfo.UserId = words[count - 1].ToString();
                        videoInfo.ImgData = bit;
                        videoInfo.ImgName = words[count - 1].ToString();

                        Window1._ClientEngine.Send(NotifyType.Request_IconUpload, videoInfo);
                    }
                    
                }
                /*************************************/
               

            }
        }

        void imgMenu_MouseRightUp(object sender, MouseButtonEventArgs e)
        {
            Image selectImg = sender as Image;
            ContextMenu contextMenu = new ContextMenu();
            MenuItem item1 = new MenuItem();
            item1.Click += new RoutedEventHandler(item1_Click);
            item1.Header = "设定图像";
            item1.DataContext = selectImg.DataContext.ToString();
            MenuItem item2 = new MenuItem();
            item2.Click += new RoutedEventHandler(item2_Click);
            item2.Header = "删除图像";
            item2.DataContext = selectImg.DataContext.ToString();
            contextMenu.Items.Add(item1);
            contextMenu.Items.Add(item2);
            contextMenu.IsOpen = true;
        }

        void item1_Click(object sender, RoutedEventArgs e)
        {
            var selectMenu = sender as MenuItem;
            string imgIcon = selectMenu.DataContext.ToString();
            ImageBrush selectIconImg = new ImageBrush();
            selectIconImg.ImageSource = ImageDownloader.GetInstance().GetImage(imgIcon);
            
            Window1.myhome.myPicture.Fill = selectIconImg;
            Window1._UserInfo.Icon = imgIcon;
            Window1._ClientEngine.Send(NotifyType.Request_UpdateUser, Window1._UserInfo);
        }
        void item2_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxCommon.Show("您确定要删除吗?", MessageBoxType.YesNo) == MessageBoxReply.No)
                return;

            var selectMenu = sender as MenuItem;
            string imgIcon = selectMenu.DataContext.ToString();
            if (imgIcon == Window1._UserInfo.Icon)
            {
                MessageBoxCommon.Show("已设定在头像的图片不能删除.", MessageBoxType.Ok);
            }
            else
            {
                for(int i=0;i<Window1.myhome.iconInfoList.Count;i++)
                {
                    if(imgIcon == Window1.myhome.iconInfoList[i].Icon)
                    {
                        Window1._ClientEngine.Send(NotifyType.Request_IconRemove, Window1.myhome.iconInfoList[i]);

                        break;
                    }
                }
                
            }
        }
       
	}
}