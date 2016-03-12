
// ChatSetupDlg.h : ͷ�ļ�
//

#pragma once

#include "NetSetup.h"
#include "ProgramSetup.h"

enum
{
	SETUP_START = 0,
	DOWNLOAD_NET = 1,
	SETUP_NET = 2,
	DOWNLOAD_PROGRAM = 3,
	SETUP_PROGRAM = 4,
	SETUP_END = 5
};

// CChatSetupDlg �Ի���
class CChatSetupDlg : public CDialogEx
{
// ����
public:
	CChatSetupDlg(CWnd* pParent = NULL);	// ��׼���캯��

// �Ի�������
	enum { IDD = IDD_CHATSETUP_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��

	void SetSetupStep( int nStep );
	void SetError( CString errorString );
	void SetupEnd();

// ʵ��
protected:
	HICON m_hIcon;

	int m_Step; 
	
	NetSetup m_NetSetup;
	ProgramSetup m_ProgramSetup;

	// ���ɵ���Ϣӳ�亯��
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg LRESULT OnDisplayStatus(WPARAM, LPARAM lParam);
	afx_msg LRESULT OnEndDownload(WPARAM, LPARAM);
	afx_msg LRESULT OnEndSetupNet(WPARAM, LPARAM);
	afx_msg LRESULT OnEndSetupProgram(WPARAM, LPARAM);
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	CProgressCtrl m_ProgressControl;
	CStatic m_StateLabel;
};
