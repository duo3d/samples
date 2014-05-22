//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// This library is part of DUO SDK that allows the use of DUO devices in your own applications
//
// For updates and file downloads go to: http://duo3d.com/
//
// Copyright 2014 (c) Code Laboratories, Inc.  All rights reserved.
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef _DUOLIB_H
#define _DUOLIB_H

#ifdef WIN32
	#include <windows.h>
	#ifdef DUOLIB_DLL
		#define API_FUNCTION(type)	__declspec(dllexport) type __cdecl
	#else
		#define API_FUNCTION(type)	__declspec(dllimport) type __cdecl
	#endif
	// define some types
	typedef signed __int8		int8_t;
	typedef signed __int16		int16_t;
	typedef signed __int32		int32_t;
	typedef unsigned __int8		uint8_t;
	typedef unsigned __int16	uint16_t;
	typedef unsigned __int32	uint32_t;
	typedef signed __int64      int64_t;
	typedef unsigned __int64    uint64_t;
#else
	#include <stdint.h>
	#define API_FUNCTION(type)	__attribute__((visibility ("default"))) type __attribute__((cdecl))
	#define CALLBACK
#endif

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// DUO C API
extern "C" {

// DUO instance
typedef void *DUOInstance;

// DUO binning
enum DUOBinning
{
	DUO_BIN_ANY = -1,
	DUO_BIN_NONE = 0,
	DUO_BIN_HORIZONTAL2 = 1,        // Horizontal binning by factor of 2
	DUO_BIN_HORIZONTAL4 = 2,        // Horizontal binning by factor of 4
	DUO_BIN_VERTICAL2 = 4,          // Vertical binning by factor of 2
	DUO_BIN_VERTICAL4 = 8           // Vertical binning by factor of 4
};

// DUO resolution info
typedef struct
{
	int   width;
	int   height;
	int   binning;
	float fps;
	float minFps;
	float maxFps;
}DUOResolutionInfo, *PDUOResolutionInfo;

// DUO Frame
typedef struct
{
	uint32_t  width;				// DUO frame width
	uint32_t  height;				// DUO frame height
	uint8_t  ledSeqTag;				// DUO frame LED tag
	uint32_t timeStamp;				// DUO frame time stamp in 100us increments
	uint8_t* leftData;				// DUO left frame data
	uint8_t* rightData;				// DUO right frame data
	uint8_t accelerometerPresent;	// True if accelerometer chip is present
	float accelData[3];				// DUO accelerometer data
	float gyroData[3];				// DUO gyroscope data
	float magData[3];				// DUO magnetometer data
	float tempData;					// DUO temperature data
}DUOFrame, *PDUOFrame;

typedef struct  
{
	uint8_t ledPwmValue[4];			// LED PWM values are in percentage [0,100]
}DUOLEDSeq, *PDUOLEDSeq;

// DUO parameters
enum DUOParameter
{
	// DUO parameter unit
	DUO_PERCENTAGE,
	DUO_MILLISECONDS,
	// DUO parameters
	DUO_DEVICE_NAME,                // Get only: parameter - string allocated by user 252 bytes
	DUO_SERIAL_NUMBER,              // Get only: parameter - string allocated by user 252 bytes
	DUO_FIRMWARE_VERSION,           // Get only: parameter - string allocated by user 252 bytes
	DUO_FIRMWARE_BUILD,             // Get only: parameter - string allocated by user 252 bytes
	DUO_RESOLUTION_INFO,            // Set/Get: (PDUOResolutionInfo) - must be first parameter to set
	DUO_FRAME_DIMENSION,            // Get only: (uint32_t, uint32_t)
	DUO_EXPOSURE,                   // Set/Get: (double [0,100], DUO_PERCENTAGE) or (double in milliseconds, DUO_MILLISECONDS)
	DUO_GAIN,                       // Set/Get: (double [0,100])
	DUO_HFLIP,                      // Set/Get: (bool [false,true])
	DUO_VFLIP,                      // Set/Get: (bool [false,true])
	DUO_SWAP_CAMERAS,               // Set/Get: (bool [false,true])
	// LED Control Parameters
	DUO_LED_PWM,                    // Set/Get: (double [0,100])
	DUO_LED_PWM_SEQ,                // Set only: (PDUOLEDSeq, int) - number of LED sequence steps (max 64)
};

API_FUNCTION(char *) GetLibVersion();

// DUO resolution enumeration
// To enumerate resolution settings for specific resolution, set width and height and optionally fps.
// To enumerate all supported resolutions set width, height and fps all to -1.
// NOTE: There are large number of resolution setting supported.
API_FUNCTION(int) EnumerateResolutions(DUOResolutionInfo *resList, int32_t resListSize, 
										int32_t width = -1, int32_t height = -1, int32_t binning = DUO_BIN_ANY, float fps = -1);

// DUO device initialization
API_FUNCTION(bool) OpenDUO(DUOInstance *duo);
API_FUNCTION(bool) CloseDUO(DUOInstance duo);

// DUO frame callback function
// NOTE: This function is called in the context of the DUO capture thread.
//		 To prevent any dropped frames, this function must return as soon as possible.
typedef void (CALLBACK *DUOFrameCallback)(const PDUOFrame pFrameData, void *pUserData);

// DUO device capture control
API_FUNCTION(bool) StartDUO(DUOInstance duo, DUOFrameCallback frameCallback, void *pUserData, bool masterMode = true);
API_FUNCTION(bool) StopDUO(DUOInstance duo);

// DUO Camera parameters control
// Do not call these functions directly
// Use below defined macros
API_FUNCTION(bool) __DUOParamSet__(DUOInstance duo, DUOParameter param, ...);
API_FUNCTION(bool) __DUOParamGet__(DUOInstance duo, DUOParameter param, ...);

// Get DUO parameters
#define GetDUODeviceName(duo, val)          __DUOParamGet__(duo, DUO_DEVICE_NAME, (char*)val)
#define GetDUOSerialNumber(duo, val)        __DUOParamGet__(duo, DUO_SERIAL_NUMBER, (char*)val)
#define GetDUOFirmwareVersion(duo, val)     __DUOParamGet__(duo, DUO_FIRMWARE_VERSION, (char*)val)
#define GetDUOFirmwareBuild(duo, val)       __DUOParamGet__(duo, DUO_FIRMWARE_BUILD, (char*)val)
#define GetDUOResolutionInfo(duo, val)      __DUOParamGet__(duo, DUO_RESOLUTION_INFO, (DUOResolutionInfo&)val)
#define GetDUOExposure(duo, val)            __DUOParamGet__(duo, DUO_EXPOSURE, (double*)val, DUO_PERCENTAGE)
#define GetDUOExposureMS(duo, val)          __DUOParamGet__(duo, DUO_EXPOSURE, (double*)val, DUO_MILLISECONDS)
#define GetDUOGain(duo, val)                __DUOParamGet__(duo, DUO_GAIN, (double*)val)
#define GetDUOHFlip(duo, val)               __DUOParamGet__(duo, DUO_HFLIP, (int*)val)
#define GetDUOVFlip(duo, val)               __DUOParamGet__(duo, DUO_VFLIP, (int*)val)
#define GetDUOCameraSwap(duo, val)          __DUOParamGet__(duo, DUO_SWAP_CAMERAS, (int*)val)
#define GetDUOLedPWM(duo, val)              __DUOParamGet__(duo, DUO_LED_PWM, (double*)val)
#define GetDUOFrameDimension(duo, w, h)     __DUOParamGet__(duo, DUO_FRAME_DIMENSION, (uint32_t*)w, (uint32_t*)h)

// Set DUO parameters
#define SetDUOExposure(duo, val)            __DUOParamSet__(duo, DUO_EXPOSURE, (double)val, DUO_PERCENTAGE)
#define SetDUOExposureMS(duo, val)          __DUOParamSet__(duo, DUO_EXPOSURE, (double)val, DUO_MILLISECONDS)
#define SetDUOHFlip(duo, val)               __DUOParamSet__(duo, DUO_HFLIP, (int)val)
#define SetDUOResolutionInfo(duo, val)      __DUOParamSet__(duo, DUO_RESOLUTION_INFO, (DUOResolutionInfo&)val)
#define SetDUOExposure(duo, val)            __DUOParamSet__(duo, DUO_EXPOSURE, (double)val, DUO_PERCENTAGE)
#define SetDUOExposureMS(duo, val)          __DUOParamSet__(duo, DUO_EXPOSURE, (double)val, DUO_MILLISECONDS)
#define SetDUOGain(duo, val)                __DUOParamSet__(duo, DUO_GAIN, (double)val)
#define SetDUOHFlip(duo, val)               __DUOParamSet__(duo, DUO_HFLIP, (int)val)
#define SetDUOVFlip(duo, val)               __DUOParamSet__(duo, DUO_VFLIP, (int)val)
#define SetDUOCameraSwap(duo, val)          __DUOParamSet__(duo, DUO_SWAP_CAMERAS, (int)val)
#define SetDUOLedPWM(duo, val)              __DUOParamSet__(duo, DUO_LED_PWM, (double)val)
#define SetDUOLedPWMSeq(duo, val, size)     __DUOParamSet__(duo, DUO_LED_PWM_SEQ, (PDUOLEDSeq)val, (uint32_t)size)

}; // extern "C"

#endif // _DUOLIB_H