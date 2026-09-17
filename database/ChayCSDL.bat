@echo off
chcp 65001 >nul
title Tao CSDL QLSanTheThao
cd /d "%~dp0"

set SERVER=%1
if "%SERVER%"=="" set SERVER=localhost

echo ============================================================
echo  Tao CSDL tren server: %SERVER%
echo  (Windows Authentication. Neu dung SQL Authentication:
echo   ChayCSDL.bat .^\SQLEXPRESS -U sa -P matkhau)
echo ============================================================
echo.

if "%2"=="-U" (
    sqlcmd -S %SERVER% -U %3 -P %4 -C -i 01_TaoCSDL.sql
    if errorlevel 1 goto loi
    sqlcmd -S %SERVER% -U %3 -P %4 -C -i 02_DuLieuMau.sql
) else (
    sqlcmd -S %SERVER% -E -C -i 01_TaoCSDL.sql
    if errorlevel 1 goto loi
    sqlcmd -S %SERVER% -E -C -i 02_DuLieuMau.sql
)

if errorlevel 1 goto loi
echo.
echo  XONG! Da tao CSDL QLSanTheThao + du lieu mau.
goto xong

:loi
echo.
echo  LOI: Khong chay duoc sqlcmd. Hay mo 2 file .sql bang SSMS va chay thu cong,
echo       hoac cai dat sqlcmd (SQL Server Command Line Utilities).

:xong
pause
