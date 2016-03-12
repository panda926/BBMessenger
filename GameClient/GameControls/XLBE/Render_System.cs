using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace GameControls.XLBE
{
    public class Render_System
    {
        public Microsoft.DirectX.Direct3D.Device m_D3DDevice;						//设备对象

        public Render_System()
        {
        }
        public virtual void Dispose()
        {
        }

        //public abstract SortedDictionary<string, Config_Option> config_options();
        //public abstract void set_config_options(string name, string value);

        public void initialize( System.Windows.Forms.Control control)
        {
            PresentParameters pp = new PresentParameters();

            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;

            m_D3DDevice = new Device(0,
                                DeviceType.Hardware,
                                control,
                                CreateFlags.SoftwareVertexProcessing,
                                pp);

        }

        public void render_window( double dt )
        {
            if (m_D3DDevice == null)
                return;

            m_D3DDevice.BeginScene();

            Root.instance().scene_director().draw_scene(dt);

            m_D3DDevice.EndScene();
            m_D3DDevice.Present();
        }

        public void shutdown()
        {
            if (m_D3DDevice != null)
            {
                m_D3DDevice.Dispose();
                m_D3DDevice = null;
            }
        }

        //public abstract bool zbuffer();
        //public abstract void set_zbuffer(bool zbuffer);

        //public abstract bool texture_filter();
        //public abstract void set_texture_filter(bool filter);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual Rect caption_rect() const= 0;
        //public abstract Rect caption_rect();
        //public abstract void set_caption_rect(Rect rc);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual bool hide_mouse() const = 0;
        //public abstract bool hide_mouse();
        //public abstract void set_hide_mouse(bool hide);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual bool deactivated_on_focus_change() const= 0;
        //public abstract bool deactivated_on_focus_change();
        //public abstract void set_deactivated_on_focus_change(bool deactivated);

        //public abstract void set_world_matrix();
        //public abstract void set_view_matrix(float width, float height);
        //public abstract void set_projection_matrix(int width, int height);


        public Render_Texture add_texture(System.Drawing.Bitmap bitmap)
        {
            if( m_D3DDevice == null )
                return null;

            d3d_texture texture = new d3d_texture();

            texture._Texture = CreateDirectXTexture(bitmap, this.m_D3DDevice);
            texture._Sprite = new Microsoft.DirectX.Direct3D.Sprite(m_D3DDevice);

            return texture;
        }

        public Texture CreateDirectXTexture(System.Drawing.Bitmap bitmap, Device device)
        {
            Texture texture = new Texture(device
                                         , bitmap.Width, bitmap.Height, 1
                                         , Usage.AutoGenerateMipMap, Format.A8R8G8B8, Pool.Managed);

            System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(
                                     new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height)
                                     , System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
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

        //public abstract void remove_texture(Render_Texture tex);
        //public abstract void remove_texture(string name);
        //public abstract void remove_all_texture();

        public void render_image(Render_Texture texture, Rect srcRect, Rect destRect, Point hot, double rotation )
        {
            d3d_texture d3dTexture = (d3d_texture)texture;

            if (hot == null)
                hot = new Point(srcRect.width() / 2, srcRect.height() / 2);

            //if( rotation == 0 )
            //    rotateOffset = new System.Drawing.Point(0,0);

            render_image(d3dTexture._Sprite, d3dTexture._Texture, hot, rotation, (int)destRect.origin_.x_, (int)destRect.origin_.y_, (int)destRect.width(), (int)destRect.height(), (int)srcRect.origin_.x_, (int)srcRect.origin_.y_, (int)srcRect.width(), (int)srcRect.height(), 255);
        }

        private bool render_image(Microsoft.DirectX.Direct3D.Sprite sprite, Texture texture, Point hot, double fRadian, int nXDest, int nYDest, int nDestWidth, int nDestHeight, int nXSource, int nYSource, int nSourceWidth, int nSourceHeight, int cbAlpha)
        {
            const double M_PI = 3.14159265358979323846;

            if (sprite == null)
                return false;

            if (fRadian == M_PI)
            {
                float degrees1 = (float)(fRadian * (180 / M_PI));
            }

            float degrees = (float)(fRadian * (180 / M_PI));

            System.Drawing.Rectangle srcRectangle = new System.Drawing.Rectangle(nXSource, nYSource, nSourceWidth, nSourceHeight);
            //Color color = Color.FromArgb( cbAlpha, Color.White );

            sprite.Begin(SpriteFlags.SortTexture | SpriteFlags.AlphaBlend);
            //sprite.Draw(_Texture, srcRectangle, Vector3.Empty, new Vector3(nXDest, nYDest, 0), cbAlpha );

            //_Sprite.Transform = Matrix.Scaling((float)nDestWidth / nSourceWidth, (float)nDestHeight / nSourceHeight, 0.0f);

            //sprite.Transform = Matrix.Translation(-nSourceWidth / 2, -nSourceHeight / 2, 0);
            //sprite.Transform *= Matrix.RotationZ((float)fRadian);
            //sprite.Transform *= Matrix.Translation(nSourceWidth / 2, nSourceHeight / 2, 0);
            //sprite.Transform *= Matrix.Scaling((float)nDestWidth / nSourceWidth, (float)nDestHeight / nSourceHeight, 0.0f);
            //sprite.Transform *= Matrix.Translation(nXDest, nYDest, 0);

            //sprite.Transform = Matrix.Scaling((float)nDestWidth / nSourceWidth, (float)nDestHeight / nSourceHeight, 0.0f);
            //sprite.Transform *= Matrix.Translation(-nDestWidth / 2, -nDestWidth / 2, 0);
            //sprite.Transform = Matrix.RotationZ((float)fRadian);
            //sprite.Transform *= Matrix.Translation(nDestWidth / 2, nDestWidth / 2, 0);
            //sprite.Transform *= Matrix.Translation(nXDest, nYDest, 0);

            sprite.Transform = Matrix.RotationZ((float)fRadian) * Matrix.Scaling((float)nDestWidth / nSourceWidth, (float)nDestHeight / nSourceHeight, 0.0f) *Matrix.Translation(nXDest, nYDest, 0);

            sprite.Draw(texture, srcRectangle, new Vector3((int)hot.x_, (int)hot.y_, 0), new Vector3(0, 0, 0), System.Drawing.Color.FromArgb(cbAlpha, System.Drawing.Color.White).ToArgb());
            //sprite.Draw(texture, srcRectangle, new Vector3(0, 0, 0), new Vector3(0, 0, 0), System.Drawing.Color.FromArgb(cbAlpha, System.Drawing.Color.White).ToArgb());

            //_Sprite.Draw(_Texture, Vector3.Empty, new Vector3(nXDest, nYDest, 0), Color.White.ToArgb());
            sprite.End();

            //sprite.Begin(SpriteFlags.None);
            //sprite.Draw2D(_Texture, Rectangle.Empty, Rectangle.Empty,
            //              new Point(5, 5), Color.White);
            //sprite.End();

            return true;
        }
    }

    public class d3d_texture : Render_Texture
    {
        public Microsoft.DirectX.Direct3D.Texture _Texture;
        public Microsoft.DirectX.Direct3D.Sprite _Sprite;

        public override void release()
        {
            if (_Texture != null)
            {
                _Texture.Dispose();
                _Texture = null;
            }

            if (_Sprite != null)
            {
                _Sprite.Dispose();
                _Sprite = null;
            }
        }

    }
}
