@echo off
if exist "%VS120COMNTOOLS%\vsvars32.bat" (
  call "%VS120COMNTOOLS%\vsvars32.bat"
  cmake -G "Visual Studio 12" .
  devenv DUOSamples.sln /build "Release"
) else (
  echo "Building Samples Require VC2013"
  pause
)
