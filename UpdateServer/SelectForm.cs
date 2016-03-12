using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UpdateServer
{
    public partial class SelectForm : Form
    {
        public string BaseFolder = string.Empty;
        public string CurrentFolder = string.Empty;

        public List<string> SelectFileList = new List<string>();

        public SelectForm()
        {
            InitializeComponent();
        }

        private void SelectForm_Load(object sender, EventArgs e)
        {
            BaseFolder = CurrentFolder;
            RefreshView();
        }

        public void RefreshView()
        {
            SourceView.Rows.Clear();

            SourceView.Rows.Add("..\\");

            DirectoryInfo sourceFolderInfo = new DirectoryInfo(CurrentFolder);

            foreach (DirectoryInfo folderInfo in sourceFolderInfo.GetDirectories())
            {
                bool isSelect = false;

                foreach (DataGridViewRow row in TargetView.Rows)
                {
                    string selFolderName = Path.GetFullPath(row.Cells[0].Value.ToString());
                    if (CurrentFolder + folderInfo.Name + "\\" == selFolderName)
                    {
                        isSelect = true;
                        break;
                    }
                }

                if (isSelect == true)
                    continue;

                SourceView.Rows.Add(folderInfo.Name + "\\", "");
            }

            foreach (FileInfo fileInfo in sourceFolderInfo.GetFiles())
            {
                bool isSelect = false;

                foreach (DataGridViewRow row in TargetView.Rows)
                {
                    string selFileName = Path.GetFullPath(row.Cells[0].Value.ToString());
                    if (CurrentFolder + fileInfo.Name == selFileName)
                    {
                        isSelect = true;
                        break;
                    }
                }

                if (isSelect == true)
                    continue;

                SourceView.Rows.Add(fileInfo.Name, fileInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm"));
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in SourceView.SelectedRows)
            {
                if (r.Cells[0].Value.ToString() == "..\\")
                    continue;

                SourceView.Rows.Remove(r);

                string fileName = string.Format( "{0}{1}", CurrentFolder, r.Cells[0].Value.ToString());

                string strTime = string.Empty;
                
                if( r.Cells[1].Value != null )
                    strTime = r.Cells[1].Value.ToString();

                TargetView.Rows.Add(fileName, strTime);
            }
        }

        private void SourceView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = SourceView.Rows[e.RowIndex];

            string fileName = row.Cells[0].Value.ToString();

            if (fileName[fileName.Length - 1] != '\\')
                return;

            CurrentFolder += fileName;

            CurrentFolder = Path.GetFullPath(CurrentFolder);

            RefreshView();
        }

        private void buttonDeSelect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in TargetView.SelectedRows)
            {
                TargetView.Rows.Remove(r);

                string fileName = r.Cells[0].Value.ToString();

                bool bFolder = false;

                if (fileName[fileName.Length - 1] == '\\')
                {
                    bFolder = true;
                    fileName = fileName.Substring(0, fileName.Length - 1);
                }

                string folderName = Path.GetDirectoryName(fileName);

                if (folderName + "\\" == CurrentFolder)
                {
                    string strTime = string.Empty;

                    if (r.Cells[1].Value != null)
                        strTime = r.Cells[1].Value.ToString();

                    fileName = Path.GetFileName(fileName);

                    if (bFolder == true)
                        fileName += "\\";

                    SourceView.Rows.Add(fileName, strTime);
                }
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            SelectFileList.Clear();

            foreach (DataGridViewRow row in TargetView.Rows)
            {
                string fileName = row.Cells[0].Value.ToString();

                if (fileName[fileName.Length - 1] == '\\')
                {
                    SelectFolder(fileName);
                    continue;
                }

                int baseCount = BaseFolder.Trim().Length;
                fileName = fileName.Substring(baseCount);

                SelectFileList.Add(fileName);
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void SelectFolder(string folderName)
        {
            DirectoryInfo rootInfo = new DirectoryInfo(folderName);

            foreach (FileInfo fileInfo in rootInfo.GetFiles())
            {
                int baseCount = BaseFolder.Trim().Length;
                string fileName = fileInfo.FullName.Substring(baseCount);

                SelectFileList.Add(fileName);
            }

            foreach (DirectoryInfo folderInfo in rootInfo.GetDirectories())
            {
                SelectFolder(folderInfo.FullName);
            }
        }
    }
}
