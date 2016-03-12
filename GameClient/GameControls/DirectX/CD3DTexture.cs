using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing.Imaging;
using ChatEngine;

namespace GameControls
{
    public class CD3DTexture
    {
        protected static Dictionary<Bitmap, Texture> _TextureDic = new Dictionary<Bitmap, Texture>();

        protected CSize m_ImageSize;						//图片大小

        Texture _Texture;
        Microsoft.DirectX.Direct3D.Sprite _Sprite;


        //纹理宽度
        public int GetWidth() 
        {
            return m_ImageSize.cx; 
        }

        //纹理高度
        public int GetHeight() 
        {
            return m_ImageSize.cy; 
        }

        public bool LoadImage(Device device, Bitmap bitmap, string typeName)
        {
            LoadImage( device, bitmap, typeName, 1, 1 );

            return true;
        }

        public bool LoadImage(Device device, Bitmap bitmap, int dwColorKey)
        {
            LoadImage(device, bitmap, string.Empty, 1, 1);

            return true;
        }


        public bool LoadImage(Device device, Bitmap bitmap, string typeName, int cols, int rows )
        {
            //return true;

            //_Texture = TextureLoader.FromFile(device, Application.StartupPath + @"\..\..\banana.bmp");

            //using (Bitmap bmp = new Bitmap(bounds.Width, bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            //{
            //    bitmapData = bmp.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            //    Marshal.Copy(screenData, 0, bitmapData.Scan0, screenData.Length);
            //    bmp.UnlockBits(bitmapData);

            //    texture = Texture.FromBitmap(d3dDevice, bmp, 0, Pool.SystemMemory);
            //}

            //texture = TextureLoader.FromStream(dev,
            //      Assembly.GetExecutingAssembly().GetManifestResourceStream(
            //      "Texture.Content.Banana.bmp"));

            //_Texture = Texture.FromBitmap(device, bitmap, 0, Pool.Managed);

            bool ret = _TextureDic.TryGetValue(bitmap, out _Texture);

            if( ret == false )
                _Texture = CreateDirectXTexture( bitmap, device );

            m_ImageSize.cx = bitmap.Width;
            m_ImageSize.cy = bitmap.Height;

            _Sprite = new Microsoft.DirectX.Direct3D.Sprite(device);

            return true;
        }

        public Texture CreateDirectXTexture( System.Drawing.Bitmap bitmap, Device device)
        {
            Texture texture = new Texture(device
                                         , bitmap.Width, bitmap.Height, 1
                                         , Usage.AutoGenerateMipMap, Format.A8R8G8B8, Pool.Managed);

            BitmapData bitmapData = bitmap.LockBits(
                                     new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height)
                                     , ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int pitch;
            GraphicsStream textureData = texture.LockRectangle(0, LockFlags.None, out pitch);

            //Debug.Assert(bitmapData.PixelFormat == PixelFormat.Format32bppArgb);
            //Debug.Assert(sizeof(int) == 4 && (bitmapData.Stride & 3) == 0 && (pitch & 3) == 0);

            unsafe
            {
                int* texturePointer = (int*)textureData.InternalDataPointer;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    int* bitmapLinePointer = (int*)bitmapData.Scan0 + y * (bitmapData.Stride / sizeof(int));
                    int* textureLinePointer = texturePointer + y * (pitch / sizeof(int));
                    int length = bitmap.Width;
                    while (--length >= 0)
                        *textureLinePointer++ = *bitmapLinePointer++;
                }
            }

            bitmap.UnlockBits(bitmapData);
            texture.UnlockRectangle(0);

            return texture;
        }

	    //创建纹理
	    public bool CreateImage(Device  pD3DDevice, int nWidth, int nHeight)
        {
            return DrawImage(pD3DDevice, new CPoint(0, 0), 0, 'z', 0, 0, m_ImageSize.cx, m_ImageSize.cy, 0, 0, m_ImageSize.cx, m_ImageSize.cy, 255);
        }

	    //绘画函数
	    
        //绘画图片
	    public bool DrawImage(Device  pD3DDevice, int nXDest, int nYDest)
        {
            return DrawImage(pD3DDevice, nXDest, nYDest, m_ImageSize.cx, m_ImageSize.cy, 0, 0 );
        }
	    
        //绘画图片
	    public bool DrawImage(Device  pD3DDevice, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource)
        {
            return DrawImage(pD3DDevice, new CPoint(0, 0), 0, 'z', nXDest, nYDest, nDestWidth, nDestHeight, nXSource, nYSource, m_ImageSize.cx - nXSource, m_ImageSize.cy - nYSource);
        }
	    
        //绘画图片
	    public bool DrawImage(Device  pD3DDevice, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int nSourceWidth, int nSourceHeight)
        { 
            return DrawImage( pD3DDevice, new CPoint(0,0), 0, 'z', nXDest, nYDest, nDestWidth, nDestHeight, nXSource, nYSource, nSourceWidth, nSourceHeight, 255);
        }

	    //绘画函数
	    //绘画图片
	    public bool DrawImage(Device  pD3DDevice, int nXDest, int nYDest, int  cbAlpha)
        {
            return DrawImage(pD3DDevice, new CPoint(0, 0), 0, 'z', nXDest, nYDest, m_ImageSize.cx, m_ImageSize.cy, 0, 0, cbAlpha);        
        }

        //绘画图片
	    public bool DrawImage(Device  pD3DDevice, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int  cbAlpha)
        {
            //ok
            return DrawImage(pD3DDevice, new CPoint(), 0, 'z', nXDest, nYDest, nDestWidth, nDestHeight, nXSource, nYSource, nDestWidth, nDestHeight, cbAlpha);
        }

        //绘画图片
	    public bool DrawImage(Device  pD3DDevice, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int nSourceWidth, int nSourceHeight, int  cbAlpha)
        {
            return DrawImage(pD3DDevice, new CPoint(0,0), 0, 'z', nXDest, nYDest, nDestWidth, nDestHeight, nXSource, nYSource, nSourceWidth, nSourceHeight, cbAlpha);
        }

	    //绘画函数
	    //绘画图片
	    public bool DrawImage(Device  pD3DDevice, CPoint ptRotationOffset, double fRadian, char chDirection, int nXDest, int nYDest)
        {
            return DrawImage(pD3DDevice, ptRotationOffset, fRadian, chDirection, nXDest, nYDest, m_ImageSize.cx, m_ImageSize.cy, 0, 0);
        }
	    
        //绘画图片
	    public bool DrawImage(Device  pD3DDevice, CPoint ptRotationOffset, double fRadian, char chDirection, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource)
        {
            return DrawImage(pD3DDevice, ptRotationOffset, fRadian, chDirection, nXDest, nYDest, nDestWidth, nDestHeight, nXSource, nYSource, m_ImageSize.cx-nXSource, m_ImageSize.cy-nYSource, 255);
        }
	    
        //绘画图片
        public bool DrawImage(Device pD3DDevice, CPoint ptRotationOffset, double fRadian, char chDirection, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int nSourceWidth, int nSourceHeight)
        {
            return DrawImage( pD3DDevice,  ptRotationOffset,  fRadian,  chDirection,  nXDest,  nYDest,  nDestWidth,  nDestHeight,  nXSource,  nYSource,  nSourceWidth,  nSourceHeight,  255);
        }

	    //绘画函数
	    //绘画图片
	    public bool DrawImage(Device  pD3DDevice, CPoint ptRotationOffset, double fRadian, char chDirection, int nXDest, int nYDest, int  cbAlpha)
        {
            return DrawImage( pD3DDevice,  ptRotationOffset,  fRadian,  chDirection,  nXDest,  nYDest, m_ImageSize.cx, m_ImageSize.cy, 0, 0, cbAlpha);
        }
	    
        //绘画图片
	    public bool DrawImage(Device  pD3DDevice, CPoint ptRotationOffset, double fRadian, char chDirection, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int  cbAlpha)
        {
            // ok
            return DrawImage(pD3DDevice, ptRotationOffset, fRadian, chDirection, nXDest, nYDest, nDestWidth, nDestHeight, nXSource, nYSource, nDestWidth, nDestHeight, cbAlpha);
        }

        //绘画图片
	    public bool DrawImage(Device  pD3DDevice, CPoint ptRotationOffset, double fRadian, char chDirection, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int nSourceWidth, int nSourceHeight, int  cbAlpha)
        {
            if (_Sprite == null)
                return false;

            if (fRadian == FishDefine.M_PI)
            {
                float degrees1 = (float)(fRadian * (180 / FishDefine.M_PI));
            }

            float degrees = (float)(fRadian * (180 / FishDefine.M_PI));

            Rectangle srcRectangle = new Rectangle(nXSource, nYSource, nSourceWidth, nSourceHeight);
            //Color color = Color.FromArgb( cbAlpha, Color.White );

            _Sprite.Begin(SpriteFlags.SortTexture | SpriteFlags.AlphaBlend);
            //sprite.Draw(_Texture, srcRectangle, Vector3.Empty, new Vector3(nXDest, nYDest, 0), cbAlpha );

            //_Sprite.Transform = Matrix.Scaling((float)nDestWidth / nSourceWidth, (float)nDestHeight / nSourceHeight, 0.0f);

            _Sprite.Transform = Matrix.RotationZ((float)-fRadian) * Matrix.Scaling((float)nDestWidth / nSourceWidth, (float)nDestHeight / nSourceHeight, 0.0f) * Matrix.Translation(nXDest, nYDest, 0);

            _Sprite.Draw(_Texture, srcRectangle, new Vector3(ptRotationOffset.x, ptRotationOffset.y, 0), new Vector3(0,0,0), Color.FromArgb( cbAlpha, Color.White ).ToArgb());

            //_Sprite.Draw(_Texture, Vector3.Empty, new Vector3(nXDest, nYDest, 0), Color.White.ToArgb());
            _Sprite.End();

            //sprite.Begin(SpriteFlags.None);
            //sprite.Draw2D(_Texture, Rectangle.Empty, Rectangle.Empty,
            //              new Point(5, 5), Color.White);
            //sprite.End();

            return true; 
        }

	    //绘画函数
	    //绘画图片
	    //public static bool DrawImage(Device  pD3DDevice, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int[] dwData, int nImageWidth, int nImageHeight){ return true; }

	    //内部函数
	    //获取资源
	    //public bool GetResourceInfo(LPCTSTR pszResource, LPCTSTR pszTypeName, tagResourceInfo & ResourceInfo){ return true; }

	    //辅助函数
	    //设置矩阵
	    //public void SetMatrix(Device  pD3DDevice, int nXDest, int nYDest, int nDestWidth, int nDestHeight){}
	    //输出位置
	    //public void SetWindowPos(tagD3DTextureVertex * pTextureVertex, int nXDest, int nYDest, int nDestWidth, int nDestHeight){}
	    //设置贴图
	    //public void SetTexturePos(Device  pD3DDevice, tagD3DTextureVertex * pTextureVertex, int nXSource, int nYSource, int nSourceWidth, int nSourceHeight){}
    }
}
