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
    public partial class MngGame : Form
    {
        public MngGame()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            comboKind.Items.Add("游戏帐号");
            comboKind.Items.Add("名字");

            comboKind.SelectedIndex = 0;

            buttonAddRoom.Visible = false;
            buttonEditRoom.Visible = false;
            buttonDelRoom.Visible = false;

            RefreshGameList();
        }

        public void RefreshGameList()
        {
            string queryStr = "select * from tblGame ";

            string contentStr = textContent.Text.Trim();

            if (contentStr.Length > 0)
            {
                string field = "";

                switch (comboKind.SelectedIndex)
                {
                    case 0:
                        field += string.Format("GameId like '%{0}%'", contentStr);
                        break;

                    case 1:
                        field += string.Format("GameName like '%{0}%'", contentStr );
                        break;
                }

                if (field != "")
                    queryStr += string.Format(" where {0} ", field);
            }

            List<GameInfo> gameList = Database.GetInstance().GetGameList( queryStr );

            if (gameList == null)
            {
                MessageBox.Show("不能在资料库获取频道信息.");
                return;
            }

            GameView.Rows.Clear();

            for (int i = 0; i < gameList.Count; i++)
            {
                GameView.RowTemplate.Height = 60;

                GameInfo gameInfo = gameList[i];

                Bitmap face = null;
                try
                {
                    Image faceImage = Image.FromFile("\\\\127.0.0.1\\" + gameInfo.Icon);
                    face = new Bitmap(faceImage, new Size(70, 60));
                }
                catch
                {
                    face = null;
                }

                GameView.Rows.Add(
                    face,
                    gameInfo.GameId,
                    gameInfo.GameName,
                    gameInfo.Width,
                    gameInfo.Height,
                    gameInfo.Source
                    );

            }

            labelSummary.Text = string.Format("总游戏数量: {0}", gameList.Count);
        }



        private void buttonFind_Click(object sender, EventArgs e)
        {
            RefreshGameList();
        }

        private void buttonAddRoom_Click(object sender, EventArgs e)
        {
            AddRoom addRoom = new AddRoom();

            if (addRoom.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("聊天频道建立成功.");
                RefreshGameList();
            }
        }

        private void buttonEditRoom_Click(object sender, EventArgs e)
        {
            if (GameView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择聊天频道.");
                return;
            }

            string roomId = GameView.SelectedRows[0].Cells["IdColumn"].Value.ToString();

            AddRoom editRoom = new AddRoom();

            editRoom._IsEditMode = true;
            editRoom._RoomId = roomId;

            if (editRoom.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("频道设置成功.");
                RefreshGameList();
            }
        }

        private void buttonDelRoom_Click(object sender, EventArgs e)
        {
            if (GameView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需删除的频道.");
                return;
            }

            string roomId = GameView.SelectedRows[0].Cells["IdColumn"].Value.ToString();

            string question = string.Format("真要删除 {0} ?", roomId);

            if (MessageBox.Show(question, "删除提问", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (Database.GetInstance().DelRoom(roomId) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            string resultStr = string.Format(" {0} 频道已被删除.", roomId);
            MessageBox.Show(resultStr);

            RefreshGameList();
        }
    }
}
