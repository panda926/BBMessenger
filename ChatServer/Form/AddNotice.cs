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
    public partial class AddNotice : Form
    {
        public bool _IsEditMode = false;
        
        public BoardInfo _NoticeInfo = new BoardInfo();

        public AddNotice()
        {
            InitializeComponent();
  
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string Title = textTitle.Text.Trim();

            if (Title.Length == 0)
            {
                MessageBox.Show("请输入标题.");
                return;
            }

            string Content = textContent.Text.Trim();

            if (Content.Length == 0)
            {
                MessageBox.Show("请输入内容.");
                return;
            }

            _NoticeInfo.Kind = (int)BoardKind.Notice;
            _NoticeInfo.Title = Title;
            _NoticeInfo.Content = Content;
            _NoticeInfo.UserId = Users.ManagerInfo.Id;
            _NoticeInfo.SendId = "alluser";

 
            if( _IsEditMode == false )
            {
                List<UserInfo> userList = null;

                switch( comboKind.SelectedIndex )
                {
                    case 0:
                        userList = Database.GetInstance().GetAllUsers();
                        _NoticeInfo.UserKind = 0;
                        break;
                    case 1:
                        userList = Database.GetInstance().GetRecommenders();
                        _NoticeInfo.UserKind = 1;
                        break; 
                    case 2:
                        userList = Database.GetInstance().GetServicemans();
                        _NoticeInfo.UserKind = 2;
                        break;
                }

                // send message to all users from admin
                // 2014-02-14: GreenRose
                _NoticeInfo.Kind = (int)BoardKind.AdminNotice;

                if( userList == null )
                {
                    ErrorInfo errorInfo = BaseInfo.GetError();
                    MessageBox.Show(errorInfo.ErrorString);
                    return;
                }

                if( comboKind.SelectedIndex > 0 )
                    userList.Add(Users.ManagerInfo);

                HomeInfo homeInfo = new HomeInfo();

                Database.GetInstance().AddBoard(_NoticeInfo);

                //foreach( UserInfo userInfo in userList )
                //{
                //    if (userInfo.Auto == 1)
                //        continue;
                    
                //    _NoticeInfo.SendId = userInfo.Id; 
                //    bool ret = Database.GetInstance().AddBoard(_NoticeInfo);

                //    if (ret == true)
                //    {
                //        homeInfo.Notices = Database.GetInstance().GetNotices(userInfo.Id);

                //        if (homeInfo.Notices != null)
                //        {
                //            UserInfo liveUser = Users.GetInstance().FindUser(userInfo.Id);

                //            if (liveUser != null)
                //                Server.GetInstance().Send(liveUser.Socket, NotifyType.Reply_NoticeList, homeInfo);
                //        }
                //    }
                //}

            }

            this.DialogResult = DialogResult.OK;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            comboKind.Items.Add("总用户");
            comboKind.Items.Add("广告员");
            comboKind.Items.Add("服务员");

            comboKind.SelectedIndex = 0;

            if (_IsEditMode == true)
            {
                this.Text = "读公告";
                buttonOk.Text = "关闭";

                if (_NoticeInfo != null)
                {
                    textTitle.Text = _NoticeInfo.Title;
                    textTitle.Enabled = false;

                    textContent.Text = _NoticeInfo.Content;
                    textContent.Enabled = false;

                    comboKind.Enabled = false;
                    comboKind.SelectedIndex = _NoticeInfo.UserKind;

                    if (_NoticeInfo.Kind == (int)BoardKind.Letter)
                    {
                        labelKind.Visible = false;
                        comboKind.Visible = false;
                    }
                }
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
