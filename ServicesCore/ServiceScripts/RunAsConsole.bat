@echo off 
set ServiceLocation=%CD%\..\HitServicesCore.exe
echo.
echo.
echo   Running as Console App: %ServiceLocation%
echo. 
echo.  
echo. 
%ServiceLocation% --console
echo.  

PAUSE