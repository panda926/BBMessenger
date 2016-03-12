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
    public partial class MngNotice : Form
    {
        public List<BoardInfo> _NoticeList = null;
        public BoardKind _boardKind = BoardKind.Notice;

        public MngNotice()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            NoticeView.RowTemplate.Height *= 2;

            if (_boardKind == BoardKind.Letter)
            {
                this.buttonAddUser.Visible = false;
                this.Text = "信息";
            }

            RefreshNoticeList();
        }

        public void RefreshNoticeList()
        {
            if (_boardKind == BoardKind.Notice)
                _NoticeList = Database.GetInstance().GetNotices( Users.ManagerInfo.Id);
            else
                _NoticeList = Database.GetInstance().GetLetters(null);

            if (_NoticeList == null)
            {
                MessageBox.Show("不能在资料库获取公告信息.");
                return;
            }

            NoticeView.Rows.Clear();

            for (int i = 0; i < _NoticeList.Count; i++)
            {
                BoardInfo noticeInfo = _NoticeList[i];


                string toUser = noticeInfo.SendId;

                if (_boardKind == BoardKind.Notice)
                {
                    switch (noticeInfo.UserKind)
                    {
                        case 0:
                            toUser = "总用户";
                            break;

                        case 1:
                            toUser = "广告员";
                            break;

                        case 2:
                            toUser = "服务员";
                            break;
                    }
                }

                NoticeView.Rows.Add(
                    noticeInfo.Id,
                    noticeInfo.UserId,
                    toUser,
                    noticeInfo.Title,
                    noticeInfo.Content,
                    noticeInfo.WriteTime.ToString("yy/MM/dd hh:mm")
                    );
            }

            labelSummary.Text = string.Format("总数: {0}", _NoticeList.Count);
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
           AddNotice addNotice = new AddNotice();

            if (addNotice.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("公告成功.");
                RefreshNoticeList();
            }
        }

        private void buttonEditUser_Click(object sender, EventArgs e)
        {
            if (NoticeView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择公告事项.");
                return;
            }

            int noticeId = Convert.ToInt32(NoticeView.SelectedRows[0].Cells["Id"].Value);

            BoardInfo noticeInfo = null;

            for (int i = 0; i < _NoticeList.Count; i++)
            {
                if (_NoticeList[i].Id == noticeId)
                {
                    noticeInfo = _NoticeList[i];
                    break;
                }
            }

            AddNotice editNotice = new AddNotice();

            editNotice._IsEditMode = true;
            editNotice._NoticeInfo = noticeInfo;

            editNotice.ShowDialog();

            //if (editNotice.ShowDialog() == DialogResult.OK)
            //{
            //    MessageBox.Show("공지사항수정이 성공하였습니다.");
            //    RefreshNoticeList();
            //}
        }

        private void buttonDelUser_Click(object sender, EventArgs e)
        {
            if (NoticeView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择公告事项.");
                return;
            }

            int noticeId = Convert.ToInt32( NoticeView.SelectedRows[0].Cells["Id"].Value );

            string question = string.Format("真要删除 {0}  吗?", noticeId);

            if (MessageBox.Show(question, "删除提问", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (Database.GetInstance().DelBoard(noticeId) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            string resultStr = string.Format("{0} 已被删除.", noticeId);
            MessageBox.Show(resultStr);

            RefreshNoticeList();

        }

    }
}
