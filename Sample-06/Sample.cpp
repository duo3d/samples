///////////////////////////////////////////////////////////////////////////////////
// This code sample demonstrates the use of DUO SDK in your own applications
// For updates and file downloads go to: http://duo3d.com/
// Copyright 2014 (c) Code Laboratories, Inc.  All rights reserved.
///////////////////////////////////////////////////////////////////////////////////
#include "Sample.h"

#define WIDTH	640
#define HEIGHT	480
#define FPS		30

int main(int argc, char* argv[])
{
	printf("DUOLib Version:       v%s\n", GetLibVersion());

	// Open DUO camera and start capturing
	if(!OpenDUOCamera(WIDTH, HEIGHT, FPS))
	{
		printf("Could not open DUO camera\n");
		return 0;
	}
	// Create OpenCV windows
	cvNamedWindow("Left");
	cvNamedWindow("Right");

	// Set exposure and LED brightness
	SetExposure(50);
	SetLed(25);

	// Create image headers for left & right frames
	IplImage *left = cvCreateImageHeader(cvSize(WIDTH, HEIGHT), IPL_DEPTH_8U, 1);
	IplImage *right = cvCreateImageHeader(cvSize(WIDTH, HEIGHT), IPL_DEPTH_8U, 1);

	// Run capture loop until <Esc> key is pressed
	while((cvWaitKey(1) & 0xff) != 27)
	{
		// Capture DUO frame
		PDUOFrame pFrameData = GetDUOFrame();
		if(pFrameData == NULL) continue;

		// Set the image data
		left->imageData = (char*)pFrameData->leftData;
		right->imageData = (char*)pFrameData->rightData;

		// Process images here (optional)

		// Display images
		cvShowImage("Left", left);
		cvShowImage("Right", right);
	}

	// Release image headers
	cvReleaseImageHeader(&left);
	cvReleaseImageHeader(&right);

	// Close DUO camera
	CloseDUOCamera();
	return 0;
}
