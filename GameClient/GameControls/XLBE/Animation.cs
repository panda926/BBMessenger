using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Animation 
    {
        public Animation( string name)
        {
            name_ = name;
        }

        public float delay() { return delay_; }
        public void set_delay(float delay) { delay_ = delay; }

        public string name() { return name_; }
        public void set_name(string name) { name_ = name; }

        public Image image(int index)
        {
            Image image = null;

            if (index < images.Count)
                image = images[index];

            return image;
        }

        public void add_image(Image image)
        {
            images.Add(image);
        }

        public int num_images()
        {
            return images.Count;
        }

	    //public void remove_image(int index);

        float delay_;
        string name_;
        List<Image> images = new List<Image>();
    };
}
