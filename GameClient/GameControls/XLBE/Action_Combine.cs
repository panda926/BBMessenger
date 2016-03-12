using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Action_Sequence : Action_Interval
    {
        //class Sequence : Action_Interval
        //{
        //    public Sequence(Action act1, Action act2);

        //    public override void update(float time);
        //    public override void stop();

        //    public override void start_with_target(Node target);

        //    protected Action_Finit_Time[] actions_ = new Action_Finit_Time[2];
        //    protected float split_;
        //    protected int last_;
        //};

        public Action_Sequence(params Action[] actions)
            : base(1)
        {
            actions_ = actions;
        }

        public override void update(double time)
        {
            if (actionIndex >= actions_.Length-1)
            {
                elapsed_ = duration_;
                return;
            }

            actions_[actionIndex].update(time);

            if (actions_[actionIndex].is_done() == true)
            {
                actionIndex++;

                if (actionIndex < actions_.Length-1)
                    actions_[actionIndex].start_with_target(target_);
            }
        }

        public override void stop()
        {
        }

        public override void start_with_target(Node target)
        {
            if (actions_.Length > 1)
                actions_[0].start_with_target(target);

            base.start_with_target(target);
        }

        Action[] actions_;
        int actionIndex;
    };

    public class Action_Repeat_Forever : Action_Impl
    {
        public Action_Repeat_Forever(Action a)
        {
            other_ = a;
        }

        public override bool is_done() { return stop_; }
        public override void stop() { other_.stop(); stop_ = true; target_ = null; }

        public override void update(double time)
        {
            other_.update(time);

            if( other_.is_done() == true )
                other_.start_with_target(target_);
        }

        public override void start_with_target(Node target)
        {
            other_.start_with_target(target);

            base.start_with_target(target);
        }

        bool stop_;
        Action other_;
    };
}
