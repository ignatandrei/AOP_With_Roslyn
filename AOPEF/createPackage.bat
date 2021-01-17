cd AOPMethodsCommon
dotnet pack -o ../../bin/ --include-source --include-symbols 
cd ..
cd AOPMethods
dotnet pack -o ../../bin/ --include-source --include-symbols 
