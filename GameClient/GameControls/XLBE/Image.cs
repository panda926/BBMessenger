using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Image
    {
        public Image()
        {
        }

        public Image(string name, Rect area, Render_Texture texture)
        {
            name_ = name;
            area_ = area;
            texture_ = texture;
        }

        public Image(Image image)
        {
            name_ = image.name();
            area_ = image.area();
            texture_ = image.texture_;
        }

        public void release()
        {
            if (texture_ != null)
                texture_.release();
        }

        public string name() { return name_; }
        public void set_name( string name) { name_ = name; } 

        public Rect area() { return area_; }
        public void set_area(Rect area)  { area_ = area; }

        public double width() { return area_.width(); }
        public double height() { return area_.height(); }

        public void draw( Point pos)
        {
            draw(pos, new Size( area_.width(), area_.height()), new Point(0,0), 0);
        }

        public void draw(Point pos, Size size, Point hot, double roation )
        {
            Rect dest = new Rect(pos.x_, pos.y_, size.width_, size.height_);
            draw(dest, area_, new Color_Rect(), new Render_Blend(), hot, roation);
        }

        public void draw( Point pos, Size size)
        {
            Rect dest = new Rect(pos.x_, pos.y_, size.width_, size.height_);
            draw(dest, area_, new Color_Rect(), new Render_Blend(), new Point(0, 0), 0);
        }

        public void draw( Point pos, Size size, Point src_pos, Size src_size)
        {
            Rect dest = new Rect(pos.x_, pos.y_, size.width_, size.height_);
            Rect src = new Rect(src_pos.x_, src_pos.y_, src_size.width_, src_size.height_);
            draw(dest, src, new Color_Rect(), new Render_Blend(), new Point(0, 0), 0);
        }

        public void draw(Rect dest, Rect src, Color_Rect colors, Render_Blend blend, Point hot, double rotation)
        {
            Rect offSrc = new Rect(src);

            offSrc.origin_.x_ = 0;
            offSrc.origin_.y_ = 0;
            offSrc.size_.width_ = src.size_.width_;
            offSrc.size_.height_ = src.size_.height_;

            Root.instance().render_system().render_image(texture_, offSrc, dest, hot, rotation);
            //texture_.DrawImage(device, dest.Left, dest.Top, dest.Width, dest.Height, src.Left, src.Top, src.Width, src.Height, (int)blend);
        }

        private string name_; 
        private Rect area_;
        private Render_Texture texture_;
    }
}
