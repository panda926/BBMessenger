using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GameControls
{
    public class CControl
    {
        public Control _control;

        public void Create( Control parent, int id )
        {
            if (_control != null)
                return;

            Type type = this.GetType();

            if (type.Equals(typeof(CSkinButton)))
            {
                _control = new PictureButton();
            }
            else if (type.Equals(typeof(CButton)))
            {
                _control = new Button();
            }
            else if (type.Equals(typeof(CEdit)))
            {
                _control = new TextBox();
            }
            else if (type.Equals(typeof(CListCtrl)))
            {
                _control = new ListView();
            }
            else if (type.Equals(typeof(CRadioButton)))
            {
                _control = new RadioButton();
            }
            else if (type.Equals(typeof(CComboBox)))
            {
                _control = new ComboBox();
            }

            if (_control == null)
                return;

            _control.Name = string.Format("{0}", id);

            parent.Controls.Add(_control);
        }

        public void Create(string name, bool isVisible, bool isEnable, CRect rect, Control parent, int id)
        {
            if (_control != null)
                return;

            Create(parent, id);

            ShowWindow(isVisible);
            EnableWindow(isEnable);
            MoveWindow(rect);
        }

        public void EnableWindow(bool bEnable)
        {
            if (_control == null)
                return;

            _control.Enabled = bEnable;
        }

        public void MoveWindow(int x, int y, int width, int height)
        {
            if (_control == null)
                return;

            _control.Left = x;
            _control.Top = y;
            _control.Width = width;
            _control.Height = height;

            _control.Invalidate();
        }

        public void MoveWindow(CRect rect)
        {
            Rectangle rectangle = rect.GetRect();

            MoveWindow(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public void GetWindowRect(ref CRect rect)
        {
            if (_control == null)
                return;

            rect.SetRect(_control.Left, _control.Top, _control.Right, _control.Bottom);
        }

        public void ShowWindow(bool isShow)
        {
            if (_control == null)
                return;

            _control.Visible = isShow;
        }

        public void SetTextColor(Color textColor)
        {
            if (_control == null)
                return;

            _control.ForeColor = textColor;
        }

        public string GetWindowText()
        {
            if (_control == null)
                return string.Empty;

            return _control.Text;
        }

        public void SetWindowText(string text)
        {
            if (_control == null)
                return;

            _control.Text = text;
        }

        public int GetDlgCtrlID()
        {
            if (_control == null)
                return -1;

            int id = -1;

            try
            {
                id = Convert.ToInt32(_control.Name);
            }
            catch { }

            return id;
        }

        public IntPtr Handle
        {
            get
            {
                if (_control == null)
                    return (IntPtr)0;

                return _control.Handle;
            }
        }

        public void Invalidate()
        {
            if (_control == null)
                return;

            _control.Invalidate();
        }

        public bool GetSafeHwnd()
        {
            if (_control == null)
                return false;

            return true;
        }

        public void SetBkColor(Color color)
        {
            if (_control == null)
                return;

            _control.BackColor = color;
        }

    }
}
