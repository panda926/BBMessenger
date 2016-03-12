using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GameControls
{
    public struct CPoint
    {
        Point _point;

        public CPoint(int x, int y)
        {
            _point = new Point();

            _point.X = x;
            _point.Y = y;
        }

        public void SetPoint(int x, int y)
        {
            if( _point == null )
                _point = new Point();

            _point.X = x;
            _point.Y = y;
        }

        public int x
        {
            get
            {
                if (_point == null)
                    _point = new Point();

                return _point.X;
            }
            set
            {
                if (_point == null)
                    _point = new Point();

                _point.X = value;
            }
        }

        public int y
        {
            get
            {
                if (_point == null)
                    _point = new Point();

                return _point.Y;
            }
            set
            {
                if (_point == null)
                    _point = new Point();

                _point.Y = value;
            }
        }

    }
}
