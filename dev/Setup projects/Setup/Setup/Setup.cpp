// Setup.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "Setup.h"

#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);

// Constants that represent registry key names and value names
// to use for detection
const TCHAR *g_szNetfx10RegKeyName = _T("Software\\Microsoft\\.NETFramework\\Policy\\v1.0");
const TCHAR *g_szNetfx10RegKeyValue = _T("3705");
const TCHAR *g_szNetfx10SPxMSIRegKeyName = _T("Software\\Microsoft\\Active Setup\\Installed Components\\{78705f0d-e8db-4b2d-8193-982bdda15ecd}");
const TCHAR *g_szNetfx10SPxOCMRegKeyName = _T("Software\\Microsoft\\Active Setup\\Installed Components\\{FDC11A6F-17D1-48f9-9EA3-9051954BAA24}");
const TCHAR *g_szNetfx11RegKeyName = _T("Software\\Microsoft\\NET Framework Setup\\NDP\\v1.1.4322");
const TCHAR *g_szNetfx20RegKeyName = _T("Software\\Microsoft\\NET Framework Setup\\NDP\\v2.0.50727");
const TCHAR *g_szNetfx30RegKeyName = _T("Software\\Microsoft\\NET Framework Setup\\NDP\\v3.0\\Setup");
const TCHAR *g_szNetfx30SpRegKeyName = _T("Software\\Microsoft\\NET Framework Setup\\NDP\\v3.0");
const TCHAR *g_szNetfx30RegValueName = _T("InstallSuccess");
const TCHAR *g_szNetfx35RegKeyName = _T("Software\\Microsoft\\NET Framework Setup\\NDP\\v3.5");
const TCHAR *g_szNetfxStandardRegValueName = _T("Install");
const TCHAR *g_szNetfxStandardSPxRegValueName = _T("SP");
const TCHAR *g_szNetfxStandardVersionRegValueName = _T("Version");

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

	STARTUPINFO         siStartupInfo;
	PROCESS_INFORMATION piProcessInfo;

	memset(&siStartupInfo, 0, sizeof(siStartupInfo));
	memset(&piProcessInfo, 0, sizeof(piProcessInfo));

	siStartupInfo.cb = sizeof(siStartupInfo);

	if(!IsNetfx20Installed())
	{

	CreateProcess(L"./Dependencies/dotnetfx.exe",     // Application name
		L"",                 // Application arguments
		0,
		0,
		FALSE,
		CREATE_DEFAULT_ERROR_MODE,
		0,
		0,                              // Working directory
		&siStartupInfo,
		&piProcessInfo);



	// Wait until application has terminated
	WaitForSingleObject(piProcessInfo.hProcess, INFINITE);

	// Close process and thread handles
	::CloseHandle(piProcessInfo.hThread);
	::CloseHandle(piProcessInfo.hProcess);
	}

		CreateProcess(L"./Dependencies/vcredist_x86.exe",     // Application name
		L"",                 // Application arguments
		0,
		0,
		FALSE,
		CREATE_DEFAULT_ERROR_MODE,
		0,
		0,                              // Working directory
		&siStartupInfo,
		&piProcessInfo);

			// Wait until application has terminated
	WaitForSingleObject(piProcessInfo.hProcess, INFINITE);

	// Close process and thread handles
	::CloseHandle(piProcessInfo.hThread);
	::CloseHandle(piProcessInfo.hProcess);

			CreateProcess(L"./Dependencies/DirectX/dxsetup.exe",     // Application name
		L"",                 // Application arguments
		0,
		0,
		FALSE,
		CREATE_DEFAULT_ERROR_MODE,
		0,
		0,                              // Working directory
		&siStartupInfo,
		&piProcessInfo);

			// Wait until application has terminated
	WaitForSingleObject(piProcessInfo.hProcess, INFINITE);

	// Close process and thread handles
	::CloseHandle(piProcessInfo.hThread);
	::CloseHandle(piProcessInfo.hProcess);

		CreateProcess(L"./Dependencies/helper.exe",     // Application name
		L"",                 // Application arguments
		0,
		0,
		FALSE,
		CREATE_DEFAULT_ERROR_MODE,
		0,
		0,                              // Working directory
		&siStartupInfo,
		&piProcessInfo);

	return 0;
}


