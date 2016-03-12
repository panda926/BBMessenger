using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Frame_Event
    {
        public uint time_since_last_event;
        public uint time_since_last_frame;
    }

    public interface Frame_Listener
    {
        bool frame_started(Frame_Event evt);
        bool frame_render_targets(Frame_Event evt);
        bool frame_ended(Frame_Event evt);
    }
}
