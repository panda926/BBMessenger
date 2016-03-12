using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class DWGameFileInfo : BaseInfo
    {
        public List<string> listGameFile = new List<string>();
        public GameInfo gameInfo = new GameInfo();

        public DWGameFileInfo()
        {
            _InfoType = InfoType.DWGameFile;
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4;

            for (int i = 0; i < listGameFile.Count; i++)
            {
                size += EncodeCount(listGameFile[i]);
            }

            size += gameInfo.GetSize();

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, listGameFile.Count);

                for (int i = 0; i < listGameFile.Count; i++)
                {
                    EncodeString(bw, listGameFile[i]);
                }

                gameInfo.GetBytes(bw);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            int nDWFileCount = DecodeInteger(br);

            for (int i = 0; i < nDWFileCount; i++)
            {
                //DecodeInteger(br);

                string strFileName = DecodeString(br);
                listGameFile.Add(strFileName);
            }

            gameInfo = (GameInfo)BaseInfo.CreateInstance(br);
        }
    }
}
