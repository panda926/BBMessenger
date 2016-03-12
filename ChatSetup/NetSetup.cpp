#include "StdAfx.h"
#include "NetSetup.h"
#include "AsyncDownloader.h"

#include <afxcoll.h>

//#define	KEY		_T("SOFTWARE\\Microsoft\\.NETFramework\\policy")
#define	KEY		_T("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full")
#define	KEY_LEN	256
#define VERSION	3	

DWORD NetSetup::m_SetupNetState = 0;

NetSetup::NetSetup(void)
{
	m_pSetupThread = NULL;
}


NetSetup::~NetSetup(void)
{
}

// Check the subkeys of the registry key HKLM\SOFTWARE\Microsoft\.NETFramework\policy
// FW specific keys will appear as subkeys formatted like this: with a V<major>.<minor>
//bool CheckRegistryKeyExistance(char *fwVersion)
//{
//	HKEY hKey;
//	DWORD dwIndex = 0;
//	DWORD cbName = KEY_LEN;
//	char sz[KEY_LEN];
//	bool result = false;
//	
//	try
//	{
//		if(ERROR_SUCCESS == ::RegOpenKeyEx(HKEY_LOCAL_MACHINE, KEY, 0, KEY_READ, &hKey))
//		{
//			// Iterate through all subkeys
//			while ((ERROR_NO_MORE_ITEMS != ::RegEnumKeyEx(hKey, dwIndex, sz, &cbName, NULL, NULL, NULL, NULL)))
//			{
//				if(!_tcsnicmp(sz, _T("V"), 1))
//					strncpy(fwVersion, sz+1, VERSION + 1);
//				dwIndex++;
//				cbName = KEY_LEN;
//			}
//			::RegCloseKey(hKey);
//			printf("Fraemwork version found: %s\n", fwVersion);
//			result = true;
//		}
//		else
//			result = false;
//		return result;
//	}
//	catch(...)
//	{
//		return false;
//	}
//}

// 2014-03-11: GreenRose
bool CheckRegistryKeyExistance(char *fwVersion)
{
	HKEY hKey;
	DWORD dwIndex = 0;
	DWORD cbName = KEY_LEN;
	char sz[KEY_LEN];
	bool result = false;

	try
	{
		if(ERROR_SUCCESS == ::RegOpenKeyEx(HKEY_LOCAL_MACHINE, KEY, 0, KEY_READ, &hKey))
		{			
			result = true;
		}
		else
			result = false;

		return result;
	}
	catch(...)
	{
		return false;
	}
}

//Compare the existing and required FW version numbers
bool CompareFWVersions( const char *existing, const char *required)
{
	int existingMajor = atoi(existing);
	int existingMinor = atoi(existing+2);
	int requiredMajor = atoi(required);
	int requiredMinor = atoi(required+2);
	bool result = false;

	// very simple comparison algorithm, really
	if(requiredMajor < existingMajor)
		result = true;
	else if(requiredMajor == existingMajor && requiredMinor <= existingMinor)
		result = true;
	else
		result = false;
	return result;
}

BOOL NetSetup::CheckNetVersion()
{
	char fwVersion[VERSION+1];

	// Check if Framework key exists and get the FW version if yes
	if(CheckRegistryKeyExistance(fwVersion))
	{
		// Compare versions

		// GreenRose commented
		/*if(CompareFWVersions(fwVersion, "4.0"))
			return true;*/

		return true;
	}

	return false;
}

BOOL NetSetup::DownloadNet()
{
	return AsyncDownloader::GetInstance()->DownloadFile( "dotNetFx40_Full_x86_x64.exe" );
}

BOOL NetSetup::SetupNet()
{
	if (m_pSetupThread != NULL)
		return true;

		// create a thread to download, but don't start yet
	m_pSetupThread = ::AfxBeginThread(NetSetup::Setup,
											NULL,
											THREAD_PRIORITY_NORMAL,
											0,
											CREATE_SUSPENDED);
	if (m_pSetupThread == NULL)
		return false;


	TRACE(_T("AfxBeginThread: 0x%08lX\n"),
			m_pSetupThread->m_nThreadID);

	/* 
		Set the m_bAutoDelete data member to FALSE. This allows
		the CWinThread object to survive after the thread has been
		terminated. You can then access the m_hThread data member
		after the thread has been terminated. If you use this
		technique, however, you are responsible for destroying the
		CWinThread object as the framework will not automatically
		delete it for you.
	*/
	m_pSetupThread->m_bAutoDelete = FALSE;

	// start
	VERIFY(m_pSetupThread->ResumeThread() == 1);


	// setting timer
	CWnd* pWnd = AsyncDownloader::GetInstance()->m_pParent;

	pWnd->SetTimer( TIMER_SETUPNET, 3000, NULL );

}

CString NetSetup::Unpack()
{
	HMODULE hModule = ::GetModuleHandle( NULL );
	
	HRSRC hSrc = ::FindResource( hModule, "NET_FRAMEWORK", "EMBEDDED_FILE" );
	HGLOBAL hGlobal = ::LoadResource( hModule, hSrc );
	
	const byte* pData = (const byte*)LockResource( hGlobal );
	long nSize = 50449456; //GlobalSize(hGlobal); //; 

	TCHAR lpTempPathBuffer[MAX_PATH];

    //  Gets the temp path env string (no guarantee it's a valid path).
    DWORD dwRetVal = GetTempPath(MAX_PATH,          // length of the buffer
                           lpTempPathBuffer); // buffer for path 

	CString tempFileName;

	if (dwRetVal > 0 && dwRetVal < MAX_PATH )
    {
		tempFileName.Format( "%s\\NetFramework.exe", lpTempPathBuffer );

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

UINT NetSetup::Setup(LPVOID pParam)
{
	CString setupFileName = Unpack();

	STARTUPINFO si;
	PROCESS_INFORMATION pi;

	ZeroMemory( &si, sizeof(STARTUPINFO));
	si.cb = sizeof(STARTUPINFO);

	CString commandStr;
	//commandStr.Format( "\"%s\" /q /norestart /ChainingPackage ADMINDEPLOYMENT", setupFileName );
	commandStr.Format( "\"%s\" /q /ChainingPackage ADMINDEPLOYMENT", setupFileName );

	BOOL isSuccess = CreateProcess ( 
		NULL, // No module name (use command line) 
		commandStr.GetBuffer(0), // I mentioned only 2 arguments.
		NULL,
		NULL, 
		FALSE,
		0, 
		NULL, 
		NULL, 
		&si, 
		&pi ); 
	
	if( isSuccess == false )
	{
		int nError = GetLastError();
		return false;
	}

	WaitForSingleObject( pi.hProcess, INFINITE );

	m_SetupNetState = 0;
	GetExitCodeProcess(pi.hProcess, &m_SetupNetState);

	//3010

	// let the dialog box know it is done
	VERIFY(::PostMessage(AsyncDownloader::GetInstance()->m_pParent->m_hWnd, WM_USER_ENDSETUPNET, 0, 0));

	return 0;
}

void NetSetup::CompleteSetupNet()
{
	CWnd* pWnd = AsyncDownloader::GetInstance()->m_pParent;
	pWnd->KillTimer( TIMER_SETUPNET );

	DWORD dwExitCode;
	if (::GetExitCodeThread(m_pSetupThread->m_hThread, &dwExitCode) &&
		dwExitCode == STILL_ACTIVE)
	{
		::WaitForSingleObject(m_pSetupThread->m_hThread, INFINITE);
	}

	delete m_pSetupThread;
	m_pSetupThread = NULL;
}



