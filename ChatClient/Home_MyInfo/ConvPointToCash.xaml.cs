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

using ControlExs;
using ChatEngine;

namespace ChatClient.Home_MyInfo
{
    /// <summary>
    /// Interaction logic for ConvPointToCash.xaml
    /// </summary>
    public partial class ConvPointToCash : UserControl
    {
        public ConvPointToCash()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtUserPoint.Text = Login._UserInfo.Point.ToString();
            if (Login._UserInfo.Point < 200000)
            {
                this.txtInputPointAmount.IsEnabled = false;
                this.btnConvert.IsEnabled = false;
            }
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nPoint = Convert.ToInt32(this.txtInputPointAmount);
                if (nPoint > Login._UserInfo.Point)
                {
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "结算金额有误请重新输入", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);

                    this.txtInputPointAmount.Text = "0";
                    this.txtInputPointAmount.Focus();

                    return;
                }

                Login._UserInfo.Point = Login._UserInfo.Point - nPoint;
                Login._UserInfo.Cash = Login._UserInfo.Cash + nPoint / 100000;

                Login._ClientEngine.Send(NotifyType.Request_UpdateUser, Login._UserInfo);                
            }
            catch (Exception)
            { }
        }
    }
}
