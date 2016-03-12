using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.AudioVideoPlayback;

namespace GameControls.XLBE
{
    public class Sound_Instance
    {
        public int id;

        public Audio audio_;
        public Audio doubleAudio_;

        public bool isLooping_;

        public static Sound_Instance FromFile(string path, bool bDouble )
        {
            Audio audio = null;
            Audio doubleAudio = null;

            try
            {
                audio = new Audio(path);

                if( bDouble )
                    doubleAudio = new Audio(path);
            }
            catch { }

            if (audio == null)
                return null;

            Sound_Instance sound_instance = new Sound_Instance();
            sound_instance.SetAudio( audio, doubleAudio );

            return sound_instance;
        }

	    public Sound_Instance() 
        {
        }

        public void SetAudio(Audio audio, Audio doubleAudio )
        {
            audio_ = audio;
            audio_.Ending += new EventHandler(audio__Ending);

            if (doubleAudio != null)
            {
                doubleAudio_ = doubleAudio;
                doubleAudio_.Ending += new EventHandler(doubleAudio__Ending);
            }
        }

        void doubleAudio__Ending(object sender, EventArgs e)
        {
            if (doubleAudio_.Playing == false)
                return;

            doubleAudio_.Stop();
        }

        public void release()
        {
            if (audio_ != null)
            {
                audio_.Dispose();
                audio_ = null;
            }

            if (doubleAudio_ != null)
            {
                doubleAudio_.Dispose();
                doubleAudio_ = null;
            }
        }

        //virtual void set_base_volume(double volume) = 0; 
        //virtual void set_base_pan(int pan) = 0;

        //virtual void adjust_pitch(double steps) = 0;

        //virtual void set_volume(double volume) = 0; 
        //virtual void set_pan(int position) = 0; 

        public virtual void play(bool looping, bool autorelease)
        {
            if (audio_ == null)
                return;

            isLooping_ = looping;

            if (audio_.Playing == false)
            {
                audio_.CurrentPosition = 0;
                audio_.Play();
            }
            else if (doubleAudio_ != null)
            {
                if (doubleAudio_.Playing == false)
                {
                    doubleAudio_.CurrentPosition = 0;
                    doubleAudio_.Play();
                }
            }
        }

        void audio__Ending(object sender, EventArgs e)
        {
            if (audio_.Playing == false)
                return;

            if (isLooping_ == true)
                audio_.CurrentPosition = 0;
            else
                audio_.Stop();
        }


        public virtual void stop()
        {
            audio_.Stop();

            if (doubleAudio_ != null)
                doubleAudio_.Stop();
        }

        public void check_state()
        {
            if (audio_ != null)
            {
                if (audio_.CurrentPosition >= audio_.Duration)
                    audio__Ending(null, null);
            }
            if (doubleAudio_ != null)
            {
                if (doubleAudio_.CurrentPosition >= doubleAudio_.Duration)
                    doubleAudio__Ending(null, null);
            }
        }

        //virtual bool is_playing() = 0;
        //virtual bool is_released() = 0;
        //virtual double volume() = 0;
    }
}
