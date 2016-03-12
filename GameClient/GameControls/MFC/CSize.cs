using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GameControls;

namespace GameControls
{
    public struct CSize
    {
        Size _size;

        public CSize(int width, int height)
        {
            _size = new Size();

            _size.Width = width;
            _size.Height = height;
        }

        public void SetSize(int width, int height)
        {
            if (_size == null)
                _size = new Size();

            _size.Width = width;
            _size.Height = height;
        }

        public int cx
        {
            get
            {
                if (_size == null)
                    _size = new Size();

                return _size.Width;
            }
            set
            {
                if (_size == null)
                    _size = new Size();

                _size.Width = value;
            }
        }

        public int cy
        {
            get
            {
                if (_size == null)
                    _size = new Size();

                return _size.Height;
            }
            set
            {
                if (_size == null)
                    _size = new Size();

                _size.Height = value;
            }
        }

    }
}
