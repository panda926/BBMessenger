using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GameControls.XLBE
{
    public class Layer : Node
    {
        public Layer()
        {
        }

        public virtual void key_char(char c)
        {
        }

        public virtual void key_down(Keys key)
        {
        }

        public virtual void key_up(Keys key)
        {
        }

        public virtual bool wants_focus()
        {
            return false;
        }

        public virtual void got_focus()
        {
        }
        
        public virtual void lost_focus()
        {
        }

        public virtual void mouse_enter()
        {
        }

        public virtual void mouse_leave()
        {
        }

        public virtual void mouse_move(Point pt)
        {
        }

        public virtual void mouse_drag(Point pt)
        {
        }
    
        public virtual void mouse_wheel(int delta)
        {
        }

        public virtual void mouse_down(Point pt, int num, int count)
        {
        }

        public virtual void mouse_up(Point pt, int num, int count)
        {   
        }

        public virtual void move(Point pt)
        {
        }

        public virtual void resize(Point pt, Size size)
        {
            set_position(pt);
            set_content_size(size);
        }
    }
}
