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

using ChatEngine;

namespace ChatClient.Tab_PNF
{
    /// <summary>
    /// Interaction logic for SiteURLUserCtrl.xaml
    /// </summary>
    public partial class SiteURLUserCtrl : UserControl
    {
        public SiteURLUserCtrl()
        {
            InitializeComponent();
        }

        public void InitSiteUrlCtrl(ClassInfo classInfo)
        {
            this.txtSiteName.Text = classInfo.Class_Type_Name;
            this.linkSiteUrl.NavigateUri = new Uri(classInfo.Class_Img_Uri, UriKind.RelativeOrAbsolute);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.OriginalString));
                e.Handled = true;
            }
            catch (System.Exception ex)
            {
                string strErrorMsg = ex.ToString();
            }
        }
    }
}
