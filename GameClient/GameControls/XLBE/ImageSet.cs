using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Imageset  
    {
        public Imageset( string name)
        {
            name_ = name;
        }

        public string name() { return name_; }
        public void set_name( string name) { name_ = name; } 

        public int image_count() { return images.Count; }
        
        public void insert_image(Image image)
        {
            images.Add(image.name(), image);
        }

        public Image image(string name)
        {
            Image image = null;

            images.TryGetValue(name, out image);

            return image;
        }

        public void release()
        {
            foreach (Image image in images.Values)
            {
                image.release();
            }

            images.Clear();
        }

        //public bool is_image_defined( string name);
        //public void define_image( string name, Rectangle area);
        //public void undefine_image( string name);
        //public void undefine_all_image();

        private string name_; 
        //private Render_Quad quad_;
        private Dictionary<string, Image> images = new Dictionary<string,Image>();
    }
}
