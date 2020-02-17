$version = Get-Date -Format "yyyy.MM.dd.HHmm"
Write-Host $version
 dotnet tool run dotnet-property "**/*.csproj" Version:"$version"