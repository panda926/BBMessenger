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
    public partial class MngGateway : Form
    {
        public MngGateway()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            comboKind.Items.Add("银行");
            comboKind.Items.Add("帐号");

            comboKind.SelectedIndex = 0;

            RefreshGatewayList();
        }

        public void RefreshGatewayList()
        {
            string queryStr = "select * from tblGateway ";

            string contentStr = textContent.Text.Trim();

            if (contentStr.Length > 0)
            {
                string field = "";

                switch (comboKind.SelectedIndex)
                {
                    case 0:
                        field += string.Format("Bank like '%{0}%'", contentStr);
                        break;

                    case 1:
                        field += string.Format("UserId like '%{0}%'", contentStr );
                        break;
                }

                if (field != "")
                    queryStr += string.Format(" where {0} ", field);
            }

            List<GatewayInfo> gatewayList = Database.GetInstance().GetGatewayList(queryStr);

            if (gatewayList == null)
            {
                MessageBox.Show("不能在资料库获取访问信息.");
                return;
            }

            GatewayView.Rows.Clear();

            foreach ( GatewayInfo gatewayInfo in gatewayList )
            {
                GatewayView.Rows.Add(
                    gatewayInfo.Bank,
                    gatewayInfo.UserId,
                    gatewayInfo.SalfStr,
                    gatewayInfo.Gateway
                    );

                GatewayView.Rows[GatewayView.Rows.Count - 1].Tag = gatewayInfo;
            }

            labelSummary.Text = string.Format("总数: {0}", gatewayList.Count);
        }



        private void buttonFind_Click(object sender, EventArgs e)
        {
            RefreshGatewayList();
        }

        private void buttonAddRoom_Click(object sender, EventArgs e)
        {
            AddGateway addGateway = new AddGateway();

            if (addGateway.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Gateway 작성이 성공하였습니다.");
                RefreshGatewayList();
            }
        }

        private void buttonEditRoom_Click(object sender, EventArgs e)
        {
            if (GatewayView.SelectedRows.Count == 0)
            {
                MessageBox.Show("편집할 Gateway을 먼저 선택하십시오.");
                return;
            }

            GatewayInfo gatewayInfo = (GatewayInfo)GatewayView.SelectedRows[0].Tag;

            AddGateway editRoom = new AddGateway();

            editRoom._IsEditMode = true;
            editRoom._GatewayId = gatewayInfo.Id;

            if (editRoom.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Gateway편집이 성공하였습니다.");
                RefreshGatewayList();
            }
        }

        private void buttonDelRoom_Click(object sender, EventArgs e)
        {
            if (GatewayView.SelectedRows.Count == 0)
            {
                MessageBox.Show("삭제할 Gateway을 먼저 선택하십시오.");
                return;
            }

            GatewayInfo gatewayInfo = (GatewayInfo)GatewayView.SelectedRows[0].Tag;

            string question = string.Format("gateway {0} 를 정말 삭제하겠습니까?", gatewayInfo.UserId);

            if (MessageBox.Show(question, "삭제문의", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (Database.GetInstance().DelGateway(gatewayInfo.Id) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            string resultStr = string.Format("gateway {0} 이 삭제되였습니다.", gatewayInfo.UserId);
            MessageBox.Show(resultStr);

            RefreshGatewayList();
        }
    }
}
