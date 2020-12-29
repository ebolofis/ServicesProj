@echo off 
set ServiceName=HitServicesCore

echo.
echo       * * * * * * * * * * * * * * * * * * * * * * * * 
echo       *                                             *
echo       *      Stoping Service '%ServiceName%'
echo       *                                             *
echo       * * * * * * * * * * * * * * * * * * * * * * * * 
echo.
sc stop %ServiceName%
echo.

PAUSE