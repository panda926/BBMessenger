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
    public partial class MngPercent : Form
    {
        public string _UserId = null;

        public MngPercent()
        {
            InitializeComponent();
        }

        private void MngPrivilege_Load(object sender, EventArgs e)
        {
            if (_UserId == null)
                return;

            UserInfo userInfo = Database.GetInstance().FindUser(_UserId);

            if (userInfo == null)
                return;

            textChatPercent.Text = userInfo.ChatPercent.ToString();
            textGamePercent.Text = userInfo.GamePercent.ToString();

            switch (userInfo.Kind)
            {
                case (int)UserKind.ServiceWoman:
                case (int)UserKind.ServiceOfficer:
                    textGamePercent.Enabled = false;
                    break;

                case (int)UserKind.Recommender:
                case (int)UserKind.RecommendOfficer:
                    textChatPercent.Enabled = false;
                    break;

                default:
                    textChatPercent.Enabled = false;
                    textGamePercent.Enabled = false;
                    break;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            int chatPercent = 0;
            int gamePercent = 0;

            try
            {
                chatPercent = Convert.ToInt32(textChatPercent.Text);
                gamePercent = Convert.ToInt32(textGamePercent.Text);
            }
            catch
            {
                MessageBox.Show("입력정보가 정확치 않습니다.");
                return;
            }

            UserInfo userInfo = Database.GetInstance().FindUser(_UserId);

            userInfo.ChatPercent = chatPercent;
            userInfo.GamePercent = gamePercent;

            if (Database.GetInstance().UpdateUser(userInfo) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
