using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    public class CMatchAgainLayer : Layer, Button_Listener
    {
        private Font m_Font;

        private Button_Widget m_btnCance;
        private Button_Widget m_btnMatch;

        private Sprite m_sprBackground;

        private int m_nCurGateGoldCount;
        private int m_nNextGateGoldCount;

        private Sprite[] m_sprCurGate;
        private Sprite[] m_sprCurGateGold;
        private Sprite[] m_sprNextGateGold;

        private static Random random_ = new Random();

        public CMatchAgainLayer()
        {
            this.m_sprCurGate = new Sprite[2];
            this.m_sprCurGateGold = new Sprite[9];
            this.m_sprNextGateGold = new Sprite[9];
            m_Font = Root.instance().font_manager().create_font_ttf("宋体", "fish\\simsun.ttc");
            m_Font.set_size(new Size(28, 32));

            m_btnCance = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_end_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_end_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_end_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_end_disable"));
            m_btnCance.set_tag(10300);
            m_btnCance.set_position(new Point(0, 0));
            m_btnCance.set_content_size(new Size(148, 51));
            m_btnCance.add_listener(this);

            m_btnMatch = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_continue_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_continue_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_continue_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_gate_continue_disable"));
            m_btnMatch.set_tag(10301);
            m_btnMatch.set_position(new Point(0, 0));
            m_btnMatch.set_content_size(new Size(148, 51));
            m_btnMatch.add_listener(this);

            m_sprBackground = new Sprite(Root.instance().imageset_manager().imageset("ui_load").image("gate_continue_back"));
            m_sprBackground.set_hot(new Point(0, 0));
            add_child(m_sprBackground);

            for (int i = 0; i < 9; i++)
            {
                m_sprCurGateGold[i] = new Sprite();
                add_child(m_sprCurGateGold[i]);

                m_sprNextGateGold[i] = new Sprite();
                add_child(m_sprNextGateGold[i]);
            }

            for (int i = 0; i < 9; i++)
            {
                m_sprCurGateGold[i].set_position(new Point(322 + i * 16, 220));
                m_sprNextGateGold[i].set_position(new Point(322 + i * 16, 182));
            }

            for (int i = 0; i < 2; i++)
            {
                m_sprCurGate[i] = new Sprite();
                add_child(m_sprCurGate[i]);
            }

            for (int i = 0; i < 2; i++)
            {
                m_sprCurGate[i].set_position(new Point(295 + i * 16, 106));
            }

            m_nCurGateGoldCount = 0;
            m_nNextGateGoldCount = 0;
        }

        public void add_widget(Node node)
        {
            node.add_child(this);
            node.add_child(m_btnCance);
            node.add_child(m_btnMatch);
        }
        public void button_press(int tag)
        {
            CGameScene pGameScene = (CGameScene)parent();
            if (tag == 10300)
            {
                Show(false);
                if (pGameScene.m_dwRoomType == 3)
                {
                    CMD_C_Gate_Ctrl_Send GateCtrlSend = new CMD_C_Gate_Ctrl_Send();

                    GateCtrlSend.wChair = pGameScene.GetMeChairID();
                    GateCtrlSend.cbFirst = 7;
                    GateCtrlSend.nGateCount = pGameScene.m_nCurGateCount;

                    //pGameScene->m_layRoles[pGameScene->GetMeChairID()]->SetFishGold(pGameScene->m_nFishScoreBase);

                    pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_GATE_CTRL_SEND, GateCtrlSend);

                    int nRand = 1 + random_.Next() % 4;
                    //pakcj Sleep(nRand * 1000);

                    ((CGameScene)parent()).window_closed(null);
                    Root.instance().queue_end_rendering();
                }
            }
            else if (tag == 10301)
            {
                if (pGameScene.m_dwRoomType == 3)
                {
                    CMD_C_Gate_Ctrl_Send GateCtrlSend = new CMD_C_Gate_Ctrl_Send();

                    GateCtrlSend.wChair = pGameScene.GetMeChairID();
                    GateCtrlSend.cbFirst = 2;
                    GateCtrlSend.nGateCount = pGameScene.m_nCurGateCount;

                    pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_GATE_CTRL_SEND, GateCtrlSend);
                }
                Show(false);
            }

        }
        public void resize(Point pt, Size size)
        {
            base.resize(pt, size);

            set_position(pt);
            m_btnCance.set_position(new Point(pt.x_ + 120, pt.y_ + 288));
            m_btnMatch.set_position(new Point(pt.x_ + 310, pt.y_ + 288));
        }

        public void Show(bool bShow)
        {
            bool bVisible = visible();
            if (bVisible == bShow)
                return;

            set_visible(bShow);
            m_btnCance.set_visible(bShow);
            m_btnMatch.set_visible(bShow);
            m_sprBackground.set_visible(bShow);

            CGameScene pGameScene = (CGameScene)parent();
            if (bShow)
            {
                SeCurGate(pGameScene.m_nCurGateCount + 1);
                SeCurGateGold(pGameScene.m_tagGateCtrlInf[pGameScene.m_nCurGateCount].nGetScore);
                SeNextGateGold(pGameScene.m_tagGateCtrlInf[pGameScene.m_nCurGateCount + 1].nGetScore);

                for (int i = 0; i < 9; i++)
                {
                    m_sprCurGateGold[i].set_position(new Point(326 + i * 16 - (6 - m_nCurGateGoldCount) * 8, 220));
                    m_sprNextGateGold[i].set_position(new Point(326 + i * 16 - (6 - m_nNextGateGoldCount) * 8, 182));
                }
            }
            else
            {
                SeCurGate(0);
                SeCurGateGold(0);
                SeNextGateGold(0);
            }
        }
        public void SeCurGate(int nNumber)
        {
            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(nNumber % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprCurGate[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprCurGate[0].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10);
            ostr.str("");
            ostr = ostr + "buy_bullet_number_" + nSingleNumber;
            m_sprCurGate[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));

            if (nNumber == 0)
            {
                m_sprCurGate[1].set_display_image(null);
            }
        }

        public void SeCurGateGold(int nNumber)
        {
            m_nCurGateGoldCount = 0;
            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(nNumber % 1000000000 / 100000000);
            if (nSingleNumber > 0)
            {
                bGotHead = true;
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprCurGateGold[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprCurGateGold[0].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100000000 / 10000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprCurGateGold[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprCurGateGold[1].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10000000 / 1000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprCurGateGold[2].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprCurGateGold[2].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 1000000 / 100000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprCurGateGold[3].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nCurGateGoldCount++;
            }
            else
            {
                m_sprCurGateGold[3].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100000 / 10000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprCurGateGold[4].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nCurGateGoldCount++;
            }
            else
            {
                m_sprCurGateGold[4].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10000 / 1000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprCurGateGold[5].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nCurGateGoldCount++;
            }
            else
            {
                m_sprCurGateGold[5].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 1000 / 100);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprCurGateGold[6].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nCurGateGoldCount++;
            }
            else
            {
                m_sprCurGateGold[6].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprCurGateGold[7].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nCurGateGoldCount++;
            }
            else
            {
                m_sprCurGateGold[7].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10);
            ostr.str("");
            ostr = ostr + "buy_bullet_number_" + nSingleNumber;
            m_sprCurGateGold[8].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));

            m_nCurGateGoldCount++;

            if (nNumber == 0)
            {
                m_sprCurGateGold[8].set_display_image(null);
            }
        }

        public void SeNextGateGold(int nNumber)
        {
            m_nNextGateGoldCount = 0;

            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(nNumber % 1000000000 / 100000000);
            if (nSingleNumber > 0)
            {
                bGotHead = true;
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprNextGateGold[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprNextGateGold[0].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100000000 / 10000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprNextGateGold[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprNextGateGold[1].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10000000 / 1000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprNextGateGold[2].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprNextGateGold[2].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 1000000 / 100000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprNextGateGold[3].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nNextGateGoldCount++;
            }
            else
            {
                m_sprNextGateGold[3].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100000 / 10000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprNextGateGold[4].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nNextGateGoldCount++;
            }
            else
            {
                m_sprNextGateGold[4].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10000 / 1000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprNextGateGold[5].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nNextGateGoldCount++;
            }
            else
            {
                m_sprNextGateGold[5].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 1000 / 100);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprNextGateGold[6].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nNextGateGoldCount++;
            }
            else
            {
                m_sprNextGateGold[6].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprNextGateGold[7].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nNextGateGoldCount++;
            }
            else
            {
                m_sprNextGateGold[7].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10);
            ostr.str("");
            ostr = ostr + "buy_bullet_number_" + nSingleNumber;
            m_sprNextGateGold[8].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));

            m_nNextGateGoldCount++;

            if (nNumber == 0)
            {
                m_sprNextGateGold[8].set_display_image(null);
            }
        }

    }
}
