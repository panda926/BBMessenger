using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    public class tagBulletBack
    {
        public Point ptStart = new Point();
        public int wChair;
        public int nBackCount;
        public double fRotation;
        public int dwMulRate;
        public int wType;
    }

    public class CBulletLayer : Layer
    {
        private List<Node> m_NodeDelete = new List<Node>();
        private List<tagBulletBack> m_BulletBackList = new List<tagBulletBack>();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	CBulletLayer();
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        public override void update(double dt)
        {
            base.update(dt);

            CGameScene pGameScene = (CGameScene)parent();



            Point ptBullet = new Point();
            Point ptTBullet = new Point();
            Point ptFish = new Point();
            Size szFish = new Size();
            double sint;
            double cost;
            Rect rcScreen = new Rect(0, 0, 1280, 738);

            foreach ( Node i in childs_)
            {
                CBulletObjectExtend pBulletObjectExtend = (CBulletObjectExtend)i.node_extend();

                UserInfo pUserData = null;
                if (pGameScene.GetClientKernel() != null)
                {
                    pUserData = pGameScene.GetClientKernel().GetUserInfo(pBulletObjectExtend.wChairID);
                }

                ptBullet = i.position();
                if (!rcScreen.pt_in_rect(ptBullet))
                {
                    if (pUserData != null)
                    {
                        if ((pBulletObjectExtend.nBackCount < 1000) && (GetBulletCount() < 51))
                        {
                            if ((pBulletObjectExtend.wChairID < GameDefine.GAME_PLAYER) && (pBulletObjectExtend.wChairID >= 0))
                            {
                                tagBulletBack tBulletBack = new tagBulletBack();

                                tBulletBack.ptStart = ptBullet;

                                if (tBulletBack.ptStart.x_ < 0)
                                {
                                    tBulletBack.ptStart.x_ = 0;
                                }
                                else if (tBulletBack.ptStart.x_ > 1280)
                                {
                                    tBulletBack.ptStart.x_ = 1280;
                                }

                                if (tBulletBack.ptStart.y_ < 0)
                                {
                                    tBulletBack.ptStart.y_ = 0;
                                }
                                else if (tBulletBack.ptStart.y_ > 738)
                                {
                                    tBulletBack.ptStart.y_ = 738;
                                }

                                tBulletBack.wChair = pBulletObjectExtend.wChairID;
                                tBulletBack.nBackCount = pBulletObjectExtend.nBackCount + 1;
                                tBulletBack.fRotation = pBulletObjectExtend.fRotation;
                                tBulletBack.dwMulRate = pBulletObjectExtend.dwMulRate;
                                tBulletBack.wType = (int)pBulletObjectExtend.CannonType;

                                m_BulletBackList.Add(tBulletBack);
                            }
                        }
                    }

                    m_NodeDelete.Add(i);

                    continue;
                }

                foreach ( Node j in pGameScene.m_layFishObject.childs())
                {
                    CFishObjectExtend pFishObjectExtend = (CFishObjectExtend)j.node_extend();
                    if (pFishObjectExtend.wID == FishDefine.INVALID_WORD)
                        continue;

                    ptFish = j.position();
                    szFish = pFishObjectExtend.GetFishObjectSize();

                    cost = Math.Cos(j.rotation());
                    sint = Math.Sin(j.rotation());
                    ptTBullet.x_ = (ptBullet.x_ - ptFish.x_) * cost + (ptBullet.y_ - ptFish.y_) * sint;
                    ptTBullet.y_ = -(ptBullet.x_ - ptFish.x_) * sint + (ptBullet.y_ - ptFish.y_) * cost;

                    if (CFishObjectExtend.ComputeCollision(szFish.width_, szFish.height_, 10, ptTBullet.x_, ptTBullet.y_))
                    {
                        pGameScene.m_layNetObject.NetFire(ptBullet, pBulletObjectExtend.wChairID, pBulletObjectExtend.CannonType, pBulletObjectExtend.dwMulRate);
                        m_NodeDelete.Add((i));
                        break;
                    }
                }

            }

            List<Node>.Enumerator k = m_NodeDelete.GetEnumerator();
            while (k.MoveNext())
            {
                remove_child((k.Current));
                //delete(k.Current).node_extend();
                //delete(k.Current);
            }

            m_NodeDelete.Clear();

            if (m_BulletBackList.Count() > 0)
            {
                List<tagBulletBack>.Enumerator it;
                foreach ( tagBulletBack its in m_BulletBackList )
                {
                    double fRotation = (its.ptStart.x_ >= 1280) ? -its.fRotation : (its.ptStart.y_ <= 0) ? -its.fRotation + Math.PI : (its.ptStart.y_ >= 738) ? -its.fRotation + Math.PI : -its.fRotation;
                    BulletFireBack(its.ptStart, fRotation, its.wChair, (FishDefine.enCannonType)its.wType, its.nBackCount, its.dwMulRate);
                }
            }

            m_BulletBackList.Clear();

        }

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	virtual void draw();

        public bool BulletFire(Point ptStart, double rotation, int wChairID, FishDefine.enCannonType CannonType, int dwMulRate)
        {
            if (GetBulletCount() > 51)
            {
                return true;
            }

            CBulletObjectExtend BulletObjectExtend = new CBulletObjectExtend();

            BulletObjectExtend.fRotation = rotation;
            BulletObjectExtend.wChairID = wChairID;
            BulletObjectExtend.CannonType = CannonType;
            BulletObjectExtend.dwMulRate = dwMulRate;

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
            ostr = ostr +  "shot_" + nFirst + "_" + (int)wChairID;

            Sprite sprBullet = new Sprite(Root.instance().imageset_manager().imageset(ostr.str()).image("0"));
            sprBullet.set_node_extend(BulletObjectExtend);

            Point pt = new Point(ptStart.x_ + 56 * Math.Cos(rotation - FishDefine.M_PI_2), ptStart.y_ + 56 * Math.Sin(rotation - FishDefine.M_PI_2));
            sprBullet.set_position(pt);
            sprBullet.set_rotation(rotation);
            sprBullet.set_scale(szScale);
            add_child(sprBullet);

            GameControls.XLBE.Action actBullet = new Action_Move_By(3, new Point(2000 * Math.Cos(rotation - FishDefine.M_PI_2), 2000 * Math.Sin(rotation - FishDefine.M_PI_2)));
            sprBullet.run_action(actBullet);

            Animation aniBullet = Root.instance().animation_manager().animation(ostr.str());
            sprBullet.run_action(new Action_Repeat_Forever(new Action_Animation(0.06, aniBullet, false)));

            //  CGameScene *pGameScene = (CGameScene *)parent();
            //  if (wChairID == pGameScene->GetMeChairID())
            //  {
            //      CMD_C_Fire Fire;
            //Fire.cbIsBack = FALSE;
            //      Fire.fRote = rotation;
            //Fire.dwMulRate = dwMulRate;
            //Fire.xStart = 0;
            //Fire.yStart = 0;
            //      pGameScene->GetClientKernel()->SendSocketData(MDM_GF_GAME, SUB_C_FIRE, &Fire, sizeof(CMD_C_Fire));
            //  }

            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(nFirst + 5);
                pSound.play(false, true);
            }
            catch
            {
            }

            return true;
        }

        public bool BulletFireBack(Point ptStart, double rotation, int wChairID, FishDefine.enCannonType CannonType, int nBackCount, int dwMulRate)
        {
            if (GetBulletCount() > 51)
            {
                return true;
            }

            CBulletObjectExtend BulletObjectExtend = new CBulletObjectExtend();

            BulletObjectExtend.nBackCount = nBackCount;
            BulletObjectExtend.fRotation = rotation;
            BulletObjectExtend.wChairID = wChairID;
            BulletObjectExtend.CannonType = CannonType;
            BulletObjectExtend.dwMulRate = dwMulRate;

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
            ostr = ostr +  "shot_" + nFirst + "_" + (int)wChairID;

            Sprite sprBullet = new Sprite(Root.instance().imageset_manager().imageset(ostr.str()).image("0"));
            sprBullet.set_node_extend(BulletObjectExtend);

            Point pt = new Point(ptStart.x_ + 56 * Math.Cos(rotation - FishDefine.M_PI_2), ptStart.y_ + 56 * Math.Sin(rotation - FishDefine.M_PI_2));
            sprBullet.set_position(pt);
            sprBullet.set_rotation(rotation);
            sprBullet.set_scale(szScale);
            add_child(sprBullet);

            GameControls.XLBE.Action actBullet = new Action_Move_By(3, new Point(2000 * Math.Cos(rotation - FishDefine.M_PI_2), 2000 * Math.Sin(rotation - FishDefine.M_PI_2)));
            sprBullet.run_action(actBullet);

            Animation aniBullet = Root.instance().animation_manager().animation(ostr.str());
            sprBullet.run_action(new Action_Repeat_Forever(new Action_Animation(0.06, aniBullet, false)));

            //CGameScene *pGameScene = (CGameScene *)parent();
            //if (wChairID == pGameScene->GetMeChairID())
            //{
            //	CMD_C_Fire Fire;
            //	Fire.cbIsBack = TRUE;
            //	Fire.fRote = rotation;
            //	Fire.dwMulRate = dwMulRate;
            //	Fire.xStart = ptStart.x_;
            //	Fire.yStart = ptStart.y_;
            //	pGameScene->GetClientKernel()->SendSocketData(MDM_GF_GAME, SUB_C_FIRE, &Fire, sizeof(CMD_C_Fire));
            //}

            return true;
        }

        public int GetBulletCount()
        {
            int nCount = 0;

            foreach ( Node i in childs_ )
            {
                CBulletObjectExtend pBulletObjectExtend = (CBulletObjectExtend)i.node_extend();

                Point ptBullet = i.position();
                Rect rcScreen = new Rect(0, 0, 1280, 738);
                if (rcScreen.pt_in_rect(ptBullet))
                {
                    nCount++;
                }
            }

            return nCount;
        }
        public void ClearAllBullets()
        {
            childs_.Clear();
        }

    }
}
