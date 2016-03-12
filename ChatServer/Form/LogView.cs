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
    public partial class LogView : Form
    {
        private static List<string> _LogStrings = new List<string>();

        public LogView()
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

        public static void AddLogString(string logString)
        {
            string str = string.Format("{0}:   {1}", BaseInfo.ConvDateToString(DateTime.Now), logString);
            _LogStrings.Add(str);
        }

        public void RefreshLog()
        {
            // show log
            bool bNewLog = false;

            int logCount = _LogStrings.Count;

            if (logCount > 0)
            {
                bNewLog = true;

                for (int i = 0; i < logCount; i++)
                {
                    ResultView.Items.Add(_LogStrings[i]);
                }

                _LogStrings.RemoveRange(0, logCount);
            }

            if( bNewLog )
                ResultView.SelectedIndex = ResultView.Items.Count - 1;
        }
    }
}
