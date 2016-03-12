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
    public partial class DelAuto : Form
    {
        public int _PossibleCount = 0;

        public DelAuto()
        {
            InitializeComponent();
        }

        private void AddAuto_Load(object sender, EventArgs e)
        {
            List<RoomInfo> roomList = Chat.GetInstance().GetRooms();

            comboRoom.Items.Add("不使用");
            comboRoom.Items.Add("选择频道");

            foreach (RoomInfo roomInfo in roomList)
                comboRoom.Items.Add(roomInfo.Id);

            comboRoom.SelectedIndex = 1;

            List<GameTable> gameList = Game.GetInstance().GetGameTables();

            comboGame.Items.Add("不使用");
            comboGame.Items.Add("选择频道");

            foreach (GameTable gameTable in gameList)
                comboGame.Items.Add(gameTable._GameInfo.GameId);

            comboGame.SelectedIndex = 1;

            _PossibleCount = Auto.GetInstance().GetLiveCount();

            textAutoNum.Text = _PossibleCount.ToString();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            int addCount = 0;

            try
            {
                addCount = Convert.ToInt32(textAutoNum.Text);
            }
            catch { }

            if (addCount <= 0 || addCount > _PossibleCount )
            {
                MessageBox.Show("请输入准确的数量.");
                return;
            }

            List<UserInfo> autoList = Auto.GetInstance()._Autos;

            string roomId = null;
            string gameId = null;

            if (comboRoom.SelectedIndex > 0)
            {
                if (comboRoom.SelectedIndex > 1)
                    roomId = comboRoom.Text;
            }

            if( comboGame.SelectedIndex > 0 )
            {
                if( comboGame.SelectedIndex > 1 )
                    gameId = comboGame.Text;
            }

            int autoIndex = 0;

            for( int i = 0; i < addCount; i++ )
            {
                if (autoList.Count <= 0)
                    break;

                UserInfo userInfo = autoList[autoIndex];

                if (roomId != null && userInfo.RoomId != roomId)
                {
                    autoIndex++;
                    continue;
                }

                if (gameId != null && userInfo.GameId != gameId)
                {
                    autoIndex++;
                    continue;
                }

                Auto.GetInstance().OutAuto(userInfo);
            }


            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        
    }
}
