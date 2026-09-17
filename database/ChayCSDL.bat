@echo off
chcp 65001 >nul
title Tao CSDL QLSanTheThao
cd /d "%~dp0"

REM =====================================================================
REM  CACH DUNG
REM    ChayCSDL.bat                          -> tao moi CSDL v3 + du lieu mau (localhost)
REM    ChayCSDL.bat .\SQLEXPRESS             -> tao moi tren server chi dinh
REM    ChayCSDL.bat .\SQLEXPRESS nangcap     -> CHI nang cap DB cu (04 roi 05), giu nguyen du lieu
REM    ChayCSDL.bat .\SQLEXPRESS -U sa -P mk -> dung SQL Authentication
REM
REM  Luu y: che do tao moi se DROP CSDL cu (mat du lieu). Dang co du lieu that
REM  thi dung che do "nangcap".
REM  Ung dung cung TU dong bo sung phan luoc do con thieu luc khoi dong,
REM  nen neu quen chay script thi app van chay duoc.
REM =====================================================================

set SERVER=%1
if "%SERVER%"=="" set SERVER=localhost
set CHE_DO=%2

where sqlcmd >nul 2>nul
if errorlevel 1 (
    echo.
    echo  LOI: Khong tim thay lenh "sqlcmd" trong PATH.
    echo       - Cai dat "SQL Server Command Line Utilities", hoac
    echo       - Mo cac file .sql trong thu muc nay bang SSMS va chay thu cong.
    goto xong
)

if /I "%CHE_DO%"=="nangcap" goto nangcap
if "%CHE_DO%"=="-U" goto sqlauth
goto windowauth

:nangcap
echo ============================================================
echo  NANG CAP CSDL QLSanTheThao tren server: %SERVER%
echo  (giu nguyen du lieu hien co)
echo ============================================================
echo.
sqlcmd -S %SERVER% -E -C -i 04_NangCapCSDL_v2.sql
if errorlevel 1 goto loi_sql
sqlcmd -S %SERVER% -E -C -i 05_BoSungVoucherDatSan_v3.sql
if errorlevel 1 goto loi_sql
echo.
echo  XONG! CSDL da duoc nang cap len ban v3 (DAT_SAN.MaVoucher).
goto xong

:windowauth
echo ============================================================
echo  TAO MOI CSDL QLSanTheThao tren server: %SERVER%
echo  (Windows Authentication. Neu dung SQL Authentication:
echo   ChayCSDL.bat .\SQLEXPRESS -U sa -P matkhau)
echo  CANH BAO: se xoa CSDL cu neu da co.
echo ============================================================
echo.
sqlcmd -S %SERVER% -E -C -i 01_TaoCSDL_v2.sql
if errorlevel 1 goto loi_sql
sqlcmd -S %SERVER% -E -C -i 02_DuLieuMau_v2.sql
if errorlevel 1 goto loi_sql
goto thanhcong

:sqlauth
sqlcmd -S %SERVER% -U %3 -P %4 -C -i 01_TaoCSDL_v2.sql
if errorlevel 1 goto loi_sql
sqlcmd -S %SERVER% -U %3 -P %4 -C -i 02_DuLieuMau_v2.sql
if errorlevel 1 goto loi_sql
goto thanhcong

:thanhcong
echo.
echo  XONG! Da tao CSDL QLSanTheThao (ban v2/v3) + du lieu mau.
echo  Tai khoan demo: admin/admin123 - quanly/admin123 - nhanvien/nv123456 - khach1/kh123456
echo  DB cu dang chay: dung "ChayCSDL.bat %SERVER% nangcap" de giu du lieu.
goto xong

:loi_sql
echo.
echo  LOI: Chay script SQL that bai (xem thong bao sqlcmd phia tren).
echo       Thuong gap:
echo        - Chua cai SQL Server / ten server sai (thu .\SQLEXPRESS hoac localhost).
echo        - Tai khoan Windows khong co quyen tao CSDL (dang nhap SSMS bang sa).
echo        - CSDL dang duoc dung boi ket noi khac (dong ung dung roi chay lai).
echo       Neu van loi: mo file .sql bang SSMS va chay thu cong de thay loi cu the.

:xong
echo.
pause
