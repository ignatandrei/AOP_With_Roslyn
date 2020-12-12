cd SkinnyControllersGenerator
dotnet pack -o ../bin/ --include-source --include-symbols 
cd ..
cd SkinnyControllersCommon
dotnet pack -o ../bin/ --include-source --include-symbols 
