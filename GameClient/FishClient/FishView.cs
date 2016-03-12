using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using GameControls;
using ChatEngine;
using System.Net.Sockets;
using GameControls.XLBE;
using System.Threading;

namespace FishClient
{
    public partial class FishView : GameView
    {
        public FishInfo m_ReceiveInfo;
        public int m_MeChairID = GameDefine.INVALID_CHAIR;

        private bool m_bLoadedResource = false;

        public FishView()
        {
            InitializeComponent();

            Root.instance().initialise(this);

            Root.instance().resource_manager().prase_resources_file( "fish\\fish.resources");
            Root.instance().resource_manager().load_resources("Load");

            Root.instance().scene_director().replace_scene( new CLoadScene(this));
        }

        public void StartGameScene()
        {
            if (m_bCloseed == true)
                return;

            m_bLoadedResource = true;

            if (m_MeChairID == GameDefine.INVALID_CHAIR)
                return;

            CGameScene gameScene = new CGameScene(this);

            Root.instance().scene_director().replace_scene(gameScene);
            
            gameScene.OnEventUserEnter(m_UserInfo, m_MeChairID, false);
            gameScene.OnEventGameScene(0, false, m_ReceiveInfo);
        }

        public override void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_TableDetail:
                    {
                        if (!(baseInfo is FishInfo))
                            return;

                        m_ReceiveInfo = (FishInfo)baseInfo;

                        for (int i = 0; i < m_ReceiveInfo._Players.Count; i++)
                        {
                            if (m_UserInfo.Id == m_ReceiveInfo._Players[i].Id)
                                m_UserInfo = m_ReceiveInfo._Players[i];
                        }

                        int oldChairID = m_MeChairID;

                        for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
                        {
                            if (m_ReceiveInfo.m_RoleObjects[i].userId == m_UserInfo.Id)
                            {
                                m_MeChairID = i;
                                break;
                            }
                        }

                        if (m_MeChairID != GameDefine.INVALID_CHAIR)
                        {
                            Scene scene = Root.instance().scene_director().scene();

                            if (scene is CGameScene)
                            {
                                ((CGameScene)scene).OnEventGameScene(0, false, m_ReceiveInfo);
                            }
                            else if( m_bLoadedResource == true )
                            {
                                StartGameScene();
                            }
                        }
                    }
                    break;

                case NotifyType.Reply_FishSend:
                    {
                        if(!(baseInfo is FishSendInfo))
                            return;

                        FishSendInfo sendInfo = (FishSendInfo)baseInfo;

                        Scene scene = Root.instance().scene_director().scene();

                        if (scene is CGameScene)
                        {
                            ((CGameScene)scene).OnEventSocket(sendInfo._SendType, sendInfo._SendInfo, 0);
                        }
                    }
                    break;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        private void FishView_Resize(object sender, EventArgs e)
        {
        }

        private void FishView_Paint(object sender, PaintEventArgs e)
        {
        }

        public void SendSocketData(int type, object data)
        {
            FishSendInfo sendInfo = new FishSendInfo(type, (BaseInfo)data);
            m_ClientSocket.Send(NotifyType.Request_FishSend, sendInfo);
        }

        public UserInfo GetUserInfo(int chairID)
        {
            if (chairID < 0)
                return null;

            UserInfo userInfo = null;
            string userId = m_ReceiveInfo.m_RoleObjects[chairID].userId;

            for (int i = 0; i < m_ReceiveInfo._Players.Count; i++)
            {
                if (m_ReceiveInfo._Players[i].Id == userId)
                {
                    userInfo = m_ReceiveInfo._Players[i];
                    break;
                }
            }

            return userInfo;
        }

        public UserInfo GetMeUserInfo()
        {
            return m_UserInfo;
        }

        public int GetMeChairID()
        {
            return m_MeChairID;
        }

        private void FishView_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
        }

        private void FishView_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
        }

        public void WindowClosed()
        {
            CallFrameWindowProc(FrameWindowMessage.Frame_Close, null);
        }

        bool m_bCloseed = false;

        public override void CloseView()
        {
            Scene curScene = Root.instance().scene_director().scene();

            if (curScene is CLoadScene)
            {
                Thread thread = ((CLoadScene)curScene).m_LoadingThread;

                if (thread != null)
                {
                    m_bCloseed = true;
                    thread.Join(5);
                }
            }

            base.CloseView();

            Cursor.Show();

            Root.instance().destroy();
        }
    }

}
