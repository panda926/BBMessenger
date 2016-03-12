using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class SendCardInfo : BaseInfo
    {
	    public int 							cbPublic;							//是否公牌
	    public int 							wCurrentUser;						//当前用户
	    public int 							cbSendCardCount;					//发牌数目
	    public int[]						cbCenterCardData = new int[DzCardDefine.MAX_CENTERCOUNT];	//中心扑克	

        public SendCardInfo()
        {
            _InfoType = InfoType.SendCard;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(cbPublic);
            size += EncodeCount(wCurrentUser);
            size += EncodeCount(cbSendCardCount);

            for (int i = 0; i < cbCenterCardData.Length; i++)
                size += EncodeCount(cbCenterCardData[i]);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, cbPublic);
                EncodeInteger(bw, wCurrentUser);
                EncodeInteger(bw, cbSendCardCount);

                for (int i = 0; i < cbCenterCardData.Length; i++)
                    EncodeInteger(bw, cbCenterCardData[i]);
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

                cbPublic = DecodeInteger(br);
                wCurrentUser = DecodeInteger(br);
                cbSendCardCount = DecodeInteger(br);

                for (int i = 0; i < cbCenterCardData.Length; i++)
                    cbCenterCardData[i] = DecodeInteger(br);
            }
            catch 
            { }
        }
    }
}
