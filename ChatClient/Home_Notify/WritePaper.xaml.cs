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
using System.Windows.Shapes;
using ChatEngine;
using Microsoft.Windows.Controls;
using System.Data;

using ControlExs;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for WritePaper.xaml
    /// </summary>
    public partial class WritePaper : BaseWindow
    {
        Microsoft.Windows.Controls.DataGrid letterDataGrid = null;
        int nSelectId = 0;

        public WritePaper()
        {
            InitializeComponent();
        }

        public void noticeListView(List<BoardInfo> noticeList)
        {
            

            //if (Main.notice_board == true)
            //    witeBt.Visibility = Visibility.Hidden;
            //else
            //    witeBt.Visibility = Visibility.Visible;

            sendList.Children.Clear();
            letterDataGrid = new Microsoft.Windows.Controls.DataGrid();
            letterDataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;

            letterDataGrid.HeadersVisibility = Microsoft.Windows.Controls.DataGridHeadersVisibility.Column;
            letterDataGrid.GridLinesVisibility = Microsoft.Windows.Controls.DataGridGridLinesVisibility.Horizontal;
            //myDataGrid.CellStyle = (Style)FindResource("cellStyle");
            letterDataGrid.Style = (Style)FindResource("DataGridStyle");
            letterDataGrid.RowStyle = (Style)FindResource("rowStyle");
            letterDataGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            letterDataGrid.CellStyle = (Style)FindResource("cellStyle");
            letterDataGrid.Background = new SolidColorBrush(Colors.Transparent);

            letterDataGrid.Width = 515;
            letterDataGrid.Height = 245;

            letterDataGrid.IsReadOnly = true;
            letterDataGrid.MouseDoubleClick += new MouseButtonEventHandler(letterDataGrid_MouseUp);
            letterDataGrid.LoadingRow += dataGrid_LoadingRow;
           
            //myDataGrid.MouseRightButtonDown += new MouseButtonEventHandler(myDataGrid_MouseRightButtonUp);
            Microsoft.Windows.Controls.DataGridTextColumn col1 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col1.Width = 255;

            Microsoft.Windows.Controls.DataGridTextColumn col2 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col2.Width = 70;
            Microsoft.Windows.Controls.DataGridTextColumn col3 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col3.Width = 120;

            Microsoft.Windows.Controls.DataGridTextColumn col4 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col4.Width = new Microsoft.Windows.Controls.DataGridLength(70, Microsoft.Windows.Controls.DataGridLengthUnitType.Star);

            Microsoft.Windows.Controls.DataGridTextColumn col5 = new Microsoft.Windows.Controls.DataGridTextColumn();
            

            letterDataGrid.Columns.Add(col1);
            letterDataGrid.Columns.Add(col2);
            letterDataGrid.Columns.Add(col3);
            letterDataGrid.Columns.Add(col4);
            


            col1.Binding = new Binding("title");


            col2.Binding = new Binding("userId");

            col3.Binding = new Binding("wirteTime");

            col4.Binding = new Binding("sendId");

            

            

            if (noticeList != null)
            {
                for (int j = 0; j < noticeList.Count; j++)
                {

                    letterDataGrid.Items.Add(new SendData { title = noticeList[j].Title, userId = noticeList[j].UserId, 
                    wirteTime = noticeList[j].WriteTime.ToString("yy/MM/dd HH:mm"), sendId = noticeList[j].SendId, readed = noticeList[j].Readed, id =noticeList[j].Id  });
                    letterDataGrid.Background = new SolidColorBrush(Colors.Transparent);
                }
            }
            col1.Header = "题目";

            col2.Header = "发送人";

            col3.Header = "日期";

            col4.Header = "收到人";

            col5.Header = "";
            col5.Visibility = Visibility.Hidden;



            sendList.Children.Add(letterDataGrid);
        }


        void letterDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (letterDataGrid.SelectedIndex < 0)
                return;
            
            SendData selectedFile = (SendData)letterDataGrid.Items[letterDataGrid.SelectedIndex];
            nSelectId = selectedFile.id;



            if (Main.notice_board == true)
            {
                
                for (int i = 0; i < Login._HomeInfo.Notices.Count; i++)
                {
                    if (Login._HomeInfo.Notices[i].Id == nSelectId)
                    {
                        Main.writeSend = new WriteSend();
                        Main.writeSend.title.Text = Login._HomeInfo.Notices[i].Title;
                        Main.writeSend.contents.Text = Login._HomeInfo.Notices[i].Content;
                        Main.writeSend.title.IsReadOnly = true;
                        Main.writeSend.contents.IsReadOnly = true;
                        Main.writeSend.btFlag = false;
                        Main.writeSend.ShowDialog();
                        
                    }
                }

                Login._BoardInfo.Id = nSelectId;
                Login._ClientEngine.Send(NotifyType.Request_ReadNotice, Login._BoardInfo);
                
            }
            else
            {
                for (int i = 0; i < Login.noticeList.Count; i++)
                {
                    if (Login.noticeList[i].Id == nSelectId)
                    {
                        Main.writeSend = new WriteSend();
                        Main.writeSend.title.Text = Login.noticeList[i].Title;
                        Main.writeSend.contents.Text = Login.noticeList[i].Content;
                        Main.writeSend.title.IsReadOnly = true;
                        Main.writeSend.contents.IsReadOnly = true;
                        Main.writeSend.btFlag = false;
                        Main.writeSend.ShowDialog();
                        
                    }
                }
            }
            
        }

        private class SendData
        {
            public int id { set; get; }
            public string title { set; get; }
            public string userId { set; get; }
            public string wirteTime { set; get; }
            public string sendId { set; get; }
            public int readed { set; get; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main.writeSend = new WriteSend();
            Main.writeSend.Show();
        }

        private void delBt_Click(object sender, RoutedEventArgs e)
        {
            //if (letterDataGrid.SelectedIndex < 0)
            //    return;

            SendData sendData = (SendData)letterDataGrid.SelectedItem;

            TempWindowForm tempWindowForm = new TempWindowForm();
            if (sendData == null)
            {                
                QQMessageBox.Show(tempWindowForm, "请选择需要删除的题目", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                return;
            }

            if (QQMessageBox.Show(tempWindowForm, "您确定要删除吗?", "提示", QQMessageBoxIcon.Question, QQMessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            
            Login._BoardInfo.Id = sendData.id;
            if (Main.notice_board == true)
            {
                Login._ClientEngine.Send(NotifyType.Request_DelNotice, Login._BoardInfo);
            }
            else
            {
                Login._ClientEngine.Send(NotifyType.Request_DelLetter, Login._BoardInfo);
            }
        }

        void dataGrid_LoadingRow(object sender, Microsoft.Windows.Controls.DataGridRowEventArgs e)
        {
            // Get the DataRow corresponding to the DataGridRow that is loading.
            SendData item = e.Row.Item as SendData;
            if (item != null)
            {
                if (item.readed != 1)
                    e.Row.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.writePaper = null;
            Main.boardView = 0;
            Main.noticeView = 0;
        }

        private void viewBt_Click(object sender, RoutedEventArgs e)
        {
            if (letterDataGrid.SelectedIndex < 0)
            {                
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "请选择需要删除的题目", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                return;
            }

            SendData selectedFile = (SendData)letterDataGrid.Items[letterDataGrid.SelectedIndex];
            nSelectId = selectedFile.id;


            if (Main.notice_board == true)
            {

                for (int i = 0; i < Login._HomeInfo.Notices.Count; i++)
                {
                    if (Login._HomeInfo.Notices[i].Id == nSelectId)
                    {
                        Main.writeSend = new WriteSend();
                        Main.writeSend.title.Text = Login._HomeInfo.Notices[i].Title;
                        Main.writeSend.contents.Text = Login._HomeInfo.Notices[i].Content;
                        Main.writeSend.title.IsReadOnly = true;
                        Main.writeSend.contents.IsReadOnly = true;
                        Main.writeSend.btFlag = false;
                        Main.writeSend.Show();

                    }
                }

                Login._BoardInfo.Id = nSelectId;
                Login._ClientEngine.Send(NotifyType.Request_ReadNotice, Login._BoardInfo);

            }
            else
            {
                for (int i = 0; i < Login.noticeList.Count; i++)
                {
                    if (Login.noticeList[i].Id == nSelectId)
                    {
                        Main.writeSend = new WriteSend();
                        Main.writeSend.title.Text = Login.noticeList[i].Title;
                        Main.writeSend.contents.Text = Login.noticeList[i].Content;
                        Main.writeSend.title.IsReadOnly = true;
                        Main.writeSend.contents.IsReadOnly = true;
                        Main.writeSend.btFlag = false;
                        Main.writeSend.Show();

                    }
                }
            }
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
