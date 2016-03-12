using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace GameControls.XLBE
{
    public class Animation_Manager 
    {
        Dictionary<string, Animation> animations_ = new Dictionary<string,Animation>();

        public Animation_Manager()
        {
        }

        public bool load_animation(GameControls.XLBE.Resource_Manager.Animation_Res aniRes)
        {
            try
            {
                XDocument configXml = XDocument.Load(aniRes.path_);

                Animation animation = new Animation(aniRes.name_);
                animations_.Add(aniRes.name_, animation);

                var images = configXml.Descendants("Image");

                foreach (XElement imageElement in images)
                {
                    string setName = (string)imageElement.Attribute("Set");
                    string imgName = (string)imageElement.Attribute("Name");

                    Image image = Root.instance().imageset_manager().imageset(setName).image(imgName);
                    animation.add_image(image);
                }

            }
            catch
            {
                return false;
            }

            return true;
        }

        public Animation animation(string name)
        {
            Animation animation = null;

            animations_.TryGetValue(name, out animation);

            return animation;
        }

        //void add_animation(Animation *ani);
        //void remove_animation(Animation *ani);
        //void remove_animation(const std::string &name);
        
        public void remove_all_animation()
        {
            animations_.Clear();
        }

        //Animation *load_from_xml(const std::string &xml);
    };
}
