using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Sound_Manager
    {
	    public Sound_Manager() 
        {
        }

        //public virtual bool initialised() = 0;
        //public virtual void initialise(HWND hwnd) = 0;

        public bool load_sound(string idString, string path, bool bDouble )
        {
            Sound_Instance soundInstance = Sound_Instance.FromFile(path, bDouble );

            if (soundInstance == null)
                return false;

            int id = Convert.ToInt32( idString );
            soundInstance.id = id;

            soundList.Add(soundInstance);

            return true;    
        }

        //public virtual void release_sound(unsigned int id) = 0;

        //public virtual void set_volume(double volume) = 0;
        //public virtual void set_base_volume(unsigned int id, double volume) = 0;
        //public virtual void set_base_pan(unsigned int id, int pan) = 0;	

        public Sound_Instance sound_instance(int id)
        {
            int soundCount = soundList.Count();

            for (int i = 0; i < soundCount; i++)
            {
                Sound_Instance soundInstance = soundList[i];

                if (soundInstance.id == id)
                    return soundInstance;
            }
            
            return null;
        }

        //public virtual void release_sounds() = 0;
        //public virtual void release_channels() = 0;

        //public virtual double master_volume() = 0;
        //public virtual void set_master_volume(double volume) = 0;

        //public virtual void flush() = 0;
        //public virtual void set_cooperative_window(HWND hwnd, bool windowed) = 0;

        public virtual void stop_all_sounds()
        {
            int soundCount = soundList.Count();

            for (int i = 0; i < soundCount; i++)
            {
                Sound_Instance soundInstance = soundList[i];

                soundInstance.stop();
                //soundInstance.release();
            }

            soundList.Clear();
        }

        public void update_sound()
        {
            int soundCount = soundList.Count();

            for (int i = 0; i < soundCount; i++)
            {
                Sound_Instance soundInstance = soundList[i];

                soundInstance.check_state();
            }
        }

        //public virtual int free_sound_id() = 0;
        //public virtual int num_sounds() = 0;

        List<Sound_Instance> soundList = new List<Sound_Instance>();
    }
}
