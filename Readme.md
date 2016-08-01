#DUO3D SDK Code Samples

![DUO3D](https://duo3d.com/public/media/products/all-duos-1.6.png)

The DUO3D SDK may change with updates, please make sure to use the latest examples shipped with our SDK as the code samples are actively being updated.

##Overview

The DUO SDK provides APIs, examples for working with multi-view vision systems, built on elegant image processing algorithms which leverage the latest SIMD technologies from Intel/ARM (SSE/Neon) and optimized parallel computing methods. The DUO SDK is a highly optimized architecture for image acquisition and processing. DUO Developers can leverage this framework to build and deploy their own applications.

These samples require the latest SDK from http://duo3d.com/

--------------------------------

##Features

* Pure C/C++ API
* C#/C++/Dense3D/OpenCV/Qt Samples
* Unity3D C# integration
* 100% Cross Platform
* Pre-compiled solutions for Windows/OS X/Linux OS
* Optional Qt5 Integration
* Roll your own algorithms
* Robust & stable processing
* Optimized imaging pipeline

--------------------------------

##Libraries Summary 

Here are the key libraries you will interact when working with the SDK:

--------------------------------

* **DUOLib** - The DUO API provides low level access to the DUO device allowing for control, configuration and device information.
* **Dense3D** - The Dense3D API provides high level access optimized disparity and point cloud extraction for DUO stereo image pair. 
* **Dense3DMT** - The Dense3DMT API provides high level multi-threaded access optimized disparity and point cloud extraction for DUO stereo image pair. 

--------------------------------

##Compilation

We use the cross platform [CMake](http://make.org) tool to generate compiler specific projects. To compile the samples you will need to install the [latest CMake](http://cmake.org/cmake/resources/software.html) and use either the command line or GUI to configure and generate the projects. We also provide examples that use OpenCV which you can download from [their website](http://opencv.org). 

----------------

###Build Environment

**Windows:**

1) Visual Studio 2013 Recommenced<br/>
2) Download and install CMake installer (Select the "Add to the PATH" option)<br/>
3) Download and extract OpenCV 2.4.10 into C:\Dev\OpenCV\2.4.10<br/>
4) Add `C:\Dev\OpenCV\2.4.10` to the system PATH variable<br/>

----------------

**OSX:**

1) Install XCode and make sure developer mode is enabled<br/>
2) Download and install CMake<br/>
3) Download and build/install OpenCV 2.4.10 from source<br/>
 * Extract the zip and open a new terminal<br/>
 * In the terminal and navigate to the OpenCV folder<br/>
 * Use Cmake command line or GUI to generate UNIX Makefiles (should generate a build folder)<br/>
 * Type 'cd build' and then `sudo make install` commands to install OpenCV<br/>
 * This will install OpenCV into `/usr/local`<br/>
 
----------------

**Linux:**

1) Install build-essential<br/>
2) Download and install CMake<br/>
3) Install `libgtk2.0-dev` <br/>
4) Download and build OpenCV 2.4.10 from source (similar to OSX)<br/>
5) Install OpenCV to default path `/usr/local`

----------------

###Building the Samples

We provide a build scripts in the samples directory which you can run to quickly generate the sample binaries. The executable files will be generated in the bin folder. You can also use CMake to generate for specific compiler such as Visual Studio/XCode/GCC.

**Windows:**

Double-click  or run `BuildAll.cmd` from command prompt. The executables will be generated and placed to `bin` folder. 

**Linux/OSX:** 

Open the terminal and type: `sh ./BuildAll.sh`. The executables will be generated and placed to `bin` folder. 

##Help

For more information about code samples please visit [DUO SDK Documentation](https://duo3d.com/docs/articles/sdk) page.

----------------
