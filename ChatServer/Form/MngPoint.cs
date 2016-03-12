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
    public partial class MngPoint : Form
    {
        public int _Kind = (int)PointKind.Bonus;

        public MngPoint()
        {
            InitializeComponent();
        }

        private void MngCash_Load(object sender, EventArgs e)
        {
            if (_Kind != (int)PointKind.Bonus)
                btnAdd.Visible = false;

            comboYear.Items.Add("全部");
            comboMonth.Items.Add("全部");
            comboDay.Items.Add("全部");

            for (int i = DateTime.Now.Year - 5; i < DateTime.Now.Year + 5; i++)
            {
                comboYear.Items.Add(i);

                if (i == DateTime.Now.Year)
                    comboYear.SelectedIndex = comboYear.Items.Count - 1;
            }

            for (int i = 1; i <= 12; i++)
            {
                comboMonth.Items.Add(i);

                if (i == DateTime.Now.Month)
                    comboMonth.SelectedIndex = comboMonth.Items.Count - 1;
            }

            for (int i = 1; i <= 31; i++)
            {
                comboDay.Items.Add(i);

                if (i == DateTime.Now.Day)
                    comboDay.SelectedIndex = comboDay.Items.Count - 1;
            }

            RefreshPointHistoryList();
        }

        public void RefreshPointHistoryList()
        {
            int year = 0;
            int month = 0;
            int day = 0;

            try
            {
                year = Convert.ToInt32(comboYear.Text);
                month = Convert.ToInt32(comboMonth.Text);
                day = Convert.ToInt32(comboDay.Text);
            }
            catch
            {
            }


            List<PointHistoryInfo> pointList = Database.GetInstance().GetDayPointHistoryList( _Kind, year, month, day);

            if (pointList == null)
            {
                MessageBox.Show("不能在资料库获取积分信息.");
                return;
            }

            HistoryView.Rows.Clear();

            for (int i = 0; i < pointList.Count; i++)
            {
                PointHistoryInfo historyInfo = pointList[i];

                HistoryView.Rows.Add(
                        historyInfo.TargetId,
                        historyInfo.Point,
                        historyInfo.Content,
                        BaseInfo.ConvDateToString(historyInfo.AgreeTime)
                    );

            }

            //labelSummary.Text = string.Format("총건수: {0}", _ChargeList.Count);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddPoint addPoint = new AddPoint();

            if (addPoint.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功.");

                View._isUpdateUserList = true;
                RefreshPointHistoryList();
            }
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            RefreshPointHistoryList();
        }

    }
}
