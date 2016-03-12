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
    public partial class MngEvaluation : Form
    {
        public string _UserId = null;

        public MngEvaluation()
        {
            InitializeComponent();
        }

        private void MngCash_Load(object sender, EventArgs e)
        {
            if (_UserId == null)
                return;

            List<EvalHistoryInfo> evalHistoryList = Database.GetInstance().GetAllEvalHistoryList(_UserId);

            if (evalHistoryList == null)
                return;

            for (int i = 0; i < evalHistoryList.Count; i++)
            {
                EvalHistoryInfo historyInfo = evalHistoryList[i];

                EvalHistoryView.Rows.Add(
                    historyInfo.BuyerId,
                    historyInfo.Value,
                    BaseInfo.ConvDateToString( historyInfo.EvalTime )
                );
            }
        }
    }
}
