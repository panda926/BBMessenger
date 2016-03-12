using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    public class CMatchStartLayer : Layer, Button_Listener
    {
        public int m_dwMatchScoreBase;

        private Font m_Font;

        private Button_Widget m_btnStart;

        private Sprite m_sprBackground;
        private Sprite[] m_sprMatchGold;

        public CMatchStartLayer()
        {
            this.m_sprMatchGold = new Sprite[9];
            m_Font = Root.instance().font_manager().create_font_ttf("simsun", "fish\\simsun.ttc");
            m_Font.set_size(new Size(14, 15));

            m_btnStart = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_match_start_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_start_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_start_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_match_start_disable"));
            m_btnStart.set_tag(10300);
            m_btnStart.set_position(new Point(0, 0));
            m_btnStart.set_content_size(new Size(354, 46));
            m_btnStart.add_listener(this);

            m_sprBackground = new Sprite();
            m_sprBackground.set_hot(new Point(0, 0));
            add_child(m_sprBackground);

            for (int i = 0; i < 9; i++)
            {
                m_sprMatchGold[i] = new Sprite();
                add_child(m_sprMatchGold[i]);
            }

            for (int i = 0; i < 9; i++)
            {
                m_sprMatchGold[i].set_position(new Point(136 + i * 16, 138));
            }

        }

        public void add_widget(Node node)
        {
            node.add_child(this);
            node.add_child(m_btnStart);
        }
        public void button_press(int tag)
        {
            if (tag == 10300)
            {
                //CGameScene *pGameScene = (CGameScene *)parent();

                //CMD_GF_WriteMatchScore pWriteMatchScore;
                //ZeroMemory(&pWriteMatchScore,sizeof(pWriteMatchScore));
                //pWriteMatchScore.lMatchScore=2000; 
                //pGameScene->GetClientKernel()->SendWriteMatchScore(&pWriteMatchScore,sizeof(pWriteMatchScore));

                //Show(false);

                CGameScene pGameScene = (CGameScene)parent();

                if ((pGameScene.m_dwRoomType == 1) || (pGameScene.m_dwRoomType == 2))
                {
                    CMD_C_Match_Start MatchStar = new CMD_C_Match_Start();
                    MatchStar.wChair = pGameScene.GetMeChairID();
                    MatchStar.dwScore = 0;
                    MatchStar.dwMatchScore = 0;

                    pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_MATCH_START, MatchStar);
                }
                else if (pGameScene.m_dwRoomType == 3)
                {
                    CMD_C_Gate_Ctrl_Send GateCtrlSend = new CMD_C_Gate_Ctrl_Send();

                    GateCtrlSend.wChair = pGameScene.GetMeChairID();
                    GateCtrlSend.cbFirst = 0;
                    GateCtrlSend.nGateCount = 0;

                    pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_GATE_CTRL_SEND, GateCtrlSend);
                    Show(false);

                }

            }
            else if (tag == 10302)
            {
                CGameScene pGameScene = (CGameScene)parent();

                if (pGameScene.m_dwRoomType == 2)
                {
                    m_sprBackground.set_display_image(Root.instance().imageset_manager().imageset("ui_load").image("match_start0_image"));
                }
                else if (pGameScene.m_dwRoomType == 1)
                {
                    m_sprBackground.set_display_image(Root.instance().imageset_manager().imageset("ui_load").image("match_start1_image"));
                }
                else if (pGameScene.m_dwRoomType == 3)
                {
                    m_sprBackground.set_display_image(Root.instance().imageset_manager().imageset("ui_load").image("match_start2_image"));
                }
            }
        }
        public void resize(Point pt, Size size)
        {
            base.resize(pt, size);

            set_position(pt);
            m_btnStart.set_position(new Point(pt.x_ + 6, pt.y_ + 164));
        }
        public void Show(bool bShow)
        {
            bool bVisible = visible();
            if (bVisible == bShow)
                return;

            set_visible(bShow);
            m_btnStart.set_visible(bShow);
            m_sprBackground.set_visible(bShow);

            for (int i = 0; i < 9; i++)
            {
                m_sprMatchGold[i].set_visible(bShow);
            }

        }

        public void SetMatchGold(int dwMatchScoreBase)
        {
            m_dwMatchScoreBase = dwMatchScoreBase;

            //std::ostringstream ostrTemp;
            //ostrTemp + m_dwMatchScoreBase;
            //MessageBox(0,ostrTemp.str().c_str(),"",0);
            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(m_dwMatchScoreBase % 1000000000 / 100000000);
            if (nSingleNumber > 0)
            {
                bGotHead = true;
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprMatchGold[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprMatchGold[0].set_display_image(null);
            }

            nSingleNumber = (int)(m_dwMatchScoreBase % 100000000 / 10000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprMatchGold[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprMatchGold[1].set_display_image(null);
            }

            nSingleNumber = (int)(m_dwMatchScoreBase % 10000000 / 1000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprMatchGold[2].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprMatchGold[2].set_display_image(null);
            }

            nSingleNumber = (int)(m_dwMatchScoreBase % 1000000 / 100000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprMatchGold[3].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprMatchGold[3].set_display_image(null);
            }

            nSingleNumber = (int)(m_dwMatchScoreBase % 100000 / 10000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprMatchGold[4].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprMatchGold[4].set_display_image(null);
            }

            nSingleNumber = (int)(m_dwMatchScoreBase % 10000 / 1000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprMatchGold[5].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprMatchGold[5].set_display_image(null);
            }

            nSingleNumber = (int)(m_dwMatchScoreBase % 1000 / 100);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprMatchGold[6].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprMatchGold[6].set_display_image(null);
            }

            nSingleNumber = (int)(m_dwMatchScoreBase % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprMatchGold[7].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprMatchGold[7].set_display_image(null);
            }

            nSingleNumber = (int)(m_dwMatchScoreBase % 10);
            ostr.str("");
            ostr = ostr + "buy_bullet_number_" + nSingleNumber;
            m_sprMatchGold[8].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));

            if (m_dwMatchScoreBase == 0)
            {
                m_sprMatchGold[8].set_display_image(null);
            }
        }


    }
}
