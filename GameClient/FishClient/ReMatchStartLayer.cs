using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;

namespace FishClient
{
    public class CReMatchStartLayer : Layer, Button_Listener
    {
        public Sprite m_sprResult;

        private Font m_Font;

        private Button_Widget m_btnCance;
        private Button_Widget m_btnReMatch;
        private Button_Widget m_btnShow;

        private Sprite m_sprBackground;

        public CReMatchStartLayer()
        {
            m_Font = Root.instance().font_manager().create_font_ttf("黑体", "fish\\simsun.ttc");
            m_Font.set_size(new Size(20, 22));

            m_btnCance = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_match_cancel_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_cancel_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_cancel_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_cancel_disable"));
            m_btnCance.set_tag(10200);
            m_btnCance.set_position(new Point(0, 0));
            m_btnCance.set_content_size(new Size(104, 21));
            m_btnCance.add_listener(this);

            m_btnReMatch = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_match_restart_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_restart_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_restart_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_restart_disable"));
            m_btnReMatch.set_tag(10201);
            m_btnReMatch.set_position(new Point(0, 0));
            m_btnReMatch.set_content_size(new Size(104, 21));
            m_btnReMatch.add_listener(this);

            m_btnShow = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_match_watch_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_watch_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_watch_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_watch_disable"));
            m_btnShow.set_tag(10202);
            m_btnShow.set_position(new Point(0, 0));
            m_btnShow.set_content_size(new Size(104, 21));
            m_btnShow.add_listener(this);

            m_sprBackground = new Sprite(Root.instance().imageset_manager().imageset("ui_load").image("match_reset_image"));
            m_sprBackground.set_hot(new Point(0, 0));
            add_child(m_sprBackground);

            m_sprResult = new Sprite();
            m_sprResult.set_position(new Point(0, 0));
            add_child(m_sprResult);
        }

        public override void draw()
        {
            base.draw();

            CGameScene pGameScene = (CGameScene)parent();
            CClientKernel pClientKernel = pGameScene.GetClientKernel();
            if (pClientKernel == null)
                return;

            if (((pGameScene.m_dwRoomType == 1) || (pGameScene.m_dwRoomType == 2)) && (visible()))
            {
                Point pt = position_absolute();

                pt.x_ += 216;
                pt.y_ += 84;

                ostringstream ostr2 = new ostringstream();
                ostr2 = ostr2 + pGameScene.m_layRoles[pGameScene.GetMeChairID()].m_dwMatchScore;
                m_Font.draw_string(pt, ostr2.str(), new Color(241, 37, 0));

                pt.x_ += 142;

                ostr2.str("");
                ostr2 = ostr2 + pGameScene.m_layRoles[pGameScene.GetMeChairID()].m_dwMatchIndex;
                m_Font.draw_string(pt, ostr2.str(), new Color(241, 37, 0));

            }

        }
        public void add_widget(Node node)
        {
            node.add_child(this);
            node.add_child(m_btnCance);
            node.add_child(m_btnReMatch);
            node.add_child(m_btnShow);
        }
        public void button_press(int tag)
        {
            CGameScene pGameScene = (CGameScene)parent();
            if (tag == 10200)
            {
                pGameScene.window_closed(null);
                Root.instance().queue_end_rendering();
            }
            else if (tag == 10201)
            {
                pGameScene.m_MatchStart.button_press(10300);
                Show(false);
            }
            else if (tag == 10202)
            {
                //pakcj ShellExecute(null, "open", pGameScene.m_cbMatchIndexURL, null, null, SW_SHOWNORMAL);
            }

        }
        public void resize(Point pt, Size size)
        {
            base.resize(pt, size);

            set_position(pt);
            m_btnCance.set_position(new Point(pt.x_ + 81, pt.y_ + 158));
            m_btnReMatch.set_position(new Point(pt.x_ + 189, pt.y_ + 158));
            m_btnShow.set_position(new Point(pt.x_ + 298, pt.y_ + 158));
            m_sprResult.set_position(new Point(pt.x_ - 168, pt.y_ - 90));
        }
        public void Show(bool bShow)
        {
            bool bVisible = visible();
            if (bVisible == bShow)
                return;

            set_visible(bShow);
            m_btnCance.set_visible(bShow);
            m_btnReMatch.set_visible(bShow);
            m_btnShow.set_visible(bShow);
            m_sprBackground.set_visible(bShow);
            m_sprResult.set_visible(bShow);
        }


    }
}
