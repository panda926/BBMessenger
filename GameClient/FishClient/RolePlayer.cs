using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{


    public class FishGoldShow
    {
        public int bEnable; //是否显示
        public Point ptBase = new Point(); //显示位置
        public Point ptBaseOld = new Point(); //原显示位置
        public Point ptChange = new Point(); //移动位置
        public int dwFishGold; //显示获得金币数
        public int dwFishScore; //显示金柱数目
        public int cbFishScoreCount; //显示步骤
        public byte cbGoldTurnCount; //金币翻转步骤
        public int wTime; //显示时间
        public int wTimeChange; //翻转间隔时间
        public int nIndex;
        public int nXPosMove; //平移
        public int nXPosMoveCount; //平移次数
        public int nShowCount;
    }

    public class CRoleLayer : Layer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	CRoleLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();


        private Sprite m_sprGunBase;
        private Sprite m_sprGunOver;
        private Sprite m_sprGunWing1;
        private Sprite m_sprGunWing2;

        private Sprite m_sprGun;
        private Sprite m_sprGunLight;

        private Sprite m_sprShotNumBack;
        private Sprite m_sprNet;

        private Sprite m_sprShotGold;
        private Sprite[] m_sprGold;

        private Sprite[] m_sprMulRate;

        private Sprite[] m_sprShow;
        private Sprite[] m_spValue;

        private Sprite m_sprCannon;

        private Sprite m_sprUserInfo;

        private Sprite m_sprWaringBullet;
        private Sprite m_sprWaringTime;

        private Sprite m_sprGunLead;
        private Sprite m_sprBuyLead;
        private Sprite m_sprPlayLead;
        private Sprite m_sprLeftKey;
        private Sprite m_sprRighKey;

        private Sprite m_sprCumBar;
        private Sprite m_sprShowBar;

        private Font m_Font;
        private Font m_FontMessage;

        private long m_dwStartTime;
        private Image[] m_imgTimer = Arrays.InitializeWithDefaultInstances<Image>(10);
        private Image[] m_imgTaskNumber = Arrays.InitializeWithDefaultInstances<Image>(10);
        private Image[] m_imgCookTime = Arrays.InitializeWithDefaultInstances<Image>(11);
        private Image[] m_imgCookFishNumber = Arrays.InitializeWithDefaultInstances<Image>(11);
        private Image[] m_imgCookGold = Arrays.InitializeWithDefaultInstances<Image>(10);

        private Sprite[] m_sprBar = Arrays.InitializeWithDefaultInstances<Sprite>(3);
        private Sprite[] m_sprBarBlank = Arrays.InitializeWithDefaultInstances<Sprite>(3);
        private Sprite m_sprBarEnough;
        private Sprite m_sprRealFly;
        private Sprite m_sprReelBack;
        private Sprite[] m_sprReelNormal = Arrays.InitializeWithDefaultInstances<Sprite>(3);
        private Sprite[] m_sprReelFast = Arrays.InitializeWithDefaultInstances<Sprite>(3);
        private Sprite m_sprReelEff;

        private Sprite[] m_sprTaskName = Arrays.InitializeWithDefaultInstances<Sprite>(4);
        private Sprite m_sprChangeEffect;

        private Sprite m_sprLaserBeanCannon;
        private Sprite m_sprLaserBean;

        private Sprite m_sprBombCannon;
        private Sprite m_sprBomb;
        private Sprite m_sprBombFuse;
        private Sprite m_sprExplorer;

        private Sprite m_sprReward;

        private Sprite m_sprCookBack;
        private Sprite m_sprCookNormal;
        private Sprite m_sprCookLost;
        private Sprite m_sprCookWin;
        private Sprite m_sprCookSay;
        private Sprite m_sprCookFish;
        private Sprite m_sprCookSayBack;
        private Sprite m_sprCookComplete;
        private Sprite m_sprCookGold;

        private Sprite m_sprConnonLevelUp;

        private int m_wChairID;
        private int m_dwFishGold;
        private int m_nFireCount;
        private bool m_bDelayBarEnough;
        private bool m_bShowRewardNubmer;
        private int m_nRewardNumber;
        private int m_nCookFish;
        private long m_dwCookStartTime;

        private FishDefine.enCannonType m_CannonType;
        private int m_dwMulRate; //炮弹倍率
        private int m_dwMaxMulRate; //最大炮弹倍率
        private int m_dwLevel; //级别

        private int m_dwIndexShow;

        public CTaskDate m_TaskDate = new CTaskDate();
        public FishGoldShow[] m_FishGoldShow = Arrays.InitializeWithDefaultInstances<FishGoldShow>(FishDefine.MAX_SHOW_TIME_COUNT + 1);
        public int m_nIndex;

        public string m_cbShowData;
        public Sprite m_sprMessage;

        public int m_dwMatchScore; //比赛成绩
        public int m_dwMatchIndex; //比赛名次

        public CRoleLayer()
        {
            this.m_wChairID = GameDefine.INVALID_CHAIR;
            this.m_sprGold = new Sprite[9];
            this.m_sprMulRate = new Sprite[5];
            this.m_sprShow = new Sprite[FishDefine.MAX_SHOW_TIME_COUNT * FishDefine.MAX_SHOW_GOLDEN_COUNT];
            this.m_spValue = new Sprite[FishDefine.MAX_SHOW_TIME_COUNT * 8];
            m_wChairID = GameDefine.INVALID_CHAIR;
            m_dwFishGold = 0;
            m_nFireCount = 0;
            m_bDelayBarEnough = false;
            m_CannonType = FishDefine.enCannonType.CannonTypeCount;
            m_dwMulRate = 0;
            m_dwMaxMulRate = 0;
            m_bShowRewardNubmer = false;
            m_nRewardNumber = 0;
            m_nCookFish = 0;
            m_dwCookStartTime = 0;

            m_sprLaserBean = new Sprite(Root.instance().imageset_manager().imageset("task").image("laserUp"));
            add_child(m_sprLaserBean);
            m_sprLaserBean.set_visible(false);

            m_sprBombFuse = new Sprite();
            add_child(m_sprBombFuse);
            m_sprBombFuse.set_scale(new Size(1.0, 1.9));
            m_sprBombFuse.set_visible(false);

            m_sprGunBase = new Sprite(Root.instance().imageset_manager().imageset("role").image("gun_base"));
            m_sprGunOver = new Sprite(Root.instance().imageset_manager().imageset("role").image("gun_over"));
            m_sprGunWing1 = new Sprite(Root.instance().imageset_manager().imageset("role").image("gun_wing1"));
            m_sprGunWing2 = new Sprite(Root.instance().imageset_manager().imageset("role").image("gun_wing2"));

            m_sprGun = new Sprite();
            m_sprGunLight = new Sprite();

            m_sprShotNumBack = new Sprite(Root.instance().imageset_manager().imageset("role").image("shot_num_back"));
            m_sprNet = new Sprite(Root.instance().imageset_manager().imageset("role").image("net_0"));
            m_sprShotGold = new Sprite(Root.instance().imageset_manager().imageset("role").image("gold_0"));

            add_child(m_sprGunWing1);
            add_child(m_sprGunWing2);
            add_child(m_sprGunBase);

            add_child(m_sprGunLight);
            add_child(m_sprGun);

            add_child(m_sprGunOver);

            add_child(m_sprShotNumBack);
            add_child(m_sprNet);
            add_child(m_sprShotGold);

            m_sprCannon = new Sprite();
            add_child(m_sprCannon);

            for (int i = 0; i < 9; i++)
            {
                m_sprGold[i] = new Sprite();
                add_child(m_sprGold[i]);

            }

            for (int i = 0; i < 5; i++)
            {
                m_sprMulRate[i] = new Sprite();
                add_child(m_sprMulRate[i]);
            }

            m_sprUserInfo = new Sprite(Root.instance().imageset_manager().imageset("role").image("btn_other_info_image"));
            add_child(m_sprUserInfo);
            m_sprUserInfo.set_visible(false);

            m_sprWaringBullet = new Sprite(Root.instance().imageset_manager().imageset("role").image("warning_bullet"));
            add_child(m_sprWaringBullet);
            m_sprWaringBullet.set_visible(false);


            m_Font = Root.instance().font_manager().create_font_ttf("simsun", "fish\\simsun.ttc");
            m_Font.set_size(new Size(15, 16));
            m_FontMessage = Root.instance().font_manager().create_font_ttf("宋体", "fish\\simsun.ttc");
            m_FontMessage.set_size(new Size(15, 16));
            m_FontMessage.set_color(new Color(23, 213, 57));

            m_sprGunLead = new Sprite(Root.instance().imageset_manager().imageset("ui_game").image("gun_lead"));
            m_sprBuyLead = new Sprite(Root.instance().imageset_manager().imageset("ui_game").image("buy_lead"));
            m_sprPlayLead = new Sprite(Root.instance().imageset_manager().imageset("ui_game").image("play_lead"));
            m_sprLeftKey = new Sprite(Root.instance().imageset_manager().imageset("ui_game").image("left_key_0"));
            m_sprRighKey = new Sprite(Root.instance().imageset_manager().imageset("ui_game").image("right_key_0"));
            add_child(m_sprGunLead);
            add_child(m_sprBuyLead);
            add_child(m_sprPlayLead);
            add_child(m_sprLeftKey);
            add_child(m_sprRighKey);

            m_sprGunLead.set_visible(false);
            m_sprBuyLead.set_visible(false);
            m_sprPlayLead.set_visible(false);
            m_sprLeftKey.set_visible(false);
            m_sprRighKey.set_visible(false);

            m_sprWaringTime = new Sprite(Root.instance().imageset_manager().imageset("role").image("warning_time"));
            add_child(m_sprWaringTime);
            m_sprWaringTime.set_visible(false);

            ostringstream ostr = new ostringstream();
            for (int i = 0; i < 10; i++)
            {
                ostr.str("");
                ostr = ostr + "time_" + i;
                m_imgTimer[i] = Root.instance().imageset_manager().imageset("account").image(ostr.str());
            }

            for (int i = 0; i < 10; i++)
            {
                ostr.str("");
                ostr = ostr + "task_number_" + i;
                m_imgTaskNumber[i] = Root.instance().imageset_manager().imageset("task").image(ostr.str());
            }

            for (int i = 0; i < 10; i++)
            {
                ostr.str("");
                ostr = ostr + "number_" + i;
                m_imgCookGold[i] = Root.instance().imageset_manager().imageset("role").image(ostr.str());
            }


            for (int i = 0; i < 11; i++)
            {
                ostr.str("");
                ostr = ostr + "fish_num_" + i;
                m_imgCookFishNumber[i] = Root.instance().imageset_manager().imageset("cook").image(ostr.str());
            }

            for (int i = 0; i < 11; i++)
            {
                ostr.str("");
                ostr = ostr + "time_" + i;
                m_imgCookTime[i] = Root.instance().imageset_manager().imageset("cook").image(ostr.str());
            }

            m_sprLaserBeanCannon = new Sprite(Root.instance().imageset_manager().imageset("laserBean").image("0"));
            add_child(m_sprLaserBeanCannon);
            m_sprLaserBeanCannon.set_visible(false);

            m_sprBombCannon = new Sprite(Root.instance().imageset_manager().imageset("bomb").image("0"));
            add_child(m_sprBombCannon);
            m_sprBombCannon.set_visible(false);

            m_sprBomb = new Sprite();
            add_child(m_sprBomb);
            m_sprBomb.set_visible(false);

            m_sprExplorer = new Sprite();
            add_child(m_sprExplorer);
            m_sprExplorer.set_visible(false);

            m_sprReward = new Sprite(Root.instance().imageset_manager().imageset("get_reward").image("4"));
            add_child(m_sprReward);
            m_sprReward.set_visible(false);

            m_sprCumBar = new Sprite(Root.instance().imageset_manager().imageset("task").image("cum_bar"));
            add_child(m_sprCumBar);

            m_sprShowBar = new Sprite();
            add_child(m_sprShowBar);

            for (int i = 0; i < 3; i++)
            {
                m_sprBarBlank[i] = new Sprite(Root.instance().imageset_manager().imageset("task").image("bar_blank"));
                m_sprBar[i] = new Sprite();
                add_child(m_sprBarBlank[i]);
                add_child(m_sprBar[i]);
            }

            m_sprRealFly = new Sprite();
            m_sprRealFly.set_visible(false);
            add_child(m_sprRealFly);

            m_sprReelBack = new Sprite(Root.instance().imageset_manager().imageset("task").image("reel_back"));
            m_sprReelBack.set_visible(false);
            add_child(m_sprReelBack);

            m_sprReelEff = new Sprite();
            m_sprReelEff.set_visible(false);
            add_child(m_sprReelEff);

            for (int i = 0; i < 3; i++)
            {
                m_sprReelNormal[i] = new Sprite(Root.instance().imageset_manager().imageset("task").image("reel_normal"));
                m_sprReelNormal[i].set_visible(false);
                add_child(m_sprReelNormal[i]);

                m_sprReelFast[i] = new Sprite(Root.instance().imageset_manager().imageset("task").image("reel_fast"));
                m_sprReelFast[i].set_visible(false);
                add_child(m_sprReelFast[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                m_sprTaskName[i] = new Sprite();
                m_sprTaskName[i].set_visible(false);
                add_child(m_sprTaskName[i]);
                m_sprTaskName[i].set_visible(false);
            }

            m_sprChangeEffect = new Sprite();
            add_child(m_sprChangeEffect);
            m_sprChangeEffect.set_visible(false);

            m_sprBarEnough = new Sprite();
            add_child(m_sprBarEnough);
            m_sprChangeEffect.set_visible(false);

            m_sprCookSayBack = new Sprite(Root.instance().imageset_manager().imageset("cook").image("say_back"));
            m_sprCookSayBack.set_visible(false);
            add_child(m_sprCookSayBack);

            m_sprCookSay = new Sprite();
            m_sprCookSay.set_visible(false);
            add_child(m_sprCookSay);

            m_sprCookBack = new Sprite(Root.instance().imageset_manager().imageset("cook").image("cook_back"));
            m_sprCookBack.set_visible(false);
            add_child(m_sprCookBack);

            m_sprCookNormal = new Sprite(Root.instance().imageset_manager().imageset("cook").image("cook_normal_14"));
            m_sprCookNormal.set_visible(false);
            add_child(m_sprCookNormal);

            m_sprCookFish = new Sprite();
            m_sprCookFish.set_visible(false);
            add_child(m_sprCookFish);

            m_sprCookComplete = new Sprite(Root.instance().imageset_manager().imageset("cook").image("quan"));
            m_sprCookComplete.set_visible(false);
            add_child(m_sprCookComplete);

            m_sprCookGold = new Sprite(Root.instance().imageset_manager().imageset("role").image("gold_0"));
            m_sprCookGold.set_visible(false);
            add_child(m_sprCookGold);

            m_sprCookLost = new Sprite();
            m_sprCookLost.set_visible(false);
            add_child(m_sprCookLost);

            m_sprCookWin = new Sprite();
            m_sprCookWin.set_visible(false);
            add_child(m_sprCookWin);

            m_sprConnonLevelUp = new Sprite();
            m_sprConnonLevelUp.set_visible(false);
            add_child(m_sprConnonLevelUp);

            m_sprMessage = new Sprite();
            m_sprMessage.set_visible(false);
            add_child(m_sprMessage);

            m_dwLevel = 0;
            m_dwIndexShow = 0;

            for (int i = 0; i < FishDefine.MAX_SHOW_TIME_COUNT; i++)
            {
                m_FishGoldShow[i].bEnable = 0;
                m_FishGoldShow[i].ptBase = new Point(0, 0);
                m_FishGoldShow[i].ptBaseOld = new Point(0, 0);
                m_FishGoldShow[i].ptChange = new Point(0, 0);
                m_FishGoldShow[i].dwFishGold = 0;
                m_FishGoldShow[i].dwFishScore = 0;
                m_FishGoldShow[i].cbFishScoreCount = 0;
                m_FishGoldShow[i].cbGoldTurnCount = 0;
                m_FishGoldShow[i].wTime = 0;
                m_FishGoldShow[i].nIndex = 0;
                m_FishGoldShow[i].nXPosMove = 0;
                m_FishGoldShow[i].nXPosMoveCount = 0;
                m_FishGoldShow[i].nShowCount = 0;

                for (int j = 0; j < FishDefine.MAX_SHOW_GOLDEN_COUNT; j++)
                {
                    m_sprShow[i * FishDefine.MAX_SHOW_GOLDEN_COUNT + j] = new Sprite();
                    add_child(m_sprShow[i * FishDefine.MAX_SHOW_GOLDEN_COUNT + j]);
                }
                for (int j = 0; j < 8; j++)
                {
                    m_spValue[i * 8 + j] = new Sprite();
                    add_child(m_spValue[i * 8 + j]);
                }
            }

            m_nIndex = 0;

            m_dwMatchScore = 0;
            m_dwMatchIndex = 0;

        }

	    public override void draw()
	    {
		    base.draw();

		    if (m_sprMessage.visible())
		    {
			    Point pt = m_sprMessage.position() + position();
			    pt.x_ -= 120;
			    ostringstream ostr = new ostringstream();
			    ostr = ostr + m_cbShowData;
    
			    m_FontMessage.draw_string(pt, ostr.str(), new Color(15,4,101));
    
		    }
    
		    if (m_sprUserInfo.visible())
		    {
			    CGameScene pGameScene = (CGameScene)parent();
			    CClientKernel pClientKernel = pGameScene.GetClientKernel();
			    if (pClientKernel == null)
				    return;
    
			    UserInfo pUserData = pClientKernel.GetUserInfo(m_wChairID);
    
			    if (pUserData != null)
			    {
				    Point pt = m_sprUserInfo.position_absolute();
				    pt.x_ -= 26;
				    pt.y_ -= 42;
    
    
				    m_Font.draw_string(pt,pUserData.Nickname,new Color(176,222,238));
    
				    pt.y_ += 24;
    
				    ostringstream ostr = new ostringstream();
				    ostr = ostr + pUserData.GetGameMoney();
    
				    m_Font.draw_string(pt, ostr.str(), new Color(176,222,238));
    
				    //std::string szRank;
				    //if (pUserData.lScore>0 && pUserData.lScore<1000)
				    //{
				    //    szRank = "渔夫";
				    //}
				    //else
				    //{
				    //    szRank = "船长";
				    //}
    
				    ostringstream ostrRank = new ostringstream();
				    ostrRank = ostrRank + pGameScene.m_layRoles[m_wChairID].GetExpValue();
    
				    pt.y_ += 24;
				    m_Font.draw_string(pt, ostrRank.str(), new Color(176,222,238));
			    }
    
    
		    }
    
		    if (m_sprWaringTime.visible())
		    {
			    Point pt = new Point(m_sprWaringTime.position_absolute());
			    Point ptDraw = new Point();
			    ostringstream ostr = new ostringstream();
                long dwTime = FishDefine.time() - m_dwStartTime;
    
			    if (dwTime >= 30)
			    {
				    CGameScene pGameScene = (CGameScene)parent();
    
    
	    //发送退出消息
				    pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_END_GAME, null);
    
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
			    else
			    {
				    ostr.str("");
				    ostr = ostr + (30 - dwTime).ToString();
    
				    ptDraw.x_ = pt.x_ - 8;
				    ptDraw.y_ = pt.y_ + 5;
    
				    DrawTimer(ostr.str(), ptDraw);
			    }
		    }
    
		    if (m_sprReward.visible() && m_bShowRewardNubmer)
		    {
			    Point pt = new Point(m_sprReward.position_absolute());
			    Point ptDraw = new Point();
			    ptDraw.x_ = pt.x_ - 46;
			    ptDraw.y_ = pt.y_;
    
			    ostringstream ostr = new ostringstream();
			    ostr.str("");
			    ostr = ostr + m_nRewardNumber;
			    DrawTaskNumber(ostr.str(),ptDraw);
		    }
    
		    if (m_TaskDate.m_enType == CTaskDate.Type.TYPE_COOK && m_sprCookFish.visible())
		    {
			    Point pt = new Point(m_sprCookNormal.position_absolute());
			    Point ptDraw = new Point();

                long dwTime = FishDefine.time() - m_dwCookStartTime;
    
			    CGameScene pGameScene = (CGameScene)parent();
			    if (pGameScene != null && pGameScene.GetMeChairID() == m_wChairID && (dwTime >= m_TaskDate.m_nDuration))
			    {
				    FireCook();
			    }
    
			    if (dwTime >= m_TaskDate.m_nDuration)
			    {
				    dwTime = m_TaskDate.m_nDuration;
			    }
    
			    ptDraw.x_ = pt.x_ - 60;
			    ptDraw.y_ = pt.y_ - 130;
    
			    DrawCookTimer((int)(m_TaskDate.m_nDuration - dwTime), ptDraw);
    
			    ptDraw.x_ = pt.x_ - 40;
			    ptDraw.y_ = pt.y_;
    
			    DrawFishNumber(ptDraw);
    
			    ptDraw.x_ = pt.x_ - 26;
			    ptDraw.y_ = pt.y_ + 36;
    
			    DrawCookGold(ptDraw);
    
		    }
        }

	    public void DrawTimer(string number, Point pt)
	    {
		    ostringstream ostr = new ostringstream();
		    Point ptDraw = new Point();
		    ptDraw = pt - new Point(number.Length / 2.0 * 24.0, 0);
    
		    for (int i = 0; i < number.Length; i++)
		    {
			    int nIndex = number[i] -'0';
			    m_imgTimer[nIndex].draw(ptDraw);
    
			    ptDraw.x_ += 24;
		    }
    
	    }
	    public void DrawCookTimer(int dwTime, Point pt)
	    {
		    Point ptDraw = new Point(pt);
    
		    int nMinute = dwTime / 60;
		    int nSecond1 = dwTime % 60 / 10;
		    int nSecond2 = dwTime % 60 % 10;
    
		    m_imgCookTime[nMinute].draw(ptDraw);
		    ptDraw.x_ += 28;
    
		    m_imgCookTime[10].draw(ptDraw);
		    ptDraw.x_ += 28;
    
		    m_imgCookTime[nSecond1].draw(ptDraw);
		    ptDraw.x_ += 28;
    
		    m_imgCookTime[nSecond2].draw(ptDraw);
	    }
	    public void DrawCookGold(Point pt)
	    {
		    Point ptDraw = new Point(pt);
    
		    int nHundred = m_TaskDate.m_nBonus / 100;
		    int nTen = m_TaskDate.m_nBonus % 100 / 10;
		    int nUnits = m_TaskDate.m_nBonus % 10;
    
		    m_imgCookGold[nHundred].draw(ptDraw);
		    ptDraw.x_ += 24;
    
		    m_imgCookGold[nTen].draw(ptDraw);
		    ptDraw.x_ += 24;
    
		    m_imgCookGold[nUnits].draw(ptDraw);
		    ptDraw.x_ += 24;
	    }
	    public void DrawFishNumber(Point pt)
	    {
		    Point ptDraw = new Point(pt);
    
		    int nCookFish1 = m_nCookFish / 10;
		    int nCookFish2 = m_nCookFish % 10;
		    int nFishCount1 = m_TaskDate.m_nFishCount / 10;
		    int nFishCount2 = m_TaskDate.m_nFishCount % 10;
    
		    m_imgCookFishNumber[nCookFish1].draw(ptDraw);
		    ptDraw.x_ += 16;
    
		    m_imgCookFishNumber[nCookFish2].draw(ptDraw);
		    ptDraw.x_ += 16;
    
		    m_imgCookFishNumber[10].draw(ptDraw);
		    ptDraw.x_ += 16;
    
		    m_imgCookFishNumber[nFishCount1].draw(ptDraw);
		    ptDraw.x_ += 16;
    
		    m_imgCookFishNumber[nFishCount2].draw(ptDraw);
	    }

        public void DrawTaskNumber(string number, Point pt)
        {
            ostringstream ostr = new ostringstream();
            Point ptDraw = new Point();
            ptDraw = pt - new Point(number.Length / 2.0 * 30.0, -16);

            for (int i = 0; i < number.Length; i++)
            {
                int nIndex = number[i] - '0';
                m_imgTaskNumber[nIndex].draw(ptDraw);

                ptDraw.x_ += 30.0;
            }

        }
        public void SetChairID(int wChairID)
        {
            if (m_wChairID == wChairID)
                return;

            m_wChairID = wChairID;

        }

        public void set_position(Point pt)
        {
            base.set_position(pt);
        }

        public void SetChairPos(int wChairID)
        {
            if (wChairID == GameDefine.INVALID_CHAIR)
                return;

            m_sprGunWing1.set_position(new Point(38, 58));
            m_sprGunWing2.set_position(new Point(262, 58));
            m_sprGunBase.set_position(new Point(150, 48));
            m_sprGunOver.set_position(new Point(150, 70));
            m_sprGun.set_position(new Point(38, 58));
            m_sprGunLight.set_position(new Point(150, 64));

            m_sprCannon.set_position(new Point(150, 70));
            m_sprLaserBeanCannon.set_position(new Point(150, 70));
            m_sprBombCannon.set_position(new Point(150, 30));
            m_sprBombFuse.set_position(new Point(150, 30));
            m_sprLaserBean.set_position(new Point(150, 70));

            m_sprUserInfo.set_position(new Point(150, 0));

            m_sprWaringBullet.set_position(new Point(150, -60));
            m_sprWaringTime.set_position(new Point(150, -100));

            m_sprGunLead.set_position(new Point(150, -100));
            m_sprBuyLead.set_position(new Point(150, -100));
            m_sprPlayLead.set_position(new Point(150, -100));
            m_sprLeftKey.set_position(new Point(280, -80));
            m_sprRighKey.set_position(new Point(280, -80));

            m_sprCumBar.set_position(new Point(150, 96));
            m_sprShowBar.set_position(new Point(150, 96));

            m_sprBarBlank[0].set_position(new Point(88, 96));
            m_sprBarBlank[1].set_position(new Point(150, 96));
            m_sprBarBlank[2].set_position(new Point(212, 96));
            m_sprBar[0].set_position(new Point(88, 95));
            m_sprBar[1].set_position(new Point(150, 95));
            m_sprBar[2].set_position(new Point(212, 95));

            m_sprTaskName[0].set_position(new Point(30, -20));
            m_sprTaskName[1].set_position(new Point(115, -80));
            m_sprTaskName[2].set_position(new Point(205, -80));
            m_sprTaskName[3].set_position(new Point(290, -20));

            m_sprChangeEffect.set_position(new Point(150, -20));

            Point ptExplorer = new Point(640, 369);
            ptExplorer -= position();

            m_sprExplorer.set_position(ptExplorer);


            m_sprShotNumBack.set_position(new Point(-56, 64));
            m_sprNet.set_position(new Point(-148, 64));
            m_sprShotGold.set_position(new Point(-118, 64));

            int nPosTemp = -70;
            m_sprGold[0].set_position(new Point(nPosTemp - 20, 64));
            m_sprGold[1].set_position(new Point(nPosTemp, 64));
            m_sprGold[2].set_position(new Point(nPosTemp + 20, 64));
            m_sprGold[3].set_position(new Point(nPosTemp + 40, 64));
            m_sprGold[4].set_position(new Point(nPosTemp + 60, 64));
            m_sprGold[5].set_position(new Point(nPosTemp + 80, 64));
            m_sprGold[6].set_position(new Point(nPosTemp + 100, 64));
            m_sprGold[7].set_position(new Point(nPosTemp + 120, 64));
            m_sprGold[8].set_position(new Point(nPosTemp + 140, 64));

            m_sprRealFly.set_position(new Point(-40, -100));
            m_sprReelBack.set_position(new Point(-40, -100));
            for (int i = 0; i < 3; i++)
            {
                m_sprReelNormal[i].set_position(new Point(-92 + i * 51, -102));
                m_sprReelFast[i].set_position(new Point(-92 + i * 51, -102));
            }

            m_sprReelEff.set_position(new Point(-40, -100));

            m_sprBombFuse.set_rotation(FishDefine.M_PI_4);

            m_sprCookNormal.set_flip_x(true);
            m_sprCookBack.set_position(new Point(-120, -100));
            m_sprCookNormal.set_position(new Point(-120, -100));
            m_sprCookSay.set_position(new Point(0, -210));
            m_sprCookSayBack.set_position(new Point(-10, -210));
            m_sprCookFish.set_position(new Point(-120, -140));
            m_sprCookGold.set_position(new Point(-160, -50));
            m_sprCookComplete.set_position(new Point(-120, -140));
            m_sprCookWin.set_position(new Point(-120, -100));
            m_sprCookLost.set_position(new Point(-120, -100));

            if (wChairID == 1)
            {
                m_sprReward.set_position(new Point(50, -140));
                m_sprMessage.set_position(new Point(92, -90));
                m_sprConnonLevelUp.set_position(new Point(50, -120));
            }
            else
            {
                m_sprReward.set_position(new Point(180, -140));
                m_sprMessage.set_position(new Point(156, -90));
                m_sprConnonLevelUp.set_position(new Point(160, -120));
            }

            ostringstream ostr = new ostringstream();
            ostr = ostr + "gun_light_" + (int)wChairID + "_0";
            m_sprGunLight.set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));

            Animation aniShotGold = Root.instance().animation_manager().animation("shot_gold");
            Root.instance().action_manager().remove_all_action_from_target(m_sprShotGold);
            GameControls.XLBE.Action actShotGold = new Action_Repeat_Forever(new Action_Animation(0.06, aniShotGold, false));
            m_sprShotGold.run_action(actShotGold);

            ostr.str("");
            ostr = ostr + "bar_" + (int)wChairID;
            m_sprBar[0].set_display_image(Root.instance().imageset_manager().imageset("task").image(ostr.str()));
            m_sprBar[1].set_display_image(Root.instance().imageset_manager().imageset("task").image(ostr.str()));
            m_sprBar[2].set_display_image(Root.instance().imageset_manager().imageset("task").image(ostr.str()));

        }

        public void SetCannonType(FishDefine.enCannonType CannonType, int dwMulRate)
        {
            if ((m_CannonType == CannonType) && (dwMulRate == m_dwMulRate))
                return;

            m_CannonType = CannonType;

            Root.instance().action_manager().remove_all_action_from_target(m_sprCannon);

            ostringstream ostr = new ostringstream();
            ostr = ostr + "gun" + (int)CannonType;
            m_sprCannon.set_display_image(Root.instance().imageset_manager().imageset(ostr.str()).image("0"));

            CGameScene pGameScene = (CGameScene)parent();

            if (m_wChairID == pGameScene.GetMeChairID())
            {
                CMD_C_Change_Cannon ChangeCannon = new CMD_C_Change_Cannon();
                ChangeCannon.wStyle = (int)CannonType;
                ChangeCannon.dwMulRate = dwMulRate;

                pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_CHANGE_CANNON, ChangeCannon);
            }

        }
        public void SetMulRate(int dwMulRate, int dwMaxMulRate)
        {
            m_dwMulRate = dwMulRate;
            m_dwMaxMulRate = dwMaxMulRate;

            bool bGotHead = false;
            int nSpritePosition = 0;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();

            int nBaseXPos = (dwMulRate >= 1000) ? 136 : (dwMulRate >= 100) ? 148 : (dwMulRate >= 10) ? 160 : 170;

            m_sprMulRate[0].set_position(new Point(nBaseXPos - 20, 72));
            m_sprMulRate[1].set_position(new Point(nBaseXPos, 72));
            m_sprMulRate[2].set_position(new Point(nBaseXPos + 20, 72));
            m_sprMulRate[3].set_position(new Point(nBaseXPos + 40, 72));
            m_sprMulRate[4].set_position(new Point(nBaseXPos + 60, 72));

            nSingleNumber = (int)(m_dwMulRate % 100000 / 10000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "connon_number_" + nSingleNumber;
                m_sprMulRate[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(m_dwMulRate % 100000 % 10000 / 1000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "connon_number_" + nSingleNumber;
                m_sprMulRate[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }


            nSingleNumber = (int)(m_dwMulRate % 100000 % 10000 % 1000 / 100);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "connon_number_" + nSingleNumber;
                m_sprMulRate[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(m_dwMulRate % 100000 % 10000 % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "connon_number_" + nSingleNumber;
                m_sprMulRate[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }


            nSingleNumber = (int)(m_dwMulRate % 100000 % 10000 % 10);
            ostr.str("");
            ostr = ostr + "connon_number_" + nSingleNumber;
            m_sprMulRate[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));

            for (int i = nSpritePosition; i < 5; i++)
            {
                m_sprMulRate[i].set_display_image(null);
            }

            //Point tPoint[5];
            //for(int i=0;i<5;i++) tPoint[i] = m_sprMulRate[i].position();

            //for (int i=0; i<5; i++)
            //{
            //	if (m_sprMulRate[i] != 0)
            //	{
            //		Point pt = tPoint[i];pt.x_ += i*12;
            //		double fSizeBase = 1.8;
            //		int nSpace = 12;
            //		pt.x_ -= 18 ;
            //		pt.y_ -= 18;
            //		m_sprMulRate[i].set_position(pt);

            //		//GameControls.XLBE.Action *act = new Action_Sequence(new Action_Scale_To(0.25, fSizeBase), new Action_Move_To(0.1, pt+new Point(-nSpace, -nSpace)), new Action_Move_To(0.1, pt+new Point(+nSpace, +nSpace)), new Action_Scale_To(0.25, 1.0), new Action_Move_To(0.1, tPoint[i]),0);
            //		GameControls.XLBE.Action *act = new Action_Sequence(new Action_Move_To(0.1, pt+new Point(-nSpace, -nSpace)), new Action_Move_To(0.1, pt+new Point(+nSpace, +nSpace)), new Action_Scale_To(0.25, 1.0), new Action_Move_To(0.1, tPoint[i]),0);
            //		m_sprMulRate[i].run_action(act);   
            //	}

            //}
        }

        public void SetFishGold(int dwFishGold)
        {
            if (dwFishGold == m_dwFishGold)
                return;

            m_dwFishGold = dwFishGold;

            bool bGotHead = false;
            int nSpritePosition = 0;
            int nSingleNumber = 0;
            ostringstream ostr = new ostringstream();

            nSingleNumber = (int)(m_dwFishGold % 1000000000 / 100000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "number_" + nSingleNumber;
                m_sprGold[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(m_dwFishGold % 100000000 / 10000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "number_" + nSingleNumber;
                m_sprGold[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(m_dwFishGold % 10000000 / 1000000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "number_" + nSingleNumber;
                m_sprGold[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(m_dwFishGold % 1000000 / 100000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "number_" + nSingleNumber;
                m_sprGold[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(m_dwFishGold % 100000 / 10000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "number_" + nSingleNumber;
                m_sprGold[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(m_dwFishGold % 10000 / 1000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "number_" + nSingleNumber;
                m_sprGold[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(m_dwFishGold % 1000 / 100);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "number_" + nSingleNumber;
                m_sprGold[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(m_dwFishGold % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + "number_" + nSingleNumber;
                m_sprGold[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }


            nSingleNumber = (int)(m_dwFishGold % 10);
            ostr.str("");
            ostr = ostr + "number_" + nSingleNumber;
            m_sprGold[nSpritePosition++].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));

            for (int i = nSpritePosition; i < 9; i++)
            {
                m_sprGold[i].set_display_image(null);
            }


        }
        public void ShowFishGoldEmpty(bool bShow)
        {

            m_sprWaringBullet.set_visible(bShow);

        }


        public void UpdateGold(int wFishGold, int wFishTime)
        {
            SetFishGold( m_dwFishGold + wFishGold );

            //闯关房间
            CGameScene pGameScene = (CGameScene)parent();
            if ((pGameScene.m_dwRoomType == 3) && (m_wChairID == pGameScene.GetMeChairID()))
            {
                if (m_dwFishGold >= pGameScene.m_tagGateCtrlInf[pGameScene.m_nCurGateCount].nFishScore)
                {
                    if (pGameScene.m_nCurGateCount == (pGameScene.m_nGateCount - 1))
                    {
                        pGameScene.m_GateEndLayer.Show(true);
                    }
                    else
                    {
                        pGameScene.m_MatchAgainLayer.Show(true);
                    }

                }

                if ((m_dwFishGold > 0) && (pGameScene.m_MatchStart.visible()))
                {
                    pGameScene.m_MatchStart.Show(false);
                }
            }

            //	SetFishGold(m_dwFishGold);

            Point ptBase0 = new Point(-120 + (0) * 42, 36);
            Point ptBase1 = new Point(-120 + (1) * 42, 36);
            Point ptBase2 = new Point(-120 + (2) * 42, 36);
            Point ptBase3 = new Point(-120 + (3) * 42, 36);

            //1.找有没有空位
            int nOKCount = 0;
            for (int i = 0; i < FishDefine.MAX_SHOW_TIME_COUNT; i++)
            {
                if (m_FishGoldShow[i].bEnable == 1)
                {
                    nOKCount++;
                }
            }

            if (nOKCount >= FishDefine.MAX_SHOW_TIME_COUNT)
            {
                for (int i = 0; i < FishDefine.MAX_SHOW_TIME_COUNT; i++)
                {
                    switch (m_FishGoldShow[i].nIndex)
                    {
                        case 0:
                            m_FishGoldShow[i].ptBaseOld = m_FishGoldShow[i].ptBase;
                            m_FishGoldShow[i].nXPosMove = 0;
                            m_FishGoldShow[i].nXPosMoveCount = 0;
                            m_FishGoldShow[i].ptBase = ptBase1;
                            m_FishGoldShow[i].nIndex = 1;
                            break;
                        case 1:
                            m_FishGoldShow[i].ptBaseOld = m_FishGoldShow[i].ptBase;
                            m_FishGoldShow[i].nXPosMove = 0;
                            m_FishGoldShow[i].nXPosMoveCount = 0;
                            m_FishGoldShow[i].ptBase = ptBase2;
                            m_FishGoldShow[i].nIndex = 2;
                            break;
                        case 2:
                            m_FishGoldShow[i].ptBaseOld = m_FishGoldShow[i].ptBase;
                            m_FishGoldShow[i].nXPosMove = 0;
                            m_FishGoldShow[i].nXPosMoveCount = 0;
                            m_FishGoldShow[i].ptBase = ptBase3;
                            m_FishGoldShow[i].nIndex = 3;
                            break;
                        default:
                            {
                                for (int j = 0; j < FishDefine.MAX_SHOW_GOLDEN_COUNT; j++)
                                {
                                    m_sprShow[i * FishDefine.MAX_SHOW_GOLDEN_COUNT + j].set_display_image(null);
                                }
                                for (int j = 0; j < 8; j++)
                                {
                                    m_spValue[i * 8 + j].set_display_image(null);
                                }

                                m_FishGoldShow[i].bEnable = 1;
                                m_FishGoldShow[i].ptBase = ptBase0;
                                m_FishGoldShow[i].ptBaseOld = m_FishGoldShow[i].ptBase;
                                m_FishGoldShow[i].ptChange = new Point(0, 0);
                                m_FishGoldShow[i].dwFishGold = wFishGold;
                                m_FishGoldShow[i].dwFishScore = (wFishTime >= FishDefine.MAX_SHOW_GOLDEN_COUNT) ? FishDefine.MAX_SHOW_GOLDEN_COUNT - 1 : wFishTime;
                                m_FishGoldShow[i].cbFishScoreCount = 0;
                                m_FishGoldShow[i].cbGoldTurnCount = 0;
                                m_FishGoldShow[i].wTime = FishDefine.MAX_SHOW_STEP_TIME;
                                m_FishGoldShow[i].wTimeChange = FishDefine.MAX_CHANGE_STEP_TIME;
                                m_FishGoldShow[i].nIndex = 0;
                                m_FishGoldShow[i].nXPosMove = 0;
                                m_FishGoldShow[i].nXPosMoveCount = 0;
                                m_FishGoldShow[i].nShowCount = 0;
                            }
                            break;
                    }
                }
            }
            else
            {
                ////移动 1--
                //INT nMax0 = -1;
                //for(int i=0;i<FishDefine.MAX_SHOW_TIME_COUNT;i++) 
                //{
                //	if(m_FishGoldShow[i].bEnable==TRUE)
                //	{
                //		if(m_FishGoldShow[i].nIndex>nMax0) nMax0 = i;
                //	}
                //}
                //if(nMax0>=0)
                //{
                //	m_FishGoldShow[nMax0].ptBaseOld = m_FishGoldShow[nMax0].ptBase;
                //	m_FishGoldShow[nMax0].nXPosMove=0;
                //	m_FishGoldShow[nMax0].nXPosMoveCount=0;
                //	m_FishGoldShow[nMax0].ptBase = ptBase3;
                //	m_FishGoldShow[nMax0].nIndex = 3;
                //}

                ////移动 2--
                //INT nMax1 = -1;
                //for(int i=0;i<FishDefine.MAX_SHOW_TIME_COUNT;i++) 
                //{
                //	if(i==nMax0) continue;

                //	if(m_FishGoldShow[i].bEnable==TRUE)
                //	{
                //		if(m_FishGoldShow[i].nIndex>nMax1) nMax1 = i;
                //	}
                //}
                //if(nMax1>=0)
                //{
                //	m_FishGoldShow[nMax1].ptBaseOld = m_FishGoldShow[nMax1].ptBase;
                //	m_FishGoldShow[nMax1].nXPosMove=0;
                //	m_FishGoldShow[nMax1].nXPosMoveCount=0;
                //	m_FishGoldShow[nMax1].ptBase = ptBase2;
                //	m_FishGoldShow[nMax1].nIndex = 2;
                //}

                ////移动 3--
                //INT nMax2 = -1;
                //for(int i=0;i<FishDefine.MAX_SHOW_TIME_COUNT;i++) 
                //{
                //	if(i==nMax0) continue;
                //	if(i==nMax1) continue;

                //	if(m_FishGoldShow[i].bEnable==TRUE)
                //	{
                //		if(m_FishGoldShow[i].nIndex>nMax2) nMax2 = i;
                //	}
                //}
                //if(nMax2>=0)
                //{
                //	m_FishGoldShow[nMax2].ptBaseOld = m_FishGoldShow[nMax2].ptBase;
                //	m_FishGoldShow[nMax2].nXPosMove=0;
                //	m_FishGoldShow[nMax2].nXPosMoveCount=0;
                //	m_FishGoldShow[nMax2].ptBase = ptBase1;
                //	m_FishGoldShow[nMax2].nIndex = 1;
                //}

                int bIsAdd = 0;
                for (int i = 0; i < FishDefine.MAX_SHOW_TIME_COUNT; i++)
                {
                    if ((m_FishGoldShow[i].bEnable == 0) && (bIsAdd == 0))
                    {
                        for (int j = 0; j < FishDefine.MAX_SHOW_GOLDEN_COUNT; j++)
                        {
                            m_sprShow[i * FishDefine.MAX_SHOW_GOLDEN_COUNT + j].set_display_image(null);
                        }
                        for (int j = 0; j < 8; j++)
                        {
                            m_spValue[i * 8 + j].set_display_image(null);
                        }

                        bIsAdd = 1;
                        m_FishGoldShow[i].bEnable = 1;
                        m_FishGoldShow[i].ptBase = (nOKCount == 0) ? ptBase3 : (nOKCount == 1) ? ptBase2 : (nOKCount == 2) ? ptBase1 : ptBase0;
                        m_FishGoldShow[i].ptBaseOld = m_FishGoldShow[i].ptBase;
                        m_FishGoldShow[i].ptChange = new Point(0, 0);
                        m_FishGoldShow[i].dwFishGold = wFishGold;
                        m_FishGoldShow[i].dwFishScore = (wFishTime >= FishDefine.MAX_SHOW_GOLDEN_COUNT) ? FishDefine.MAX_SHOW_GOLDEN_COUNT - 1 : wFishTime;
                        m_FishGoldShow[i].cbFishScoreCount = 0;
                        m_FishGoldShow[i].cbGoldTurnCount = 0;
                        m_FishGoldShow[i].wTime = FishDefine.MAX_SHOW_STEP_TIME;
                        m_FishGoldShow[i].wTimeChange = FishDefine.MAX_CHANGE_STEP_TIME;
                        m_FishGoldShow[i].nIndex = (nOKCount == 0) ? 3 : (nOKCount == 1) ? 2 : (nOKCount == 2) ? 1 : 0;
                        m_FishGoldShow[i].nXPosMove = 0;
                        m_FishGoldShow[i].nXPosMoveCount = 0;
                        m_FishGoldShow[i].nShowCount = 0;
                    }
                }
            }

            return;
        }

        public bool UpdateGoldEnd(Node node, int tag)
        {
            if (tag == 1000)
            {
                SetFishGold(m_dwFishGold);
            }

            remove_child(node);
            node = null;

            return true;
        }
        public bool UpdateGoldReal(Node node, int tag)
        {
            SetFishGold(m_dwFishGold + tag);

            remove_child(node);
            node = null;

            return true;
        }
        public bool UpdateGoldSound(Node node, int tag)
        {

            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(15);
                pSound.play(false, true);
            }
            catch
            {
            }


            return true;
        }
        public void SetCannonRatation(Point ptMouse)
        {
            double fRorate;
            Point pt = new Point(ptMouse);


            int wChairTemp = (m_wChairID == 0) ? 3 : (m_wChairID == 1) ? 2 : m_wChairID;

            if (wChairTemp < 2)
            {
                pt = pt - m_sprCannon.position_absolute();
                if (pt.y_ >= 0)
                {
                    fRorate = Math.Acos(pt.x_ / Math.Sqrt(pt.x_ * pt.x_ + pt.y_ * pt.y_)) + FishDefine.M_PI_2;
                }
                else
                {
                    if (pt.x_ >= 0)
                    {
                        fRorate = FishDefine.M_PI_2;
                    }
                    else
                    {
                        fRorate = FishDefine.M_PI_2 + Math.PI;
                    }
                }

            }
            else
            {
                pt = m_sprCannon.position_absolute() - pt;
                if (pt.y_ >= 0)
                {
                    fRorate = Math.Acos(pt.x_ / Math.Sqrt(pt.x_ * pt.x_ + pt.y_ * pt.y_)) - FishDefine.M_PI_2;
                }
                else
                {
                    if (pt.x_ <= 0)
                    {
                        fRorate = FishDefine.M_PI_2;
                    }
                    else
                    {
                        fRorate = FishDefine.M_PI_2 + Math.PI;
                    }
                }

            }

            m_sprCannon.set_rotation(fRorate);
            m_sprLaserBeanCannon.set_rotation(fRorate);
        }
        public void SetCannonRatation(double fRate)
        {
            m_sprLaserBeanCannon.set_rotation(fRate);
            m_sprCannon.set_rotation(fRate);
        }

        public void GunAnimation()
        {
            ostringstream ostr = new ostringstream();
            ostr = ostr + "gun" + (int)m_CannonType;
            Animation aniGun = Root.instance().animation_manager().animation(ostr.str());

            Root.instance().action_manager().remove_all_action_from_target(m_sprCannon);
            m_sprCannon.run_action(new Action_Animation(0.06, aniGun, true));

        }
        public void GunBaseAnimation()
        {
            ostringstream ostr = new ostringstream();
            ostr = ostr + "gun_light_" + (int)m_wChairID;
            Animation aniGunLight = Root.instance().animation_manager().animation(ostr.str());

            Root.instance().action_manager().remove_all_action_from_target(m_sprGunLight);
            m_sprGunLight.run_action(new Action_Animation(0.06, aniGunLight, true));
        }
        public void RewardAnimation(int nRewardNumber, bool bDelay)
        {
            m_nRewardNumber = nRewardNumber;

            Animation ani = Root.instance().animation_manager().animation("get_reward");

            Root.instance().action_manager().remove_all_action_from_target(m_sprReward);

            if (bDelay)
            {
                m_sprReward.run_action(new Action_Sequence(new Action_Delay(2), new Action_Show(), new Action_Animation(0.2, ani, true), new Action_Func(RewardAnimationEnd1), new Action_Delay(2), new Action_Func(RewardAnimationEnd2), null));
            }
            else
            {
                m_sprReward.run_action(new Action_Sequence(new Action_Show(), new Action_Animation(0.2, ani, true), new Action_Func(RewardAnimationEnd1), new Action_Delay(2), new Action_Func(RewardAnimationEnd2), null));
            }
        }
        public bool RewardAnimationEnd1(Node node, int tag)
        {
            m_bShowRewardNubmer = true;
            //((Sprite*)node).set_display_image(Root.instance().imageset_manager().imageset("get_reward").image("8"));
            return true;
        }
        public bool RewardAnimationEnd2(Node node, int tag)
        {
            node.set_visible(false);
            m_bShowRewardNubmer = false;
            return true;
        }
        public void ShowBarAnimation()
        {
            Animation aniShowBar = Root.instance().animation_manager().animation("show_bar");
            GameControls.XLBE.Action actShowBar = new Action_Repeat_Forever(new Action_Animation(0.2, aniShowBar, false));
            actShowBar.set_tag(0);
            m_sprShowBar.run_action(actShowBar);
        }
        public void DestroyShowBarAnimation()
        {
            Root.instance().action_manager().remove_all_action_from_target(m_sprShowBar);
            m_sprShowBar.set_display_image(null);
        }

        public void ShowUserInfo(bool bVisible)
        {
            m_sprUserInfo.set_visible(bVisible);
        }
        public void ShowGunLead(bool bShow)
        {
            m_sprGunLead.set_visible(bShow);
            m_sprLeftKey.set_visible(bShow);
        }
        public void ShowBuyLead(bool bShow)
        {
            m_sprBuyLead.set_visible(bShow);
        }
        public void ShowPlayLead(bool bShow)
        {
            m_sprPlayLead.set_visible(bShow);
            m_sprRighKey.set_visible(bShow);
        }
        public void ShowWaringTime(bool bShow)
        {
            if (bShow)
            {
                m_dwStartTime = FishDefine.time();
            }

            m_sprWaringTime.set_visible(bShow);
        }
        public bool IsShowWaringTime()
        {
            return m_sprWaringTime.visible();
        }
        public bool CommandTaskStart(CMD_S_Task pTask)
        {
            m_dwCookStartTime = FishDefine.time();
            m_TaskDate.m_enType = (CTaskDate.Type)pTask.nTask;
            m_TaskDate.m_enState = CTaskDate.State.STATE_PREPARE1;
            m_TaskDate.m_nStartWheel[0] = pTask.nStartWheel[0];
            m_TaskDate.m_nStartWheel[1] = pTask.nStartWheel[1];
            m_TaskDate.m_nStartWheel[2] = pTask.nStartWheel[2];
            m_TaskDate.m_nEndWheel[0] = pTask.nEndWheel[0];
            m_TaskDate.m_nEndWheel[1] = pTask.nEndWheel[1];
            m_TaskDate.m_nEndWheel[2] = pTask.nEndWheel[2];
            m_TaskDate.m_nDuration = pTask.nDuration;
            m_TaskDate.m_nFishType = pTask.cbFishType;
            m_TaskDate.m_nFishCount = pTask.cbFishCount;
            m_TaskDate.m_nBonus = pTask.nBonus;

            int nLevel = pTask.cbLevel;

            int wChairTemp = (m_wChairID == 0) ? 3 : (m_wChairID == 1) ? 2 : m_wChairID;

            Point ptBarEnough = new Point();
            if (wChairTemp == 0 || wChairTemp == 1)
            {
                ptBarEnough.x_ = 104 + nLevel * 62;
                ptBarEnough.y_ = 0;
            }
            else
            {
                ptBarEnough.x_ = 104 + nLevel * 62;
                ptBarEnough.y_ = 96;
            }

            m_sprBarEnough.set_visible(true);
            m_sprBarEnough.set_position(ptBarEnough);

            Animation aniBarEnough = Root.instance().animation_manager().animation("bar_enough");

            GameControls.XLBE.Action actBarEnough = new Action_Sequence(new Action_Animation(0.06, aniBarEnough, false), new Action_Hide(), new Action_Func(BarEnoughEnd), null);
            m_sprBarEnough.run_action(actBarEnough);

            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(18);
                pSound.play(false, true);
            }
            catch
            {
            }

            return true;
        }

        public bool BarEnoughEnd(Node node, int tag)
        {
            DestroyShowBarAnimation();

            Animation aniRealFly = Root.instance().animation_manager().animation("real_fly");
            GameControls.XLBE.Action actRealFly = new Action_Sequence(new Action_Show(), new Action_Animation(0.06, aniRealFly, false), new Action_Hide(), new Action_Func(RealFlyEnd), null);
            m_sprRealFly.run_action(actRealFly);


            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(19);
                pSound.play(false, true);
            }
            catch
            {
            }
            return true;
        }
        public bool RealFlyEnd(Node node, int tag)
        {
            m_sprReelBack.set_visible(true);

            for (int i = 0; i < 3; i++)
            {
                int nWheel = m_TaskDate.m_nStartWheel[i];
                int nEndWheel = m_TaskDate.m_nEndWheel[i];
                int nDistance = nEndWheel - nWheel;
                if (nDistance <= 0)
                {
                    nDistance += 4;
                }

                m_sprReelNormal[i].set_visible(true);

                m_sprReelNormal[i].set_content_size(new Size(39, 46));
                m_sprReelNormal[i].set_src_size(new Size(39, 46));

                m_sprReelNormal[i].set_src_position(new Point(0, nWheel * 46));
                GameControls.XLBE.Action actReelNormal;
                if (i == 2)
                {
                    actReelNormal = new Action_Sequence(new Action_Delay(1.0), new Action_Hide(), new Action_Delay(3.0 + i), new Action_Show(), new Action_Move_Src_Acceleration_To(5.0, new Point(0, 46 * ((4 - nWheel) - nDistance - 4)), 184), new Action_Delay(3.0), new Action_Func(ReelNormalEnd), null);
                }
                else
                {
                    actReelNormal = new Action_Sequence(new Action_Delay(1.0), new Action_Hide(), new Action_Delay(3.0 + i), new Action_Show(), new Action_Move_Src_Acceleration_To(5.0, new Point(0, 46 * ((4 - nWheel) - nDistance - 4)), 184), null);
                }

                m_sprReelNormal[i].run_action(actReelNormal);

                m_sprReelFast[i].set_content_size(new Size(39, 46));
                m_sprReelFast[i].set_src_size(new Size(39, 46));

                m_sprReelFast[i].set_src_position(new Point(0, 80 * nWheel));
                GameControls.XLBE.Action actReelFas = new Action_Sequence(new Action_Delay(1.0), new Action_Show(), new Action_Move_Src_To(3.0 + i, new Point(0, 80 * (nWheel + 10)), 320), new Action_Hide(), null);
                m_sprReelFast[i].run_action(actReelFas);
            }

            if (m_TaskDate.m_enType != CTaskDate.Type.TYPE_NULL)
            {
                Animation aniReelEff = Root.instance().animation_manager().animation("reel_eff");
                GameControls.XLBE.Action actReelEff = new Action_Sequence(new Action_Delay(11.0), new Action_Show(), new Action_Func(ReelEffEnd), new Action_Animation(0.1, aniReelEff, false), null);
                m_sprReelEff.run_action(actReelEff);
            }



            return true;
        }
        public bool ReelEffEnd(Node node, int tag)
        {
            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(23);
                pSound.play(false, true);
            }
            catch
            {
            }

            return true;
        }

        public bool ReelNormalEnd(Node node, int tag)
        {
            m_sprReelEff.set_visible(false);
            m_sprReelBack.set_visible(false);

            for (int i = 0; i < 3; i++)
            {
                m_sprReelFast[i].set_visible(false);
                m_sprReelNormal[i].set_visible(false);
            }

            if (m_TaskDate.m_enType == CTaskDate.Type.TYPE_BEAN)
            {
                m_sprTaskName[0].set_display_image(Root.instance().imageset_manager().imageset("task").image("laser_bean_0"));
                m_sprTaskName[1].set_display_image(Root.instance().imageset_manager().imageset("task").image("laser_bean_1"));
                m_sprTaskName[2].set_display_image(Root.instance().imageset_manager().imageset("task").image("laser_bean_2"));
                m_sprTaskName[3].set_display_image(Root.instance().imageset_manager().imageset("task").image("laser_bean_3"));
            }
            else if (m_TaskDate.m_enType == CTaskDate.Type.TYPE_BOMB)
            {
                m_sprTaskName[0].set_display_image(Root.instance().imageset_manager().imageset("task").image("bomb_0"));
                m_sprTaskName[1].set_display_image(Root.instance().imageset_manager().imageset("task").image("bomb_1"));
                m_sprTaskName[2].set_display_image(Root.instance().imageset_manager().imageset("task").image("bomb_2"));
                m_sprTaskName[3].set_display_image(Root.instance().imageset_manager().imageset("task").image("bomb_3"));
            }
            else if (m_TaskDate.m_enType == CTaskDate.Type.TYPE_COOK)
            {
                m_sprTaskName[0].set_display_image(Root.instance().imageset_manager().imageset("task").image("cook_0"));
                m_sprTaskName[1].set_display_image(Root.instance().imageset_manager().imageset("task").image("cook_1"));
                m_sprTaskName[2].set_display_image(Root.instance().imageset_manager().imageset("task").image("cook_2"));
                m_sprTaskName[3].set_display_image(Root.instance().imageset_manager().imageset("task").image("cook_3"));
            }
            else if (m_TaskDate.m_enType == CTaskDate.Type.TYPE_BONUS)
            {
                FireBonus();
            }
            else
            {
            }

            if (m_TaskDate.m_enType > CTaskDate.Type.TYPE_BONUS)
            {
                GameControls.XLBE.Action func = new Action_Func(TaskNameEnd);
                func.set_tag(0);
                GameControls.XLBE.Action act = new Action_Sequence(new Action_Delay(0.2), new Action_Show(), func, null);
                m_sprTaskName[0].run_action(act);

                func = new Action_Func(TaskNameEnd);
                func.set_tag(1);
                act = new Action_Sequence(new Action_Delay(0.4), new Action_Show(), func, null);
                m_sprTaskName[1].run_action(act);

                func = new Action_Func(TaskNameEnd);
                func.set_tag(2);
                act = new Action_Sequence(new Action_Delay(0.6), new Action_Show(), func, null);
                m_sprTaskName[2].run_action(act);

                func = new Action_Func(TaskNameEnd);
                func.set_tag(3);
                act = new Action_Sequence(new Action_Delay(0.8), new Action_Show(), new Action_Delay(0.8), func, null);
                m_sprTaskName[3].run_action(act);

                m_TaskDate.m_enState = CTaskDate.State.STATE_PREPARE2;

                try
                {
                    Sound_Instance pSound = Root.instance().sound_manager().sound_instance(20);
                    pSound.play(false, true);
                }
                catch
                {
                }
            }

            SetFireCountDelay(false);

            return true;
        }

        public bool TaskNameEnd(Node node, int tag)
        {
            if (tag == 3)
            {
                m_sprTaskName[0].set_visible(false);
                m_sprTaskName[1].set_visible(false);
                m_sprTaskName[2].set_visible(false);
                m_sprTaskName[3].set_visible(false);

                if (m_TaskDate.m_enType > CTaskDate.Type.TYPE_COOK)
                {

                    try
                    {
                        Sound_Instance pSound = Root.instance().sound_manager().sound_instance(24);
                        pSound.play(false, true);
                    }
                    catch
                    {
                    }

                    Animation ani = Root.instance().animation_manager().animation("change_eff");
                    GameControls.XLBE.Action act = new Action_Sequence(new Action_Animation(0.06, ani, false), new Action_Func(ChangeEffectEnd), null);
                    m_sprChangeEffect.run_action(act);
                    m_sprChangeEffect.set_visible(true);
                }
                else if (m_TaskDate.m_enType == CTaskDate.Type.TYPE_COOK)
                {
                    m_sprCookBack.set_visible(true);
                    m_sprCookNormal.set_visible(true);
                    m_sprCookSayBack.set_visible(true);
                    m_sprCookSay.set_visible(true);

                    m_sprCookSay.set_color(new Color_Rect());
                    m_sprCookSayBack.set_color(new Color_Rect());
                    m_sprCookSay.set_display_image(Root.instance().imageset_manager().imageset("cook").image("say_1"));

                    Animation ani = Root.instance().animation_manager().animation("cook_normal");
                    GameControls.XLBE.Action act = new Action_Sequence(new Action_Animation(0.2, ani, true), new Action_Func(CookNormalEnd), new Action_Fade_Out(1.0), new Action_Func(CookNormalFadeOutEnd), null);
                    m_sprCookNormal.run_action(act);
                }
            }
            else
            {
                try
                {
                    Sound_Instance pSound = Root.instance().sound_manager().sound_instance(20);
                    pSound.play(false, true);
                }
                catch
                {
                }
            }

            return true;
        }

        public bool CookNormalEnd(Node node, int tag)
        {
            GameControls.XLBE.Action actCookSay = new Action_Fade_Out(1.0);
            m_sprCookSay.run_action(actCookSay);

            GameControls.XLBE.Action actCookSayBack = new Action_Fade_Out(1.0);
            m_sprCookSayBack.run_action(actCookSayBack);

            ostringstream ostr = new ostringstream();
            ostr = ostr + "fish_" + m_TaskDate.m_nFishType;

            m_sprCookFish.set_display_image(Root.instance().imageset_manager().imageset("cook").image(ostr.str()));
            m_sprCookFish.set_visible(true);
            m_sprCookGold.set_visible(true);

            return true;
        }
        public bool CookNormalFadeOutEnd(Node node, int tag)
        {

            CGameScene pGameScene = (CGameScene)parent();

            if (pGameScene.GetMeChairID() == m_wChairID)
            {
                CMD_C_Task_Prepared TaskPrepared = new CMD_C_Task_Prepared();
                TaskPrepared.nTask = (int)m_TaskDate.m_enType;

                pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_TASK_PREPARED, TaskPrepared);
            }

            m_TaskDate.m_enState = CTaskDate.State.STATE_RUNNING;

            return true;
        }
        public bool ChangeEffectEnd(Node node, int tag)
        {
            m_sprChangeEffect.set_visible(false);

            if (m_TaskDate.m_enType == CTaskDate.Type.TYPE_BEAN)
            {
                m_TaskDate.m_enState = CTaskDate.State.STATE_RUNNING;
                m_sprLaserBeanCannon.set_visible(true);

            }
            else if (m_TaskDate.m_enType == CTaskDate.Type.TYPE_BOMB)
            {
                m_sprBombCannon.set_visible(true);
                m_sprCannon.set_visible(false);

                m_sprBomb.set_visible(true);
                m_sprBomb.set_position(m_sprBombCannon.position());
                Root.instance().action_manager().remove_all_action_from_target(m_sprBomb);

                Animation ani = Root.instance().animation_manager().animation("bomb_task");
                GameControls.XLBE.Action act = new Action_Repeat_Forever(new Action_Animation(0.1, ani, false));
                m_sprBomb.run_action(act);

                Point pt = new Point(640, 369);
                pt -= position();

                act = new Action_Sequence(new Action_Move_To(0.5, pt), new Action_Func(BombMoveEnd), null);
                m_sprBomb.run_action(act);
            }

            return true;
        }
        public bool BombMoveEnd(Node node, int tag)
        {
            m_sprBombFuse.set_visible(true);

            Animation ani = Root.instance().animation_manager().animation("bomb_fuse");
            GameControls.XLBE.Action act = new Action_Repeat_Forever(new Action_Animation(0.1, ani, false));
            m_sprBombFuse.run_action(act);

            m_TaskDate.m_enState = CTaskDate.State.STATE_RUNNING;

            return true;
        }

        public void SetFireCount(int nFireCount)
        {
            m_nFireCount = nFireCount;

            if (m_bDelayBarEnough)
            {
                return;
            }

            int nCount = (m_nFireCount % FishDefine.EXP_CHANGE_TO_LEVEL) / (FishDefine.EXP_CHANGE_TO_LEVEL / 3) + 1;
            double fPercentage = m_nFireCount % (FishDefine.EXP_CHANGE_TO_LEVEL / 3) / 1000.00;

            int wChairTemp = (m_wChairID == 0) ? 3 : (m_wChairID == 1) ? 2 : m_wChairID;

            Point pt = new Point();
            if (wChairTemp == 0 || wChairTemp == 1)
            {
                pt.x_ = 88;
                pt.y_ = -1;
            }
            else
            {
                pt.x_ = 88;
                pt.y_ = 95;
            }

            for (int i = 0; i < 3; i++)
            {
                m_sprBar[i].set_position(new Point(pt.x_ + i * 62, pt.y_));
                m_sprBar[i].set_content_size(new Size(1, 1));
                m_sprBar[i].set_src_size(new Size(1, 1));
            }

            for (int i = 0; i < nCount && i < 3; i++)
            {
                if (i == (nCount - 1))
                {
                    m_sprBar[i].set_position(new Point(pt.x_ + i * 62 - (31.0 - 31.0 * fPercentage), pt.y_));
                    m_sprBar[i].set_content_size(new Size(62 * fPercentage, 9));
                    m_sprBar[i].set_src_size(new Size(62 * fPercentage, 9));
                }
                else
                {
                    m_sprBar[i].set_position(new Point(pt.x_ + i * 62, pt.y_));
                    m_sprBar[i].set_content_size(new Size(62, 9));
                    m_sprBar[i].set_src_size(new Size(62, 9));
                }
            }


            if (m_nFireCount % FishDefine.EXP_CHANGE_TO_LEVEL / 3 > 970)
            {
                ShowBarAnimation();
            }
        }
        public void FireBombFish(long lBigStock, long lSmallStock, int dwMulRate)
        {
            long lAllBigFishScore = 0;
            long lAllSmallFishScore = 0;

            Point ptFish = new Point();
            Rect rcScreen = new Rect(0, 0, 1280, 738);
            CGameScene pGameScene = (CGameScene)parent();

            CMD_C_Bomb Bomb = new CMD_C_Bomb();
            Bomb.cbCount = 0;

            //计算屏幕中的鱼是否超过库存
            foreach ( Node j in pGameScene.m_layFishObject.childs())
            {
                if (Bomb.cbCount >= FishDefine.MAX_FISH_IN_NET_BOMB)
                    break;

                CFishObjectExtend pFishObjectExtend = (CFishObjectExtend)j.node_extend();

                if (pFishObjectExtend.wID == FishDefine.INVALID_WORD)
                    continue;

                ptFish = j.position();
                if (rcScreen.pt_in_rect(ptFish))
                {
                    //if(pFishObjectExtend.FishType = CGameCore::FishType_14) 
                    //{
                    //	(*j).stop_all_action();
                    //	pFishObjectExtend.wID = FishDefine.INVALID_WORD;
                    //	pGameScene.m_layFishObject.FishDestory(*j,1);
                    //}
                    //else
                    {
                        Bomb.FishNetObjects[Bomb.cbCount].wID = pFishObjectExtend.wID;
                        Bomb.FishNetObjects[Bomb.cbCount].wRoundID = pFishObjectExtend.wRoundID;
                        Bomb.FishNetObjects[Bomb.cbCount].wType = (int)pFishObjectExtend.FishType;
                        Bomb.FishNetObjects[Bomb.cbCount].dwTime = pFishObjectExtend.dwTime;

                        if (pFishObjectExtend.GetFishGoldByStyle() >= 10)
                        {
                            lAllBigFishScore += pFishObjectExtend.GetFishGoldByStyle() * dwMulRate;
                        }
                        else
                        {
                            lAllSmallFishScore += pFishObjectExtend.GetFishGoldByStyle() * dwMulRate;
                        }

                        Bomb.cbCount++;
                    }

                }
            }

            if (lBigStock >= (lAllBigFishScore + lAllBigFishScore))
            {

                m_sprExplorer.set_visible(true);
                m_sprExplorer.set_scale(new Size(5.0, 5.0));

                Root.instance().action_manager().remove_all_action_from_target(m_sprBomb);
                Root.instance().action_manager().remove_all_action_from_target(m_sprBomb);

                m_sprBombCannon.set_visible(false);
                m_sprCannon.set_visible(true);
                m_sprBomb.set_visible(false);
                m_sprBombFuse.set_visible(false);

                Animation ani = Root.instance().animation_manager().animation("explorer");
                GameControls.XLBE.Action act = new Action_Sequence(new Action_Animation(0.06, ani, false), new Action_Func(FireBombEnd), null);
                m_sprExplorer.run_action(act);

                pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_BOMB, Bomb);

                try
                {
                    Sound_Instance pSound = Root.instance().sound_manager().sound_instance(21);
                    pSound.play(false, true);
                }
                catch
                {
                }
            }
        }

        public void FireBomb()
        {
            m_TaskDate.m_enType = CTaskDate.Type.TYPE_NULL;
            m_TaskDate.m_enState = CTaskDate.State.STATE_NULL;

            m_sprExplorer.set_visible(true);
            m_sprExplorer.set_scale(new Size(5.0, 5.0));

            Root.instance().action_manager().remove_all_action_from_target(m_sprBomb);
            Root.instance().action_manager().remove_all_action_from_target(m_sprBomb);

            m_sprBombCannon.set_visible(false);
            m_sprCannon.set_visible(true);
            m_sprBomb.set_visible(false);
            m_sprBombFuse.set_visible(false);

            Animation ani = Root.instance().animation_manager().animation("explorer");
            GameControls.XLBE.Action act = new Action_Sequence(new Action_Animation(0.06, ani, false), new Action_Func(FireBombEnd), null);
            m_sprExplorer.run_action(act);

            Point ptFish = new Point();
            Rect rcScreen = new Rect(0, 0, 1280, 738);
            CGameScene pGameScene = (CGameScene)parent();
            CMD_C_Bomb Bomb = new CMD_C_Bomb();
            Bomb.cbCount = 0;

            foreach ( Node j in pGameScene.m_layFishObject.childs())
            {
                if (Bomb.cbCount >= FishDefine.MAX_FISH_IN_NET_BOMB)
                    break;

                CFishObjectExtend pFishObjectExtend = (CFishObjectExtend)j.node_extend();

                if (pFishObjectExtend.wID == FishDefine.INVALID_WORD)
                    continue;

                ptFish = j.position();
                if (rcScreen.pt_in_rect(ptFish))
                {
                    Bomb.FishNetObjects[Bomb.cbCount].wID = pFishObjectExtend.wID;
                    Bomb.FishNetObjects[Bomb.cbCount].wRoundID = pFishObjectExtend.wRoundID;
                    Bomb.FishNetObjects[Bomb.cbCount].wType = (int)pFishObjectExtend.FishType;
                    Bomb.FishNetObjects[Bomb.cbCount].dwTime = pFishObjectExtend.dwTime;

                    Bomb.cbCount++;

                }
            }

            pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_BOMB, Bomb);

            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(21);
                pSound.play(false, true);
            }
            catch
            {
            }
        }
        public void FireLaserBean()
        {
            if (m_TaskDate.m_enType != CTaskDate.Type.TYPE_BEAN)
            {
                return;
            }
            m_TaskDate.m_enType = CTaskDate.Type.TYPE_NULL;
            m_TaskDate.m_enState = CTaskDate.State.STATE_NULL;

            m_sprLaserBean.set_visible(true);
            m_sprLaserBean.set_rotation(m_sprLaserBeanCannon.rotation());
            m_sprLaserBean.set_scale(new Size(2.0, 1.0));

            GameControls.XLBE.Action act = new Action_Sequence(new Action_Scale_To(0.3, 2.0, 6.0), new Action_Scale_To(0.8, 0.1, 6.0), new Action_Func(FireLaserBeanEnd), null);
            m_sprLaserBean.run_action(act);

            Point ptNet = new Point();
            Point ptTNet = new Point();
            Point ptFish = new Point();
            double sint;
            double cost;


            Rect rcScreen = new Rect(0, 0, 1280, 738);
            CGameScene pGameScene = (CGameScene)parent();
            CMD_C_Laser_Bean LaserBean = new CMD_C_Laser_Bean();
            LaserBean.cbCount = 0;
            LaserBean.fRote = m_sprLaserBeanCannon.rotation();

            ptNet = m_sprLaserBean.position_absolute();
            cost = Math.Cos(m_sprLaserBean.rotation());
            sint = Math.Sin(m_sprLaserBean.rotation());
            Rect rcNet = new Rect(new Point(0, 0), new Size(100 * 2, 1000 * 2));

            foreach ( Node j in pGameScene.m_layFishObject.childs())
            {
                if (LaserBean.cbCount >= FishDefine.MAX_FISH_IN_NET)
                    break;

                CFishObjectExtend pFishObjectExtend = (CFishObjectExtend)j.node_extend();

                if (pFishObjectExtend.wID == FishDefine.INVALID_WORD)
                    continue;

                ptFish = j.position();

                ptTNet.x_ = (ptFish.x_ - ptNet.x_) * cost + (ptFish.y_ - ptNet.y_) * sint;
                ptTNet.y_ = -(ptFish.x_ - ptNet.x_) * sint + (ptFish.y_ - ptNet.y_) * cost;

                if (CFishObjectExtend.ComputeCollision(150, 1500, 100, ptTNet.x_, ptTNet.y_))
                {
                    LaserBean.FishNetObjects[LaserBean.cbCount].wID = pFishObjectExtend.wID;
                    LaserBean.FishNetObjects[LaserBean.cbCount].wRoundID = pFishObjectExtend.wRoundID;
                    LaserBean.FishNetObjects[LaserBean.cbCount].wType = (int)pFishObjectExtend.FishType;
                    LaserBean.FishNetObjects[LaserBean.cbCount].dwTime = pFishObjectExtend.dwTime;

                    LaserBean.cbCount++;

                }
            }

            pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_LASER_BEAN, LaserBean );

            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(22);
                pSound.play(false, true);
            }
            catch
            {
            }
        }

        public void FireBonus()
        {
            m_TaskDate.m_enType = CTaskDate.Type.TYPE_NULL;
            m_TaskDate.m_enState = CTaskDate.State.STATE_NULL;

            CGameScene pGameScene = (CGameScene)parent();

            if (pGameScene.GetMeChairID() == m_wChairID)
            {
                CMD_C_Bonus Bonus = new CMD_C_Bonus();
                Bonus.nBonus = m_TaskDate.m_nBonus;

                pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_BONUS, Bonus);
            }
        }
        public void FireCook()
        {
            m_TaskDate.m_enType = CTaskDate.Type.TYPE_NULL;
            m_TaskDate.m_enState = CTaskDate.State.STATE_NULL;

            CGameScene pGameScene = (CGameScene)parent();

            if (pGameScene.GetMeChairID() == m_wChairID)
            {
                CMD_C_Cook Cook = new CMD_C_Cook();
                Cook.nBonus = m_TaskDate.m_nBonus;

                pGameScene.GetClientKernel().SendSocketData(FishDefine.SUB_C_COOK, Cook);
            }

            if (m_nCookFish >= m_TaskDate.m_nFishCount)
            {
                m_sprCookComplete.set_visible(false);
                m_sprCookFish.set_visible(false);
                m_sprCookGold.set_visible(false);

                m_sprCookSay.set_color(new Color_Rect());
                m_sprCookSay.set_display_image(Root.instance().imageset_manager().imageset("cook").image("say_0"));

                GameControls.XLBE.Action actCookSay = new Action_Sequence(new Action_Delay(1.6), new Action_Fade_Out(1.0), null);
                m_sprCookSay.run_action(actCookSay);

                m_sprCookSayBack.set_color(new Color_Rect());
                GameControls.XLBE.Action actCookSayBack = new Action_Sequence(new Action_Delay(1.6), new Action_Fade_Out(1.0), null);
                m_sprCookSayBack.run_action(actCookSayBack);

                m_sprCookWin.set_visible(true);
                Animation aniCookWin = Root.instance().animation_manager().animation("cook_win");
                GameControls.XLBE.Action actCookWin = new Action_Sequence(new Action_Animation(0.2, aniCookWin, false), new Action_Func(FireCookEnd), null);
                m_sprCookWin.run_action(actCookWin);
            }
            else
            {
                m_sprCookComplete.set_visible(false);
                m_sprCookFish.set_visible(false);
                m_sprCookGold.set_visible(false);

                m_sprCookSay.set_color(new Color_Rect());
                m_sprCookSay.set_display_image(Root.instance().imageset_manager().imageset("cook").image("say_3"));

                GameControls.XLBE.Action actCookSay = new Action_Sequence(new Action_Delay(1.6), new Action_Fade_Out(1.0), null);
                m_sprCookSay.run_action(actCookSay);

                m_sprCookSayBack.set_color(new Color_Rect());
                GameControls.XLBE.Action actCookSayBack = new Action_Sequence(new Action_Delay(1.6), new Action_Fade_Out(1.0), null);
                m_sprCookSayBack.run_action(actCookSayBack);

                m_sprCookLost.set_visible(true);
                Animation aniCookLost = Root.instance().animation_manager().animation("cook_lost");
                GameControls.XLBE.Action actCookLost = new Action_Sequence(new Action_Animation(0.2, aniCookLost, false), new Action_Delay(1.2), new Action_Func(FireCookEnd), null);
                m_sprCookLost.run_action(actCookLost);
            }

            m_nCookFish = 0;
        }
        public bool FireCookEnd(Node node, int tag)
        {
            m_sprCookSay.set_color(new Color_Rect());
            m_sprCookSay.set_visible(false);
            m_sprCookSayBack.set_color(new Color_Rect());
            m_sprCookSayBack.set_visible(false);
            m_sprCookLost.set_visible(false);
            m_sprCookLost.set_color(new Color_Rect());
            m_sprCookWin.set_visible(false);
            m_sprCookWin.set_color(new Color_Rect());
            m_sprCookNormal.set_color(new Color_Rect());
            m_sprCookNormal.set_visible(false);

            m_sprCookBack.set_visible(false);

            return true;
        }
        public void NetFireBomb()
        {
            m_TaskDate.m_enType = CTaskDate.Type.TYPE_NULL;
            m_TaskDate.m_enState = CTaskDate.State.STATE_NULL;

            m_sprExplorer.set_visible(true);
            m_sprExplorer.set_scale(new Size(3.0, 3.0));

            Root.instance().action_manager().remove_all_action_from_target(m_sprBomb);
            Root.instance().action_manager().remove_all_action_from_target(m_sprBomb);

            m_sprBombCannon.set_visible(false);
            m_sprCannon.set_visible(true);
            m_sprBomb.set_visible(false);
            m_sprBombFuse.set_visible(false);

            Animation ani = Root.instance().animation_manager().animation("explorer");
            GameControls.XLBE.Action act = new Action_Sequence(new Action_Animation(0.06, ani, false), new Action_Func(FireBombEnd), null);
            m_sprExplorer.run_action(act);

            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(21);
                pSound.play(false, true);
            }
            catch
            {
            }
        }

        public void NetFireLaserBean(double fRote)
        {
            m_TaskDate.m_enType = CTaskDate.Type.TYPE_NULL;
            m_TaskDate.m_enState = CTaskDate.State.STATE_NULL;

            m_sprCannon.set_rotation(fRote);
            m_sprLaserBeanCannon.set_rotation(fRote);
            m_sprLaserBean.set_visible(true);
            m_sprLaserBean.set_rotation(fRote);
            m_sprLaserBean.set_scale(new Size(2.0, 1.0));

            GameControls.XLBE.Action act = new Action_Sequence(new Action_Scale_To(0.3, 2.0, 6.0), new Action_Scale_To(0.8, 0.1, 6.0), new Action_Func(FireLaserBeanEnd), null);
            m_sprLaserBean.run_action(act);

            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(22);
                pSound.play(false, true);
            }
            catch
            {
            }
        }
        public void NetFireBouns()
        {
            m_TaskDate.m_enType = CTaskDate.Type.TYPE_NULL;
            m_TaskDate.m_enState = CTaskDate.State.STATE_NULL;

        }
        public void NetFireCook()
        {
            m_TaskDate.m_enType = CTaskDate.Type.TYPE_NULL;
            m_TaskDate.m_enState = CTaskDate.State.STATE_NULL;

            if (m_nCookFish >= m_TaskDate.m_nFishCount)
            {
                m_sprCookComplete.set_visible(false);
                m_sprCookFish.set_visible(false);
                m_sprCookGold.set_visible(false);

                m_sprCookSay.set_color(new Color_Rect());
                m_sprCookSay.set_display_image(Root.instance().imageset_manager().imageset("cook").image("say_0"));

                GameControls.XLBE.Action actCookSay = new Action_Sequence(new Action_Delay(1.6), new Action_Fade_Out(1.0), null);
                m_sprCookSay.run_action(actCookSay);

                m_sprCookSayBack.set_color(new Color_Rect());
                GameControls.XLBE.Action actCookSayBack = new Action_Sequence(new Action_Delay(1.6), new Action_Fade_Out(1.0), null);
                m_sprCookSayBack.run_action(actCookSayBack);

                m_sprCookWin.set_visible(true);
                Animation aniCookWin = Root.instance().animation_manager().animation("cook_win");
                GameControls.XLBE.Action actCookWin = new Action_Sequence(new Action_Animation(0.2, aniCookWin, false), new Action_Func(FireCookEnd), null);
                m_sprCookWin.run_action(actCookWin);
            }
            else
            {
                m_sprCookComplete.set_visible(false);
                m_sprCookFish.set_visible(false);
                m_sprCookGold.set_visible(false);

                m_sprCookSay.set_color(new Color_Rect());
                m_sprCookSay.set_display_image(Root.instance().imageset_manager().imageset("cook").image("say_3"));

                GameControls.XLBE.Action actCookSay = new Action_Sequence(new Action_Delay(1.6), new Action_Fade_Out(1.0), null);
                m_sprCookSay.run_action(actCookSay);

                m_sprCookSayBack.set_color(new Color_Rect());
                GameControls.XLBE.Action actCookSayBack = new Action_Sequence(new Action_Delay(1.6), new Action_Fade_Out(1.0), null);
                m_sprCookSayBack.run_action(actCookSayBack);

                m_sprCookLost.set_visible(true);
                Animation aniCookLost = Root.instance().animation_manager().animation("cook_lost");
                GameControls.XLBE.Action actCookLost = new Action_Sequence(new Action_Animation(0.2, aniCookLost, false), new Action_Delay(1.2), new Action_Func(FireCookEnd), null);
                m_sprCookLost.run_action(actCookLost);
            }

            m_nCookFish = 0;

        }
        public bool FireLaserBeanEnd(Node node, int tag)
        {
            m_sprLaserBean.set_visible(false);
            m_sprLaserBeanCannon.set_visible(false);

            return true;
        }
        public bool FireBombEnd(Node node, int tag)
        {
            m_sprExplorer.set_visible(false);
            return true;
        }

        public void ShowFishGoldGet() //金币柱动画
        {
	        int nGoldHeight = 4;
	        int nGoldChangHeight = 22;

	        Point ptTemp = new Point(0,0);

	        for(int i=0;i<FishDefine.MAX_SHOW_TIME_COUNT;i++)
	        {
		        //1.需要显示该金柱
		        if(m_FishGoldShow[i].bEnable > 0 )
		        {
			        //2.进入全面显示状态
			        if(m_FishGoldShow[i].nXPosMoveCount<6)
			        {
				        m_FishGoldShow[i].nXPosMoveCount++;
				        m_FishGoldShow[i].ptBaseOld.x_ += (m_FishGoldShow[i].ptBase.x_ - m_FishGoldShow[i].ptBaseOld.x_)/6;
			        }
			        else
			        {
				        m_FishGoldShow[i].ptBaseOld.x_ = m_FishGoldShow[i].ptBase.x_;
			        }

			        if(m_FishGoldShow[i].cbFishScoreCount >= m_FishGoldShow[i].dwFishScore)
			        {
				        int nCoinCount = m_FishGoldShow[i].dwFishGold;
				        //显示金币数
				        if(m_FishGoldShow[i].wTime==FishDefine.MAX_SHOW_STEP_TIME)
				        {
					        m_FishGoldShow[i].wTime = FishDefine.MAX_SHOW_STEP_TIME - m_FishGoldShow[i].nShowCount;

					        int nOffset;
					        string strPrex;
					        bool bGotHead = false;
					        int nSingleNumber = 0;
					        ostringstream ostr = new ostringstream();
					        nOffset = 14;
					        strPrex = "gold_number_";

					        int nOff = 6;
					        Point pt = m_FishGoldShow[i].ptChange;
					        //ostr = ostr + strPrex + 10;
					        //spValue[0] = new Sprite(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
				
					        nSingleNumber = (int)(nCoinCount/1000000);

					        if (nSingleNumber>0)
					        {
						        bGotHead = true;
						        ostr.str("");
						        ostr = ostr + strPrex + nSingleNumber;
						        m_spValue[i*8+1].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
						        pt.x_ += nOff;
					        }
					        else m_spValue[i*8+1].set_display_image(null);

					        nSingleNumber = (int)(nCoinCount%1000000/100000);
					        if (nSingleNumber>0 || bGotHead)
					        {
						        bGotHead = true;
						        ostr.str("");
						        ostr = ostr + strPrex + nSingleNumber;
						        m_spValue[i*8+2].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
						        pt.x_ += nOff;
					        }
					        else m_spValue[i*8+2].set_display_image(null);

					        nSingleNumber = (int)(nCoinCount%100000/10000);
					        if (nSingleNumber>0 || bGotHead)
					        {
						        bGotHead = true;
						        ostr.str("");
						        ostr = ostr + strPrex + nSingleNumber;
						        m_spValue[i*8+3].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
						        pt.x_ += nOff;
					        }
					        else m_spValue[i*8+3].set_display_image(null);

					        nSingleNumber = (int)(nCoinCount%10000/1000);
					        if (nSingleNumber>0 || bGotHead)
					        {
						        bGotHead = true;
						        ostr.str("");
						        ostr = ostr + strPrex + nSingleNumber;
						        m_spValue[i*8+4].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
						        pt.x_ += nOff;
					        }
					        else m_spValue[i*8+4].set_display_image(null);

					        nSingleNumber = (int)(nCoinCount%1000/100);
					        if (nSingleNumber>0 || bGotHead)
					        {
						        bGotHead = true;
						        ostr.str("");
						        ostr = ostr + strPrex + nSingleNumber;
						        m_spValue[i*8+5].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
						        pt.x_ += nOff;
					        }
					        else m_spValue[i*8+5].set_display_image(null);

					        nSingleNumber = (int)(nCoinCount%100/10);
					        if (nSingleNumber>0 || bGotHead)
					        {
						        bGotHead = true;
						        ostr.str("");
						        ostr = ostr + strPrex + nSingleNumber;
						        m_spValue[i*8+6].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
						        pt.x_ += nOff;
					        }
					        else m_spValue[i*8+6].set_display_image(null);

					        nSingleNumber = (int)(nCoinCount%10);
					        ostr.str("");
					        ostr = ostr + strPrex + nSingleNumber;
					        m_spValue[i*8+7].set_display_image(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
					        pt.x_ += nOff;
					
				        }

				        if(m_FishGoldShow[i].wTime>0)  m_FishGoldShow[i].wTime--;
				        if(m_FishGoldShow[i].wTime<=0) 
				        {
					        m_FishGoldShow[i].bEnable = 0; 
					        for(int j=0;j<FishDefine.MAX_SHOW_GOLDEN_COUNT;j++)
					        {
						        m_sprShow[i*FishDefine.MAX_SHOW_GOLDEN_COUNT+j].set_display_image(null);
					        }
					        for(int j=0;j<8;j++)
					        {
						        m_spValue[i*8+j].set_display_image(null);
					        }

					        Point ptBase0 = new Point(-120+(0)*42,36);
					        Point ptBase1 = new Point(-120+(1)*42,36);
					        Point ptBase2 = new Point(-120+(2)*42,36);
					        Point ptBase3 = new Point(-120+(3)*42,36);

					        //移动 1--
					        int nMax0 = -1,nIndex0 = -1;
					        for( int k=0;k<FishDefine.MAX_SHOW_TIME_COUNT;k++) 
					        {
						        if(m_FishGoldShow[k].bEnable==1)
						        {
							        if(m_FishGoldShow[k].nIndex>nIndex0) {nIndex0=m_FishGoldShow[k].nIndex; nMax0 = k;}
						        }
					        }
					        if(nIndex0>=0)
					        {
						        m_FishGoldShow[nMax0].ptBaseOld = m_FishGoldShow[nMax0].ptBase;
						        m_FishGoldShow[nMax0].nXPosMove=0;
						        m_FishGoldShow[nMax0].nXPosMoveCount=0;
						        m_FishGoldShow[nMax0].ptBase = ptBase3;
						        m_FishGoldShow[nMax0].nIndex = 3;
					        }

					        //移动 2--
					        int nMax1 = -1,nIndex1 = -1;
					        for( int k=0;k<FishDefine.MAX_SHOW_TIME_COUNT;k++) 
					        {
						        if(k==nMax0) continue;
						        if(m_FishGoldShow[k].bEnable==1)
						        {
							        if(m_FishGoldShow[k].nIndex>nIndex1) {nIndex1=m_FishGoldShow[k].nIndex; nMax1 = k;}
						        }
					        }
					        if(nIndex1>=0)
					        {
						        m_FishGoldShow[nMax1].ptBaseOld = m_FishGoldShow[nMax1].ptBase;
						        m_FishGoldShow[nMax1].nXPosMove=0;
						        m_FishGoldShow[nMax1].nXPosMoveCount=0;
						        m_FishGoldShow[nMax1].ptBase = ptBase2;
						        m_FishGoldShow[nMax1].nIndex = 2;
					        }

					        //移动 3--
					        int nMax2 = -1,nIndex2 = -1;
					        for(int k=0;k<FishDefine.MAX_SHOW_TIME_COUNT;k++) 
					        {
						        if(k==nMax0) continue;
						        if(k==nMax1) continue;
						        if(m_FishGoldShow[k].bEnable==1)
						        {
							        if(m_FishGoldShow[k].nIndex>nIndex2) {nIndex2=m_FishGoldShow[k].nIndex; nMax2 = k;}
						        }
					        }
					        if(nIndex2>=0)
					        {
						        m_FishGoldShow[nMax2].ptBaseOld = m_FishGoldShow[nMax2].ptBase;
						        m_FishGoldShow[nMax2].nXPosMove=0;
						        m_FishGoldShow[nMax2].nXPosMoveCount=0;
						        m_FishGoldShow[nMax2].ptBase = ptBase1;
						        m_FishGoldShow[nMax2].nIndex = 1;
					        }
				        }
			        }
			        else
			        {
				        //3.显示下面的金柱
				        if(m_FishGoldShow[i].cbFishScoreCount < (m_FishGoldShow[i].dwFishScore-1))
				        {
					        int nStep = (m_FishGoldShow[i].dwFishScore-1)/5 + 1;
					        int nLoop = (m_FishGoldShow[i].cbFishScoreCount < (m_FishGoldShow[i].dwFishScore-1) - nStep) ? nStep : 1;

					        if((m_FishGoldShow[i].cbFishScoreCount==0)&&(m_FishGoldShow[i].dwFishScore>=20)) nLoop = 20;
					        else if(m_FishGoldShow[i].dwFishScore-1-m_FishGoldShow[i].cbFishScoreCount > 3)  nLoop = (nLoop==1) ? 2 : nLoop;

					        for(int k=0;k<nLoop;k++)
					        {
						        int nIndex = m_FishGoldShow[i].cbFishScoreCount+k;
						        m_FishGoldShow[i].ptChange = new Point(0,nIndex*nGoldHeight);
						        m_sprShow[i*FishDefine.MAX_SHOW_GOLDEN_COUNT+nIndex].set_display_image(Root.instance().imageset_manager().imageset("task").image("coin_show_5"));
						        //m_sprShow[i*FishDefine.MAX_SHOW_GOLDEN_COUNT+nIndex].set_position(m_FishGoldShow[i].ptBase-m_FishGoldShow[i].ptChange);
					        }
					        m_FishGoldShow[i].cbFishScoreCount += nLoop;
					        m_FishGoldShow[i].nShowCount++;
				        }
				        else
				        {
					        //3.显示正在翻转的金币
					        Point ptChang = m_FishGoldShow[i].ptChange;
					        ostringstream ostr = new ostringstream();

					        if(m_FishGoldShow[i].cbGoldTurnCount>=5)//3.1 指向下一个翻转的金币
					        {
						        m_FishGoldShow[i].cbGoldTurnCount=0;
						        m_FishGoldShow[i].wTimeChange=FishDefine.MAX_CHANGE_STEP_TIME;

						        ostr.str("");
						        ostr = ostr + "coin_show_5";
						        ptChang.y_ += nGoldHeight;

						        m_FishGoldShow[i].cbFishScoreCount++;
					        }
					        else//3.1 显示翻转的金币
					        {
						        ostr.str("");
						        ostr = ostr + "coin_show_" + (int) m_FishGoldShow[i].cbGoldTurnCount;

						        m_FishGoldShow[i].cbGoldTurnCount++;
						        ptChang.y_ += nGoldChangHeight - m_FishGoldShow[i].cbGoldTurnCount*4;
						        m_FishGoldShow[i].nShowCount++;
					        }

					        m_sprShow[i*FishDefine.MAX_SHOW_GOLDEN_COUNT+m_FishGoldShow[i].dwFishScore-1].set_display_image(Root.instance().imageset_manager().imageset("task").image(ostr.str()));
					        //m_sprShow[i*FishDefine.MAX_SHOW_GOLDEN_COUNT+m_FishGoldShow[i].dwFishScore].set_position(m_FishGoldShow[i].ptBase-ptChang);
				        }
			        }

			        //移动
			        if(m_FishGoldShow[i].cbFishScoreCount >= m_FishGoldShow[i].dwFishScore)
			        {
				        for(int k=0;k<m_FishGoldShow[i].dwFishScore;k++)
				        {
					        m_FishGoldShow[i].ptChange = new Point(0,k*nGoldHeight);
					        m_sprShow[i*FishDefine.MAX_SHOW_GOLDEN_COUNT+k].set_position(m_FishGoldShow[i].ptBaseOld-new Point(0,k*nGoldHeight));
				        }

				        int nCoinCount = m_FishGoldShow[i].dwFishGold;
				        //显示金币数
				        Point pt = new Point(0,(m_FishGoldShow[i].dwFishScore-1)*nGoldHeight);

				        int nOffSet = 6;
				        if(nCoinCount<10) pt.x_ += nOffSet + 12*5;
				        else if(nCoinCount<100) pt.x_ += nOffSet*2 + 12*4;
				        else if(nCoinCount<1000) pt.x_ += nOffSet*3 + 12*3;
				        else if(nCoinCount<10000) pt.x_ += nOffSet*4 + 12*2;
				        else if(nCoinCount<100000) pt.x_ += nOffSet*5 + 12*1;
				        else if(nCoinCount<1000000) pt.x_ += nOffSet*6 + 12*0;
				        else if(nCoinCount<10000000) pt.x_ += nOffSet*7 - 12;

				        pt.x_ += 18;
				        pt.y_ += 18;
				        for (int y=1; y<8; y++)
				        {
					        pt.x_ -= 12;
					        m_spValue[i*8+y].stop_all_action();
					        m_spValue[i*8+y].set_position(m_FishGoldShow[i].ptBaseOld - pt);
					        GameControls.XLBE.Action act = new Action_Sequence(new Action_Scale_To(0.01, 0.5), null);
					        m_spValue[i*8+y].run_action(act);   
				        }
			        }
			        else
			        {
				        //3.显示下面的金柱
				        for(int k=0;k<(m_FishGoldShow[i].dwFishScore-1);k++)
				        {
					        m_sprShow[i*FishDefine.MAX_SHOW_GOLDEN_COUNT+k].set_position(m_FishGoldShow[i].ptBaseOld-new Point(0,k*nGoldHeight));
				        }

				        ////3.显示正在翻转的金币
				        Point ptChang = new Point(0,(m_FishGoldShow[i].dwFishScore-2)*nGoldHeight);
				        ostringstream ostr;

				        if(m_FishGoldShow[i].cbGoldTurnCount>=5)//3.1 指向下一个翻转的金币
				        {
					        ptChang.y_ += nGoldHeight;
				        }
				        else//3.1 显示翻转的金币
				        {
					        ptChang.y_ += nGoldChangHeight - m_FishGoldShow[i].cbGoldTurnCount*4;
				        }

				        m_sprShow[i*FishDefine.MAX_SHOW_GOLDEN_COUNT+m_FishGoldShow[i].dwFishScore-1].set_position(m_FishGoldShow[i].ptBaseOld-ptChang);
			        }
		        }
	        }
        }

        public bool ConnonLevelUpEnd(Node node, int tag)
        {
            m_sprConnonLevelUp.set_visible(false);
            return true;
        }
        public void ClearUp()
        {
            m_dwStartTime = 0;

            m_sprBarEnough.set_visible(false);
            m_sprRealFly.set_visible(false);
            m_sprReelBack.set_visible(false);
            for (int i = 0; i < 3; i++)
            {
                m_sprReelNormal[i].set_visible(false);
            }

            for (int i = 0; i < 3; i++)
            {
                m_sprReelFast[i].set_visible(false);
            }

            m_sprReelEff.set_visible(false);

            for (int i = 0; i < 4; i++)
            {
                m_sprTaskName[i].set_visible(false);
            }

            m_sprChangeEffect.set_visible(false);

            m_sprLaserBeanCannon.set_visible(false);
            m_sprLaserBean.set_visible(false);

            m_sprBombCannon.set_visible(false);
            m_sprBomb.set_visible(false);
            m_sprBombFuse.set_visible(false);
            m_sprExplorer.set_visible(false);

            m_sprReward.set_visible(false);

            m_sprCookBack.set_visible(false);
            m_sprCookNormal.set_visible(false);
            m_sprCookLost.set_visible(false);
            m_sprCookWin.set_visible(false);
            m_sprCookSay.set_visible(false);
            m_sprCookFish.set_visible(false);
            m_sprCookSayBack.set_visible(false);
            m_sprCookComplete.set_visible(false);
            m_sprCookGold.set_visible(false);

            int m_nFireCount = 0;
            m_bDelayBarEnough = false;
            m_bShowRewardNubmer = false;
            m_nRewardNumber = 0;
            m_nCookFish = 0;
            m_dwCookStartTime = 0;

            m_TaskDate.m_enType = CTaskDate.Type.TYPE_NULL;
        }
        public bool ValueEnd(Node node, int tag)
        {

            remove_child(node);
            node = null;

            return true;
        }

        public void ShowMessageBox(int wChair, int nLen, string cbData)
        {
            if (nLen > 0)
            {
                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
                m_cbShowData = cbData;

                m_sprMessage.set_display_image(Root.instance().imageset_manager().imageset("ui_load").image("message_show_image"));
                m_sprMessage.set_visible(true);

                CGameScene pGameScene = (CGameScene)parent();

                pGameScene.m_TimerShowMessage[wChair].reset();
            }
        }

        public void SetFireCountDelay(bool bDelay) 
        { 
            m_bDelayBarEnough = bDelay; 
        }

        public void AddCookFish(int nFishType)
        {
            if (m_TaskDate.m_enType == CTaskDate.Type.TYPE_COOK && nFishType == m_TaskDate.m_nFishType && m_TaskDate.m_enState == CTaskDate.State.STATE_RUNNING)
            {
                m_nCookFish++;
                CGameScene pGameScene = (CGameScene)parent();
                if ((pGameScene != null && pGameScene.GetMeChairID() == m_wChairID) && (m_nCookFish >= m_TaskDate.m_nFishCount && pGameScene != null))
                {
                    FireCook();
                }
            }
        }

        public void ConnonLevelUp()
        {
            m_sprConnonLevelUp.set_display_image(Root.instance().imageset_manager().imageset("connon_level").image("0"));

            m_sprConnonLevelUp.set_visible(true);
            Animation ani = Root.instance().animation_manager().animation("connon_level");
            GameControls.XLBE.Action act = new Action_Sequence(new Action_Animation(0.1, ani, true), new Action_Func(ConnonLevelUpEnd), null);
            m_sprConnonLevelUp.run_action(act);

        }

        public Point GetCannonPosition() 
        { 
            return m_sprCannon.position_absolute(); 
        }

        public int GetFireCount() 
        { 
            return m_nFireCount; 
        }

        public int GetMaxMulRate() 
        { 
            return m_dwMaxMulRate; 
        }										//获取炮弹倍率

        public int GetFishGold() 
        { 
            return m_dwFishGold; 
        }

	    public int GetExpValue() 
        { 
            return m_dwLevel; 
        }

        public int GetChairID() 
        { 
            return m_wChairID; 
        }

	    public void  SetExpValue(int dwLevel) 
        { 
            m_dwLevel = dwLevel; 
        }

	    public FishDefine.enCannonType GetConnonType() 
        { 
            return m_CannonType; 
        }

        public int GetMulRate() 
        { 
            return m_dwMulRate; 
        }										//获取炮弹倍率

        public double GetCannonRataion() 
        { 
            return m_sprCannon.rotation(); 
        }


    }
}
