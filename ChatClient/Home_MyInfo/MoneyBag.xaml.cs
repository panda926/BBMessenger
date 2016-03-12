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
using System.Windows.Shapes;
using ChatEngine;
using System.Net.Sockets;

using ControlExs;

namespace ChatClient
{
	/// <summary>
	/// MyHome.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MoneyBag : BaseWindow
	{
        public List<ChatHistoryInfo> chatHistoryList = null;

        public MoneyBag()
		{
			this.InitializeComponent();
            // 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.

		}
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Login._ClientEngine.AttachHandler(NotifyOccured);

            listBox1.SelectedIndex = 0;            
            gridCommon.Children.Clear();

            gridRight.Visibility = Visibility.Hidden;
            backGrid.Visibility = Visibility.Visible;            
            InnerControl innerControl = new InnerControl();
            innerControl.InnerChatting(chatHistoryList);            
            backGrid.Children.Clear();
            backGrid.Children.Add(innerControl);

            //int nCash = Login._UserInfo.Cash;
            //int nChangeMoney = nCash / 100 * 90;

            //this.txtCashAmount.Content = nCash.ToString();
            //this.txtChangingMoney.Content = nChangeMoney.ToString();

            //BankAccountReg wndBankAccountReg = new BankAccountReg();
            //wndBankAccountReg.InitAllControlValues(false);
            //gridCommon.Children.Add(wndBankAccountReg);
        }
        public void InitTabSetting(UserInfo userInfo)
        {
            ImageBrush faceImage = new ImageBrush();
            faceImage.Stretch = Stretch.Fill;
            faceImage.ImageSource = ImageDownloader.GetInstance().GetImage(userInfo.Icon);
            myPicture.Fill = faceImage;
            myPicture.Cursor = Cursors.Hand;

            //ListBoxItem item1 = new ListBoxItem();
            //item1.Content = "银行信息";
            //item1.MouseUp += new MouseButtonEventHandler(item1_MouseUp);
            ListBoxItem item2 = new ListBoxItem();
            item2.FocusVisualStyle = null;
            item2.Content = "消费记录";
            item2.MouseUp += new MouseButtonEventHandler(item2_MouseUp);
            //ListBoxItem item3 = new ListBoxItem();
            //item3.Content = "结    算";
            //item3.MouseUp += new MouseButtonEventHandler(item3_MouseUp);
            ListBoxItem item4 = new ListBoxItem();
            item4.FocusVisualStyle = null;
            item4.Content = "积分兑换";
            item4.MouseUp += new MouseButtonEventHandler(item4_MouseUp);


            //listBox1.Items.Add(item1);
            listBox1.Items.Add(item2);
            //listBox1.Items.Add(item3);
            listBox1.Items.Add(item4);
        }

        void item3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            gridRight.Visibility = Visibility.Visible;
            backGrid.Visibility = Visibility.Hidden;
            gridCommon.Children.Clear();
            Payment wndPayment = new Payment();
            wndPayment.InitAllValues();
            gridCommon.Children.Add(wndPayment);
        }

        void item2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            gridRight.Visibility = Visibility.Hidden;
            backGrid.Visibility = Visibility.Visible;
            //gridCommon.Children.Clear();
            InnerControl innerControl = new InnerControl();
            innerControl.InnerChatting(chatHistoryList);
            //gridCommon.Children.Add(innerControl);
            backGrid.Children.Clear();
            backGrid.Children.Add(innerControl);
        }

        void item4_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                gridRight.Visibility = Visibility.Hidden;
                backGrid.Visibility = Visibility.Visible;
                ChatClient.Home_MyInfo.ConvPointToCash convPointToCash = new Home_MyInfo.ConvPointToCash();
                

                backGrid.Children.Clear();
                backGrid.Children.Add(convPointToCash);
            }
            catch (Exception)
            { }
        }

        void item1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            gridRight.Visibility = Visibility.Visible;
            backGrid.Visibility = Visibility.Hidden;
            gridCommon.Children.Clear();
            BankAccountReg wndBankAccountReg = new BankAccountReg();
            wndBankAccountReg.InitAllControlValues(false);
            gridCommon.Children.Add(wndBankAccountReg);
        }
        
        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_UpdateUser:
                    {
                        UserInfo userName = (UserInfo)baseInfo;
                        Login._UserInfo.Point = userName.Point;

                        gridRight.Visibility = Visibility.Hidden;
                        backGrid.Visibility = Visibility.Visible;
                        ChatClient.Home_MyInfo.ConvPointToCash convPointToCash = new Home_MyInfo.ConvPointToCash();

                        backGrid.Children.Clear();
                        backGrid.Children.Add(convPointToCash);

                        TempWindowForm tempWindowForm = new TempWindowForm();
                        QQMessageBox.Show(tempWindowForm, "更新成功", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                    }
                    break;

                case NotifyType.Reply_UserInfo:
                    {
                        Login._UserInfo = (UserInfo)baseInfo;
                        this.txtCashAmount.Content = Login._UserInfo.Cash.ToString();
                        this.txtChangingMoney.Content = (Login._UserInfo.Cash / 100 * 90).ToString();
                    }
                    break;
            }
        }
        

        private void Window_Closed(object sender, EventArgs e)
        {
            Login.myhome = null;
            Login._ClientEngine.DetachHandler(NotifyOccured);
            this.Close();
        }

        private void myPicture_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        
       
	}
}