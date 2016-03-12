using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Rect
    {
        public Rect()
        {
            this.origin_ = new Point(0, 0);
            this.size_ = new Size(0, 0);
        }
        public Rect(double x, double y, double width, double height)
        {
            this.origin_ = new Point(x, y);
            this.size_ = new Size(width, height);
        }
        public Rect(Point point, Size size)
        {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: this.origin_ = point;
            this.origin_.CopyFrom(point);
            this.size_ = size;
        }
        public Rect(Rect rect)
        {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: this.origin_ = rect.origin_;
            this.origin_.CopyFrom(rect.origin_);
            this.size_ = rect.size_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: float width() const
        public double width()
        {
            return size_.width_;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: float height() const
        public double height()
        {
            return size_.height_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Size size() const
        public Size size()
        {
            return size_;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Point origin_point() const
        public Point origin_point()
        {
            return origin_;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Point center_point() const
        public Point center_point()
        {
            return new Point(origin_.x_ + size_.width_ / 2.0, origin_.y_ + size_.height_ / 2.0);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool empty() const
        public bool empty()
        {
            return (size_.width_ == 0 && size_.height_ == 0);
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool equal_rect(const Rect &r) const
        public bool equal_rect(Rect r)
        {
            return (origin_ == r.origin_ && size_ == r.size_);
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool pt_in_rect(const Point &point) const
        public bool pt_in_rect(Point point)
        {
            return (point.x_ > origin_.x_ && point.y_ > origin_.y_ && point.x_ < size_.width_ + origin_.x_ && point.y_ < size_.height_ + origin_.y_);
        }

        public void set_rect(float x, float y, float width, float height)
        {
            origin_.set_point(x, y);
            size_.set_size(width, height);
        }
        public void set_rect(Point point, Size size)
        {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: origin_=point;
            origin_.CopyFrom(point);
            size_ = size;
        }
        public void set_rect_empty()
        {
            origin_.set_point(0, 0);
            size_.set_size(0, 0);
        }

        public void inflate_rect(float width, float height)
        {
            size_.width_ += width;
            size_.height_ += height;
        }
        public void inflate_rect(Size size)
        {
            size_ += size;
        }

        public void deflate_rect(float width, float height)
        {
            size_.width_ -= width;
            size_.height_ -= height;
        }
        public void deflate_rect(Size size)
        {
            size_ -= size;
        }

        public void offset_rect(float x, float y)
        {
            origin_.x_ += x;
            origin_.y_ += y;
        }
        public void offset_rect(Size size)
        {
            origin_.x_ += size.width_;
            origin_.y_ += size.height_;
        }

        //public bool intersect_rect(Rect l, Rect r)
        //{
        //}
        //public bool union_rect(Rect l, Rect r)
        //{
        //}
        //public bool subtract_rect(Rect l, Rect r)
        //{
        //}

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool collision(const Rect &r) const
        public bool collision(Rect r)
        {
            if (r.origin_.y_ + r.size_.height_ < origin_.y_ || r.origin_.y_ > origin_.y_ + size_.height_ || r.origin_.x_ + r.size_.width_ < origin_.x_ || r.origin_.x_ > origin_.x_ + size_.width_)
            {
                return false;
            }

            return true;

        }

        //C++ TO C# CONVERTER NOTE: This 'CopyFrom' method was converted from the original C++ copy assignment operator:
        //ORIGINAL LINE: Rect& operator = (const Rect& r)
        public Rect CopyFrom(Rect r)
        {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: origin_=r.origin_;
            origin_.CopyFrom(r.origin_);
            size_ = r.size_;
            return this;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool operator == (const Rect& r) const
        public static bool operator ==(Rect ImpliedObject, Rect r)
        {
            return (ImpliedObject.origin_ == r.origin_ && ImpliedObject.size_ == r.size_);
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool operator != (const Rect& r) const
        public static bool operator !=(Rect ImpliedObject, Rect r)
        {
            return (ImpliedObject.origin_ != r.origin_ || ImpliedObject.size_ != r.size_);
        }

        public Point origin_ = new Point();
        public Size size_ = new Size();
    }
}
