@echo off
echo Building GTA 6 Countdown for Release...
echo.

REM Navigate to project directory
cd /d "GTA 6 Countdown"

REM Clean previous builds
echo Cleaning previous builds...
if exist "bin\Release" rmdir /s /q "bin\Release"
if exist "obj\Release" rmdir /s /q "obj\Release"

REM Build for release
echo Building application...
dotnet publish -c Release --self-contained false --runtime win-x64 -o "..\Release"

if %errorlevel% neq 0 (
    echo.
    echo ❌ Build failed!
    pause
    exit /b 1
)

echo.
echo ✅ Build completed successfully!
echo.
echo Release files are in the 'Release' folder:
dir "..\Release\*.exe" /b 2>nul

echo.
echo Next steps:
echo 1. Upload the .exe file to your GitHub release
echo 2. Update the version tag (e.g., v1.0.0, v1.1.0)
echo 3. Make sure the release is set to public
echo.
pause
