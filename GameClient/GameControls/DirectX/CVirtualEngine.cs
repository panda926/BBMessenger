using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace GameControls
{
    public class CVirtualEngine
    {
	    //配置变量
	    Device					    m_pD3DDevice;						//设备对象

	    //状态变量
	    CVirtualWindow 				m_pWindowLeave;						//离开窗口
	    CVirtualWindow 				m_pWindowCapture;					//捕获窗口

	    //内核变量
	    List<CVirtualWindow>			m_VirtualWindowPtrArray = new List<CVirtualWindow>();			//窗口数组

	    //函数定义

	    //配置函数
	    //获取设备
	    public Device GetD3DDevice() 
        { 
            return m_pD3DDevice; 
        }

	    //设置设备
	    public void SetD3DDevice(Device pD3DDevice) 
        { 
            m_pD3DDevice=pD3DDevice; 
        }

	    //驱动函数
	    //绘画窗口
        public bool DrawWindows() 
        {
            return true;
        }

	    //消息处理
        bool DefWindowProc(int uMessage, int wParam, int lParam) 
        {
            return true;
        }

	    //消息解释
        bool PreTranslateMessage(int uMessage, int wParam, int lParam)
        {
            return true;
        }

	    //捕获函数
	    //离开注册
        bool RegisterLeave(CVirtualWindow pVirtualWindow, bool bRegister)
        {
            return true;
        }


	    //扑获注册
        bool RegisterCapture(CVirtualWindow pVirtualWindow, bool bRegister)
        {
            return true;
        }

	    //更新函数
	    //更新窗口
        void InvalidWindow() { }

	    //更新窗口
        void InvalidWindow(int nXPos, int nYPos, int nWidth, int nHeight) { }

	    //窗口搜索
	    //获取窗口
	    CVirtualWindow  SwitchToWindow(int nXMousePos, int nYMousePos)
        {
            return null;
        }

	    //获取窗口
        CVirtualWindow SwitchToWindow(CVirtualWindow pVirtualWindow, int nXMousePos, int nYMousePos)
        {
            return null;
        }
    }
}
