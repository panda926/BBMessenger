using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    public class CNetLayer : Layer
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	CNetLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void update(float dt);
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void draw();

        public bool NetFire(Point pt, int wChairID, FishDefine.enCannonType CannonType, int dwMulRate)
        {
            CNetObjectExtend pNetObjectExtend = new CNetObjectExtend();

            pNetObjectExtend.wChairID = wChairID;
            pNetObjectExtend.CannonType = CannonType;
            pNetObjectExtend.dwMulRate = dwMulRate;

            int nFirst = 0;
            if (CannonType > FishDefine.enCannonType.CannonType_5)
            {
                nFirst = 1;
            }

            Size szScale = new Size();
            switch (CannonType)
            {
                case FishDefine.enCannonType.CannonType_0:
                    {
                        szScale.width_ = 0.5;
                        szScale.height_ = 0.5;
                        break;
                    }
                case FishDefine.enCannonType.CannonType_1:
                    {
                        szScale.width_ = 0.6;
                        szScale.height_ = 0.6;
                        break;
                    }
                case FishDefine.enCannonType.CannonType_2:
                    {
                        szScale.width_ = 0.7;
                        szScale.height_ = 0.7;
                        break;
                    }
                case FishDefine.enCannonType.CannonType_3:
                    {
                        szScale.width_ = 0.8;
                        szScale.height_ = 0.8;
                        break;
                    }
                case FishDefine.enCannonType.CannonType_4:
                    {
                        szScale.width_ = 0.9;
                        szScale.height_ = 0.9;
                        break;
                    }
                case FishDefine.enCannonType.CannonType_5:
                    {
                        szScale.width_ = 0.9;
                        szScale.height_ = 0.9;
                        break;
                    }
                case FishDefine.enCannonType.CannonType_6:
                    {
                        szScale.width_ = 1.0;
                        szScale.height_ = 1.0;
                        break;
                    }
            }


            ostringstream ostr = new ostringstream();
            ostr = ostr + "net_" + nFirst + "_" + (int)wChairID;

            Sprite sprNet = new Sprite(Root.instance().imageset_manager().imageset(ostr.str()).image("0"));
            sprNet.set_node_extend(pNetObjectExtend);

            sprNet.set_position(pt);
            sprNet.set_scale(szScale);
            add_child(sprNet);

            Animation aniBullet = Root.instance().animation_manager().animation(ostr.str());
            sprNet.run_action(new Action_Sequence(new Action_Animation(0.06, aniBullet, false), new Action_Func(NetEnd), null));

            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(nFirst + 7);
                pSound.play(false, true);
            }
            catch
            {
            }

            return true;
        }

        public bool NetEnd(Node node, int tag)
        {
            Point ptNet = new Point();
            Point ptTNet = new Point();
            Point ptFish = new Point();
            Size szFish = new Size();
            Point ptDifference = new Point();
            Rect rcScreen = new Rect(0, 0, 1280, 738);
            double sint;
            double cost;


            CGameScene pGameScene = (CGameScene)parent();
            CNetObjectExtend pNetObjectExtend = (CNetObjectExtend)node.node_extend();

            if (pNetObjectExtend.wChairID == pGameScene.GetMeChairID())
            {
                CMD_C_Cast_Net CastNet = new CMD_C_Cast_Net();
                CastNet.wChairID = pNetObjectExtend.wChairID;
                CastNet.cbCount = 0;

                ptNet = node.position();

                foreach ( Node j in pGameScene.m_layFishObject.childs())
                {
                    if (CastNet.cbCount >= FishDefine.MAX_FISH_IN_NET)
                        break;

                    CFishObjectExtend pFishObjectExtend = (CFishObjectExtend)j.node_extend();

                    if (pFishObjectExtend.wID == FishDefine.INVALID_WORD)
                        continue;

                    ptFish = j.position();
                    szFish = pFishObjectExtend.GetFishObjectSize();

                    cost = Math.Cos(j.rotation());
                    sint = Math.Sin(j.rotation());
                    ptTNet.x_ = (ptNet.x_ - ptFish.x_) * cost + (ptNet.y_ - ptFish.y_) * sint;
                    ptTNet.y_ = -(ptNet.x_ - ptFish.x_) * sint + (ptNet.y_ - ptFish.y_) * cost;

                    if (CFishObjectExtend.ComputeCollision(szFish.width_, szFish.height_, pNetObjectExtend.GetNetRadius(), ptTNet.x_, ptTNet.y_))
                    {
                        CastNet.FishNetObjects[CastNet.cbCount].wID = pFishObjectExtend.wID;
                        CastNet.FishNetObjects[CastNet.cbCount].wRoundID = pFishObjectExtend.wRoundID;
                        CastNet.FishNetObjects[CastNet.cbCount].wType = (int)pFishObjectExtend.FishType;
                        CastNet.FishNetObjects[CastNet.cbCount].dwTime = pNetObjectExtend.dwMulRate; //时间没用上，占时存放倍数

                        CastNet.cbCount++;

                    }
                }

                CClientKernel pClientKernel = pGameScene.GetClientKernel();

                if ((pClientKernel != null) && (pNetObjectExtend.dwMulRate <= 1000))
                {
                    pClientKernel.SendSocketData(FishDefine.SUB_C_CAST_NET, CastNet);
                }
            }

            remove_child(node);

            return true;
        }
    }
}
