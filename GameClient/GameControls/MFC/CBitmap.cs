using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameControls
{
    public struct CBitmap
    {
        Bitmap _bitmap;

        public void CreateCompatibleBitmap(CDC dc, int width, int height)
        {
            if (_bitmap != null)
                return;

            _bitmap = new Bitmap(width, height, dc.GetGraphics());
        }

        public Bitmap GetBitmap()
        {
            return _bitmap;
        }
    }
}
