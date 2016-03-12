using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameControls
{
    public struct CFont
    {
        public const int ANTIALIASED_QUALITY = 4;

        Font _font;

        public void CreateFont(int cHeight, int cWidth, int cEscapement, int cOrientation, int cWeight, int bItalic,
                             int bUnderline, int bStrikeOut, int iCharSet, int iOutPrecision, int iClipPrecision,
                             int iQuality, int iPitchAndFamily, string pszFaceName)
        {
            _font = new Font(pszFaceName, (float)Math.Abs(cHeight));
        }

        public Font GetFont()
        {
            return _font;
        }
    }
}
