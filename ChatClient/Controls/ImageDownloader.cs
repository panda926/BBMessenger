using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Net;
using System.Diagnostics;
using System.Windows;

namespace ChatClient
{
    class ImageDownloader
    {
        private static ImageDownloader _instance = null;

        public static ImageDownloader GetInstance()
        {
            if (_instance == null)
                _instance = new ImageDownloader();

            return _instance;
        }

        public BitmapImage GetImage(string imagePath)
        {
            BitmapImage bi = new BitmapImage();

            try
            {
                if (imagePath == null)
                    return bi;


                //string filePath = string.Format("/Resources;component/image/face/{0}", imagePath);

                //string strPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string strPath = System.Windows.Forms.Application.StartupPath;
                string filePath = strPath + "\\" + imagePath;
                

                if (File.Exists(filePath) == true)
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    //bi = new BitmapImage();

                    bi.BeginInit();
                    bi.UriSource = new Uri(fileInfo.FullName, UriKind.RelativeOrAbsolute);
                    bi.EndInit();
                }
                else
                {
                    //Uri uriSource = new Uri(Login._ServerPath + imagePath, UriKind.RelativeOrAbsolute);
                    ////string uri = Login._ServerPath + imagePath;

                    ////Stopwatch stopWatch = new Stopwatch();
                    ////stopWatch.Start();

                    //WebClient webClient = new WebClient();
                    //webClient.Proxy = null;

                    //Stream stream = new MemoryStream(webClient.DownloadData(uriSource.AbsoluteUri));

                    //using (FileStream fileStream = System.IO.File.Create(filePath, (int)stream.Length))
                    //{
                    //    // Fill the bytes[] array with the stream data
                    //    byte[] bytesInStream = new byte[stream.Length];
                    //    stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                    //    // Use FileStream object to write to the specified file
                    //    fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                    //}

                    ////stopWatch.Stop();
                    ////double elpased = stopWatch.Elapsed.TotalMilliseconds;

                    ////Byte[] buffer = new Byte[response.ContentLength];
                    ////int offset = 0, actuallyRead = 0;

                    ////do
                    ////{
                    ////    actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                    ////    offset += actuallyRead;
                    ////}
                    ////while (actuallyRead > 0);

                    ////File.WriteAllBytes(filePath, buffer);

                    //// bi = new BitmapImage();

                    //bi.BeginInit();
                    //bi.StreamSource = stream;
                    //bi.EndInit();

                    bi.BeginInit();
                    bi.UriSource = new Uri(strPath + "\\image\\face\\noimage.png", UriKind.RelativeOrAbsolute);
                    bi.EndInit();
                }
            }
            catch( Exception ex )
            {
                //MessageBoxCommon.Show("봉사기가 응답이 없습니다. 프로그램을 종료합니다.", MessageBoxType.Ok);
                //Login.ShowError(ChatEngine.ErrorType.Notrespond_Server);

                //if (Login.main != null)
                //    Login.main.Image_MouseUp(null, null);
                //else
                //    Application.Current.Shutdown();
                string strPath = System.Windows.Forms.Application.StartupPath;

                bi.BeginInit();
                bi.UriSource = new Uri(strPath + "\\image\\face\\noimage.png", UriKind.RelativeOrAbsolute);
                bi.EndInit();


                string strError = ex.ToString() + imagePath;
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }

            return bi;
        }
    }

}
