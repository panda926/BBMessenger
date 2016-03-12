using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    public class CAccountLayer : Layer, Button_Listener
    {
        private Font m_Font;

        private Button_Widget m_btnAccountAndExit;
        private Button_Widget m_btnAccount;
        private Button_Widget m_btnClose;

        private Sprite m_sprBackground;
        private Image[] m_imgNumber = Arrays.InitializeWithDefaultInstances<Image>(10);
        private Image[] m_imgTimer = Arrays.InitializeWithDefaultInstances<Image>(10);

        private long m_dwStartTime;

        public int[] m_CaptureFishs;


        public CAccountLayer()
        {
            this.m_CaptureFishs = new int[15];
            m_Font = Root.instance().font_manager().create_font_ttf("simsun", "fish\\simsun.ttc");
            m_Font.set_size(new Size(14, 15));

            /*m_btnMatchAndExit = new Button_Widget(Root::instance()->imageset_manager()->imageset("account")->image("btn_match_and_exit_image"),
                Root::instance()->imageset_manager()->imageset("account")->image("btn_match_and_exit_over"),
                Root::instance()->imageset_manager()->imageset("account")->image("btn_match_and_exit_down"),
                Root::instance()->imageset_manager()->imageset("account")->image("btn_match_and_exit_disable"));
            m_btnMatchAndExit->set_tag(10200);
            m_btnMatchAndExit->set_position(new Point(514,560));
            m_btnMatchAndExit->set_content_size(new Size(158,57));
            m_btnMatchAndExit->add_listener(this);
	
            m_btnMatch = new Button_Widget(Root::instance()->imageset_manager()->imageset("account")->image("btn_match_image"),
                Root::instance()->imageset_manager()->imageset("account")->image("btn_match_over"),
                Root::instance()->imageset_manager()->imageset("account")->image("btn_match_down"),
                Root::instance()->imageset_manager()->imageset("account")->image("btn_match_disable"));
            m_btnMatch->set_tag(10201);
            m_btnMatch->set_position(new Point(514,560));
            m_btnMatch->set_content_size(new Size(158,57));
            m_btnMatch->add_listener(this);*/

            m_btnAccountAndExit = new Button_Widget(Root.instance().imageset_manager().imageset("account").image("btn_account_and_exit_image"), Root.instance().imageset_manager().imageset("account").image("btn_account_and_exit_over"), Root.instance().imageset_manager().imageset("account").image("btn_account_and_exit_down"), Root.instance().imageset_manager().imageset("account").image("btn_account_and_exit_disable"));
            m_btnAccountAndExit.set_tag(10200);
            m_btnAccountAndExit.set_position(new Point(360, 523));
            m_btnAccountAndExit.set_content_size(new Size(158, 57));
            m_btnAccountAndExit.add_listener(this);

            m_btnAccount = new Button_Widget(Root.instance().imageset_manager().imageset("account").image("btn_account_image"), Root.instance().imageset_manager().imageset("account").image("btn_account_over"), Root.instance().imageset_manager().imageset("account").image("btn_account_down"), Root.instance().imageset_manager().imageset("account").image("btn_account_disable"));
            m_btnAccount.set_tag(10201);
            m_btnAccount.set_position(new Point(540, 523));
            m_btnAccount.set_content_size(new Size(158, 57));
            m_btnAccount.add_listener(this);

            m_btnClose = new Button_Widget(Root.instance().imageset_manager().imageset("account").image("btn_close_image"), Root.instance().imageset_manager().imageset("account").image("btn_close_over"), Root.instance().imageset_manager().imageset("account").image("btn_close_down"), Root.instance().imageset_manager().imageset("account").image("btn_close_disable"));
            m_btnClose.set_tag(10202);
            m_btnClose.set_position(new Point(720, 523));
            m_btnClose.set_content_size(new Size(158, 57));
            m_btnClose.add_listener(this);


            m_sprBackground = new Sprite(Root.instance().imageset_manager().imageset("account").image("bg"));
            m_sprBackground.set_hot(new Point(0, 0));
            add_child(m_sprBackground);

            for (int i = 0; i < m_CaptureFishs.Length; i++)
            {
                m_CaptureFishs[i] = 0;
            }

            ostringstream ostr = new ostringstream();
            for (int i = 0; i < 10; i++)
            {
                ostr.str("");
                ostr = ostr + "number_" + i;
                m_imgNumber[i] = Root.instance().imageset_manager().imageset("role").image(ostr.str());
            }

            for (int i = 0; i < 10; i++)
            {
                ostr.str("");
                ostr = ostr + "time_" + i;
                m_imgTimer[i] = Root.instance().imageset_manager().imageset("account").image(ostr.str());
            }


        }
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void update(float dt);

        public override void draw()
        {
            if (!visible_)
            {
                return;
            }

            base.draw();

            CGameScene pGameScene = (CGameScene)parent();
            CClientKernel pClientKernel = pGameScene.GetClientKernel();
            if (pClientKernel == null)
                return;

            Point pt = new Point(position_absolute());
            Point ptDraw = new Point();
            ostringstream ostr = new ostringstream();

            //for (int i=0; i<4; i++)
            //{
            //    ostr.str("");
            //    ostr = ostr + m_CaptureFishs[i];

            //    ptDraw.x_ = pt.x_ + 136+i*180;
            //    ptDraw.y_ = pt.y_ +104;

            //    DrawNumber(ostr.str(), ptDraw);
            //}

            //for (int i=0; i<4; i++)
            //{
            //    ostr.str("");
            //    ostr = ostr + m_CaptureFishs[i+4];

            //    ptDraw.x_ = pt.x_ + 136+i*180;
            //    ptDraw.y_ = pt.y_ +104+74;

            //    DrawNumber(ostr.str(), ptDraw);
            //}

            //for (int i=0; i<4; i++)
            //{
            //    ostr.str("");
            //    ostr = ostr + m_CaptureFishs[i+8];

            //    ptDraw.x_ = pt.x_ + 136+i*180;
            //    ptDraw.y_ = pt.y_ +104+148;

            //    DrawNumber(ostr.str(), ptDraw);
            //}


            int wMeChairID = pGameScene.GetMeChairID();

            if (wMeChairID != GameDefine.INVALID_CHAIR)
            {
                int nFishGold = pGameScene.m_layRoles[wMeChairID].GetFishGold();
                ostr.str("");
                ostr = ostr + nFishGold;
                ptDraw.x_ = pt.x_ + 296;
                ptDraw.y_ = pt.y_ + 310;
                DrawNumber(ostr.str(), ptDraw);

                int lCellScore = pGameScene.m_layBuyBulletLayer.GetCellScore();

                ostr.str("");
                ostr = ostr + nFishGold * lCellScore;
                ptDraw.x_ = pt.x_ + 500;
                ptDraw.y_ = pt.y_ + 310;
                DrawNumber(ostr.str(), ptDraw);



                UserInfo pUserData = pClientKernel.GetMeUserInfo();

                if (pUserData != null)
                {
                    ostr.str("");
                    ostr = ostr + 0;
                    ptDraw.x_ = pt.x_ + 240;
                    ptDraw.y_ = pt.y_ + 352;
                    DrawNumber(ostr.str(), ptDraw);

                    ostr.str("");
                    ostr = ostr + pUserData.GetGameMoney();
                    ptDraw.x_ = pt.x_ + 580;
                    ptDraw.y_ = pt.y_ + 352;
                    DrawNumber(ostr.str(), ptDraw);
                }
            }

            long dwTime = FishDefine.time() - m_dwStartTime;

            if (dwTime >= 20 )
            {
                ShowWidnow(false);
            }
            else
            {
                ostr.str("");
                ostr = ostr + (20 - dwTime).ToString();

                ptDraw.x_ = pt.x_ + 696;
                ptDraw.y_ = pt.y_ + 38;

                DrawTimer(ostr.str(), ptDraw);
            }
        }

        public void add_widget(Node node)
        {
            node.add_child(this);

            node.add_child(m_btnAccountAndExit);
            node.add_child(m_btnAccount);
            node.add_child(m_btnClose);
        }
        public void DrawNumber(string number, Point pt)
        {
            ostringstream ostr = new ostringstream();
            Point ptDraw = new Point();
            ptDraw = pt - new Point(number.Length / 2.0 * 24.0, 0);

            for (int i = 0; i < number.Length; i++)
            {
                int nIndex = number[i] - '0';
                m_imgNumber[nIndex].draw(ptDraw);

                ptDraw.x_ += 24;
            }

        }
        public void DrawTimer(string number, Point pt)
        {
            ostringstream ostr = new ostringstream();
            Point ptDraw = new Point();
            ptDraw = pt - new Point(number.Length / 2.0 * 24.0, 0);

            for (int i = 0; i < number.Length; i++)
            {
                int nIndex = number[i] - '0';
                m_imgTimer[nIndex].draw(ptDraw);

                ptDraw.x_ += 24;
            }

        }

        public void ShowWidnow(bool bShow)
        {
            set_visible(bShow);



            m_btnAccountAndExit.set_visible(bShow);
            m_btnAccount.set_visible(bShow);




            m_btnClose.set_visible(bShow);

            if (bShow == true)
            {
                m_dwStartTime = FishDefine.time();
            }
        }
        public void DisableWindow(bool bDisable)
        {
            set_disable(bDisable);
            m_btnAccountAndExit.set_disable(bDisable);
            m_btnAccount.set_disable(bDisable);
            m_btnClose.set_disable(bDisable);
        }
        public void button_press(int tag)
	{
		CGameScene pGameScene = (CGameScene)parent();
    
		if (tag == 10200)
		{
			pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_END_GAME, null); //123
			try
			{
				pGameScene.window_closed(null);
				Root.instance().queue_end_rendering();
			}
			catch
			{
				//pakcj global::exit(0);
			}
		}
		else if (tag == 10201)
		{
    
				CMD_C_Account Account = new CMD_C_Account();
				Account.wChairID = pGameScene.GetMeChairID();
    
				pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_ACCOUNT, Account);
    
				DisableWindow(true);
    
    
		}
		else if (tag == 10202)
		{
			ShowWidnow(false);
		}
	}


    }
}
