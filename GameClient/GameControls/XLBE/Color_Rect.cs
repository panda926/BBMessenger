using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class Xlbe_Export Color
    public class Color
    {
        public Color()
        {
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Color(int color);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Color(int color, int alpha);

        public Color(int red, int green, int blue)
        {
            red_ = red;
            green_ = green;
            blue_ = blue;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Color(int red, int green, int blue, int alpha);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int red() const;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	int red();
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int green() const;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	int green();
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int blue() const;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	int blue();
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int alpha() const;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	int alpha();

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: uint toint() const;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	uint toint();

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return red_;
                    case 1: return green_;
                    case 2: return blue_;
                }

                return alpha_;
            }
            set
            {
                switch (index)
                {
                    case 0:
                        red_ = value;
                        return;
                    case 1:
                        green_ = value;
                        return;
                    case 2:
                        blue_ = value;
                        return;
                }

                alpha_ = value;
            }
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int operator [] (int index) const;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	int operator [] (int index);

        public int red_;
        public int green_;
        public int blue_;
        public int alpha_;
    }

    public class Color_Rect
    {
        public Color_Rect()
        {
        }
        public Color_Rect(Color col)
        {
            top_left_ = top_right_ = bottom_left_ = bottom_right_ = col;
        }
        public Color_Rect(Color top_left, Color top_right, Color bottom_left, Color bottom_right)
        {
            top_left_ = top_left;
            top_right_ = top_right;
            bottom_left_ = bottom_left;
            bottom_right_ = bottom_right;
        }

        public void set_alpha(int alpha)
        {
            top_left_[3] = top_right_[3] = bottom_left_[3] = bottom_right_[3] = alpha;
        }
        public void set_color(Color col)
        {
            top_left_ = top_right_ = bottom_left_ = bottom_right_ = col;
        }

        public Color top_left_ = new Color();
        public Color top_right_ = new Color();
        public Color bottom_left_ = new Color();
        public Color bottom_right_ = new Color();
    }
}
