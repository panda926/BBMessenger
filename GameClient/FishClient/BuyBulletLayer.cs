using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    //C++ TO C# CONVERTER TODO TASK: Multiple inheritance is not available in C#:
    public class CBuyBulletLayer : Layer, Button_Listener
    {
        /*Font *m_Font;*/

        private Button_Widget m_btnCancle;
        private Button_Widget m_btnMax;
        private Button_Widget m_btnOk;
        private Button_Widget m_btnMinus;
        private Button_Widget m_btnAdd;

        private Slider_Widget m_sldNumber;

        private Sprite m_sprBackground;
        private Sprite[] m_sprFishGold;
        private Sprite[] m_sprGameGold;

        private int m_nMaxBuyFishGold;
        private int m_nMaxFishGold;
        private int m_nFishGold;
        private int m_nGameGold;

        private bool m_bDragging;
        private Point m_ptDragMouse = new Point();

        private int m_lCellScore;
        private bool m_bSendMessage;


        public CBuyBulletLayer()
        {
            this.m_sprFishGold = new Sprite[9];
            this.m_sprGameGold = new Sprite[9];
            this.m_nFishGold = 0;
            this.m_nGameGold = 0;
            this.m_bDragging = false;
            this.m_lCellScore = 1;
            this.m_bSendMessage = false;
            //m_Font = Root::instance()->font_manager()->create_font_ttf("simsun", "fish\\simsun.ttc");
            //m_Font->set_size(new Size(14,15));

            m_btnCancle = new Button_Widget(Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_cancle_image"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_cancle_over"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_cancle_down"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_cancle_disable"));
            m_btnCancle.set_tag(10010);
            m_btnCancle.set_position(new Point(388 + 56 + 32, 257 + 160));
            m_btnCancle.set_content_size(new Size(104, 52));
            m_btnCancle.add_listener(this);

            m_btnMax = new Button_Widget(Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_max_image"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_max_over"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_max_down"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_max_disable"));
            m_btnMax.set_tag(10011);
            m_btnMax.set_position(new Point(388 + 350 + 32, 257 + 160));
            m_btnMax.set_content_size(new Size(104, 52));
            m_btnMax.add_listener(this);

            m_btnOk = new Button_Widget(Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_ok_image"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_ok_over"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_ok_down"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_ok_disable"));
            m_btnOk.set_tag(10012);
            m_btnOk.set_position(new Point(388 + 180 + 32, 257 + 160));
            m_btnOk.set_content_size(new Size(148, 52));
            m_btnOk.add_listener(this);

            m_btnMinus = new Button_Widget(Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_minus_image"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_minus_over"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_minus_down"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_minus_disable"));
            m_btnMinus.set_tag(10013);
            m_btnMinus.set_position(new Point(388 + 20 + 32, 257 + 110));
            m_btnMinus.set_content_size(new Size(42, 42));
            m_btnMinus.add_listener(this);

            m_btnAdd = new Button_Widget(Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_add_image"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_add_over"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_add_down"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_add_disable"));
            m_btnAdd.set_tag(10014);
            m_btnAdd.set_position(new Point(388 + 442 + 32, 257 + 110));
            m_btnAdd.set_content_size(new Size(42, 42));
            m_btnAdd.add_listener(this);

            m_sldNumber = new Slider_Widget(null, Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_slider_thumb"), Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_slider_image"));
            m_sldNumber.set_tag(10015);
            m_sldNumber.set_position(new Point(388 + 70 + 32, 257 + 110));
            m_sldNumber.set_content_size(new Size(370, 45));
            m_sldNumber.add_listener(slider_value);

            m_sprBackground = new Sprite(Root.instance().imageset_manager().imageset("ui_game").image("buy_bullet_bg"));
            m_sprBackground.set_hot(new Point(0, 0));
            add_child(m_sprBackground);

            for (int i = 0; i < 9; i++)
            {
                m_sprFishGold[i] = new Sprite();
                add_child(m_sprFishGold[i]);
            }

            for (int i = 0; i < 9; i++)
            {
                m_sprGameGold[i] = new Sprite();
                add_child(m_sprGameGold[i]);
            }

            for (int i = 0; i < 9; i++)
            {
                m_sprFishGold[i].set_position(new Point(98 + i * 16, 76));
                m_sprGameGold[i].set_position(new Point(356 + i * 16, 76));
            }

            SetFishGold(100);
            SetGameGold(100 * m_lCellScore);

            m_nMaxFishGold = 10;
            m_nMaxBuyFishGold = 10;

        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        public void button_press(int tag)
        {
            int nMaxFishGold = m_nMaxBuyFishGold;

            int nSetp = nMaxFishGold / 100;
            if (10013 == tag)
            {
                m_nFishGold -= nSetp;
                if (m_nFishGold < nSetp)
                {
                    m_nFishGold = nSetp;
                }

                SetFishGold(m_nFishGold);
                SetGameGold(m_nFishGold * m_lCellScore);
                double dSetp = (nSetp == 0) ? 0.00 : (double)(m_nFishGold - nSetp) / (nSetp * 99);
                m_sldNumber.set_value(dSetp);

            }
            else if (10014 == tag)
            {
                m_nFishGold += nSetp;
                if (m_nFishGold > nMaxFishGold)
                {
                    m_nFishGold = nMaxFishGold;


                }

                SetFishGold(m_nFishGold);
                SetGameGold(m_nFishGold * m_lCellScore);
                double dSetp = (nSetp == 0) ? 0.00 : (double)(m_nFishGold - nSetp) / (nSetp * 99);
                m_sldNumber.set_value(dSetp);
            }
            else if (10011 == tag)
            {
                m_nFishGold = nMaxFishGold;

                SetFishGold(m_nFishGold);
                SetGameGold(m_nFishGold * m_lCellScore);
                double dSetp = (nSetp == 0) ? 0.00 : (double)(m_nFishGold - nSetp) / (nSetp * 99);
                m_sldNumber.set_value(dSetp);
            }
            else if (10010 == tag)
            {
                ShowWidnow(false);
            }
            else if (10012 == tag)
            {
                CMD_C_Buy_Bullet BuyBullet = new CMD_C_Buy_Bullet();
                BuyBullet.dwCount = (int)m_nFishGold;

                CGameScene pGameScene = (CGameScene)parent();
                pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_BUY_BULLET, BuyBullet);
                m_bSendMessage = true;
                ShowWidnow(false);
            }
        }

        public void slider_value(int tag, double value)
        {
            int nMaxFishGold = m_nMaxBuyFishGold;

            int nStep = nMaxFishGold / 100;

            for (int i = 0; i < 100; i++)
            {
                if (value >= i * 0.01 && value < i * 0.01 + 0.01)
                {
                    m_nFishGold = (i == 99) ? nMaxFishGold : nStep * i;
                }
            }

            SetFishGold(m_nFishGold);
            SetGameGold(m_nFishGold * m_lCellScore);
        }


        public void add_widget(Node node)
        {
            node.add_child(this);
            node.add_child(m_btnCancle);
            node.add_child(m_btnMax);
            node.add_child(m_btnOk);
            node.add_child(m_btnMinus);
            node.add_child(m_btnAdd);
            node.add_child(m_sldNumber);
        }
        public void ShowWidnow(bool bIsShow)
        {
            CGameScene pGameScene = (CGameScene)parent();

            bool bShow = (pGameScene.m_dwRoomType == 0) ? bIsShow : false;

            bool bVisible = visible();
            if (bVisible == bShow)
                return;

            set_visible(bShow);
            m_btnCancle.set_visible(bShow);
            m_btnMax.set_visible(bShow);
            m_btnOk.set_visible(bShow);
            m_btnMinus.set_visible(bShow);
            m_btnAdd.set_visible(bShow);
            m_sldNumber.set_visible(bShow);

            if (bShow)
            {
                try
                {
                    Sound_Instance pSound = Root.instance().sound_manager().sound_instance(16);
                    pSound.play(false, true);
                }
                catch
                {
                }

            }

            if (bIsShow == true)
            {
                CClientKernel pClientKernel = pGameScene.GetClientKernel();
                if (pClientKernel == null)
                    return;

                UserInfo pUserData = pClientKernel.GetMeUserInfo();
                if ((pUserData != null) && (pUserData.GetGameMoney() > 100))
                {
                    int nMeBuyCount = (pUserData.GetGameMoney() - 100) / m_lCellScore;
                    m_nMaxBuyFishGold = (m_nMaxFishGold <= nMeBuyCount) ? m_nMaxFishGold : nMeBuyCount;
                }
                else
                {
                    m_nMaxBuyFishGold = 0;
                }

                button_press(10011);
            }
        }

        public void SetFishGold(int nFishGold)
        {
            //if (nFishGold == m_nFishGold)
            //     return;

            m_nFishGold = nFishGold;

            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();
            nSingleNumber = (int)(m_nFishGold % 1000000000 / 100000000);
            if (nSingleNumber > 0)
            {
                bGotHead = true;
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprFishGold[0].set_display_image(null);
            }

            nSingleNumber = (int)(m_nFishGold % 100000000 / 10000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprFishGold[1].set_display_image(null);
            }

            nSingleNumber = (int)(m_nFishGold % 10000000 / 1000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[2].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprFishGold[2].set_display_image(null);
            }

            nSingleNumber = (int)(m_nFishGold % 1000000 / 100000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[3].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprFishGold[3].set_display_image(null);
            }

            nSingleNumber = (int)(m_nFishGold % 100000 / 10000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[4].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprFishGold[4].set_display_image(null);
            }

            nSingleNumber = (int)(m_nFishGold % 10000 / 1000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[5].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprFishGold[5].set_display_image(null);
            }

            nSingleNumber = (int)(m_nFishGold % 1000 / 100);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[6].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprFishGold[6].set_display_image(null);
            }

            nSingleNumber = (int)(m_nFishGold % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprFishGold[7].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprFishGold[7].set_display_image(null);
            }

            nSingleNumber = (int)(m_nFishGold % 10);
            ostr.str("");
            ostr = ostr + "buy_bullet_number_" + nSingleNumber;
            m_sprFishGold[8].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
        }

        public void SetGameGold(int nGameGold)
        {
            //if (nGameGold == m_nGameGold)
            //    return;

            m_nGameGold = nGameGold;

            bool bGotHead = false;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();

            nSingleNumber = (int)(m_nGameGold % 1000000000 / 100000000);
            if (nSingleNumber > 0)
            {
                bGotHead = true;
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGameGold[0].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprGameGold[0].set_display_image(null);
            }

            nSingleNumber = (int)(m_nGameGold % 100000000 / 10000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGameGold[1].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprGameGold[1].set_display_image(null);
            }


            nSingleNumber = (int)(m_nGameGold % 10000000 / 1000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGameGold[2].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprGameGold[2].set_display_image(null);
            }


            nSingleNumber = (int)(m_nGameGold % 1000000 / 100000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGameGold[3].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprGameGold[3].set_display_image(null);
            }

            nSingleNumber = (int)(m_nGameGold % 100000 / 10000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGameGold[4].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprGameGold[4].set_display_image(null);
            }

            nSingleNumber = (int)(m_nGameGold % 10000 / 1000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGameGold[5].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprGameGold[5].set_display_image(null);
            }

            nSingleNumber = (int)(m_nGameGold % 1000 / 100);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGameGold[6].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprGameGold[6].set_display_image(null);
            }

            nSingleNumber = (int)(m_nGameGold % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "buy_bullet_number_" + nSingleNumber;
                m_sprGameGold[7].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
            }
            else
            {
                m_sprGameGold[7].set_display_image(null);
            }

            nSingleNumber = (int)(m_nGameGold % 10 / 1);
            ostr.str("");
            ostr = ostr + "buy_bullet_number_" + nSingleNumber;
            m_sprGameGold[8].set_display_image(Root.instance().imageset_manager().imageset("ui_game").image(ostr.str()));
        }


        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void SetCellScore(int lCellScore);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void SetMaxFishGold(int nMaxFishGold);

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int GetCellScore() const
        public int GetCellScore()
        {
            return m_lCellScore;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool IsSendBuyBulletMessage() const
        public bool IsSendBuyBulletMessage()
        {
            return m_bSendMessage;
        }
        public void SetSendBuyBulletMessage(bool bSend)
        {
            m_bSendMessage = bSend;
        }

        public void mouse_down(Point pt, int num, int count)
        {
            m_bDragging = true;
            m_ptDragMouse = pt;

        }
        public void mouse_drag(Point pt)
        {
            if (m_bDragging)
            {
                Point ptNew = position_ + pt - m_ptDragMouse;
                move(ptNew);
            }
        }
        public void mouse_up(Point pt, int num, int count)
        {
            if (m_bDragging)
            {
                m_bDragging = false;
            }
        }
        public void resize(Point pt, Size size)
        {
            base.resize(pt, size);

            set_position(pt);
            m_btnCancle.set_position(new Point(pt.x_ + 56, pt.y_ + 160));
            m_btnMax.set_position(new Point(pt.x_ + 350, pt.y_ + 160));
            m_btnOk.set_position(new Point(pt.x_ + 180, pt.y_ + 160));
            m_btnMinus.set_position(new Point(pt.x_ + 20, pt.y_ + 110));
            m_btnAdd.set_position(new Point(pt.x_ + 442, pt.y_ + 110));
            m_sldNumber.set_position(new Point(pt.x_ + 70, pt.y_ + 110));

        }
        public void SetCellScore(int lCellScore)
        {
            m_lCellScore = lCellScore;

            SetFishGold(m_nFishGold);
            SetGameGold(m_nFishGold * m_lCellScore);
        }

        public void SetMaxFishGold(int nMaxFishGold)
        {
            m_nMaxFishGold = nMaxFishGold;

            CGameScene pGameScene = (CGameScene)parent();
            CClientKernel pClientKernel = pGameScene.GetClientKernel();
            if (pClientKernel == null)
                return;

            UserInfo pUserData = pClientKernel.GetMeUserInfo();
            if ((pUserData != null) && (pUserData.GetGameMoney() > 100))
            {
                int nMeBuyCount = (pUserData.GetGameMoney() - 100) / m_lCellScore;
                m_nMaxBuyFishGold = (m_nMaxFishGold <= nMeBuyCount) ? m_nMaxFishGold : nMeBuyCount;
            }
            else
            {
                m_nMaxBuyFishGold = 0;
            }
        }


    }
}
