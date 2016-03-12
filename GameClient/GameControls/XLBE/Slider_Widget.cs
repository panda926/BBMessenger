using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public delegate void SliderValue_Handler( int tag, double value );

    public class Slider_Widget : Layer
    {
        private bool dragging_;
        private double value_;
        private int rel_x_;

        private Image track_image_;
        private Image thumb_image_;
        private Image slider_image_;

        SliderValue_Handler listeners_ ;

        public Slider_Widget(Image track_Image, Image thumb_image, Image slider_image)
        {
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void draw();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void mouse_enter();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void mouse_leave();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void mouse_down(Point pt, int num, int count);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void mouse_up(Point pt, int num, int count);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void mouse_drag(Point pt);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual double value() const
        public virtual double value()
        {
            return value_;
        }

        public	virtual void set_value(double value)
        {
        }

        public void add_listener(SliderValue_Handler listener)
        {
            listeners_ = listener;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void remove_listener(Slider_Listener listener);

    }
}
