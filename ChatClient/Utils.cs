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
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}
