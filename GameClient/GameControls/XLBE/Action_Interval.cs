using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Action_Interval : Action_Finit_Time
    {
        public Action_Interval(double d)
            : base(d)
        {
        }

        public override bool is_done() { return ((elapsed_ >= duration_) || stop_); }
        public override void stop() { stop_ = true; target_ = null; }

        //public override void step(double dt);

        public override void update(double time)
        {
            elapsed_ += time;
        }

        public override double speed() { return speed_; }
        public override void set_speed(double speed) { speed_ = speed; }

        public override void start_with_target(Node target)
        {
            base.start_with_target(target);
            elapsed_ = 0;
        }

        protected double elapsed_;
        protected bool stop_;
        protected double speed_;
    }

    public class Action_Delay : Action_Interval
    {
        public Action_Delay(double d) 
            : base(d)
        {
        }
    }

    public class Action_Move_To : Action_Interval
    {
        public Action_Move_To(double d, Point end) 
            : base(d)
        {
            end_ = end;
        }

        public override void update(double time)
        {
            base.update(time);

            Point curPos = target_.position();

            int xpos = (int)(start_.x_ + (end_.x_ - start_.x_) * elapsed_ / duration_);
            int ypos = (int)(start_.y_ + (end_.y_ - start_.y_) * elapsed_ / duration_);

            target_.set_position(new Point(xpos, ypos));
        }
        
        public override void start_with_target(Node target)
        {
            start_ = target.position();

            base.start_with_target(target);
        }

        protected Point start_;
        protected Point end_;
        protected Point delta_;
    };

    public class Action_Move_By : Action_Move_To
    {
        public Action_Move_By(double d, Point delta) 
            : base(d, delta)
        {
            delta_ = delta;
        }

        public override void start_with_target(Node target)
        {
            base.start_with_target(target);

            end_.x_ = (int)(start_.x_ + delta_.x_ * duration_);
            end_.y_ = (int)(start_.y_ + delta_.y_ * duration_);
        }
    };

    public class Action_Fade_Out : Action_Interval
    {
        public Action_Fade_Out(double d) 
            : base(d)
        {
        }

        public override void update(double time)
        {
            base.update(time);

            //Render_Blend blend = new Render_Blend();
            //blend.alpha_ = 255 - (int)(255 * elapsed_ / duration_);

            //target_.set_blend(blend);
        }
    }

    public class Action_Scale_To : Action_Interval
    {
        public Action_Scale_To(double d, double scale)
            : base(d)
        {
            end_scale_x_ = scale;
            end_scale_y_ = scale;
        }

        public Action_Scale_To(double d, double scalex, double scaley)
            : base((double)d)
        {
            end_scale_x_ = scalex;
            end_scale_y_ = scaley;
        }

        public override void update(double time)
        {
            base.update(time);

            Size newScale = new Size();
            newScale.width_ = start_scale_x_ + (end_scale_x_ - start_scale_x_) * elapsed_ / duration_;
            newScale.height_ = start_scale_y_ + (end_scale_y_ - start_scale_y_) * elapsed_ / duration_;

            if (newScale.width_ > end_scale_x_)
                newScale.width_ = end_scale_x_;

            if (newScale.height_ > end_scale_y_)
                newScale.height_ = end_scale_y_;

            target_.set_scale(newScale);
        }

        public override void start_with_target(Node target)
        {
            start_scale_x_ = target.scale().width_;
            start_scale_y_ = target.scale().height_;

            base.start_with_target(target);
        }

        protected double start_scale_x_;
        protected double start_scale_y_;
        protected double end_scale_x_;
        protected double end_scale_y_;
        protected double delta_x_;
        protected double delta_y_;
    }

    public class Action_Animation : Action_Interval
    {
        public Action_Animation(double d, Animation animation, bool origin)
            : base((double)d)
        {
            origin_ = origin;
            animation_ = animation;

            interval_ = d;
            duration_ = interval_ * animation.num_images();
        }

        public override void stop()
        {
            if (origin_ == true)
            {
                if (target_ is Sprite)
                {
                    ((Sprite)target_).set_display_image(origin_image_);
                }
            }

            base.stop();
        }
        
        public override void update(double time)
        {
            base.update(time);

            if (target_ is Sprite)
            {
                int index = (int)(elapsed_ / interval_);

                if (index >= animation_.num_images())
                    index = animation_.num_images() - 1;

                ((Sprite)target_).set_display_image(animation_, index);
            }
        }

        public override void start_with_target(Node target)
        {
            base.start_with_target(target);
            
            if( target is Sprite )
            {
                origin_image_ = ((Sprite)target).displayed_image();
            }
        }

        bool origin_;
        Image origin_image_;
        Animation animation_;
        double interval_;
    };

    public class  Action_Move_Src_To : Action_Interval
    {
        public Action_Move_Src_To(double d, Point end, int danwei)
            : base(d)
        {
            end_ = end;
            danwei_ = danwei;
        }

        public override void update(double time)
        {
            Point pt = new Point((int)(start_.x_+delta_.x_*time), (int)(start_.y_+delta_.y_*time));

            int x = (int)pt.x_;
            int y = (int)pt.y_;

            y %= danwei_;
            //y += danwei_;
            //y %= danwei_;

            pt.x_ = x;
            pt.y_= y;

            target_.set_src_position(pt);
        }

        public override void start_with_target(Node target)
        {
            start_ = target.src_position();
            delta_ = new Point( end_.x_ - start_.x_, end_.y_ - start_.y_);

            base.start_with_target(target);
        }

        protected Point start_;
        protected Point end_;
        protected Point delta_;
        protected int danwei_;
    };

    public class  Action_Move_Src_Acceleration_To : Action_Interval
    {
        public Action_Move_Src_Acceleration_To(double d, Point end, int danwei)
            :base(d)
        {
            end_ = end;
            danwei_ = danwei;
        }

        public override void update(double time)
        {
            Point pt = new Point((int)(start_.x_+delta_.x_*Math.Sqrt(time)), (int)(start_.y_+delta_.y_*Math.Sqrt(time)));

            int x = (int)pt.x_;
            int y = (int)pt.y_;

            y %= danwei_;
            y += danwei_;
            y %= danwei_;
            pt.x_ = x;
            pt.y_= y;

            target_.set_src_position(pt);
        }

        public override void start_with_target(Node target)
        {
            start_ = target.src_position();
            delta_ = new Point( end_.x_ - start_.x_, end_.y_ - start_.y_);

            base.start_with_target(target);
        }

        protected Point start_;
        protected Point end_;
        protected Point delta_;
        protected int danwei_;
    };

    public class Action_Show : Action_Interval
    {
        public Action_Show()
            : base(0)
        {
        }

        public override void start_with_target(Node target)
        {
            target.set_visible(true);
            base.start_with_target(target);
        }
    };


    public class Action_Hide : Action_Interval
    {
        public Action_Hide()
            : base(0)
        {
        }

        public override void start_with_target(Node target)
        {
            target.set_visible(false);
            base.start_with_target(target);
        }
    };

    public delegate bool SlotFuncHandler(Node node, int tag);

    ///////////////////////////////////////////////////////////////////
    public class Action_Func : Action_Interval
    {
        public Action_Func(SlotFuncHandler slot)
            : base(0)
        {
            slot_func_ = slot;
        }

        public override void start_with_target(Node target)
        {
            base.start_with_target(target);

            slot_func_(target_, target_.tag());
        }

        SlotFuncHandler slot_func_;
    };
}
