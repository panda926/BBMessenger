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
    public partial class Loginuser : Form
    {
        public Loginuser()
        {
            InitializeComponent();
        }

        private void Loginuser_Load(object sender, EventArgs e)
        {
            string configPath = System.IO.Path.GetFullPath("Config.ini");

            IniFileEdit configInfo = new IniFileEdit(configPath);

            Database._IP = configInfo.GetIniValue("Database", "IP");
            Users.WebResourceUrl = configInfo.GetIniValue("Web", "Resource");

            // server initialize
            string port = configInfo.GetIniValue("Chatting", "TcpPort");

            Server server = Server.GetInstance();

            server.AttachHandler(Users.GetInstance().NotifyOccured);
            server.AttachHandler(Chat.GetInstance().NotifyOccured);
            server.AttachHandler(Game.GetInstance().NotifyOccured);

            server.Connect("127.0.0.1", port);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string userID = textUserID.Text.Trim();
            string password = textPassword.Text.Trim();

            if ( userID == string.Empty)
            {
                MessageBox.Show("输入的资料不准确.");
                return;
            }

            UserInfo userInfo = Database.GetInstance().FindUser(userID);

            if (userInfo == null)
            {
                MessageBox.Show("没登记的帐号.");
                return;
            }
            else if (userInfo.Password != password)
            {
                MessageBox.Show("密码错误.");
                return;
            }

            switch (userInfo.Kind)
            {
                case (int)UserKind.Banker:
                case (int)UserKind.Manager:
                case (int)UserKind.ServiceOfficer:
                case (int)UserKind.RecommendOfficer:
                    break;

                default:
                    MessageBox.Show("没允许的会员.");
                    return;
            }
            Users.ManagerInfo = userInfo;


            userInfo.LoginTime = BaseInfo.ConvDateToString(DateTime.Now); ;
            Database.GetInstance().UpdateUser(userInfo);

            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


    }
}
