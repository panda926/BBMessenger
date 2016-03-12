using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace GameControls
{
    abstract public class CVirtualWindow
    {
        public const int WS_VISIBLE = 1;
        public const int WS_DISABLED = 2;

        public const int WM_LBUTTONDOWN = 100;
        public const int WM_LBUTTONDBLCLK = 101;

	    //窗口属性
	    bool							m_bActive;							//激活标志
	    protected bool					m_bEnable;							//启用标志
	    bool							m_bVisible;							//显示标志

	    //属性变量
	    int							m_uWindowID;						//窗口标识
	    int							m_uLayerIndex;						//窗口等级

	    //位置变量
	    CSize							m_WindowSize;						//窗口大小
	    CPoint							m_BenchmarkPos;						//基准位置

	    //内核变量
	    protected CVirtualWindow 		m_pParentWindow;					//上层窗口
	    protected CVirtualEngine 		m_pVirtualEngine;					//虚拟框架
	    //CVirtualWindowPtrArray			m_VirtualWindowPtrArray;			//窗口数组

	    //函数定义

	    //获取标识
	    int GetWindowID() 
        { 
            return m_uWindowID; 
        }

	    //设置标识
	    void SetWindowID(int uWindowID) 
        { 
            m_uWindowID=uWindowID; 
        }

	    //窗口等级
	    
        //设置等级
        void SetLayerIndex(int uLayerIndex)
        {
        }
	    
        //获取等级
	    int GetLayerIndex() 
        { 
            return m_uLayerIndex; 
        }

	    //属性对象
	    //上层窗口
	    CVirtualWindow  GetParentWindow() 
        { 
            return m_pParentWindow; 
        }

	    //虚拟框架
	    CVirtualEngine  GetVirtualEngine() 
        { 
            return m_pVirtualEngine; 
        }

	    //管理函数
	    //删除窗口
        void DeleteWindow() { }
	    
        //激活窗口
	    public bool ActiveWindow(CRect rcWindow, int dwStyle, int uWindowID, CVirtualEngine pVirtualEngine, CVirtualWindow pParentWindow)
        {
            m_BenchmarkPos.x = rcWindow.left;
            m_BenchmarkPos.y = rcWindow.top;

            m_WindowSize.cx = rcWindow.Width();
            m_WindowSize.cy = rcWindow.Height();

            if( (dwStyle & WS_VISIBLE) > 0 )
                m_bVisible = true;

            if( (dwStyle & WS_DISABLED) == 0 )
                m_bEnable = true;

            m_uWindowID = uWindowID;

            m_pVirtualEngine = pVirtualEngine;

            return true;
        }
        
    
    

	    //窗口属性
	    //激活属性
	    bool IsWindowActive() 
        { 
            return m_bActive; 
        }

	    //控制属性
	    bool IsWindowEnable() 
        { 
            return m_bEnable; 
        }

	    //显示属性
	    bool IsWindowVisible() 
        { 
            return m_bVisible; 
        }

	    //窗口控制
	    //显示窗口
        public void ShowWindow(bool bVisible) 
        {
            m_bVisible = bVisible;
        }


	    //禁止窗口
        public void EnableWindow(bool bEnable) 
        {
            m_bEnable = bEnable;
        }
	    
        //更新窗口
        void InvalidWindow(bool bCoerce) { }

	    //窗口位置
	    
        //窗口大小
	    void GetClientRect( ref CRect rcClient)
        {
            rcClient.left = 0;//m_BenchmarkPos.x;
            rcClient.top = 0;//m_BenchmarkPos.y;
            rcClient.right = rcClient.left + m_WindowSize.cx;
            rcClient.bottom = rcClient.bottom + m_WindowSize.cy;
        }

	    //窗口大小
	    public void GetWindowRect( ref CRect rcWindow)
        {
            rcWindow.left = m_BenchmarkPos.x;
            rcWindow.top = m_BenchmarkPos.y;
            rcWindow.right = rcWindow.left + m_WindowSize.cx;
            rcWindow.bottom = rcWindow.bottom + m_WindowSize.cy;
        }

	    //设置位置
        public void SetWindowPos(int nXPos, int nYPos, int nWidth, int nHeight, int uFlags) 
        {
            if ((uFlags & GameGraphics.SWP_NOSIZE) > 0)
            {
                m_BenchmarkPos.x = nXPos;
                m_BenchmarkPos.y = nYPos;
            }
        }

	    //功能函数
	    //下属窗口
        bool IsChildWindow(CVirtualWindow pVirtualWindow)
        {
            return false;
        }

	    //系统事件
	    //动画消息
	    public virtual void OnWindowMovie() {}
	    //创建消息
	    public virtual void OnWindowCreate(Device  pD3DDevice) {}
	    //销毁消息
	    public virtual void OnWindowDestory(Device  pD3DDevice) {}
	    //位置消息
	    public virtual void OnWindowPosition(Device  pD3DDevice) {}

	    //重载函数
	    //鼠标事件
        public abstract void OnEventMouse(int uMessage, int nFlags, int nXMousePos, int nYMousePos);
	    //按钮事件
        public abstract void OnEventButton(int uButtonID, int uMessage, int nXMousePos, int nYMousePos);
	    //绘画窗口
        public abstract void OnEventDrawWindow(Device pD3DDevice, int nXOriginPos, int nYOriginPos);

	    //内部函数
        //重置窗口
        void ResetWindow() { }

	    //绘画窗口
        void OnEventDrawChildWindow(Device pD3DDevice, int nXOriginPos, int nYOriginPos) { }
    }
}
