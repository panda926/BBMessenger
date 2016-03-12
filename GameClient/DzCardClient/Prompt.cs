using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DzCardClient
{
    public partial class Prompt : Form
    {
	    //数据变量
	    string m_tStr;				//对白

        public Prompt()
        {
            InitializeComponent();
        }

        private void Prompt_Paint(object sender, PaintEventArgs e)
        {
	        //CPaintDC dc(this); 
            Graphics g = e.Graphics;

	        //设置 DC
	        //dc.SetBkMode(TRANSPARENT);
            Brush brush = new SolidBrush( Color.Black );
	        //dc.SetTextColor(RGB(0,0,0));
	        //dc.SelectObject(CSkinResourceManager::GetDefaultFont());

	        //创建字体
	        //CFont ViewFont;
	        //ViewFont.CreateFont(-12,0,0,0,400,0,0,0,134,3,2,1,1,TEXT("Arial"));
	        //CFont *pOldFont=dc.SelectObject(&ViewFont);
            Font ViewFont = new Font( "Arial", 12 );

	        Rectangle rcScore = new Rectangle(0,30,220,30+19);
	        g.DrawString( m_tStr,ViewFont, brush, rcScore);

	        //还原字体
	        //dc.SelectObject(pOldFont);
	        //ViewFont.DeleteObject();

	        return;
        }
    }
}
