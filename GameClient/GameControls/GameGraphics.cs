using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace GameControls
{
    public class GameGraphics
    {
        [DllImport("user32.dll")]
        public static extern IntPtr BeginDeferWindowPos(int nNumWindows);

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOACTIVATE = 0x0010;
        public const uint SWP_NOCOPYBITS = 0x0100;

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);

        [DllImport("user32.dll")]
        public static extern IntPtr DeferWindowPos(IntPtr hWinPosInfo, IntPtr hWnd,
           int hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll")]
        public static extern bool EndDeferWindowPos(IntPtr hWinPosInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(String str);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern int BitBlt(
            IntPtr hdcDest,     // handle to destination DC (device context)
            int nXDest,         // x-coord of destination upper-left corner
            int nYDest,         // y-coord of destination upper-left corner
            int nWidth,         // width of destination rectangle
            int nHeight,        // height of destination rectangle
            IntPtr hdcSrc,      // handle to source DC
            int nXSrc,          // x-coordinate of source upper-left corner
            int nYSrc,          // y-coordinate of source upper-left corner
            System.Int32 dwRop  // raster operation code
            );

        public enum BITBLT
        {
            SRCCOPY = 0x00CC0020, /* dest = source*/
            SRCPAINT = 0x00EE0086, /* dest = source OR dest*/
            SRCAND = 0x008800C6, /* dest = source AND dest*/
            SRCINVERT = 0x00660046, /* dest = source XOR dest*/
            SRCERASE = 0x00440328, /* dest = source AND (NOT dest )*/
            NOTSRCCOPY = 0x00330008, /* dest = (NOT source)*/
            NOTSRCERASE = 0x001100A6, /* dest = (NOT src) AND (NOT dest) */
            MERGECOPY = 0x00C000CA, /* dest = (source AND pattern)*/
            MERGEPAINT = 0x00BB0226, /* dest = (NOT source) OR dest*/
            PATCOPY = 0x00F00021, /* dest = pattern*/
            PATPAINT = 0x00FB0A09, /* dest = DPSnoo*/
            PATINVERT = 0x005A0049, /* dest = pattern XOR dest*/
            DSTINVERT = 0x00550009, /* dest = (NOT dest)*/
            BLACKNESS = 0x00000042, /* dest = BLACK*/
            WHITENESS = 0x00FF0062, /* dest = WHITE*/
        };

        static public void ZeroMemory(int[] intArray)
        {
            Array.Clear(intArray, 0, intArray.Length);
        }

        static public bool IntersectRect(CRect rect1, CRect rect2)
        {
            if (rect1.left == rect2.left && rect1.top == rect2.top && rect1.Width() == rect2.Width() && rect1.Height() == rect2.Height())
                return false;

            return true;
        }

        static public void DrawImage(Graphics g, Image image, int dstX, int dstY, int width, int height, int srcX, int srcY)
        {
            DrawImage(g, image, dstX, dstY, width, height, srcX, srcY, width, height);
        }

        static public void DrawImage(Graphics g, Image image, int dstX, int dstY, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Rectangle dstRect = new Rectangle(dstX, dstY, width, height);
            Rectangle srcRect = new Rectangle(srcX, srcY, srcWidth, srcHeight);

            g.DrawImage(image, dstRect, srcRect, GraphicsUnit.Pixel);
        }

        static public void DrawImage(Graphics g, Image image, int dstX, int dstY, int width, int height, int srcX, int srcY, int cbAlpha)
        {
            DrawImage(g, image, dstX, dstY, width, height, srcX, srcY, width, height, cbAlpha);
        }

        static public void DrawImage(Graphics g, Image image, int dstX, int dstY, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight, int cbAlpha)
        {
            Rectangle dstRect = new Rectangle(dstX, dstY, width, height);
            Rectangle srcRect = new Rectangle(srcX, srcY, srcWidth, srcHeight);

            System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix();
            cm.Matrix33 = (float)(cbAlpha / 100f);

            System.Drawing.Imaging.ImageAttributes attr = new System.Drawing.Imaging.ImageAttributes();
            attr.SetColorMatrix(cm);

            g.DrawImage(image, dstRect, srcX, srcY, width, height, GraphicsUnit.Pixel, attr);
        }

        static public void DrawAlphaImage(Graphics g, Image image, int dstX, int dstY, int width, int height, int srcX, int srcY, Color tranColor)
        {
            DrawAlphaImage(g, image, dstX, dstY, width, height, srcX, srcY, width, height, tranColor);
        }

        static public void DrawAlphaImage(Graphics g, Image image, int dstX, int dstY, int width, int height, int srcX, int srcY, int srcWidth, int srcHeight, Color tranColor)
        {
            Rectangle dstRect = new Rectangle(dstX, dstY, width, height);
            Rectangle srcRect = new Rectangle(srcX, srcY, srcWidth, srcHeight);

            System.Drawing.Imaging.ImageAttributes attr = new System.Drawing.Imaging.ImageAttributes();
            attr.SetColorKey(tranColor, tranColor);

            g.DrawImage(image, dstRect, srcX, srcY, width, height, GraphicsUnit.Pixel, attr);

        }

        static public void BitBlt(Graphics g, Image image, int dstX, int dstY, int width, int height)
        {
            IntPtr display_dc = g.GetHdc();

            Graphics backingStoreGraphics = Graphics.FromImage(image);
            IntPtr backingStoreDC = backingStoreGraphics.GetHdc();

            BitBlt(display_dc, 0, 0, width, height, backingStoreDC, 0, 0, (int)BITBLT.SRCCOPY);
        }

        static public void DrawString(Graphics g, string str, Font viewFont, Brush textBrush, Rectangle rect)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.FormatFlags = StringFormatFlags.NoWrap;

            g.DrawString(str, viewFont, textBrush, rect, stringFormat);
        }

        

        static public Cursor LoadCursorFromResource(byte[] curResource)
        {
            Cursor result = null;

            try
            {
                string path = Path.GetTempFileName();
                File.WriteAllBytes(path, curResource);
                result = new Cursor(LoadCursorFromFile(path));
                File.Delete(path);
            }
            catch
            {
                result = Cursors.Default;
            }

            return result;
        }


        //整性变字符
        public static string GetGoldString(int lGold)
        {
            string szString = string.Empty;

            int lTemp = (int)lGold;
            if (lGold < 0L)
                return szString;

            //处理小数点
            int dwCharBit = 0;
            //lGold+=0.001;
            //if(lGold-lTemp>0)
            //{
            //	lTemp = (LONG)((lGold-lTemp)*100);

            //	bool bZero=(lTemp<10)?true:false;

            //	//转换字符
            //	do
            //	{
            //		szString[dwCharBit++]=(TCHAR)(lTemp%10+TEXT('0'));
            //		lTemp/=10;
            //	} while (lTemp>0L);

            //	//加0位
            //	if(bZero)szString[dwCharBit++]=TEXT('0');

            //	szString[dwCharBit++]=TEXT('.');
            //}

            //转换字符
            lTemp = (int)lGold;
            int dwNumBit = 0;
            do
            {
                dwNumBit++;
                szString = (lTemp % 10).ToString() + szString;
                if (dwNumBit % 3 == 0)
                    szString = "," + szString;
                lTemp /= 10;
            } while (lTemp > 0L);

            //调整字符
            if (szString[0] == ',')
                szString = szString.Substring(1, szString.Length-1);

            //尾头交换

            return szString;
        }

    }
}
