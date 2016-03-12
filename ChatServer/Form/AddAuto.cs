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
    public partial class AddAuto : Form
    {
        public int _PossibleCount = 0;

        public AddAuto()
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

            List<GameInfo> gameList = Database.GetInstance().GetAllGames();

            comboGame.Items.Add("不使用");
            comboGame.Items.Add("选择频道");

            //foreach (GameInfo gameInfo in gameList)
            //    comboGame.Items.Add(gameInfo.GameId);
            // modified by usc at 2014/02/21
            foreach (GameInfo gameInfo in gameList)
            {
                comboGame.Items.Add(gameInfo.GameId);

                int nPointGameId = Convert.ToInt32(gameInfo.GameId) + 1;

                comboGame.Items.Add(nPointGameId.ToString());
            }

            comboGame.SelectedIndex = 1;

            int liveCount = Auto.GetInstance().GetLiveCount();
            int autoCount = Auto.GetInstance().GetAutoCount();

            _PossibleCount = autoCount - liveCount;

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

            if (addCount <= 0 || addCount > _PossibleCount)
            {
                MessageBox.Show("请输入准确数量.");
                return;
            }

            lock (Server.GetInstance()._objLockMain)
            {
                List<UserInfo> autoList = Database.GetInstance().GetAutoList();
                Random random = new Random();

                if (comboRoom.SelectedIndex > 0)
                {
                    List<RoomInfo> roomList = new List<RoomInfo>();

                    if (comboRoom.SelectedIndex > 1)
                        roomList.Add(Chat.GetInstance().FindRoom(comboRoom.Text));
                    else
                        roomList = Database.GetInstance().GetAllRooms();
                    //roomList = Chat.GetInstance().GetRooms();

                    foreach (RoomInfo roomInfo in roomList)
                    {
                        if (roomInfo.MaxUsers <= 0)
                            continue;

                        int roomAddCount = 0;

                        if (roomList.Count == 1)
                        {
                            roomAddCount = addCount;

                            if (roomAddCount > roomInfo.MaxUsers - roomInfo.UserCount)
                                roomAddCount = roomInfo.MaxUsers - roomInfo.UserCount;
                        }
                        else
                        {
                            roomAddCount = random.Next() % (roomInfo.MaxUsers - roomInfo.UserCount);

                            if (roomAddCount > addCount)
                                roomAddCount = addCount;
                        }


                        bool isEmpty = false;

                        if (roomInfo.UserCount == 0)
                            isEmpty = true;

                        while (roomAddCount > 0 && autoList.Count > 0)
                        {
                            UserInfo userInfo = autoList[0];

                            if (isEmpty == true && userInfo.Kind != (int)UserKind.ServiceWoman)
                                break;

                            autoList.RemoveAt(0);

                            if (Auto.GetInstance().GetAuto(userInfo.Id) != null)
                                continue;

                            Auto.GetInstance().AddAuto(roomInfo, userInfo);

                            roomAddCount--;
                            addCount--;
                        }

                        if (addCount <= 0)
                            break;

                        if (autoList.Count <= 0)
                            break;
                    }
                }



                if (comboGame.SelectedIndex > 0)
                {
                    List<GameInfo> gameList = new List<GameInfo>();

                    //if (comboGame.SelectedIndex > 1)
                    //    gameList.Add(Database.GetInstance().FindGame(comboGame.Text));
                    //else
                    //    gameList = Database.GetInstance().GetAllGames();
                    if (comboGame.SelectedIndex > 1)
                    {
                        // modified by usc at 2014/02/23
                        int nGameId = 0;

                        if (comboGame.SelectedIndex % 2 == 0)
                            nGameId = Convert.ToInt32(comboGame.Text);      // cash game
                        else
                            nGameId = Convert.ToInt32(comboGame.Text) - 1;  // point game

                        gameList.Add(Database.GetInstance().FindGame(nGameId.ToString()));
                    }
                    else
                        gameList = Database.GetInstance().GetAllGames();


                    foreach (GameInfo gameInfo in gameList)
                    {
                        int gameAddCount = random.Next() % 5 + 5;

                        if (gameList.Count == 1)
                            gameAddCount = addCount;

                        if (gameAddCount > addCount)
                            gameAddCount = addCount;

                        while (gameAddCount > 0 && autoList.Count > 0)
                        {
                            int nCashOrPoint = 0;

                            UserInfo userInfo = autoList[0];
                            autoList.RemoveAt(0);

                            if (Auto.GetInstance().GetAuto(userInfo.Id) != null)
                                continue;

                            // added by usc at 2014/02/23
                            if (comboGame.SelectedIndex > 1)
                                nCashOrPoint = comboGame.SelectedIndex % 2;
                            else
                                nCashOrPoint = random.Next() % 2;

                            if (nCashOrPoint == 0)
                            {
                                gameInfo.nCashOrPointGame = 0;

                                Auto.GetInstance().AddAuto(gameInfo, userInfo, true);
                            }
                            else
                            {
                                GameInfo pointGameInfo = new GameInfo();

                                pointGameInfo._NotifyHandler = gameInfo._NotifyHandler;
                                pointGameInfo.Bank = gameInfo.Bank;
                                pointGameInfo.Commission = gameInfo.Commission;
                                pointGameInfo.GameId = (Convert.ToInt32(gameInfo.GameId) + 1).ToString();
                                pointGameInfo.GameName = gameInfo.GameName;
                                pointGameInfo.Height = gameInfo.Height;
                                pointGameInfo.Icon = gameInfo.Icon;
                                pointGameInfo.nCashOrPointGame = 1;
                                pointGameInfo.Source = gameInfo.Source;
                                pointGameInfo.UserCount = gameInfo.UserCount;
                                pointGameInfo.Width = gameInfo.Width;

                                Auto.GetInstance().AddAuto(pointGameInfo, userInfo, true);
                            }

                            gameAddCount--;
                            addCount--;
                        }

                        if (addCount <= 0)
                            break;

                        if (autoList.Count <= 0)
                            break;
                    }
                }
            }
                
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
