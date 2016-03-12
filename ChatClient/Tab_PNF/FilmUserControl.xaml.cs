using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChatEngine;

using System.IO;
using System.ComponentModel;
using System.Net;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ControlExs;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for FilmUserControl.xaml
    /// </summary>
    public partial class FilmUserControl : UserControl
    {
        bool fpFlag = true;
        
        // 2013-12-13: GreenRose
        // 비디오보기에서 비디오이미지파일 클릭하였을떄...
        ClassTypeInfo m_classTypeInfo = new ClassTypeInfo();

        // 2013-12-28: GreenRose
        // VideoTitles Count
        int _nVideoTitleCount = 0;

        public FilmUserControl()
        {
            InitializeComponent();
        }

        public void InitFilmView(ClassTypeInfo classTypeInfo)
        {
            string strFilePath = string.Empty;

            if (classTypeInfo.Class_File_Type == 2)
            {
                string[] strVideoTitles = new string[]{""};
                string[] stringSeparators = new string[] { "||" };
                strVideoTitles = classTypeInfo.Class_Video_Title.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                //strFilePath = "VideoFile/" + classTypeInfo.Class_Video_Title;
                //strFilePath = "VideoFile/" + strVideoTitles[0];
                strFilePath = strVideoTitles[0];
                strFilePath = classTypeInfo.Class_File_Name.Substring(0, classTypeInfo.Class_File_Name.LastIndexOf("/")) + "/" + strFilePath;
                image2.DataContext = classTypeInfo.Class_File_Name;

                _nVideoTitleCount = strVideoTitles.Length;
            }
            else
            {
                //strFilePath = "ImageFile/" + classTypeInfo.Class_File_Name;
                strFilePath = classTypeInfo.Class_File_Name;
                image2.DataContext = classTypeInfo.Class_Img_Folder;
            }

            WebDownloader.GetInstance().DownloadFile(strFilePath, TitleDownloadComplete, this);

            dateLabel.Content = classTypeInfo.Class_File_Date.ToString("yyyy-MM-dd");

            char[] delimiterChars = { '.' };
            string[] words = classTypeInfo.Class_File_Name.Split(delimiterChars);
            int count = words.Length;

            // 2014-01-05: GreenRose
            int nDelimiterIndex = classTypeInfo.Class_File_Name.LastIndexOf(".");
            //string strVideoFileName = classTypeInfo.Class_File_Name.Substring(0, nDelimiterIndex);
            string strVideoFileName = classTypeInfo.Class_Img_Folder;

            if (classTypeInfo.Class_File_Type == 2)
            {
                fpFlag = true;
                
                // 2013-12-13: GreenRose
                // 제목의 길이가 5문자이상이라면 ToolTip에 제목을 현시하여 준다.
                //if (words[0].ToString().Length > 8)
                if (strVideoFileName.Length > 8)
                {
                    //string strTitle = words[0].ToString().Substring(0, 8) + "...";
                    string strTitle = strVideoFileName.Substring(0, 8) + "...";
                    titleLabel.Content = strTitle;
                    titleLabel.ToolTip = strVideoFileName;
                    //titleLabel.ToolTip = words[0].ToString();
                }
                else
                {
                    titleLabel.Content = strVideoFileName;
                    //titleLabel.Content = words[0].ToString();
                }

                //lblCountOfImage.Content = string.Empty;
                lblCountOfImage.Content = _nVideoTitleCount.ToString() + "张";

                // 2013-12-13: GreenRose
                // 비디오보기에서 비디오이미지를 클릭하였을때의 사건처리.
                m_classTypeInfo = classTypeInfo;
                image1.Cursor = Cursors.Hand;
                image1.MouseLeftButtonDown += new MouseButtonEventHandler(image1_LButtonClick);
            }
            else
            {
                fpFlag = false;

                // 2013-12-13: GreenRose
                // 제목의 길이가 5문자이상이라면 ToolTip에 제목을 현시하여 준다.
                if (classTypeInfo.Class_Img_Folder.Length > 8)
                {
                    string strTitle = classTypeInfo.Class_Img_Folder.Substring(0, 8) + "...";
                    titleLabel.Content = strTitle;
                    titleLabel.ToolTip = classTypeInfo.Class_Img_Folder;
                }
                else
                    titleLabel.Content = classTypeInfo.Class_Img_Folder;

                lblCountOfImage.Content = classTypeInfo.Class_File_Count + "张";


                m_classTypeInfo = classTypeInfo;
                image1.Cursor = Cursors.Hand;
                image1.MouseLeftButtonDown += new MouseButtonEventHandler(image1_LButtonClick1);
            }

            

            for (int i = 0; i < Login._ClassListInfo.Classes.Count; i++)
            {
                if (Login._ClassListInfo.Classes[i].Class_Type_Id == classTypeInfo.Class_File_Area)
                {
                    positionLabel.Content = Login._ClassListInfo.Classes[i].Class_Type_Name;
                    break;
                }
            }
            
        }

        // 2013-12-13: GreenRose
        // 비디오보기에서 이미지를 클릭하였을때의 처리.
        private void image1_LButtonClick(object sender, MouseButtonEventArgs e)
        {
            Login._ClientEngine.Send(NotifyType.Request_ClassPictureDetail, m_classTypeInfo);
        }

        // 2014-02-08: GreenRose
        // 비디오보기에서 이미지를 클릭하였을때의 처리.
        private void image1_LButtonClick1(object sender, MouseButtonEventArgs e)
        {
            if (fpFlag == true)
            {
                //WebDownloader.GetInstance().DownloadFile("VideoFile/" + image2.DataContext.ToString(), VideoDownloadComplete, this);
                WebDownloader.GetInstance().DownloadFile(image2.DataContext.ToString(), VideoDownloadComplete, this);
                return;
            }
            else
            {
                for (int i = 0; i < Login._ClassTypeListInfo.ClassType.Count; i++)
                {
                    string folderName = Login._ClassTypeListInfo.ClassType[i].Class_Img_Folder;
                    if (folderName == image2.DataContext.ToString())
                    {
                        Login._ClassTypeInfo = Login._ClassTypeListInfo.ClassType[i];
                        Login._ClientEngine.Send(NotifyType.Request_ClassPictureDetail, Login._ClassTypeInfo);
                        break;
                    }
                }
            }
        }

        public void TitleDownloadComplete(string filePath)
        {
            try
            {
                //복호화한다. 2014-2-25 by pakcj 
                //string decPath = WebDownloader.GetInstance().DecryptFile(filePath);
                string decPath = filePath;

                string strImagePath = decPath;
                BitmapImage bi = new BitmapImage();

                bi.BeginInit();
                bi.UriSource = new Uri(strImagePath, UriKind.RelativeOrAbsolute);
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();

                image1.Source = bi;            
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        public void VideoDownloadComplete(string filePath)
        {            
            //if (Main.loadWnd != null)
            //{
            //    Main.loadWnd.Close();
            //    Main.loadWnd = null;
            //}
            
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Length == 0)
            {
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "对不起， 没有资料。", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
            }

            //복호화한다. 2014-2-25 by pakcj 
            //string decPath = WebDownloader.GetInstance().DecryptFile(filePath);
            string decPath = filePath;

            Utils.FileExecute(decPath);
            bClickFlag = false;
        }

        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (fpFlag == true)
            //{
            //    WebDownloader.GetInstance().DownloadFile("VideoFile/" + image2.DataContext.ToString(), VideoDownloadComplete, this);
            //    return;
            //}
            //else
            //{
            //    for (int i = 0; i < Login._ClassTypeListInfo.ClassType.Count; i++)
            //    {
            //        string folderName = Login._ClassTypeListInfo.ClassType[i].Class_Img_Folder;
            //        if (folderName == image2.DataContext.ToString())
            //        {
            //            Login._ClassTypeInfo = Login._ClassTypeListInfo.ClassType[i];
            //            Login._ClientEngine.Send(NotifyType.Request_ClassPictureDetail, Login._ClassTypeInfo);
            //            break;
            //        }
            //    }
            //}
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //gridFilm.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightSkyBlue);
            gridFilm.Background = (System.Windows.Media.LinearGradientBrush)FindResource("GradientColor");
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            gridFilm.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
        }

        bool bClickFlag = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (bClickFlag == false)
            {
                if (fpFlag == true)
                {
                    //Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                    //{
                    //    if (Main.loadWnd == null)
                    //    {
                    //        Main.loadWnd = new LoadingWnd();
                    //        Main.loadWnd.Owner = GetParentWindow(this);
                    //        Main.loadWnd.ShowDialog();
                    //    }
                    //}));
                    bClickFlag = true;
                    //WebDownloader.GetInstance().DownloadFile("VideoFile/" + image2.DataContext.ToString(), VideoDownloadComplete, this);
                    WebDownloader.GetInstance().DownloadFile(image2.DataContext.ToString(), VideoDownloadComplete, this);
                    return;
                }
                else
                {
                    for (int i = 0; i < Login._ClassTypeListInfo.ClassType.Count; i++)
                    {
                        string folderName = Login._ClassTypeListInfo.ClassType[i].Class_Img_Folder;
                        if (folderName == image2.DataContext.ToString())
                        {
                            Login._ClassTypeInfo = Login._ClassTypeListInfo.ClassType[i];
                            Login._ClientEngine.Send(NotifyType.Request_ClassPictureDetail, Login._ClassTypeInfo);
                            break;
                        }
                    }
                }
            }
        }

        public static Window GetParentWindow(DependencyObject child)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }

            Window parent = parentObject as Window;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return GetParentWindow(parentObject);
            }
        }
    }
}
