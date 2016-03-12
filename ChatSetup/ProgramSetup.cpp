#include "StdAfx.h"
#include "ProgramSetup.h"
#include "AsyncDownloader.h"


DWORD ProgramSetup::m_SetupProgramState = 0;
CString ProgramSetup::m_SetupFolder = "C:\\HBMessenger\\";

ProgramSetup::ProgramSetup(void)
{
	m_pExtractThread = NULL;
}


ProgramSetup::~ProgramSetup(void)
{
}

BOOL ProgramSetup::DownloadProgram()
{
	return AsyncDownloader::GetInstance()->DownloadFile( "Program.Zip" );
}

BOOL ProgramSetup::SetupProgram()
{
	if (m_pExtractThread != NULL)
		return true;

		// create a thread to download, but don't start yet
	m_pExtractThread = ::AfxBeginThread(ProgramSetup::Extract,
											NULL,
											THREAD_PRIORITY_NORMAL,
											0,
											CREATE_SUSPENDED);
	if (m_pExtractThread == NULL)
		return false;


	TRACE(_T("AfxBeginThread: 0x%08lX\n"),
			m_pExtractThread->m_nThreadID);

	/* 
		Set the m_bAutoDelete data member to FALSE. This allows
		the CWinThread object to survive after the thread has been
		terminated. You can then access the m_hThread data member
		after the thread has been terminated. If you use this
		technique, however, you are responsible for destroying the
		CWinThread object as the framework will not automatically
		delete it for you.
	*/
	m_pExtractThread->m_bAutoDelete = FALSE;

	// start
	VERIFY(m_pExtractThread->ResumeThread() == 1);


	// setting timer
	CWnd* pWnd = AsyncDownloader::GetInstance()->m_pParent;

	pWnd->SetTimer( TIMER_EXTRACT, 500, NULL );
}


CString ProgramSetup::Unpack()
{
	HMODULE hModule = ::GetModuleHandle( NULL );
	
	HRSRC hSrc = ::FindResource( hModule, "CLIENT_PROGRAM", "EMBEDDED_FILE" );
	HGLOBAL hGlobal = ::LoadResource( hModule, hSrc );
	
	const byte* pData = (const byte*)LockResource( hGlobal );
	long nSize = 27134757;//22323921;//34245479;//34227834; //GlobalSize(hGlobal); //; 	 

	TCHAR lpTempPathBuffer[MAX_PATH];

    //  Gets the temp path env string (no guarantee it's a valid path).
    DWORD dwRetVal = GetTempPath(MAX_PATH,          // length of the buffer
                           lpTempPathBuffer); // buffer for path 

	CString tempFileName;

	if (dwRetVal > 0 && dwRetVal < MAX_PATH )
    {
		tempFileName.Format( "%s\\Program.zip", lpTempPathBuffer );

		CFile file;
		if( file.Open( tempFileName, CFile::modeCreate|CFile::modeWrite ))
		{
			long index = 0;
			
			while( index < nSize )
			{
				int nCount = 100000;

				if( index + nCount > nSize )
					nCount = nSize - index;
				
				file.Write( pData+index, nCount); 

				index += nCount;
			}

			file.Close();
		}
    }

	::FreeResource( hGlobal );	

	return tempFileName;
}

UINT ProgramSetup::Extract(LPVOID pParam)
{
	CString programZipName = Unpack();

	m_SetupProgramState = 0;

	CUnzipper m_uz;

	if (m_uz.OpenZip(programZipName))
	{
		int totalCount = m_uz.GetFileCount();

		if (m_uz.Unzip(m_SetupFolder.GetBuffer(0)))
			m_SetupProgramState = 1;
	}

	// let the dialog box know it is done
	VERIFY(::PostMessage(AsyncDownloader::GetInstance()->m_pParent->m_hWnd, WM_USER_ENDSETUPPROGRAM, 0, 0));

	return 0;
}

void ProgramSetup::CompleteSetupProgram()
{
	CWnd* pWnd = AsyncDownloader::GetInstance()->m_pParent;
	pWnd->KillTimer( TIMER_EXTRACT );

	DWORD dwExitCode;
	if (::GetExitCodeThread(m_pExtractThread->m_hThread, &dwExitCode) &&
		dwExitCode == STILL_ACTIVE)
	{
		::WaitForSingleObject(m_pExtractThread->m_hThread, INFINITE);
	}

	delete m_pExtractThread;
	m_pExtractThread = NULL;
}



