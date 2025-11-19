# Backend application must have write permissions to the ProductImages folder
$folderPath="C:\inetpub\vitaaid_customer_frontend\ClientApp\build\ProductImages"

# Array of users to grant permissions to
$users = @(
    "IIS APPPOOL\DefaultAppPool"
    "Users"
    # You can add more users here, for example:
    # "IIS APPPOOL\AnotherAppPool",
    # "DOMAIN\Username",
    # "NT AUTHORITY\IUSR"
)

# Get the security descriptor object for the folder
$acl = Get-Acl $folderPath

# Iterate through each user and add permissions
foreach ($user in $users) {
    # Create a new access rule granting full control
    $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule(
        $user, 
        "FullControl", 
        "ContainerInherit, ObjectInherit", 
        "None", 
        "Allow"
    )

    # Add the new permission rule to the existing ACL
    $acl.AddAccessRule($accessRule)
    
    Write-Host "Permissions added for user: $user" -ForegroundColor Green
}

# Apply the updated ACL to the folder
Set-Acl -Path $folderPath -AclObject $acl
Write-Host "Permission configuration completed!" -ForegroundColor Green
