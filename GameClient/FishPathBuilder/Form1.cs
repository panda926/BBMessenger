using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FishPathBuilder
{
    public partial class Form1 : Form
    {
        private List<List<Key_Frame>> m_SmallFishPaths = new List<List<Key_Frame>>();
        private List<List<Key_Frame>> m_BigFishPaths = new List<List<Key_Frame>>();
        private List<List<Key_Frame>> m_HugeFishPaths = new List<List<Key_Frame>>();
        private List<List<Key_Frame>> m_SpecialFishPaths = new List<List<Key_Frame>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MakePath();
        }

        private void MakePath()
        {
            m_SmallFishPaths.Clear();
            m_BigFishPaths.Clear();
            m_HugeFishPaths.Clear();
            m_SpecialFishPaths.Clear();

            for (int i = 0; i < 70; i++)
            {
                m_SmallFishPaths.Add(GenerateRandomFrame(6, 7));
            }

            SavePathFile("SmallFishPath.pak", m_SmallFishPaths);

            for (int i = 0; i < 40; i++)
            {
                m_BigFishPaths.Add(GenerateRandomFrame(6, 6));
            }

            SavePathFile("BigFishPath.pak", m_BigFishPaths);

            for (int i = 0; i < 20; i++)
            {
                m_HugeFishPaths.Add(GenerateRandomFrame(5, 5));
            }

            SavePathFile("HugeFishPath.pak", m_HugeFishPaths);

            for (int i = 0; i < 10; i++)
            {
                m_SpecialFishPaths.Add(GenerateRandomFrame(8, 8));
            }

            SavePathFile("SpecialFishPath.pak", m_SpecialFishPaths);

        }

        public bool SavePathFile( string name, List<List<Key_Frame>> pathData )
        {
            FileStream file = new FileStream(name, FileMode.Create); 

            BinaryWriter write_fs = new BinaryWriter(file);

            write_fs.Write( pathData.Count);

            for( int i = 0; i < pathData.Count; i++ )
            {
                List<Key_Frame> frames = pathData[i];

                write_fs.Write( frames.Count );

                for( int k = 0; k < frames.Count; k++ )
                {
                    Key_Frame keyFrame = frames[k];

                    write_fs.Write( keyFrame.angle_ );
                    write_fs.Write( keyFrame.position_.x_ );
                    write_fs.Write( keyFrame.position_.y_ );
                }
            }

            write_fs.Close();
            file.Close();

            return true;
        }

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

        Random random = new Random();

        private Point makeEndPt()
        {
            int startDirect = random.Next() % 4;
            Point startPt = new Point();

            if (startDirect == 0)
            {
                startPt.x_ = m_dwSmallFishBase[random.Next() % 28, random.Next() % 6];
                startPt.y_ = -600 - random.Next() % 800;
            }
            else if (startDirect == 1)
            {
                startPt.x_ = m_dwSmallFishBase[random.Next() % 28, random.Next() % 6];
                startPt.y_ = 1024 + random.Next() % 800;
            }
            else if (startDirect == 2)
            {
                startPt.x_ = -600 - random.Next() % 800;
                startPt.y_ = m_dwSmallFishBase[random.Next() % 28, random.Next() % 6];
            }
            else if (startDirect == 3)
            {
                startPt.x_ = 1300 + random.Next() % 800;
                startPt.y_ = m_dwSmallFishBase[random.Next() % 28, random.Next() % 6];
            }

            return startPt;
        }

        private Point makeMiddlePt(Point basePt, int distance)
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

        private List<Key_Frame> GenerateRandomFrame(int minspeed, int maxspeed)
        {
            List<Key_Frame> frames = new List<Key_Frame>();
            int speed = random.Next(minspeed, maxspeed);

            Point startPoint = makeEndPt();
            //Point startPoint = makeScreenPoint();

            List<Point> pointPath = new List<Point>();

            while (frames.Count < 3000)
            {
                Point screenPoint = makeScreenPoint();

                int xdist = (int)(screenPoint.x_ - startPoint.x_);
                int ydist = (int)(screenPoint.y_ - startPoint.y_);
                double offAngle = Math.Atan2(ydist, xdist);

                double angle = (double)((random.Next() % 180) * Math.PI / 180 - Math.PI / 2);
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

        private void button2_Click(object sender, EventArgs e)
        {
            if( m_SmallFishPaths.Count <= 0 )
            {
                MessageBox.Show( "Create Path first.");
                return;
            }

            Graphics g = Graphics.FromHwnd(this.Handle);

            Point startPt = m_SmallFishPaths[0][0].position_;

            for (int i = 1; i < m_SmallFishPaths[0].Count; i++)
            {
                Point endPt = m_SmallFishPaths[0][i].position_;

                g.DrawLine( Pens.Black, new System.Drawing.Point( (int)startPt.x_, (int)startPt.y_ ), new System.Drawing.Point( (int)endPt.x_, (int)endPt.y_ ));
                startPt = endPt;
            }
        }

    }

    public class Key_Frame
    {
        public Key_Frame()
        {
            this.angle_ = 0F;
        }
        public Key_Frame(Point position, double angle)
        {
            this.position_ = position;
            this.angle_ = angle;
        }
        public void Dispose()
        {
        }

        public double angle_;
        public Point position_ = new Point();
    }

    public class Point
    {
        public Point()
        {
            this.x_ = 0F;
            this.y_ = 0F;
        }
        public Point(double x, double y)
        {
            this.x_ = x;
            this.y_ = y;
        }
        public Point(Point point)
        {
            this.x_ = point.x_;
            this.y_ = point.y_;
        }
        public void Dispose()
        {
        }

        public void offset(double x, double y)
        {
            x_ += x;
            y_ += y;
        }

        public void set_point(double x, double y)
        {
            x_ = x;
            y_ = y;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool operator == (const Point &point) const
        public static bool operator ==(Point ImpliedObject, Point point)
        {
            if ((object)ImpliedObject == null && (object)point == null)
                return true;
            if ((object)ImpliedObject != null || (object)point != null)
                return false;

            return (ImpliedObject.x_ == point.x_ && ImpliedObject.y_ == point.y_);
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool operator != (const Point &point) const
        public static bool operator !=(Point ImpliedObject, Point point)
        {
            if ((object)ImpliedObject == null && (object)point == null)
                return false;
            if ((object)ImpliedObject != null || (object)point != null)
                return true;

            return (ImpliedObject.x_ != point.x_ || ImpliedObject.y_ != point.y_);
        }

        //C++ TO C# CONVERTER NOTE: This 'CopyFrom' method was converted from the original C++ copy assignment operator:
        //ORIGINAL LINE: Point &operator = (const Point &point)
        public Point CopyFrom(Point point)
        {
            x_ = point.x_;
            y_ = point.y_;
            return this;
        }

        //C++ TO C# CONVERTER TODO TASK: The += operator cannot be overloaded in C#:
        //public static void operator += (Point point)
        //{
        //    x_ += point.x_;
        //    y_ += point.y_;
        //}
        //C++ TO C# CONVERTER TODO TASK: The -= operator cannot be overloaded in C#:
        //public static void operator -= (Point point)
        //{
        //    x_ -= point.x_;
        //    y_ -= point.y_;
        //}

        public static Point operator +(Point ImpliedObject, Point point)
        {
            return new Point(ImpliedObject.x_ + point.x_, ImpliedObject.y_ + point.y_);
        }
        public static Point operator -(Point ImpliedObject, Point point)
        {
            return new Point(ImpliedObject.x_ - point.x_, ImpliedObject.y_ - point.y_);
        }
        public static Point operator -(Point ImpliedObject)
        {
            return new Point(-ImpliedObject.x_, -ImpliedObject.y_);
        }

        public static Point operator *(Point ImpliedObject, double multip)
        {
            return new Point(ImpliedObject.x_ * multip, ImpliedObject.y_ * multip);
        }

        public double x_;
        public double y_;
    }

}
