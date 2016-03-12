using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Resources;

namespace GameControls
{
    public struct CPngImage
    {
        public Bitmap _bitmap;

        public void Dispose()
        {
            if (_bitmap == null)
                return;

            _bitmap.Dispose();
        }

        public void LoadImage(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public void LoadFromResource( object hInstance, Bitmap bitmap)
        {
            LoadImage(bitmap);
        }

        public int GetWidth()
        {
            if (_bitmap == null)
                return 0;

            return _bitmap.Width;
        }

        public int GetHeight()
        {
            if (_bitmap == null)
                return 0;

            return _bitmap.Height;
        }

        public void SetTransparentColor(Color transColor)
        {
            if (_bitmap == null)
                return;

            //_bitmap.MakeTransparent(transColor);

            // modified by usc at 2014/04/28
            using (Bitmap bmp = new Bitmap(_bitmap))
            {
                bmp.MakeTransparent(transColor);

                _bitmap = new Bitmap(bmp);
            }
        }

        public void DrawImage(CDC DCBuffer, int left, int top)
        {
            if (_bitmap == null)
                return;

            GameGraphics.DrawImage(DCBuffer.GetGraphics(), _bitmap, left, top, GetWidth(), GetHeight(), 0, 0);
        }

        public void DrawImage(Graphics g, int left, int top, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight )
        {
            if (_bitmap == null)
                return;

            GameGraphics.DrawImage(g, _bitmap, left, top, width, height, srcX, srcY, srcWidth, srcHeight);
        }

        public void DrawImage(Graphics g, int left, int top, int width, int height, int srcX, int srcY)
        {
            if (_bitmap == null)
                return;

            GameGraphics.DrawImage(g, _bitmap, left, top, width, height, srcX, srcY);
        }

        public void DrawImage(Graphics g, int left, int top)
        {
            if (_bitmap == null)
                return;

            GameGraphics.DrawImage(g, _bitmap, left, top, GetWidth(), GetHeight(), 0, 0);
        }


        public void BitBlt( Graphics g, int xDest, int yDest )
        {
            if (_bitmap == null)
                return;

            GameGraphics.DrawImage(g, _bitmap, xDest, yDest, _bitmap.Width, _bitmap.Height, 0, 0);
        }
    }
}
