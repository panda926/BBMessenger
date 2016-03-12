using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameControls.XLBE
{
    public class Sprite : Node
    {
        public Sprite()
        {
        }

        public Sprite(Image image)
        {
            SetImage(image);
        }

        public override void draw()
        {
            if (visible_ == false)
                return;

            if (image_ == null)
                return;

            if (scale_.width_ != 1.0)
                scale_.width_ = scale_.width_;

            //Size size = new Size(image_.area().width() * scale_.width_, image_.area().height() * scale_.height_);
            Size size = new Size(content_size_.width_ * scale_.width_, content_size_.height_ * scale_.height_);
            
            image_.draw(position_absolute(), size, hot_,rotation_);
        }

        public void set_display_image(Image image)
        {
            SetImage(image);
        }

        public void set_display_image(Animation animation, int index)
        {
            SetImage( animation.image(index));
        }

        public bool is_image_displayed(Image image)
        {
            if (image_ != null)
                return true;

            return false;
        }

        public Image displayed_image()
        {
            return image_;
        }

        private void SetImage(Image image)
        {
            image_ = image;

            Size size = content_size();

            if (image_ != null)
                size = new Size(image_.width(), image_.height());

            set_content_size(size);
        }

        Image image_;
    }
}
