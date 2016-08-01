using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUODeviceLib;

namespace DUOLibConsoleDemo
{
    class Program
    {
        static IntPtr duo = IntPtr.Zero;
        static DUOFrameCallback frameCallback = new DUOFrameCallback(FrameCallback);
        static int frameCount = 0;

        static void FrameCallback(ref DUOFrame pFrameData, IntPtr pUserData)
        {
            frameCount++;
            Console.WriteLine("Frame ID: {0}, Timestamp: {1}", frameCount, pFrameData.timeStamp);
            if (pFrameData.IMUPresent)
            {
                for (int i = 0; i < pFrameData.IMUSamples; i++)
                {
                    Console.WriteLine(" Sample #{0}", i+1);
                    Console.WriteLine("  Timestamp: {0}", pFrameData.IMUData[i].timeStamp);
                    Console.WriteLine("    Acceleration : ({0}:{1}:{2})", pFrameData.IMUData[i].accelData[0], pFrameData.IMUData[i].accelData[1], pFrameData.IMUData[i].accelData[2]);
                    Console.WriteLine("    Gyro : ({0}:{1}:{2})", pFrameData.IMUData[i].gyroData[0], pFrameData.IMUData[i].gyroData[1], pFrameData.IMUData[i].gyroData[2]);
                    Console.WriteLine("    Temperature : {0} C", pFrameData.IMUData[i].tempData);
                }
            }
        }
        static void Main(string[] args)
        {
            string libName = DUOLib.GetLibVersion();
            Console.WriteLine("DUO Library Version: {0}", libName);
            bool isOpened = DUOLib.OpenDUO(ref duo);
            if (isOpened)
            {
                string infoString = "";
                if (DUOLib.GetDUODeviceName(duo, ref infoString))
                    Console.WriteLine("Device Name: {0}", infoString);
                if (DUOLib.GetDUOSerialNumber(duo, ref infoString))
                    Console.WriteLine("Serial Number: {0}", infoString);
                if (DUOLib.GetDUOFirmwareVersion(duo, ref infoString))
                    Console.WriteLine("Firmware Version: {0}", infoString);
                if (DUOLib.GetDUOFirmwareBuild(duo, ref infoString))
                    Console.WriteLine("Firmware Build: {0}", infoString);
                DUOResolutionInfo[] resInfos = new DUOResolutionInfo[1];
                int resolutionsCount = DUOLib.EnumerateResolutions(resInfos, 1, 320, 240, (DUOBinning)((int)DUOBinning.DUO_BIN_HORIZONTAL2 + DUOBinning.DUO_BIN_VERTICAL2), 60);
                if (resolutionsCount == 0x00)
                {
                    DUOLib.CloseDUO(duo);
                    return;
                }
                DUOLib.SetDUOResolutionInfo(duo, resInfos[0]);

                DUOResolutionInfo resInfo = new DUOResolutionInfo();
                if(DUOLib.GetDUOResolutionInfo(duo, ref resInfo))
                    Console.WriteLine("DUO Resolution: {0}x{1}, {2}, {3}", resInfo.width, resInfo.height, resInfo.binning, resInfo.fps);

                uint w = 0, h = 0;
                if (DUOLib.GetDUOFrameDimension(duo, ref w, ref h))
                    Console.WriteLine("DUO Frame Dimension: {0}x{1}", w, h);

                DUOLib.SetDUOCameraSwap(duo, false);
                DUOLib.SetDUOHFlip(duo, false);
                DUOLib.SetDUOVFlip(duo, false);
                DUOLib.SetDUOExposure(duo, 10.0);
                DUOLib.SetDUOGain(duo, 10.0);
                DUOLib.SetDUOLedPWM(duo, 40.0);
                double paramValue = 0.0;
                if (DUOLib.GetDUOExposure(duo, ref paramValue))
                    Console.WriteLine("DUO Exposure: {0}", paramValue);
                if (DUOLib.GetDUOGain(duo, ref paramValue))
                    Console.WriteLine("DUO Gain: {0}", paramValue);
                if (DUOLib.GetDUOLedPWM(duo, ref paramValue))
                    Console.WriteLine("DUO PWM: {0}", paramValue);
                bool bValue = false;
                if (DUOLib.GetDUOCameraSwap(duo, ref bValue))
                    Console.WriteLine("DUO Camera Swap: {0}", bValue ? "YES" : "NO");
                if (DUOLib.GetDUOVFlip(duo, ref bValue))
                    Console.WriteLine("DUO VFlip: {0}", bValue ? "YES" : "NO");
                if (DUOLib.GetDUOHFlip(duo, ref bValue))
                    Console.WriteLine("DUO HFlip: {0}", bValue ? "YES" : "NO");
                Console.WriteLine("Press Any Key to start frame grabbing");
                Console.ReadKey();
                if (DUOLib.StartDUO(duo, frameCallback, IntPtr.Zero))
                {
                    Console.WriteLine("Press Any Key to stop frame grabbing");
                    Console.ReadKey();
                    DUOLib.StopDUO(duo);
                }
                DUOLib.CloseDUO(duo);
            }
            Console.WriteLine("Press Any Key to exit application");
            Console.ReadKey();
        }
    }
}
