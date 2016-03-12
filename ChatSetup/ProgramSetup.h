#pragma once

#include "Unzipper.h"

class ProgramSetup
{
public:
	ProgramSetup(void);
	~ProgramSetup(void);

	BOOL DownloadProgram();
	BOOL SetupProgram();

	void CompleteSetupProgram();

private:
	static CString Unpack();
	static UINT Extract(LPVOID pParam);

public:
	static DWORD m_SetupProgramState;
	static CString m_SetupFolder;

	CWinThread *m_pExtractThread;
};

