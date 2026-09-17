@echo off
chcp 65001 >nul
title Quan ly cho thue san the thao - Build va chay
cd /d "%~dp0"

echo ============================================================
echo  BUILD SOLUTION (Debug)
echo ============================================================
dotnet build SportFieldBooking.sln -c Debug
if errorlevel 1 (
    echo.
    echo  BUILD THAT BAI - xem loi o tren. Nhan phim bat ky de thoat.
    pause >nul
    exit /b 1
)

echo.
echo ============================================================
echo  CHAY UNG DUNG  ^(dong cua so nay de tat ung dung^)
echo ============================================================
dotnet run --project "src\SportFieldBooking.WinForms" -c Debug
pause
