using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatEngine
{
    public class NicknameInfo : BaseInfo
    {
        public int Id = 0;

        public string Nickname = "";
        public string AutoId = "";
        public DateTime StartTime;
        public DateTime EndTime;

        public bool IsValid()
        {
            if (Id <= 0)
                return false;

            if (string.IsNullOrEmpty(Nickname))
                return false;

            return true;
        }
    }
}
