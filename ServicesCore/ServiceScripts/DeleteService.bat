@echo off 
set ServiceName=HitServicesCore

echo.
echo       * * * * * * * * * * * * * * * * * * * * * * * * 
echo       *                                             *
echo       *      Deleting Service '%ServiceName%'
echo       *                                             *
echo       * * * * * * * * * * * * * * * * * * * * * * * * 
echo.
sc delete  %ServiceName%
echo.

PAUSE