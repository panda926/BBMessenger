
// ChatSetupDlg.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "ChatSetup.h"
#include "ChatSetupDlg.h"
#include "afxdialogex.h"
#include "AsyncDownloader.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CChatSetupDlg �Ի���




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


// CChatSetupDlg ��Ϣ�������

BOOL CChatSetupDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// ���ô˶Ի����ͼ�ꡣ��Ӧ�ó��������ڲ��ǶԻ���ʱ����ܽ��Զ�
	//  ִ�д˲���
	SetIcon(m_hIcon, TRUE);			// ���ô�ͼ��
	SetIcon(m_hIcon, FALSE);		// ����Сͼ��

	// TODO: �ڴ���Ӷ���ĳ�ʼ������
	m_ProgressControl.SetRange( 0, 100 );

	SetSetupStep( SETUP_START );

	return TRUE;  // ���ǽ��������õ��ؼ������򷵻� TRUE
}

// �����Ի��������С����ť������Ҫ����Ĵ���
//  �����Ƹ�ͼ�ꡣ����ʹ���ĵ�/��ͼģ�͵� MFC Ӧ�ó���
//  �⽫�ɿ���Զ���ɡ�

void CChatSetupDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // ���ڻ��Ƶ��豸������

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// ʹͼ���ڹ����������о���
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// ����ͼ��
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
			m_StateLabel.SetWindowTextA( "׼�����" );
			m_ProgressControl.SetPos(0);

			if( m_NetSetup.DownloadNet() == false )
				SetError( "�޷�׼����ܡ��˳�����" );
		}
		break;

	case SETUP_NET:
		{
			m_StateLabel.SetWindowTextA( "��װ���" );
			m_ProgressControl.SetPos(0);

			if( m_NetSetup.SetupNet() == false )
				SetError( "�޷���װ���" );
		}
		break;

	case DOWNLOAD_PROGRAM:
		{
			m_StateLabel.SetWindowTextA( "׼�����" );
			m_ProgressControl.SetPos(0);

			if( m_ProgramSetup.DownloadProgram() == false )
				SetError( "�޷�׼�����" );
		}
		break;

	case SETUP_PROGRAM:
		{
			m_StateLabel.SetWindowTextA( "��װ���" );
			m_ProgressControl.SetPos(0);

			if( m_ProgramSetup.SetupProgram() == false )
				SetError( "�޷���װ���" );
		}
		break;

	case SETUP_END:
		{
			SetupEnd();

			if (MessageBox( "��װ�ɹ����. �������Ͽ�ʼ��","ѯ��",MB_YESNO)==IDYES) 
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

	sShortcutPath.Format(_T("%s\\��ҹ��.lnk"), szPath);

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
		SetError("��׼����װ");
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
		SetError( "�û�ȡ���˰�װ" );
		return 0;
	case SETUPNET_OCCURERROR:
		SetError( "�ڰ�װ�����г�������������");
		return 0;
	case SETUPNET_NOTENOUGH:
		SetError( "�û��ļ����������ϵͳҪ��");
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
		SetError( "�޷���װ����" );
		return 0;
	}

	m_ProgramSetup.CompleteSetupProgram();

	SetSetupStep( SETUP_END );

	return 0;
}


void CChatSetupDlg::SetError( CString errorString )
{
	MessageBox( errorString,"����",MB_OK);
	AfxGetMainWnd()->PostMessage( WM_CLOSE, 0, 0 );
}



//���û��϶���С������ʱϵͳ���ô˺���ȡ�ù��
//��ʾ��
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
