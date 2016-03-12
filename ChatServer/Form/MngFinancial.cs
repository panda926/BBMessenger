using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChatServer
{
    public partial class MngFinancial : Form
    {
        public MngFinancial()
        {
            InitializeComponent();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            Database.GetInstance().GetFinanceInfo(2014, 2);
        }
    }

    public class FinanceInfo
    {
        public string dayNo;
        public int chargeSum;
    }
}
