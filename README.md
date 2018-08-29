# AOP_With_Roslyn

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/ignatandrei/Interpreter/blob/master/LICENSE)  

[![NuGet](https://img.shields.io/nuget/v/dotnet-aop.svg)](https://www.nuget.org/packages/dotnet-aop)

[![codecov](https://codecov.io/gh/ignatandrei/AOP_With_Roslyn/branch/master/graph/badge.svg)](https://codecov.io/gh/ignatandrei/AOP_With_Roslyn)

 <a target='_blank' href='https://ci.appveyor.com/api/projects/ignatandrei/aop-with-roslyn/artifacts/coveragereport/badge_combined.svg' >Link to code coverage badge</a>
 
 
 
Install as global tool as

dotnet tool install -g dotnet-aop

go to your solution folder
run

dotnet aop


For customizing what you want to insert at every method at first line and last line, see https://github.com/ignatandrei/AOP_With_Roslyn/blob/master/AOPRoslyn/processme.txt


You can save the document in your solution root and execute with


dotnet aop &lt;your file name&gt;
