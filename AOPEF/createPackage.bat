cd AOPEFCommon
dotnet pack -o ../../bin/ --include-source --include-symbols 
cd ..
cd AOPEF
dotnet pack -o ../../bin/ --include-source --include-symbols 
