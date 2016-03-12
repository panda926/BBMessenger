using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace UpdateServer
{
    public class UpdateCheckInfo : BaseInfo
    {
        public int nUserID = -1;
        public List<UpdateFileInfo> FileList = new List<UpdateFileInfo>();

        public UpdateCheckInfo()
        {
            _InfoType = InfoType.UpdateCheck;
        }


        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(nUserID);
            size += 4;

            foreach (UpdateFileInfo fileInfo in FileList)
                size += fileInfo.GetSize();

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);
                EncodeInteger(bw, nUserID);
                EncodeInteger(bw, FileList.Count);

                foreach (UpdateFileInfo fileInfo in FileList)
                    fileInfo.GetBytes(bw);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            nUserID = DecodeInteger(br);
            int fileCount = DecodeInteger(br);

            for (int i = 0; i < fileCount; i++)
            {
                UpdateFileInfo fileInfo = (UpdateFileInfo)BaseInfo.CreateInstance(br);
                FileList.Add(fileInfo);
            }
        }

    }
}
