using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GameControls.XLBE
{
    public class Resource_Manager
    {
        public enum Res_Type
        {
            RES_TYPE_IMAGESET,
            RES_TYPE_ANIMATION,
            RES_TYPE_SOUND,
            RES_TYPE_MUSIC

        }

        public class Base_Res
        {
            public Res_Type type_;
            public string name_;
            public string path_;
            public string group_;

            public virtual void Dispose()
            {
            }
            public virtual void delete_resource()
            {
            }
        }

        public class Imageset_Res : Base_Res
        {
            public Imageset_Res()
            {
                type_ = Res_Type.RES_TYPE_IMAGESET;
            }
            public override void Dispose()
            {
                base.Dispose();
            }

            public override void delete_resource()
            {
            }
        }

        public class Animation_Res : Base_Res
        {
            public Animation_Res()
            {
                type_ = Res_Type.RES_TYPE_ANIMATION;
            }
            public override void Dispose()
            {
                base.Dispose();
            }
            //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
            //		virtual void delete_resource();
        }

        public class Sound_Res : Base_Res
        {
            public Sound_Res()
            {
                type_ = Res_Type.RES_TYPE_SOUND;
            }
            public override void Dispose()
            {
                base.Dispose();
            }
            //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
            //		virtual void delete_resource();
        }

        public class Music_Res : Base_Res
        {
            public Music_Res()
            {
                type_ = Res_Type.RES_TYPE_MUSIC;
            }
            public override void Dispose()
            {
                base.Dispose();
            }
            //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
            //		virtual void delete_resource();
        }


        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Resource_Manager();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        public void get_attribute(Base_Res baseRes, XElement element)
        {
            baseRes.name_ = (string)element.Attribute("Name");
            baseRes.path_ = AppDomain.CurrentDomain.BaseDirectory + (string)element.Attribute("Path");
        }

        public void prase_resources_file(string file)
        {
            default_path_ = AppDomain.CurrentDomain.BaseDirectory + file.Trim();
            res_group_map_.Clear();
            loaded_groups_.Clear();
            
            imageset_map_.Clear();
            animation_map_.Clear();

            XDocument xmlDocument = XDocument.Load(default_path_);
            var resources = xmlDocument.Descendants("Resources");

            foreach (XElement resource in resources)
            {
                string resourceName = (string)resource.Attribute("Name");

                List<Base_Res> group = new List<Base_Res>();

                foreach (XElement image in resource.Descendants("Imageset"))
                {
                    Imageset_Res imageRes = new Imageset_Res();
                    get_attribute(imageRes, image);
                    group.Add(imageRes);
                }

                foreach (XElement sound in resource.Descendants("Sound"))
                {
                    Sound_Res soundRes = new Sound_Res();
                    get_attribute(soundRes, sound);
                    group.Add(soundRes);
                }

                foreach (XElement music in resource.Descendants("Music"))
                {
                    Music_Res musicRes = new Music_Res();
                    get_attribute(musicRes, music);
                    group.Add(musicRes);
                }

                foreach (XElement animation in resource.Descendants("Animation"))
                {
                    Animation_Res animationRes = new Animation_Res();
                    get_attribute(animationRes, animation);
                    group.Add(animationRes);
                }

                res_group_map_.Add(resourceName, group);
            }
        }

        public int number_resources(string group)
        {
            List<Base_Res> group_list;
            if (res_group_map_.TryGetValue(group, out group_list) == false)
                return 0;

            return group_list.Count;
        }

        public void start_load_resources(string group)
        {
            if (loaded_groups_.Contains(group) == true)
                return;

            if (res_group_map_.TryGetValue(group, out cur_res_group_list_) == false)
                return;

            cur_res_group_ = group;

            cur_res_group_listor_ = cur_res_group_list_.GetEnumerator();
            cur_res_group_listor_.MoveNext();
        }

        public bool load_next_resource()
        {
            try
            {
                if (loaded_groups_.Contains(cur_res_group_) == true)
                    return false;

                Base_Res baseRes = cur_res_group_listor_.Current;

                if (baseRes == null)
                    return false;

                if (baseRes is Imageset_Res)
                {
                    Imageset_Res imageRes = (Imageset_Res)baseRes;

                    if (Root.instance().imageset_manager().load_imageset(imageRes) == true)
                        imageset_map_.Add(imageRes.name_, imageRes);
                }
                else if (baseRes is Animation_Res)
                {
                    Animation_Res aniRes = (Animation_Res)baseRes;

                    if (Root.instance().animation_manager().load_animation(aniRes) == true)
                        animation_map_.Add(aniRes.name_, aniRes);
                }
                else if (baseRes is Sound_Res)
                {
                    Sound_Res soundRes = (Sound_Res)baseRes;

                    if (Root.instance().sound_manager().load_sound(soundRes.name_, soundRes.path_, true) == true)
                        sound_map_.Add(soundRes.name_, soundRes);

                }
                else if (baseRes is Music_Res)
                {
                    Music_Res musicRes = (Music_Res)baseRes;

                    if (Root.instance().music_manager().load_sound(musicRes.name_, musicRes.path_, false) == true)
                        music_map_.Add(musicRes.name_, musicRes);
                }

                cur_res_group_listor_.MoveNext();

                if (cur_res_group_listor_.Current == null)
                {
                    if (this.loaded_groups_.Contains(this.cur_res_group_) == false)
                        this.loaded_groups_.Add(this.cur_res_group_);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void load_resources(string group)
        {
            start_load_resources(group);

            //System.IO.StreamReader sr = new System.IO.StreamReader(default_path_);
            //string content = sr.ReadToEnd();

            while (true)
            {
                if (load_next_resource() == false)
                    break;
            }
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void remove_resources(string group);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	bool is_group_loaded(string group);
        public string current_resource_group()
        {
            return cur_res_group_;
        }
        public List<Base_Res> current_resource_group_list()
        {
            return cur_res_group_list_;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	int number_resources(string group, ClassicMap<string, Base_Res> resmap);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void remove_resources(ClassicMap<string, Base_Res> resmap, string group);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void do_load_imageset(Imageset_Res res);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void do_load_animation(Animation_Res res);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void do_load_sound(Sound_Res res);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void do_load_music(Music_Res res);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void prase_resources(TiXmlElement element);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void prase_set_defualts(TiXmlElement element);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void parse_common_resource(TiXmlElement element, Base_Res baseres, ClassicMap<string, Base_Res> resmap);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void prase_imageset_resources(TiXmlElement element);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void prase_animation_resources(TiXmlElement element);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void prase_sound_resources(TiXmlElement element);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void prase_music_resources(TiXmlElement element);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void delete_map(ClassicMap<string, Base_Res> resmap);

        public Imageset_Res GetImageRes(string imagesetName)
        {
            Imageset_Res imageRes;

            if (imageset_map_.TryGetValue(imagesetName, out imageRes) == false)
                return null;

            return imageRes;
        }

        public Animation_Res GetAnimationRes(string animationName)
        {
            Animation_Res aniRes;

            if (animation_map_.TryGetValue(animationName, out aniRes) == false)
                return null;

            return aniRes;
        }

        private Dictionary<string, Imageset_Res> imageset_map_ = new Dictionary<string, Imageset_Res>();
        private Dictionary<string, Animation_Res> animation_map_ = new Dictionary<string, Animation_Res>();
        private Dictionary<string, Sound_Res> sound_map_ = new Dictionary<string, Sound_Res>();
        private Dictionary<string, Music_Res> music_map_ = new Dictionary<string, Music_Res>();

        private string cur_res_group_;
        private string default_path_;

        private Dictionary<string, List<Base_Res>> res_group_map_ = new Dictionary<string, List<Base_Res>>();
        private List<Base_Res> cur_res_group_list_;
        private List<Base_Res>.Enumerator cur_res_group_listor_;

        private HashSet<string> loaded_groups_ = new HashSet<string>();

        //private TiXmlDocument xml_doc_;
    }
}
