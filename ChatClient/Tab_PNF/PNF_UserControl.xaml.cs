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
            label2.Content = classInfo.ClassCount + "张";
            if (classInfo.ClassCount > 0)
                label2.Foreground = new SolidColorBrush(Colors.Red);
            else
                label2.Foreground = new SolidColorBrush(Colors.Black);
            classGrid.DataContext = classInfo.Class_Type_Id;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //this.Background = new SolidColorBrush(Colors.LightSkyBlue);
            this.Background = (System.Windows.Media.LinearGradientBrush)FindResource("GradientColor");
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var imgIdex = classGrid.DataContext;
            for (int i = 0; i < Login._ClassListInfo.Classes.Count; i++)
            {
                if (Convert.ToInt32(imgIdex) == Login._ClassListInfo.Classes[i].Class_Type_Id)
                {
                    if (Login._ClassListInfo.Classes[i].ClassInfo_Id == 1)
                    {
                        Login._ClassListInfo.Classes[i].ToIndex = 1;
                        Login._ClassListInfo.Classes[i].FromIndex = 18;
                    }
                    else
                    {
                        Login._ClassListInfo.Classes[i].ToIndex = 1;
                        Login._ClassListInfo.Classes[i].FromIndex = 12;
                    }

                    Login._ClassInfo = Login._ClassListInfo.Classes[i];

                    Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                    {
                        if (Main.loadWnd == null)
                        {
                            Main.loadWnd = new LoadingWnd();
                            Main.loadWnd.Owner = GetParentWindow(this);
                            Main.loadWnd.ShowDialog();
                        }
                    }));

                    Login._ClientEngine.Send(NotifyType.Request_ClassTypeInfo, Login._ClassInfo);
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
