using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Input;
using System.Windows.Threading;
using System.Diagnostics;

namespace ChatClient
{
    public class BaseWindow : Window
    {
   

        #region 常量
        private const int RESIZE_BORDER = 8;
        private const int HIDE_BORDER = 3;
        private const int FLASH_TIME = 500;
        private const int MAGNET_BORDER = 20;
        #endregion

        #region 变量
        private HwndSource _HwndSource;
        private bool _IsHidded = false;
        #endregion

        #region 构造函数
        public BaseWindow()
        {
            this.SourceInitialized += delegate(object sender, EventArgs e)
            {
                this._HwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            };
        }
        #endregion

        #region 事件
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Win32.POINT p;
            if (!Win32.GetCursorPos(out p))
                return;
          
            if (this.Left + RESIZE_BORDER < p.x && this.Left + this.ActualWidth - RESIZE_BORDER > p.x && this.Top + RESIZE_BORDER < p.y && this.Top + this.ActualHeight - RESIZE_BORDER > p.y)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                   
                }
            }
            base.OnMouseLeftButtonDown(e);
        }

       

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Win32.POINT p;
            if (!Win32.GetCursorPos(out p))
                return;

            //Debug.WriteLine("this.top:" + this.Top.ToString());
            //Debug.WriteLine("p.y:" + p.y.ToString());
            //Debug.WriteLine("是否隐藏了:" + this._IsHidded.ToString());

            if (this.Left <= p.x && this.Left + RESIZE_BORDER >= p.x
                && this.Top <= p.y && this.Top + RESIZE_BORDER >= p.y)
            {
                //this.Cursor = Cursors.SizeNWSE;
                this.Cursor = Cursors.Arrow;
                if (e.LeftButton == MouseButtonState.Pressed)
                    Win32.SendMessage(_HwndSource.Handle, 0x112, (IntPtr)(61444), IntPtr.Zero);
            }
            else if (this.Left <= p.x && this.Left + RESIZE_BORDER >= p.x
                && this.Top + this.ActualHeight - RESIZE_BORDER <= p.y && this.Top + this.ActualHeight >= p.y)
            {
                //this.Cursor = Cursors.SizeNESW;
                this.Cursor = Cursors.Arrow;
                if (e.LeftButton == MouseButtonState.Pressed)
                    Win32.SendMessage(_HwndSource.Handle, 0x112, (IntPtr)(61447), IntPtr.Zero);
            }
            else if (this.Left + this.ActualWidth - RESIZE_BORDER <= p.x && this.Left + this.ActualWidth >= p.x
                && this.Top <= p.y && this.Top + RESIZE_BORDER >= p.y)
            {
                //this.Cursor = Cursors.SizeNESW;
                this.Cursor = Cursors.Arrow;
                if (e.LeftButton == MouseButtonState.Pressed)
                    Win32.SendMessage(_HwndSource.Handle, 0x112, (IntPtr)(61445), IntPtr.Zero);
            }
            else if (this.Left + this.ActualWidth - RESIZE_BORDER <= p.x && this.Left + this.ActualWidth >= p.x
                && this.Top + this.ActualHeight - RESIZE_BORDER <= p.y && this.Top + this.ActualHeight >= p.y)
            {
                //this.Cursor = Cursors.SizeNWSE;
                this.Cursor = Cursors.Arrow;
                if (e.LeftButton == MouseButtonState.Pressed)
                    Win32.SendMessage(_HwndSource.Handle, 0x112, (IntPtr)(61448), IntPtr.Zero);
            }
            else if (this.Top <= p.y && this.Top + RESIZE_BORDER >= p.y)
            {
                //this.Cursor = Cursors.SizeNS;
                this.Cursor = Cursors.Arrow;
                if (e.LeftButton == MouseButtonState.Pressed)
                    Win32.SendMessage(_HwndSource.Handle, 0x112, (IntPtr)(61443), IntPtr.Zero);
            }
            else if (this.Left <= p.x && this.Left + RESIZE_BORDER >= p.x)
            {
                //this.Cursor = Cursors.SizeWE;
                this.Cursor = Cursors.Arrow;
                if (e.LeftButton == MouseButtonState.Pressed)
                    Win32.SendMessage(_HwndSource.Handle, 0x112, (IntPtr)(61441), IntPtr.Zero);
            }
            else if (this.Top + this.ActualHeight - RESIZE_BORDER <= p.y && this.Top + this.ActualHeight >= p.y)
            {
                //this.Cursor = Cursors.SizeNS;
                this.Cursor = Cursors.Arrow;
                if (e.LeftButton == MouseButtonState.Pressed)
                    Win32.SendMessage(_HwndSource.Handle, 0x112, (IntPtr)(61446), IntPtr.Zero);
            }
            else if (this.Left + this.ActualWidth - RESIZE_BORDER <= p.x && this.Left + this.ActualWidth >= p.x)
            {
                    //this.Cursor = Cursors.SizeWE;
                this.Cursor = Cursors.Arrow;
                    if (e.LeftButton == MouseButtonState.Pressed)
                        Win32.SendMessage(_HwndSource.Handle, 0x112, (IntPtr)(61442), IntPtr.Zero);
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }

            if (this.WindowState == WindowState.Normal)
            {
                if (this._IsHidded)
                {

                    if (this.Top < 0)
                    {
                        this.Top = 0;
                        this.Topmost = false;
                    }
                    else
                    {
                        _IsHidded = false;
                    }
                }
                else
                {
                    if (this.Top < 0 && this.Left < 0)
                    {
                        this.Left = 0;
                        this.Top = HIDE_BORDER - this.ActualHeight;
                        this._IsHidded = true;
                        this.Topmost = true;
                    }
                    else if (this.Top <0 && this.Left >= SystemParameters.VirtualScreenWidth - this.ActualWidth)
                    {
                        this.Left = SystemParameters.VirtualScreenWidth - this.ActualWidth;
                        this.Top = HIDE_BORDER - this.ActualHeight;
                        this._IsHidded = true;
                        this.Topmost = true;
                    }
                    else if (this.Top < 0)
                    {
                        this.Top = HIDE_BORDER - this.ActualHeight;
                        this._IsHidded = true;
                        this.Topmost = true;
                    }
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                if (this.Top <= 0)
                {
                    this.Top = HIDE_BORDER - this.ActualHeight;
                    this._IsHidded = true;
                    this.Topmost = true;
                }

            }
            base.OnMouseLeave(e);
        }
        #endregion
    }
}
