using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameControls;

namespace DzCardClient
{
    public partial class ShowHand : Form
    {
        int		m_bTimes;		//时间数目

        //资源变量
        Bitmap						m_ImageBack;						//背景图案

        //控件变量
        PictureButton						m_btOK = new PictureButton();						//继续按钮
        PictureButton						m_btCancel = new PictureButton();						//退出按钮

        public ShowHand()
        {
            InitializeComponent();

          	m_ImageBack = Properties.Resources.SHOWHAND_COMFIRE;

            m_btOK.Create(true, true, new Rectangle(), this, 0 );
            m_btCancel.Create(true, true, new Rectangle(), this, 0 );

            m_btOK.SetButtonImage( Properties.Resources.BT_YES1 );
            m_btCancel.SetButtonImage( Properties.Resources.BT_NO1 );

            m_btOK.Click += OnOK;
            m_btCancel.Click += OnCancel;

            this.ClientSize = new Size( m_ImageBack.Width, m_ImageBack.Height );

            m_btOK.Left = (this.Width / 2 - m_btOK.Width) / 2;
            m_btOK.Top = this.ClientSize.Height - m_btOK.Height -2;

            m_btCancel.Left = (this.Width / 2 - m_btCancel.Width) / 2 + this.Width / 2;
            m_btCancel.Top = m_btOK.Top;

            m_bTimes = 5;

            timer1.Interval = 5 * 1000;
            timer1.Enabled = true;

            timer2.Interval = 1000;
            timer2.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            this.Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
		    m_bTimes--;
            Invalidate();
        }

        private void ShowHand_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

	        //绘画背景
	        //CImageHandle ImageHandle(&m_ImageBack);
	        GameGraphics.DrawImage( g, m_ImageBack, 0,0, m_ImageBack.Width, m_ImageBack.Height, 0, 0 );

	        //设置 DC
	        //dc.SetBkMode(TRANSPARENT);
            Brush textbrush = new SolidBrush( Color.Black );
	        //dc.SetTextColor(RGB(0,0,0));
	        //dc.SelectObject(CSkinResourceManager::GetDefaultFont());

	        //创建字体
	        Font ViewFont = new Font("Arial", 15);
	        //ViewFont.CreateFont(-15,0,0,0,400,0,0,0,134,3,2,1,1,TEXT("Arial"));
	        //CFont *pOldFont=dc.SelectObject(&ViewFont);

            string tCh = string.Format( "{0}", m_bTimes );
	        //TCHAR tCh[128]=TEXT("");
	        //_snprintf(tCh,sizeof(tCh),TEXT("%ld"),m_bTimes);

	        Rectangle rcScore = new Rectangle(85,10,85+65,10+19);
	        GameGraphics.DrawString( g, tCh, ViewFont, textbrush, rcScore );

	        //还原字体
	        //dc.SelectObject(pOldFont);
	        //ViewFont.DeleteObject();        
        }

        private void OnOK( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.OK;
        }

        private void OnCancel( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
