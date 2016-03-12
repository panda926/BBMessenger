using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;
using System.IO;

namespace FishClient
{
    public class CFishLayer : Layer
    {
        public static int[,] m_dwSmallFishBase = new int[,]
        {
	        {0   ,30  ,45  ,0   ,40  ,45  },
	        {50  ,80  ,95  ,50  ,90  ,95  },
	        {100 ,130 ,145 ,100 ,140 ,145 },
	        {150 ,180 ,195 ,150 ,190 ,195 },
	        {200 ,230 ,245 ,200 ,240 ,245 },
	        {250 ,280 ,295 ,250 ,290 ,295 },
	        {300 ,330 ,345 ,300 ,340 ,345 },
	        {350 ,380 ,395 ,350 ,390 ,395 },
	        {400 ,430 ,445 ,400 ,440 ,445 },
	        {450 ,480 ,495 ,450 ,490 ,495 },
	        {500 ,530 ,545 ,500 ,540 ,545 },
	        {550 ,580 ,595 ,550 ,590 ,595 },
	        {600 ,630 ,645 ,600 ,640 ,645 },
	        {650 ,680 ,695 ,650 ,690 ,695 },
	        {700 ,730 ,745 ,700 ,740 ,745 },
	        {750 ,780 ,795 ,750 ,790 ,795 },
	        {800 ,830 ,845 ,800 ,840 ,845 },
	        {850 ,880 ,895 ,850 ,890 ,895 },
	        {900 ,930 ,945 ,900 ,940 ,945 },
	        {950 ,980 ,995 ,950 ,990 ,995 },
	        {1000,1030,1045,1000,1040,1045},
	        {1050,1080,1095,1050,1090,1095},
	        {1100,1130,1145,1100,1140,1145},
	        {1150,1180,1195,1150,1190,1195},
	        {1200,1230,1245,1200,1240,1245},
	        {1250,1280,1295,1250,1290,1295},
	        {1300,1330,1345,1300,1340,1345},
	        {1350,1380,1395,1350,1390,1395},
	        {1400,1430,1445,1400,1440,1445},
	        {1450,1480,1495,1450,1490,1495},
	        {1500,1530,1545,1500,1540,1545},
	        {1550,1580,1595,1550,1590,1595},
	        {1600,1630,1645,1600,1640,1645},
	        {1650,1680,1695,1650,1690,1695},
	        {1700,1730,1745,1700,1740,1745},
        };

        private List<Node> m_NodeDelete = new List<Node>();
        private List<List<Key_Frame>> m_SmallFishPaths = new List<List<Key_Frame>>();
        private List<List<Key_Frame>> m_BigFishPaths = new List<List<Key_Frame>>();
        private List<List<Key_Frame>> m_HugeFishPaths = new List<List<Key_Frame>>();
        private List<List<Key_Frame>> m_SpecialFishPaths = new List<List<Key_Frame>>();

        private Image m_imgBackgroundMask;

        private int m_nPosMove;
        private int m_nMoveCount;
        private Point m_ptMove = new Point();

        public CFishLayer()
        {
            this.m_imgBackgroundMask = null;
            int ptX;
            int ptY;
            int nStaff;
            double fAngle;
            string szPath = new string(new char[100]);
            //Pack_File pf;
            //ostringstream ostr = new ostringstream();

            Random random = new Random();

            m_SmallFishPaths = LoadPathData(AppDomain.CurrentDomain.BaseDirectory + "fish\\path\\SmallFishPath.pak");
            m_BigFishPaths = LoadPathData(AppDomain.CurrentDomain.BaseDirectory + "fish\\path\\BigFishPath.pak");
            m_HugeFishPaths = LoadPathData(AppDomain.CurrentDomain.BaseDirectory + "fish\\path\\HugeFishPath.pak");
            m_SpecialFishPaths = LoadPathData(AppDomain.CurrentDomain.BaseDirectory + "fish\\path\\SpecialFishPath.pak");

            //for (int i = 0; i < 208; i++)
            //{
            //    //List<Key_Frame> frames = new List<Key_Frame>();

            //    //ostr.str("");
            //    //ostr = ostr + "small\\" + i + ".dat";
            //    //pf = pfopen(ostr.str().c_str(), "rb");
            //    //if (pf == 0)
            //    //{
            //    //    MessageBox.Show("CFishLayer::CFishLayer()", "");
            //    //}

            //    //pfgets(szPath, 100, pf);

            //    //while (pfgets(szPath, 100, pf))

            //    m_SmallFishPaths.Add(GenerateRandomFrame(6,7));

            //}

            //for (int i = 0; i < 130; i++)
            //{
            //    //List<Key_Frame> frames = new List<Key_Frame>();

            //    //ostr.str("");
            //    //ostr = ostr + "big\\" + i + ".dat";
            //    //pf = pfopen(ostr.str().c_str(), "rb");
            //    //if (pf == 0)
            //    //{
            //    //    MessageBox.Show("CFishLayer::CFishLayer()", "");
            //    //}

            //    //pfgets(szPath, 100, pf);

            //    //while (pfgets(szPath, 100, pf))
            //    //{
            //    //    std.sscanf(szPath, "(%d,%d,%f,%d)", ptX, ptY, fAngle, nStaff);
            //    //    frames.Add(Key_Frame(new Point(ptX, ptY), fAngle));
            //    //}

            //    m_BigFishPaths.Add(GenerateRandomFrame(6,6));
            //}


            //for (int i = 0; i < 62; i++)
            //{
            //    //List<Key_Frame> frames = new List<Key_Frame>();

            //    //ostr.str("");
            //    //ostr = ostr + "huge\\" + i + ".dat";
            //    //pf = pfopen(ostr.str().c_str(), "rb");
            //    //if (pf == 0)
            //    //{
            //    //    MessageBox.Show("CFishLayer::CFishLayer()", "");
            //    //}

            //    //pfgets(szPath, 100, pf);

            //    //while (pfgets(szPath, 100, pf))
            //    //{
            //    //    std.sscanf(szPath, "(%d,%d,%f,%d)", ptX, ptY, fAngle, nStaff);
            //    //    frames.Add(Key_Frame(new Point(ptX, ptY), fAngle));
            //    //}

            //    m_HugeFishPaths.Add(GenerateRandomFrame(5,5));
            //}

            //for (int i = 0; i < 28; i++)
            //{
            //    //List<Key_Frame> frames = new List<Key_Frame>();

            //    //ostr.str("");
            //    //ostr = ostr + "special\\" + i + ".dat";
            //    //pf = pfopen(ostr.str().c_str(), "rb");
            //    //if (pf == 0)
            //    //{

            //    //    MessageBox.Show("CFishLayer::CFishLayer()", "");
            //    //}

            //    //pfgets(szPath, 100, pf);

            //    //while (pfgets(szPath, 100, pf))
            //    //{
            //    //    std.sscanf(szPath, "(%d,%d,%f,%d)", ptX, ptY, fAngle, nStaff);
            //    //    frames.Add(Key_Frame(new Point(ptX, ptY), fAngle));
            //    //}

            //    m_SpecialFishPaths.Add(GenerateRandomFrame(8,8));

            //}

            m_nMoveCount = 0;
            m_ptMove = new Point(-5, -5);
            m_nPosMove = 0;
        }

        private List<List<Key_Frame>> LoadPathData(string name)
        {
            List<List<Key_Frame>> pathList = new List<List<Key_Frame>>();

            try
            {
                FileStream file = new FileStream(name, FileMode.Open, FileAccess.ReadWrite);

                BinaryReader read_fs = new BinaryReader(file);

                int pathCount = read_fs.ReadInt32();

                for (int i = 0; i < pathCount; i++)
                {
                    List<Key_Frame> frames = new List<Key_Frame>();

                    int frameCount = read_fs.ReadInt32();

                    for (int k = 0; k < frameCount; k++)
                    {
                        Key_Frame keyFrame = new Key_Frame();

                        keyFrame.angle_ = read_fs.ReadDouble();
                        keyFrame.position_.x_ = read_fs.ReadDouble();
                        keyFrame.position_.y_ = read_fs.ReadDouble();

                        frames.Add(keyFrame);
                    }

                    pathList.Add(frames);
                }

                read_fs.Close();
                file.Close();
            }
            catch
            {
                return null;
            }


            return pathList;
        }

        Random random = new Random();

        private Point makeEndPt()
        {
            int startDirect = random.Next() % 4;
            Point startPt = new Point();

            if (startDirect == 0)
            {
                startPt.x_ = FishInfo.m_dwSmallFishBase[random.Next() % 28, random.Next() % 6];
                startPt.y_ = -600 - random.Next() % 800;
            }
            else if (startDirect == 1)
            {
                startPt.x_ = FishInfo.m_dwSmallFishBase[random.Next() % 28, random.Next() % 6];
                startPt.y_ = 1024 + random.Next() % 800;
            }
            else if (startDirect == 2)
            {
                startPt.x_ = -600 - random.Next() % 800;
                startPt.y_ = FishInfo.m_dwSmallFishBase[random.Next() % 28, random.Next() % 6];
            }
            else if (startDirect == 3)
            {
                startPt.x_ = 1300 + random.Next() % 800;
                startPt.y_ = FishInfo.m_dwSmallFishBase[random.Next() % 28, random.Next() % 6];
            }

            return startPt;
        }

        private Point makeMiddlePt( Point basePt, int distance )
        {
            Point middlePt = new Point();

            middlePt.x_ = basePt.x_ + random.Next() % (distance * 2) - distance;
            middlePt.y_ = basePt.y_ + random.Next() % (distance * 2) - distance;

            return middlePt;
        }

        Point Bezier(List<Point> p, int n, double mu)
        {

            int k, kn, nn, nkn;

            double blend, muk, munk;

            Point b = new Point();

            muk = 1;

            munk = Math.Pow(1 - mu, (double)n);

            for (k = 0; k <= n; k++)
            {

                nn = n;

                kn = k;

                nkn = n - k;

                blend = muk * munk;

                muk *= mu;

                munk /= (1 - mu);

                while (nn >= 1)
                {

                    blend *= nn;

                    nn--;

                    if (kn > 1)
                    {

                        blend /= (double)kn;

                        kn--;

                    }

                    if (nkn > 1)
                    {

                        blend /= (double)nkn;

                        nkn--;

                    }

                }

                b.x_ += p[k].x_ * blend;

                b.y_ += p[k].y_ * blend;
            }

            return (b);
        }

        private Point makeScreenPoint()
        {
            Point screenPoint = new Point();
            screenPoint.x_ = 100 + random.Next() % 1000;
            screenPoint.y_ = 50 + random.Next() % 600;

            return screenPoint;
        }

        private List<Key_Frame> GenerateRandomFrame( int minspeed, int maxspeed)
        {
            List<Key_Frame> frames = new List<Key_Frame>();
            int speed = random.Next(minspeed, maxspeed);

            Point startPoint = makeEndPt();
            //Point startPoint = makeScreenPoint();

            List<Point> pointPath = new List<Point>();

            while (frames.Count < 6000)
            {
                Point screenPoint = makeScreenPoint();

                int xdist = (int)(screenPoint.x_ - startPoint.x_);
                int ydist = (int)(screenPoint.y_ - startPoint.y_);
                double offAngle = Math.Atan2(ydist, xdist);

                double angle = (double)((random.Next() % 180) * Math.PI / 180 - Math.PI/2);
                double distance = 1400;

                Point endPoint = new Point();
                endPoint.x_ = (int)(Math.Cos(angle + offAngle) * distance) + screenPoint.x_;
                endPoint.y_ = (int)(Math.Sin(angle + offAngle) * distance) + screenPoint.y_;

                //endPoint = makeScreenPoint();

                pointPath.Clear();
                pointPath.Add(startPoint);
                pointPath.Add(screenPoint);
                pointPath.Add(endPoint);

                if (endPoint.x_ >= 0 && endPoint.x_ <= 1280 && endPoint.y_ >= 0 && endPoint.y_ <= 738)
                    distance = 1400;

                int segmentCount = 200;
                Point startPt = pointPath[0];

                for (int i = 0; i < segmentCount; i++)
                {
                    Point endPt = Bezier(pointPath, pointPath.Count - 1, (double)i / segmentCount);

                    xdist = (int)(endPt.x_ - startPt.x_);
                    ydist = (int)(endPt.y_ - startPt.y_);

                    distance = Math.Sqrt(xdist * xdist + ydist * ydist);
                    int frameCount = (int)(distance / speed);

                    if (frameCount <= 0)
                        continue;

                    angle = Math.Atan2(ydist, xdist) * 180 / Math.PI - 90;

                    for (int k = 0; k < frameCount; k++)
                    {
                        int x = (int)(startPt.x_ + xdist * k / frameCount);
                        int y = (int)(startPt.y_ + ydist * k / frameCount);

                        frames.Add(new Key_Frame(new Point(x, y), angle));
                    }

                    startPt = endPt;
                }

                startPoint = endPoint;
            }

            return frames;
        }
    

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        public override void update(double dt)
        {
            base.update(dt);

            long dwTime = FishDefine.time();

            foreach ( Node child in childs_)
            {
                CFishObjectExtend pFishObjectExtend = (CFishObjectExtend)child.node_extend();

                if (dwTime - pFishObjectExtend.dwTime >= FishDefine.FISH_DESTROY_TIME)
                {
                    m_NodeDelete.Add(child);
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


        }
        public override void draw()
        {
            base.draw();

            if (m_imgBackgroundMask != null)
            {
                m_nMoveCount++;

                if (m_nMoveCount > 6)
                {
                    m_nMoveCount = 0;
                    m_nPosMove++;
                    if (m_nPosMove >= 50)
                    {
                        m_nPosMove = 0;
                    }
                    if (m_nPosMove < 25)
                    {
                        m_ptMove -= new Point(5, 5);
                    }
                    else
                    {
                        m_ptMove += new Point(5, 5);
                    }
                }

                m_imgBackgroundMask.draw(position() + m_ptMove);

            }
        }

        public bool NetAddLinePathSmallFishGroup(CMD_S_Small_Fish_Group pSmallFishGroup)
        {
            if (m_imgBackgroundMask != null)
            {

            }
            else
            {
                m_imgBackgroundMask = Root.instance().imageset_manager().imageset("bg_game_mask").image("bg");
            }

            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        int wFishID = (k == 0) ? pSmallFishGroup.wFishGroupTop[i,j].wFishID : pSmallFishGroup.wFishGroupBottom[i,j].wFishID;
                        int wRoundID = (k == 0) ? pSmallFishGroup.wFishGroupTop[i,j].wRoundID : pSmallFishGroup.wFishGroupBottom[i,j].wRoundID;

                        Point ptBase = (k == 0) ? new Point(m_dwSmallFishBase[j,i], 150) : new Point(m_dwSmallFishBase[j,i], 580);

                        if (wFishID != FishDefine.INVALID_WORD)
                        {
                            CFishObjectExtend pNodeExtend = new CFishObjectExtend();
                            pNodeExtend.wID = wFishID;
                            pNodeExtend.wRoundID = wRoundID;
                            pNodeExtend.FishType = FishDefine.WordToFishType(pSmallFishGroup.wFishType);
                            pNodeExtend.dwTime = FishDefine.time();

                            ostringstream ostr = new ostringstream();
                            ostr = ostr + "fish_" + (int)pNodeExtend.FishType;

                            Sprite sprFish = new Sprite(Root.instance().imageset_manager().imageset(ostr.str()).image("0"));
                            sprFish.set_node_extend(pNodeExtend);
                            add_child(sprFish);

                            ostr.str("");
                            ostr = ostr + "fish_" + (int)pNodeExtend.FishType + "_move";
                            Animation aniFish = Root.instance().animation_manager().animation(ostr.str());
                            GameControls.XLBE.Action actFish = new Action_Repeat_Forever(new Action_Animation(0.06, aniFish, false));
                            sprFish.run_action(actFish);
                            sprFish.set_position(ptBase);

                            int nOffMove = (k == 0) ? 1800 : -1800;
                            int nOffEnd = (k == 0) ? 2 : -2;

                            GameControls.XLBE.Action actMove = null;
                            actMove = new Action_Sequence(new Action_Delay(pSmallFishGroup.nTime + i * 2), new Action_Move_To(4, new Point(ptBase.x_, ptBase.y_ + nOffMove)), null);
                            actMove.set_tag(0);
                            sprFish.run_action(actMove);

                            double rote;
                            double length;
                            Point tv = new Point(new Point(ptBase.x_, ptBase.y_ + nOffEnd) - sprFish.position());
                            length = Math.Sqrt(tv.x_ * tv.x_ + tv.y_ * tv.y_);

                            if (length > 0)
                            {
                                if (tv.y_ >= 0)
                                {
                                    rote = Math.Acos(tv.x_ / length);
                                }
                                else
                                {
                                    rote = -Math.Acos(tv.x_ / length);
                                }
                                sprFish.set_rotation(rote - FishDefine.M_PI_2);
                            }
                        }

                    }
                }
            }

            return true;
        }

        public bool NetAddLinePathSmallBottomFish(CMD_S_Send_Line_Path_Fish pAddLinePathFish)
        {
            if (m_imgBackgroundMask != null)
            {

            }
            else
            {
                m_imgBackgroundMask = Root.instance().imageset_manager().imageset("bg_game_mask").image("bg");
            }

            int nNetFishCount = pAddLinePathFish.cbCount;
            for (int i = 0; i < nNetFishCount; i++)
            {
                CFishObjectExtend pNodeExtend = new CFishObjectExtend();
                pNodeExtend.wID = pAddLinePathFish.FishPaths[i].FishNetObject.wID;
                pNodeExtend.wRoundID = pAddLinePathFish.FishPaths[i].FishNetObject.wRoundID;
                pNodeExtend.FishType = FishDefine.WordToFishType(pAddLinePathFish.FishPaths[i].FishNetObject.wType);
                pNodeExtend.dwTime = FishDefine.time(); //pAddLinePathFish->FishPaths[i].FishNetObject.dwTime;

                ostringstream ostr = new ostringstream();
                ostr = ostr + "fish_" + (int)pNodeExtend.FishType;

                Sprite sprFish = new Sprite(Root.instance().imageset_manager().imageset(ostr.str()).image("0"));
                sprFish.set_node_extend(pNodeExtend);
                add_child(sprFish);

                ostr.str("");
                ostr = ostr + "fish_" + (int)pNodeExtend.FishType + "_move";
                Animation aniFish = Root.instance().animation_manager().animation(ostr.str());
                GameControls.XLBE.Action actFish = new Action_Repeat_Forever(new Action_Animation(0.06, aniFish, false));
                sprFish.run_action(actFish);
                sprFish.set_position(new Point(pAddLinePathFish.FishPaths[i].LinePath.spStart.x, pAddLinePathFish.FishPaths[i].LinePath.spStart.y));

                int nOffMove = (pAddLinePathFish.cbForward % 2 == 0) ? 1800 : -1800;

                GameControls.XLBE.Action actMove = null;
                if ((pAddLinePathFish.cbForward == 0) || (pAddLinePathFish.cbForward == 1))
                {
                    actMove = new Action_Sequence(new Action_Move_To(pAddLinePathFish.FishPaths[i].LinePath.dwTime, new Point(pAddLinePathFish.FishPaths[i].LinePath.spEnd.x, pAddLinePathFish.FishPaths[i].LinePath.spEnd.y)), new Action_Delay(95), new Action_Move_To(pAddLinePathFish.FishPaths[i].LinePath.dwTime, new Point(pAddLinePathFish.FishPaths[i].LinePath.spEnd.x, pAddLinePathFish.FishPaths[i].LinePath.spEnd.y + nOffMove)), null);
                }
                else
                {
                    actMove = new Action_Sequence(new Action_Move_To(pAddLinePathFish.FishPaths[i].LinePath.dwTime, new Point(pAddLinePathFish.FishPaths[i].LinePath.spEnd.x, pAddLinePathFish.FishPaths[i].LinePath.spEnd.y + nOffMove)), null);
                }
                actMove.set_tag(0);
                sprFish.run_action(actMove);

                double rote;
                double length;
                Point tv = new Point(new Point(pAddLinePathFish.FishPaths[i].LinePath.spEnd.x, pAddLinePathFish.FishPaths[i].LinePath.spEnd.y) - sprFish.position());
                length = Math.Sqrt(tv.x_ * tv.x_ + tv.y_ * tv.y_);

                if (length > 0)
                {
                    if (tv.y_ >= 0)
                    {
                        rote = Math.Acos(tv.x_ / length);
                    }
                    else
                    {
                        rote = -Math.Acos(tv.x_ / length);
                    }
                    sprFish.set_rotation(rote - FishDefine.M_PI_2);
                }
            }



            return true;
        }

        public bool NetAddLinePathFish(CMD_S_Send_Line_Path_Fish pAddLinePathFish)
        {
            if (m_imgBackgroundMask != null)
            {

            }
            else
            {
                m_imgBackgroundMask = Root.instance().imageset_manager().imageset("bg_game_mask").image("bg");
            }

            int nNetFishCount = pAddLinePathFish.cbCount;
            for (int i = 0; i < nNetFishCount; i++)
            {
                CFishObjectExtend pNodeExtend = new CFishObjectExtend();
                pNodeExtend.wID = pAddLinePathFish.FishPaths[i].FishNetObject.wID;
                pNodeExtend.wRoundID = pAddLinePathFish.FishPaths[i].FishNetObject.wRoundID;
                pNodeExtend.FishType = FishDefine.WordToFishType(pAddLinePathFish.FishPaths[i].FishNetObject.wType);
                pNodeExtend.dwTime = FishDefine.time(); //pAddLinePathFish->FishPaths[i].FishNetObject.dwTime;

                ostringstream ostr = new ostringstream();
                ostr = ostr + "fish_" + (int)pNodeExtend.FishType;

                Sprite sprFish = new Sprite(Root.instance().imageset_manager().imageset(ostr.str()).image("0"));
                sprFish.set_node_extend(pNodeExtend);
                add_child(sprFish);

                ostr.str("");
                ostr = ostr + "fish_" + (int)pNodeExtend.FishType + "_move";
                Animation aniFish = Root.instance().animation_manager().animation(ostr.str());
                GameControls.XLBE.Action actFish = new Action_Repeat_Forever(new Action_Animation(0.06, aniFish, false));
                sprFish.run_action(actFish);

                sprFish.set_position(new Point(pAddLinePathFish.FishPaths[i].LinePath.spStart.x, pAddLinePathFish.FishPaths[i].LinePath.spStart.y));
                GameControls.XLBE.Action actMove = new Action_Move_To(pAddLinePathFish.FishPaths[i].LinePath.dwTime, new Point(pAddLinePathFish.FishPaths[i].LinePath.spEnd.x, pAddLinePathFish.FishPaths[i].LinePath.spEnd.y));
                actMove.set_tag(0);
                sprFish.run_action(actMove);

                double rote;
                double length;
                Point tv = new Point(new Point(pAddLinePathFish.FishPaths[i].LinePath.spEnd.x, pAddLinePathFish.FishPaths[i].LinePath.spEnd.y) - sprFish.position());
                length = Math.Sqrt(tv.x_ * tv.x_ + tv.y_ * tv.y_);

                if (length > 0)
                {
                    if (tv.y_ >= 0)
                    {
                        rote = Math.Acos(tv.x_ / length);
                    }
                    else
                    {
                        rote = -Math.Acos(tv.x_ / length);
                    }
                    sprFish.set_rotation(rote - FishDefine.M_PI_2);
                }
            }



            return true;
        }

        public bool NetAddPointPathFish(CMD_S_Send_Point_Path_Fish pAddPointFish)
        {
            if (m_imgBackgroundMask != null)
            {

            }
            else
            {
                m_imgBackgroundMask = Root.instance().imageset_manager().imageset("bg_game_mask").image("bg");
            }

            int nNetFishCount = pAddPointFish.cbCount;
            for (int i = 0; i < nNetFishCount; i++)
            {
                if (GetFishCount() > FishDefine.MAX_FISH_IN_POOL)
                    continue;

                CFishObjectExtend pNodeExtend = new CFishObjectExtend();
                pNodeExtend.wID = pAddPointFish.FishPaths[i].FishNetObject.wID;
                pNodeExtend.wRoundID = pAddPointFish.FishPaths[i].FishNetObject.wRoundID;
                pNodeExtend.FishType = FishDefine.WordToFishType(pAddPointFish.FishPaths[i].FishNetObject.wType);
                pNodeExtend.dwTime = FishDefine.time(); //pAddPointFish->FishPaths[i].FishNetObject.dwTime;

                ostringstream ostr = new ostringstream();
                ostr = ostr + "fish_" + (int)pNodeExtend.FishType;

                Sprite sprFish = new Sprite(Root.instance().imageset_manager().imageset(ostr.str()).image("0"));
                sprFish.set_node_extend(pNodeExtend);
                add_child(sprFish);


                ostr.str("");
                ostr = ostr + "fish_" + (int)pNodeExtend.FishType + "_move";
                Animation aniFish = Root.instance().animation_manager().animation(ostr.str());
                GameControls.XLBE.Action actFish = new Action_Repeat_Forever(new Action_Animation(0.06, aniFish, false));
                sprFish.run_action(actFish);


                List<Key_Frame> pFrames = null;
                if ((int)pNodeExtend.FishType >= 7)
                {
                    pFrames = m_HugeFishPaths[pAddPointFish.FishPaths[i].wPath];
                }
                else if ((int)pNodeExtend.FishType > 3 && (int)pNodeExtend.FishType < 7)
                {
                    pFrames = m_BigFishPaths[pAddPointFish.FishPaths[i].wPath];
                }
                else
                {
                    pFrames = m_SmallFishPaths[pAddPointFish.FishPaths[i].wPath];
                }


                GameControls.XLBE.Action act = new Action_Sequence(new Action_Key_Frame(((int)pNodeExtend.FishType == 13) ? 0.15 : 0.05, pFrames, new Point(0, 0)), new Action_Func(FishDestory), null);
                act.set_tag(0);
                sprFish.run_action(act);
            }

            return true;
        }

        public bool NetAddSpecialPointPathFish(CMD_S_Send_Special_Point_Path pSendSpecialPointPathFish)
        {
            if (m_imgBackgroundMask != null)
            {

            }
            else
            {
                m_imgBackgroundMask = Root.instance().imageset_manager().imageset("bg_game_mask").image("bg");
            }

            int nNetFishCount = pSendSpecialPointPathFish.cbCount;
            for (int i = 0; i < nNetFishCount; i++)
            {
                CFishObjectExtend pNodeExtend = new CFishObjectExtend();
                pNodeExtend.wID = pSendSpecialPointPathFish.SpecialPointPath[i].FishNetObject.wID;
                pNodeExtend.wRoundID = pSendSpecialPointPathFish.SpecialPointPath[i].FishNetObject.wRoundID;
                pNodeExtend.FishType = FishDefine.WordToFishType(pSendSpecialPointPathFish.SpecialPointPath[i].FishNetObject.wType);
                pNodeExtend.dwTime = FishDefine.time(); //pAddPointFish->FishPaths[i].FishNetObject.dwTime;

                ostringstream ostr = new ostringstream();
                ostr = ostr + "fish_" + (int)pNodeExtend.FishType;

                Sprite sprFish = new Sprite(Root.instance().imageset_manager().imageset(ostr.str()).image("0"));
                sprFish.set_node_extend(pNodeExtend);
                sprFish.set_visible(false);
                add_child(sprFish);

                ostr.str("");
                ostr = ostr + "fish_" + (int)pNodeExtend.FishType + "_move";
                Animation aniFish = Root.instance().animation_manager().animation(ostr.str());
                GameControls.XLBE.Action actFish = new Action_Repeat_Forever(new Action_Animation(0.06, aniFish, false));
                sprFish.run_action(actFish);

                List<Key_Frame> pFrames;
                pFrames = m_SpecialFishPaths[pSendSpecialPointPathFish.SpecialPointPath[i].wPath];

                GameControls.XLBE.Action act = new Action_Sequence(new Action_Delay(pSendSpecialPointPathFish.SpecialPointPath[i].fDelay), new Action_Show(), new Action_Key_Frame(0.05, pFrames, new Point(0, 0)), new Action_Func(FishDestory), null);
                act.set_tag(0);
                sprFish.run_action(act);
            }

            return true;
        }

        public bool NetAddGroupPointPathFish(CMD_S_Send_Group_Point_Path_Fish pAddGroupPointFish)
        {
            if (m_imgBackgroundMask != null)
            {

            }
            else
            {
                m_imgBackgroundMask = Root.instance().imageset_manager().imageset("bg_game_mask").image("bg");
            }

            Point ptOffset = new Point();
            double fDelay = 0.05;
            int nNetFishCount = pAddGroupPointFish.cbCount;
            for (int i = 0; i < nNetFishCount; i++)
            {
                if (GetFishCount() > FishDefine.MAX_FISH_IN_POOL)
                    continue;

                CFishObjectExtend pNodeExtend = new CFishObjectExtend();
                pNodeExtend.wID = pAddGroupPointFish.FishNetObject[i].wID;
                pNodeExtend.wRoundID = pAddGroupPointFish.FishNetObject[i].wRoundID;
                pNodeExtend.FishType = FishDefine.WordToFishType(pAddGroupPointFish.FishNetObject[i].wType);
                pNodeExtend.dwTime = FishDefine.time(); //pAddGroupPointFish->FishNetObject[i].dwTime;

                ostringstream ostr = new ostringstream();
                ostr = ostr + "fish_" + (int)pNodeExtend.FishType;

                Sprite sprFish = new Sprite(Root.instance().imageset_manager().imageset(ostr.str()).image("0"));
                sprFish.set_node_extend(pNodeExtend);
                add_child(sprFish);

                ostr.str("");
                ostr = ostr + "fish_" + (int)pNodeExtend.FishType + "_move";
                Animation aniFish = Root.instance().animation_manager().animation(ostr.str());
                GameControls.XLBE.Action actFish = new Action_Repeat_Forever(new Action_Animation(0.06, aniFish, false));
                sprFish.run_action(actFish);

                List<Key_Frame> pFrames = null;

                pFrames = m_SmallFishPaths[pAddGroupPointFish.wPath];

                if (pAddGroupPointFish.cbPahtType == 0)
                {
                    GameControls.XLBE.Action act = new Action_Sequence(new Action_Delay(i * 0.3 + 0.05), new Action_Key_Frame(0.05, pFrames, new Point(0, 0)), new Action_Func(FishDestory), null);
                    act.set_tag(0);
                    sprFish.run_action(act);
                }
                else if (pAddGroupPointFish.cbPahtType == 1)
                {
                    if (i % 4 == 0)
                    {
                    }
                    else if (i % 4 == 1)
                    {
                        ptOffset.x_ = 0;
                        ptOffset.y_ = 0;

                        fDelay += 0.3;
                    }
                    else if (i % 4 == 2)
                    {
                        ptOffset.x_ = -14;
                        ptOffset.y_ = -18;

                        fDelay += 0.2;
                    }
                    else if (i % 4 == 3)
                    {
                        ptOffset.x_ = 14;
                        ptOffset.y_ = 18;
                    }

                    GameControls.XLBE.Action act = new Action_Sequence(new Action_Delay(fDelay), new Action_Key_Frame(0.05, pFrames, ptOffset), new Action_Func(FishDestory), null);
                    act.set_tag(0);
                    sprFish.run_action(act);
                }
                else
                {
                    if (i == 0)
                    {
                    }
                    else if (i % 2 == 0)
                    {
                        ptOffset.x_ = 14;
                        ptOffset.y_ = 14;

                        fDelay += 0.3;
                    }
                    else
                    {
                        ptOffset.x_ = -14;
                        ptOffset.y_ = -14;
                    }


                    GameControls.XLBE.Action act = new Action_Sequence(new Action_Delay(fDelay), new Action_Key_Frame(0.05, pFrames, ptOffset), new Action_Func(FishDestory), null);
                    act.set_tag(0);
                    sprFish.run_action(act);
                }
            }

            return true;
        }

        public bool FishDestory(Node node, int tag)
        {
            remove_child(node);
            //node.node_extend() = null;
            //node = null;
            return true;
        }
        public void SpeedUpFishObject(double fSpeed)
        {
            foreach ( Node i in childs_ )
            {
                GameControls.XLBE.Action actMove = i.action_by_tag(0);

                if (actMove != null)
                {
                    actMove.set_speed(fSpeed);
                }


            }
        }
        public int GetFishCount()
        {
            int nCount = 0;

            foreach ( Node i in childs_)
            {
                GameControls.XLBE.Action actMove = i.action_by_tag(0);

                if (actMove != null)
                {
                    nCount++;
                }
            }

            return nCount;
        }

        public bool FishDead(Node node, int tag)
        {
            remove_child(node);
            //node.node_extend() = null;
            //node = null;

            return true;
        }

        public int FishCaptured(int wId, int wChairID, int dwMulRate)
        {
            CGameScene pGameScene = (CGameScene)parent();

            foreach ( Node i in childs_ )
            {
                CFishObjectExtend pFishObjectExtend = (CFishObjectExtend)i.node_extend();
                pFishObjectExtend.dwMulRate = dwMulRate;

                if ((pFishObjectExtend.wID) == wId)
                {
                    if (pGameScene.GetMeChairID() == wChairID)
                    {
                        pGameScene.m_layAccount.m_CaptureFishs[(int)pFishObjectExtend.FishType]++;
                    }

                    ostringstream ostr = new ostringstream();
                    ostr = ostr + "fish_" + (int)pFishObjectExtend.FishType + "_capture";

                    Animation aniDead = Root.instance().animation_manager().animation(ostr.str());
                    GameControls.XLBE.Action actDead = new Action_Sequence(new Action_Animation(0.06, aniDead, false), new Action_Func(FishDead), null);

                    GameControls.XLBE.Action actFun = new Action_Func(FishEnd);
                    actFun.set_tag(wChairID);

                    i.stop_all_action();
                    i.run_action(new Action_Sequence(actFun, actDead, null));

                    pFishObjectExtend.wID = FishDefine.INVALID_WORD;

                    if (pFishObjectExtend.FishType == FishDefine.enFishType.FishType_12)
                    {
                        try
                        {
                            Sound_Instance pSound = Root.instance().sound_manager().sound_instance(25);
                            pSound.play(false, true);
                        }
                        catch
                        {
                        }
                    }
                    else if (pFishObjectExtend.FishType == FishDefine.enFishType.FishType_14)
                    {
                        try
                        {
                            Sound_Instance pSound = Root.instance().sound_manager().sound_instance(26);
                            pSound.play(false, true);
                        }
                        catch
                        {
                        }
                    }
                    else if ((int)pFishObjectExtend.FishType > 6)
                    {
                        try
                        {
                            Sound_Instance pSound = Root.instance().sound_manager().sound_instance((int)pFishObjectExtend.FishType + 3);
                            pSound.play(false, true);
                        }
                        catch
                        {
                        }
                    }

                    pGameScene.m_layRoles[wChairID].AddCookFish((int)pFishObjectExtend.FishType);

                    return pFishObjectExtend.GetFishGoldByStyle() * dwMulRate;
                }
            }

            return 0;
        }

        public int FishCapturedScore(int wId)
        {
            CGameScene pGameScene = (CGameScene)parent();

            foreach ( Node i in childs_)
            {
                CFishObjectExtend pFishObjectExtend = (CFishObjectExtend)i.node_extend();

                if ((pFishObjectExtend.wID) == wId)
                {
                    return pFishObjectExtend.GetFishGoldByStyle();
                }
            }

            return 0;
        }
        public int FishGoldByStyle(int nFishStyle, int wID)
        {
            switch (nFishStyle)
            {
                case 0:
                    return 1;
                case 1:
                    return 2;
                case 2:
                    return 3;
                case 3:
                    return 4;
                case 4:
                    return 5;
                case 5:
                    return 7;
                case 6:
                    return 9;
                case 7:
                    return 10;
                case 8:
                    return 12;
                case 9:
                    return 15;
                case 10:
                    return 20;
                case 11:
                    return 30 + wID % 21;
                case 12:
                    return 40 + wID % 81;
                case 13:
                    return 0;
                case 14:
                    return 300;
                case 15:
                    return 300;
                default:
                    return 0;
            }
        }

        public bool CoinAlpha(Node node, int tag)
        {
            node.set_visible(true);

            return true;
        }

        public bool FishEnd(Node node, int tag)
        {
            try
            {
                Sound_Instance pSound = Root.instance().sound_manager().sound_instance(9);
                pSound.play(false, true);
            }
            catch
            {
            }

            CGameScene pGameScene = (CGameScene)parent();
            CFishObjectExtend pFishObjectExtend = (CFishObjectExtend)node.node_extend();

            int nFishGold = pFishObjectExtend.GetFishGoldByStyle();
            int nCoinCount = nFishGold * pFishObjectExtend.dwMulRate;
            int nCointTen = (nFishGold / 10) % 10;
            int nCointOne = nFishGold % 10;

            Point ptCoin = new Point();
            Point ptNode = new Point();
            Point ptRole = pGameScene.m_layRoles[tag].GetCannonPosition();

            if (nCointTen == 1)
            {
                ptNode = node.position();
            }
            else if (nCointTen <= 5)
            {
                ptNode = node.position() + new Point(-((double)(nCointTen)) / 2 * 48, -(nCointTen / 5) * 48);
            }
            else
            {
                ptNode = node.position() + new Point(-5.0f / 2 * 48, -(nCointTen / 5) * 48);
            }

            for (int i = 0; i < nCointTen; i++)
            {
                if (i > 4)
                {
                    ptCoin = ptNode + new Point((i % 5) * 48, +48);
                }
                else
                {
                    ptCoin = ptNode + new Point(i * 48, 0);
                }


                Sprite sprTenCoin = new Sprite(Root.instance().imageset_manager().imageset("role").image("coin_gold_0"));
                Sprite sprTenCoin1 = new Sprite(Root.instance().imageset_manager().imageset("role").image("coin_gold_0"));
                Sprite sprTenCoin2 = new Sprite(Root.instance().imageset_manager().imageset("role").image("coin_gold_0"));

                sprTenCoin.set_position(ptCoin);
                sprTenCoin1.set_position(ptCoin + new Point(0, 4));
                sprTenCoin2.set_position(ptCoin + new Point(0, 8));

                sprTenCoin1.set_visible(false);
                sprTenCoin2.set_visible(false);

                Color_Rect ColorRect = sprTenCoin1.color();
                ColorRect.set_alpha(150);
                sprTenCoin1.set_color(ColorRect);
                ColorRect.set_alpha(80);
                sprTenCoin2.set_color(ColorRect);

                pGameScene.m_layCoinObject.add_child(sprTenCoin2);
                pGameScene.m_layCoinObject.add_child(sprTenCoin1);
                pGameScene.m_layCoinObject.add_child(sprTenCoin);

                Animation aniCoin = Root.instance().animation_manager().animation("coin_gold");
                sprTenCoin.run_action(new Action_Repeat_Forever(new Action_Animation(0.06, aniCoin, false)));

                GameControls.XLBE.Action actSequence;
                GameControls.XLBE.Action[] actMove = new GameControls.XLBE.Action[5];

                actMove[0] = new Action_Move_To(0.3, ptCoin + new Point(0, -150));
                actMove[1] = new Action_Move_To(0.3, ptCoin + new Point(0, 0));
                actMove[2] = new Action_Move_To(0.1, ptCoin + new Point(0, -15));
                actMove[3] = new Action_Move_To(0.1, ptCoin + new Point(0, 0));
                actMove[4] = new Action_Move_To(0.4, ptRole);

                GameControls.XLBE.Action actFunc = new Action_Func(pGameScene.m_layCoinObject.CoinGoldEnd);
                actFunc.set_tag(tag);

                actSequence = new Action_Sequence(actMove[0], actMove[1], actMove[2], actMove[3], actMove[4], actFunc, null);
                sprTenCoin.run_action(actSequence);

                actMove[0] = new Action_Move_To(0.3, ptCoin + new Point(0, -150));
                actMove[1] = new Action_Move_To(0.3, ptCoin + new Point(0, 0));
                actMove[2] = new Action_Move_To(0.1, ptCoin + new Point(0, -15));
                actMove[3] = new Action_Move_To(0.1, ptCoin + new Point(0, 0));
                actMove[4] = new Action_Move_To(0.4, ptRole);
                actSequence = new Action_Sequence(new Action_Delay(0.04), actMove[0], actMove[1], actMove[2], actMove[3], new Action_Func(CoinAlpha), actMove[4], new Action_Func(pGameScene.m_layCoinObject.CoinGoldEnd), null);
                /*actSequence->set_tag(tag);*/
                sprTenCoin1.run_action(actSequence);

                actMove[0] = new Action_Move_To(0.3, ptCoin + new Point(0, -150));
                actMove[1] = new Action_Move_To(0.3, ptCoin + new Point(0, 0));
                actMove[2] = new Action_Move_To(0.1, ptCoin + new Point(0, -15));
                actMove[3] = new Action_Move_To(0.1, ptCoin + new Point(0, 0));
                actMove[4] = new Action_Move_To(0.4, ptRole);
                actSequence = new Action_Sequence(new Action_Delay(0.08), actMove[0], actMove[1], actMove[2], actMove[3], new Action_Func(CoinAlpha), actMove[4], new Action_Func(pGameScene.m_layCoinObject.CoinGoldEnd), null);
                /*actSequence->set_tag(tag);*/
                sprTenCoin2.run_action(actSequence);

            }

            if (nCointOne == 1)
            {
                ptNode = node.position();
            }
            else if (nCointOne <= 5)
            {
                ptNode = node.position() + new Point(-((double)(nCointOne)) / 2 * 36, -(nCointOne / 5) * 36);
            }
            else
            {
                ptNode = node.position() + new Point(-5.0f / 2 * 36, -(nCointOne / 5) * 36);
            }

            for (int i = 0; i < nCointOne; i++)
            {
                if (i > 4)
                {
                    ptCoin = ptNode + new Point((i % 5) * 36, 36);
                }
                else
                {
                    ptCoin = ptNode + new Point(i * 36, 0);
                }

                Sprite sprTenCoin = new Sprite(Root.instance().imageset_manager().imageset("role").image("coin_silver_0"));
                Sprite sprTenCoin1 = new Sprite(Root.instance().imageset_manager().imageset("role").image("coin_silver_0"));
                Sprite sprTenCoin2 = new Sprite(Root.instance().imageset_manager().imageset("role").image("coin_silver_0"));

                sprTenCoin.set_position(ptCoin);
                sprTenCoin1.set_position(ptCoin + new Point(0, 4));
                sprTenCoin2.set_position(ptCoin + new Point(0, 8));

                sprTenCoin1.set_visible(false);
                sprTenCoin2.set_visible(false);

                Color_Rect ColorRect = sprTenCoin1.color();
                ColorRect.set_alpha(150);
                sprTenCoin1.set_color(ColorRect);
                ColorRect.set_alpha(80);
                sprTenCoin2.set_color(ColorRect);

                pGameScene.m_layCoinObject.add_child(sprTenCoin2);
                pGameScene.m_layCoinObject.add_child(sprTenCoin1);
                pGameScene.m_layCoinObject.add_child(sprTenCoin);



                Animation aniCoin = Root.instance().animation_manager().animation("coin_silver");
                sprTenCoin.run_action(new Action_Repeat_Forever(new Action_Animation(0.06, aniCoin, false)));

                GameControls.XLBE.Action actSequence;
                GameControls.XLBE.Action[] actMove = new GameControls.XLBE.Action[5];

                actMove[0] = new Action_Move_To(0.3, ptCoin + new Point(0, -150));
                actMove[1] = new Action_Move_To(0.3, ptCoin + new Point(0, 0));
                actMove[2] = new Action_Move_To(0.1, ptCoin + new Point(0, -15));
                actMove[3] = new Action_Move_To(0.1, ptCoin + new Point(0, 0));
                actMove[4] = new Action_Move_To(0.4, ptRole);

                GameControls.XLBE.Action actFunc = new Action_Func(pGameScene.m_layCoinObject.CoinSilverEnd);
                actFunc.set_tag(tag);

                actSequence = new Action_Sequence(actMove[0], actMove[1], actMove[2], actMove[3], actMove[4], actFunc, null);
                sprTenCoin.run_action(actSequence);

                actMove[0] = new Action_Move_To(0.3, ptCoin + new Point(0, -150));
                actMove[1] = new Action_Move_To(0.3, ptCoin + new Point(0, 0));
                actMove[2] = new Action_Move_To(0.1, ptCoin + new Point(0, -15));
                actMove[3] = new Action_Move_To(0.1, ptCoin + new Point(0, 0));
                actMove[4] = new Action_Move_To(0.4, ptRole);
                actSequence = new Action_Sequence(new Action_Delay(0.04), actMove[0], actMove[1], actMove[2], actMove[3], new Action_Func(CoinAlpha), actMove[4], new Action_Func(pGameScene.m_layCoinObject.CoinSilverEnd), null);
                /*actSequence->set_tag(tag);*/
                sprTenCoin1.run_action(actSequence);

                actMove[0] = new Action_Move_To(0.3, ptCoin + new Point(0, -150));
                actMove[1] = new Action_Move_To(0.3, ptCoin + new Point(0, 0));
                actMove[2] = new Action_Move_To(0.1, ptCoin + new Point(0, -15));
                actMove[3] = new Action_Move_To(0.1, ptCoin + new Point(0, 0));
                actMove[4] = new Action_Move_To(0.4, ptRole);
                actSequence = new Action_Sequence(new Action_Delay(0.08), actMove[0], actMove[1], actMove[2], actMove[3], new Action_Func(CoinAlpha), actMove[4], new Action_Func(pGameScene.m_layCoinObject.CoinSilverEnd), null);
                /*actSequence->set_tag(tag);*/
                sprTenCoin2.run_action(actSequence);

            }

            int nOffset;
            string strPrex;
            bool bGotHead = false;
            int nSingleNumber = 0;
            Sprite[] spValue = { null, null, null, null, null, null, null, null };
            ostringstream ostr = new ostringstream();

            if (tag == pGameScene.GetMeChairID())
            {
                nOffset = 22;
                strPrex = "gold_number_";
            }
            else
            {
                nOffset = 20;
                strPrex = "silver_number_";
            }

            ostr = ostr + strPrex + 10;
            spValue[0] = new Sprite(Root.instance().imageset_manager().imageset("role").image(ostr.str()));

            nSingleNumber = (int)(nCoinCount / 1000000);

            if (nSingleNumber > 0)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + strPrex + nSingleNumber;
                spValue[1] = new Sprite(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(nCoinCount % 1000000 / 100000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + strPrex + nSingleNumber;
                spValue[2] = new Sprite(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(nCoinCount % 100000 / 10000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + strPrex + nSingleNumber;
                spValue[3] = new Sprite(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(nCoinCount % 10000 / 1000);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + strPrex + nSingleNumber;
                spValue[4] = new Sprite(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(nCoinCount % 1000 / 100);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + strPrex + nSingleNumber;
                spValue[5] = new Sprite(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(nCoinCount % 100 / 10);
            if (nSingleNumber > 0 || bGotHead)
            {
                bGotHead = true;
                ostr.str("");
                ostr = ostr + strPrex + nSingleNumber;
                spValue[6] = new Sprite(Root.instance().imageset_manager().imageset("role").image(ostr.str()));
            }

            nSingleNumber = (int)(nCoinCount % 10);
            ostr.str("");
            ostr = ostr + strPrex + nSingleNumber;
            spValue[7] = new Sprite(Root.instance().imageset_manager().imageset("role").image(ostr.str()));

            Point pt = node.position();
            pt.x_ -= 10;
            int nSpace = 58;
            for (int i = 0; i < 8; i++)
            {
                //if(i==0) { delete spValue[0];continue;}

                if (spValue[i] != null)
                {
                    double fSizeBase = (nFishGold >= 10) ? 1.6 : 0.8;
                    int nOffsetTemp = (nFishGold >= 10) ? nOffset + 16 : nOffset;
                    pt.x_ += nOffsetTemp;
                    spValue[i].set_position(pt);
                    pGameScene.m_layCoinObject.add_child(spValue[i]);
                    //GameControls.XLBE.Action *act = new Action_Sequence(new Action_Scale_To(0.25, fSizeBase), new Action_Move_To(0.1, pt+new Point(0, -nSpace)), new Action_Move_To(0.1, pt+new Point(0, -nSpace)), new Action_Move_To(0.1, pt+new Point(0, -nSpace)), new Action_Move_To(0.1, pt+new Point(0, +nSpace)), new Action_Move_To(0.1, pt+new Point(0, +nSpace)), new Action_Move_To(0.1, pt+new Point(0, +nSpace)), new Action_Move_To(0.1, pt+new Point(0, -nSpace)), new Action_Scale_To(0.25, fSizeBase), new Action_Delay(0.5), new Action_Func(rak::mem_fn(pGameScene->m_layCoinObject, &CCoinLayer::CoinValueEnd)), 0);
                    GameControls.XLBE.Action act = new Action_Sequence(new Action_Scale_To(0.25, fSizeBase), new Action_Delay(0.5), new Action_Func(pGameScene.m_layCoinObject.CoinValueEnd), null);
                    spValue[i].run_action(act);
                }
            }

            //remove_child(node);
            //   delete node->node_extend();
            //   delete node;

            return true;
        }
    }
}
