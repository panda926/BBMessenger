using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class Xlbe_Export Action
    public abstract class Action
    {
        public Action()
        {
        }
        public virtual void Dispose()
        {
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual int tag() const = 0;
        public abstract int tag();
        public abstract void set_tag(int tag);

        public abstract bool is_done();
        public abstract void stop();

        public abstract void step(double dt);
        public abstract void update(double dt);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual double speed() const = 0;
        public abstract double speed();
        public abstract void set_speed(double speed);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: virtual Node *target() const = 0;
        public abstract Node target();
        public abstract void start_with_target(Node target);

        //virtual Action *clone() const = 0;
        //virtual Action *reverse() const = 0;
    }

    public class Action_Impl : Action
    {
        public Action_Impl()
        {
            tag_ = -1;
        }

        public override int tag() { return tag_; }
        public override void set_tag(int tag) { tag_ = tag; }

        public override bool is_done() { return true; }
        public override void stop() { target_ = null; }

        public override void step(double dt) { }
        public override void update(double time) { }

        public override double speed() { return 1.0f; }
        public override void set_speed(double speed) { }

        public override Node target() { return target_; }
        
        public override void start_with_target(Node target) 
        { 
            target_ = target;
            target_.set_tag(tag_);
        } 

        protected int	tag_;
        protected Node target_;
    }

    ///////////////////////////////////////////////////////////////////
    public class Action_Finit_Time : Action_Impl
    {
        public Action_Finit_Time(double d)
        {
            duration_ = d;
        }

        public virtual double duration() { return duration_; }
        public virtual void set_duration(double d) { duration_ = d; }

        protected double duration_;
    }

}
