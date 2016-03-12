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
    public partial class ErrorView : Form
    {
        private static List<string> _ErrorStrings = new List<string>();

        public ErrorView()
        {
            InitializeComponent();
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        } 

        public static void AddErrorString()
        {
            ErrorInfo errorInfo = BaseInfo.GetError();

            if (errorInfo.ErrorType == ErrorType.None)
                return;
            if (errorInfo.ErrorString == null)
                return;

            string str = string.Format("{0}:   {1}", BaseInfo.ConvDateToString(DateTime.Now), errorInfo.ErrorString);

            _ErrorStrings.Add(str);

            BaseInfo.SetError(ErrorType.None, null);
        }

        public void RefreshError()
        {
            // show error
            bool bNewError = false;

            int errorCount = _ErrorStrings.Count;

            if (errorCount > 0)
            {
                bNewError = true;

                for (int i = 0; i < errorCount; i++)
                {
                    ResultView.Items.Add(_ErrorStrings[i]);
                }

                _ErrorStrings.RemoveRange(0, errorCount);
            }

            int serverLogCount = Server.GetInstance()._errorLogs.Count;

            if (serverLogCount > 0)
            {
                bNewError = true;

                for (int i = 0; i < serverLogCount; i++)
                {
                    ResultView.Items.Add(Server.GetInstance()._errorLogs[i]);
                }

                Server.GetInstance()._errorLogs.RemoveRange(0, serverLogCount);
            }

            if (bNewError == true)
            {
                ResultView.SelectedIndex = ResultView.Items.Count - 1;
            }
        }

        private void ResultView_DoubleClick(object sender, EventArgs e)
        {
            if (ResultView.SelectedIndex < 0)
                return;

            string errorString = ResultView.Items[ResultView.SelectedIndex].ToString();

            MessageBox.Show(errorString);
        }
    }
}
