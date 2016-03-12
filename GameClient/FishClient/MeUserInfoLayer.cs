using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    public class CMeUserInfoLayer : Layer, Button_Listener
    {
        public CMeUserInfoLayer()
        {
            m_ptDown = new Point(196, -116);
            m_ptUp = new Point(196, 32);

            m_Font = Root.instance().font_manager().create_font_ttf("simsun", "fish\\simsun.ttc");
            m_Font.set_size(new Size(14, 15));

            m_btnAccount = new Button_Widget(Root.instance().imageset_manager().imageset("role").image("btn_account_image"), Root.instance().imageset_manager().imageset("role").image("btn_account_over"), Root.instance().imageset_manager().imageset("role").image("btn_account_down"), Root.instance().imageset_manager().imageset("role").image("btn_account_disable"));
            m_btnAccount.set_tag(10100);
            m_btnAccount.set_position(new Point(514, 560));
            m_btnAccount.set_content_size(new Size(76, 27));
            m_btnAccount.add_listener(this);

            m_btnBullet = new Button_Widget(Root.instance().imageset_manager().imageset("role").image("btn_buy_image"), Root.instance().imageset_manager().imageset("role").image("btn_buy_over"), Root.instance().imageset_manager().imageset("role").image("btn_buy_down"), Root.instance().imageset_manager().imageset("role").image("btn_buy_disable"));
            m_btnBullet.set_tag(10101);
            m_btnBullet.set_position(new Point(514, 560));
            m_btnBullet.set_content_size(new Size(76, 27));
            m_btnBullet.add_listener(this);

            m_btnDown = new Button_Widget(Root.instance().imageset_manager().imageset("role").image("btn_me_info_down_image"), Root.instance().imageset_manager().imageset("role").image("btn_me_info_down_over"), Root.instance().imageset_manager().imageset("role").image("btn_me_info_down_down"), Root.instance().imageset_manager().imageset("role").image("btn_me_info_down_disable"));
            m_btnDown.set_tag(10102);
            m_btnDown.set_position(new Point(514, 560));
            m_btnDown.set_content_size(new Size(86, 15));
            m_btnDown.set_visible(false);
            m_btnDown.add_listener(this);

            m_btnUp = new Button_Widget(Root.instance().imageset_manager().imageset("role").image("btn_me_info_up_image"), Root.instance().imageset_manager().imageset("role").image("btn_me_info_up_over"), Root.instance().imageset_manager().imageset("role").image("btn_me_info_up_down"), Root.instance().imageset_manager().imageset("role").image("btn_me_info_up_disable"));
            m_btnUp.set_tag(10103);
            m_btnUp.set_position(new Point(514, 560));
            m_btnUp.set_content_size(new Size(86, 15));
            m_btnUp.set_visible(true);
            m_btnUp.add_listener(this);

            m_sprBackground = new Sprite(Root.instance().imageset_manager().imageset("role").image("btn_me_info_image"));
            m_sprBackground.set_hot(new Point(0, 0));
            add_child(m_sprBackground);

            m_Font = Root.instance().font_manager().create_font_ttf("simsun", "fish\\simsun.ttc");
            m_Font.set_size(new Size(15, 16));

            m_bIsOk = false;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void update(float dt);

        public override void draw()
        {
            base.draw();

            CGameScene pGameScene = (CGameScene)parent();
            CClientKernel pClientKernel = pGameScene.GetClientKernel();
            if (pClientKernel == null)
                return;

            UserInfo pUserData = pClientKernel.GetMeUserInfo();


            //for(WORD wChair=0;wChair<GAME_PLAYER;wChair++)
            //{
            //	if(pGameScene->m_layRoles[wChair]->m_sprMessage->visible())
            //	{
            //		Point pt = pGameScene->m_layRoles[wChair]->m_sprMessage->position() + pGameScene->m_layRoles[wChair]->position();
            //		std::ostringstream ostr;
            //		ostr = ostr + "new Point(" + pt.x_ + "," + pt.y_ +")" + pGameScene->m_layRoles[wChair]->m_cbShowData;

            //		m_Font->draw_string(pt, ostr.str().c_str(), Color(176,222,238));

            //	}

            //}

            if (pUserData != null)
            {
                int wChair = ((CGameScene)parent()).GetClientKernel().GetMeChairID();

                Point pt = position_absolute();
                pt.x_ += 56;
                pt.y_ += 14;

                //   m_Font->draw_string(pt, pUserData->szNickName, Color(14,236,212));
                //m_Font->draw_string(pt, g_WideCharToMultiByte(pUserData->szNickName), Color(14,236,212));
                m_Font.draw_string(pt, pUserData.Nickname, new Color(14, 236, 212));

                //std::string szRank;
                //if (pUserData->lScore>0 && pUserData->lScore<1000)
                //{
                //    szRank = "渔夫";
                //}
                //else
                //{
                //    szRank = "船长";
                //}

                ostringstream ostrRank = new ostringstream();
                ostrRank = ostrRank + pGameScene.m_layRoles[wChair].GetExpValue();

                pt.x_ += 160;
                m_Font.draw_string(pt, ostrRank.str(), new Color(14, 236, 212));

                pt.x_ -= 160;
                pt.y_ += 28;

                int nFishGold = pGameScene.m_layRoles[pGameScene.GetMeChairID()].GetFishGold();

                ostringstream ostr = new ostringstream();
                ostr = ostr + pUserData.GetGameMoney() + "(+" + nFishGold + ")";

                m_Font.draw_string(pt, ostr.str(), new Color(14, 236, 212));

                pt.x_ += 30;
                pt.y_ += 27;

                ostringstream ostr1 = new ostringstream();
                ostr1 = ostr1 + pGameScene.m_layRoles[wChair].GetMaxMulRate();
                m_Font.draw_string(pt, ostr1.str(), new Color(14, 236, 212));

                pt.x_ += 16;
                pt.y_ += 26;

                int nExpToLevel = FishDefine.EXP_CHANGE_TO_LEVEL;
                ostringstream ostr2 = new ostringstream();
                ostr2 = ostr2 + pGameScene.m_layRoles[wChair].GetFireCount() % FishDefine.EXP_CHANGE_TO_LEVEL + " / " + nExpToLevel;
                m_Font.draw_string(pt, ostr2.str(), new Color(14, 236, 212));
            }
        }

        public void add_widget(Node node)
        {
            node.add_child(this);
            node.add_child(m_btnAccount);
            node.add_child(m_btnBullet);
            node.add_child(m_btnDown);
            node.add_child(m_btnUp);
        }

        public void button_press(int tag)
        {
            if (tag == 10102)
            {
                m_bIsOk = true;
                resize(m_ptUp, new Size(253, 173));
                m_btnDown.set_visible(false);
                m_btnUp.set_visible(true);

                CGameScene pGameScene = (CGameScene)parent();
                pGameScene.m_TimerShowUserInf.reset();
            }
            else if (tag == 10103)
            {
                m_bIsOk = true;
                resize(m_ptDown, new Size(253, 173));
                m_btnDown.set_visible(true);
                m_btnUp.set_visible(false);
            }
            else if (tag == 10100)
            {
                CGameScene pGameScene = (CGameScene)parent();
                if (!pGameScene.m_layAccount.visible())
                {
                    pGameScene.m_layBuyBulletLayer.ShowWidnow(false);
                    pGameScene.m_layAccount.ShowWidnow(true);

                    resize(m_ptDown, new Size(253, 173));
                    m_btnDown.set_visible(true);
                    m_btnUp.set_visible(false);
                }
            }
            else if (tag == 10101)
            {
                CGameScene pGameScene = (CGameScene)parent();
                int nFishGold = pGameScene.m_layRoles[pGameScene.GetMeChairID()].GetFishGold();
                if (!pGameScene.m_layAccount.visible())
                {
                    if (pGameScene.m_dwRoomType == 0)
                    {
                        if (!pGameScene.m_layBuyBulletLayer.IsSendBuyBulletMessage())
                        {
                            pGameScene.m_layBuyBulletLayer.ShowWidnow(true);

                            resize(m_ptDown, new Size(253, 173));
                            m_btnDown.set_visible(true);
                            m_btnUp.set_visible(false);
                        }
                    }
                }
            }
        }
        public void resize(Point pt, Size size)
        {
            base.resize(pt, size);

            set_position(pt);
            m_btnAccount.set_position(new Point(pt.x_ + 34, pt.y_ + 114));
            m_btnBullet.set_position(new Point(pt.x_ + 140, pt.y_ + 114));
            m_btnDown.set_position(new Point(pt.x_ + 84, pt.y_ + 150));
            m_btnUp.set_position(new Point(pt.x_ + 84, pt.y_ + 150));
        }


        public bool m_bIsOk;
        public Point m_ptDown = new Point();
        public Point m_ptUp = new Point();

        private Font m_Font;

        private Button_Widget m_btnAccount;
        private Button_Widget m_btnBullet;
        private Button_Widget m_btnDown;
        private Button_Widget m_btnUp;

        private Sprite m_sprBackground;
    }
}
