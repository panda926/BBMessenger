#pragma once
class AsyncDownloader
{
private:
	static AsyncDownloader* m_pInstance;
	static CString m_DownloadRoot;

public:
	static CWnd* m_pParent;

	static AsyncDownloader* GetInstance();
	static void SetParent( CWnd* pParent );
	static void SetDownloadRoot( CString downloadRoot );

	AsyncDownloader();
	~AsyncDownloader(void);

	BOOL DownloadFile( char* url );
	BOOL CompleteDownload();

public:
	struct DOWNLOADPARAM
	{
		HWND hWnd;
		HANDLE hEventStop;
		CString strURL;
		CString strFileName;
	};

	struct DOWNLOADSTATUS
	{
		ULONG ulProgress;
		ULONG ulProgressMax;
		ULONG ulStatusCode;
		LPCWSTR szStatusText;
	};

	CWinThread *m_pDownloadThread;
	DOWNLOADPARAM m_downloadParam;
	CEvent m_eventStop;

	CString	m_strURL;

private:
	static UINT Download(LPVOID pParam);
};


