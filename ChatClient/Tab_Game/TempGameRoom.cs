using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ControlExs;
using ChatEngine;
using SicboClient;
using DzCardClient;
using HorseClient;
using GameControls;
using System.Net.Sockets;
using System.Drawing.Drawing2D;

namespace ChatClient
{
    public partial class TempGameRoom : FormEx
    {
        public GameInfo _GameInfo;
        public GameView _GameView;

        public TempGameRoom(GameInfo gameInfo)
        {
            InitializeComponent();

            _GameInfo = gameInfo;

            switch (gameInfo.Source)
            {
                case "Sicbo":
                    _GameView = new SicboView();
                    break;

                case "DzCard":
                    _GameView = new DzCardView();
                    break;

                case "Horse":
                    _GameView = new HorseView();
                    break;
            }

            if (_GameView == null)
                return;

            _GameView.Width = gameInfo.Width;
            _GameView.Height = gameInfo.Height;

            _GameView.SetClientSocket(Login._ClientEngine);
            _GameView.SetUserInfo(Login._UserInfo);

            this.Controls.Add(_GameView);

            this.Width = _GameInfo.Width;
            this.Height = _GameInfo.Height;

            if (_GameView is SicboView ||
                _GameView is HorseView)
                Login._ClientEngine.Send(NotifyType.Request_PlayerEnter, Login._UserInfo);
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
    }
}
