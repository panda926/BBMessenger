using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ChatClient
{
    public class Utils
    {
        public static void FileExecute(string strFileName)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo pi = new System.Diagnostics.ProcessStartInfo();
            pi.UseShellExecute = true;
            pi.FileName = strFileName;
            p.StartInfo = pi;

            try
            {
                p.Start();
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        public static string GetExePath()
        {
            //return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return System.Windows.Forms.Application.StartupPath;
        }

        public static string GetFileName(string filePath)
        {
            string fileName = string.Empty;

            string[] folders = filePath.Split('\\');

            if (folders.Length > 0)
                fileName = folders[folders.Length - 1];

            return fileName;
        }
    }
}
