del *.nupkg 
dotnet tool uninstall -g dotnet-aop
dotnet pack --output ./
dotnet tool install -g dotnet-aop --add-source ./ --version 2.0.0