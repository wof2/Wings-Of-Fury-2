// Launcher.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include <shellapi.h>

const TCHAR *g_szNetfx20RegKeyName = _T("Software\\Microsoft\\NET Framework Setup\\NDP\\v2.0.50727");
const TCHAR *g_szNetfxStandardRegValueName = _T("Install");

bool RegistryGetValue(HKEY, const TCHAR*, const TCHAR*, DWORD, LPBYTE, DWORD);
bool IsNetfx20Installed();

bool RegistryGetValue(HKEY hk, const TCHAR * pszKey, const TCHAR * pszValue, DWORD dwType, LPBYTE data, DWORD dwSize)
{
	HKEY hkOpened;

	// Try to open the key
	if (RegOpenKeyEx(hk, pszKey, 0, KEY_READ, &hkOpened) != ERROR_SUCCESS)
	{
		return false;
	}

	// If the key was opened, try to retrieve the value
	if (RegQueryValueEx(hkOpened, pszValue, 0, &dwType, (LPBYTE)data, &dwSize) != ERROR_SUCCESS)
	{
		RegCloseKey(hkOpened);
		return false;
	}

	// Clean up
	RegCloseKey(hkOpened);

	return true;
}

bool IsNetfx20Installed()
{
	bool bRetValue = false;
	DWORD dwRegValue=0;

	if (RegistryGetValue(HKEY_LOCAL_MACHINE, g_szNetfx20RegKeyName, g_szNetfxStandardRegValueName, NULL, (LPBYTE)&dwRegValue, sizeof(DWORD)))
	{
		if (1 == dwRegValue)
			bRetValue = true;
	}

	return bRetValue;
}


int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
	char dir[255];
	char dirFilename[255];
	GetCurrentDirectoryA(255,dir);

	if(!IsNetfx20Installed())
	{
		MessageBox(0,L".NET framework 2.0 is required.",L"Wings of Fury launching error!",MB_ICONINFORMATION);
		return 0;
	}

	strcat(dir,"\\bin\\Release");
	strncpy(dirFilename,dir,255);
	strcat(dirFilename,"\\Wof.exe");

	ShellExecuteA(0,"open",dirFilename,"",dir,SW_SHOWNORMAL);

	/*
	//Wysy³anie WM_CLOSE do okienka eziriza

	HWND found;
	for(int i = 0 ; i < 10 ; i++)
	{
	Sleep(1000);
	found = FindWindow(L"WindowsForms10.Window.8.app.0.2004eee",NULL);
	if(found != NULL) break;
	}
	PostMessage(found,WM_CLOSE,0,0);
	*/
	return 0;
}


