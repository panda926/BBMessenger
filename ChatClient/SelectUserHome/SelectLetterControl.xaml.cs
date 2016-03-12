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
        public SelectLetterControl()
		{
			this.InitializeComponent();
		}
        public void markInfo(UserDetailInfo userDetail)
        {

            markPan.Children.Clear();

            DataGrid moneyGrid = new DataGrid();
            DataGridTextColumn col1 = new DataGridTextColumn();
            DataGridTextColumn col2 = new DataGridTextColumn();
            DataGridTextColumn col3 = new DataGridTextColumn();

            moneyGrid.Columns.Add(col1);
            moneyGrid.Columns.Add(col2);
            moneyGrid.Columns.Add(col3);


            col1.Binding = new Binding("buyId");
            col2.Binding = new Binding("value");
            col3.Binding = new Binding("time");

            if (userDetail != null)
            {
                for (int j = 0; j < userDetail.EvalHistories.Count; j++)
                {

                    moneyGrid.Items.Add(new EvalData
                    {
                        buyId = userDetail.EvalHistories[j].BuyerId,
                        value = userDetail.EvalHistories[j].Value.ToString(),
                        time = userDetail.EvalHistories[j].EvalTime.ToString()
                    });

                }
            }
            col1.Header = "고객아이디";
            col2.Header = "평점";
            col3.Header = "날자";


            markPan.Children.Add(moneyGrid);
        }
        public struct EvalData
        {
            public string buyId { set; get; }
            public string value { set; get; }
            public string time { set; get; }
        }
	}
}