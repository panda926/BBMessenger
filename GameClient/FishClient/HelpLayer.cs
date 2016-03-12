using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;

namespace FishClient
{
    public class CHelpLayer : Layer, Button_Listener
    {
        private Button_Widget m_btnExit;

        private Sprite m_sprBackground;

        public CHelpLayer()
        {
            m_btnExit = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_help_exit_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_help_exit_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_help_exit_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_help_exit_disable"));
            m_btnExit.set_tag(10200);
            m_btnExit.set_position( new Point(73, 300));
            m_btnExit.set_content_size( new Size(76, 27));
            m_btnExit.add_listener(this);


            m_sprBackground = new Sprite(Root.instance().imageset_manager().imageset("ui_load").image("help_show_image"));
            m_sprBackground.set_hot( new Point(0, 0));
            add_child(m_sprBackground);

        }
        public void Dispose()
        {
        }

        public override void draw()
        {
            base.draw();

            CGameScene pGameScene = (CGameScene)parent();
            CClientKernel pClientKernel = pGameScene.GetClientKernel();
            if (pClientKernel == null)
                return;

        }
        public void add_widget(Node node)
        {
            node.add_child(this);
            node.add_child(m_btnExit);
        }
        public void button_press(int tag)
        {
            if (tag == 10200)
            {
                CGameScene pGameScene = (CGameScene)parent();
                Show(false);
            }

        }
        public void resize(Point pt, Size size)
        {
            base.resize(pt, size);

            set_position(pt);
            m_btnExit.set_position( new Point(pt.x_ + 73, pt.y_ + 300));
        }
        public void Show(bool bShow)
        {
            bool bVisible = visible();
            if (bVisible == bShow)
                return;

            set_visible(bShow);
            m_btnExit.set_visible(bShow);
            m_sprBackground.set_visible(bShow);
        }

    }
}
