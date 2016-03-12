using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Timer
    {
        public Timer()    { reset(); }

        public void reset()  
        { 
            zero_clock_ = System.Environment.TickCount; 
        }

        public int get_milli_seconds()
        {
            return System.Environment.TickCount - zero_clock_;
        }

        //unsigned long get_micro_seconds()
        //{
        //    clock_t newclock = clock();
        //    return (unsigned long)( (float)( newclock - zero_clock_ ) / ( (float)CLOCKS_PER_SEC / 1000000.0 ) ) ;
        //}

        int zero_clock_;
    }
}
