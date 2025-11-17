# Visual Studio 2022 Select Build -> Publish vitaaid.com

# Options:
# Deployment mode（部署模式）：
# Framework-dependent（依赖共享框架）

# Target Framework
# net5.0-windows

# Target runtime（目标运行时）：
# Portable（不要选 win-x86 / win-x64）

# Configuration：Release

# Then Copy deploy_vitaaid.bat to DeployServer


# NOT WORKING.......
Remove-Item "C:\Users\riozh\Downloads\release\*" -Recurse -Force
Set-Location "..\vitaaid.com"
# dotnet publish ".\vitaaid.com\vitaaid.com.csproj" /p:PublishProfile=".\scripts\FolderProfile3.pubxml"

dotnet publish ".\vitaaid.com.csproj" -c Release -f net5.0-windows --self-contained false -o "C:\Users\riozh\Downloads\release"
