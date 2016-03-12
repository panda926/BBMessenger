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
    public partial class MngEnviroment : Form
    {
        public MngEnviroment()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            EnvInfo envInfo = Database.GetInstance().GetEnviroment();

            textNewAccount.Text = envInfo.NewCount.ToString();
            textBonusPoint.Text = envInfo.LoginBonusPoint.ToString();
            textChargePoint.Text = envInfo.ChargeBonusPoint.ToString();
            
            textImageUploadPath.Text = envInfo.ImageUploadPath;
            txtChargeSiteUrl.Text = envInfo.ChargeSiteUrl;

            textChargeGivePercent.Text = envInfo.ChargeGivePercent.ToString();
            textEveryDayPoint.Text = envInfo.EveryDayPoint.ToString();
            textCashRate.Text = envInfo.CashRate.ToString();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            EnvInfo envInfo = new EnvInfo();

            try
            {
                envInfo.NewCount = Convert.ToInt32(textNewAccount.Text);
                envInfo.LoginBonusPoint = Convert.ToInt32(textBonusPoint.Text);
                envInfo.ChargeBonusPoint = Convert.ToInt32(textChargePoint.Text);

                envInfo.ImageUploadPath = textImageUploadPath.Text;
                envInfo.ChargeSiteUrl = txtChargeSiteUrl.Text;

                envInfo.ChargeGivePercent = Convert.ToInt32(textChargeGivePercent.Text);
                envInfo.EveryDayPoint = Convert.ToInt32(textEveryDayPoint.Text);
                envInfo.CashRate = Convert.ToInt32(textCashRate.Text);
            }
            catch
            {
                MessageBox.Show("输入的信息不准确.");
                return;
            }

            if (Database.GetInstance().SetEnviroment(envInfo) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            MessageBox.Show("设置成功.");
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBonusPoint_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textChargePoint_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
