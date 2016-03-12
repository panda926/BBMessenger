﻿using System;
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
    public partial class MngPrivilege : Form
    {
        public string _UserId = null;

        public MngPrivilege()
        {
            InitializeComponent();
        }

        private void MngPrivilege_Load(object sender, EventArgs e)
        {
            if (_UserId == null)
                return;

            UserInfo userInfo = Database.GetInstance().FindUser(_UserId);

            if (userInfo == null)
                return;

            textMaxBuyers.Text = userInfo.MaxBuyers.ToString();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            int maxBuyers = 0;

            try
            {
                maxBuyers = Convert.ToInt32( textMaxBuyers.Text );
            }
            catch
            {
                MessageBox.Show("输入信息不准确.");
                return;
            }

            UserInfo userInfo = Database.GetInstance().FindUser(_UserId);

            userInfo.MaxBuyers = maxBuyers;

            if (Database.GetInstance().UpdateUser(userInfo) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
