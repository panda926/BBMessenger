using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using GameControls;
using System.Drawing;
using System.Windows.Forms;

namespace GameControls
{
    //根据基点画图.. 显示器外图片不画
    public struct tagBASE
    {
        public CPoint ptBase;		//基点
        public int nWidth;		//宽
        public int nHeight;	//高
    };

    public struct CMyD3DTexture  
    {
        Bitmap _bitmap;

        public void Dispose()
        {
            if (_bitmap == null)
                return;

            _bitmap.Dispose();
        }

	    //绘画函数
        public void LoadImage( Control hResInstance, Bitmap bitmap )
        {
            _bitmap = bitmap;
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

	    //绘画图片
        public bool DrawImage(Graphics g, int nXDest, int nYDest)
        {
            if (_bitmap == null)
                return false;

            GameGraphics.DrawImage(g, _bitmap, nXDest, nYDest, _bitmap.Width, _bitmap.Height, 0, 0);

            return true;
        }

        public bool DrawImage(Graphics g, int xDest, int nYDest, int width, int height, int srcX, int srcY)
        {
            if (_bitmap == null)
                return false;

            GameGraphics.DrawImage(g, _bitmap, xDest, nYDest, width, height, srcX, srcY);

            return true;
        }

        // added by usc at 2014/03/11
        public bool DrawImage(Graphics g, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int nSrcWidth, int nSrcHeight)
        {
            if (_bitmap == null)
                return false;

            GameGraphics.DrawImage(g, _bitmap, nXDest, nYDest, nDestWidth, nDestHeight, nXSource, nYSource, nSrcWidth, nSrcHeight);

            return true;
        }	    

        public bool DrawImage(Graphics g, tagBASE pBase, int nXDest, int nYDest)
        {
            if (_bitmap == null)
                return false;

            CRect rcImage = new CRect( nXDest, nYDest, nXDest + GetWidth(), nYDest + GetHeight());
	        CRect rcBack = new CRect( pBase.ptBase.x, pBase.ptBase.y, pBase.ptBase.x + pBase.nWidth, pBase.ptBase.y + pBase.nHeight );

            if (GameGraphics.IntersectRect(rcImage, rcBack))
                GameGraphics.DrawImage(g, _bitmap, nXDest - pBase.ptBase.x, nYDest - pBase.ptBase.y, GetWidth(), GetHeight(), 0, 0);
            
            return true;
        }

	    //绘画图片
        public bool DrawImage(Graphics g, tagBASE pBase, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource)
        {
            if (_bitmap == null)
                return false;

            CRect rcImage = new CRect( nXDest, nYDest, nXDest + nDestWidth, nYDest + nDestHeight);
	        CRect rcBack = new CRect( pBase.ptBase.x, pBase.ptBase.y, pBase.ptBase.x + pBase.nWidth, pBase.ptBase.y + pBase.nHeight );

            if (GameGraphics.IntersectRect(rcImage, rcBack))
                GameGraphics.DrawImage(g, _bitmap, nXDest - pBase.ptBase.x, nYDest - pBase.ptBase.y, nDestWidth, nDestHeight, nXSource, nYSource, nDestWidth, nDestHeight);

            return true;
        }

	    //绘画图片
        public bool DrawImage(Graphics g, tagBASE pBase, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int nSrcWidth, int nSrcHeight)
        {
            if (_bitmap == null)
                return false;

            CRect rcImage = new CRect(nXDest, nYDest, nXDest + nDestWidth, nYDest + nDestHeight);
            CRect rcBack = new CRect(pBase.ptBase.x, pBase.ptBase.y, pBase.ptBase.x + pBase.nWidth, pBase.ptBase.y + pBase.nHeight);

            if (GameGraphics.IntersectRect(rcImage, rcBack))
                GameGraphics.DrawImage(g, _bitmap, nXDest - pBase.ptBase.x, nYDest - pBase.ptBase.y, nDestWidth, nDestHeight, nXSource, nYSource, nSrcWidth, nSrcHeight);

            return true;
        }	    

        //绘画函数
        //绘画图片
        public bool DrawImage(Graphics g, tagBASE pBase, int nXDest, int nYDest, int cbAlpha)
        {
            if (_bitmap == null)
                return false;

            CRect rcImage = new CRect(nXDest, nYDest, nXDest + GetWidth(), nYDest + GetHeight());
            CRect rcBack = new CRect(pBase.ptBase.x, pBase.ptBase.y, pBase.ptBase.x + pBase.nWidth, pBase.ptBase.y + pBase.nHeight);

            if (GameGraphics.IntersectRect(rcImage, rcBack))
                GameGraphics.DrawImage(g, _bitmap, nXDest - pBase.ptBase.x, nYDest - pBase.ptBase.y, GetWidth(), GetHeight(), 0, 0, GetWidth(), GetHeight(), cbAlpha);

            return true;
        }

        //绘画图片
        bool DrawImage(Graphics g, tagBASE pBase, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int cbAlpha)
        {
            if (_bitmap == null)
                return false;

            CRect rcImage = new CRect(nXDest, nYDest, nXDest + nDestWidth, nYDest + nDestHeight);
            CRect rcBack = new CRect(pBase.ptBase.x, pBase.ptBase.y, pBase.ptBase.x + pBase.nWidth, pBase.ptBase.y + pBase.nHeight);

            if (GameGraphics.IntersectRect(rcImage, rcBack))
                GameGraphics.DrawImage(g, _bitmap, nXDest - pBase.ptBase.x, nYDest - pBase.ptBase.y, nDestWidth, nDestHeight, nXSource, nYSource, nDestWidth, nDestHeight, cbAlpha);

            return true;
        }

        //绘画图片
        bool DrawImage(Graphics g, tagBASE pBase, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int nSrcWidth, int nSrcHeight, int cbAlpha)
        {
            if (_bitmap == null)
                return false;

            CRect rcImage = new CRect(nXDest, nYDest, nXDest + nDestWidth, nYDest + nDestHeight);
            CRect rcBack = new CRect(pBase.ptBase.x, pBase.ptBase.y, pBase.ptBase.x + pBase.nWidth, pBase.ptBase.y + pBase.nHeight);

            if (GameGraphics.IntersectRect(rcImage, rcBack))
                GameGraphics.DrawImage(g, _bitmap, nXDest - pBase.ptBase.x, nYDest - pBase.ptBase.y, nDestWidth, nDestHeight, nXSource, nYSource, nSrcWidth, nSrcHeight, cbAlpha);

            return true;
        }

        bool BitBlt(Graphics g, tagBASE pBase, int nXPos, int nYPos)
        {
            if (_bitmap == null)
                return false;

            CRect rcImage = new CRect(nXPos, nYPos, nXPos + GetWidth(), nYPos + GetHeight());
            CRect rcBack = new CRect(pBase.ptBase.x, pBase.ptBase.y, pBase.ptBase.x + pBase.nWidth, pBase.ptBase.y + pBase.nHeight);

            if (GameGraphics.IntersectRect(rcImage, rcBack))
                GameGraphics.BitBlt(g, _bitmap, nXPos - pBase.ptBase.x, nYPos - pBase.ptBase.y, GetWidth(), GetHeight());

            return true;
        }

        bool AlphaDrawImage(Graphics g, tagBASE pBase, int nXPos, int nYPos, Color tranColor)
        {
            if (_bitmap == null)
                return false;

            CRect rcImage = new CRect(nXPos, nYPos, nXPos + GetWidth(), nYPos + GetHeight());
            CRect rcBack = new CRect(pBase.ptBase.x, pBase.ptBase.y, pBase.ptBase.x + pBase.nWidth, pBase.ptBase.y + pBase.nHeight);

            if (GameGraphics.IntersectRect(rcImage, rcBack))
                GameGraphics.DrawAlphaImage(g, _bitmap, nXPos - pBase.ptBase.x, nYPos - pBase.ptBase.y, GetWidth(), GetHeight(), 0, 0, GetWidth(), GetHeight(), tranColor);

            return true;
        }

        bool AlphaDrawImage(Graphics g, tagBASE pBase, int nXPos, int nYPos, int nDestWidth, int nDestHeight, int nXSource, int nYSource, Color tranColor)
        {
            if (_bitmap == null)
                return false;

            CRect rcImage = new CRect(nXPos, nYPos, nXPos + nDestWidth, nYPos + nDestHeight);
            CRect rcBack = new CRect(pBase.ptBase.x, pBase.ptBase.y, pBase.ptBase.x + pBase.nWidth, pBase.ptBase.y + pBase.nHeight);

            if (GameGraphics.IntersectRect(rcImage, rcBack))
                GameGraphics.DrawAlphaImage(g, _bitmap, nXPos - pBase.ptBase.x, nYPos - pBase.ptBase.y, nDestWidth, nDestHeight, nXSource, nYSource, nDestWidth, nDestHeight, tranColor);

            return true;
        }
    };

    class MyImage
    {
    }
}
