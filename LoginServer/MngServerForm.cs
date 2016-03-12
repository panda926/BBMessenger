using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using ChatEngine;
using System.Net.Sockets;

namespace LoginServer
{
    public partial class MngServerForm : Form
    {
        IniFileEdit ini = new IniFileEdit(AppDomain.CurrentDomain.BaseDirectory + "loginServer.ini");
        List<RunServerInfo> _ServerList = new List<RunServerInfo>();
        int _StartPort = 2500;

        bool _UpdateServerList = false;        

        public MngServerForm()
        {
            InitializeComponent();
        }

        private void MngServerForm_Load(object sender, EventArgs e)
        {
            textBoxSourcePath.Text = ini.GetIniValue("Path", "Source");
            textBoxTargetPath.Text = ini.GetIniValue("Path", "Target");
            textBoxDownloadPath.Text = ini.GetIniValue("Path", "Download");
            Database._IP = ini.GetIniValue("Path", "MainServerAddress");

            timer1.Enabled = true;

            // server initialize
            Server server = Server.GetInstance();
            server.AttachHandler(NotifyOccured);

            int lisenPort = 2400;
            server.Connect("127.0.0.1", lisenPort.ToString());

            // 2014-05-14: GreenRose
            // 24시간 간격으로 Main서버에 프로그램의 사용상태를 체크한다.
            // 만일 사용상태가 사용불가능이라면 ChatServer프로그램을 종료한다.
            CheckUsingState();
            CheckUsingStateTimerStart();
        }

        // 2014-05-13: GreenRose
        private static System.Timers.Timer checkTimer = new System.Timers.Timer();
        private void CheckUsingStateTimerStart()
        {
            checkTimer.Interval = 1000 * 60 * 60 * 24;
            checkTimer.Elapsed += new System.Timers.ElapsedEventHandler(checkTimer_Elapsed);
            checkTimer.Enabled = true;
        }

        void checkTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckUsingState();
        }

        private void CheckUsingState()
        {
            int nResult = Database.GetInstance().GetUsingState();
            if (nResult == -1 || nResult == 0)
            {
                for(int i = 0; i < _ServerList.Count; i++)
                {
                    _ServerList[i]._ProcessInfo.Kill();
                }
            }

            this.Close();
        }

        private void browseSourcePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = textBoxSourcePath.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxSourcePath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void browseTargetPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = textBoxTargetPath.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxTargetPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void MngServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            checkTimer.Enabled = false;

            ini.SetIniValue("Path", "Source", textBoxSourcePath.Text);
            ini.SetIniValue("Path", "Target", textBoxTargetPath.Text);
            ini.SetIniValue("Path", "Download", textBoxDownloadPath.Text);
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (textBoxSourcePath.Text.Trim().Length == 0 ||
                textBoxTargetPath.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入路径.");
                return;
            }

            if (Directory.Exists(textBoxSourcePath.Text) == false ||
                Directory.Exists(textBoxTargetPath.Text) == false)
            {
                MessageBox.Show("不正确的路径");
                return;
            }

            DateTime curTime = DateTime.Now;
            string newFolder = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", curTime.Year, curTime.Month, curTime.Day, curTime.Hour, curTime.Minute, curTime.Second);
            string targetFolder = string.Format("{0}\\{1}", textBoxTargetPath.Text, newFolder);

            if( Directory.CreateDirectory(targetFolder) == null )
            {
                MessageBox.Show("无法创建新的运行路径");
                return;
            }

            try
            {
                CopyFolder(textBoxSourcePath.Text, targetFolder);

                Process processInfo = RunServer(targetFolder);

                if( processInfo == null )
                    return;

                int serverPort = _StartPort;

                while (true)
                {
                    bool foundPort = false;

                    foreach (RunServerInfo runInfo in _ServerList)
                    {
                        if (runInfo._ServerPort == serverPort)
                        {
                            foundPort = true;
                            break;
                        }
                    }

                    if (foundPort == false)
                        break;

                    serverPort++;
                }

                IniFileEdit iniFile = new IniFileEdit(targetFolder + "\\Config.ini");
                iniFile.SetIniValue("Chatting", "TcpPort", serverPort.ToString());
                iniFile.SetIniValue("Download", "Folder", textBoxDownloadPath.Text);

                if (_ServerList.Count > 0)
                {
                    RunServerInfo oldServerInfo = _ServerList[_ServerList.Count - 1];

                    Client client = new Client(null);
                    if (client.Connect("127.0.0.1", oldServerInfo._ServerPort.ToString(), ProtocolType.Tcp) == true)
                    {
                        ResultInfo resultInfo = new ResultInfo();
                        client.Send(NotifyType.Notify_OldServer, resultInfo);
                    }
                }

                RunServerInfo serverInfo = new RunServerInfo();

                serverInfo._StarTime = curTime;
                serverInfo._RunFolder = newFolder;
                serverInfo._DownloadFolder = textBoxDownloadPath.Text;
                serverInfo._ServerPort = serverPort;
                serverInfo._ProcessInfo = processInfo;

                _ServerList.Add(serverInfo);
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.ToString());
                return;
            }

            _UpdateServerList = true;
            //MessageBox.Show( "新的更新服务器已成功启动");
        }

        public void CopyFolder( string sourceFolder, string targetFolder)
        {
            string[] fileEntries = Directory.GetFiles(sourceFolder);

            foreach (string sourceFileName in fileEntries)
            {
                string targetFileName = string.Format( "{0}\\{1}", targetFolder, Path.GetFileName(sourceFileName));

                File.Copy(sourceFileName, targetFileName);
            }
        }

        public Process RunServer( string folderName )
        {
            string[] fileEntries = Directory.GetFiles(folderName);

            string runFileName = string.Empty;

            foreach (string fileName in fileEntries)
            {
                string extension = Path.GetExtension(fileName).ToUpper();

                if (extension == ".EXE")
                {
                    string exeName = Path.GetFileNameWithoutExtension(fileName);

                    if( exeName.Contains("."))
                        continue;

                    if (runFileName.Length > 0)
                    {
                        MessageBox.Show("有在运行路径多一个可执行文件.");
                        return null;
                    }
                    runFileName = fileName;
                }
            }

            if (runFileName.Length == 0)
            {
                MessageBox.Show("有没有在运行路径的可执行文件");
                return null;
            }

            // Prepare the process to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            //start.Arguments = arguments;
            // Enter the executable to run, including the complete path
            start.FileName = runFileName;
            start.WorkingDirectory = Path.GetDirectoryName(runFileName);
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

            return proc;
        }

        public void RefreshView()
        {
            ServerView.Rows.Clear();

            foreach (RunServerInfo serverInfo in _ServerList)
            {
                string timeString = serverInfo._StarTime.ToString("MM/dd HH:mm:ss");
                ServerView.Rows.Add(timeString, serverInfo._RunFolder, serverInfo._DownloadFolder, serverInfo._LoginCount);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int index = 0;

            while (index < _ServerList.Count)
            {
                RunServerInfo serverInfo = _ServerList[index];

                if (serverInfo._ProcessInfo.HasExited == true)
                {
                    _ServerList.Remove(serverInfo);
                    _UpdateServerList = true;
                    continue;
                }
                index++;
            }

            if (_UpdateServerList == true)
            {
                _UpdateServerList = false;
                RefreshView();
            }
        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            Server server = Server.GetInstance();

            switch (notifyType)
            {
                case NotifyType.Request_Server:
                    {
                        if (_ServerList.Count == 0)
                        {
                            buttonUpdate_Click(null, null);

                            if (_ServerList.Count == 0)
                            {
                                ResultInfo resultInfo = new ResultInfo();
                                resultInfo.ErrorType = ErrorType.Notrespond_Server;

                                server.Send(socket, NotifyType.Reply_Error, resultInfo);
                                return;
                            }
                        }
                        RunServerInfo runServer = _ServerList[_ServerList.Count-1];
                        runServer._LoginCount++;

                        ServerInfo serverInfo = new ServerInfo();

                        serverInfo.ServerPort = runServer._ServerPort;
                        serverInfo.DownloadAddress = runServer._DownloadFolder;

                        server.Send(socket, NotifyType.Reply_Server, serverInfo);

                        _UpdateServerList = true;
                    }
                    break;
            }
        }
    }

    public class RunServerInfo
    {
        public DateTime _StarTime;
        public string _RunFolder;
        public string _DownloadFolder;
        public int _ServerPort;
        
        public Process _ProcessInfo;
        public int _LoginCount;
    }
  
}
