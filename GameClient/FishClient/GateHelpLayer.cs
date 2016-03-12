using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;

namespace FishClient
{
    public class CGateHelpLayer : Layer, Button_Listener
    {
        private Font m_Font;

        private Button_Widget m_btnEnter;

        private int m_nFishGoldCount;
        private int m_nGetGoldCount;

        private Sprite m_sprBackground;
        private Sprite[] m_sprFishGold;
        private Sprite[] m_sprGetGold;
        private Sprite[] m_sprCurGate;
        private Sprite[] m_sprAllGate;

        public CGateHelpLayer()
        {
            this.m_sprFishGold = new Sprite[9];
            this.m_sprGetGold = new Sprite[9];
            this.m_sprCurGate = new Sprite[2];
            this.m_sprAllGate = new Sprite[2];
            m_Font = Root.instance().font_manager().create_font_ttf("宋体", "fish\\simsun.ttc");
            m_Font.set_size(new Size(28, 32));

            m_btnEnter = new Button_Widget(Root.instance().imageset_manager().imageset("ui_load").image("btn_help_exit_image"), Root.instance().imageset_manager().imageset("ui_load").image("btn_help_exit_over"), Root.instance().imageset_manager().imageset("ui_load").image("btn_help_exit_down"), Root.instance().imageset_manager().imageset("ui_load").image("btn_help_exit_disable"));
            m_btnEnter.set_tag(10500);
            m_btnEnter.set_position(new Point(0, 0));
            m_btnEnter.set_content_size(new Size(148, 51));
            m_btnEnter.add_listener(this);

            m_sprBackground = new Sprite(Root.instance().imageset_manager().imageset("ui_load").image("gate_help_back"));
            m_sprBackground.set_hot(new Point(0, 0));
            add_child(m_sprBackground);

            for (int i = 0; i < 9; i++)
            {
                m_sprFishGold[i] = new Sprite();
                add_child(m_sprFishGold[i]);

                m_sprGetGold[i] = new Sprite();
                add_child(m_sprGetGold[i]);
            }

            for (int i = 0; i < 2; i++)
            {
                m_sprCurGate[i] = new Sprite();
                add_child(m_sprCurGate[i]);

                m_sprAllGate[i] = new Sprite();
                add_child(m_sprAllGate[i]);
            }

            m_nFishGoldCount = 0;
            m_nGetGoldCount = 0;

        }

        public void add_widget(Node node)
        {
            node.add_child(this);
            node.add_child(m_btnEnter);
        }
        public void button_press(int tag)
        {
            if (tag == 10500)
            {
                Show(false);
            }
        }
        public void resize(Point pt, Size size)
        {
            base.resize(pt, size);

            set_position(pt);
            m_btnEnter.set_position(new Point(pt.x_ + 248, pt.y_ + 188));
        }
        public void Show(bool bShow)
        {
            bool bVisible = visible();
            if (bVisible == bShow)
                return;

            set_visible(bShow);
            m_btnEnter.set_visible(bShow);
            m_sprBackground.set_visible(bShow);

            CGameScene pGameScene = (CGameScene)parent();
            if (bShow)
            {
                SeCurGate(pGameScene.m_nCurGateCount + 1);
                SeAllGate(pGameScene.m_nGateCount);
                SeFishGold(pGameScene.m_tagGateCtrlInf[pGameScene.m_nCurGateCount].nFishScore);
                SeGetGold(pGameScene.m_tagGateCtrlInf[pGameScene.m_nCurGateCount].nGetScore);

                for (int i = 0; i < 2; i++)
                {
                    m_sprCurGate[i].set_position(new Point(142 + i * 16, 114));
                    m_sprAllGate[i].set_position(new Point(266 + i * 22, 60));
                }

                for (int i = 0; i < 9; i++)
                {
                    m_sprFishGold[i].set_position(new Point(318 + i * 16 - (9 - m_nFishGoldCount) * 8, 114));
                    m_sprGetGold[i].set_position(new Point(318 + i * 16 - (9 - m_nGetGoldCount) * 8, 148));
                }
            }
            else
            {
                SeCurGate(0);
                SeAllGate(0);
                SeFishGold(0);
                SeGetGold(0);
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
        public void SeAllGate(int nNumber)
        {
            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(nNumber % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "connon_number_" + nSingleNumber;
                m_sprAllGate[0].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }
            else
            {
                m_sprAllGate[0].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10);
            ostr.str("");
            ostr = ostr + "connon_number_" + nSingleNumber;
            m_sprAllGate[1].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));

            if (nNumber == 0)
            {
                m_sprAllGate[1].set_display_image(null);
            }
        }

        public void SeFishGold(int nNumber)
        {
            m_nFishGoldCount = 0;
            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(nNumber % 1000000000 / 100000000);
            if (nSingleNumber > 0)
            {
                bGotHead = true;
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nFishGoldCount++;
            }
            else
            {
                m_sprFishGold[0].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100000000 / 10000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nFishGoldCount++;
            }
            else
            {
                m_sprFishGold[1].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10000000 / 1000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[2].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nFishGoldCount++;
            }
            else
            {
                m_sprFishGold[2].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 1000000 / 100000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[3].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nFishGoldCount++;
            }
            else
            {
                m_sprFishGold[3].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100000 / 10000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[4].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nFishGoldCount++;
            }
            else
            {
                m_sprFishGold[4].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10000 / 1000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[5].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nFishGoldCount++;
            }
            else
            {
                m_sprFishGold[5].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 1000 / 100);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[6].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nFishGoldCount++;
            }
            else
            {
                m_sprFishGold[6].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[7].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nFishGoldCount++;
            }
            else
            {
                m_sprFishGold[7].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10);
            ostr.str("");
            ostr = ostr + "buy_bullet_number_" + nSingleNumber;
            m_sprFishGold[8].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));

            m_nFishGoldCount++;

            if (nNumber == 0)
            {
                m_sprFishGold[8].set_display_image(null);
            }
        }

        public void SeGetGold(int nNumber)
        {
            m_nGetGoldCount = 0;

            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(nNumber % 1000000000 / 100000000);
            if (nSingleNumber > 0)
            {
                bGotHead = true;
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGetGold[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nGetGoldCount++;
            }
            else
            {
                m_sprGetGold[0].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100000000 / 10000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGetGold[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nGetGoldCount++;
            }
            else
            {
                m_sprGetGold[1].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10000000 / 1000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGetGold[2].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nGetGoldCount++;
            }
            else
            {
                m_sprGetGold[2].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 1000000 / 100000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGetGold[3].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nGetGoldCount++;
            }
            else
            {
                m_sprGetGold[3].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100000 / 10000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGetGold[4].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nGetGoldCount++;
            }
            else
            {
                m_sprGetGold[4].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10000 / 1000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGetGold[5].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nGetGoldCount++;
            }
            else
            {
                m_sprGetGold[5].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 1000 / 100);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGetGold[6].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nGetGoldCount++;
            }
            else
            {
                m_sprGetGold[6].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGetGold[7].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
                m_nGetGoldCount++;
            }
            else
            {
                m_sprGetGold[7].set_display_image(null);
            }

            nSingleNumber = (int)(nNumber % 10);
            ostr.str("");
            ostr = ostr + "buy_bullet_number_" + nSingleNumber;
            m_sprGetGold[8].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));

            m_nGetGoldCount++;

            if (nNumber == 0)
            {
                m_sprGetGold[8].set_display_image(null);
            }
        }

    }
}
