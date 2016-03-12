using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Size
    {
	    public Size()
	    {
		    this.width_ = 0F;
		    this.height_ = 0F;
	    }
	    public Size(double width, double height)
	    {
		    this.width_ = width;
		    this.height_ = height;
	    }
	    public Size(Size size)
	    {
		    this.width_ = size.width_;
		    this.height_ = size.height_;
	    }
	    public void Dispose()
	    {
	    }

    //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    //ORIGINAL LINE: bool operator == (const Size &size) const
	    public static bool operator == (Size ImpliedObject, Size size)
	    {
		    return (size.width_ == ImpliedObject.width_ && size.height_ == ImpliedObject.height_);
	    }
    //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    //ORIGINAL LINE: bool operator != (const Size &size) const
	    public static bool operator != (Size ImpliedObject, Size size)
	    {
		    return (size.width_ != ImpliedObject.width_ || size.height_ != ImpliedObject.height_);
	    }

    //C++ TO C# CONVERTER NOTE: This 'CopyFrom' method was converted from the original C++ copy assignment operator:
    //ORIGINAL LINE: Size &operator = (const Size &size)
	    public Size CopyFrom (Size size)
	    {
		    width_ = size.width_;
		    height_ = size.height_;
		    return this;
	    }

    //C++ TO C# CONVERTER TODO TASK: The += operator cannot be overloaded in C#:
        //public static Size operator += (Size size)
        //{
        //    width_ += size.width_;
        //    height_ += size.height_;
        //}
    //C++ TO C# CONVERTER TODO TASK: The -= operator cannot be overloaded in C#:
        //public static Size operator -= (Size size)
        //{
        //    width_ -= size.width_;
        //    height_ -= size.height_;
        //}

	    public void set_size(float width, float height)
	    {
		    width_ = width;
		    height_ = height;
	    }

	    public static Size operator + (Size ImpliedObject, Size size)
	    {
            return new Size(ImpliedObject.width_ + size.width_, ImpliedObject.height_ + size.height_);
	    }
	    public static Size operator - (Size ImpliedObject, Size size)
	    {
            return new Size(ImpliedObject.width_ - size.width_, ImpliedObject.height_ - size.height_);
	    }
	    public static Size operator - (Size ImpliedObject)
	    {
            return new Size(-ImpliedObject.width_, -ImpliedObject.height_);
	    }

	    public double width_;
	    public double height_;
    }
}
