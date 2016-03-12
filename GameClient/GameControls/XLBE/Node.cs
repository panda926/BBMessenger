using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameControls.XLBE
{
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class Xlbe_Export Node_Extend
    public class Node_Extend
    {
        public Node_Extend()
        {
            this.group_ = -1;
            this.mask_ = 0;
            this.node_ = null;
        }
        public virtual void Dispose()
        {
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual int group() const
        public virtual int group()
        {
            return group_;
        }
        public virtual void set_group(int group)
        {
            group_ = group;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual int mask() const
        public virtual int mask()
        {
            return mask_;
        }
        public virtual void set_mask(int mask)
        {
            mask_ = mask;
        }

        public virtual Node node()
        {
            return node_;
        }
        public virtual void set_node(Node node)
        {
            node_ = node;
        }

        private int group_;
        private int mask_;
        private Node node_;
    }

    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class Xlbe_Export Node
    public class Node
    {

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Node();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int tag() const
        public int tag()
        {
            return tag_;
        }
        public void set_tag(int tag)
        {
            tag_ = tag;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool visible() const
        public bool visible()
        {
            return visible_;
        }
        public void set_visible(bool visible)
        {
            visible_ = visible;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool disable() const
        public bool disable()
        {
            return disable_;
        }
        public void set_disable(bool disable)
        {
            disable_ = disable;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool running() const
        public bool running()
        {
            return running_;
        }
        public void set_running(bool running)
        {
            running_ = running;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int zorder() const
        public int zorder()
        {
            return zorder_;
        }
        public void set_zorder(int z)
        {
            zorder_ = z;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: float rotation() const
        public double rotation()
        {
            return rotation_;
        }
        public void set_rotation(double rotation)
        {
            rotation_ = rotation;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Size scale() const
        public Size scale()
        {
            return scale_;
        }
        public void set_scale(Size scale)
        {
            scale_ = scale;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool flip_x() const
        public bool flip_x()
        {
            return flip_x_;
        }
        public void set_flip_x(bool flip)
        {
            flip_x_ = flip;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool flip_y() const
        public bool flip_y()
        {
            return flip_y_;
        }
        public void set_flip_y(bool flip)
        {
            flip_y_ = flip;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Point hot() const
        public Point hot()
        {
            return hot_;
        }
        public void set_hot(Point hot)
        {
            hot_ = hot;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Color_Rect color() const
        public Color_Rect color()
        {
            return color_;
        }
        public void set_color(Color_Rect color)
        {
            color_ = color;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Render_Blend blend() const
        public Render_Blend blend()
        {
            return blend_;
        }
        public void set_blend(Render_Blend blend)
        {
            blend_ = blend;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Point position() const
        public Point position()
        {
            return new Point( position_.x_, position_.y_ );
        }
        public void set_position(Point position)
        {
            position_ = position;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Size content_size() const
        public Size content_size()
        {
            return new Size( content_size_.width_, content_size_.height_ );
        }
        public void set_content_size(Size size)
        {
            content_size_ = size;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Point src_position() const
        public Point src_position()
        {
            return src_position_;
        }
        public void set_src_position(Point position)
        {
            src_position_ = position;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Size src_size() const
        public Size src_size()
        {
            return src_size_;
        }
        public void set_src_size(Size size)
        {
            src_size_ = size;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Rect boundbox();

        public Point position_absolute()
        {
            Point pos = this.position();

            if (parent_ != null)
                pos += parent_.position_absolute();

            return pos;
        }

        public Node parent()
        {
            return parent_;
        }
        public void set_parent(Node parent)
        {
            parent_ = parent;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool has_child() const
        public bool has_child()
        {
            return childs_.Count() > 0;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int number_of_child() const
        public int number_of_child()
        {
            return childs_.Count();
        }

        public void add_child(Node child)
        {
            child.set_parent(this);
            childs_.Add(child);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void add_child(Node child, int z);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void add_child(Node child, int z, int tag);

        public void remove_child(Node child)
        {
            childs_.Remove(child);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Node remove_child_by_tag(int tag);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void remove_all_child();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void remove_from_parent();

        //public List<Node>.Enumerator child_begin()
        //{
        //    return childs_.GetEnumerator();
        //}
        //public List<Node>.Enumerator child_end()
        //{
        //    return childs_.end();
        //}

        public List<Node> childs()
        {
            return childs_;
        }


        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Node child_by_tag(int tag);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void reorder_child(Node child, int z);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	int number_of_running_action();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        public Action action_by_tag(int tag)
        {
            foreach (Action action in actions_)
            {
                if (action.tag() == tag)
                    return action;
            }

            return null;
        }

        public void run_action(Action action)
        {
            actions_.Add(action);

            action.start_with_target(this);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void stop_action(Action action);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void stop_action_by_tag(int tag);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        public void stop_all_action()
        {
            foreach (Action action in actions_)
            {
                action.stop();
            }
            actions_.Clear();
        }

        public Node_Extend node_extend()
        {
            return extend_;
        }

        public void set_node_extend(Node_Extend extend)
        {
            extend_ = extend;
            extend_.set_node(this);
        }

        public virtual void enter()
        {
        }
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void exit();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void cleanup();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        public virtual void update(double dt) 
        {
            int i = 0;
            while (i < actions_.Count)
            {
                Action action = actions_[i];
                action.update(dt);

                if (action.is_done())
                {
                    action.stop();
                    actions_.Remove(action);
                    continue;
                }
                i++;
            }

            List<Node> nodeList = new List<Node>();

            foreach (Node child in childs_)
                nodeList.Add(child);

            foreach( Node child in nodeList )
                child.update(dt);
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        public virtual void draw() 
        {
            if (visible_ == false)
                return;

            foreach (Node child in childs_)
                child.draw();
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void insert_child(Node node, int index);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void delete_child(Node node);

        public void clear_actions()
        {
            actions_.Clear();
        }

        protected int tag_;

        protected bool visible_ = true;
        protected bool disable_;
        protected bool running_;
        protected int zorder_;

        protected double rotation_;
        protected Size scale_ = new Size(1,1);
        protected Point hot_;
        protected Point position_ = new Point();
        protected bool flip_x_;
        protected bool flip_y_;
        protected Size content_size_ = new Size();
        protected Color_Rect color_ = new Color_Rect();
        protected Render_Blend blend_ ;

        protected Point src_position_ = new Point();
        protected Size src_size_ = new Size();

        protected int update_count_;

        protected Node parent_;
        protected List<Node> childs_ = new List<Node>();
        protected List<Action> actions_ = new List<Action>();

        protected Node_Extend extend_;

    }

}
