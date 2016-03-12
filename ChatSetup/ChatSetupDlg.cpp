
// ChatSetupDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "ChatSetup.h"
#include "ChatSetupDlg.h"
#include "afxdialogex.h"
#include "AsyncDownloader.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CChatSetupDlg 对话框




CChatSetupDlg::CChatSetupDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CChatSetupDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CChatSetupDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_PROGRESS1, m_ProgressControl);
	DDX_Control(pDX, IDC_SETUPSTATE, m_StateLabel);
}

BEGIN_MESSAGE_MAP(CChatSetupDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_MESSAGE(WM_USER_DISPLAYSTATUS, OnDisplayStatus)
	ON_MESSAGE(WM_USER_ENDDOWNLOAD, OnEndDownload)
	ON_MESSAGE(WM_USER_ENDSETUPNET, OnEndSetupNet)
	ON_MESSAGE(WM_USER_ENDSETUPPROGRAM, OnEndSetupProgram)
	ON_WM_TIMER()
END_MESSAGE_MAP()


// CChatSetupDlg 消息处理程序

BOOL CChatSetupDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码
	m_ProgressControl.SetRange( 0, 100 );

	SetSetupStep( SETUP_START );

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CChatSetupDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作区矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

void CChatSetupDlg::SetSetupStep( int nStep )
{
	m_Step = nStep;

	switch( m_Step )
	{
	case SETUP_START:
		{
			AsyncDownloader::SetParent( this );
			AsyncDownloader::SetDownloadRoot( "http://www.z-114.com/download/");

			if( m_NetSetup.CheckNetVersion() == false )
				SetSetupStep( SETUP_NET );
			else
				SetSetupStep( SETUP_PROGRAM );
		}
		break;

	case DOWNLOAD_NET:
		{
			m_StateLabel.SetWindowTextA( "准备框架" );
			m_ProgressControl.SetPos(0);

			if( m_NetSetup.DownloadNet() == false )
				SetError( "无法准备框架。退出程序" );
		}
		break;

	case SETUP_NET:
		{
			m_StateLabel.SetWindowTextA( "安装框架" );
			m_ProgressControl.SetPos(0);

			if( m_NetSetup.SetupNet() == false )
				SetError( "无法安装框架" );
		}
		break;

	case DOWNLOAD_PROGRAM:
		{
			m_StateLabel.SetWindowTextA( "准备软件" );
			m_ProgressControl.SetPos(0);

			if( m_ProgramSetup.DownloadProgram() == false )
				SetError( "无法准备软件" );
		}
		break;

	case SETUP_PROGRAM:
		{
			m_StateLabel.SetWindowTextA( "安装软件" );
			m_ProgressControl.SetPos(0);

			if( m_ProgramSetup.SetupProgram() == false )
				SetError( "无法安装软件" );
		}
		break;

	case SETUP_END:
		{
			SetupEnd();

			if (MessageBox( "安装成功完成. 你想马上开始？","询问",MB_YESNO)==IDYES) 
			{
				CString sPath;
				sPath.Format("%sUpdate\\UpdateClient.exe", ProgramSetup::m_SetupFolder);
				ShellExecute(NULL, "Open", sPath, NULL, ProgramSetup::m_SetupFolder, SW_SHOW );

				AfxGetMainWnd()->PostMessage( WM_CLOSE, 0, 0 );
			}
			else 
			{
				AfxGetMainWnd()->PostMessage( WM_CLOSE, 0, 0 );
			}

		}
		break;
	}
}

void CChatSetupDlg::SetupEnd()
{
	m_ProgressControl.SetPos(100);

	TCHAR szPath[MAX_PATH] = {0};
	SHGetSpecialFolderPath(NULL, szPath, CSIDL_COMMON_DESKTOPDIRECTORY, FALSE);


	HRESULT hRet = E_FAIL;
	CComPtr<IShellLink> ipShellLink;

	CoInitialize(NULL);

	// Get a pointer to the IShellLink interface
	hRet = CoCreateInstance(CLSID_ShellLink, NULL, CLSCTX_INPROC_SERVER, IID_IShellLink, (void**)&ipShellLink);

	if ( FAILED(hRet) )
		return;

	CString sPath;
	CString sShortcutPath;
	CString sWorkDir;

	sPath.Format("%sUpdate\\UpdateClient.exe", ProgramSetup::m_SetupFolder);

	// Get a pointer to the IPersistFile interface
	CComQIPtr<IPersistFile> ipPersistFile(ipShellLink);

	// Set the path to the shortcut target and add the description
	if ( FAILED((hRet = ipShellLink->SetPath(sPath))) )
	return;

	// if ( FAILED((hRet = ipShellLink->SetIconLocation(sShortcutPath, 0))) )
	// return;
	//
	// if ( FAILED((hRet = ipShellLink->SetDescription(_T("Test")))) )
	// return;

	sWorkDir = ProgramSetup::m_SetupFolder;

	if ( FAILED((hRet = ipShellLink->SetWorkingDirectory(sWorkDir))) )
		return;

	sShortcutPath.Format(_T("%s\\不夜城.lnk"), szPath);

	wchar_t wsz[MAX_PATH];
	MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, sShortcutPath, -1, (LPWSTR)wsz,MAX_PATH);
	ipPersistFile->Save( (LPOLESTR)wsz, TRUE);

	CoUninitialize();
}

//-----------------------------------------------------------------------------
// sent from the downloading thread to the dialog box to display the progress
// wParam: not used
// lParam: DOWNLOADSTATUS *, NULL if canceled
//		   {
//			   ULONG ulProgress - IN
//			   ULONG ulProgressMax - IN
//			   ULONG ulStatusCode - IN
//			   LPCWSTR szStatusText - IN
//		   }
// return value: not used
LRESULT CChatSetupDlg::OnDisplayStatus(WPARAM, LPARAM lParam)
{
	const AsyncDownloader::DOWNLOADSTATUS *const pDownloadStatus =
		reinterpret_cast<AsyncDownloader::DOWNLOADSTATUS *>(lParam);

	// form the status text
	CString strStatus;

	if (pDownloadStatus != NULL)
	{
		int nStep = 0;

		if( pDownloadStatus->ulProgressMax > 0 )
			nStep = pDownloadStatus->ulProgress * 100 / pDownloadStatus->ulProgressMax;

		m_ProgressControl.SetPos( nStep );
	}

	return 0;
}

//-----------------------------------------------------------------------------
// sent when the downloading thread ends
// wParam and lParam: not used
// return value: not used
LRESULT CChatSetupDlg::OnEndDownload(WPARAM, LPARAM)
{
	if( AsyncDownloader::GetInstance()->CompleteDownload() == false )
	{
		SetError("不准备安装");
		return 0;
	}

	if( m_Step == DOWNLOAD_NET )
		SetSetupStep( SETUP_NET );
	else if( m_Step == DOWNLOAD_PROGRAM )
		SetSetupStep( SETUP_PROGRAM );

	return 0;
}

LRESULT CChatSetupDlg::OnEndSetupNet(WPARAM, LPARAM)
{
	switch( NetSetup::m_SetupNetState )
	{
	case SETUPNET_CANCELBYUSER:
		SetError( "用户取消了安装" );
		return 0;
	case SETUPNET_OCCURERROR:
		SetError( "在安装过程中出现了致命错误");
		return 0;
	case SETUPNET_NOTENOUGH:
		SetError( "用户的计算机不符合系统要求");
		return 0;
	}

	m_NetSetup.CompleteSetupNet();

	SetSetupStep( SETUP_PROGRAM );

	return 0;
}

LRESULT CChatSetupDlg::OnEndSetupProgram(WPARAM, LPARAM)
{
	if( ProgramSetup::m_SetupProgramState == 0 )
	{
		SetError( "无法安装程序" );
		return 0;
	}

	m_ProgramSetup.CompleteSetupProgram();

	SetSetupStep( SETUP_END );

	return 0;
}


void CChatSetupDlg::SetError( CString errorString )
{
	MessageBox( errorString,"错误",MB_OK);
	AfxGetMainWnd()->PostMessage( WM_CLOSE, 0, 0 );
}



//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR CChatSetupDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CChatSetupDlg::OnTimer(UINT_PTR nIDEvent)
{
	// TODO: Add your message handler code here and/or call default
	int nPos = ( m_ProgressControl.GetPos() + 1 ) % 100;

	m_ProgressControl.SetPos( nPos );

	CDialogEx::OnTimer(nIDEvent);
}
