using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GameControls
{
    public class CSkinButton : CControl
    {
        public void Dispose()
        {
            if (_control == null)
                return;

            PictureButton picturButton = (PictureButton)_control;
            picturButton.Dispose();
        }

        public bool SetButtonImage(Bitmap uBitmapID, Control parent, bool bRenderImage, bool bTransparent)
        {
            if (_control == null)
                return false;

            PictureButton picturButton = (PictureButton)_control;
            picturButton.SetButtonImage(uBitmapID);

            return true;
        }

    }
}
