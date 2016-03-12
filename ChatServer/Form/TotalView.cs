using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChatEngine;

namespace ChatServer
{
    public partial class TotalView : Form
    {
        public TotalView()
        {
            InitializeComponent();
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        } 

        private void TotalView_Load(object sender, EventArgs e)
        {
        }

        public void RefreshTotal()
        {
            if (ResultView.Rows.Count <= 0)
            {
                ResultView.Rows.Add("日期", "");
                ResultView.Rows.Add("消费者", "");
                ResultView.Rows.Add("宝贝", "");
                ResultView.Rows.Add("聊天收益", "0");
                ResultView.Rows.Add("游戏收益", "0");
                ResultView.Rows.Add("元宝充值", "0");
                ResultView.Rows.Add("元宝兑换", "0");
            }

            List<UserInfo> userList = Users.GetInstance().GetUsers();

            int buyerCount = 0;
            int servicemanCount = 0;

            foreach (UserInfo userInfo in userList)
            {
                if (userInfo.Kind == (int)UserKind.ServiceWoman)
                    servicemanCount++;
                else
                    buyerCount++;
            }

            Database database = Database.GetInstance();

            ResultView.Rows[0].Cells[1].Value = string.Format("{0}", DateTime.Now.ToString("yyyy/MM/dd"));
            ResultView.Rows[1].Cells[1].Value = string.Format("{0} 名", buyerCount);
            ResultView.Rows[2].Cells[1].Value = string.Format("{0} 名", servicemanCount);
            ResultView.Rows[3].Cells[1].Value = string.Format("{0}", -database.SumChatHistory(string.Empty, DateTime.Now));
            ResultView.Rows[4].Cells[1].Value = string.Format("{0}", -database.SumGameHistory(string.Empty, DateTime.Now));
            ResultView.Rows[5].Cells[1].Value = string.Format("{0}", database.SumChargeHistory(null, DateTime.Now, (int)ChargeKind.Charge));
            //ResultView.Rows[6].Cells[1].Value = string.Format("{0}", database.SumChargeHistory(null, DateTime.Now, (int)ChargeKind.Discharge));
        }
    }
}
