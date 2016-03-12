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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for PNF_UserControl.xaml
    /// </summary>
    public partial class PNF_UserControl : UserControl
    {
        public PNF_UserControl()
        {
            InitializeComponent();
        }

        public void InitPNFSetting(ClassInfo classInfo)
        {
            label1.Content = classInfo.Class_Type_Name;
            label2.Content = classInfo.ClassCount + "건";
            if (classInfo.ClassCount > 0)
                label2.Foreground = new SolidColorBrush(Colors.Red);
            else
                label2.Foreground = new SolidColorBrush(Colors.Black);
            classGrid.DataContext = classInfo.Class_Type_Id;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.LightSkyBlue);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var imgIdex = classGrid.DataContext;
            for (int i = 0; i < Window1._ClassListInfo.Classes.Count; i++)
            {
                if (Convert.ToInt32(imgIdex) == Window1._ClassListInfo.Classes[i].Class_Type_Id)
                {
                    if (Window1._ClassListInfo.Classes[i].ClassInfo_Id == 1)
                    {
                        Window1._ClassListInfo.Classes[i].ToIndex = 1;
                        Window1._ClassListInfo.Classes[i].FromIndex = 18;
                    }
                    else
                    {
                        Window1._ClassListInfo.Classes[i].ToIndex = 1;
                        Window1._ClassListInfo.Classes[i].FromIndex = 12;
                    }

                    Window1._ClassInfo = Window1._ClassListInfo.Classes[i];
                    Window1._ClientEngine.Send(NotifyType.Request_ClassTypeInfo, Window1._ClassInfo);
                }
            }
        }
    }
}
