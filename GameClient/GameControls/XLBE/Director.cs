using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    using System.Collections.Generic;

    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class Xlbe_Export Director
    public class Director
    {

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Director();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void push_scene(Scene scene);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void pop_scene();

        public void replace_scene(Scene scene)
        {
            running_scene_ = scene;
        }

        public void draw_scene(double dt)
        {
            if (running_scene_ == null)
                return;

            running_scene_.update(dt);
            
            running_scene_.draw();
        }

        public Scene scene()
        {
            return running_scene_;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void pause();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void resume();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void set_next_scene();

        private bool paused_;
        private bool new_push_;
        private bool runing_destroy_;

        private Scene running_scene_;
        private Scene next_scene_;

        private Stack<Scene> scenes_ = new Stack<Scene>();
    }
}
