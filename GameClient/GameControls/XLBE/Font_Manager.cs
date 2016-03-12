using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace GameControls.XLBE
{
    public class Font_Manager
    {
        Dictionary<string, Font> Fonts;
        List<Font> font_ttfs_ = new List<Font>();

        public Font_Manager()
        {
        }

        public Font create_font_ttf(string name, string file)
        {
            Font font = new Font( name );
            return font;
            //System.Drawing.Font gdiFont = new System.Drawing.Font( name, 10.0f, System.Drawing.FontStyle.Regular);

            //Microsoft.DirectX.Direct3D.Font d3dFont = new Microsoft.DirectX.Direct3D.Font (device_, gdiFont);
            //Microsoft.DirectX.Direct3D.Sprite sprite = new Microsoft.DirectX.Direct3D.Sprite(device_);

            //return new Font(name, sprite, d3dFont);
        }

        //void destory_font_ttf(const std::string& name);
        //void destory_font_ttf(Font *font);
        //Font *font_ttf(const std::string& name);

    }
}
