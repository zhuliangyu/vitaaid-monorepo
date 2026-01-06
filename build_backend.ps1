Remove-Item -Path "C:\Users\riozh\Downloads\release" -Recurse -Force -ErrorAction SilentlyContinue
Set-Location "D:\Code\vitaaid.com-monorepo\vitaaid.com"
dotnet build vitaaid.com.sln -c Release
dotnet publish vitaaid.com.csproj -c Release --self-contained false -o "C:\Users\riozh\Downloads\release"

Remove-Item -Path "C:\Users\riozh\Downloads\release\ClientApp" -Recurse -Force -ErrorAction SilentlyContinue

Copy-Item -Path "D:\Code\vitaaid.com-monorepo\vitaaid.com\sysconfig.xml" -Destination "C:\Users\riozh\Downloads\release" -Force

Compress-Archive -Path "C:\Users\riozh\Downloads\release\*" -DestinationPath "C:\Users\riozh\Downloads\vitaaid-customer-backend.zip" -Force
Write-Host "Backend build and zip completed successfully!" -ForegroundColor Green

# open C:\Users\riozh\Downloads\
Start-Process "C:\Users\riozh\Downloads\"

# OneDrive
Start-Process "https://onedrive.live.com/?id=%2Fpersonal%2Faed0b8f2b01f7cb2%2FDocuments%2FShare%2FNAPI"
