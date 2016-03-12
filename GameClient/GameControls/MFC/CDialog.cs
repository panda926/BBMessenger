using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ChatEngine;

namespace GameControls
{
    public delegate void MessageHandler();

    public struct MessageMap
    {
        public string name;
        public MessageHandler messageHandler;
    }

    public class CDialog : Form
    {
        List<MessageMap> _messageMaps = new List<MessageMap>();
        List<CControl> _controlList = new List<CControl>();

        public void InitDialog()
        {
            DoDataExchange();

            BEGIN_MESSAGE_MAP();

            OnCtlColor();

            OnInitDialog();
        }

        public virtual void DoDataExchange()
        {
        }

        public virtual void BEGIN_MESSAGE_MAP()
        {
        }

        public virtual void OnCtlColor()
        {
        }

        public virtual bool OnInitDialog()
        {
            return true;
        }

        public void DDX_Control(Control parent, int id, CControl child)
        {
            if (child == null)
                return;

            child.Create(parent, id);

            _controlList.Add(child);
        }

        public void ON_BN_CLICKED(int id, MessageHandler messageHandler)
        {
            Control[] findControls = this.Controls.Find(id.ToString(), true);

            if (findControls == null || findControls.Length != 1)
                return;

            findControls[0].MouseDown += new MouseEventHandler(CDialog_MouseDown);

            MessageMap messageMap;
            messageMap.name = id.ToString();
            messageMap.messageHandler = messageHandler;

            _messageMaps.Add(messageMap);
        }

        public void ON_EN_CHANGE(int id, MessageHandler messageHandler)
        {
            Control[] findControls = this.Controls.Find(id.ToString(), true);

            if (findControls == null || findControls.Length != 1)
                return;

            findControls[0].TextChanged += new EventHandler(CDialog_TextChanged); 

            MessageMap messageMap;
            messageMap.name = id.ToString();
            messageMap.messageHandler = messageHandler;

            _messageMaps.Add(messageMap);
        }

        void CDialog_TextChanged(object sender, EventArgs e)
        {
            DispatchMessage(sender);

            //throw new NotImplementedException();
        }

        void CDialog_MouseDown(object sender, MouseEventArgs e)
        {
            DispatchMessage(sender);

            //throw new NotImplementedException();
        }

        void DispatchMessage( object sender )
        {
            Control control = (Control)sender;

            for (int i = 0; i < _messageMaps.Count; i++)
            {
                if (_messageMaps[i].name == control.Name)
                {
                    _messageMaps[i].messageHandler();
                    break;
                }
            }
        }

        public void ASSERT(bool bValid)
        {
            if (bValid == false)
            {
                throw new NotImplementedException();
            }
        }


        public CControl GetDlgItem(int id)
        {
            for (int i = 0; i < _controlList.Count; i++)
            {
                if( _controlList[i].GetDlgCtrlID() == id )
                    return _controlList[i];
            }

            return null;
        }

        public void SetDlgItemText(int id, string text)
        {
            CControl control = GetDlgItem(id);

            if (control == null)
                return;

            control.SetWindowText(text);
        }

        public void ShowWindow(bool bShowWindow)
        {
            this.Visible = bShowWindow;
        }

        public void GetWindowRect(ref CRect rect)
        {
            rect.SetRect(this.Left, this.Top, this.Right, this.Bottom);
        }

        public bool IsWindowVisible()
        {
            return this.Visible;
        }

        public const int SWP_NOSIZE    =      0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOREDRAW = 0x0008;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_FRAMECHANGED = 0x0020; /* The frame changed: send WM_NCCALCSIZE */
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int SWP_HIDEWINDOW = 0x0080;
        public const int SWP_NOCOPYBITS = 0x0100;
        public const int SWP_NOOWNERZORDER = 0x0200; /* Don't do owner Z ordering */
        public const int SWP_NOSENDCHANGING = 0x0400;/* Don't send WM_WINDOWPOSCHANGING */

        public void SetWindowPos(Control parent, int left, int top, int width, int height, int flags)
        {
            if (parent != null)
            {
                Point point = new Point(left, top );
                point = parent.PointToScreen(point);

                left = point.X;
                top = point.Y;
            }

            if ((flags & SWP_NOMOVE) == 0)
            {
                this.Left = left;
                this.Top = top;
            }

            if ((flags & SWP_NOSIZE) == 0)
            {
                this.Width = width;
                this.Height = height;
            }

            Invalidate();
        }

        public string TEXT(string str)
        {
            return str;
        }

        public void SetForegroundWindow()
        {
            this.BringToFront();
        }

        public void GetClientRect(ref CRect rect)
        {
           Rectangle rectangle = this.ClientRectangle;

           rect.SetRect(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }

        public bool GetSafeHwnd()
        {
            return true;
        }

        public int ConverToInt(string str)
        {
            int val = 0;

            try
            {
                val = Convert.ToInt32(str);
            }
            catch { }

            return val;
        }
    }
}
