using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public interface Progress_Listener
    {
        void progress_step(int tag, int pos);
    }

    public class Progress_Widget : Layer
    {
        private int lower_;
        private int upper_;
        private int step_;
        private int pos_range_;

        private Image image_left_;
        private Image image_middle_;
        private Image image_right_;
        private Image step_image_;

        private List<Progress_Listener> listeners_ = new List<Progress_Listener>();

        public Progress_Widget(Image image_left, Image image_middle, Image image_right, Image step_image)
        {
            image_left_ = image_left;
            image_middle_ = image_middle;
            image_right_ = image_right;
            step_image_ = step_image;

            lower_ = 0;
            upper_ = 100;
            step_ = 1;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void range(int &lower, int &upper) const
        public void range(ref int lower, ref int upper)
        {
            lower = lower_;
            upper = upper_;
        }
        public void set_range(int lower, int upper)
        {
            lower_ = lower;
            upper_ = upper;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int pos_range() const
        public int pos_range()
        {
            return pos_range_;
        }
        public void set_pos_range(int pos)
        {
            pos_range_ = pos;
        }

        public void set_step(int step)
        {
            step_ = step;
        }
        public void step()
        {
            pos_range_ += step_;
        }

        public override void draw()
        {
            //int width = (int)(content_size().width_) - (int)image_left_.width() - (int)image_right_.width();

            //Point startPt = position_absolute();

            //image_left_.draw(startPt);
            //startPt.x_ += (int)(image_left_.width());

            //while( width > 0 )
            //{
            //    image_middle_.draw(startPt);
            //    startPt.x_ += (int)(image_middle_.width());

            //    width -= (int)(image_middle_.width());
            //}

            //image_right_.draw(startPt);

            double width = (pos_range_ - lower_) * content_size_.width_ / upper_;

            Point drawPt = position_absolute();

            while (width > 0 )
            {
                step_image_.draw(drawPt);
                drawPt.x_ += step_image_.width();
                width -= step_image_.width();
            }
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void add_listener(Progress_Listener listener);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void remove_listener(Progress_Listener listener);

    }
}
