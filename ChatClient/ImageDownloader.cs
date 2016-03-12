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


                //string filePath = string.Format("image/face/{0}", imagePath);

                string strPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
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
                    Uri uriSource = new Uri(Window1._ServerPath + imagePath, UriKind.RelativeOrAbsolute);
                    //string uri = Window1._ServerPath + imagePath;

                    //Stopwatch stopWatch = new Stopwatch();
                    //stopWatch.Start();

                    WebClient webClient = new WebClient();
                    webClient.Proxy = null;

                    Stream stream = new MemoryStream(webClient.DownloadData(uriSource.AbsoluteUri));

                    //stopWatch.Stop();
                    //double elpased = stopWatch.Elapsed.TotalMilliseconds;

                    //Byte[] buffer = new Byte[response.ContentLength];
                    //int offset = 0, actuallyRead = 0;

                    //do
                    //{
                    //    actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                    //    offset += actuallyRead;
                    //}
                    //while (actuallyRead > 0);

                    //File.WriteAllBytes(filePath, buffer);

                    // bi = new BitmapImage();

                    bi.BeginInit();
                    bi.StreamSource = stream;
                    bi.EndInit();
                }
            }
            catch( Exception e )
            {
                //MessageBoxCommon.Show("봉사기가 응답이 없습니다. 프로그램을 종료합니다.", MessageBoxType.Ok);
                //Window1.ShowError(ChatEngine.ErrorType.Notrespond_Server);

                //if (Window1.main != null)
                //    Window1.main.Image_MouseUp(null, null);
                //else
                //    Application.Current.Shutdown();

                bi.BeginInit();
                bi.UriSource = new Uri("image\\face\\noimage.png", UriKind.RelativeOrAbsolute);
                bi.EndInit();
            }

            return bi;
        }
    }

}
