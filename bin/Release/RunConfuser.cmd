del wof_secure\Wof.exe 
del wof_secure\EnhancedVersionHelper.exe

"../../Setup projects/ConfuserEx_bin/Confuser.CLI.exe" wof_confuser.crproj
move Wof.exe Wof.exe.backup
move EnhancedVersionHelper.exe EnhancedVersionHelper.exe.backup
copy /Y wof_secure\EnhancedVersionHelper.exe .
copy /Y wof_secure\Wof.exe .

echo Finished!
pause