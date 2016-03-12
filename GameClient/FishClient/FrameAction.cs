using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;

namespace FishClient
{
    public class Key_Frame
    {
        public Key_Frame()
        {
            this.angle_ = 0F;
        }
        public Key_Frame(Point position, double angle)
        {
            this.position_ = position;
            this.angle_ = angle;
        }
        public void Dispose()
        {
        }

        public double angle_;
        public Point position_ = new Point();
    }

    public class Action_Key_Frame : Action_Interval
    {
        public Action_Key_Frame(double d, List<Key_Frame> frames, Point offset)
            : base(d)
        {
            Key_Frame frame = new Key_Frame();

            foreach (Key_Frame i in frames)
            {
                if (offset.x_ != 0 && offset.y_ != 0)
                {
                    frame = i;
                    frame.position_ += offset;
                    key_frames_.Add(frame);

                }
                else
                {
                    key_frames_.Add(i);
                }


            }

            duration_ = d * key_frames_.Count();
        }

        public virtual void Dispose()
        {
        }


        public override void update(double time)
        {
            base.update(time);

            double fDiff;
            double fIndex = this.elapsed_ * key_frames_.Count() / duration_;
            int index = (int)fIndex;

            fDiff = fIndex - index;

            if (index >= key_frames_.Count())
            {
                index = key_frames_.Count() - 1;
            }

            Key_Frame key = new Key_Frame();

            if (index < key_frames_.Count() - 1)
            {
                Key_Frame key1 = key_frames_[index];
                Key_Frame key2 = key_frames_[index + 1];

                key.position_ = key1.position_ * (1.0 - fDiff) + key2.position_ * fDiff;
                key.angle_ = key1.angle_ * (1.0 - fDiff) + key2.angle_ * fDiff;

                //if (Math.Abs(key1.angle_ - key2.angle_) > 180.0)
                //{
                //    key.angle_ = key1.angle_;
                //}
            }
            else
            {
                key = key_frames_[index];
            }

            target_.set_position(key.position_);
            //target_.set_rotation((360.0 - key.angle_) * Math.PI / 180.0);
            target_.set_rotation((360.0 + key.angle_) * Math.PI / 180.0);
        }



        private List<Key_Frame> key_frames_ = new List<Key_Frame>();
    }
}
