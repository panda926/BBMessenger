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
using System.Windows.Navigation;
using System.Windows.Shapes;

using ControlExs;
using ChatEngine;
using System.Net.Sockets;
using ChatClient.Tab_Game;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for GameInfoUserCtrl.xaml
    /// </summary>
    /// 
    // 2014-01-16: GreenRose
    public partial class GameInfoUserCtrl : UserControl
    {
        public GameInfo _gameInfo = null;
        public bool _bDownloading = false;

        public GameInfoUserCtrl()
        {
            InitializeComponent();

            //Login._ClientEngine.AttachHandler(NotifyOccured);
        }

        GameDownload gameDownloadWnd;        

        public void InitUserCtrl(ChatEngine.GameInfo gameInfo)
        {
            _gameInfo = gameInfo;
            
            lbl_GameName.Content = gameInfo.GameName;
            img_GameIcon.Source = ImageDownloader.GetInstance().GetImage(gameInfo.Icon);

            bool bDownloaded = CheckDownloadState(_gameInfo);
            if (bDownloaded)
            {
                img_GameIcon.Opacity = 0.8;
            }
            else
            {
                img_GameIcon.Opacity = 0.2;
                img_GamePointIcon.Visibility = Visibility.Hidden;
                img_GameCashIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources;component/image/Download-icon.png", UriKind.RelativeOrAbsolute));

            }
        }

        // 2014-01-21: GreenRose
        private bool CheckDownloadState(GameInfo gameInfo)
        {
            if (gameInfo.Downloadfolder.Trim().Length == 0)
                return true;

            string strIniFilePath = System.Windows.Forms.Application.StartupPath + "\\userinfo.ini";
            IniFileEdit iniFileEdit = new IniFileEdit(strIniFilePath);

            string strVal = iniFileEdit.GetIniValue("GameInfo", gameInfo.Source);
            if (strVal == string.Empty)
                return false;

            else if (strVal == "0")
                return false;

            return true;
        }

        // 2014-01-21: GreenRose
        private void SetDownloadState(GameInfo gameInfo)
        {
            string strIniFilePath = System.Windows.Forms.Application.StartupPath + "\\userinfo.ini";
            IniFileEdit iniFileEdit = new IniFileEdit(strIniFilePath);

            iniFileEdit.SetIniValue("GameInfo", gameInfo.Source, "1");
        }

        private void img_GameCashIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            img_GameCashIcon.Opacity = 1;
        }

        private void img_GamePointIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            img_GamePointIcon.Opacity = 1;
        }

        private void img_GameCashIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            img_GameCashIcon.Opacity = 0.3;
        }

        private void img_GamePointIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            img_GamePointIcon.Opacity = 0.3;
        }

        private void img_GameCashIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UpdateGame(0);   
        }

        private void img_GamePointIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UpdateGame(1);
        }

        private void UpdateGame(int nGameType)
        {
            if (!_bDownloading)
            {
                _gameInfo.nCashOrPointGame = nGameType;
                DownloadGame();
                //Login._ClientEngine.Send(NotifyType.Request_Download_GameFile, _gameInfo);

                _bDownloading = true;
            }
        }

        private void EnterGame(int nGameType)
        {
            if (Main.activeGame != null)
            {
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "你必须选择一个游戏", "提示", QQMessageBoxIcon.Warning, QQMessageBoxButtons.OK);

                return;
            }

            // modified by usc at 2014/02/09
            if (Main.m_bGameRunning)
                return;

            //if (_gameInfo.Source != "Dice" && nGameType == 0)
            //{
            //    TempWindowForm tempWindowForm = new TempWindowForm();
            //    QQMessageBox.Show(tempWindowForm, "游戏测试中...", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
            //    return;
            //}

            GameInfo enterGameinfo = new GameInfo();

            enterGameinfo._NotifyHandler = _gameInfo._NotifyHandler;
            enterGameinfo.Bank = _gameInfo.Bank;
            enterGameinfo.Commission = _gameInfo.Commission;
            enterGameinfo.GameId = (Convert.ToInt32(_gameInfo.GameId) + nGameType).ToString();
            enterGameinfo.GameName = _gameInfo.GameName;
            enterGameinfo.Height = _gameInfo.Height;
            enterGameinfo.Icon = _gameInfo.Icon;
            enterGameinfo.nCashOrPointGame = nGameType;
            enterGameinfo.Source = _gameInfo.Source;
            enterGameinfo.UserCount = _gameInfo.UserCount;
            enterGameinfo.Width = _gameInfo.Width;

            Login._UserInfo.GameId = enterGameinfo.GameId;
            Login._UserInfo.nCashOrPointGame = enterGameinfo.nCashOrPointGame;

            Main.m_bGameRunning = true;

            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                if (Main.loadWnd == null)
                {
                    Main.loadWnd = new LoadingWnd();
                    Main.loadWnd.Owner = Main.GetParentWindow(this);
                    Main.loadWnd.ShowDialog();
                }
            }));

                
            Login._ClientEngine.Send(NotifyType.Request_EnterGame, enterGameinfo);
        }

        void DownloadGame()
        {
            string downloadPath = string.Format("Games/{0}/server.version", _gameInfo.Downloadfolder);

            GameWebDownloader.GetInstance().DownloadFile(downloadPath, DownloadVersionCompleted, this);
        }

        List<DownloadFileInfo> _ServerVersions;
        List<DownloadFileInfo> _LocalVersions;

        public void DownloadVersionCompleted(string filePath)
        {
            string localRoot = AppDomain.CurrentDomain.BaseDirectory + "Games\\" + _gameInfo.Downloadfolder;

            _ServerVersions = VersionInfo.ReadInfoFile(localRoot, false);
            _LocalVersions = VersionInfo.ReadInfoFile(localRoot, true);

            if (NeedDownload())
            {
                gameDownloadWnd = new GameDownload();
                gameDownloadWnd.progressUpdate.Maximum = GetDownloadCount();
                gameDownloadWnd.progressUpdate.Value = 0;
                gameDownloadWnd.Show();

                GameWebDownloader.gameDownloadWnd = gameDownloadWnd;
                GameWebDownloader.GetInstance().DownloadFile(_ServerVersions[0].FilePath, DownloadFileCompleted, this);
            }
        }

        public int GetDownloadCount()
        {
            int count = 0;

            foreach ( DownloadFileInfo serverInfo in _ServerVersions )
            {
                DownloadFileInfo foundInfo = null;

                foreach (DownloadFileInfo localInfo in _LocalVersions)
                {
                    if (localInfo.FilePath == serverInfo.FilePath)
                    {
                        foundInfo = localInfo;
                        break;
                    }
                }

                if (foundInfo == null)
                    count++;
                else if (foundInfo.FileVersion < serverInfo.FileVersion)
                    count++;
            }

            return count;
        }

        public bool NeedDownload()
        {
            while (_ServerVersions.Count > 0)
            {
                DownloadFileInfo foundInfo = null;

                foreach (DownloadFileInfo localInfo in _LocalVersions)
                {
                    if (localInfo.FilePath == _ServerVersions[0].FilePath)
                    {
                        foundInfo = localInfo;
                        break;
                    }
                }

                if (foundInfo == null)
                    return true;

                if (foundInfo.FileVersion < _ServerVersions[0].FileVersion)
                    return true;

                _ServerVersions.RemoveAt(0);
            }

            string localRoot = AppDomain.CurrentDomain.BaseDirectory + "Games\\" + _gameInfo.Downloadfolder;
            VersionInfo.WriteInfoFile(localRoot, _LocalVersions, true);

            string engineName = AppDomain.CurrentDomain.BaseDirectory + "ChatEngine.dll";
            System.IO.File.Copy(engineName, localRoot + "\\ChatEngine.dll", true);

            string controlName = AppDomain.CurrentDomain.BaseDirectory + "GameControls.dll";
            System.IO.File.Copy(controlName, localRoot + "\\GameControls.dll", true);

            if( gameDownloadWnd != null )
                gameDownloadWnd.Close();
            SetDownloadState(_gameInfo);

            img_GameIcon.Opacity = 0.8;
            img_GameCashIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources;component/image/GoldCoin.jpg", UriKind.RelativeOrAbsolute));
            img_GamePointIcon.Visibility = Visibility.Visible;

            _bDownloading = false;

            EnterGame(_gameInfo.nCashOrPointGame);
            return false;
        }

        public void DownloadFileCompleted(string filePath)
        {
            DownloadFileInfo foundInfo = null;

            foreach (DownloadFileInfo localInfo in _LocalVersions)
            {
                if (localInfo.FilePath == _ServerVersions[0].FilePath)
                {
                    foundInfo = localInfo;
                    break;
                }
            }

            if (foundInfo == null)
            {
                foundInfo = new DownloadFileInfo();
                foundInfo.FilePath = _ServerVersions[0].FilePath;

                _LocalVersions.Add(foundInfo);
            }

            foundInfo.FileVersion = _ServerVersions[0].FileVersion;

            _ServerVersions.RemoveAt(0);
            gameDownloadWnd.progressUpdate.Value++;

            if (NeedDownload())
                GameWebDownloader.GetInstance().DownloadFile(_ServerVersions[0].FilePath, DownloadFileCompleted, this);
        }
    }
}
