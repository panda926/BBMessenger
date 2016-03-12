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
    public partial class AddRoom : Form
    {
        public bool _IsEditMode = false;
        public string _RoomId = "";

        public AddRoom()
        {
            InitializeComponent();
        }

        private void AddRoom_Load(object sender, EventArgs e)
        {
            List<UserInfo> userList = Database.GetInstance().GetServicemans();

            for (int i = 0; i < userList.Count; i++)
            {
                comboOwner.Items.Add(userList[i].Id);
            }

            if( comboOwner.Items.Count > 0 )
                comboOwner.SelectedIndex = 0;

            textCash.Text = "0";
            textPoint.Text = "0";
            textMaxUsers.Text = "0";

            if (_IsEditMode == true)
            {
                RoomInfo roomInfo = Database.GetInstance().FindRoom(_RoomId);

                if (roomInfo != null)
                {
                    textId.Text = roomInfo.Id;
                    textName.Text = roomInfo.Name;
                    textCash.Text = roomInfo.Cash.ToString();
                    textPoint.Text = roomInfo.Point.ToString();
                    textMaxUsers.Text = roomInfo.MaxUsers.ToString();

                    for (int i = 0; i < comboOwner.Items.Count; i++)
                    {
                        if (comboOwner.Items[i].ToString() == roomInfo.Owner)
                        {
                            comboOwner.SelectedIndex = i;
                            break;
                        }
                    }

                    textId.ReadOnly = true;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string roomId = textId.Text.Trim();

            if (roomId.Length == 0)
            {
                MessageBox.Show("请输入频道号码.");
                return;
            }

            if (_IsEditMode == false)
            {
                RoomInfo roomInfo = Database.GetInstance().FindRoom(roomId);

                if (roomInfo != null)
                {
                    MessageBox.Show("已存在的频道.");
                    return;
                }
            }

            string roomName = textName.Text.Trim();

            if (roomName.Length == 0)
            {
                MessageBox.Show("请输入频道名称.");
                return;
            }

            string roomOwner = comboOwner.Text.Trim();

            if (roomOwner.Length == 0)
            {
                MessageBox.Show("请选择频道所有者.");
                return;
            }

            if (textMaxUsers.Text.Trim().Length == 0)
            {
                MessageBox.Show("请选择频道人数.");
                return;
            }

            RoomInfo newInfo = new RoomInfo();

            newInfo.Id = roomId;
            newInfo.Name = roomName;
            newInfo.Kind = 0;
            newInfo.Owner = roomOwner;
            newInfo.Cash = BaseInfo.ConvToInt(textCash.Text);
            newInfo.Point = BaseInfo.ConvToInt(textPoint.Text);
            newInfo.MaxUsers = BaseInfo.ConvToInt(textMaxUsers.Text);

            bool ret = false;

            if (_IsEditMode == false)
                ret = Database.GetInstance().AddRoom(newInfo);
            else
                ret = Database.GetInstance().UpdateRoom(newInfo);

            if ( ret == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }


    }
}
