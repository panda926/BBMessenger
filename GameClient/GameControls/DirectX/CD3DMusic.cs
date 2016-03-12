using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectSound;
using System.IO;

namespace GameControls
{
    public class CD3DMusic
    {
        static CD3DMusic _Instance;						//对象指针

        Device _Device;
        Microsoft.DirectX.DirectSound.Buffer _SoundBuffer;

        public static CD3DMusic GetInstance()
        {
            if (_Instance == null)
                _Instance = new CD3DMusic();

            return _Instance;
        }

        public void Start( string resourceName )
        {
            //Stream stream = GetType().Module.Assembly.GetManifestResourceStream(resourceName);

            //// setup sound buffer from resource stream
            //_Device = new Device();
            //_SoundBuffer = new Microsoft.DirectX.DirectSound.Buffer(stream, _Device);

            //_SoundBuffer.Play(0, BufferPlayFlags.Default);
        }

        public void Stop()
        {
            //_SoundBuffer.Stop();
        }
    }
}
