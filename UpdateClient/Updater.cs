using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.ComponentModel;
using ChatEngine;
using System.Windows.Forms;
using ControlExs;
using System.Diagnostics;

namespace UpdateClient
{
    public class Updater
    {
        private static Updater _instance = null;

        private IniFileEdit _UpdateIni;
        private Client _LoginEngine;
        
        private ServerInfo _ServerInfo;
        private string _DownloadPath;

        List<DownloadFileInfo> _ServerVersions;
        List<DownloadFileInfo> _LocalVersions;

        public ProgressForm _ProgressForm = null;

        public static Updater GetInstance()
        {
            if (_instance == null)
                _instance = new Updater();

            return _instance;
        }

        public bool Init()
        {
            _UpdateIni = new IniFileEdit(AppDomain.CurrentDomain.BaseDirectory + "Update.ini");

            string loginServerIP = _UpdateIni.GetIniValue( "LoginServer", "IP" );
            string loginServerPort = _UpdateIni.GetIniValue("LoginServer", "PORT");

            _LoginEngine = new Client(InvokeSocket);

            if (_LoginEngine.Connect(loginServerIP, loginServerPort, ProtocolType.Tcp) == false)
            {
                QQMessageBox.Show(_ProgressForm, "服务器连接失败", "消息", QQMessageBoxIcon.Warning, QQMessageBoxButtons.OK);
                Environment.Exit(0);
            }

            _LoginEngine.AttachHandler(NotifyOccured);
            
            ResultInfo resultInfo = new ResultInfo();
            _LoginEngine.Send(NotifyType.Request_Server, resultInfo);

            return true;
        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Notify_Socket:
                    {
                        QQMessageBox.Show(_ProgressForm, "服务器连接失败", "消息", QQMessageBoxIcon.Warning, QQMessageBoxButtons.OK);
                        Environment.Exit(0);
                    }
                    break;

                case NotifyType.Reply_Server:
                    {
                        _ServerInfo = (ServerInfo)baseInfo;
                        _DownloadPath = _UpdateIni.GetIniValue("DownloadServer", "URL") + "/" + _ServerInfo.DownloadAddress;

                        if (_DownloadPath[_DownloadPath.Length - 1] != '/')
                            _DownloadPath += '/';

                        string localRoot = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "\\..\\");
                        WebDownloader.localRoot = localRoot;

                        WebDownloader.progressForm = _ProgressForm;
                        WebDownloader.GetInstance().DownloadFile("server.version", DownloadVersionCompleted, _DownloadPath);
                    }
                    break;
            }
        }

        public void DownloadVersionCompleted(string filePath)
        {
            _LoginEngine.DetachHandler(NotifyOccured);
            _LoginEngine.Disconnect();

            _ServerVersions = VersionInfo.ReadInfoFile(WebDownloader.localRoot, false);
            _LocalVersions = VersionInfo.ReadInfoFile(WebDownloader.localRoot, true);


            if( NeedDownload())
                WebDownloader.GetInstance().DownloadFile(_ServerVersions[0].FilePath, DownloadFileCompleted, _DownloadPath);
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

            VersionInfo.WriteInfoFile(WebDownloader.localRoot, _LocalVersions, true);

            string gameRoot = WebDownloader.localRoot + "Games\\";

            if (Directory.Exists(gameRoot) == true)
            {
                DirectoryInfo gameRootInfo = new DirectoryInfo(gameRoot);

                foreach (DirectoryInfo gameFolderInfo in gameRootInfo.GetDirectories())
                {
                    string engineName = WebDownloader.localRoot + "ChatEngine.dll";
                    File.Copy(engineName, gameFolderInfo.FullName + "\\ChatEngien.dll", true);

                    string controlName = WebDownloader.localRoot + "GameControls.dll";
                    File.Copy(controlName, gameFolderInfo.FullName + "\\GameControls.dll", true);
                }
            }

            IniFileEdit UserIni = new IniFileEdit(WebDownloader.localRoot + "UserInfo.ini");

            UserIni.SetIniValue("ServerInfo", "ServerUri", _UpdateIni.GetIniValue("LoginServer", "IP"));
            UserIni.SetIniValue("ServerInfo", "ServerPort", _ServerInfo.ServerPort.ToString());
            UserIni.SetIniValue("ServerInfo", "ServerGamePath", _DownloadPath );


            // Prepare the process to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            //start.Arguments = arguments;
            // Enter the executable to run, including the complete path
            start.FileName = WebDownloader.localRoot + "ChatClient.exe";
            start.WorkingDirectory = WebDownloader.localRoot;
            // Do you want to show a console window?
            //start.WindowStyle = ProcessWindowStyle.Hidden;
            //start.CreateNoWindow = true;

            // Run the external process & wait for it to finish
            //using (Process proc = Process.Start(start))
            //{
            //    proc.WaitForExit();

            //    // Retrieve the app's exit code
            //    exitCode = proc.ExitCode;
            //}



            Process proc = Process.Start(start);
            //System.Diagnostics.Process.Start(WebDownloader.localRoot + "ChatClient.exe");
            Environment.Exit(0);

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

            if( NeedDownload())
                WebDownloader.GetInstance().DownloadFile(_ServerVersions[0].FilePath, DownloadFileCompleted, _DownloadPath);
        }



        void InvokeSocket()
        {
            this._ProgressForm.BeginInvoke(new MethodInvoker(delegate
            {
                _LoginEngine.NotifyReceiveData();
            }));
        }

    }


}
