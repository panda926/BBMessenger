using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    //C++ TO C# CONVERTER TODO TASK: Multiple inheritance is not available in C#:
    public class CGameScene : Scene, Input_Listener
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	CGameScene();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        //network
        public virtual void ResetGameFrame()
        {
        }
        public virtual void CloseGameFrame()
        {
        }
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool OnEventSocket(TCP_Command Command, object pBuffer, int wDataSize);
        //public virtual void OnEventUserMemberOrder(UserInfo pUserData, int wChairID, bool bLookonUser)
        //{
        //}
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void OnEventUserStatus(UserInfo pUserData, int wChairID, bool bLookonUser);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool OnEventUserLeave(UserInfo pUserData, int wChairID, bool bLookonUser);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool OnEventUserEnter(UserInfo pUserData, int wChairID, bool bLookonUserr);
        //public virtual void OnEventUserScore(UserInfo pUserData, int wChairID, bool bLookonUser)
        //{
        //}
        //frame
        public virtual void window_moved(Render_Window rw)
        {
        }
        public virtual void window_focus_change(Render_Window rw)
        {
        }
        public virtual void window_resized(Render_Window rw)
        {
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void enter();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void exit();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool keyPressed(OIS::KeyEvent arg);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool keyReleased(OIS::KeyEvent arg);

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool mouseMoved(OIS::MouseEvent arg);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool mousePressed(OIS::MouseEvent arg, OIS::MouseButtonID id);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual bool mouseReleased(OIS::MouseEvent arg, OIS::MouseButtonID id);

        public bool IsAndroidLogicChairID()
        {
            return m_bAndroidLogicChairID;
        }
        public CClientKernel GetClientKernel()
        {
            return m_pClientKernel;
        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	void MatchOver();

        private bool m_bIsConnect;
        private CClientKernel m_pClientKernel;

        public CBackgroundLayer m_layBackground;
        public CFishLayer m_layFishObject;
        public CNetLayer m_layNetObject;
        public CBulletLayer m_layBulletObject;
        public CCoinLayer m_layCoinObject;
        public CRoleLayer[] m_layRoles;
        public CMeUserInfoLayer m_layMeUserInfo;
        public CBuyBulletLayer m_layBuyBulletLayer;
        public CAccountLayer m_layAccount;
        public CFrameLayer m_layFrame;
        public CCursorLayer m_layCursor;
        public CSettingLayer m_laySetting;
        public CMessageLayer m_sendMessage;
        public CHelpLayer m_Help;
        public CMatchStartLayer m_MatchStart;
        public CReMatchStartLayer m_ReMatchStart;
        public CMatchTimeLayer m_MatchTimeLayer;
        public CMatchAgainLayer m_MatchAgainLayer;
        public CGameEndLayer m_GateEndLayer;
        public CGateHelpLayer m_GateHelpLayer;

        public Timer m_TimerChange = new Timer();
        public Timer m_Timer = new Timer();
        public Timer m_TimerShowUserInf = new Timer();
        public Timer[] m_TimerShowRole = Arrays.InitializeWithDefaultInstances<Timer>(FishDefine.MAX_SHOW_TIME_COUNT);

        public bool m_bFirstGame;
        public bool m_bFireBullet;
        public bool m_bAndroidLogicChairID;
        public bool m_bIsCheckVer;

        public int m_nMouseNotMoveCount;

        public bool m_bIsShowMessageBox;
        public bool m_bIsLeftKeyDowning;
        public Timer m_TimerKey = new Timer();
        public Timer[] m_TimerShowMessage = Arrays.InitializeWithDefaultInstances<Timer>(FishDefine.GAME_PLAYER);

        public bool m_bMatchStartFlag;
        public bool m_bIsSysExit;

        public int m_nMatchHour;
        public int m_nMatchMinute;
        public int m_nMatchSecond;

        public bool m_bIsMatchExit;
        public int m_dwRoomType; //房间类型
        public int m_dwMatchScoreBase;
        public string m_cbMatchIndexURL = new string(new char[128]);

        public int m_nGateCount;
        public int m_nCurGateCount;
        public int m_nFishScoreBase;

        public bool m_bGameTimeEnd;

        public int m_nShootCount;

        public GateCtrlInf[] m_tagGateCtrlInf = Arrays.InitializeWithDefaultInstances<GateCtrlInf>(FishDefine.MAX_GATE_COUNT);

        public CGameScene( FishView fishView )
        {
            this.m_layBackground = null;
            this.m_layFishObject = null;
            this.m_layRoles = new CRoleLayer[FishDefine.GAME_PLAYER];
            //////////////////////////////////////////////////////////////////////
            m_pClientKernel = new CClientKernel();
            m_pClientKernel.fishView_ = fishView;

            Root.instance().input_manager().add_input_listener(this);

            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
            {
                m_TimerShowMessage[i].reset();
            }
            m_TimerKey.reset();
            m_Timer.reset();
            for (int i = 0; i < FishDefine.MAX_SHOW_TIME_COUNT; i++)
            {
                m_TimerShowRole[i].reset();
            }

            /////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////
            m_layBackground = new CBackgroundLayer();
            m_layBackground.set_position(new Point(4, 31));
            m_layBackground.set_content_size(new Size(1272, 703));
            m_layBackground.set_disable(true);
            add_child(m_layBackground);

            m_layFishObject = new CFishLayer();
            m_layFishObject.set_disable(true);
            m_layFishObject.set_content_size(new Size(1280, 738));
            add_child(m_layFishObject);

            m_layNetObject = new CNetLayer();
            m_layNetObject.set_disable(true);
            m_layNetObject.set_content_size(new Size(1280, 738));
            add_child(m_layNetObject);

            m_layBulletObject = new CBulletLayer();
            m_layBulletObject.set_disable(true);
            m_layBulletObject.set_content_size(new Size(1280, 738));
            add_child(m_layBulletObject);

            m_layCoinObject = new CCoinLayer();
            m_layCoinObject.set_disable(true);
            m_layCoinObject.set_content_size(new Size(1280, 738));
            add_child(m_layCoinObject);

            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
            {
                m_layRoles[i] = new CRoleLayer();
                m_layRoles[i].set_disable(true);
                m_layRoles[i].set_content_size(new Size(300, 96));
                add_child(m_layRoles[i]);

                m_layRoles[i].set_visible(false);
            }

            m_layRoles[0].set_position(new Point(168, 638 - 8));
            m_layRoles[1].set_position(new Point(160 + 426 * 2, 638 - 8));
            m_layRoles[2].set_position(new Point(160 + 426, 638 - 8));
            //m_layRoles[3]->set_position(new Point(-1000,-1000));

            m_layMeUserInfo = new CMeUserInfoLayer();
            m_layMeUserInfo.set_content_size(new Size(253, 173));
            m_layMeUserInfo.add_widget(this);
            m_layMeUserInfo.set_disable(true);
            m_layMeUserInfo.resize(new Point(-1000, -1000), new Size(253, 173));
            m_TimerShowUserInf.reset();

            m_layBuyBulletLayer = new CBuyBulletLayer();
            m_layBuyBulletLayer.set_position(new Point(388, 257));
            m_layBuyBulletLayer.set_content_size(new Size(503, 223));
            m_layBuyBulletLayer.add_widget(this);
            m_layBuyBulletLayer.ShowWidnow(false);

            m_layAccount = new CAccountLayer();
            m_layAccount.set_content_size(new Size(787, 456));
            m_layAccount.add_widget(this);
            m_layAccount.resize(new Point(246, 140), new Size(787, 456));
            m_layAccount.ShowWidnow(false);

            m_laySetting = new CSettingLayer();
            m_laySetting.set_content_size(new Size(503, 265));
            m_laySetting.add_widget(this);
            m_laySetting.resize(new Point(388, 140), new Size(503, 265));
            m_laySetting.ShowWidnow(false);
            m_laySetting.set_value(0.3);

            m_layFrame = new CFrameLayer(1);
            m_layFrame.set_position(new Point(0, 0));
            m_layFrame.set_content_size(new Size(1280, 738));
            m_layFrame.set_disable(true);
            m_layFrame.add_widget(this);

            m_sendMessage = new CMessageLayer();
            m_sendMessage.set_content_size(new Size(253, 173));
            m_sendMessage.add_widget(this);
            m_sendMessage.set_disable(true);
            m_sendMessage.resize(new Point(100, 100), new Size(286, 30));
            m_sendMessage.Show(false);

            m_Help = new CHelpLayer();
            m_Help.set_content_size(new Size(253, 173));
            m_Help.add_widget(this);
            m_Help.set_disable(true);
            m_Help.resize(new Point(529, 200), new Size(222, 342));
            m_Help.Show(false);

            m_MatchStart = new CMatchStartLayer();
            m_MatchStart.set_content_size(new Size(253, 173));
            m_MatchStart.add_widget(this);
            m_MatchStart.set_disable(true);
            m_MatchStart.resize(new Point(458, 260), new Size(365, 216));
            m_MatchStart.Show(false);

            m_ReMatchStart = new CReMatchStartLayer();
            m_ReMatchStart.set_content_size(new Size(490, 198));
            m_ReMatchStart.add_widget(this);
            m_ReMatchStart.set_disable(true);
            m_ReMatchStart.resize(new Point(396, 200), new Size(490, 198));
            m_ReMatchStart.Show(false);

            m_MatchAgainLayer = new CMatchAgainLayer();
            m_MatchAgainLayer.set_content_size(new Size(574, 362));
            m_MatchAgainLayer.add_widget(this);
            m_MatchAgainLayer.set_disable(true);
            m_MatchAgainLayer.resize(new Point(366, 200), new Size(574, 362));
            m_MatchAgainLayer.Show(false);

            m_GateEndLayer = new CGameEndLayer();
            m_GateEndLayer.set_content_size(new Size(566, 202));
            m_GateEndLayer.add_widget(this);
            m_GateEndLayer.set_disable(true);
            m_GateEndLayer.resize(new Point(366, 200), new Size(566, 202));
            m_GateEndLayer.Show(false);

            m_GateHelpLayer = new CGateHelpLayer();
            m_GateHelpLayer.set_content_size(new Size(566, 202));
            m_GateHelpLayer.add_widget(this);
            m_GateHelpLayer.set_disable(true);
            m_GateHelpLayer.resize(new Point(366, 200), new Size(566, 233));
            m_GateHelpLayer.Show(false);

            m_MatchTimeLayer = new CMatchTimeLayer();
            m_MatchTimeLayer.set_content_size(new Size(180, 72));
            m_MatchTimeLayer.add_widget(this);
            m_MatchTimeLayer.set_disable(true);
            m_MatchTimeLayer.resize(new Point(8, 38), new Size(180, 72));
            m_MatchTimeLayer.Show(false);

            m_layCursor = new CCursorLayer();
            m_layCursor.set_position(new Point(0, 0));
            m_layCursor.set_content_size(new Size(80, 80));
            m_layFrame.set_disable(true);
            m_layCursor.SetCursor(Root.instance().imageset_manager().imageset("role").image("cursor_fire"));
            add_child(m_layCursor);

            m_bFirstGame = true;
            m_Timer.reset();
            m_TimerShowUserInf.reset();

            m_bFireBullet = false;
            m_bAndroidLogicChairID = false;

            m_bIsShowMessageBox = false;

            m_nMouseNotMoveCount = 0;

            m_bMatchStartFlag = false;
            m_bIsCheckVer = false;
            m_dwRoomType = 0;

            m_bIsMatchExit = false;
            m_cbMatchIndexURL = "http://www.game516.com/ph.htm";

            m_nMatchHour = 0;
            m_nMatchMinute = 0;
            m_nMatchSecond = 0;

            m_bIsSysExit = false;
            m_bGameTimeEnd = false;

            m_nShootCount = 0;

        }

        public void enter()
        {
            base.enter();

            System.IntPtr hWnd = Root.instance().render_window().window_handle();
            m_bIsConnect = true; // m_pClientKernel.InitClientKernel(hWnd, g_MultiByteToWideChar((string)theApp.cmd_line()).c_str(), this);
        }

        public void window_closed(Render_Window rw)
        {
            if (m_pClientKernel != null)
            {
                m_pClientKernel.WindowClosed();

                m_bIsConnect = false;

                m_pClientKernel = null;
                m_pClientKernel = null;
            }
        }

        public bool OnEventUserLeave(UserInfo pUserData, int wChairID, bool bLookonUser)
        {
            m_layRoles[wChairID].SetChairID(GameDefine.INVALID_CHAIR);
            m_layRoles[wChairID].SetFishGold(0);
            m_layRoles[wChairID].SetCannonType(FishDefine.enCannonType.CannonTypeCount, 0);

            m_layRoles[wChairID].set_visible(false);

            m_layRoles[wChairID].ClearUp();

            return true;
        }
        public bool OnEventUserEnter(UserInfo pUserData, int wChairID, bool bLookonUserr)
        {
            int wMeChair = GetMeChairID();
            int nPosX = wMeChair == 0 ? 196 : wMeChair == 1 ? 196 + 800 : 196 + 426;
            m_layMeUserInfo.m_ptDown = new Point(nPosX, -116);
            m_layMeUserInfo.m_ptUp = new Point(nPosX, 32);
            m_layMeUserInfo.resize(m_layMeUserInfo.m_ptUp, new Size(253, 173));
            m_TimerShowUserInf.reset();

            m_layRoles[wChairID].SetChairID(wChairID);
            m_layRoles[wChairID].SetChairPos(wChairID);
            m_layRoles[wChairID].SetFishGold(0);
            m_layRoles[wChairID].SetCannonType(FishDefine.enCannonType.CannonType_5, FishDefine.BASE_MUL_RATE);
            //m_layRoles[wChairID]->SetFireCount(0);
            m_layRoles[wChairID].SetMulRate(FishDefine.BASE_MUL_RATE, 50);

            m_layRoles[wChairID].set_visible(true);

            //if (wChairID == GetMeChairID())
            //    m_layRoles[wChairID]->ShowGunLead(true);

            return true;
        }

        public bool OnEventGameScene(byte cbGameStation, bool bLookonOther, FishInfo pStatusFree )
        {
            //FILE *pFile = fopen( "D:\\test.txt", "a+" );
            //fprintf(pFile,"\r\n-------------------[%d]----------------------\r\n",GetMeChairID());
            //fclose(pFile);

            //FishInfo pStatusFree = (FishInfo)pBuffer;

            m_layBackground.m_cbSceneSound = pStatusFree.m_cbSceneSound;
            m_layBackground.SetSceneType(FishDefine.ByteToSceneType(pStatusFree.m_cbScene));
            m_layBuyBulletLayer.SetCellScore(pStatusFree.m_nCellScore);
            m_layBuyBulletLayer.SetMaxFishGold(pStatusFree.m_nScoreMaxBuy);

            //设置房间类型
            m_dwRoomType = (int)pStatusFree.m_dwRoomType;

            m_layFrame.button_press(10010);

            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
            {
                if (i == GetMeChairID())
                    continue;

                m_layRoles[i].SetChairID(pStatusFree.m_RoleObjects[i].wID);
                m_layRoles[i].SetChairPos(pStatusFree.m_RoleObjects[i].wID);
                m_layRoles[i].SetFishGold(pStatusFree.m_RoleObjects[i].dwFishGold);
                m_layRoles[i].SetMulRate(pStatusFree.m_RoleObjects[i].dwMulRate, pStatusFree.m_RoleObjects[i].dwMaxMulRate);
                m_layRoles[i].SetCannonType(pStatusFree.m_RoleObjects[i].dwMulRate >= 100 ? FishDefine.enCannonType.CannonType_6 : FishDefine.enCannonType.CannonType_5, pStatusFree.m_RoleObjects[i].dwMulRate);
                m_layRoles[i].SetFireCount(pStatusFree.m_RoleObjects[i].dwExpValue);

                if (m_layRoles[i].GetChairID() != GameDefine.INVALID_CHAIR)
                {
                    m_layRoles[i].set_visible(true);
                }
                else
                {
                    m_layRoles[i].set_visible(false);
                }
            }


            m_layRoles[GetMeChairID()].ShowBarAnimation();

            if (m_bIsCheckVer == false)
            {
                m_bIsCheckVer = true;
                //if(m_pClientKernel->GetFileVersion(::GetModuleHandle(NULL))!=(char *)pStatusFree->cbVer) 
                //{
                //	ShowWindow(Root::instance()->render_window()->window_handle(), SW_HIDE);
                //	MessageBox(0,"版本更换，将重启游戏，请重新登录，以便更新游戏！","温馨提示",0);
                //	//window_closed(null);
                //	Root::instance()->queue_end_rendering();

                //	//m_pClientKernel->RestartMainWindow("");
                //}
                //else 
                if ((pStatusFree.m_dwRoomType == 1) || (pStatusFree.m_dwRoomType == 2)) //当前房间是比赛房间
                {
                    if (pStatusFree.m_cbMatchNot == 1)
                    {
                        //pakcj ShowWindow(Root.instance().render_window().window_handle(), SW_HIDE);
                        System.Windows.Forms.MessageBox.Show("未到比赛时间，具体比赛时间安排请关注网站！", "温馨提示");
                        window_closed(null);
                        Root.instance().queue_end_rendering();
                    }
                    else
                    {
                        /*if(strlen((char *)pStatusFree->cbIPList) > 5)
                            m_pClientKernel->GetPubIP();
                        if(strstr((char *)pStatusFree->cbIPList,(char *)m_pClientKernel->m_cbPubIP)==NULL)
                        {
                            ShowWindow(Root::instance()->render_window()->window_handle(), SW_HIDE);
                            MessageBox(0,_T("比赛地点不正确，具体比赛地点安排请关注网站！"),_T("温馨提示"),0);
                            window_closed(null);
                            Root::instance()->queue_end_rendering();		
	
                        }*/
                    }
                }
                //比赛设置
                if ((m_dwRoomType == 1) || (m_dwRoomType == 2) || (m_dwRoomType == 3))
                {
                    //pakcj m_cbMatchIndexURL = (string)pStatusFree.cbURL;
                    m_MatchStart.Show(true);
                    if (pStatusFree.m_dwRoomType == 2)
                    {
                        m_MatchStart.SetMatchGold(pStatusFree.m_dwMatchScore);
                    }
                    else
                    {
                        m_MatchStart.SetMatchGold(0);
                    }
                    m_MatchStart.button_press(10302); //按比赛类型更换背景
                }
            }

            return true;
        }

        public bool OnEventSocket(int Command, object pBuffer, int wDataSize)
        {
            if (Command == FishDefine.SUB_S_SEND_LINE_PATH_SMALL_BOTTOM_FISH)
            {
                return OnSubGameAddLinePathSmallBottomFish(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_SEND_LINE_PATH_FISH)
            {
                return OnSubGameAddLinePathFish(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_SEND_POINT_PATH_FISH)
            {
                return OnSubGameAddPointPathFish(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_BUY_BULLET_SUCCESS)
            {
                return OnSubGameBuyBulletSuccess(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_BUY_BULLET_FAILED)
            {
                return OnSubGameBuyBulletFailed(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_FIRE_SUCCESS)
            {
                return OnSubGameFireSuccess(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_FIRE_FAILED)
            {
                return OnSubGameFireFailed(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_CHANGE_CANNON)
            {
                return OnSubGameChangeCannon(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_CAST_NET_SUCCESS)
            {
                return OnSubGameCastNetSuccess(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_CAST_NET_FAILED)
            {
                return OnSubGameCastNetFailed(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_CHANGE_SCENE)
            {
                return OnSubGameChangeScene(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_ACCOUNT)
            {
                return OnSubGameAccount(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_SEND_GROUP_POINT_PATH_FISH)
            {
                return OnSubGameAddGroupPointPathFish(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_SEND_SPECIAL_POINT_PATH)
            {
                return OnSubGameAddSpecialPointPathFish(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_TASK)
            {
                return OnSubGameTask(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_LASER_BEAN_SUCCESS)
            {
                return OnSubGameLaserBeanSucess(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_BOMB_SUCCESS)
            {
                return OnSubGameBombSucess(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_BONUS)
            {
                return OnSubBonus(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_COOK)
            {
                return OnSubGameCook(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_BOMB_FISH)
            {
                return OnSubBombFish(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_SEND_MESSAGE)
            {
                return OnSubSendMessage(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_CANNON_RUN)
            {
                return OnSubCannonRun(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_MATCH_START)
            {
                return OnSubMatchStart(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_MATCH_INDEX)
            {
                return OnSubMatchIndex(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_MATCH_TIME_SEND)
            {
                return OnSubMatchTimeSend(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_SMALL_FISH_GROUP)
            {
                return OnSubSmallFishGroup(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_GATE_CTRL_SEND)
            {
                return OnSubGateCtrlSend(pBuffer, wDataSize);
            }
            else if (Command == FishDefine.SUB_S_MATCH_OVER)
            {
                return OnSubMatchOver(pBuffer, wDataSize);
            }
            return true;
        }

        public bool OnSubGameAddLinePathSmallBottomFish(object pBuffer, int wDataSize)
        {
            CMD_S_Send_Line_Path_Fish pSendLinePathFish = (CMD_S_Send_Line_Path_Fish)pBuffer;

            m_layFishObject.NetAddLinePathSmallBottomFish(pSendLinePathFish);

            return true;
        }
        public bool OnSubGameAddLinePathFish(object pBuffer, int wDataSize)
        {
            //if (wDataSize!=sizeof(SendLinePathFish)-(MAX_FISH_SEND-SendLinePathFish.cbCount)*sizeof(Fish_With_Line_Path)) return true;

            CMD_S_Send_Line_Path_Fish pSendLinePathFish = (CMD_S_Send_Line_Path_Fish)pBuffer;

            m_layFishObject.NetAddLinePathFish(pSendLinePathFish);

            return true;
        }
        public bool OnSubGameAddSpecialPointPathFish(object pBuffer, int wDataSize)
        {
            CMD_S_Send_Special_Point_Path pSendSpecialPointPathFish = (CMD_S_Send_Special_Point_Path)pBuffer;

            m_layFishObject.NetAddSpecialPointPathFish(pSendSpecialPointPathFish);

            return true;
        }
        public bool OnSubGameTask(object pBuffer, int wDataSize)
        {
            CMD_S_Task pTask = (CMD_S_Task)pBuffer;

            int wChairID = pTask.wChairID;

            m_layRoles[wChairID].CommandTaskStart(pTask);

            if (pTask.nTask != (int)CTaskDate.Type.TYPE_NULL)
            {
                //m_layRoles[wChairID]->SetFireCountDelay(true);
                //m_layRoles[wChairID]->SetFireCount(0);
            }

            return true;
        }
        public bool OnSubGameAddPointPathFish(object pBuffer, int wDataSize)
        {
            CMD_S_Send_Point_Path_Fish pSendPointPathFish = (CMD_S_Send_Point_Path_Fish)pBuffer;

            m_layFishObject.NetAddPointPathFish(pSendPointPathFish);

            return true;
        }

        public bool OnSubGameAddGroupPointPathFish(object pBuffer, int wDataSize)
        {
            CMD_S_Send_Group_Point_Path_Fish pSendGroupPointPathFish = (CMD_S_Send_Group_Point_Path_Fish)pBuffer;

            m_layFishObject.NetAddGroupPointPathFish(pSendGroupPointPathFish);

            return true;
        }
        public bool OnSubGameBuyBulletSuccess(object pBuffer, int wDataSize)
        {
            if (!(pBuffer is CMD_S_Buy_Bullet_Success))
            {
                return true;
            }

            CMD_S_Buy_Bullet_Success pBuyBulletSuccess = (CMD_S_Buy_Bullet_Success)pBuffer;

            int wChairID = pBuyBulletSuccess.wChairID;
            int dwCount = pBuyBulletSuccess.dwCount;

            if (wChairID == GetMeChairID())
            {
                m_layBuyBulletLayer.SetSendBuyBulletMessage(false);
                if (m_bFirstGame)
                {
                    m_layRoles[wChairID].ShowPlayLead(false); //true
                    m_layRoles[wChairID].ShowBuyLead(false);
                }
            }

            m_layRoles[wChairID].ShowFishGoldEmpty(false);
            m_layRoles[wChairID].SetFishGold(m_layRoles[wChairID].GetFishGold() + dwCount);

            UserInfo userInfo = m_pClientKernel.GetMeUserInfo();
            userInfo.SetGameMoney(userInfo.GetGameMoney() - dwCount);

            return true;
        }
        public bool OnSubGameBuyBulletFailed(object pBuffer, int wDataSize)
        {
            if (!(pBuffer is CMD_S_Buy_Bullet_Failed))
            {
                return true;
            }

            CMD_S_Buy_Bullet_Failed pBuyBulletFailed = (CMD_S_Buy_Bullet_Failed)pBuffer;

            int wChairID = pBuyBulletFailed.wChairID;

            if (wChairID == GetMeChairID())
            {
                m_layBuyBulletLayer.SetSendBuyBulletMessage(false);
            }

            return true;
        }

        public bool OnSubGameFireSuccess(object pBuffer, int wDataSize)
        {
            //if (wDataSize != sizeof(CMD_S_Fire_Success))
            //{
            //    return true;
            //}

            CMD_S_Fire_Success pFireSuccess = (CMD_S_Fire_Success)pBuffer;

            int wChairID = pFireSuccess.wChairID;
            double fRote = pFireSuccess.fRote;

            //m_bAndroidLogicChairID = (pFireSuccess->wAndroidLogicChairID == GetMeChairID());

            //FILE *pFile = fopen( "D:\\test.txt", "a+" );
            //fprintf(pFile,"----[%d](%d)  %d %d-------\r\n",GetMeChairID(),wChairID,pFireSuccess->cbIsBack,pFireSuccess->dwMulRate);
            //fclose(pFile);

            {
                //if (wChairID != GetMeChairID())
                double fRoteChange = fRote;

                if (pFireSuccess.cbIsBack > 0 )
                {
                    m_layBulletObject.BulletFireBack(new Point(pFireSuccess.xStart, pFireSuccess.yStart), fRoteChange, wChairID, m_layRoles[wChairID].GetConnonType(), 1000, pFireSuccess.dwMulRate);
                }
                else
                {
                    m_layBulletObject.BulletFire(m_layRoles[wChairID].GetCannonPosition(), fRoteChange, wChairID, m_layRoles[wChairID].GetConnonType(), pFireSuccess.dwMulRate);

                    int dwFishGold = (pFireSuccess.dwMulRate > 1000000) ? pFireSuccess.dwMulRate - 1000000 : pFireSuccess.dwMulRate; //m_layRoles[wChairID]->GetConnonType()+1;

                    if (dwFishGold > m_layRoles[wChairID].GetFishGold())
                    {
                        dwFishGold = 0;
                        m_layRoles[wChairID].ShowFishGoldEmpty(true);
                    }
                    else
                    {
                        dwFishGold = m_layRoles[wChairID].GetFishGold() - dwFishGold;
                        m_layRoles[wChairID].ShowFishGoldEmpty(false);
                    }

                    m_layRoles[wChairID].SetFishGold(dwFishGold); //edit by guojm
                    m_layRoles[wChairID].SetCannonRatation(fRoteChange);
                    m_layRoles[wChairID].GunAnimation();
                    m_layRoles[wChairID].GunBaseAnimation();

                    if (wChairID == GetMeChairID())
                    {
                        if ((m_dwRoomType == 3) && (dwFishGold == 0))
                        {
                            m_MatchStart.Show(true);
                        }
                    }
                }
            }

            return true;
        }
        public bool OnSubGameFireFailed(object pBuffer, int wDataSize)
        {
            //if (wDataSize != sizeof(CMD_S_Fire_Failed))
            //{
            //    return true;
            //}

            //MessageBox(0,"OnSubGameFireFailed","OnSubGameFireFailed",0);

            return true;
        }
        public bool OnSubGameChangeCannon(object pBuffer, int wDataSize)
        {
            CMD_S_Change_Cannon pChangeCannon = (CMD_S_Change_Cannon)pBuffer;

            int wChairID = pChangeCannon.wChairID;
            int wStyle = pChangeCannon.wStyle;
            int dwMulRate = pChangeCannon.dwMulRate;
            int dwMaxMulRate = pChangeCannon.dwMaxMulRate;
            int dwExpValue = pChangeCannon.dwExpValue;
            //DWORD dwLevel = ((pChangeCannon->dwExpValue/EXP_CHANGE_TO_LEVEL) >= MAX_CANNON_LEVEL) ? MAX_CANNON_LEVEL - 1 : (pChangeCannon->dwExpValue/EXP_CHANGE_TO_LEVEL);

            //   if (wChairID != GetMeChairID())
            //   {
            //	m_layRoles[wChairID]->SetCannonType(CGameCore::WordToCannonType(wStyle),dwMulRate);
            //}
            //FILE *pFile = fopen( "D:\\test.txt", "a+" );
            //fprintf(pFile,"\r\n-------------------[%d]----------------------\r\n",dwMulRate);
            //fclose(pFile);

            m_layRoles[wChairID].SetFireCount(dwExpValue);
            m_layRoles[wChairID].SetExpValue(pChangeCannon.dwLevel);
            m_layRoles[wChairID].SetMulRate(dwMulRate, dwMaxMulRate);
            m_layRoles[wChairID].SetCannonType(dwMulRate >= 100 ? FishDefine.enCannonType.CannonType_6 : FishDefine.enCannonType.CannonType_5, dwMulRate);

            return true;
        }

        public bool OnSubGameCastNetSuccess(object pBuffer, int wDataSize)
        {
            CMD_S_Cast_Net_Success pCastNetSuccess = (CMD_S_Cast_Net_Success)pBuffer;

            //if (wDataSize != sizeof(CMD_S_Cast_Net_Success) - (FishDefine.MAX_FISH_IN_NET - pCastNetSuccess.cbCount) * sizeof(Fish_Net_Object))
            //{
            //    return true;
            //}

            int cbCount = pCastNetSuccess.cbCount;
            int wChairID = pCastNetSuccess.wChairID;
            int wFishID;

            //先计数获得渔币
            int nCoinCount = 0;
            int nFishTime = 0;

            int bHaveBigFish = 0;
            for (byte i = 0; i < cbCount; i++)
            {
                wFishID = pCastNetSuccess.FishNetObjects[i].wID;

                if (m_layFishObject.FishCapturedScore(wFishID) >= 30)
                {
                    bHaveBigFish = 1;
                }

                nFishTime += m_layFishObject.FishGoldByStyle(pCastNetSuccess.FishNetObjects[i].wType, pCastNetSuccess.FishNetObjects[i].wRoundID);
                nCoinCount += (int)(m_layFishObject.FishGoldByStyle(pCastNetSuccess.FishNetObjects[i].wType, (int)(pCastNetSuccess.FishNetObjects[i].wRoundID)) * pCastNetSuccess.FishNetObjects[i].dwTime);
            }

            if (nCoinCount > 0)
            {
                m_layRoles[wChairID].UpdateGold(nCoinCount, nFishTime);
                if (bHaveBigFish != 0)
                {
                    m_layRoles[wChairID].RewardAnimation(nCoinCount, true);
                }
            }

            //再显示鱼死亡动画
            for (byte i = 0; i < cbCount; i++)
            {
                wFishID = pCastNetSuccess.FishNetObjects[i].wID;

                m_layFishObject.FishCaptured(wFishID, wChairID, (int)pCastNetSuccess.FishNetObjects[i].dwTime);
            }

            m_layRoles[wChairID].SetExpValue(pCastNetSuccess.dwLevel);
            m_layRoles[wChairID].SetFireCount(pCastNetSuccess.dwExpValue);

            if (pCastNetSuccess.cbLevelUp == 1)
            {
                m_layRoles[wChairID].ConnonLevelUp();
                m_layMeUserInfo.button_press(10102);
                m_TimerShowUserInf.reset();
            }

            return true;
        }


        public bool OnSubGameLaserBeanSucess(object pBuffer, int wDataSize)
        {
            CMD_S_Laser_Bean_Success pLaserBeanuccess = (CMD_S_Laser_Bean_Success)pBuffer;

            //if (wDataSize != sizeof(CMD_S_Laser_Bean_Success) - (FishDefine.MAX_FISH_IN_NET - pLaserBeanuccess.cbCount) * sizeof(Fish_Net_Object))
            //{
            //    return true;
            //}

            int cbCount = pLaserBeanuccess.cbCount;
            int wChairID = pLaserBeanuccess.wChairID;
            int wFishID;

            int nCoinCount = 0;

            if (GetMeChairID() != pLaserBeanuccess.wChairID)
            {
                m_layRoles[wChairID].NetFireLaserBean(pLaserBeanuccess.fRote);
            }

            for (byte i = 0; i < cbCount; i++)
            {
                wFishID = pLaserBeanuccess.FishNetObjects[i].wID;

                nCoinCount += m_layFishObject.FishCaptured(wFishID, wChairID, m_layRoles[wChairID].GetMulRate());
            }


            if (nCoinCount > 0)
            {
                m_layRoles[wChairID].UpdateGold(nCoinCount, 0);
                m_layRoles[wChairID].RewardAnimation(nCoinCount, true);
            }

            return true;
        }
        public bool OnSubGameBombSucess(object pBuffer, int wDataSize)
        {
            CMD_S_Bomb_Success pBombSuccess = (CMD_S_Bomb_Success)pBuffer;

            //if (wDataSize!=sizeof(CMD_S_Bomb_Success)-(FishDefine.MAX_FISH_IN_NET-pBombSuccess->cbCount)*sizeof(Fish_Net_Object)) return true;

            int cbCount = pBombSuccess.cbCount;
            int wChairID = pBombSuccess.wChairID;
            int wFishID;

            int nCoinCount = 0;

            if (GetMeChairID() != pBombSuccess.wChairID)
            {
                m_layRoles[wChairID].NetFireBomb();
            }

            for (byte i = 0; i < cbCount; i++)
            {
                wFishID = pBombSuccess.FishNetObjects[i].wID;

                m_layFishObject.FishCaptured(wFishID, wChairID, m_layRoles[wChairID].GetMulRate());
                nCoinCount += (int)(m_layFishObject.FishGoldByStyle(pBombSuccess.FishNetObjects[i].wType, wFishID) * pBombSuccess.FishNetObjects[i].dwTime);
            }

            if (nCoinCount > 0)
            {
                m_layRoles[wChairID].UpdateGold(nCoinCount, FishDefine.MAX_SHOW_GOLDEN_COUNT);

                m_layRoles[wChairID].RewardAnimation(nCoinCount, true);
                if (pBombSuccess.cbLevelUp == 1)
                {
                    m_layRoles[wChairID].ConnonLevelUp();
                }
            }

            return true;
        }
        public bool OnSubBonus(object pBuffer, int wDataSize)
        {
            if (!(pBuffer is CMD_S_Bonus))
            {
                return true;
            }

            CMD_S_Bonus pBonus = (CMD_S_Bonus)pBuffer;

            if (GetMeChairID() != pBonus.wChairID)
            {
                m_layRoles[pBonus.wChairID].NetFireBouns();
            }

            m_layRoles[pBonus.wChairID].UpdateGold(pBonus.nBonus, FishDefine.MAX_SHOW_GOLDEN_COUNT);
            m_layRoles[pBonus.wChairID].RewardAnimation(pBonus.nBonus, false);

            return true;
        }

        public bool OnSubGameCook(object pBuffer, int wDataSize)
        {
            //if (wDataSize != sizeof(CMD_S_Cook))
            //{
            //    return true;
            //}

            CMD_S_Cook pCook = (CMD_S_Cook)pBuffer;

            if (GetMeChairID() != pCook.wChairID)
            {
                m_layRoles[pCook.wChairID].NetFireCook();
            }

            if (pCook.cbSucess > 0 )
            {
                m_layRoles[pCook.wChairID].UpdateGold(pCook.nBonus, FishDefine.MAX_SHOW_GOLDEN_COUNT);
                m_layRoles[pCook.wChairID].RewardAnimation(pCook.nBonus, true);
            }

            return true;
        }
        public bool OnSubBombFish(object pBuffer, int wDataSize)
        {
            CMD_S_Bomb_Fish pBombFish = (CMD_S_Bomb_Fish)pBuffer;

            if (GetMeChairID() == pBombFish.wChairID)
            {
                m_layRoles[GetMeChairID()].FireBombFish(pBombFish.lBigStock, pBombFish.lSmallStock, pBombFish.dwMulRate);
                m_bFireBullet = false;
            }

            return true;

        }
        public bool OnSubSendMessage(object pBuffer, int wDataSize)
        {
            CMD_S_Send_Message pSendMessage = (CMD_S_Send_Message)pBuffer;

            m_layRoles[pSendMessage.wChair].ShowMessageBox(pSendMessage.wChair, pSendMessage.nLen, pSendMessage.cbData);
            if (pSendMessage.wChair == GetMeChairID())
            {
                m_sendMessage.button_press(10200);
            }
            return true;
        }
        public bool OnSubCannonRun(object pBuffer, int wDataSize)
        {
            CMD_S_Connon_Run pConnonRun = (CMD_S_Connon_Run)pBuffer;

            if (GetMeChairID() != pConnonRun.wChair)
            {
                m_layRoles[pConnonRun.wChair].SetCannonRatation(pConnonRun.fRote);
            }

            return true;
        }
        public bool OnSubMatchStart(object pBuffer, int wDataSize)
        {
            m_bIsMatchExit = false;

            CMD_S_Match_Start pMatchStart = (CMD_S_Match_Start)pBuffer;

            if ((GetMeChairID() == pMatchStart.wChair) && (pMatchStart.dwScore > 0))
            {
                m_bMatchStartFlag = true;
                m_dwMatchScoreBase = pMatchStart.dwScore;
                m_MatchStart.Show(false);
            }

            m_layRoles[pMatchStart.wChair].SetFishGold(pMatchStart.dwScore);

            return true;
        }

        public bool OnSubMatchIndex(object pBuffer, int wDataSize)
        {
            CMD_S_Match_Index pMatchStart = (CMD_S_Match_Index)pBuffer;

            if (GetMeChairID() == pMatchStart.wChair)
            {
                m_bMatchStartFlag = false;
                m_layRoles[pMatchStart.wChair].m_dwMatchIndex = pMatchStart.dwIndex;

                if (m_bIsMatchExit)
                {
                    if (m_bIsSysExit == false)
                    {
                        m_bIsSysExit = true;
                        m_bIsMatchExit = false;
                        if (m_bGameTimeEnd)
                        {
                            //pakcj ShellExecute(null, "open", m_cbMatchIndexURL, null, null, SW_SHOWNORMAL);
                            //pakcj Sleep(1000);
                        }

                        window_closed(null);
                        Root.instance().queue_end_rendering();
                    }
                }
                else
                {
                    if (m_layRoles[GetMeChairID()].m_dwMatchIndex == 1)
                    {
                        m_ReMatchStart.m_sprResult.set_display_image(Root.instance().imageset_manager().imageset("ui_load").image("match_result_gxnl"));
                    }
                    else if (m_layRoles[GetMeChairID()].m_dwMatchScore < m_dwMatchScoreBase)
                    {
                        m_ReMatchStart.m_sprResult.set_display_image(Root.instance().imageset_manager().imageset("ui_load").image("match_result_fhyb"));
                    }
                    else if ((m_layRoles[GetMeChairID()].m_dwMatchScore >= m_dwMatchScoreBase) && (m_layRoles[GetMeChairID()].m_dwMatchScore <= 2 * m_dwMatchScoreBase))
                    {
                        m_ReMatchStart.m_sprResult.set_display_image(Root.instance().imageset_manager().imageset("ui_load").image("match_result_jxnl"));
                    }
                    else
                    {
                        m_ReMatchStart.m_sprResult.set_display_image(Root.instance().imageset_manager().imageset("ui_load").image("match_result_ddbc"));
                    }

                    m_ReMatchStart.Show(true);
                }

                //m_bMatchStartFlag = false;
                //m_layRoles[pMatchStart->wChair]->m_dwMatchIndex = pMatchStart->dwIndex;
                //m_layRoles[pMatchStart->wChair]->m_dwMatchScore = pMatchStart->dwMatchScore;
                //m_ReMatchStart->Show(true);
            }
            else
            {
                m_layRoles[pMatchStart.wChair].SetFishGold(0);
            }

            return true;
        }
        public bool OnSubMatchTimeSend(object pBuffer, int wDataSize)
        {
            CMD_S_Match_Time_Send pMatchTimeSend = (CMD_S_Match_Time_Send)pBuffer;

            if ((pMatchTimeSend.nHour <= 0) && (pMatchTimeSend.nMinute <= 0) && (pMatchTimeSend.nSecond <= 0))
            {
                if (m_bMatchStartFlag)
                {
                    m_bGameTimeEnd = true;
                    m_bIsMatchExit = true;
                    m_bMatchStartFlag = true;
                    MatchOver();
                }

                //m_bMatchStartFlag = false;
                //window_closed(null);
                //Root::instance()->queue_end_rendering();		

                //ShellExecute(NULL, "open",m_cbMatchIndexURL,NULL,NULL,SW_SHOWNORMAL);
            }
            else
            {
                m_nMatchHour = pMatchTimeSend.nHour;
                m_nMatchMinute = pMatchTimeSend.nMinute;
                m_nMatchSecond = pMatchTimeSend.nSecond;
                m_MatchTimeLayer.Show(true);

                if ((m_nMatchHour == 0) && (m_nMatchMinute == 0))
                {
                    if (m_nMatchSecond <= 10) //警报
                    {
                        //try
                        //{
                        //	Sound_Instance *pSound = Root::instance()->sound_manager()->sound_instance(17);
                        //	pSound->play(false, true);
                        //}
                        //catch(...)
                        //{
                        //}
                    }
                }
            }

            return true;
        }
        public bool OnSubSmallFishGroup(object pBuffer, int wDataSize)
        {
            //if (wDataSize != sizeof(CMD_S_Small_Fish_Group))
            //{
            //    return true;
            //}

            CMD_S_Small_Fish_Group pSmallFishGroup = (CMD_S_Small_Fish_Group)pBuffer;

            //if ((GetMeChairID() == pSmallFishGroup.wChair) && (pSmallFishGroup.nTime > 2))
            if ((pSmallFishGroup.nTime > 2))
            {
                m_layFishObject.NetAddLinePathSmallFishGroup(pSmallFishGroup);
            }

            return true;
        }
        public bool OnSubGateCtrlSend(object pBuffer, int wDataSize)
        {
            //if (wDataSize != sizeof(CMD_S_Gate_Ctrl_Send))
            //{
            //    return true;
            //}

            CMD_S_Gate_Ctrl_Send pGateCtrlSend = (CMD_S_Gate_Ctrl_Send)pBuffer;

            if (GetMeChairID() == pGateCtrlSend.wChair)
            {
                if (pGateCtrlSend.cbFirst == 0)
                {
                    m_MatchAgainLayer.Show(false);
                }

                m_nGateCount = pGateCtrlSend.nGateCount;
                m_nCurGateCount = pGateCtrlSend.nCurGateCount;
                m_nFishScoreBase = pGateCtrlSend.nFishScoreBase;

                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
                Array.Clear( m_tagGateCtrlInf, 0, m_tagGateCtrlInf.Length );
            }

            if ((pGateCtrlSend.cbFirst == 0) || (pGateCtrlSend.cbFirst == 1))
            {
                m_layRoles[pGateCtrlSend.wChair].SetFishGold(pGateCtrlSend.nFishScoreBase);
            }

            return true;
        }
        public bool OnSubMatchOver(object pBuffer, int wDataSize)
        {
            CMD_S_Match_Over pMatchOver = (CMD_S_Match_Over)pBuffer;

            //if((GetMeChairID()==pMatchOver->wChair)&&(pMatchOver->lMatchScore==(pMatchOver->lMatchScoreCheck-888888)))
            //{
            //	CMD_GF_WriteMatchScore WriteMatchScore;
            //	WriteMatchScore.lMatchScore=pMatchOver->lMatchScore; 
            //	GetClientKernel()->SendWriteMatchScore(&WriteMatchScore,sizeof(CMD_GF_WriteMatchScore));

            //	m_layRoles[GetMeChairID()]->m_dwMatchScore = pMatchOver->lMatchScore;
            //	m_layRoles[GetMeChairID()]->SetFishGold(0);
            //}

            return true;
        }
        public bool OnSubGameCastNetFailed(object pBuffer, int wDataSize)
        {
            return true;
        }
        public bool OnSubGameChangeScene(object pBuffer, int wDataSize)
        {
            CMD_S_Change_Scene pChangeScene = (CMD_S_Change_Scene)pBuffer;

            m_layFishObject.SpeedUpFishObject(5.0);
            m_layBackground.m_cbSceneSound = pChangeScene.cbSceneSound;
            m_layBackground.ChangeSceneType(FishDefine.ByteToSceneType(pChangeScene.cbScene));

            return true;
        }
        public bool OnSubGameAccount(object pBuffer, int wDataSize)
        {
            CMD_S_Account pAccount = (CMD_S_Account)pBuffer;
            if (pAccount.wChairID == GetMeChairID())
            {
                m_layAccount.DisableWindow(false);
                m_layAccount.ShowWidnow(false);
            }

            UserInfo userInfo = m_pClientKernel.GetMeUserInfo();
            userInfo.SetGameMoney(userInfo.GetGameMoney() + m_layRoles[pAccount.wChairID].GetFishGold());

            m_layRoles[pAccount.wChairID].SetFishGold(0);

            return true;

        }

        public int GetMeChairID()
        {
            return m_pClientKernel.GetMeChairID();
        }

        public bool keyPressed(KeyEvent arg)
        {
            //FILE *pFile = fopen("D:\\test.txt","w+");
            //fprintf(pFile,"ddddd");
            //fclose(pFile);

            return true;
        }
        public bool keyReleased(KeyEvent arg)
        {
            m_nShootCount = 0;
            return true;
        }

        public bool mouseMoved(MouseEvent arg)
        {
            int nFullWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int nFullHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            Point ptOffset1 = new Point(arg.X, arg.Y);

            if (nFullWidth < 1280)
            {
                ptOffset1.x_ = arg.X * (1280.0 / 1024.0);
                ptOffset1.y_ = arg.Y * (738.0 / 590.0);
            }

            m_nMouseNotMoveCount = 0;

            Point pt = new Point(ptOffset1);

            m_layCursor.set_position(pt);

            bool bHandle = false;
            Point ptUI = m_layBuyBulletLayer.position();
            Size szUI = m_layBuyBulletLayer.content_size();
            Rect rcUI = new Rect(ptUI, szUI);
            if (m_layBuyBulletLayer.visible() && rcUI.pt_in_rect(pt))
            {
                bHandle = true;
                m_layCursor.SetCursor(Root.instance().imageset_manager().imageset("role").image("cursor_hand"));
            }

            rcUI.origin_ = m_layAccount.position();
            rcUI.size_ = m_layAccount.content_size();
            if (m_layAccount.visible() && rcUI.pt_in_rect(pt))
            {
                bHandle = true;
                m_layCursor.SetCursor(Root.instance().imageset_manager().imageset("role").image("cursor_hand"));
            }

            rcUI.origin_ = m_layMeUserInfo.position();
            rcUI.size_ = m_layMeUserInfo.content_size();
            if (m_layMeUserInfo.visible() && rcUI.pt_in_rect(pt))
            {
                bHandle = true;
                m_layCursor.SetCursor(Root.instance().imageset_manager().imageset("role").image("cursor_hand"));
            }

            rcUI.origin_ = m_sendMessage.position();
            rcUI.size_ = m_sendMessage.content_size();
            if (m_sendMessage.visible() && rcUI.pt_in_rect(pt))
            {
                bHandle = true;
                m_layCursor.SetCursor(Root.instance().imageset_manager().imageset("role").image("cursor_hand"));
            }

            rcUI.origin_ = m_laySetting.position();
            rcUI.size_ = m_laySetting.content_size();
            if (m_laySetting.visible() && rcUI.pt_in_rect(pt))
            {
                bHandle = true;
                m_layCursor.SetCursor(Root.instance().imageset_manager().imageset("role").image("cursor_hand"));
            }

            if (!bHandle)
            {
                m_layCursor.SetCursor(Root.instance().imageset_manager().imageset("role").image("cursor_fire"));
            }

            if (GetMeChairID() != GameDefine.INVALID_CHAIR)
            {
                m_layRoles[GetMeChairID()].SetCannonRatation(pt);
            }

            Rect rc = new Rect();
            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
            {
                if (GetMeChairID() != i)
                {
                    rc.origin_ = m_layRoles[i].position();
                    rc.size_ = m_layRoles[i].content_size();
                    rc.origin_.x_ += 50;
                    rc.size_.width_ -= 100;

                    if (rc.pt_in_rect(pt))
                    {
                        m_layRoles[i].ShowUserInfo(true);
                    }
                    else
                    {
                        m_layRoles[i].ShowUserInfo(false);
                    }
                }

            }

            //return __super::mouseMoved(arg1);

            if (mouse_down_)
            {
                mouse_drag(new Point(pt.x_, pt.y_));

                return true;
            }

            mouse_position(new Point(pt.x_, pt.y_));

            return true;
        }

        public bool mousePressed(MouseEvent arg, MouseButtonID id)
        {
            int nFullWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int nFullHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            Point ptOffset1 = new Point(arg.X, arg.Y);

            m_bIsLeftKeyDowning = false;

            if (nFullWidth < 1280)
            {
                ptOffset1.x_ = arg.X * (1280.0 / 1024.0);
                ptOffset1.y_ = arg.Y * (738.0 / 590.0);
            }

            Point pt = new Point(ptOffset1);
            Rect rcScreen = new Rect(4, 31, 1272, 703);

            Point pos = new Point();
            Node node = node_by_position(pt, pos);

            m_bFireBullet = false;
            if (rcScreen.pt_in_rect(pt) && node == null)
            {
                if (id == 0)
                {
                    //if(!(m_MatchStart->visible()||m_ReMatchStart->visible()||m_MatchAgainLayer->visible())) m_Help->Show(true);
                    //if(m_sendMessage->visible()) ::SetFocus(GetClientKernel()->m_hWndEdit);
                }
                else if ((int)id == 1)
                {
                    {
                        //if(m_dwRoomType==0)
                        m_bIsLeftKeyDowning = true;
                        int wMeChairID = GetMeChairID();
                        if (wMeChairID != GameDefine.INVALID_CHAIR && ((m_layRoles[wMeChairID].m_TaskDate.m_enState < CTaskDate.State.STATE_PREPARE2) || ((m_layRoles[wMeChairID].m_TaskDate.m_enType == CTaskDate.Type.TYPE_COOK && m_layRoles[wMeChairID].m_TaskDate.m_enState == CTaskDate.State.STATE_RUNNING))))
                        {
                            try
                            {
                                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(17);
                                pSound.play(false, true);
                            }
                            catch
                            {
                            }
                            //m_layRoles[wMeChairID]->SetCannonType(CGameCore::WordToCannonType((m_layRoles[wMeChairID]->GetConnonType()+1)%CGameCore::CannonTypeCount));
                            m_layRoles[wMeChairID].SetCannonType(FishDefine.WordToCannonType((int)m_layRoles[wMeChairID].GetConnonType()), m_layRoles[wMeChairID].GetMulRate() + 50);
                        }
                    }
                }
            }


            //return __super::mousePressed(arg1, id);


            Point mouse = new Point(ptOffset1);

            mouse_position(mouse);

            mouse_down_ = true;

            node = node_by_position(mouse, pos);

            last_down_node_ = node;

            if (node != null)
            {
                if (((Layer)(node)).wants_focus())
                {
                    set_focus(node);
                }

                ((Layer)(node)).mouse_down(pos, 0, 0);
            }

            return true;
        }
        public bool mouseReleased(MouseEvent arg, MouseButtonID id)
        {
            m_bIsLeftKeyDowning = false;

            int nFullWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int nFullHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            Point ptOffset1 = new Point(arg.X, arg.Y);

            m_bFireBullet = false;

            if (nFullWidth < 1280)
            {
                ptOffset1.x_ = arg.X * (1280.0 / 1024.0);
                ptOffset1.y_ = arg.Y * (738.0 / 590.0);
            }

            Point pos = new Point();
            Point mouse = new Point(ptOffset1);

            mouse_down_ = false;

            if (last_down_node_!= null)
            {
                Node node = last_down_node_;
                ((Layer)(node)).mouse_up(new Point(mouse - node.position()), 0, 0);
            }

            mouse_position(mouse);

            return true;
        }

        public override void update(double dt)
        {
            base.update(dt);

            if (GetMeChairID() == GameDefine.INVALID_CHAIR)
                return;

            if ((m_dwRoomType == 1) || (m_dwRoomType == 2))
            {
                if (m_bIsMatchExit)
                    return;
            }

            if (m_nMouseNotMoveCount > 200)
            {
                m_layCursor.SetCursor(Root.instance().imageset_manager().imageset("role").image("cursor_none"));
            }
            else
            {
                m_nMouseNotMoveCount++;
            }

            int lShow = m_TimerShowRole[0].get_milli_seconds();
            if (lShow > 16)
            {
                m_layRoles[0].ShowFishGoldGet();
                m_TimerShowRole[0].reset();
            }

            lShow = m_TimerShowRole[1].get_milli_seconds();
            if (lShow > 18)
            {
                m_layRoles[1].ShowFishGoldGet();
                m_TimerShowRole[1].reset();
            }

            lShow = m_TimerShowRole[2].get_milli_seconds();
            if (lShow > 20)
            {
                m_layRoles[2].ShowFishGoldGet();
                m_TimerShowRole[2].reset();
            }

            lShow = m_TimerShowRole[3].get_milli_seconds();
            if (lShow > 22)
            {
                //m_layRoles[3]->ShowFishGoldGet();
                //m_TimerShowRole[3].reset();
            }

            int lCloseUserInf = m_TimerShowUserInf.get_milli_seconds();
            if (lCloseUserInf > 10 * 1000)
            {
                m_layMeUserInfo.button_press(10103);
                m_TimerShowUserInf.reset();
            }

            int time = m_Timer.get_milli_seconds();
            if (time > 60 * 1000 && !m_layRoles[GetMeChairID()].IsShowWaringTime())
            {
                m_layRoles[GetMeChairID()].ShowWaringTime(true);
                m_Timer.reset();
            }

            MouseState ms = Root.instance().input_manager().mouse().getMouseState();

            int nFullWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int nFullHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            Point ptOffset1 = new Point(ms.X, ms.Y);
            Point ptOffset2 = new Point(ms.X, ms.Y);


            if (nFullWidth < 1280)
            {
                ptOffset1.x_ = ms.X * (1280.0 / 1024.0);
                ptOffset1.y_ = ms.Y * (738.0 / 590.0);
            }

            Point pt = new Point(ptOffset1);
            Rect rcScreen = new Rect(4, 31, 1272, 703);

            if (m_bIsShowMessageBox == false)
            {

                if (Root.instance().input_manager().keyboard().isKeyDown((int)System.Windows.Forms.Keys.A) || Root.instance().input_manager().keyboard().isKeyDown((int)System.Windows.Forms.Keys.Left))
                {
                    time = m_TimerChange.get_milli_seconds();
                    if (time < 20)
                        return;

                    m_TimerChange.reset();

                    int wMeChair = GetMeChairID();
                    if (wMeChair != GameDefine.INVALID_CHAIR)
                    {
                        double fRatation = m_layRoles[GetMeChairID()].GetCannonRataion() - 0.1;
                        if (fRatation <= -1.57)
                        {
                            fRatation = -1.57F;
                        }
                        m_layRoles[GetMeChairID()].SetCannonRatation(fRatation);
                        //发送到服务器
                        CMD_C_Connon_Run ConnonRun = new CMD_C_Connon_Run();
                        ConnonRun.wChair = GetMeChairID();
                        ConnonRun.fRote = fRatation;
                        GetClientKernel().SendSocketData(FishDefine.SUB_C_CANNON_RUN, ConnonRun );
                    }
                }
                else if (Root.instance().input_manager().keyboard().isKeyDown((int)System.Windows.Forms.Keys.D) || Root.instance().input_manager().keyboard().isKeyDown((int)System.Windows.Forms.Keys.Right))
                {
                    time = m_TimerChange.get_milli_seconds();
                    if (time < 20)
                        return;

                    m_TimerChange.reset();

                    int wMeChair = GetMeChairID();
                    if (wMeChair != GameDefine.INVALID_CHAIR)
                    {
                        double fRatation = m_layRoles[GetMeChairID()].GetCannonRataion() + 0.1;
                        if (fRatation > 1.57)
                        {
                            fRatation = 1.57F;
                        }
                        m_layRoles[GetMeChairID()].SetCannonRatation(fRatation);
                        //发送到服务器
                        CMD_C_Connon_Run ConnonRun = new CMD_C_Connon_Run();
                        ConnonRun.wChair = GetMeChairID();
                        ConnonRun.fRote = fRatation;
                        GetClientKernel().SendSocketData(FishDefine.SUB_C_CANNON_RUN, ConnonRun);
                    }
                }
                else if (Root.instance().input_manager().keyboard().isKeyDown((int)System.Windows.Forms.Keys.W) || Root.instance().input_manager().keyboard().isKeyDown((int)System.Windows.Forms.Keys.Up))
                {
                    {
                        //if(m_dwRoomType==0)
                        int lTime = m_bIsLeftKeyDowning ? 30 : 80;
                        if (m_Timer.get_milli_seconds() < lTime)
                            return;
                        m_Timer.reset();

                        int wMeChair = GetMeChairID();

                        try
                        {
                            Sound_Instance pSound = Root.instance().sound_manager().sound_instance(17);
                            pSound.play(false, true);
                        }
                        catch
                        {
                        }
                        m_layRoles[wMeChair].SetCannonType(FishDefine.WordToCannonType((int)m_layRoles[wMeChair].GetConnonType()), m_layRoles[wMeChair].GetMulRate() + 5);
                    }
                }
                else if (Root.instance().input_manager().keyboard().isKeyDown((int)System.Windows.Forms.Keys.S) || Root.instance().input_manager().keyboard().isKeyDown((int)System.Windows.Forms.Keys.Down))
                {
                    {
                        //if(m_dwRoomType==0)
                        int lTime = m_bIsLeftKeyDowning ? 30 : 80;
                        if (m_Timer.get_milli_seconds() < lTime)
                            return;
                        m_Timer.reset();

                        int wMeChair = GetMeChairID();

                        try
                        {
                            Sound_Instance pSound = Root.instance().sound_manager().sound_instance(17);
                            pSound.play(false, true);
                        }
                        catch
                        {
                        }
                        m_layRoles[wMeChair].SetCannonType(FishDefine.WordToCannonType((int)m_layRoles[wMeChair].GetConnonType()), m_layRoles[wMeChair].GetMulRate() - 5);
                    }
                }
                else if (Root.instance().input_manager().keyboard().isKeyDown((int)System.Windows.Forms.Keys.Return))
                {
                    if (m_Timer.get_milli_seconds() < 500)
                        return;

                    m_sendMessage.button_press(10202);
                    m_Timer.reset();
                    m_TimerKey.reset();
                }

                if ((Root.instance().input_manager().keyboard().isKeyDown((int)System.Windows.Forms.Keys.Space)) && 
                    (m_layBulletObject.GetBulletCount() < 50) && (m_layBackground.m_IsSceneing == 0) && 
                    ((m_layRoles[GetMeChairID()].m_TaskDate.m_enState < CTaskDate.State.STATE_PREPARE2) || 
                    ((m_layRoles[GetMeChairID()].m_TaskDate.m_enType == CTaskDate.Type.TYPE_COOK && 
                    m_layRoles[GetMeChairID()].m_TaskDate.m_enState == CTaskDate.State.STATE_RUNNING))) || 
                    Root.instance().input_manager().mouse().getMouseState().buttonDown((int)MouseButtonID.Left))
                {



                    int wMeChairID = GetMeChairID();
                    if (wMeChairID != GameDefine.INVALID_CHAIR)
                    {

                        if (m_layRoles[wMeChairID].GetFishGold() <= 0 && !m_layAccount.visible())
                        {
                            if (!m_layBuyBulletLayer.IsSendBuyBulletMessage())
                            {
                                if (m_dwRoomType == 0)
                                {
                                    m_layBuyBulletLayer.ShowWidnow(true);
                                }
                            }

                            if (m_bFirstGame)
                            {
                                m_bFirstGame = false;

                                m_layRoles[wMeChairID].ShowGunLead(false);
                                m_layRoles[wMeChairID].ShowBuyLead(false);
                                m_layRoles[wMeChairID].ShowPlayLead(false);

                                m_layRoles[wMeChairID].DestroyShowBarAnimation();
                            }
                        }
                        else if (!m_layAccount.visible())
                        {
                            if (m_Help.visible())
                            {
                                m_Help.Show(false);
                            }
                            if (m_GateHelpLayer.visible())
                            {
                                m_GateHelpLayer.Show(false);
                            }

                            if ((m_dwRoomType == 3) && (m_MatchStart.visible() || m_MatchAgainLayer.visible() || m_GateEndLayer.visible()))
                                return;
                            if (((m_dwRoomType == 1) || (m_dwRoomType == 2)) && (m_MatchStart.visible() || m_ReMatchStart.visible()))
                                return;

                            time = m_Timer.get_milli_seconds();
                            if (time < 320)
                                return;
                            //m_nShootCount++; if(m_nShootCount > 10) return;

                            m_Timer.reset();
                            m_layRoles[GetMeChairID()].ShowWaringTime(false);


                            int dwMulRate = m_layRoles[wMeChairID].GetMulRate();
                            if (dwMulRate > m_layRoles[wMeChairID].GetFishGold())
                            {
                                dwMulRate += 1000000;
                            }

                            CMD_C_Fire Fire = new CMD_C_Fire();
                            Fire.cbIsBack = 0;
                            Fire.fRote = m_layRoles[wMeChairID].GetCannonRataion();
                            Fire.dwMulRate = dwMulRate;
                            Fire.xStart = (int)m_layRoles[wMeChairID].GetCannonPosition().x_;
                            Fire.yStart = (int)m_layRoles[wMeChairID].GetCannonPosition().y_;
                            GetClientKernel().SendSocketData(FishDefine.SUB_C_FIRE, Fire);

                            //激光炮

                            m_layRoles[wMeChairID].FireLaserBean();

                            //m_layRoles[wMeChairID]->FireLaserBean();


                            int dwFishGold = m_layRoles[wMeChairID].GetMulRate(); //m_layRoles[wMeChairID]->GetConnonType()+1;

                            if (dwFishGold < dwFishGold)
                            {
                                m_layRoles[wMeChairID].ShowFishGoldEmpty(true);
                            }
                            else
                            {
                                m_layRoles[wMeChairID].ShowFishGoldEmpty(false);
                            }

                            dwMulRate = m_layRoles[wMeChairID].GetMulRate();
                            if (dwFishGold > m_layRoles[wMeChairID].GetFishGold())
                            {
                                dwMulRate += 1000000;
                                dwFishGold = 0;
                            }
                            else
                            {
                                dwFishGold = m_layRoles[wMeChairID].GetFishGold() - dwFishGold;
                            }

                            m_layBulletObject.BulletFire(m_layRoles[wMeChairID].GetCannonPosition(), m_layRoles[wMeChairID].GetCannonRataion(), wMeChairID, m_layRoles[wMeChairID].GetConnonType(), dwMulRate);
                            m_layRoles[wMeChairID].GunAnimation();
                            m_layRoles[wMeChairID].GunBaseAnimation();
                            m_layRoles[wMeChairID].SetFishGold(dwFishGold);

                            if ((m_dwRoomType == 3) && (dwFishGold == 0))
                            {
                                m_MatchStart.Show(true);
                            }
                        }
                    }
                }
            }
            else
            {
                if (Root.instance().input_manager().keyboard().isKeyDown(((int)System.Windows.Forms.Keys.Return)))
                {
                    time = m_TimerKey.get_milli_seconds();
                    if (time > 500)
                    {
                        m_TimerKey.reset();
                        m_sendMessage.button_press(10201);
                        m_Timer.reset();
                    }
                }
            }

            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
            {
                int lShowMessage = m_TimerShowMessage[i].get_milli_seconds();
                if (lShowMessage > 6 * 1000)
                {
                    m_layRoles[i].m_sprMessage.set_visible(false);
                    m_TimerShowMessage[i].reset();
                }
            }
        }

        public void MatchOver()
        {
            CMD_C_Match_Over tagMatchOver = new CMD_C_Match_Over();
            tagMatchOver.wChair = GetMeChairID();
            tagMatchOver.lMatchScore = 1988;
            tagMatchOver.lMatchScoreCheck = 2385;

            GetClientKernel().SendSocketData(FishDefine.SUB_C_MATCH_OVER, tagMatchOver);
        }
    }

    //----------------------------------------------------------------------------------------
    //	Copyright © 2006 - 2013 Tangible Software Solutions Inc.
    //	This class can be used by anyone provided that the copyright notice remains intact.
    //
    //	This class provides the ability to initialize array elements with the default
    //	constructions for the array type.
    //----------------------------------------------------------------------------------------
    internal static class Arrays
    {
        internal static T[] InitializeWithDefaultInstances<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = new T();
            }
            return array;
        }
    }
}
