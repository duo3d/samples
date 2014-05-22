@echo off
for /d /r . %%d in (CMakeFiles;debug;release;*.dir;win32) do @if exist "%%d" (
echo Deleting "%%d"
rd /s/q "%%d"
)
for /r . %%d in (CMakeCache.txt;*.cmake;*.sln;*.suo;.DS_Store;._.DS_Store;*.vcxproj;*.vcxproj.filters;*.vcxproj.user;*.exe;*.manifest) do @if exist "%%d" (
echo Deleting "%%d"
del "%%d"
)
