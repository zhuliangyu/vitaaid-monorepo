@echo off

:: Stop IIS
iisreset /stop

:: Ask for zip file path in command line
set /p zipPath=Please enter the full path to the ZIP file: 
set "deployFolder=C:\inetpub\vitaaid_customer_frontend"
set "backupRoot=C:\TEMP\DeployBackups"

:: Check the deploy zip file path existence
if not exist "%zipPath%" (
    echo ZIP file not found!
    pause
    exit /b
)

:: if C:\TEMP folder does not exist, create it
if not exist "C:\TEMP" (
    mkdir "C:\TEMP"
    echo Created C:\TEMP folder
) else (
    echo C:\TEMP folder already exists
)

:: Create temp folder
set tempDir=C:\TEMP\unzip_%RANDOM%_%RANDOM%

mkdir "%tempDir%"
echo Created temp directory: %tempDir%

:: Unzip using PowerShell (.NET built-in)
echo Unzipping the latest deploy version...
powershell -command "Expand-Archive -LiteralPath '%zipPath%' -DestinationPath '%tempDir%' -Force"

echo Unzipping the latest deploy version completed!
echo The latest deploy version is extracted to: %tempDir%

:: Target directory (ClientApp) to remove (including the folder itself)
:: ClientApp will be deployed independently
set deleteTarget=%tempDir%/ClientApp

:: Check if the directory exists
if not exist "%deleteTarget%" (
    echo Directory does not exist: %deleteTarget%
)

echo Deleting React directory and all contents: %deleteTarget%

:: Remove the directory (/ClientApp) and everything inside it
rmdir /s /q "%deleteTarget%"

:: open the temp directory
explorer %tempDir%

:: ===== 2. Check deploy folder exists =====
if not exist "%deployFolder%" (
    echo Deploy folder does not exist: %deployFolder%
    goto :EOF
)

:: ===== 3. Backup root folder =====
if not exist "%backupRoot%" (
    mkdir "%backupRoot%"
)

:: ===== 4. Get timestamp: yyyyMMdd_HHmmss =====
for /f %%i in ('powershell -NoProfile -Command "Get-Date -Format yyyyMMdd_HHmmss"') do (
    set "timestamp=%%i"
)

:: ===== 5. Build backup zip full path =====
set "backupZip=%backupRoot%\deploy_vitaaid_customer_frontend_%timestamp%.zip"

echo Creating backup: %backupZip%

:: ===== 6. Zip backup using PowerShell Compress-Archive =====
powershell -NoProfile -Command "Compress-Archive -Path '%deployFolder%\*' -DestinationPath '%backupZip%' -Force"

explorer %backupRoot%

:: ===== 7. Move all files from tempDir to deployFolder =====
:: Make sure target folder exists
if not exist "%deployFolder%" (
    mkdir "%deployFolder%"
)

echo Moving files from %tempDir% to %deployFolder%

:: Use robocopy to move everything and overwrite silently
robocopy "%tempDir%" "%deployFolder%" /E /MOVE /NFL /NDL /NJH /NJS /NP /R:0 /W:0

:: Check robocopy result
if errorlevel 16 (
    echo ERROR: Serious error occurred in robocopy.
    goto :EOF
) else if errorlevel 8 (
    echo ERROR: Some files or directories failed to copy.
    goto :EOF
) else if errorlevel 4 (
    echo WARNING: Mismatched files or some issues occurred.
) else if errorlevel 2 (
    echo WARNING: Some extra files or differences detected.
) else if errorlevel 1 (
    echo INFO: Files were copied successfully with minor issues.
) else (
    echo SUCCESS: Robocopy completed successfully with no errors.
)

echo Move the latest version to deploy folder completed.

explorer %deployFolder%

echo Restarting IIS to apply the latest version...
iisreset /start

pause
