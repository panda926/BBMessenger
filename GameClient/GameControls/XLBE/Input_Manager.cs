using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace GameControls.XLBE
{
    public class KeyEvent
    {
        public int KeyCode;
    }

    public class MouseEvent
    {
        public int X;
        public int Y;
    }

    public enum MouseButtonID
    {
        Left,
        Right
    }

    public interface Input_Listener
    {
        bool keyPressed(KeyEvent arg);
        bool keyReleased(KeyEvent arg);

        bool mouseMoved(MouseEvent arg);
        bool mousePressed(MouseEvent arg, MouseButtonID id);
        bool mouseReleased(MouseEvent arg, MouseButtonID id);
    }

    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class Xlbe_Export Input_Manager : public OIS::KeyListener,public OIS::MouseListener
    //C++ TO C# CONVERTER TODO TASK: Multiple inheritance is not available in C#:
    public class Input_Manager 
    {
        private List<Input_Listener> input_listeners_ = new List<Input_Listener>();
        private MouseState mouseState_ = new MouseState();
        private MouseState mouseOldState_ = new MouseState();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Input_Manager();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        public void initialise( System.Windows.Forms.Control control )
        {
            control.MouseMove += new System.Windows.Forms.MouseEventHandler(this.InputManager_MouseMove);
            control.MouseDown += new System.Windows.Forms.MouseEventHandler(this.InputManager_MouseDown);
            control.MouseUp += new System.Windows.Forms.MouseEventHandler(this.InputManager_MouseUp);
        }

        public void add_input_listener(Input_Listener listener)
        {
            input_listeners_.Add(listener);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void remove_input_listener(Input_Listener listener);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool keyPressed(OIS::KeyEvent arg);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool keyReleased(OIS::KeyEvent arg);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool mouseMoved(OIS::MouseEvent arg);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool mousePressed(OIS::MouseEvent arg, OIS::MouseButtonID id);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool mouseReleased(OIS::MouseEvent arg, OIS::MouseButtonID id);

        public Input_Manager input_manager()
        {
            return this;
        }
        public Input_Manager mouse()
        {
            return this;
        }
        public Input_Manager keyboard()
        {
            return this;
        }

        public bool isKeyDown(int keyCode)
        {
            return false;
        }

        public MouseState getMouseState()
        {
            return mouseState_;
        }

        public void InputManager_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseOldState_.X = mouseState_.X;
            mouseOldState_.Y = mouseState_.Y;

            mouseState_.X = e.X;
            mouseState_.Y = e.Y;

            if (SendMouseLeaveMessage(Root.instance().scene_director().scene(), new Point(mouseOldState_.X, mouseOldState_.Y), new Point(e.X, e.Y)) == true)
                return;

            MouseEvent mouseEvent = new MouseEvent();
            mouseEvent.X = e.X;
            mouseEvent.Y = e.Y;

            foreach (Input_Listener inputListener in this.input_listeners_)
            {
                inputListener.mouseMoved(mouseEvent);
            }
        }

        private bool SendMouseLeaveMessage(Node node, Point oldPoint, Point newPoint )
        {
            if (node == null)
                return false;

            if (node.visible() == false)
                return false;

            if (node.disable() == true)
                return false;

            if (node is Button_Widget)
            {
                Layer layer = (Layer)node;
                Rect area = new Rect(layer.position_absolute(), layer.content_size());

                if (area.pt_in_rect(oldPoint) == true && area.pt_in_rect(newPoint) == false)
                {
                    layer.mouse_leave();
                    return true;
                }
                else if (area.pt_in_rect(oldPoint) == false && area.pt_in_rect(newPoint) == true)
                {
                    layer.mouse_enter();
                    return true;
                }
            }

            foreach (Node child in node.childs())
            {
                if (SendMouseLeaveMessage(child, oldPoint, newPoint) == true)
                    return true;
            }

            return false;
        }

        public void InputManager_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (SendMouseDownMessage(Root.instance().scene_director().scene(), new Point(e.X, e.Y)) == true)
                return;

            MouseEvent mouseEvent = new MouseEvent();
            mouseEvent.X = e.X;
            mouseEvent.Y = e.Y;

            MouseButtonID buttonId = MouseButtonID.Left;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                buttonId = MouseButtonID.Right;

            mouseState_.buttons |= ( 1 << (int)buttonId );

            foreach (Input_Listener inputListener in this.input_listeners_)
            {
                inputListener.mousePressed(mouseEvent, buttonId );
            }
        }

        private bool SendMouseDownMessage(Node node, Point newPoint )
        {
            if (node == null)
                return false;

            if (node.visible() == false)
                return false;

            if (node.disable() == true)
                return false;

            if (node is Button_Widget)
            {
                Button_Widget layer = (Button_Widget)node;

                Rect area = new Rect(layer.position_absolute(), layer.content_size());

                if (area.pt_in_rect(newPoint) == true && layer.mouse_over() == true )
                {
                    layer.mouse_down( newPoint, 1, 1);
                    return true;
                }
            }

            foreach (Node child in node.childs())
            {
                if (SendMouseDownMessage(child, newPoint) == true)
                    return true;
            }

            return false;
        }

        public void InputManager_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            SendMouseUpMessage(Root.instance().scene_director().scene(), new Point(e.X, e.Y));

            MouseEvent mouseEvent = new MouseEvent();
            mouseEvent.X = e.X;
            mouseEvent.Y = e.Y;

            MouseButtonID buttonId = MouseButtonID.Left;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                buttonId = MouseButtonID.Right;

            if( mouseState_.buttonDown( (int)buttonId ) == true )
                mouseState_.buttons ^= (1 << (int)buttonId);

            foreach (Input_Listener inputListener in this.input_listeners_)
            {
                inputListener.mouseReleased(mouseEvent, buttonId);
            }
        }

        private bool SendMouseUpMessage(Node node, Point newPoint )
        {
            if (node == null)
                return false;

            if (node.visible() == false)
                return false;

            if (node.disable() == true)
                return false;

            if (node is Button_Widget)
            {
                Button_Widget layer = (Button_Widget)node;

                Rect area = new Rect(layer.position_absolute(), layer.content_size());

                if (area.pt_in_rect(newPoint) == true && layer.mouse_downed() == true)
                {
                    layer.mouse_up(newPoint, 1, 1);
                    return true;
                }
            }

            foreach (Node child in node.childs())
            {
                if (SendMouseUpMessage(child, newPoint) == true)
                    return true;
            }

            return false;
        }
    }
}
