using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;

namespace FishClient
{
    public class CMatchTimeLayer : Layer
    {
        private Font m_Font;

        private Sprite m_sprBackground;

        private Sprite[] m_sprTimeHour;
        private Sprite[] m_sprTimeMinute;
        private Sprite[] m_sprTimeSecond;

        public CMatchTimeLayer()
        {
            this.m_sprTimeHour = new Sprite[2];
            this.m_sprTimeMinute = new Sprite[2];
            this.m_sprTimeSecond = new Sprite[2];
            m_Font = Root.instance().font_manager().create_font_ttf("宋体", "fish\\simsun.ttc");
            m_Font.set_size(new Size(20, 22));

            m_sprBackground = new Sprite(Root.instance().imageset_manager().imageset("ui_game").image("match_time_back"));
            m_sprBackground.set_hot(new Point(0, 0));
            add_child(m_sprBackground);

            for (int i = 0; i < 2; i++)
            {
                m_sprTimeHour[i] = new Sprite();
                add_child(m_sprTimeHour[i]);

                m_sprTimeMinute[i] = new Sprite();
                add_child(m_sprTimeMinute[i]);

                m_sprTimeSecond[i] = new Sprite();
                add_child(m_sprTimeSecond[i]);
            }

        }

        public override void draw()
        {
            base.draw();

            CGameScene pGameScene = (CGameScene)parent();
            CClientKernel pClientKernel = pGameScene.GetClientKernel();
            if (pClientKernel == null)
                return;

            if (visible())
            {
                SetTimeHour(pGameScene.m_nMatchHour);
                SetTimeMinute(pGameScene.m_nMatchMinute);
                SetTimeSecond(pGameScene.m_nMatchSecond);

                Point pt = position_absolute();
                for (int i = 0; i < 2; i++)
                {
                    m_sprTimeHour[i].set_position( new Point(pt.x_ + 38 + i * 12, pt.y_ + 12));
                    m_sprTimeMinute[i].set_position( new Point(pt.x_ + 88 + i * 12, pt.y_ + 12));
                    m_sprTimeSecond[i].set_position( new Point(pt.x_ + 124 + i * 12, pt.y_ + 12));
                }

            }
            else
            {
                SetTimeHour(0);
                SetTimeMinute(0);
                SetTimeSecond(0);
            }

            //if(visible())
            //{
            //	char data[10];
            //	Point pt = position_absolute();

            //	std::ostringstream ostrRank;

            //	pt += Point(50,57);
            //	ostrRank + pGameScene->m_nMatchHour;
            //	m_Font->draw_string(pt, ostrRank.str().c_str(), Color(241,32,0));

            //	pt += Point(42,0);
            //	sprintf(data,"%02d",pGameScene->m_nMatchMinute);
            //	m_Font->draw_string(pt, data, Color(241,32,0));

            //	pt += Point(36,0);
            //	sprintf(data,"%02d",pGameScene->m_nMatchSecond);
            //	m_Font->draw_string(pt, data, Color(241,32,0));
            //}
        }

        public void add_widget(Node node)
        {
            node.add_child(this);
        }

        public void resize(Point pt, Size size)
        {
            base.resize(pt, size);

            set_position(pt);
        }
        public void Show(bool bShow)
        {
            bool bVisible = visible();
            if (bVisible == bShow)
                return;

            set_visible(bShow);
            m_sprBackground.set_visible(bShow);
        }
        public void SetTimeHour(int nNumber)
        {
            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(nNumber % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "match_number_" + nSingleNumber;
                m_sprTimeHour[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                nSingleNumber = 0;
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "match_number_" + nSingleNumber;
                m_sprTimeHour[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }

            nSingleNumber = (int)(nNumber % 10);
            ostr.str("");
            ostr = ostr + "match_number_" + nSingleNumber;
            m_sprTimeHour[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));

            //if(nNumber==0)
            //{
            //	m_sprTimeHour[1]->set_display_image(0);
            //}
        }
        public void SetTimeMinute(int nNumber)
        {
            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(nNumber % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "match_number_" + nSingleNumber;
                m_sprTimeMinute[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                nSingleNumber = 0;
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "match_number_" + nSingleNumber;
                m_sprTimeMinute[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }

            nSingleNumber = (int)(nNumber % 10);
            ostr.str("");
            ostr = ostr + "match_number_" + nSingleNumber;
            m_sprTimeMinute[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));

            //if(nNumber==0)
            //{
            //	m_sprTimeMinute[1]->set_display_image(0);
            //}
        }
        public void SetTimeSecond(int nNumber)
        {
            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(nNumber % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "match_number_" + nSingleNumber;
                m_sprTimeSecond[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                nSingleNumber = 0;
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "match_number_" + nSingleNumber;
                m_sprTimeSecond[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }

            nSingleNumber = (int)(nNumber % 10);
            ostr.str("");
            ostr = ostr + "match_number_" + nSingleNumber;
            m_sprTimeSecond[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));

            //if(nNumber==0)
            //{
            //	m_sprTimeSecond[1]->set_display_image(0);
            //}
        }


    }
}
