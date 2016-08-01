using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUODeviceLib;

namespace DUODeviceConsoleDemo
{
    class Program
    {
        static void DUODeviceStatusChanged(DUODevice sender, bool isRunning)
        {
            if (isRunning)
                Console.WriteLine("[START DUO DEVICE]");
            else
                Console.WriteLine("[STOP DUO DEVICE]");
        }
        static int frameCount = 0;
        static void DUOFrameReceived(DUODevice sender, ref DUOFrame pFrameData)
        {
            frameCount++;
            Console.WriteLine("Frame ID: {0}, Timestamp: {1}", frameCount, pFrameData.timeStamp);
            if (pFrameData.IMUPresent)
            {
                for (int i = 0; i < pFrameData.IMUSamples; i++)
                {
                    Console.WriteLine(" Sample #{0}", i + 1);
                    Console.WriteLine("  Timestamp: {0}", pFrameData.IMUData[i].timeStamp);
                    Console.WriteLine("    Acceleration : ({0}:{1}:{2})", pFrameData.IMUData[i].accelData[0], pFrameData.IMUData[i].accelData[1], pFrameData.IMUData[i].accelData[2]);
                    Console.WriteLine("    Gyro : ({0}:{1}:{2})", pFrameData.IMUData[i].gyroData[0], pFrameData.IMUData[i].gyroData[1], pFrameData.IMUData[i].gyroData[2]);
                    Console.WriteLine("    Temperature : {0} C", pFrameData.IMUData[i].tempData);
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Press Any Key to Create DUODevice");
            Console.ReadKey();
            DUODevice device = new DUODevice();
            Console.WriteLine(device);
            Console.WriteLine("\nPress Any Key to Start Capture");
            Console.ReadKey();
            device.DUODeviceStatusChanged += DUODeviceStatusChanged;
            device.DUOFrameReceived += DUOFrameReceived;
            device.Start();
            Console.WriteLine("\nPress Any Key to Stop Capture");
            Console.ReadKey();
            device.Stop();
            device.Dispose();
        }
    }
}
