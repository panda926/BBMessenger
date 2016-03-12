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
    public partial class AddGateway : Form
    {
        public bool _IsEditMode = false;
        public int _GatewayId;

        public AddGateway()
        {
            InitializeComponent();
        }

        private void AddRoom_Load(object sender, EventArgs e)
        {
            if (_IsEditMode == true)
            {
                GatewayInfo gatewayInfo = Database.GetInstance().FindGateway(_GatewayId);

                if (gatewayInfo != null)
                {
                    textBank.Text = gatewayInfo.Bank;
                    textId.Text = gatewayInfo.UserId;
                    textSalf.Text = gatewayInfo.SalfStr;
                    textGateway.Text = gatewayInfo.Gateway;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string bank = textBank.Text.Trim();

            if (bank.Length == 0)
            {
                MessageBox.Show("请输入银行资料.");
                return;
            }

            string id = textId.Text.Trim();

            if (id.Length == 0)
            {
                MessageBox.Show("请输入频道名称.");
                return;
            }

            string salf = textSalf.Text.Trim();

            if (salf.Length == 0)
            {
                MessageBox.Show("请选择频道所有者.");
                return;
            }

            string gateway = textGateway.Text.Trim();

            if (gateway.Length == 0)
            {
                MessageBox.Show("请选择频道人数.");
                return;
            }

            GatewayInfo newInfo = new GatewayInfo();

            newInfo.Id = _GatewayId;
            newInfo.Bank = bank;
            newInfo.UserId = id;
            newInfo.SalfStr = salf;
            newInfo.Gateway = gateway;

            bool ret = false;

            if (_IsEditMode == false)
                ret = Database.GetInstance().AddGateway(newInfo);
            else
                ret = Database.GetInstance().UpdateGateway(newInfo);

            if ( ret == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }


    }
}
