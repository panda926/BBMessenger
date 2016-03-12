using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class Action_Manager
    {
        public class Action_Element 
        {
            bool pause_;
            Node targe_;
            List<Action> actions_ = new List<Action>();
        };

        Dictionary<Node, Action_Element> Elements = new Dictionary<Node,Action_Element>();

        public Action_Manager()
        {
        }

        //public void update(float dt)
        //{
        //}

        //public void pause_target(Node target)
        //{
        //}

        //public void resume_targer(Node target)
        //{
        //}

        //public int number_of_running_actions_in_target(Node target)
        //{
        //}

        //public Action action_by_tag(int tag, Node target)
        //{
        //}

        //public void add_action(Action action, Node target, bool pause)
        //{
        //}

        public void remove_all_action_from_target(Node target)
        {
            target.clear_actions();
        }

        //public void remove_action_by_tag(int tag, Node target)
        //{
        //}

        //public void stop_all_action_from_target(Node target)
        //{
        //}

        //public void stop_action_by_tag(int tag, Node target)
        //{
        //}
    }
}
