using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class MouseState
    {
        public MouseState()
        {
            this.width = 50;
            this.height = 50;
            this.buttons = 0;
        }

        /** Represents the height/width of your display area.. used if mouse clipping
        or mouse grabbed in case of X11 - defaults to 50.. Make sure to set this
        and change when your size changes.. */
        public int width;
        public int height;

        //! X Axis component
        public int X = 0;

        //! Y Axis Component
        public int Y = 0;

        //! Z Axis Component
        public int Z = 0;

        //! represents all buttons - bit position indicates button down
        public int buttons;

        //! Button down test
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: inline bool buttonDown(MouseButtonID button) const
        public bool buttonDown(int button)
        {
            return ((buttons & (1 << (int)button)) == 0) ? false : true;
        }

        //! Clear all the values
        public void clear()
        {
            X = 0;
            Y = 0;
            Z = 0;
            buttons = 0;
        }
    }
}
