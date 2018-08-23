rem dotnet tool install --global coverlet.console
rem dotnet tool install -g dotnet-reportgenerator-globaltool --version 4.0.0-alpha10
rem please be sure to include into csproj  <PackageReference Include="coverlet.msbuild" Version="2.1.0" />
dotnet test TestAOP.csproj --configuration release /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
reportgenerator "-reports:coverage.opencover.xml" "-targetdir:coveragereport" -reporttypes:HTMLInline;HTMLSummary;Badges

	