using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms.Integration;
using ChatEngine;
using SicboClient;
using DzCardClient;
using HorseClient;
using GameControls;
using System.Net.Sockets;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for GameRoom.xaml
    /// </summary>
    public partial class GameRoom : Window
    {
        public GameInfo _GameInfo;
        public GameView _GameView;

        public GameRoom(GameInfo gameInfo)
        {
            InitializeComponent();

            _GameInfo = gameInfo;

            switch (gameInfo.Source)
            {
                case "Sicbo":
                    _GameView = new SicboView();
                    break;

                case "DzCard":
                    _GameView = new DzCardView();
                    break;

                case "Horse":
                    _GameView = new HorseView();
                    break;
            }

            if (_GameView == null)
                return;

            _GameView.Width = gameInfo.Width;
            _GameView.Height = gameInfo.Height;

            _GameView.SetClientSocket(Window1._ClientEngine);
            _GameView.SetUserInfo(Window1._UserInfo);


            Expander expender = new Expander();
            WindowsFormsHost host = new WindowsFormsHost();

            host.Child = _GameView;

            expender.Content = host;
            expender.IsExpanded = true;

            this.Content = expender;
            
            this.Width = _GameInfo.Width;
            this.Height = _GameInfo.Height;

            if( _GameView is SicboView ||
                _GameView is HorseView )
                Window1._ClientEngine.Send(NotifyType.Request_PlayerEnter, Window1._UserInfo);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBoxCommon.Show("Do you want to exit a game?", MessageBoxType.YesNo) == MessageBoxReply.No)
            {
                e.Cancel = true;
                return;
            }

            _GameView.CloseView();

            Main._GameRoom = null;
            Main.activeGame = null;
            Main.u_gamrId = null;
            Window1._ClientEngine.Send(NotifyType.Request_OutGame, Window1._UserInfo);
            Window1._ClientEngine.DetachHandler(_GameView.NotifyOccured);
        }

    }
}
