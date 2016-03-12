using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;

// 2014-01-08: GreenRose
using System.Runtime.InteropServices;//DLLImport
using System.IO;
using ANYCHATAPI;
using ChatEngine;

namespace UpdateServer
{
    public partial class UpdateForm : Form
    {
        IniFileEdit ini = new IniFileEdit(AppDomain.CurrentDomain.BaseDirectory + "updateServer.ini");
        List<DownloadKindInfo> DownloadKindList = new List<DownloadKindInfo>();

        public UpdateForm()
        {
            InitializeComponent();
        }
        
        private void UpdateForm_Load(object sender, EventArgs e)
        {
            textBoxSourcePath.Text = ini.GetIniValue("Path", "Source");
            textBoxTargetPath.Text = ini.GetIniValue("Path", "Target");
        }

        private void UpdateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ini.SetIniValue("Path", "Source", textBoxSourcePath.Text);
            ini.SetIniValue("Path", "Target", textBoxTargetPath.Text);
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

        private void buttonUpdate_Click_1(object sender, EventArgs e)
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

            if (Directory.CreateDirectory(targetFolder) == null)
            {
                MessageBox.Show("无法创建新的运行路径");
                return;
            }

            SelectForm selForm = new SelectForm();

            selForm.CurrentFolder = textBoxSourcePath.Text + "\\";
            if( selForm.ShowDialog() != System.Windows.Forms.DialogResult.OK )
                return;

            List<DownloadKindInfo> kindList = new List<DownloadKindInfo>();

            try
            {
                kindList.Add(new DownloadKindInfo("Main", ProcessVersions( "", selForm.SelectFileList)));

                DirectoryInfo gamesFoldersInfo = new DirectoryInfo(textBoxSourcePath.Text + "\\" + "Games");

                if (gamesFoldersInfo.Exists)
                {
                    foreach (DirectoryInfo gameFolderInfo in gamesFoldersInfo.GetDirectories())
                    {
                        string targetGameFolder = string.Format("{0}\\Games\\{1}", targetFolder, gameFolderInfo.Name);

                        if( Directory.Exists( targetGameFolder ) == false )
                            Directory.CreateDirectory(targetGameFolder);

                        kindList.Add( new DownloadKindInfo( gameFolderInfo.Name, ProcessVersions( "Games\\" + gameFolderInfo.Name, selForm.SelectFileList )));
                    }
                }

                CopyFolder(textBoxSourcePath.Text, targetFolder, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            textBoxDownloadPath.Text = newFolder;
            comboDownloadKind.Items.Clear();
            FilesView.Rows.Clear();
            DownloadKindList = kindList;

            foreach (DownloadKindInfo kindInfo in DownloadKindList)
            {
                comboDownloadKind.Items.Add(kindInfo.KindName);
            }

            if (comboDownloadKind.Items.Count > 0)
                comboDownloadKind.SelectedIndex = 0;

            MessageBox.Show("注册成功完成");
        }

        public bool CopyFolder(string sourceFolder, string targetFolder, bool bRoot )
        {
            DirectoryInfo sourceFolderInfo = new System.IO.DirectoryInfo(sourceFolder);

            foreach(FileInfo sourceFileInfo in sourceFolderInfo.GetFiles())
            {
                string targetFileName = string.Format("{0}\\{1}", targetFolder, sourceFileInfo.Name);

                sourceFileInfo.CopyTo(targetFileName);
            }

            foreach (DirectoryInfo sourceSubFolderInfo in sourceFolderInfo.GetDirectories())
            {
                string targetSubFolder = string.Format( "{0}\\{1}", targetFolder, sourceSubFolderInfo.Name );
                Directory.CreateDirectory(targetSubFolder);

                CopyFolder(sourceSubFolderInfo.FullName, targetSubFolder, false );
            }

            return true;
        }

        private void comboDownloadKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboDownloadKind.SelectedIndex;

            if (index < 0 || index >= DownloadKindList.Count)
                return;

            DownloadKindInfo kindInfo = DownloadKindList[index];

            FilesView.Rows.Clear();

            foreach (DownloadFileInfo fileInfo in kindInfo.FileList)
            {
                FilesView.Rows.Add(fileInfo.FilePath, fileInfo.FileVersion.ToString());
            }

            LabelTotalNum.Text = string.Format("可下载的文件列表:   {0} 数", FilesView.Rows.Count);
        }

        public List<DownloadFileInfo> ProcessVersions(string folder, List<string> updateList)
        {
            string fullPath = textBoxSourcePath.Text + "\\" + folder;
            List<DownloadFileInfo>versionList = VersionInfo.ReadInfoFile(fullPath, false);

            foreach (string fileName in updateList)
            {
                if (folder.Length == 0)
                {
                    if (fileName.Length >= 5 && fileName.Substring(0, 5) == "Games")
                        continue;
                }
                else
                {
                    if (fileName.Length < folder.Length || folder != fileName.Substring(0, folder.Length))
                        continue;
                }

                DownloadFileInfo foundInfo = null;

                foreach (DownloadFileInfo fileInfo in versionList)
                {
                    if (fileInfo.FilePath == fileName)
                    {
                        foundInfo = fileInfo;
                        break;
                    }
                }

                if (foundInfo == null)
                {
                    foundInfo = new DownloadFileInfo();
                    foundInfo.FilePath = fileName;

                    versionList.Add(foundInfo);
                }

                foundInfo.FileVersion++;
            }

            VersionInfo.WriteInfoFile(fullPath, versionList, false);

            return versionList;
        }
    }

    public class DownloadKindInfo
    {
        public string KindName;
        public List<DownloadFileInfo> FileList;

        public DownloadKindInfo( string kindName, List<DownloadFileInfo> fileList)
        {
            KindName = kindName;
            FileList = fileList;
        }
    }
}
