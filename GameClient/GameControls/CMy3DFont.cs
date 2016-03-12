using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameControls
{
    public struct CMy3DFont
    {
        public void DrawText(Graphics g, string szUserInfo, CRect rcUserInfo, int flags, Color color)
        {
            CDC dc = new CDC();
            dc.SetGraphics(g);

            dc.SetTextColor(color);

            dc.DrawText(szUserInfo, rcUserInfo, flags);
        }

        public void DrawText(Graphics g, string szUserInfo, int xpos, int ypos, int flags, Color color)
        {
            CDC dc = new CDC();
            dc.SetGraphics(g);

            dc.SetTextColor(color);

            dc.DrawText(szUserInfo, xpos, ypos, flags);
        }

    }
}
