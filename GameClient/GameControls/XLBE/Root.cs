using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    using System.Collections.Generic;

    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class Xlbe_Export Root
    public class Root
    {

        public enum Frame_Event_Time_Type
        {
            FETT_ANY = 0,
            FETT_STARTED = 1,
            FETT_QUEUED = 2,
            FETT_ENDED = 3,
            FETT_FPS = 4,
            FETT_COUNT = 5
        }

        public Root()
        {
            //Timer timer_;
            render_system_ = new Render_System();
            resource_manager_ = new Resource_Manager();
            //Render_Window render_window_;
            scene_director_ = new Director();
            action_manager_ = new Action_Manager();
            imageset_manager_ = new Imageset_Manager();
            animation_manager_ = new Animation_Manager();
            input_manager_ = new Input_Manager();
            sound_manager_ = new Sound_Manager();
            music_manager_ = new Sound_Manager();
            font_manager_ = new Font_Manager();
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Root(Root NamelessParameter);
        //C++ TO C# CONVERTER NOTE: This 'CopyFrom' method was converted from the original C++ copy assignment operator:
        //ORIGINAL LINE: const Root& operator =(const Root&);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Root CopyFrom(Root NamelessParameter);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        public static Root instance()
        {
            if (instance_ == null)
                instance_ = new Root();

            return instance_;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Config_Option_Map config_options();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void set_config_options(string name, string value);

        public bool initialised()
        {
            return initialised_;
        }

        public void initialise(System.Windows.Forms.Control control)
        {
            render_system().initialize(control);
            input_manager().initialise(control);

            renderTimer_ = new System.Windows.Forms.Timer();

            renderTimer_.Interval = 30;
            renderTimer_.Tick += new System.EventHandler(this.RenderTimer_Tick);
            renderTimer_.Enabled = true;

            initialised_ = true;
        }

        protected void RenderTimer_Tick(object sender, EventArgs e)
        {
            string errStr = string.Empty;

            try
            {
                if (isDestroy_ == true)
                {
                    scene_director().replace_scene(null);
                    render_system().shutdown();

                    imageset_manager().remove_all_imageset();
                    animation_manager().remove_all_animation();
                    sound_manager().stop_all_sounds();
                    music_manager().stop_all_sounds();

                    renderTimer_.Enabled = false;

                    instance_ = null;
                }
                else
                {
                    render_system().render_window((double)renderTimer_.Interval / 1000);

                    music_manager().update_sound();
                    sound_manager().update_sound();
                }
            }
            catch (Exception ex)
            {
                errStr = ex.ToString();
            }
        }

        public void destroy()
        {
            isDestroy_ = true;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void start_rendering();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void render_one_frame();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	int fps();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void set_fps(int nfs);


        public void queue_end_rendering()
        {
            //render_system_.shutdown();
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: uint get_next_frame_number() const
        public uint get_next_frame_number()
        {
            return next_frame_;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void add_frame_listener(Frame_Listener listener);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void remove_frame_listener(Frame_Listener listener);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void add_input_listener(Input_Listener listener);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void remove_input_listener(Input_Listener listener);

        public Timer timer()
        {
            return timer_;
        }
        public Render_System render_system()
        {
            return render_system_;
        }
        public Resource_Manager resource_manager()
        {
            return resource_manager_;
        }
        public Render_Window render_window()
        {
            return render_window_;
        }
        public Director scene_director()
        {
            return scene_director_;
        }
        public Action_Manager action_manager()
        {
            return action_manager_;
        }
        public Imageset_Manager imageset_manager()
        {
            return imageset_manager_;
        }
        public Animation_Manager animation_manager()
        {
            return animation_manager_;
        }

        public Input_Manager input_manager()
        {
            return input_manager_;
        }
        public Sound_Manager sound_manager()
        {
            return sound_manager_;
        }
        public Sound_Manager music_manager()
        {
            return music_manager_;
        }
        public Font_Manager font_manager()
        {
            return font_manager_;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void fire_frame_started();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void fire_frame_render_targets();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void fire_frame_ended();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void populate_frame_event(Frame_Event_Time_Type type, Frame_Event evttoupdate);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	uint calculate_event_time(uint now, Frame_Event_Time_Type type);

        private Timer timer_;
        private Render_System render_system_;
        private Resource_Manager resource_manager_;
        private Render_Window render_window_;
        private Director scene_director_;
        private Action_Manager action_manager_;
        private Imageset_Manager imageset_manager_;
        private Animation_Manager animation_manager_;
        private Input_Manager input_manager_;
        private Sound_Manager sound_manager_;
        private Sound_Manager music_manager_;
        private Font_Manager font_manager_;

        private List<Frame_Listener> frame_listeners_ = new List<Frame_Listener>();
        private List<Input_Listener> input_listeners_ = new List<Input_Listener>();

        private Queue<uint>[] event_times = new Queue<uint>[(int)Frame_Event_Time_Type.FETT_COUNT];

        private bool initialised_;
        private bool queue_end_;
        private uint next_frame_;
        private uint fixed_delta_;

        private SortedDictionary<string, Config_Option> options_ = new SortedDictionary<string, Config_Option>();

        private static Root instance_;

        public object sect_ = new object();
        
        public bool isDestroy_ = false;

        private System.Windows.Forms.Timer renderTimer_;

    }

    public class Config_Option
    {
        public string name;
        public string value;
    }

    public class Crit_Sect
    {
        //protected CRITICAL_SECTION critical_section_ = new CRITICAL_SECTION();
        ////C++ TO C# CONVERTER TODO TASK: C# has no concept of a 'friend' class:
        ////	friend class Auto_Crit;

        //public Crit_Sect()
        //{
        //    InitializeCriticalSection(critical_section_);
        //}
        //public void Dispose()
        //{
        //    DeleteCriticalSection(critical_section_);
        //}
    }
}
