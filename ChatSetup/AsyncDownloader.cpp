#include "StdAfx.h"
#include "BSCallbackImpl.h"
#include "AsyncDownloader.h"

#include <wininet.h>

AsyncDownloader* AsyncDownloader::m_pInstance = NULL;
CWnd* AsyncDownloader::m_pParent = NULL;
CString AsyncDownloader::m_DownloadRoot;

AsyncDownloader* AsyncDownloader::GetInstance()
{
	if( m_pInstance == NULL )
	{
		m_pInstance = new AsyncDownloader();
	}

	return m_pInstance;
}

void AsyncDownloader::SetParent( CWnd* pParent )
{
	m_pParent = pParent;
}

void AsyncDownloader::SetDownloadRoot( CString downloadRoot )
{
	m_DownloadRoot = downloadRoot;
}

AsyncDownloader::AsyncDownloader()
{
	m_pDownloadThread = NULL;
}


AsyncDownloader::~AsyncDownloader(void)
{
}

//-----------------------------------------------------------------------------
// static member function - the controlling function for the worker thread
// pParam: DOWNLOADPARAM *
//		   {
//			   HWND hWnd - IN				the window handle to display status
//			   HANDLE hEventStop - IN		the event object to signal to stop
//			   CString strURL - IN			the URL of the file to be downloaded
//			   CString strFileName - OUT	the filename of the downloaded file
//		   }
// return value: not used
UINT AsyncDownloader::Download(LPVOID pParam)
{
	DOWNLOADPARAM *const pDownloadParam = static_cast<DOWNLOADPARAM *>(pParam);
	ASSERT_POINTER(pDownloadParam, DOWNLOADPARAM);

	ASSERT(::IsWindow(pDownloadParam->hWnd));

	/*
		URLDownloadToCacheFile is a blocking function. Even though the data is
		downloaded asynchronously the function does not return until all the
		data is downloaded. If complete asynchronous downloading is desired,
		one of the other UOS functions, such as URLOpenStream, or perhaps
		general URL monikers would be more appropriate.
	*/

	::DeleteUrlCacheEntry(pDownloadParam->strURL);

	CBSCallbackImpl bsc(pDownloadParam->hWnd, pDownloadParam->hEventStop);

	const HRESULT hr = ::URLDownloadToCacheFile(NULL,
												pDownloadParam->strURL,
												pDownloadParam->strFileName.
													GetBuffer(MAX_PATH),
												URLOSTRM_GETNEWESTVERSION,
												0,
												&bsc);

	/*
		The resource from the cache is used for the second and subsequent
		calls to URLDownloadToCacheFile during the session of the program
		unless the following setting is selected, in which case, every call
		to URLDownloadToCacheFile downloads the resource from the Internet.

		Control Panel/Internet/General/Temporary Internet files/Settings/
		Check for newer versions of stored pages -> Every visit to the page
	*/

	// empty the filename string if failed or canceled
	pDownloadParam->strFileName.ReleaseBuffer(SUCCEEDED(hr) ? -1 : 0);

	TRACE(_T("URLDownloadToCacheFile ends: 0x%08lX\nCache file name: %s\n"),
		  hr, pDownloadParam->strFileName);

	// let the dialog box know it is done
	VERIFY(::PostMessage(pDownloadParam->hWnd, WM_USER_ENDDOWNLOAD, 0, 0));

	return 0;
}

BOOL AsyncDownloader::DownloadFile( char* url )
{
	if (m_pDownloadThread == NULL)
	{
		// Download
		// retrieve the URL
		m_strURL = m_DownloadRoot + url;

		// check if the URL is valid
		USES_CONVERSION;
		if (m_strURL.IsEmpty() ||
			::IsValidURL(NULL, T2CW(m_strURL), 0) != S_OK)
		{
			return false;
		}

		// parameters to be passed to the thread
		m_downloadParam.hWnd = m_pParent->GetSafeHwnd();

		m_eventStop.ResetEvent();  // nonsignaled
		m_downloadParam.hEventStop = m_eventStop;

		m_downloadParam.strURL = m_strURL;

		m_downloadParam.strFileName.Empty();

		// create a thread to download, but don't start yet
		m_pDownloadThread = ::AfxBeginThread(AsyncDownloader::Download,
											 &m_downloadParam,
											 THREAD_PRIORITY_NORMAL,
											 0,
											 CREATE_SUSPENDED);
		if (m_pDownloadThread != NULL)
		{
			TRACE(_T("AfxBeginThread: 0x%08lX\n"),
				  m_pDownloadThread->m_nThreadID);

			/* 
				Set the m_bAutoDelete data member to FALSE. This allows
				the CWinThread object to survive after the thread has been
				terminated. You can then access the m_hThread data member
				after the thread has been terminated. If you use this
				technique, however, you are responsible for destroying the
				CWinThread object as the framework will not automatically
				delete it for you.
			*/
			m_pDownloadThread->m_bAutoDelete = FALSE;

			// start
			VERIFY(m_pDownloadThread->ResumeThread() == 1);
		}
		else
		{
			//::AfxMessageBox(IDS_ERRCREATETHREAD, MB_OK | MB_ICONSTOP);
		}
	}
	else
	{
		// Stop
		VERIFY(m_eventStop.SetEvent());  // signaled
	}

	return true;
}

BOOL AsyncDownloader::CompleteDownload()
{
	DWORD dwExitCode;
	if (::GetExitCodeThread(m_pDownloadThread->m_hThread, &dwExitCode) &&
		dwExitCode == STILL_ACTIVE)
	{
		::WaitForSingleObject(m_pDownloadThread->m_hThread, INFINITE);
	}

	delete m_pDownloadThread;
	m_pDownloadThread = NULL;

	// display the result
	if (m_downloadParam.strFileName.IsEmpty())
		return false;

	return true;
}

