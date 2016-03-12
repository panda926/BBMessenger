using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;

namespace FishClient
{
    public class CFrameLayer : Layer, Button_Listener
    {
        private Image image_tl_;
        private Image image_tm_;
        private Image image_tr_;
        private Image image_ml_;
        private Image image_mr_;
        private Image image_bl_;
        private Image image_bm_;
        private Image image_br_;

        private Font m_Font;

        private Button_Widget m_btnClose;
        private Button_Widget m_btnMax;
        private Button_Widget m_btnMin;
        private Button_Widget m_btnHelp;
        private Button_Widget m_btnMessage;
        private Button_Widget m_btnSetting;
        private Button_Widget m_btnShow;
        private Button_Widget m_btnGateHelp;

        private int m_nScene;

        public CFrameLayer(int nScene)
        {
            string ui;

            if (nScene == 0)
            {
                ui = "ui_load";
            }
            else
            {
                ui = "ui_game";
            }

            m_nScene = nScene;

            image_tl_ = Root.instance().imageset_manager().imageset(ui).image("frame_tl_");
            image_tm_ = Root.instance().imageset_manager().imageset(ui).image("frame_tm_");
            image_tr_ = Root.instance().imageset_manager().imageset(ui).image("frame_tr_");
            image_ml_ = Root.instance().imageset_manager().imageset(ui).image("frame_ml_");
            image_mr_ = Root.instance().imageset_manager().imageset(ui).image("frame_mr_");
            image_bl_ = Root.instance().imageset_manager().imageset(ui).image("frame_bl_");
            image_bm_ = Root.instance().imageset_manager().imageset(ui).image("frame_bm_");
            image_br_ = Root.instance().imageset_manager().imageset(ui).image("frame_br_");

            m_Font = Root.instance().font_manager().create_font_ttf("simsun", "fish\\simsun.ttc");
            m_Font.set_size(new Size(14, 15));

            m_btnClose = new Button_Widget(Root.instance().imageset_manager().imageset(ui).image("btn_close_image"), Root.instance().imageset_manager().imageset(ui).image("btn_close_over"), Root.instance().imageset_manager().imageset(ui).image("btn_close_down"), Root.instance().imageset_manager().imageset(ui).image("btn_close_disable"));
            m_btnClose.set_tag(10000);
            m_btnClose.set_position(new Point(1280 - 54, 4));
            m_btnClose.set_content_size(new Size(41, 24));
            m_btnClose.add_listener(this);
            m_btnClose.set_visible(false);

            m_btnMax = new Button_Widget(Root.instance().imageset_manager().imageset(ui).image("btn_max_image"), Root.instance().imageset_manager().imageset(ui).image("btn_max_over"), Root.instance().imageset_manager().imageset(ui).image("btn_max_down"), Root.instance().imageset_manager().imageset(ui).image("btn_max_disable"));
            m_btnMax.set_tag(10001);
            m_btnMax.set_position(new Point(1280 - 54 - 30, 4));
            m_btnMax.set_content_size(new Size(29, 24));
            m_btnMax.add_listener(this);
            m_btnMax.set_visible(false);

            m_btnMin = new Button_Widget(Root.instance().imageset_manager().imageset(ui).image("btn_min_image"), Root.instance().imageset_manager().imageset(ui).image("btn_min_over"), Root.instance().imageset_manager().imageset(ui).image("btn_min_down"), Root.instance().imageset_manager().imageset(ui).image("btn_min_disable"));
            m_btnMin.set_tag(10002);
            m_btnMin.set_position(new Point(1280 - 54 - 30 - 33, 4));
            m_btnMin.set_content_size(new Size(32, 24));
            m_btnMin.add_listener(this);
            m_btnMin.set_visible(false);

            m_btnHelp = new Button_Widget(Root.instance().imageset_manager().imageset(ui).image("btn_help_image"), Root.instance().imageset_manager().imageset(ui).image("btn_help_over"), Root.instance().imageset_manager().imageset(ui).image("btn_help_down"), Root.instance().imageset_manager().imageset(ui).image("btn_help_disable"));
            m_btnHelp.set_tag(10003);
            m_btnHelp.set_position(new Point(1072 - 43, 4));
            m_btnHelp.set_content_size(new Size(42, 24));
            m_btnHelp.add_listener(this);
            m_btnHelp.set_visible(false);

            m_btnMessage = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_message_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_message_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_message_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_message_disable"));
            m_btnMessage.set_tag(10005);
            m_btnMessage.set_position(new Point(1072, 4));
            m_btnMessage.set_content_size(new Size(34, 24));
            m_btnMessage.add_listener(this);
            m_btnMessage.set_visible(false);

            m_btnSetting = new Button_Widget(Root.instance().imageset_manager().imageset(ui).image("btn_setting_image"), Root.instance().imageset_manager().imageset(ui).image("btn_setting_over"), Root.instance().imageset_manager().imageset(ui).image("btn_setting_down"), Root.instance().imageset_manager().imageset(ui).image("btn_setting_disable"));
            m_btnSetting.set_tag(10004);
            m_btnSetting.set_position(new Point(1072 + 34, 4));
            m_btnSetting.set_content_size(new Size(42, 24));
            m_btnSetting.add_listener(this);
            m_btnSetting.set_visible(false);

            m_btnShow = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_index_show_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_index_show_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_index_show_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_index_show_disable"));
            m_btnShow.set_tag(10008);
            m_btnShow.set_position(new Point(1072 - 160, -100));
            m_btnShow.set_content_size(new Size(85, 24));
            m_btnShow.add_listener(this);
            m_btnShow.set_visible(false);

            m_btnGateHelp = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_help_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_help_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_help_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_help_disable"));
            m_btnGateHelp.set_tag(10009);
            m_btnGateHelp.set_position(new Point(1072 - 160, -100));
            m_btnGateHelp.set_content_size(new Size(73, 24));
            m_btnGateHelp.add_listener(this);
            m_btnGateHelp.set_visible(false);
        }

        public override void draw()
        {
            base.draw();

            if (!visible_)
                return;

            Point pt = position_absolute();
            Size size = content_size();

            Point pt_draw = new Point();
            Size size_draw = new Size();

            //top
            pt_draw = pt;
            image_tl_.draw(pt_draw);

            pt_draw = pt + new Point(5, 0);
            size_draw.width_ = size.width_ - 5;
            size_draw.height_ = 32;
            image_tm_.draw(pt_draw, size_draw);

            pt_draw = pt + new Point(size.width_ - 248, 0);
            image_tr_.draw(pt_draw);

            //middle
            pt_draw = pt + new Point(0, 32);
            size_draw.width_ = 5;
            size_draw.height_ = size.height_ - 32;
            image_ml_.draw(pt_draw, size_draw);

            pt_draw = pt + new Point(1280 - 5, 32);
            size_draw.width_ = 5;
            size_draw.height_ = size.height_ - 32;
            image_mr_.draw(pt_draw, size_draw);

            //bottom
            pt_draw = pt + new Point(0, size.height_ - 5);
            image_bl_.draw(pt_draw);

            pt_draw = pt_draw + new Point(5, 0);
            size_draw.width_ = size.width_ - 5;
            size_draw.height_ = 5;
            image_bm_.draw(pt_draw, size_draw);

            pt_draw = pt + new Point(size.width_ - 5, size.height_ - 5);
            image_br_.draw(pt_draw);

            string szWindowText = new string(new char[64]);
            //pakcj GetWindowText(Root.instance().render_window().window_handle(), szWindowText, 64);

            m_Font.set_align(0);
            //  m_Font->draw_string(new Point(20,20), szWindowText, Color(252,187,123));
            m_Font.draw_string(new Point(20, 20), szWindowText, new Color(252, 187, 123));

        }
        public void add_widget(Node node)
        {
            node.add_child(this);
            node.add_child(m_btnClose);
            node.add_child(m_btnMax);
            node.add_child(m_btnMin);
            node.add_child(m_btnHelp);
            node.add_child(m_btnMessage);
            node.add_child(m_btnSetting);
            node.add_child(m_btnShow);
            node.add_child(m_btnGateHelp);
        }

        public void button_press(int tag)
        {
            if (tag == 10000)
            {
                if (m_nScene > 0)
                {
                    CGameScene pGameScene = (CGameScene)parent();
                    if (!pGameScene.m_layAccount.visible())
                    {
                        pGameScene.m_layBuyBulletLayer.ShowWidnow(false);
                        if (!pGameScene.m_ReMatchStart.visible())
                        {
                            pGameScene.m_layAccount.ShowWidnow(true);
                        }
                    }
                               ((CGameScene)parent()).window_closed(null);
                               Root.instance().queue_end_rendering();	
                }
                else
                {
                              //theApp->stop_loading_thread();
                              Root.instance().queue_end_rendering();	
                }


            }
            else if (tag == 10002)
            {
                //pakcj ShowWindow(Root.instance().render_window().window_handle(), SW_MINIMIZE);
            }
            else if (tag == 10004)
            {
                if (m_nScene > 0)
                {
                    CGameScene pGameScene = (CGameScene)parent();
                    if (!pGameScene.m_laySetting.visible())
                    {
                        pGameScene.m_laySetting.ShowWidnow(true);
                    }
                }

            }
            else if (tag == 10005)
            {
                CGameScene pGameScene = (CGameScene)parent();
                pGameScene.m_sendMessage.button_press(10202);
            }
            else if (tag == 10003)
            {
                CGameScene pGameScene = (CGameScene)parent();
                pGameScene.m_Help.Show(true);
            }
            else if (tag == 10008)
            {
                CGameScene pGameScene = (CGameScene)parent();
                //pakcj ShellExecute(null, "open", pGameScene.m_cbMatchIndexURL, null, null, SW_SHOWNORMAL);
            }
            else if (tag == 10009)
            {
                CGameScene pGameScene = (CGameScene)parent();
                if (pGameScene.m_dwRoomType == 3)
                {
                    if (pGameScene.m_MatchStart.visible() || pGameScene.m_MatchAgainLayer.visible() || pGameScene.m_GateEndLayer.visible())
                    {
                        pGameScene.m_GateHelpLayer.Show(false);
                    }
                    else
                    {
                        pGameScene.m_GateHelpLayer.Show(true);
                    }
                }
            }
            else if (tag == 10010)
            {
                CGameScene pGameScene = (CGameScene)parent();
                if ((pGameScene.m_dwRoomType == 0) || (pGameScene.m_dwRoomType == 3))
                {
                    m_btnShow.set_position(new Point(1072 - 160, -100));
                    m_btnShow.set_visible(false);

                    if (pGameScene.m_dwRoomType == 3)
                    {
                        m_btnGateHelp.set_visible(true);
                        m_btnGateHelp.set_position(new Point(1072 - 120, 4));
                    }
                }
                else if ((pGameScene.m_dwRoomType == 1) || (pGameScene.m_dwRoomType == 2))
                {
                    m_btnShow.set_visible(true);
                    m_btnShow.set_position(new Point(1072 - 142, 4));
                    m_btnGateHelp.set_position(new Point(1072 - 160, -100));
                    m_btnGateHelp.set_visible(false);
                }
            }
        }
    }
}
