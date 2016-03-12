using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// 2014-01-06: GreenRose
using ControlExs;
using System.Drawing.Drawing2D;

// 2014-01-08: GreenRose
using System.Runtime.InteropServices;//DLLImport
using System.IO;
using ANYCHATAPI;

// 2014-04-01: GreenRose
using System.Diagnostics;

namespace UpdateClient
{
    public partial class ProgressForm : FormEx
    {
        public int _State = 0;
        public UpdateCheckInfo updateCheckInfo = new UpdateCheckInfo();

        public ProgressForm()
        {
            InitializeComponent();

            KillForIsRunning();
        }

        // 2014-04-01: GreenRose
        // 현재 ChatClient프로그램이 동작하고 있다면 모두 꺼버린다.
        private void KillForIsRunning()
        {
            try
            {
                Process[] pUpdateClient = Process.GetProcessesByName("UpdateClient");
                if (pUpdateClient.Length > 1)
                {
                    Environment.Exit(0);
                }

                Process[] pname = Process.GetProcessesByName("ChatClient");
                if(pname.Length != 0)
                {
                    for(int i = 0; i<pname.Length; i++)
                    {
                        pname[i].Kill();
                    }
                }
            }
            catch (System.Exception)
            { }
        }

        #region Override

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    cp.ExStyle |= (int)WindowStyle.WS_CLIPCHILDREN;
                }
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawFromAlphaMainPart(this, e.Graphics);
        }

        #endregion

        #region Private

        /// <summary>
        /// 绘制窗体主体部分白色透明层
        /// </summary>
        /// <param name="form"></param>
        /// <param name="g"></param>
        public static void DrawFromAlphaMainPart(Form form, Graphics g)
        {
            Color[] colors = 
            {
                Color.FromArgb(5, Color.White),
                Color.FromArgb(30, Color.White),
                Color.FromArgb(145, Color.White),
                Color.FromArgb(150, Color.White),
                Color.FromArgb(30, Color.White),
                Color.FromArgb(5, Color.White)
            };

            float[] pos = 
            {
                0.0f,
                0.04f,
                0.10f,
                0.90f,
                0.97f,
                1.0f      
            };

            ColorBlend colorBlend = new ColorBlend(6);
            colorBlend.Colors = colors;
            colorBlend.Positions = pos;

            RectangleF destRect = new RectangleF(0, 0, form.Width, form.Height);
            using (LinearGradientBrush lBrush = new LinearGradientBrush(destRect, colors[0], colors[5], LinearGradientMode.Vertical))
            {
                lBrush.InterpolationColors = colorBlend;
                g.FillRectangle(lBrush, destRect);
            }
        }


        private void SetStyles()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        #endregion

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            this.labelState.Text = "升级更新中 请稍等...";

            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;

            Updater updater = Updater.GetInstance();
            updater._ProgressForm = this;
            updater.Init();
        }

        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {            
            e.Cancel = true;
            return;
        }

        public void ProgressValue(int nVal)
        {
            this.progressUpdate.Value = nVal;
        }

        string bitUnit1 = null;
        string bitUnit2 = null;        
        public void ProgrssStateDisplay(string _name, double _perBit, double _capacity, double _currentcap)
        {
            //labelState.Text = _name;            
            if ((_capacity % 1000) > 0)
            {
                _capacity = _capacity / 1000;
                bitUnit1 = "MB";
            }
            else
                bitUnit1 = "KB";

            if ((_currentcap % 1000) > 0)
            {
                _currentcap = _currentcap / 1000;
                bitUnit2 = "MB";
            }
            else
                bitUnit2 = "KB";

            //lbl_speed.Text = Math.Round(_perBit, 1).ToString();
            lbl_progress.Text = Math.Round(_capacity, 1) + bitUnit1 + "/" + Math.Round(_currentcap, 1) + bitUnit2;            
        }
    }
}
