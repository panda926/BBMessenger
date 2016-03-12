using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GameControls.XLBE
{
    public class Imageset_Manager
    {
        Dictionary<string, Imageset> _imagesetDic = new Dictionary<string, Imageset>();

        public Imageset_Manager()
        {
        }

        public bool load_imageset(GameControls.XLBE.Resource_Manager.Imageset_Res imageRes)
        {
            try
            {
                Imageset imageSet = new Imageset( imageRes.name_);

                XDocument configXml = XDocument.Load(imageRes.path_);

                System.Drawing.Bitmap bitmap = null;
                var imageset = configXml.Descendants("Imageset");

                foreach (XElement element in imageset)
                {
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + (string)element.Attribute("Image");
                    bitmap = new System.Drawing.Bitmap(filePath);
                    break;
                }


                List<Image> imageList = new List<Image>();
                var images = configXml.Descendants("Image");

                foreach (XElement imageElement in images)
                {
                    string imageName = (string)imageElement.Attribute("Name");

                    Rect area = new Rect();
                    area.origin_.x_ = (int)imageElement.Attribute("XPos");
                    area.origin_.y_ = (int)imageElement.Attribute("YPos");
                    area.size_.width_ = (int)imageElement.Attribute("Width");
                    area.size_.height_ = (int)imageElement.Attribute("Height");

                    System.Drawing.Bitmap subBmp = new System.Drawing.Bitmap((int)area.width(), (int)area.height());
                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(subBmp);

                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, subBmp.Width, subBmp.Height);

                    g.DrawImage(bitmap, destRect, (int)area.origin_.x_, (int)area.origin_.y_, subBmp.Width, subBmp.Height, System.Drawing.GraphicsUnit.Pixel);
                    g.Dispose();

                    Render_Texture texture = Root.instance().render_system().add_texture(subBmp);

                    Image image = new Image(imageName, area, texture);
                    imageSet.insert_image(image);
                }

                _imagesetDic.Add(imageRes.name_, imageSet);
            }
            catch 
            {
                return false;
            }

            return true;
        }

        public Imageset imageset( string name)
        {
            Imageset imageSet = null;

            _imagesetDic.TryGetValue(name, out imageSet);

            return imageSet;
        }

        public void remove_all_imageset()
        {
            foreach (Imageset imageSet in _imagesetDic.Values)
                imageSet.release();

            _imagesetDic.Clear();
        }

        //public Imageset load_from_xml(string xml)
    }
}
