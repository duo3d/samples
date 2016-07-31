#DUO3D Code Samples

![DUO3D](https://duo3d.com/public/media/products/all-duos-1.6.png)

DUO3D-SDK - This may change with updates, please make sure to use the latest examples shipped with our SDK as our build environment is actively updated.

##Overview

The DUO SDK provides APIs, examples and tooling for working with multi-view vision systems. Built on elegant image processing algorithms which leverage the latest technologies from Intel/AMD (SSE, AVX) and parallel computing methods. This is a highly optimized architecture for image acquisition and processing. DUO Developers can leverage this framework to build and deploy their own applications.

These samples require the latest SDK from http://duo3d.com/

--------------------------------

###Features

* Pure C API
* C++/OpenCV Samples
* 100% Cross Platform
* Pre-compiled solutions 
* Optional Qt5 Integration
* Roll your own algorithms
* Robust & stable processing
* Optimized imaging pipeline
* Middleware APIs in the works

--------------------------------



###Libraries Summary 

Here are the key libraries you will interact when working with the SDK:


--------------------------------

* **DUOLib** - The DUO API provides low level access to the device allowing for control, configuration and device information.

--------------------------------

<div class="page-break"></div>

##Compilation


We use the cross platform [CMake](http://make.org) tool to generate IDE/compiler specific projects. To compile the samples you will need to install the [latest CMake](http://cmake.org/cmake/resources/software.html) and use either the command line or GUI to configure and generate the projects. We also provide examples that use OpenCV which you can download from [their website](http://opencv.org). If you wish to bypass the OpenCV install simply remove Sample-06 from the CMakeLists.txt before generating your build files.

----------------

####Build Environment


**Windows:**

1) Visual Studio 2010 SP1 Recommenced<br/>
2) Download and install CMake installer (Select the "Add to the PATH" option)<br/>
3) Download and extract OpenCV 2.4.7.2 into C:\OpenCV\2.4.7.2<br/>
4) Add `C:\OpenCV\2.4.7.2` to the system PATH variable<br/>

----------------

**OSX:**

1) Install XCode and make sure developer mode is enabled<br/>
2) Download and install CMake<br/>
3) Download and build/install OpenCV 2.4.7 from source<br/>
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
4) Download and build OpenCV 2.4.7 from source (similar to OSX)<br/>
5) Install OpenCV to default path `/usr/local`

----------------

<div class="page-break"></div>	

####Building the Samples


We provide a build script in the samples directory which you can run to quickly generate the sample binaries. The executable files will be generated in the bin folder. You can also use Cmake to generate for specific IDE/compiler such as Visual Studio/XCode/etc.

**Windows:**

Double-click  or run `BuildAll.cmd` from command prompt 

**Linux/OSX:** 

In terminal type: `sh ./BuildAll.sh`


--------------------------------

<div class="page-break"></div>

##Samples



Included with the SDK are several examples to help developers get started. We will also publish more here as we update.


--------------------------------


<a href="samples#Sample01"><h3>Capturing Motion Data</h3>
**Sample 01** - Shows how to capture and debug the DUO minilx motion data.</a>

<a href="samples#Sample02"><h3>Capturing Image Data</h3>
**Sample 02** - Shows how to capture the DUO image frame data from CMOS sensors.</a>

<a href="samples#Sample03"><h3>Configuring Parameters</h3>
**Sample 03** - Shows how to configure the programmable LED Array.</a>

<a href="samples#Sample04"><h3>Configuring LED Sequences</h3>
**Sample 04** - Shows how to pass sequences the programmable LED Array.</a>

<a href="samples#Sample05"><h3>Capture frames using polling mechanism</h3>
**Sample 05** - Demonstrates polling mechanism for capturing frames.</a>

<a href="samples#Sample06"><h3>Capture frames using polling mechanism (OpenCV)</h3>
**Sample 06** - Demonstrates polling mechanism and displays captured frames using OpenCV.</a>
