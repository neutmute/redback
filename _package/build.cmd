msbuild ..\redback.sln /p:configuration=debug  /t:clean,build
msbuild ..\redback.sln /p:configuration=release /t:clean,build