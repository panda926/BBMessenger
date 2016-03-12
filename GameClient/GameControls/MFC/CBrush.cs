using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameControls
{
    public struct CBrush
    {
        Brush _brush;

        public static Color RGB(int r, int g, int b)
        {
            return Color.FromArgb(r, g, b);
        }

        public void CreateSolidBrush(Color crColor)
        {
            _brush = new SolidBrush(crColor);
        }
    }
}
