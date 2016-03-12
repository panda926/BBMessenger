using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Point
    {
	    public Point()
	    {
		    this.x_ = 0F;
		    this.y_ = 0F;
	    }
	    public Point(double x, double y)
	    {
		    this.x_ = x;
		    this.y_ = y;
	    }
	    public Point(Point point)
	    {
		    this.x_ = point.x_;
		    this.y_ = point.y_;
	    }
	    public void Dispose()
	    {
	    }

	    public void offset(double x, double y)
	    {
		    x_ += x;
		    y_ += y;
	    }

	    public void set_point(double x, double y)
	    {
		    x_ = x;
		    y_ = y;
	    }

    //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    //ORIGINAL LINE: bool operator == (const Point &point) const
        public static bool operator ==(Point ImpliedObject, Point point)
        {
            if ((object)ImpliedObject == null && (object)point == null)
                return true;
            if ((object)ImpliedObject != null || (object)point != null)
                return false;

            return (ImpliedObject.x_ == point.x_ && ImpliedObject.y_ == point.y_);
        }
    //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    //ORIGINAL LINE: bool operator != (const Point &point) const
        public static bool operator !=(Point ImpliedObject, Point point)
        {
            if ((object)ImpliedObject == null && (object)point == null)
                return false;
            if ((object)ImpliedObject != null || (object)point != null)
                return true;

            return (ImpliedObject.x_ != point.x_ || ImpliedObject.y_ != point.y_);
        }

    //C++ TO C# CONVERTER NOTE: This 'CopyFrom' method was converted from the original C++ copy assignment operator:
    //ORIGINAL LINE: Point &operator = (const Point &point)
	    public Point CopyFrom (Point point)
	    {
		    x_ = point.x_;
		    y_ = point.y_;
		    return this;
	    }

    //C++ TO C# CONVERTER TODO TASK: The += operator cannot be overloaded in C#:
        //public static void operator += (Point point)
        //{
        //    x_ += point.x_;
        //    y_ += point.y_;
        //}
    //C++ TO C# CONVERTER TODO TASK: The -= operator cannot be overloaded in C#:
        //public static void operator -= (Point point)
        //{
        //    x_ -= point.x_;
        //    y_ -= point.y_;
        //}

	    public static Point operator + (Point ImpliedObject, Point point)
	    {
            return new Point(ImpliedObject.x_ + point.x_, ImpliedObject.y_ + point.y_);
	    }
	    public static Point operator - (Point ImpliedObject, Point point)
	    {
            return new Point(ImpliedObject.x_ - point.x_, ImpliedObject.y_ - point.y_);
	    }
	    public static Point operator - (Point ImpliedObject)
	    {
            return new Point(-ImpliedObject.x_, -ImpliedObject.y_);
	    }

	    public static Point operator * (Point ImpliedObject, double multip)
	    {
            return new Point(ImpliedObject.x_ * multip, ImpliedObject.y_ * multip);
	    }

	    public double x_;
	    public double y_;
    }
}
