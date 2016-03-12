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
    public partial class CheckUserList : Window
    {
        List<string> selectUserNameList = new List<string>();

        DataGrid userGrid = null;

        public CheckUserList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stackPanel1.Children.Clear();

            userGrid = new DataGrid();
            userGrid.Height = 220;
            userGrid.MouseDoubleClick += new MouseButtonEventHandler(userGrid_MouseDoubleClick);
            userGrid.SelectionUnit = DataGridSelectionUnit.Cell;
            userGrid.SelectionMode = DataGridSelectionMode.Extended;

            userGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            userGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;

            userGrid.IsReadOnly = true;
            userGrid.CellStyle = (Style)FindResource("cellStyle");
            userGrid.RowStyle = (Style)FindResource("rowStyle");
            userGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            userGrid.Background = new SolidColorBrush(Colors.Transparent);

            userGrid.SelectionMode = DataGridSelectionMode.Single;
            userGrid.SelectionUnit = DataGridSelectionUnit.FullRow;

            DataGridCheckBoxColumn col1 = new DataGridCheckBoxColumn();
            col1.Width = 48;
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Width = 115;
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Width = new DataGridLength(115, DataGridLengthUnitType.Star);
            
            userGrid.Columns.Add(col1);
            userGrid.Columns.Add(col2);
            userGrid.Columns.Add(col3);

            col1.Binding = new Binding("check");
            col2.Binding = new Binding("id");
            col3.Binding = new Binding("nickname");
            

            
            for (int j = 0; j < Window1.userList.Count; j++)
            {
                CheckBox chk = new CheckBox();
                userGrid.Items.Add(new ChargeData
                {
                    check = chk,
                    id = Window1.userList[j].Id,
                    nickname = Window1.userList[j].Nickname
                });

            }
            
            col1.Header = "";
            col2.Header = "id";
            col3.Header = "niakName";

            stackPanel1.Children.Add(userGrid);
        }

        void userGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IInputElement element = e.MouseDevice.DirectlyOver;
            if (element != null && element is FrameworkElement)
            {
                if (((FrameworkElement)element).Parent is DataGridCell)
                {
                    var grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        for (int i = 0; i < grid.Columns.Count; i++)
                        {
                            var item = new DataGridCellInfo(grid.Items[grid.SelectedIndex], grid.Columns[0]);
                            var col = item.Column as DataGridColumn;
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

        
    }
}
