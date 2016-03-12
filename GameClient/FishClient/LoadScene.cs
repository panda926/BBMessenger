using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using System.Threading;

namespace FishClient
{
    class CLoadScene : Scene, Input_Listener
    {
	    CLoadLayer m_layLoad;
	    Progress_Widget m_widgetProgress;

	    CFrameLayer m_layFrame;
	    CCursorLayer m_layCursor;

        int m_Step;

        FishView m_Parent;

        public Thread m_LoadingThread;

        public void LoadingThread()
        {
            try
            {
                int totalCount = Root.instance().resource_manager().number_resources("Game");
                int curIndex = 0;

                Root.instance().resource_manager().start_load_resources("Game");
                bool bLoad = true;
                
                while (true)
                {
                    lock (Root.instance().sect_)
                    {
                        bLoad = Root.instance().resource_manager().load_next_resource();
                    }

                    if (bLoad == false)
                        break;

                    curIndex++;
                    m_Step = (int)(curIndex * 100 / totalCount);
                }

                if (curIndex != totalCount)
                    return;

                m_Step = 100;

                m_Parent.Invoke((System.Windows.Forms.MethodInvoker)delegate
                {
                    m_Parent.StartGameScene();
                });

            }
            catch { }
        }

        public    CLoadScene( FishView parent )
        {
            m_Parent = parent;

	        m_layLoad = new CLoadLayer();
	        m_layLoad.set_position( new Point(4,31));
	        m_layLoad.set_content_size( new Size(1272,703));
	        add_child(m_layLoad);

	        m_widgetProgress = new Progress_Widget(Root.instance().imageset_manager().imageset("ui_load").image("progress_left"), Root.instance().imageset_manager().imageset("ui_load").image("progress_left"), Root.instance().imageset_manager().imageset("ui_load").image("progress_left"), Root.instance().imageset_manager().imageset("ui_load").image("progress_step"));
	        m_widgetProgress.set_tag(10001);
	        m_widgetProgress.set_position( new Point(462, 446));
	        m_widgetProgress.set_content_size( new Size(360,11));
	        m_widgetProgress.set_pos_range(0);
	        add_child(m_widgetProgress);

	        m_layFrame = new CFrameLayer(0);
	        m_layFrame.set_position( new Point(0,0));
	        m_layFrame.set_content_size( new Size(1280, 738));
	        m_layFrame.set_disable(true);
	        m_layFrame.add_widget(this);

	        m_layCursor = new CCursorLayer();
	        m_layCursor.set_position( new Point(0,0));
	        m_layCursor.set_content_size( new Size(80, 80));
	        m_layCursor.SetCursor(Root.instance().imageset_manager().imageset("ui_load").image("cursor"));
	        add_child(m_layCursor);

            Root.instance().input_manager().add_input_listener(this);

            m_LoadingThread = new Thread(LoadingThread);
            m_LoadingThread.Start();
        }

        public override void update(double dt)
        {
            base.update(dt);

            m_widgetProgress.set_pos_range(m_Step);

            //m_widgetProgress.set_pos_range(theApp.loading_thread_progress() * 100);
        }

        public bool mouseMoved( MouseEvent arg)
        {
            Point pt = new Point(arg.X, arg.Y);

            m_layCursor.set_position(pt);

            return true;
        }

        public bool keyPressed(KeyEvent arg) { return true; }
        public bool keyReleased(KeyEvent arg) { return true; }

        public bool mousePressed(MouseEvent arg, MouseButtonID id) { return true; }
        public bool mouseReleased(MouseEvent arg, MouseButtonID id) { return true; }


    }
}
