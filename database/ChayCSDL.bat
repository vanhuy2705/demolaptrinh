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
    sqlcmd -S %SERVER% -U %3 -P %4 -C -i 01_TaoCSDL_v2.sql
    if errorlevel 1 goto loi
    sqlcmd -S %SERVER% -U %3 -P %4 -C -i 02_DuLieuMau_v2.sql
) else (
    sqlcmd -S %SERVER% -E -C -i 01_TaoCSDL_v2.sql
    if errorlevel 1 goto loi
    sqlcmd -S %SERVER% -E -C -i 02_DuLieuMau_v2.sql
)

if errorlevel 1 goto loi
echo.
echo  XONG! Da tao CSDL QLSanTheThao (ban v2/v3) + du lieu mau.
echo  DB cu dang chay: chay them 04_NangCapCSDL_v2.sql roi 05_BoSungVoucherDatSan_v3.sql
goto xong

:loi
echo.
echo  LOI: Khong chay duoc sqlcmd. Hay mo 2 file .sql bang SSMS va chay thu cong,
echo       hoac cai dat sqlcmd (SQL Server Command Line Utilities).

:xong
pause
