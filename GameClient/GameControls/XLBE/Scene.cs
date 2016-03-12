using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GameControls.XLBE
{
    public class Scene : Node
    {
        //public Scene();

        //public bool keyPressed(KeyEvent arg) { return true; }
        //public bool keyReleased(KeyEvent arg) { return true; }

        //public bool mouseMoved(MouseEvent arg) { return true; }
        //public bool mousePressed(MouseEvent arg, MouseButtonID id) { return true; }
        //public bool mouseReleased(MouseEvent arg, MouseButtonID id) { return true; }

        public Node node_by_position(MouseEvent arg)
        {
            return null;
        }

        public Node node_by_position(Point mousePos, Point pos )
        {
            return null;
        }

        public void mouse_position(Point mouse)
        {
        }

        public void set_focus(Node node)
        {
        }

        public void mouse_drag(Point mouse)
        {
        }

        public Node last_down_node_;
        public Node over_node_;
        public Node focus_node_;

        public Point last_mouse_;
        public bool mouse_down_;
        public bool[] key_down_ = new bool[0xFF];
    }
}
