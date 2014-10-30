@echo off
if exist %VS110COMNTOOLS%\vsvars32.bat (
  call "%VS110COMNTOOLS%\vsvars32.bat"
  cmake -G "Visual Studio 11" .
  devenv DUOSamples.sln /build "Release"
) else if exist %VS100COMNTOOLS%\vsvars32.bat (
  call "%VS100COMNTOOLS%\vsvars32.bat"
  cmake -G "Visual Studio 10" .
  devenv DUOSamples.sln /build "Release"
)