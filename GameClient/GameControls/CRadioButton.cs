using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GameControls
{
    public class CRadioButton : CControl
    {
        public bool GetCheck()
        {
            if (_control == null)
                return false;

            return ((RadioButton)_control).Checked;
        }

        public void SetCheck(bool value)
        {
            if (_control == null)
                return;

            ((RadioButton)_control).Checked = value;
        }
    }
}
