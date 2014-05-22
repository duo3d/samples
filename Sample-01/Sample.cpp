///////////////////////////////////////////////////////////////////////////////////
// This code sample demonstrates the use of DUO SDK in your own applications
// For updates and file downloads go to: http://duo3d.com/
// Copyright 2014 (c) Code Laboratories, Inc.  All rights reserved.
///////////////////////////////////////////////////////////////////////////////////
#include "Sample.h"

void CALLBACK DUOCallback(const PDUOFrame pFrameData, void *pUserData)
{
	printf("DUO Frame Timestamp: %10.1f ms\n", pFrameData->timeStamp/10.0f);
	printf("  Accelerometer: [%8.5f, %8.5f, %8.5f]\n", pFrameData->accelData[0], 
												pFrameData->accelData[1], 
												pFrameData->accelData[2]);
	printf("  Gyro:          [%8.5f, %8.5f, %8.5f]\n", pFrameData->gyroData[0], 
										pFrameData->gyroData[1], 
										pFrameData->gyroData[2]);
	printf("  Temperature:   %8.6f C\n", pFrameData->tempData); 
	printf("------------------------------------------------------\n");
}

int main(int argc, char* argv[])
{
	printf("DUOLib Version:       v%s\n", GetLibVersion());

	DUOResolutionInfo ri;
	// Select 320x240 resolution with 2x2 binning capturing at 10FPS
	if(EnumerateResolutions(&ri, 1, 320, 240, DUO_BIN_HORIZONTAL2+DUO_BIN_VERTICAL2, 30))
	{
		DUOInstance duo;
		// Open DUO
		if(OpenDUO(&duo))
		{
			char tmp[260];
			// Get some DUO parameter values
			GetDUODeviceName(duo, tmp);
			printf("DUO Device Name:      '%s'\n", tmp);

			GetDUODeviceName(duo, tmp);
			printf("DUO Serial Number:    %s\n", tmp);

			GetDUOFirmwareVersion(duo, tmp);
			printf("DUO Firmware Version: v%s\n", tmp);

			GetDUOFirmwareBuild(duo, tmp);
			printf("DUO Firmware Build:   %s\n", tmp);

			printf("\nHit any key to start capturing");
			_getch();

			// Set selected resolution
			SetDUOResolutionInfo(duo, ri);
			// Start capture and pass DUOCallback function that will be called on every frame captured
			if(StartDUO(duo, DUOCallback, NULL))
			{
				// Wait for any key
				_getch();
				// Stop capture
				StopDUO(duo);
				// Close DUO
				CloseDUO(duo);
			}
		}
	}
	return 0;
}
