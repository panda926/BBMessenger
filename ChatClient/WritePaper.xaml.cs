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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for WritePaper.xaml
    /// </summary>
    public partial class WritePaper : Window
    {
        DataGrid letterDataGrid = null;
        int nSelectId = 0;

        public WritePaper()
        {
            InitializeComponent();
        }

        public void noticeListView(List<BoardInfo> noticeList)
        {
            

            if (Main.notice_board == true)
                witeBt.Visibility = Visibility.Hidden;
            else
                witeBt.Visibility = Visibility.Visible;

            sendList.Children.Clear();
            letterDataGrid = new DataGrid();
            letterDataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;

            letterDataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            letterDataGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
            //myDataGrid.CellStyle = (Style)FindResource("cellStyle");
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
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Width = 255;

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Width = 70;
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Width = 120;

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Width = new DataGridLength(70, DataGridLengthUnitType.Star);

            DataGridTextColumn col5 = new DataGridTextColumn();
            

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

            col4.Header = "Send To";

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
                
                for (int i = 0; i < Window1._HomeInfo.Notices.Count; i++)
                {
                    if (Window1._HomeInfo.Notices[i].Id == nSelectId)
                    {
                        Main.writeSend = new WriteSend();
                        Main.writeSend.title.Text = Window1._HomeInfo.Notices[i].Title;
                        Main.writeSend.contents.Text = Window1._HomeInfo.Notices[i].Content;
                        Main.writeSend.title.IsReadOnly = true;
                        Main.writeSend.contents.IsReadOnly = true;
                        Main.writeSend.btFlag = false;
                        Main.writeSend.ShowDialog();
                        
                    }
                }

                Window1._BoardInfo.Id = nSelectId;
                Window1._ClientEngine.Send(NotifyType.Request_ReadNotice, Window1._BoardInfo);
                
            }
            else
            {
                for (int i = 0; i < Window1.noticeList.Count; i++)
                {
                    if (Window1.noticeList[i].Id == nSelectId)
                    {
                        Main.writeSend = new WriteSend();
                        Main.writeSend.title.Text = Window1.noticeList[i].Title;
                        Main.writeSend.contents.Text = Window1.noticeList[i].Content;
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

            if (sendData == null)
            {
                MessageBoxCommon.Show("请选择需要删除的题目", MessageBoxType.Ok);
                return;
            }

            if (MessageBoxCommon.Show("您确定要删除吗?", MessageBoxType.YesNo) == MessageBoxReply.No)
                return;

            Window1._BoardInfo.Id = sendData.id;
            if (Main.notice_board == true)
            {
                Window1._ClientEngine.Send(NotifyType.Request_DelNotice, Window1._BoardInfo);
            }
            else
            {
                Window1._ClientEngine.Send(NotifyType.Request_DelLetter, Window1._BoardInfo);
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
                MessageBoxCommon.Show("请选择需要删除的题目", MessageBoxType.Ok);
                return;
            }

            SendData selectedFile = (SendData)letterDataGrid.Items[letterDataGrid.SelectedIndex];
            nSelectId = selectedFile.id;


            if (Main.notice_board == true)
            {

                for (int i = 0; i < Window1._HomeInfo.Notices.Count; i++)
                {
                    if (Window1._HomeInfo.Notices[i].Id == nSelectId)
                    {
                        Main.writeSend = new WriteSend();
                        Main.writeSend.title.Text = Window1._HomeInfo.Notices[i].Title;
                        Main.writeSend.contents.Text = Window1._HomeInfo.Notices[i].Content;
                        Main.writeSend.title.IsReadOnly = true;
                        Main.writeSend.contents.IsReadOnly = true;
                        Main.writeSend.btFlag = false;
                        Main.writeSend.Show();

                    }
                }

                Window1._BoardInfo.Id = nSelectId;
                Window1._ClientEngine.Send(NotifyType.Request_ReadNotice, Window1._BoardInfo);

            }
            else
            {
                for (int i = 0; i < Window1.noticeList.Count; i++)
                {
                    if (Window1.noticeList[i].Id == nSelectId)
                    {
                        Main.writeSend = new WriteSend();
                        Main.writeSend.title.Text = Window1.noticeList[i].Title;
                        Main.writeSend.contents.Text = Window1.noticeList[i].Content;
                        Main.writeSend.title.IsReadOnly = true;
                        Main.writeSend.contents.IsReadOnly = true;
                        Main.writeSend.btFlag = false;
                        Main.writeSend.Show();

                    }
                }
            }
        }
        
    }
}
