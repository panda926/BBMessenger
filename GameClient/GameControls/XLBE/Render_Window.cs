using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class Xlbe_Export Render_Window
    public abstract class Render_Window
    {
        public Render_Window()
        {
        }
        public virtual void Dispose()
        {
        }

        public abstract void create(string title, uint width, uint height, bool fullscreen);
        public abstract void destroy();

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual bool visible() const = 0;
        public abstract bool visible();
        public abstract void set_visible(bool visible);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual bool active() const = 0;
        public abstract bool active();
        public abstract void set_active(bool active);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual bool fullscreen() const =0;
        public abstract bool fullscreen();
        public abstract void set_fullscreen(bool fullScreen);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual Size size() const = 0;
        public abstract Size size();
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual Point position() const =0;
        public abstract Point position();
        public abstract void reposition(int left, int top);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual bool drag_caption() const = 0;
        public abstract bool drag_caption();
        public abstract void set_drag_caption(bool drag);

        public abstract void window_moved_or_resized();

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual System.IntPtr window_handle() const = 0;
        public abstract System.IntPtr window_handle();
    }

    public class Render_Texture
    {
        Rect area_ = new Rect();

        public Render_Texture()
        {
        }

        public virtual void release()
        {
        }

        public void set_area(Rect rect)
        {
            area_ = rect;
        }

        public Rect area()
        {
            return area_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual string name() const=0;
        //public abstract string name();
        //public abstract void set_name(string name);

        //public abstract void create(int width, int height);
        //public abstract void load(string file);
        //public abstract void destory();

        //public abstract Size size();

        //public uint @lock(bool @readonly, int left, int top, int width)
        //{
        //    return @lock(@readonly, left, top, width, 0);
        //}
        //public uint @lock(bool @readonly, int left, int top)
        //{
        //    return @lock(@readonly, left, top, 0, 0);
        //}
        //public uint @lock(bool @readonly, int left)
        //{
        //    return @lock(@readonly, left, 0, 0, 0);
        //}
        //public uint @lock(bool @readonly)
        //{
        //    return @lock(@readonly, 0, 0, 0, 0);
        //}
        ////C++ TO C# CONVERTER WARNING: C# has no equivalent to methods returning pointers to value types:
        ////ORIGINAL LINE: virtual uint *lock(bool readonly, int left=0, int top=0, int width=0, int height=0)=0;
        ////C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //public abstract uint @lock(bool @readonly, int left, int top, int width, int height);
        //public abstract void unlock();
    }
}
