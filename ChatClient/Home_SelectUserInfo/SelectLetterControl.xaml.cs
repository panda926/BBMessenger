using System;
using System.Collections.Generic;
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
using Microsoft.Windows.Controls;

namespace ChatClient
{
	/// <summary>
	/// LetterControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class SelectLetterControl : UserControl
	{
        string contents = null;
        int count = 0;

        public SelectLetterControl()
		{
			this.InitializeComponent();
		}

        public void markInfo(UserDetailInfo userDetail)
        {
            try
            {
                DisplayLabel(userDetail.EvalHistories);
                markPan.Children.Clear();

                Microsoft.Windows.Controls.DataGrid moneyGrid = new Microsoft.Windows.Controls.DataGrid();
                moneyGrid.Height = 300;
                moneyGrid.HeadersVisibility = Microsoft.Windows.Controls.DataGridHeadersVisibility.Column;
                moneyGrid.GridLinesVisibility = Microsoft.Windows.Controls.DataGridGridLinesVisibility.Horizontal;

                moneyGrid.IsReadOnly = true;
                moneyGrid.Style = (Style)FindResource("DataGridStyle");
                moneyGrid.CellStyle = (Style)FindResource("cellStyle");
                moneyGrid.RowStyle = (Style)FindResource("rowStyle");
                moneyGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
                moneyGrid.Background = new SolidColorBrush(Colors.Transparent);

                moneyGrid.SelectionMode = Microsoft.Windows.Controls.DataGridSelectionMode.Single;
                moneyGrid.SelectionUnit = Microsoft.Windows.Controls.DataGridSelectionUnit.FullRow;
                moneyGrid.LoadingRow += dataGrid_LoadingRow;

                Microsoft.Windows.Controls.DataGridTextColumn col1 = new Microsoft.Windows.Controls.DataGridTextColumn();
                col1.Width = 130;
                Microsoft.Windows.Controls.DataGridTextColumn col2 = new Microsoft.Windows.Controls.DataGridTextColumn();
                col2.Width = 130;
                Microsoft.Windows.Controls.DataGridTextColumn col3 = new Microsoft.Windows.Controls.DataGridTextColumn();
                col3.Width = new Microsoft.Windows.Controls.DataGridLength(133, Microsoft.Windows.Controls.DataGridLengthUnitType.Star);

                moneyGrid.Columns.Add(col1);
                moneyGrid.Columns.Add(col2);
                moneyGrid.Columns.Add(col3);


                col1.Binding = new Binding("buyId");
                col2.Binding = new Binding("content");
                col3.Binding = new Binding("time");

                if (userDetail != null)
                {
                    for (int j = 0; j < userDetail.EvalHistories.Count; j++)
                    {

                        if (userDetail.EvalHistories[j].Value == 0)
                            contents = "不好";
                        else
                            contents = "好";

                        moneyGrid.Items.Add(new EvalData
                        {
                            buyId = userDetail.EvalHistories[j].BuyerId,
                            content = contents,
                            time = userDetail.EvalHistories[j].EvalTime.ToString("yy/MM/dd HH:mm"),
                            value = userDetail.EvalHistories[j].Value
                        });

                    }
                }
                col1.Header = "会员账号";
                col2.Header = "评价";
                col3.Header = "日期";


                markPan.Children.Add(moneyGrid);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        public class EvalData
        {
            public string buyId { set; get; }
            public string content { set; get; }
            public string time { set; get; }
            public int value { set; get; }
        }

        void dataGrid_LoadingRow(object sender, Microsoft.Windows.Controls.DataGridRowEventArgs e)
        {
            // Get the DataRow corresponding to the DataGridRow that is loading.
            EvalData item = e.Row.Item as EvalData;
            if (item != null)
            {
                if (item.value == 1)
                    e.Row.Foreground = new SolidColorBrush(Colors.HotPink);
            }
        }

        private void DisplayLabel(List<EvalHistoryInfo> evalHistoryListResult)
        {
            label2.Content = 0;
            label4.Content = 0;
            label6.Content = 0;
            label8.Content = 0;

            label2.Content = evalHistoryListResult.Count;
            for (int i = 0; i < evalHistoryListResult.Count; i++)
            {
                if (evalHistoryListResult[i].Value == 1)
                {
                    ++count;
                }
            }
            label4.Content = count;
            label6.Content = evalHistoryListResult.Count - count;
            if (count != 0 && evalHistoryListResult.Count != 0)
            {
                label8.Content = (int)(count / Convert.ToDouble(evalHistoryListResult.Count) * 100) + "%";
            }
            else
                label8.Content = 0 + "%";
        }
	}
}