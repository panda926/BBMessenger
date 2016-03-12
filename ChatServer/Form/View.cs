using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChatEngine;
using System.Net.Sockets;

namespace ChatServer
{
    public partial class View : Form
    {
        public static bool _isUpdateUserList = true;
        public static bool _isUpdateMeetList = true;
        public static bool _isUpdateRoomList = true;
        public static bool _isUpdateGameList = true;

        public View()
        {
            InitializeComponent();
        }

        private void View_Load(object sender, EventArgs e)
        {
            // ui initialize
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

        //private Object lockObject = new Object();
        public void RefreshView()
        {
            Database database = Database.GetInstance();
            Users users = Users.GetInstance();

            // Show user List
            if (_isUpdateUserList == true)
            {
                List<UserInfo> userList = Users.GetInstance().GetUsers();

                if (userList == null)
                {
                    ErrorView.AddErrorString();
                    return;
                }

                UserView.Rows.Clear();

                //lock (lockObject)
                {
                    for (int i = 0; i < userList.Count; i++)
                    {
                        UserInfo userInfo = userList[i];

                        int chatCash = database.SumChatHistory(userInfo, DateTime.Now);
                        int gameCash = database.SumGameHistory(userInfo, DateTime.Now);
                        int chargeCash = database.SumChargeHistory(userInfo.Id, DateTime.Now, -1);

                        string auto = string.Empty;

                        if (userInfo.Auto == 1)
                            auto = "auto";

                        UserView.Rows.Add(
                            userInfo.Id,
                            userInfo.Nickname,
                            userInfo.KindString,
                            string.Format("{0} / {1}", userInfo.Cash, userInfo.Point),
                            userInfo.LoginTime,
                            userInfo.RoomId,
                            chatCash,
                            userInfo.GameId,
                            gameCash,
                            chargeCash,
                            auto
                            );
                    }
                }

                int liveCount = Auto.GetInstance().GetLiveCount();
                int autoCount = Auto.GetInstance().GetAutoCount();

                labelAutoSummary.Text = string.Format("Auto: {0}/{1}", liveCount, autoCount);

                Main._TotalView.RefreshTotal();

                _isUpdateUserList = false;
            }

            // Show meet List
            //if (_isUpdateRoomList == true)
            //{
            //    List<RoomInfo> meetList = Chat.GetInstance().GetMeetings();

            //    if (meetList == null)
            //    {
            //        ErrorView.AddErrorString();
            //        return;
            //    }

            //    MeetView.Rows.Clear();

            //    for (int i = 0; i < meetList.Count; i++)
            //    {
            //        RoomInfo meetInfo = meetList[i];

            //        List<UserInfo> meetUsers = Users.GetInstance().GetRoomUsers(meetInfo.Id);

            //        //if (meetUsers == null || meetUsers.Count != 2)
            //        //{
            //        //    BaseInfo.SetError(ErrorType.Invalid_Id, "没有聊天对象.");
            //        //    ErrorView.AddErrorString();
            //        //    continue;
            //        //}

            //        UserInfo servicemanInfo = meetUsers[0];
            //        UserInfo buyerInfo = meetUsers[1];

            //        if (servicemanInfo.Id != meetInfo.Owner)
            //        {
            //            servicemanInfo = meetUsers[1];
            //            buyerInfo = meetUsers[0];
            //        }

            //        TimeSpan delayTime = DateTime.Now - servicemanInfo.EnterTime;

            //        MeetView.Rows.Add(
            //            meetInfo.Id,
            //            string.Format("{0} ( {1} )", servicemanInfo.Nickname, servicemanInfo.Id),
            //            string.Format("{0} ( {1} )", buyerInfo.Nickname, buyerInfo.Id),
            //            BaseInfo.ConvDateToString(servicemanInfo.EnterTime),
            //            string.Format("{0:00}:{1:00}:{2:00}", delayTime.Hours, delayTime.Minutes, delayTime.Seconds),
            //            meetInfo.Cash,
            //            -Database.GetInstance().SumChatHistory(meetInfo.Id, DateTime.Now)
            //            );
            //    }

            //    //    _isUpdateMeetList = false;
            //    //}

            //    // Show room List
            //    //if (_isUpdateRoomList == true)
            //    //{
            //    List<RoomInfo> roomList = Chat.GetInstance().GetRooms();

            //    //RoomView.Rows.Clear();

            //    for (int i = 0; i < roomList.Count; i++)
            //    {
            //        RoomInfo roomInfo = roomList[i];

            //        List<UserInfo> roomUsers = Users.GetInstance().GetRoomUsers(roomInfo.Id);

            //        if (roomUsers == null || roomUsers.Count < 1)
            //        {
            //            BaseInfo.SetError(ErrorType.Invalid_Id, "此频道不能进入.");
            //            ErrorView.AddErrorString();
            //            continue;
            //        }

            //        string buyers = "";
            //        for (int k = 0; k < roomUsers.Count; k++)
            //        {
            //            if (roomUsers[k].Id == roomInfo.Owner)
            //                continue;

            //            buyers += roomUsers[k].Id;

            //            if (k < roomUsers.Count - 1)
            //                buyers += ", ";
            //        }

            //        //RoomView.Rows.Add(
            //        //    roomInfo.Id,
            //        //    roomInfo.Name,
            //        //    roomInfo.Cash,
            //        //    string.Format("{0} / {1} ", roomUsers.Count, roomInfo.MaxUsers),
            //        //    roomInfo.Owner,
            //        //    buyers,
            //        //    -Database.GetInstance().SumChatHistory(roomInfo.Id, DateTime.Now )
            //        //    );
            //    }

            //    _isUpdateRoomList = false;
            //}

            // show game list
            //if (_isUpdateGameList == true)
            //{
            //    List<GameTable> gameList = Game.GetInstance().GetGameTables();

            //    if (gameList == null)
            //    {
            //        ErrorView.AddErrorString();
            //        return;
            //    }

            //    GameView.Rows.Clear();

            //    for (int i = 0; i < gameList.Count; i++)
            //    {
            //        GameInfo gameInfo = gameList[i]._GameInfo;
            //        TableInfo tableInfo = gameList[i]._TableInfo;

            //        string gamers = string.Empty;

            //        foreach (UserInfo userInfo in tableInfo._Players)
            //        {
            //            gamers += userInfo.Id;

            //            if (userInfo != tableInfo._Players[tableInfo._Players.Count - 1])
            //                gamers += ", ";
            //        }

            //        string round = tableInfo._RoundIndex.ToString();

            //        if (tableInfo._RoundIndex == 0)
            //            round += " (Ready)";
            //        else if (tableInfo._RoundIndex == gameList[i]._Rounds.Count - 1)
            //            round += " (End)";
            //        else
            //            round += " (Run)";

            //        GameView.Rows.Add(
            //            gameInfo.GameId,
            //            gameInfo.GameName,
            //            gameInfo.Commission,
            //            tableInfo._Players.Count,
            //            gamers,
            //            round,
            //            -Database.GetInstance().SumGameHistory(gameInfo.GameId, DateTime.Now)
            //            );

            //    }
            //    _isUpdateGameList = false;
            //}
        }

        private void buttonOutUser_Click(object sender, EventArgs e)
        {
            Users users = Users.GetInstance();

            if (UserView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }
            
            string userId = UserView.SelectedRows[0].Cells["Id"].Value.ToString();

            string question = string.Format("真要强退 {0} 吗?", userId);

            if (MessageBox.Show(question, "强退提问", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            UserInfo userInfo = users.FindUser(userId);

            if (userInfo == null)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            lock (Server.GetInstance()._objLockMain)
            {
                if (userInfo.Auto == 1)
                {
                    Auto.GetInstance().OutAuto(userInfo);
                }
                else
                {
                    Server.GetInstance()._NotifyHandler(NotifyType.Request_Logout, userInfo.Socket, userInfo);
                }
            }

            string resultStr = string.Format("{0} 已退出.", userInfo.Id);
            MessageBox.Show(resultStr);

            _isUpdateUserList = true;            
        }

        private void buttonAddAuto_Click(object sender, EventArgs e)
        {
            AddAuto addAuto = new AddAuto();
            addAuto.ShowDialog();
        }

        private void buttonDelAuto_Click(object sender, EventArgs e)
        {
            DelAuto delAuto = new DelAuto();
            delAuto.ShowDialog();
        }

        private void View_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
        }

        //// 2013-12-17: GreenRose
        //private void btnView_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.dataGridPayment.Rows.Clear();

        //        List<PaymentInfo> listPaymentInfo = Database.GetInstance().GetAllPaymentInfos();
        //        if (listPaymentInfo == null)
        //        {
        //            MessageBox.Show("검색자료가 한건도 없습니다");
        //            return;
        //        }

        //        int nStep = 1;
        //        foreach (PaymentInfo paymentInfo in listPaymentInfo)
        //        {
        //            this.dataGridPayment.Rows.Add(nStep++, paymentInfo.strID, paymentInfo.strAccountID, paymentInfo.strAccountNumber, paymentInfo.nPaymentMoney);
        //        }
        //    }
        //    catch (System.Exception)
        //    {
            	
        //    }
        //}

        //private void btnPayment_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (MessageBox.Show("정말 결제를 끝내시겠습니까?", "통보", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        {
        //            if(Database.GetInstance().DeletePaymentInfos())
        //                this.dataGridPayment.Rows.Clear();
        //        }
        //    }
        //    catch (System.Exception)
        //    {
            	
        //    }
        //}

    };

}

