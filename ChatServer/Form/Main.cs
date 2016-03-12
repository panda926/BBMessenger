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

using System.Net;
//using IMLibrary3;
//using IMLibrary3.IO;
//using IMLibrary3.Net;
//using IMLibrary3.Server;
//using IMLibrary3.Organization;
//using IMLibrary3.Protocol;
//using IMLibrary3.Security;
//using IMLibrary3.Operation;
//using IMLibrary3.Net.TCP;
//using IMLibrary3.Net.UDP;

namespace ChatServer
{
    public partial class Main : Form
    {
        //public static int _TcpPort = 2500;
        
        public static int pingDelay = 0;
        public static int timerDelay = 0;
        public static int refreshDelay = 0;

        public static View _View = new View();
        public static TotalView _TotalView = new TotalView();
        public static LogView _LogView = new LogView();
        public static ErrorView _ErrorView = new ErrorView();

        public static Main _instance = null;

        //public P2PAVServer p2pAVServer = null;

        public Main()
        {
            InitializeComponent();

            _instance = this;
        }

        private void Main_Load(object sender, EventArgs e)
        {

            // 2014-02-25: GreenRose
            //if (p2pAVServer == null) p2pAVServer = new P2PAVServer(54);
            //p2pAVServer.Start();

            // menu initialize
            UserKind userKind = (UserKind)Users.ManagerInfo.Kind;

            if (Users.ManagerInfo.Kind != (int)UserKind.Manager)
            {
                menuStrip1.Items.Remove(EnvMenus);
                menuStrip1.Items.Remove(mailboxToolStripMenuItem);

                UserMenus.DropDownItems.Remove(AutoMenuItem);


                if (userKind != UserKind.RecommendOfficer)
                {
                    UserMenus.DropDownItems.Remove(FirstBuyerMenuItem);
                    menuStrip1.Items.Remove(GameMenus);
                }

                if (userKind != UserKind.ServiceOfficer)
                {
                    menuStrip1.Items.Remove(RoomMenus);
                }

                if (userKind != UserKind.Banker)
                {
                    menuStrip1.Items.Remove(ChargeMenus);
                }
            }

            //// server initialize
            //IniFileEdit serverIni = new IniFileEdit(AppDomain.CurrentDomain.BaseDirectory + "\\server.ini");
            //string port = serverIni.GetIniValue("Chatting", "TcpPort");

            //Server server = Server.GetInstance();

            //server.AttachHandler(Users.GetInstance().NotifyOccured);
            //server.AttachHandler(Chat.GetInstance().NotifyOccured);
            //server.AttachHandler(Game.GetInstance().NotifyOccured);

            //server.Connect("127.0.0.1", port);

            // udp initialize
            //UdpServerClient udpServer = UdpServerClient.GetInstance();

            //udpServer.AttachUdpHandler(Chat.GetInstance().NotifyUdpOccured);

            //udpServer.Connect(_TcpPort, 1);

            // timer
            timer1.Enabled = true;

            // show
            if (Users.ManagerInfo.Kind == (int)UserKind.Manager)
            {
                _View.MdiParent = this;
                _View.Show();

                _LogView.MdiParent = this;
                _LogView.Show();

                _ErrorView.MdiParent = this;
                _ErrorView.Show();

                _TotalView.MdiParent = this;
                _TotalView.Show();
            }
        }

        public static void ReplyError(Socket socket)
        {
            ResultInfo resultInfo = new ResultInfo();

            ErrorInfo errorInfo = BaseInfo.GetError();
            resultInfo.ErrorType = errorInfo.ErrorType;

            ErrorView.AddErrorString();

            Server.GetInstance().Send(socket, NotifyType.Reply_Error, resultInfo);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timerDelay++;
                pingDelay++;
                refreshDelay++;

                // control game and chat flow
                if (pingDelay > 10)
                {
                    pingDelay = 0;

                    //Cash.GetInstance().NotifyCash();
                    //Auto.GetInstance().NotifyTimer();
                    lock (Server.GetInstance()._objLockMain)
                    {
                        Game.GetInstance().NotifyTimer();
                        Users.GetInstance().NotifyPing();
                    }
                }

                // control auto
                if (timerDelay > 300)
                {
                    timerDelay = 0;
                    Users.GetInstance().NotifyAutoCheck();
                }

                // refresh
                if (refreshDelay > 100)
                {
                    refreshDelay = 0;

                    _ErrorView.RefreshError();
                    _LogView.RefreshLog();

                    _View.RefreshView();
                }
            }
            catch (Exception ex)
            {
                BaseInfo.SetError(ErrorType.Exception_Occur, ex.ToString());
                ErrorView.AddErrorString();
            }
        }

        private void ManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MngManager mngManager = new MngManager();

            ShowForm( mngManager );
        }

        private void RecommenderMenuItem_Click(object sender, EventArgs e)
        {
            MngRecommender mngRecommender = new MngRecommender();

            ShowForm( mngRecommender );
        }

        private void ServicemanpMenuItem_Click(object sender, EventArgs e)
        {
            MngServiceman mngServiceman = new MngServiceman();

            ShowForm( mngServiceman );
        }

        private void FirstBuyerMenuItem_Click(object sender, EventArgs e)
        {
            MngUser mngUser = new MngUser();

            ShowForm( mngUser );
        }

        private void RoomMenuItem_Click(object sender, EventArgs e)
        {
            MngRoom manageRoom = new MngRoom();

            ShowForm( manageRoom );
        }

        private void ChatHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MngChatHistory mngChatHistory = new MngChatHistory();

            ShowForm( mngChatHistory );
        }

        private void FreeChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MngPoint mngPoint = new MngPoint();
            mngPoint._Kind = (int)PointKind.Chat;

            ShowForm( mngPoint );
        }

        private void GameMenuItem_Click(object sender, EventArgs e)
        {
            MngGame mngGame = new MngGame();

            ShowForm( mngGame );
        }

        private void GameHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MngGameHistory mngGameHistory = new MngGameHistory();

            ShowForm( mngGameHistory );

        }

        private void ChargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MngGateway mngGateway = new MngGateway();

            ShowForm(mngGateway);
        }

        private void ChaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MngChargeHistory mngChargeHistory = new MngChargeHistory();
            mngChargeHistory._Kind = (int)ChargeKind.Charge;

            ShowForm(mngChargeHistory);
        }

        private void DischargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MngChargeHistory mngChargeHistory = new MngChargeHistory();
            //mngChargeHistory._Kind = (int)ChargeKind.Discharge;

            ShowForm( mngChargeHistory );
        }

        private void PointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MngPoint mngPoint = new MngPoint();
            mngPoint._Kind = (int)PointKind.Bonus;

            ShowForm( mngPoint );
        }

        private void EnvMenus_Click(object sender, EventArgs e)
        {
            MngEnviroment mngEnviroment = new MngEnviroment();

            ShowForm( mngEnviroment );
        }

        public void ShowForm(Form form)
        {
            try
            {
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm.GetType() == form.GetType())
                    {

                        openForm.BringToFront();

                        openForm.Focus();
                        return;
                    }
                }

                //form.MdiParent = this;
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                BaseInfo.SetError(ErrorType.Exception_Occur, ex.ToString());
                ErrorView.AddErrorString();
            }
        }

        private void mailboxToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lettersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MngBoard mngBoard = new MngBoard();
            //ShowForm(mngBoard);

            MngNotice mngNotice = new MngNotice();
            mngNotice._boardKind = BoardKind.Letter;

            ShowForm(mngNotice);

        }

        private void noticesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MngNotice mngNotice = new MngNotice();
            mngNotice._boardKind = BoardKind.Notice;

            ShowForm(mngNotice);
        }

        private void AutoMenuItem_Click(object sender, EventArgs e)
        {
            MngAuto mngAuto = new MngAuto();

            ShowForm(mngAuto);

        }

        private void ChatPercentMenuItem_Click(object sender, EventArgs e)
        {
            MngChatPercent mngChatPercent = MngChatPercent.GetInstance();
            ShowForm(mngChatPercent);
        }

        private void ApproveChargeMenuItem_Click(object sender, EventArgs e)
        {
            AddCharge addCharge = new AddCharge();

            ShowForm(addCharge);
        }

        private void FinancialMenus_Click(object sender, EventArgs e)
        {

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //p2pAVServer.Stop();
        }

    }
}
