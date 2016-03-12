using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameControls
{
    public enum ButtonState
    {
        Up,
        Down,
        Over,
        Disable
    }

    public partial class PictureButton : UserControl
    {
        Image       _buttonImage;
        Image       _selectImage;

        ButtonState _buttonState = ButtonState.Up;

        public int _ButtonId;

        public PictureButton()
        {
            InitializeComponent();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        public void Dispose()
        {
            if (_buttonImage != null)
                _buttonImage.Dispose();

            if (_selectImage != null)
                _selectImage.Dispose();
        }


        public void Create( bool isVisible, bool isEnable, Rectangle rect, Control parent, int id )
        {
            this.Visible = isVisible;
            this.Enabled = isEnable;

            this.Left = rect.Left;
            this.Top = rect.Top;
            this.Width = rect.Width;
            this.Height = rect.Height;

            _ButtonId = id;

            parent.Controls.Add(this);
        }

        public void ShowWindow(bool isShow)
        {
            this.Visible = isShow;
        }

        public void EnableWindow(bool isEnable)
        {
            this.Enabled = isEnable;
        }

        public Image SelectImage
        {
            get
            {
                return _selectImage;
            }
            set
            {
                _selectImage = value;
            }
        }

        public void SetButtonImage( Image buttonImage )
        {
            _buttonImage = buttonImage;

            Width = buttonImage.Width/5;
            Height = buttonImage.Height;
        }

        public void SetTransparentColor(Color transColor)
        {
            if (_buttonImage == null)
                return;

            //Bitmap bmp = (Bitmap)_buttonImage;
            //bmp.MakeTransparent(transColor);

            // modified by usc at 2014/04/28
            using (Bitmap bmp = new Bitmap(_buttonImage))
            {
                bmp.MakeTransparent(transColor);

                _buttonImage = new Bitmap(bmp);
            }
        }

        // When the mouse button is pressed, set the "pressed" flag to true 
        // and invalidate the form to cause a repaint.  The .NET Compact Framework 
        // sets the mouse capture automatically.
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _buttonState = ButtonState.Down;
            this.Invalidate();
            base.OnMouseDown(e);
        }

        // When the mouse is released, reset the "pressed" flag 
        // and invalidate to redraw the button in the unpressed state.
        protected override void OnMouseUp(MouseEventArgs e)
        {
            _buttonState = ButtonState.Up;
            this.Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_buttonState != ButtonState.Over)
            {
                _buttonState = ButtonState.Over;
                this.Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _buttonState = ButtonState.Up;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        // Override the OnPaint method to draw the background image and the text.
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_buttonImage != null)
            {
                int x = 0;

                if (this.Enabled == false)
                {
                    x = _buttonImage.Width / 5 * 4;
                }
                else
                {
                    switch (_buttonState)
                    {
                        case ButtonState.Up:
                            x = 0;
                            break;

                        case ButtonState.Down:
                            x = _buttonImage.Width / 5;
                            break;

                        case ButtonState.Over:
                            x = _buttonImage.Width / 5 * 2;
                            break;

                        case ButtonState.Disable:
                            x = _buttonImage.Width / 5 * 4;
                            break;
                    }
                }

                Rectangle dstRect = new Rectangle(0, 0, this.Width, this.Height);
                Rectangle srcRect = new Rectangle(x, 0, _buttonImage.Width / 5, _buttonImage.Height);

                e.Graphics.DrawImage(_buttonImage, dstRect, srcRect, GraphicsUnit.Pixel);

            }
            else if (_selectImage != null)
            {
                if (_buttonState == ButtonState.Over || _buttonState == ButtonState.Down)
                {
                    Rectangle dstRect = new Rectangle(0, 0, this.Width, this.Height);
                    Rectangle srcRect = new Rectangle(0, 0, _selectImage.Width, _selectImage.Height);

                    e.Graphics.DrawImage(_selectImage, dstRect, srcRect, GraphicsUnit.Pixel);
                }
            }
            base.OnPaint(e);
        }
    }
}
