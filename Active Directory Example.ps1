Import-Module ActiveDirectory 

Clear-Host


$password = convertto-SecureString -String "M21nW2t3rSh4t0ff" -AsPlainText -Force #password has to be in SecureString format
$username = "pisceasdotnet"
$domain = Get-ADDomain
$activeDirectoryLocation = "CN=Pisceas.Net,OU=Configurations,OU=Rieker," + $domain.DistinguishedName

#only the pisceasdotnet user can access the location.
$credentials = new-object -typename System.Management.Automation.PSCredential -argumentlist $username, $password

$activeDirectoryEntry = Get-ADObject -Credential $credentials -Properties "url" $activeDirectoryLocation

#The array number is arbitrary.  It is just the order I entered the data in.
$connectionString = $activeDirectoryEntry.url[0]

Write-Host $connectionString

$updatePath = $activeDirectoryEntry.url[1]

Write-Host $updatePath



