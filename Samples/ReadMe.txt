DUO SDK Compilation

------------------
http://duo3d.com/docs/articles/sdk
------------------

Build Requirements
------------------

Windows:
	Visual Studio 2010/2012
	Download and install CMake and add to the PATH
	Download and extract OpenCV 2.4.7.2 into C:\OpenCV\2.4.7.2
	Add C:\OpenCV\2.4.7.2 to the PATH
	
	Optional:
	Download DUOSDK and put in C:\DUO_SDK
	Add DUOSDK location to the DUO_SDK environment variable

Linux:
	Install build-essential, CMake, libgtk2.0-dev
	Download and build OpenCV 2.4.7 from source
	Install OpenCV (/usr/local)
	
	Optional:
	Download DUOSDK and put in /usr/local/DUOSDK
	Add DUOSDK location to the DUO_SDK environment variable.

OS X:
	Install XCode
	Install CMake
	Download and build OpenCV 2.4.7 from source
	Install OpenCV (/usr/local)
	
	Optional:	
	Download DUOSDK and put in /usr/local/DUOSDK
	Add DUOSDK location to the DUO_SDK environment variable.

Building Samples
----------------

Windows: Double-click on BuildAll-x86.cmd or BuildAll-x64.cmd

Linux & OS X: In terminal type:

$ sh ./BuildAll.sh

You will find the executable files in the bin folder.

Related References
----------------
http://duo3d.com/docs/articles/api
http://duo3d.com/docs/articles/samples