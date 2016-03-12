using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameControls
{
    public struct CDC
    {
        Graphics _graphics;

        Font _font;
        Brush _textBrush;

        public void CreateCompatibleDC(CBitmap bitmap)
        {
            Bitmap bmp = bitmap.GetBitmap();

            _graphics = Graphics.FromImage(bmp);
        }

        public Graphics GetGraphics()
        {
            return _graphics;
        }

        public void SetGraphics(Graphics g)
        {
            _graphics = g;
        }

        public void SelectObject(CFont font)
        {
            _font = font.GetFont();
        }

        public const int TA_LEFT = 0;
        public const int TA_RIGHT = 2;
        public const int TA_CENTER = 6;

        public const int TA_TOP = 0;
        public const int TA_BOTTOM = 8;
        public const int TA_BASELINE = 24;


        public void SetTextAlign( int align )
        {
        }

        public void SetTextColor(Color color)
        {
            _textBrush = new SolidBrush(color);
        }

        public const int DT_TOP              =        0x00000000;
        public const int DT_LEFT             =        0x00000000;
        public const int DT_CENTER           =        0x00000001;
        public const int DT_RIGHT            =        0x00000002;
        public const int DT_VCENTER          =        0x00000004;
        public const int DT_BOTTOM           =        0x00000008;
        public const int DT_WORDBREAK        =        0x00000010;
        public const int DT_SINGLELINE       =        0x00000020;
        public const int DT_EXPANDTABS       =        0x00000040;
        public const int DT_TABSTOP          =        0x00000080;
        public const int DT_NOCLIP           =        0x00000100;
        public const int DT_EXTERNALLEADING  =        0x00000200;
        public const int DT_CALCRECT         =        0x00000400;
        public const int DT_NOPREFIX         =        0x00000800;
        public const int DT_INTERNAL = 0x00001000;

        public const int DT_END_ELLIPSIS = 0x00008000;

        public void DrawText(string str, CRect rect, int align)
        {
            if (_graphics == null)
                return;

            if (_font == null)
                _font = SystemFonts.DefaultFont;

            if (_textBrush == null)
                _textBrush = new SolidBrush(Color.Black);

            StringFormat stringFormat = new StringFormat();

            if ((align & DT_CENTER) != 0)
                stringFormat.LineAlignment = StringAlignment.Center;
            if ((align & DT_RIGHT) != 0)
                stringFormat.LineAlignment = StringAlignment.Far;

            if( (align & DT_VCENTER) != 0 )
                stringFormat.Alignment = StringAlignment.Center;
            if ((align & DT_BOTTOM) != 0)
                stringFormat.Alignment = StringAlignment.Far;

            stringFormat.FormatFlags = StringFormatFlags.NoWrap;

            _graphics.DrawString(str, _font, _textBrush, rect.GetRect(), stringFormat);

        }

        public void DrawText(string str, int x, int y, int align)
        {
            if (_graphics == null)
                return;

            if (_font == null)
                _font = SystemFonts.DefaultFont;

            if (_textBrush == null)
                _textBrush = new SolidBrush(Color.Black);

            StringFormat stringFormat = new StringFormat();

            if ((align & DT_CENTER) != 0)
                stringFormat.LineAlignment = StringAlignment.Center;
            if ((align & DT_RIGHT) != 0)
                stringFormat.LineAlignment = StringAlignment.Far;

            if ((align & DT_VCENTER) != 0)
                stringFormat.Alignment = StringAlignment.Center;
            if ((align & DT_BOTTOM) != 0)
                stringFormat.Alignment = StringAlignment.Far;

            stringFormat.FormatFlags = StringFormatFlags.NoWrap;

            _graphics.DrawString(str, _font, _textBrush, x, y, stringFormat);
        }

        public void BitBlt(int x, int y, int nWidth, int nHeight, CBitmap srcBitmap, int xSrc, int ySrc, int dwRop)
        {
            if( _graphics == null )
                return;

            GameGraphics.DrawImage(_graphics, srcBitmap.GetBitmap(), x, y, nWidth, nHeight, xSrc, ySrc);
        }

        public Graphics GetSafeHdc()
        {
            return _graphics;
        }
    }
}
