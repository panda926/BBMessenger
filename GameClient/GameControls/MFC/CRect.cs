using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameControls
{
    public struct CRect
    {
        Rectangle _rect;

        public CRect(int left, int top, int right, int bottom)
        {
            _rect = new Rectangle();

            SetRect(left, top, right, bottom);
        }

        public void SetRect(int left, int top, int right, int bottom)
        {
            if (_rect == null)
                _rect = new Rectangle();

            _rect.X = left;
            _rect.Y = top;
            _rect.Width = right - left;
            _rect.Height = bottom - top;
        }

        public Rectangle GetRect()
        {
            if (_rect == null)
                return new Rectangle();

            return _rect;
        }

        public void OffsetRect(int x, int y)
        {
            if (_rect == null)
                return;

            _rect.Offset(x, y);
        }

        public bool PtInRect(CPoint point)
        {
            if (_rect == null)
                return false;

            if (point.x < _rect.Left)
                return false;
            if (point.x > _rect.Right)
                return false;

            if (point.y < _rect.Top)
                return false;
            if (point.y > _rect.Bottom)
                return false;

            return true;
        }


        public int left
        {
            get
            {
                if (_rect == null)
                    return 0;

                return _rect.Left;
            }
            set
            {
                if (_rect == null)
                    return;

                _rect.X = value;
            }
        }

        public int top
        {
            get
            {
                if (_rect == null)
                    return 0;

                return _rect.Top;
            }
            set
            {
                if (_rect == null)
                    return;

                _rect.Y = value;
            }
        }

        public int right
        {
            get
            {
                if (_rect == null)
                    return 0;

                return _rect.Right;
            }
            set
            {
                if (_rect == null)
                    return;

                _rect.Width = value - _rect.Left;
            }
        }

        public int bottom
        {
            get
            {
                if (_rect == null)
                    return 0;

                return _rect.Bottom;
            }
            set
            {
                if (_rect == null)
                    return;

                _rect.Height = value - _rect.Top;
            }
        }


        public int Width()
        {
            if (_rect == null)
                return 0;

            return _rect.Width;
        }

        public int Height()
        {
            if (_rect == null)
                return 0;

            return _rect.Height;
        }
    }
}
