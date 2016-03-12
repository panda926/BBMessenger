using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class AddScoreInfo : BaseInfo
    {
	    public int								wCurrentUser;						//当前用户
	    public int								wAddScoreUser;						//加注用户
	    public int								lAddScoreCount;						//加注数目
	    public int								lTurnLessScore;						//最少加注
	    public int								lTurnMaxScore;						//最大下注
	    public int								lAddLessScore;						//加最小注
	    public int[]							cbShowHand = new int[DzCardDefine.GAME_PLAYER];			//梭哈用户

        public bool _isGiveUp = false;

        public AddScoreInfo()
        {
            _InfoType = InfoType.AddScore;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(wCurrentUser);
            size += EncodeCount(wAddScoreUser);
            size += EncodeCount(lAddScoreCount);
            size += EncodeCount(lTurnLessScore);
            size += EncodeCount(lTurnMaxScore);
            size += EncodeCount(lAddLessScore);

            for (int i = 0; i < cbShowHand.Length; i++)
                size += EncodeCount(cbShowHand[i]);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger( bw, wCurrentUser );
                EncodeInteger( bw, wAddScoreUser );
                EncodeInteger(bw, lAddScoreCount);
                EncodeInteger(bw, lTurnLessScore);
                EncodeInteger(bw, lTurnMaxScore);
                EncodeInteger(bw, lAddLessScore);

                for (int i = 0; i < cbShowHand.Length; i++)
                    EncodeInteger(bw, cbShowHand[i]);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            try
            {
                base.FromBytes(br);

                wCurrentUser = DecodeInteger(br);
                wAddScoreUser = DecodeInteger(br);
                lAddScoreCount = DecodeInteger(br);
                lTurnLessScore = DecodeInteger(br);
                lTurnMaxScore = DecodeInteger(br);
                lAddLessScore = DecodeInteger(br);

                for (int i = 0; i < cbShowHand.Length; i++)
                    cbShowHand[i] = DecodeInteger(br);
            }
            catch 
            { }
        }
    }
}
