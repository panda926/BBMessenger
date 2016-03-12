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
    public partial class MngRoom : Form
    {
        public MngRoom()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            comboKind.Items.Add("频道号码");
            comboKind.Items.Add("名字");
            comboKind.Items.Add("创建者");

            comboKind.SelectedIndex = 0;

            RefreshRoomList();
        }

        public void RefreshRoomList()
        {
            string queryStr = "select * from tblRoom ";

            string contentStr = textContent.Text.Trim();

            if (contentStr.Length > 0)
            {
                string field = "";

                switch (comboKind.SelectedIndex)
                {
                    case 0:
                        field += string.Format("Id like '%{0}%'", contentStr);
                        break;

                    case 1:
                        field += string.Format("Name like '%{0}%'", contentStr );
                        break;

                    case 2:
                        field += string.Format("Owner like '%{0}%'", contentStr);
                        break;
                }

                if (field != "")
                    queryStr += string.Format(" where {0} ", field);
            }

            List<RoomInfo> roomList = Database.GetInstance().GetRoomList( queryStr );

            if (roomList == null)
            {
                MessageBox.Show("不能在资料库获取频道信息.");
                return;
            }

            RoomView.Rows.Clear();

            for (int i = 0; i < roomList.Count; i++)
            {
                RoomInfo roomInfo = roomList[i];

                RoomView.Rows.Add(
                    roomInfo.Id,
                    roomInfo.Name,
                    roomInfo.MaxUsers,
                    roomInfo.Owner,
                    roomInfo.Cash,
                    roomInfo.Point
                    );

                if (roomInfo.Cash == 0 && roomInfo.Point == 0 ||
                    roomInfo.Cash > 0 && roomInfo.Point > 0 )
                    RoomView.Rows[RoomView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;
            }

            labelSummary.Text = string.Format("总数: {0}", roomList.Count);
        }



        private void buttonFind_Click(object sender, EventArgs e)
        {
            RefreshRoomList();
        }

        private void buttonAddRoom_Click(object sender, EventArgs e)
        {
            AddRoom addRoom = new AddRoom();

            if (addRoom.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功.");
                RefreshRoomList();
            }
        }

        private void buttonEditRoom_Click(object sender, EventArgs e)
        {
            if (RoomView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择频道.");
                return;
            }

            string roomId = RoomView.SelectedRows[0].Cells["IdColumn"].Value.ToString();

            AddRoom editRoom = new AddRoom();

            editRoom._IsEditMode = true;
            editRoom._RoomId = roomId;

            if (editRoom.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功.");
                RefreshRoomList();
            }
        }

        private void buttonDelRoom_Click(object sender, EventArgs e)
        {
            if (RoomView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择频道.");
                return;
            }

            string roomId = RoomView.SelectedRows[0].Cells["IdColumn"].Value.ToString();

            string question = string.Format("真要删除 {0} 吗?", roomId);

            if (MessageBox.Show(question, "删除提问", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (Database.GetInstance().DelRoom(roomId) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            string resultStr = string.Format(" {0} 已被删除.", roomId);
            MessageBox.Show(resultStr);

            RefreshRoomList();
        }
    }
}
