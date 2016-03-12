using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace GameControls.XLBE
{
    public class Font
    {
        string name_;
        Size size_;
        Color color_;

        Microsoft.DirectX.Direct3D.Font d3dFont_;
        Microsoft.DirectX.Direct3D.Sprite sprite_;

        public Font( string name )
        {
            name_ = name;
        }

        public virtual void Dispose()
        {
        }

        public string name()
        {
            return name_;
        }


        //public abstract int height();
        //public abstract int string_width(string s);

        public void set_size(Size size)
        {
            size_ = size;

            Device device = Root.instance().render_system().m_D3DDevice;

            if (device == null)
                return;

            System.Drawing.Font gdiFont = new System.Drawing.Font( name_, 10.0f, System.Drawing.FontStyle.Regular);

            d3dFont_ = new Microsoft.DirectX.Direct3D.Font(device, gdiFont);
            sprite_ = new Microsoft.DirectX.Direct3D.Sprite(device);
        }

        public void set_color(Color color)
        {
            color_ = color;
        }

        public int set_align(int align)
        {
            return 0;
        }

        public void draw_string(Point pt, string s, Color color)
        {
            sprite_.Begin(SpriteFlags.SortTexture | SpriteFlags.AlphaBlend);
            d3dFont_.DrawText(sprite_, s, (int)pt.x_, (int)pt.y_, System.Drawing.Color.FromArgb( color.red_, color.green_, color.blue_));
            sprite_.End();
        }
    }

}
