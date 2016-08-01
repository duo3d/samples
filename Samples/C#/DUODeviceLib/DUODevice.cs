using System;
using System.Runtime.InteropServices;

namespace DUODeviceLib
{
    public delegate void DUOFrameReceivedHandler(DUODevice sender, ref DUOFrame pFrameData);
    public delegate void DUODeviceStatusChangedHandler(DUODevice sender, bool isRunning);
    
    public class DUODevice : IDisposable
    {
        #region Private Properties
        private bool disposed = false;
        private GCHandle handle;
        private object duoLock = new object();
        private IntPtr duoInstance = IntPtr.Zero;
        private static DUOFrameCallback duoFrameCallback = new DUOFrameCallback(OnFrameCallback);
        private string deviceName = DUOLibInternal.UNKNOWN_STRING;
        private string serialNumber = DUOLibInternal.UNKNOWN_STRING;
        private string firmwareVersion = DUOLibInternal.UNKNOWN_STRING;
        private string firmwareBuild = DUOLibInternal.UNKNOWN_STRING;
        private DUOResolutionInfo resolutionInfo = DUOLibInternal.DUO_DEFAULT_RESOLUTION_INFO;
        private bool running = false;
        private double exposure = DUOLib.DUO_DEFAULT_EXPOSURE;
        private double gain = DUOLib.DUO_DEFAULT_GAIN;
        private double led = DUOLib.DUO_DEFAULT_PWM;
        private bool hFlip = DUOLib.DUO_DEFAULT_HFLIP;
        private bool vFlip = DUOLib.DUO_DEFAULT_VFLIP;
        private bool cameraSwap = DUOLib.DUO_DEFAULT_SWAP_CAMERAS;
        private bool undistort = DUOLib.DUO_DEFAULT_UNDISTORT;
        #endregion
        #region Public Properties
        public static string LibVersion
        {
            get { return DUOLib.GetLibVersion(); }
        }
        public string DeviceName
        {
            get { return deviceName; }
        }
        public string SerialNumber
        {
            get { return serialNumber; }
        }
        public string FirmwareVersion
        {
            get { return firmwareVersion; }
        }
        public string FirmwareBuild
        {
            get { return firmwareBuild; }
        }
        public DUOResolutionInfo Resolution
        {
            set
            {
                DUOResolutionInfo newResolution = new DUOResolutionInfo();
                if (DUOLibInternal.EnumerateDUOResolutions(ref newResolution, 1, value.width, value.height, value.binning, value.fps) == 0)
                    return;
                resolutionInfo = newResolution;
                if (duoInstance != IntPtr.Zero)
                    DUOLib.SetDUOResolutionInfo(duoInstance, resolutionInfo);
            }
            get
            {
                return resolutionInfo;
            }
        }
        public int FrameWidth
        {
            get { return resolutionInfo.width; }
        }
        public int FrameHeight
        {
            get { return resolutionInfo.height; }
        }
        public DUOBinning FrameBinning
        {
            get { return resolutionInfo.binning; }
        }
        public float FPS
        {
            get { return resolutionInfo.fps; }
        }
        public bool Running
        {
            get { return running; }
        }
        public double Exposure
        {
            set
            {
                if (duoInstance != IntPtr.Zero && DUOLib.SetDUOExposure(duoInstance, value))
                    exposure = value;
            }
            get
            {
                if (duoInstance != IntPtr.Zero)
                    DUOLib.GetDUOExposure(duoInstance, ref exposure);
                return exposure;
            }
        }
        public double Gain
        {
            set
            {
                if (duoInstance != IntPtr.Zero && DUOLib.SetDUOGain(duoInstance, value))
                    gain = value;
            }
            get
            {
                if (duoInstance != IntPtr.Zero)
                    DUOLib.GetDUOGain(duoInstance, ref gain);
                return gain;
            }
        }
        public double LED
        {
            set
            {
                if (duoInstance != IntPtr.Zero && DUOLib.SetDUOLedPWM(duoInstance, value))
                    led = value;
            }
            get
            {
                if (duoInstance != IntPtr.Zero)
                    DUOLib.GetDUOLedPWM(duoInstance, ref led);
                return led;
            }
        }
        public bool HFlip
        {
            set
            {
                if (duoInstance != IntPtr.Zero && DUOLib.SetDUOHFlip(duoInstance, value))
                    hFlip = value;
            }
            get
            {
                if (duoInstance != IntPtr.Zero)
                    DUOLib.GetDUOHFlip(duoInstance, ref hFlip);
                return hFlip;
            }
        }
        public bool VFlip
        {
            set
            {
                if (duoInstance != IntPtr.Zero && DUOLib.SetDUOVFlip(duoInstance, value))
                    vFlip = value;
            }
            get
            {
                if (duoInstance != IntPtr.Zero)
                    DUOLib.GetDUOVFlip(duoInstance, ref vFlip);
                return vFlip;
            }
        }
        public bool CameraSwap
        {
            set
            {
                if (duoInstance != IntPtr.Zero && DUOLib.SetDUOCameraSwap(duoInstance, value))
                    vFlip = value;
            }
            get
            {
                if (duoInstance != IntPtr.Zero)
                    DUOLib.GetDUOCameraSwap(duoInstance, ref cameraSwap);
                return vFlip;
            }
        }
        public bool Undistort
        {
            set
            {
                if (duoInstance != IntPtr.Zero && DUOLib.SetDUOUndistort(duoInstance, value))
                    undistort = value;
            }
            get
            {
                if (duoInstance != IntPtr.Zero)
                    DUOLib.GetDUOUndistort(duoInstance, ref undistort);
                return undistort;
            }
        }
        #endregion
        #region .ctor
        public DUODevice()
        {
            handle = GCHandle.Alloc(this);
            if (DUOLib.OpenDUO(ref duoInstance))
            {
                DUOLib.SetDUOResolutionInfo(duoInstance, resolutionInfo);
                DUOLib.SetDUOCameraSwap(duoInstance, cameraSwap);
                DUOLib.SetDUOHFlip(duoInstance, hFlip);
                DUOLib.SetDUOVFlip(duoInstance, vFlip);
                DUOLib.SetDUOExposure(duoInstance, exposure);
                DUOLib.SetDUOGain(duoInstance, gain);
                DUOLib.SetDUOLedPWM(duoInstance, led);
                DUOLib.SetDUOUndistort(duoInstance, undistort);
                if (!DUOLib.GetDUODeviceName(duoInstance, ref deviceName))
                    deviceName = DUOLibInternal.UNKNOWN_STRING;
                if (!DUOLib.GetDUOSerialNumber(duoInstance, ref serialNumber))
                    deviceName = DUOLibInternal.UNKNOWN_STRING;
                if (!DUOLib.GetDUOFirmwareVersion(duoInstance, ref firmwareVersion))
                    deviceName = DUOLibInternal.UNKNOWN_STRING;
                if (!DUOLib.GetDUOFirmwareBuild(duoInstance, ref firmwareBuild))
                    deviceName = DUOLibInternal.UNKNOWN_STRING;
            }
        }
        ~DUODevice()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        #endregion
        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose children objects
                }
                // free native resources if there are any.
                Destroy();
                disposed = true;
            }
            // free managed resources
            Stop();
        }
        private void Destroy()
        {
            lock (duoLock)
            {
                if (duoInstance == IntPtr.Zero)
                    return;
                DUOLib.CloseDUO(duoInstance);
                duoInstance = IntPtr.Zero;
            }
        }
        #endregion
        #region Methods
        private static void OnFrameCallback(ref DUOFrame pFrameData, IntPtr pUserData)
        {
            if (pUserData == IntPtr.Zero)
                return;
            GCHandle gch = GCHandle.FromIntPtr(pUserData);
            DUODevice device = gch.Target as DUODevice;
            if (device == null)
                return;
            device.OnFrameCallback(ref pFrameData);
        }
        private void OnFrameCallback(ref DUOFrame pFrameData)
        {
            if (DUOFrameReceived != null)
                DUOFrameReceived(this, ref pFrameData);
        }
        public bool Start()
        {
            lock (duoLock)
            {
                if ((duoInstance == IntPtr.Zero) || (running))
                  return false;
                bool ret = DUOLib.StartDUO(duoInstance, duoFrameCallback, GCHandle.ToIntPtr(handle));
                if (ret)
                {
                    running = true;
                    if (DUODeviceStatusChanged != null)
                        DUODeviceStatusChanged(this, true);
                }
                return ret;
            }
        }
        public bool Stop()
        {
            lock (duoLock)
            {
                if ((duoInstance == IntPtr.Zero) || (!running))
                    return false;
                bool ret = DUOLib.StopDUO(duoInstance);
                if (ret)
                {
                    running = false;
                    if (DUODeviceStatusChanged != null)
                        DUODeviceStatusChanged(this, false);
                }
                return ret;
            }
        }
        #endregion
        #region ToString Logic
        private static string GetResolutionInfoString(DUOResolutionInfo resolutionInfo)
        {
            int hBinning = 1;
            if ((resolutionInfo.binning & DUOBinning.DUO_BIN_HORIZONTAL2) == DUOBinning.DUO_BIN_HORIZONTAL2)
                hBinning = 2;
            if ((resolutionInfo.binning & DUOBinning.DUO_BIN_HORIZONTAL4) == DUOBinning.DUO_BIN_HORIZONTAL4)
                hBinning = 4;
            int vBinning = 1;
            if ((resolutionInfo.binning & DUOBinning.DUO_BIN_VERTICAL2) == DUOBinning.DUO_BIN_VERTICAL2)
                vBinning = 2;
            if ((resolutionInfo.binning & DUOBinning.DUO_BIN_VERTICAL4) == DUOBinning.DUO_BIN_VERTICAL4)
                vBinning = 4;
            return String.Format("[Resolution Info]\nFrame Resolution [w/h] : {0} x {1}\nFrame Binning [h/v] : {2} x {3}\nFramerate : {4}\n", resolutionInfo.width, resolutionInfo.height, hBinning, vBinning, resolutionInfo.fps);
        }
        private string GetDeviceDescriptionString()
        {
            if (duoInstance == IntPtr.Zero)
                return DUOLibInternal.UNKNOWN_STRING;
            return String.Format("Device Name : {0}\nSerial Number : {1}\n" +
                                  "Firmware Version : {2}\nFirmware Build : {3}\n" +
                                  "Device Status: {4} \n" +
                                  "Exposure [%] : {5}\nGain [%] : {6}\nLED [%] : {7}\n" +
                                  "Camera Flip [h/v] : [{8}, {9}]\nCamera Swap : [{10}]\n",
                                  DeviceName, SerialNumber, FirmwareVersion, FirmwareBuild, Running ? "Running" : "Not Running",
                                  Exposure, Gain, LED, HFlip ? "YES" : "NO", VFlip ? "YES" : "NO", CameraSwap ? "YES" : "NO");
        }
        public override string ToString()
        {
            if (duoInstance == IntPtr.Zero)
                return DUOLibInternal.UNKNOWN_STRING;
            return GetDeviceDescriptionString() + GetResolutionInfoString(resolutionInfo);
        }
        #endregion
        #region Events
        public event DUOFrameReceivedHandler DUOFrameReceived;
        public event DUODeviceStatusChangedHandler DUODeviceStatusChanged;
        #endregion
    }
}