using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ChatClient
{
    class ErrorCollection
    {
        private static ErrorCollection _instance = null;

        public static ErrorCollection GetInstance()
        {
            if (_instance == null)
                _instance = new ErrorCollection();

            return _instance;
        }

        public void SetErrorInfo(string strErrorMessage)
        {
            try
            {
                string strAppPath = ".\\";
                string strErrorFilePath = strAppPath + "Error.txt";

                FileStream fileStream = new FileStream(strErrorFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] wb = null;

                wb = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}{1}< {2}>{3}", DateTime.Now.ToString(), Environment.NewLine, strErrorMessage, Environment.NewLine));

                fileStream.Seek(0, SeekOrigin.End);
                fileStream.Write(wb, 0, wb.Length); //파일의 끝에 문자열을 붙인다.
                fileStream.Close();
            }
            catch (Exception)
            {                
            }
        }
    }
}
