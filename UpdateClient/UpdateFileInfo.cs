using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ChatEngine;

namespace UpdateClient
{
    public class UpdateFileInfo : BaseInfo
    {
        public string FilePath = string.Empty;    // 20
        public int nFileVersion = -1;

        public UpdateFileInfo()
        {
            //_InfoType = InfoType.UpdateFile;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount(FilePath) + EncodeCount(nFileVersion);
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, FilePath);
                EncodeInteger(bw, nFileVersion);
            }
            catch (Exception)
            {
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            try
            {
                base.FromBytes(br);

                FilePath = DecodeString(br);
                nFileVersion = DecodeInteger(br);
            }
            catch { }
        }
    }
}
