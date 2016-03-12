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
using System.Windows.Threading;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for CheckUserList.xaml
    /// </summary>
    public partial class CheckUserList : BaseWindow
    {
        List<string> selectUserNameList = new List<string>();

        Microsoft.Windows.Controls.DataGrid userGrid = null;

        public CheckUserList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                stackPanel1.Children.Clear();

                userGrid = new Microsoft.Windows.Controls.DataGrid();
                
                userGrid.Height = 220;
                //userGrid.MouseDown += new MouseButtonEventHandler(userGrid_MouseClick);
                userGrid.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(userGrid_MouseClick);
                userGrid.SelectionUnit = Microsoft.Windows.Controls.DataGridSelectionUnit.Cell;
                userGrid.SelectionMode = Microsoft.Windows.Controls.DataGridSelectionMode.Extended;

                userGrid.HeadersVisibility = Microsoft.Windows.Controls.DataGridHeadersVisibility.Column;
                userGrid.GridLinesVisibility = Microsoft.Windows.Controls.DataGridGridLinesVisibility.Horizontal;

                userGrid.IsReadOnly = true;
                userGrid.Style = (Style)FindResource("DataGridStyle");
                userGrid.CellStyle = (Style)FindResource("cellStyle");
                userGrid.RowStyle = (Style)FindResource("rowStyle");
                userGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
                userGrid.Background = new SolidColorBrush(Colors.Transparent);

                userGrid.SelectionMode = Microsoft.Windows.Controls.DataGridSelectionMode.Single;
                userGrid.SelectionUnit = Microsoft.Windows.Controls.DataGridSelectionUnit.FullRow;

                Microsoft.Windows.Controls.DataGridCheckBoxColumn col1 = new Microsoft.Windows.Controls.DataGridCheckBoxColumn();
                col1.Width = 48;
                Microsoft.Windows.Controls.DataGridTextColumn col2 = new Microsoft.Windows.Controls.DataGridTextColumn();
                col2.Width = 130;
                Microsoft.Windows.Controls.DataGridTextColumn col3 = new Microsoft.Windows.Controls.DataGridTextColumn();
                //col3.Width = new Microsoft.Windows.Controls.DataGridLength(115, Microsoft.Windows.Controls.DataGridLengthUnitType.Star);
                col3.Width = 115;

                userGrid.Columns.Add(col1);
                userGrid.Columns.Add(col2);
                userGrid.Columns.Add(col3);

                col1.Binding = new Binding("check");
                col2.Binding = new Binding("id");
                col3.Binding = new Binding("nickname");



                for (int j = 0; j < Login.userList.Count; j++)
                {
                    CheckBox chk = new CheckBox();
                    chk.IsChecked = false;
                    userGrid.Items.Add(new ChargeData
                    {
                        check = chk,
                        id = Login.userList[j].Id,
                        nickname = Login.userList[j].Nickname
                    });

                }

                col1.Header = "";
                col2.Header = "id";
                col3.Header = "niakName";                

                stackPanel1.Children.Add(userGrid);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        void userGrid_MouseClick(object sender, MouseButtonEventArgs e)
        {           
            try
            {
                IInputElement element = e.MouseDevice.DirectlyOver;
                if (element != null && element is FrameworkElement)
                {
                    if (((FrameworkElement)element).Parent is Microsoft.Windows.Controls.DataGridCell)
                    {
                        var grid = sender as Microsoft.Windows.Controls.DataGrid;
                        if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                        {
                            for (int i = 0; i < grid.Columns.Count; i++)
                            {
                                var item = new Microsoft.Windows.Controls.DataGridCellInfo(grid.Items[grid.SelectedIndex], grid.Columns[0]);
                                var col = item.Column as Microsoft.Windows.Controls.DataGridColumn;
                                var fc = col.GetCellContent(item.Item);

                                if (fc is CheckBox)
                                {
                                    CheckBox checkCell = (CheckBox)fc;
                                    if (checkCell != null && checkCell.IsChecked == false)
                                    {
                                        checkCell.IsChecked = true;
                                        var addItemId = (ChargeData)item.Item;
                                        selectUserNameList.Add(addItemId.id);
                                    }
                                    else if (checkCell != null && checkCell.IsChecked == true)
                                    {
                                        checkCell.IsChecked = false;
                                        var delItemId = (ChargeData)item.Item;
                                        selectUserNameList.Remove(delItemId.id);
                                    }
                                }
                            }
                            if (selectUserNameList.Count > 0)
                                button1.IsEnabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }



        public class ChargeData
        {
            public CheckBox check { set; get; }
            public string id { set; get; }
            public string nickname { set; get; }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string str = null;
            for (int i = 0; i < selectUserNameList.Count; i++)
            {
                str += selectUserNameList[i] + ",";
            }
            Main.writeSend.sendName.Text = str;
            this.Close();
        }

        private void cancelBt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
