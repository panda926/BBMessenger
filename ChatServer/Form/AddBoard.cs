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
    public partial class AddBoard : Form
    {
        public bool _IsEditMode = false;
        
        public BoardInfo _BoardInfo = new BoardInfo();

        public AddBoard()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string Title = textTitle.Text.Trim();

            if (Title.Length == 0)
            {
                MessageBox.Show("请输入标题.");
                return;
            }

            string Source = textSource.Text.Trim();

            if (Source.Length == 0)
            {
                MessageBox.Show("请输入浏览地址.");
                return;
            }

            _BoardInfo.Kind = (int)BoardKind.Letter;
            _BoardInfo.Title = Title;
            _BoardInfo.Source = Source;

            bool ret = true;

            if( _IsEditMode == false )
            {
                ret = Database.GetInstance().AddBoard(_BoardInfo);
            }
            else
            {
                ret = Database.GetInstance().UpdateBoard(_BoardInfo);
            }

            if( ret == false )
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            if (_IsEditMode == true)
            {
                this.Text = "修改内容";
                buttonOk.Text = "设置";

                if ( _BoardInfo != null)
                {
                    textTitle.Text = _BoardInfo.Title;
                    textSource.Text = _BoardInfo.Source;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            PreviewVideo previewVideo = new PreviewVideo();
            previewVideo._VideoSource = textSource.Text.Trim();

            previewVideo.ShowDialog();
        }

    }
}
