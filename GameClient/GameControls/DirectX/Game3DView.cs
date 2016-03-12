using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace GameControls
{
    public class Game3DView : GameView
    {
	    //状态变量
	    protected bool						m_bInitD3D;							//创建标志
	    protected Thread 					m_hRenderThread;					//渲染线程

	    //组件变量
	    //protected CD3DFont				m_D3DFont;							//字体对象
	    //protected CD3DDirect				m_D3DDirect;						//环境对象
	    public Device				        m_D3DDevice;						//设备对象
	    protected CVirtualEngine			m_VirtualEngine = new CVirtualEngine();					//虚拟引擎
	    //protected CD3DFont				m_D3DRollFont;						//滚动字体

        public Game3DView()
        {
        }

        protected override void InitGameView()
        {
            OnInitDevice();

            InitGame3DView();
        }


        //6603配置设备
        protected void OnInitDevice()
        {
            // init directX
            PresentParameters pp = new PresentParameters();

            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;

            m_D3DDevice = new Device(0,
                                DeviceType.Hardware,
                                this,
                                CreateFlags.SoftwareVertexProcessing,
                                pp);

	        //6603设置变量
	        m_bInitD3D=true;

	        //6603获取对象
	        //ASSERT(CSkinResourceManager::GetInstance()!=NULL);
	        //CSkinResourceManager * pSkinResourceManager=CSkinResourceManager::GetInstance();

	        //6603获取字体
            //LOGFONT LogFont;
            //pSkinResourceManager->GetDefaultFont().GetLogFont(&LogFont);

	        //6603创建对象
            //m_D3DFont.CreateFont(LogFont,0L);

            //LOGFONT LogRollFont;
            //ZeroMemory(&LogRollFont,sizeof(LogRollFont));
            //LogRollFont.lfHeight=20;
            //_sntprintf(LogRollFont.lfFaceName,CountArray(LogRollFont.lfFaceName),TEXT("黑体"));
            //LogRollFont.lfWeight=200;
            //LogRollFont.lfCharSet=134;
            //m_D3DRollFont.CreateFont(LogRollFont,0L);

	        //6603虚拟设备
	        m_VirtualEngine.SetD3DDevice(m_D3DDevice);

	        //6603设置头像
            //CGameFrameAvatar * pGameFrameAvatar=CGameFrameAvatar::GetInstance();
            //if (pGameFrameAvatar!=NULL) pGameFrameAvatar->Initialization(pD3DDevice);

	        //6603加载资源
            //HINSTANCE hInstance=GetModuleHandle(GAME_FRAME_DLL_NAME);
            //m_D3DTextureReady.LoadImage(pD3DDevice,hInstance,TEXT("USER_READY"),TEXT("PNG"));
            //m_D3DTextureMember.LoadImage(pD3DDevice,hInstance,TEXT("MEMBER_FLAG"),TEXT("PNG"));
            //m_D3DTextureClockItem.LoadImage(pD3DDevice,hInstance,TEXT("USER_CLOCK_ITEM"),TEXT("PNG"));
            //m_D3DTextureClockBack.LoadImage(pD3DDevice,hInstance,TEXT("USER_CLOCK_BACK"),TEXT("PNG"));

	        //6603配置窗口
	        //InitGameView(pD3DDevice,nWidth,nHeight);

	        return;
        }

        //6603渲染线程
        protected void StartRenderThread()
        {
	        //6603效验状态
	        //ASSERT(m_hRenderThread==NULL);
	        if (m_hRenderThread!=null) 
                return;

	        //6603创建线程
            m_hRenderThread = new Thread(D3DRenderThread);
            m_hRenderThread.Start();
	        return;
        }

        //6603渲染线程
        void D3DRenderThread()
        {
	        //6603效验参数
	        //ASSERT(pThreadData!=NULL);
	        //if (pThreadData==NULL) return;

	        int nPaintingTime = 0;
	        
            //6603渲染循环
	        while (true)
	        {
		        //6603渲染等待
		        if( nPaintingTime >= 15 )
		        {
			        Thread.Sleep(1);
		        }
		        else
		        {
			        Thread.Sleep(15 - nPaintingTime);
		        }
        		
		        //6603发送消息
                Stopwatch stopwatch = Stopwatch.StartNew();

                this.Invoke((MethodInvoker)delegate
                {
                    OnRenderWindow();
                });

		        stopwatch.Stop();

                nPaintingTime = (int)stopwatch.Elapsed.TotalMilliseconds;

		        //TRACE(TEXT(" F [%d] \n"), nPaintingTime);
	        }
        }

        //6603渲染窗口
        void OnRenderWindow()
        {
            m_D3DDevice.Clear(ClearFlags.Target, System.Drawing.Color.FromArgb(255, 255, 255).ToArgb(), 1.0f, 0);

            m_D3DDevice.BeginScene();

	        //6603动画驱动
	        UpdateGameView();

            int nWidth = this.Width;
            int nHeight = this.Height;

	        //6603绘画窗口
	        DrawGameView(nWidth,nHeight);

	        //6603虚拟框架
	        m_VirtualEngine.DrawWindows();

	        //6603设置变量
	        //m_dwDrawCurrentCount++;

	        //6603累计判断
            //if ((GetTickCount()-m_dwDrawBenchmark)>=1000L)
            //{
            //    //6603设置变量
            //    m_dwDrawLastCount=m_dwDrawCurrentCount;

            //    //6603统计还原
            //    m_dwDrawCurrentCount=0L;
            //    m_dwDrawBenchmark=GetTickCount();
            //}

            m_D3DDevice.EndScene();

            m_D3DDevice.Present();

	        return;
        }

        protected virtual void UpdateGameView() 
        { 
        }

        protected virtual void DrawGameView(int width, int height)
        {
        }

        protected virtual void InitGame3DView()
        {
        }
    }
}
