using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameControls.XLBE
{
    public class ostringstream
    {
        private string str_;

        public ostringstream()
        {
        }

        public ostringstream(string str)
        {
            str_ = str;
        }

        public string str()
        {
            return str_;
        }

        public void str(string str)
        {
            str_ = str;
        }

        public static ostringstream operator +(ostringstream ImpliedObject, string str)
        {
            return new ostringstream(ImpliedObject.str() + str);
        }

        public static ostringstream operator +(ostringstream ImpliedObject, int value)
        {
            return new ostringstream(ImpliedObject.str() + value.ToString());
        }
    }
}
