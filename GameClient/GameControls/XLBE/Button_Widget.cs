using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public interface Button_Listener
    {
        void button_press(int tag);
    }

    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class Xlbe_Export Button_Widget : public Layer
    public class Button_Widget : Layer
    {

        public Button_Widget(Image button_image, Image over_image, Image down_image, Image disabled_image)
        {
            button_image_ = button_image;
            over_image_ = over_image;
            down_image_ = down_image;
            disabled_image_ = disabled_image;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        public override void draw()
        {
            if (visible_ == false)
                return;

            Image drawImage = button_image_;

            if (over_ == true)
                drawImage = over_image_;

            if (down_ == true)
                drawImage = down_image_;

            if (disable() == true)
                drawImage = disabled_image_;

            drawImage.draw(position());
        }

        public override void mouse_enter()
        {
            over_ = true;
        }

        public override void mouse_leave()
        {
            over_ = false;
        }

        public override void mouse_down(Point pt, int num, int count)
        {
            down_ = true;

            foreach (Button_Listener buttonListener in listeners_)
                buttonListener.button_press(tag_);
        }

        public override void mouse_up(Point pt, int num, int count)
        {
            down_ = false;
        }

        public void add_listener(Button_Listener listener)
        {
            listeners_.Add(listener);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void remove_listener(Button_Listener listener);

        public bool mouse_over()
        {
            return over_;
        }

        public bool mouse_downed()
        {
            return down_;
        }

        private bool over_;
        private bool down_;

        private Image button_image_;
        private Image over_image_;
        private Image down_image_;
        private Image disabled_image_;

        private List<Button_Listener> listeners_ = new List<Button_Listener>();
    }
}
