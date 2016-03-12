using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;
using ChatEngine;

namespace FishClient
{
    public class CFishObjectExtend : Node_Extend
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        public CFishObjectExtend()
        {
            wID = FishDefine.INVALID_WORD;
            wRoundID = 0;
            dwMulRate = 5;
            dwCoinCount = 0;
        }
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	public void Dispose();

        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        public int GetFishGoldByStyle()
        {
            return GetFishGoldByStyle(FishType);
        }

	    public Size GetFishObjectSize()
	    {
		    switch (FishType)
		    {
		    case FishDefine.enFishType.FishType_0:
			    return new Size(14,36);
		    case FishDefine.enFishType.FishType_1:
			    return new Size(12,36);
		    case FishDefine.enFishType.FishType_2:
			    return new Size(20,70);
		    case FishDefine.enFishType.FishType_3:
			    return new Size(24,72);
		    case FishDefine.enFishType.FishType_4:
			    return new Size(30,80);
		    case FishDefine.enFishType.FishType_5:
			    return new Size(40,60);
		    case FishDefine.enFishType.FishType_6:
			    return new Size(20,160);
		    case FishDefine.enFishType.FishType_7:
			    return new Size(98,116);
		    case FishDefine.enFishType.FishType_8:
			    return new Size(70,180);
		    case FishDefine.enFishType.FishType_9:
			    return new Size(90,180);
		    case FishDefine.enFishType.FishType_10:
			    return new Size(77,287);
		    case FishDefine.enFishType.FishType_11:
			    return new Size(112,400);
		    case FishDefine.enFishType.FishType_12:
			    return new Size(112,400);
		    case FishDefine.enFishType.FishType_13:
			    return new Size(100,200);
		    case FishDefine.enFishType.FishType_14:
			    return new Size(160,400);
		    case FishDefine.enFishType.FishType_15:
			    return new Size(90,180);
		    default:
			    return new Size(0,0);
		    };
	    }

        public int GetFishGoldByStyle(FishDefine.enFishType FishType)
        {
            switch (FishType)
            {
                case FishDefine.enFishType.FishType_0:
                    return 1;
                case FishDefine.enFishType.FishType_1:
                    return 2;
                case FishDefine.enFishType.FishType_2:
                    return 3;
                case FishDefine.enFishType.FishType_3:
                    return 4;
                case FishDefine.enFishType.FishType_4:
                    return 5;
                case FishDefine.enFishType.FishType_5:
                    return 7;
                case FishDefine.enFishType.FishType_6:
                    return 9;
                case FishDefine.enFishType.FishType_7:
                    return 10;
                case FishDefine.enFishType.FishType_8:
                    return 12;
                case FishDefine.enFishType.FishType_9:
                    return 15;
                case FishDefine.enFishType.FishType_10:
                    return 20;
                case FishDefine.enFishType.FishType_11:
                    return 30 + wRoundID % 21;
                case FishDefine.enFishType.FishType_12:
                    return 40 + wRoundID % 81;
                case FishDefine.enFishType.FishType_13:
                    return 0;
                case FishDefine.enFishType.FishType_14:
                    return 300;
                case FishDefine.enFishType.FishType_15:
                    return 300;
                default:
                    return 0;
            }
        }

        public static bool ComputeCollision(double w, double h, double r, double rx, double ry)
        {
            double dx = Math.Min(rx, w * 0.5f);
            dx = Math.Max(dx, -w * 0.5f);

            double dy = Math.Min(ry, h * 0.5f);
            dy = Math.Max(dy, -h * 0.5f);

            return (rx - dx) * (rx - dx) + (ry - dy) * (ry - dy) <= r * r;
        }

        public int wID;
        public int wRoundID;
        public FishDefine.enFishType FishType;
        public long dwTime;
        public int dwMulRate;
        public uint dwCoinCount;
    }

    public class CBulletObjectExtend : Node_Extend
    {
        public CBulletObjectExtend()
        {
            nBackCount = 0;
            fRotation = 0.0;
            wChairID = FishDefine.INVALID_WORD;
            CannonType = FishDefine.enCannonType.CannonTypeCount;
            dwMulRate = 5;
        }
        public void Dispose()
        {
        }

        public int nBackCount;
        public double fRotation;
        public int wChairID;
        public FishDefine.enCannonType CannonType;
        public int dwMulRate;
    }

    public class CNetObjectExtend : Node_Extend
    {
        public CNetObjectExtend()
        {
            wChairID = FishDefine.INVALID_WORD;
            CannonType = FishDefine.enCannonType.CannonTypeCount;
            dwMulRate = 5;
        }
        public void Dispose()
        {
        }

        public int GetNetRadius()
        {
            return (int)GetRadious();
        }

        double GetRadious()
        {
            switch (CannonType)
            {
                case FishDefine.enCannonType.CannonType_0:
                    return 70 * 0.5;
                case FishDefine.enCannonType.CannonType_1:
                    return 70 * 0.6;
                case FishDefine.enCannonType.CannonType_2:
                    return 70 * 0.7;
                case FishDefine.enCannonType.CannonType_3:
                    return 70 * 0.8;
                case FishDefine.enCannonType.CannonType_4:
                    return 70 * 0.9;
                case FishDefine.enCannonType.CannonType_5:
                    return 70 * 1.0;
                case FishDefine.enCannonType.CannonType_6:
                    return 70 * 1.1;
                case FishDefine.enCannonType.CannonTypeCount:
                    return 0;
                default:
                    return 0;
            }
        }

        public int wChairID;
        public FishDefine.enCannonType CannonType;
        public int dwMulRate;
    }
}
