using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GameControls
{
    public class CEdit : CControl
    {
        public void SetBorderStyle(BorderStyle borderStyle)
        {
            if (_control == null)
                return;

            TextBox textBox = (TextBox)_control;

            textBox.BorderStyle = borderStyle;
        }

        public void SetTextAlign( HorizontalAlignment align )
        {
            if (_control == null)
                return;

            TextBox textBox = (TextBox)_control;

            textBox.TextAlign = align;
        }

    }
}
