#pragma once

#define SETUPNET_SUCCESS		0
#define SETUPNET_CANCELBYUSER	1602
#define SETUPNET_OCCURERROR		1603
#define SETUPNET_RESTART1		1641
#define SETUPNET_RESTART2		3010
#define SETUPNET_NOTENOUGH		5100

class NetSetup
{
public:
	NetSetup(void);
	~NetSetup(void);

	BOOL CheckNetVersion();

	BOOL DownloadNet();
	BOOL SetupNet();

	void CompleteSetupNet();

private:
	static CString Unpack();
	static UINT Setup(LPVOID pParam);

public:
	static DWORD m_SetupNetState;

	CWinThread *m_pSetupThread;

};

