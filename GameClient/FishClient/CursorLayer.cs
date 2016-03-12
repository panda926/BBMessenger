using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameControls.XLBE;

namespace FishClient
{
    public class CCursorLayer : Layer
    {
        public CCursorLayer()
        {
            m_sprCursor = new Sprite();
            add_child(m_sprCursor);
        }
        public void Dispose()
        {
        }

        public void SetCursor(Image cursor)
        {
            m_sprCursor.set_display_image(cursor);
            //Size contentSize = m_sprCursor.content_size();

            //Point pt = new Point(-contentSize.width_ / 2, -contentSize.height_ / 2);
            //m_sprCursor.set_position(pt);
        }

        private Sprite m_sprCursor;

    }
}
