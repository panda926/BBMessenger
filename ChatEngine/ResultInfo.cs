using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class ResultInfo : BaseInfo
    {
        private ErrorType _ErrorType;

        public ResultInfo()
        {
            _InfoType = InfoType.Result;
        }

        public ErrorType ErrorType
        {
            get
            {
                return _ErrorType;
            }
            set
            {
                _ErrorType = value;
            }
        }

        override public int GetSize()
        {
            return base.GetSize() + 4;
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, (int)_ErrorType );
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            _ErrorType = (ErrorType)DecodeInteger(br);
        }
    }

    public class ErrorInfo : BaseInfo
    {
        private ErrorType _ErrorType = ErrorType.None;
        private string _ErrorString = "";

        public ErrorType ErrorType
        {
            get
            {
                return _ErrorType;
            }
            set
            {
                _ErrorType = value;
            }
        }

        public string ErrorString
        {
            get
            {
                return _ErrorString;
            }
            set
            {
                _ErrorString = value;
            }
        }
    }
}
