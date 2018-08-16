del *.nupkg 
dotnet tool uninstall -g dotnet-aop
dotnet pack --output ./
dotnet tool install -g dotnet-aop --add-source ./ --version 2.0.0
cd ..
dotnet aop
cd aopCmd
del *.nupkg 
dotnet tool uninstall -g dotnet-aop
dotnet pack --output ./
dotnet tool install -g dotnet-aop --add-source ./ --version 2.0.0
echo now run 
echo dotnet aop
echo in whatever folder you have the .cs files