// Launcher.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include <shellapi.h>

int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
	char dir[255];
	char dirFilename[255];
	GetCurrentDirectoryA(255,dir);

	strcat(dir,"\\bin\\Release");
	strncpy(dirFilename,dir,255);
	strcat(dirFilename,"\\Wof.exe");

	ShellExecuteA(0,"open",dirFilename,"",dir,SW_SHOWNORMAL);

	HWND found;
	for(int i = 0 ; i < 10 ; i++)
	{
	Sleep(1000);
	found = FindWindow(L"WindowsForms10.Window.8.app.0.2004eee",NULL);
	if(found != NULL) break;
	}
	PostMessage(found,WM_CLOSE,0,0);
	return 0;
}


