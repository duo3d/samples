@echo off
rd /s/q win32
rd /s/q x64
for /d /r . %%d in (CMakeFiles;debug;release;*.dir;) do @if exist "%%d" (
echo Deleting "%%d"
rd /s/q "%%d"
)
for /r . %%d in (CMakeCache.txt;*.cmake;*.sln;*.suo;.DS_Store;._.DS_Store;*.vcxproj;*.vcxproj.filters;*.vcxproj.user;*.exe;*.manifest;*.ilk;*.pdb;Makefile) do @if exist "%%d" (
echo Deleting "%%d"
del "%%d"
)
