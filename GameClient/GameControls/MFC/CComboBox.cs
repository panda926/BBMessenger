using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GameControls
{
    public class CComboBox : CControl
    {
        public void AddString(string str)
        {
            if (_control == null)
                return;

            ((ComboBox)_control).Items.Add(str);
        }

        public int GetCurSel()
        {
            if (_control == null)
                return -1;

            return ((ComboBox)_control).SelectedIndex;
        }
    }
}
