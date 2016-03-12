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
    public partial class MngBoard : Form
    {
        List<BoardInfo> _BoardList = null;

        public MngBoard()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            //BoardView.RowTemplate.Height *= 3;

            RefreshBoardList();
        }

        public void RefreshBoardList()
        {
            //_BoardList = Database.GetInstance().GetBoards();

            //if (_BoardList == null)
            //{
            //    MessageBox.Show( "디비에서 게시판목록을 얻을수 없습니다.");
            //    return;
            //}

            //BoardView.Rows.Clear();

            //for (int i = 0; i < _BoardList.Count; i++)
            //{
            //    BoardInfo boardInfo = _BoardList[i];

            //    BoardView.Rows.Add(
            //        boardInfo.Id.ToString(),
            //        boardInfo.WriteTime.ToString("yyyy/MM/dd"),
            //        boardInfo.Title,
            //        boardInfo.Source
            //        );
            //}

            //labelSummary.Text = string.Format("총 게시판수: {0}", _BoardList.Count);
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            AddBoard addBoard = new AddBoard();

            if (addBoard.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("成功登陆论坛.");
                RefreshBoardList();
            }
        }

        private void buttonEditUser_Click(object sender, EventArgs e)
        {
            if (BoardView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需修改的内容.");
                return;
            }

            int BoardId = Convert.ToInt32( BoardView.SelectedRows[0].Cells["Id"].Value );

            BoardInfo BoardInfo = null;

            for (int i = 0; i < _BoardList.Count; i++)
            {
                if (_BoardList[i].Id == BoardId)
                {
                    BoardInfo = _BoardList[i];
                    break;
                }
            }

            AddBoard editBoard = new AddBoard();

            editBoard._IsEditMode = true;
            editBoard._BoardInfo = BoardInfo;

            if (editBoard.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("内容修改成功.");
                RefreshBoardList();
            }
        }

        private void buttonDelUser_Click(object sender, EventArgs e)
        {
            if (BoardView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需删除的内容.");
                return;
            }

            int BoardId = Convert.ToInt32( BoardView.SelectedRows[0].Cells["Id"].Value );

            string question = string.Format("真的要删除{0} 吗?", BoardId);

            if (MessageBox.Show(question, "删除提问", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (Database.GetInstance().DelBoard(BoardId) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            string resultStr = string.Format("{0} 内容已被删除.", BoardId);
            MessageBox.Show(resultStr);

            RefreshBoardList();

        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (BoardView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需御览的内容.");
                return;
            }

            int BoardId = Convert.ToInt32(BoardView.SelectedRows[0].Cells["Id"].Value);

            BoardInfo BoardInfo = null;

            for (int i = 0; i < _BoardList.Count; i++)
            {
                if (_BoardList[i].Id == BoardId)
                {
                    BoardInfo = _BoardList[i];
                    break;
                }
            }

            PreviewVideo previewVideo = new PreviewVideo();
            previewVideo._VideoSource = BoardInfo.Source;

            previewVideo.ShowDialog();
        }
    }
}
