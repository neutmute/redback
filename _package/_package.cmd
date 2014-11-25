del /q *.nupkg
..\.nuget\nuget.exe pack ..\src\redback\redback.csproj -IncludeReferencedProjects -Prop Configuration=%1