using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace GameControls
{
    public delegate void ClickHandler(object sender, object param);

    public class CVirtualButton : CVirtualWindow
    {
	    //状态变量
	    protected bool					m_bMouseDown;						//按下标志
	    protected bool					m_bMouseMove;						//经过标志
	    CRect							m_rcButtonRect;						//按钮区域

	    //资源变量
	    string							m_pszTypeName;						//资源类型
	    string							m_pszResource;						//资源信息
	    //HINSTANCE						m_hResInstance;						//资源句柄

	    //动画变量
	    int							    m_wImageIndex;						//过渡索引
	    CLapseCount						m_ImageLapseCount = new CLapseCount();					//流逝计数

	    //资源变量
	    CD3DTexture						m_D3DTextureButton = new CD3DTexture();					//按钮纹理

        public ClickHandler ClickHandler;

	    //函数定义

	    //系统事件
	    //动画消息
	    public override void OnWindowMovie(){}
	    //创建消息
        public override void OnWindowCreate(Device pD3DDevice) { }
	    //销毁消息
        public override void OnWindowDestory(Device pD3DDevice) { }

	    //重载函数
	    
        //鼠标事件
        public override void OnEventMouse(int uMessage, int nFlags, int nXMousePos, int nYMousePos)
        {
        }

	    //按钮事件
        public override void OnEventButton(int uButtonID, int uMessage, int nXMousePos, int nYMousePos)
        {
        }

	    //绘画窗口
        public override void OnEventDrawWindow(Device pD3DDevice, int nXOriginPos, int nYOriginPos)
        {
        }

	    //功能函数
	    //设置区域
        public void SetButtonRect(CRect rcButtonRect)
        {
        }
	    
        //加载位图
        public void SetButtonImage(Device pD3DDevice, Bitmap bitmap)
        {
            m_D3DTextureButton.LoadImage(pD3DDevice, bitmap, string.Empty);
        }

        public CD3DTexture GetD3DTexture() 
        { 
            return m_D3DTextureButton; 
        }

	    //内部函数
	    //调整控件
        public void RectifyControl(Device pD3DDevice)
        {
        }
    }
}
