@echo off

call "%VS100COMNTOOLS%\vsvars32.bat"
cmake .
msbuild /p:Configuration=Release DUO_SAMPLES.sln