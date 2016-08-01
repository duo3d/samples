using System;
using System.Runtime.InteropServices;

namespace DUODeviceLib
{
	#region [ DUO Enums ]
	// DUO binning
	public enum DUOBinning
	{
		DUO_BIN_ANY         = -1,
		DUO_BIN_NONE        = 0x00,
		DUO_BIN_HORIZONTAL2 = 0x01, // Horizontal binning by factor of 2
		DUO_BIN_HORIZONTAL4 = 0x02, // Horizontal binning by factor of 4
		DUO_BIN_VERTICAL2   = 0x04, // Vertical binning by factor of 2
		DUO_BIN_VERTICAL4   = 0x08  // Vertical binning by factor of 4
	}
	#endregion
	#region [ DUO Structures ]
	// DUO resolution info
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct DUOResolutionInfo
	{
		[MarshalAs(UnmanagedType.I4)]
		public int width;
		[MarshalAs(UnmanagedType.I4)]
		public int height;
		[MarshalAs(UnmanagedType.I4)]
		public DUOBinning binning;
		[MarshalAs(UnmanagedType.R4)]
		public float fps;
		[MarshalAs(UnmanagedType.R4)]
		public float minFps;
		[MarshalAs(UnmanagedType.R4)]
		public float maxFps;
	};
    // DUO IMU Sample
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DUOIMUSample
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint timeStamp;			    // IMU time stamp in 100us increments
        [MarshalAs(UnmanagedType.R4)]
        public float tempData;				// DUO temperature data
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
        public float[] accelData;			// DUO accelerometer data
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
        public float[] gyroData;			// DUO gyroscope data
    };
	// DUO Frame
	[StructLayout(LayoutKind.Sequential)]
	public struct DUOFrame
	{
		[MarshalAs(UnmanagedType.U4)]
		public uint  width;				    // DUO frame width
		[MarshalAs(UnmanagedType.U4)]
		public uint  height;		    	// DUO frame height
		[MarshalAs(UnmanagedType.U1)]
		public byte  ledSeqTag;			    // DUO frame LED tag
		[MarshalAs(UnmanagedType.U4)]
		public uint  timeStamp;			    // DUO frame time stamp in 100us increments
		public IntPtr leftData;		        // DUO left frame data
		public IntPtr rightData;			// DUO right frame data
		[MarshalAs(UnmanagedType.U1)]
        public bool IMUPresent;  	        // True if accelerometer chip is present
        [MarshalAs(UnmanagedType.U4)]
        public uint IMUSamples;             // Number of IMU data samples in this frame
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public DUOIMUSample[] IMUData;		// DUO IMU samples
	}
    // DUO LED PWM
    [StructLayout(LayoutKind.Sequential)]
    public struct DUOLEDSeq
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        byte[] ledPwmValue;			// LED PWM values are in percentage [0,100]
    }
    // DUO Intrinsics
    [StructLayout(LayoutKind.Sequential)]
    public struct DUOIntr
    {
        [MarshalAs(UnmanagedType.R4)]
        public double k1;
        [MarshalAs(UnmanagedType.R4)]
        public double k2;
        [MarshalAs(UnmanagedType.R4)]
        public double k3;
        [MarshalAs(UnmanagedType.R4)]
        public double k4;
        [MarshalAs(UnmanagedType.R4)]
        public double k5;
        [MarshalAs(UnmanagedType.R4)]
        public double k6;
        [MarshalAs(UnmanagedType.R4)]
        public double p1;
        [MarshalAs(UnmanagedType.R4)]
        public double p2;
        [MarshalAs(UnmanagedType.R4)]
        public double fx;
        [MarshalAs(UnmanagedType.R4)]
        public double fy;
        [MarshalAs(UnmanagedType.R4)]
        public double cx;
        [MarshalAs(UnmanagedType.R4)]
        public double cy;
    }
    // DUO Intrinsics
    [StructLayout(LayoutKind.Sequential)]
    public struct DUOIntrinsics
    {
        [MarshalAs(UnmanagedType.U2)]
        public ushort width;			    // DUO frame width
        [MarshalAs(UnmanagedType.U2)]
        public ushort height;		    	// DUO frame height
		[MarshalAs(UnmanagedType.ByValArray, SizeConst=12, ArraySubType=UnmanagedType.R4)]
		public double[] left;	            // DUO left camera intrinsics
		[MarshalAs(UnmanagedType.ByValArray, SizeConst=12, ArraySubType=UnmanagedType.R4)]
		public double[] right;	            // DUO right camera intrinsics
    }
    // DUO Extrinsics
    [StructLayout(LayoutKind.Sequential)]
    public struct DUOExtrinsics
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.R4)]
        public double[] rotation;	        // DUO camera rotation matrix
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
        public double[] translation;        // DUO camera translation vector
    }
    // DUO Stereo
    [StructLayout(LayoutKind.Sequential)]
    public struct DUOStereo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.R4)]
        public double[] M1;	                // 3x3 - Camera matrix
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.R4)]
        public double[] M2;	                // 3x3 - Camera matrix
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.R4)]
        public double[] D1;	                // 1x8 - Camera distortion parameters
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.R4)]
        public double[] D2;	                // 1x8 - Camera distortion parameters
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.R4)]
        public double[] R;	                // 3x3 - Rotation between left and right camera
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
        public double[] T;                  // 3x1 - Translation vector between left and right camera
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.R4)]
        public double[] R1;	                // 3x3 - Rectified rotation matrix
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.R4)]
        public double[] R2;	                // 3x3 - Rectified rotation matrix
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.R4)]
        public double[] P1;	                // 3x4 - Rectified projection matrix
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.R4)]
        public double[] P2;	                // 3x4 - Rectified projection matrix
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.R4)]
        public double[] Q;	                // 4x4 - Disparity to depth mapping matrix
    }
    #endregion
	#region [ DUO Callbacks ]
	public delegate void DUOFrameCallback(ref DUOFrame pFrameData, IntPtr pUserData);
	#endregion
	#region [ DUO Library ]
	public static class DUOLib
	{
        #region [ DUO Internal Helpers ]
        internal class UnmanagedString : IDisposable
        {
            protected IntPtr strPtr;
            protected bool disposed;

            public UnmanagedString()
            {
                strPtr = Marshal.AllocHGlobal(DUOLibInternal.CL_STRING_MAX_LENGTH);
            }
            ~UnmanagedString()
            {
                Dispose();
            }
            public void Dispose()
            {
                if (!disposed)
                {
                    Marshal.FreeHGlobal(strPtr);
                    disposed = true;
                }
            }
            public IntPtr Pointer
            {
                get { return strPtr; }
            }
            public string String
            {
                get { return Marshal.PtrToStringAnsi(strPtr); }
            }
        }
        #endregion
        #region [ DUO Default Settings ]
        public const double DUO_DEFAULT_GAIN = 10.0;
		public const double DUO_DEFAULT_EXPOSURE = 20.0;
		public const double DUO_DEFAULT_PWM = 20.0;
		public const int DUO_DEFAULT_RESOLUTION_WIDTH = 320;
		public const int DUO_DEFAULT_RESOLUTION_HEIGHT = 240;
		public const DUOBinning DUO_DEFAULT_RESOLUTION_BINNING = DUOBinning.DUO_BIN_HORIZONTAL2 | DUOBinning.DUO_BIN_VERTICAL2;
		public const float DUO_DEFAULT_RESOLUTION_FRAMERATE = 30.0f;
		public const bool DUO_DEFAULT_HFLIP = false;
		public const bool DUO_DEFAULT_VFLIP = false;
		public const bool DUO_DEFAULT_SWAP_CAMERAS = false;
        public const bool DUO_DEFAULT_UNDISTORT = false;
        #endregion
		#region [ DUO Library Generic Logic ]
		public static string GetLibVersion()
		{
			string libVersionString = null;
			IntPtr rawPtr = DUOLibInternal.GetDUOLibVersion();
            if (rawPtr != IntPtr.Zero)
            {
                try { libVersionString = Marshal.PtrToStringAnsi(rawPtr); }
                catch { }
            }
			return libVersionString;
		}
		public static int EnumerateResolutions(DUOResolutionInfo[] resolutions, int resListSize, int width = -1, int height = -1, 
                                               DUOBinning binning = DUOBinning.DUO_BIN_ANY, float fps = -1)
		{
			return DUOLibInternal.EnumerateDUOResolutions(ref resolutions[0], resListSize, width, height, binning, fps);
		}
		#endregion
        #region [ DUO Device Initialization ]
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "OpenDUO")]
        public static extern bool OpenDUO(ref IntPtr duo);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CloseDUO")]
        public static extern bool CloseDUO(IntPtr duo);
        #endregion
        #region [ DUO Device Capture Control ]
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "StartDUO")]
        public static extern bool StartDUO(IntPtr duo, DUOFrameCallback frameCallback, IntPtr pUserData, bool masterMode = true);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "StopDUO")]
        public static extern bool StopDUO(IntPtr duo);
        #endregion
		#region [ DUO Get Parameters ]
		public static bool GetDUODeviceName(IntPtr duo, ref string val)
		{
            try
            {
                UnmanagedString str = new UnmanagedString();
                if (DUOLibInternal.DUOGetDeviceName(duo, str.Pointer))
                {
                    val = str.String;
                    return true;
                }
                else return false;
            }
            catch{ return false; }
		}
		public static bool GetDUOSerialNumber(IntPtr duo, ref string val)
		{
            try
            {
                UnmanagedString str = new UnmanagedString();
                if (DUOLibInternal.DUOGetSerialNumber(duo, str.Pointer))
                {
                    val = str.String;
                    return true;
                }
                else return false;
            }
            catch { return false; }
		}
		public static bool GetDUOFirmwareVersion(IntPtr duo, ref string val)
		{
            try
            {
                UnmanagedString str = new UnmanagedString();
                if (DUOLibInternal.DUOGetFirmwareVersion(duo, str.Pointer))
                {
                    val = str.String;
                    return true;
                }
                else return false;
            }
            catch { return false; }
		}
		public static bool GetDUOFirmwareBuild(IntPtr duo, ref string val)
		{
            try
            {
                UnmanagedString str = new UnmanagedString();
                if (DUOLibInternal.DUOGetFirmwareBuild(duo, str.Pointer))
                {
                    val = str.String;
                    return true;
                }
                else return false;
            }
            catch { return false; }
		}
		public static bool GetDUOResolutionInfo(IntPtr duo, ref DUOResolutionInfo val)
		{
            return DUOLibInternal.DUOGetResolutionInfo(duo, ref val);
		}
		public static bool GetDUOFrameDimension(IntPtr duo, ref uint w, ref uint h)
		{
            return DUOLibInternal.DUOGetFrameDimension(duo, ref w, ref h);
		}
		public static bool GetDUOExposure(IntPtr duo, ref double val)
		{
            return DUOLibInternal.DUOGetExposure(duo, ref val);
		}
		public static bool GetDUOExposureMS(IntPtr duo, ref double val)
		{
            return DUOLibInternal.DUOGetExposureMS(duo, ref val);
		}
		public static bool GetDUOGain(IntPtr duo, ref double val)
		{
            return DUOLibInternal.DUOGetGain(duo, ref val);
		}
		public static bool GetDUOHFlip(IntPtr duo, ref bool val)
		{
            return DUOLibInternal.DUOGetHFlip(duo, ref val);
		}
		public static bool GetDUOVFlip(IntPtr duo, ref bool val)
		{
            return DUOLibInternal.DUOGetVFlip(duo, ref val);
        }
		public static bool GetDUOCameraSwap(IntPtr duo, ref bool val)
		{
            return DUOLibInternal.DUOGetameraSwap(duo, ref val);
        }
		public static bool GetDUOLedPWM(IntPtr duo, ref double val)
		{
            return DUOLibInternal.DUOGetLedPWM(duo, ref val);
        }
        public static bool GetDUOCalibrationPresent(IntPtr duo, ref bool val)
		{
            return DUOLibInternal.DUOGetCalibrationPresent(duo, ref val);
        }
		public static bool GetDUOFOV(IntPtr duo, ref double[] val)
		{
            return DUOLibInternal.DUOGetFOV(duo, ref val);
        }
		public static bool GetDUOUndistort(IntPtr duo, ref bool val)
		{
            return DUOLibInternal.DUOGetUndistort(duo, ref val);
		}
		public static bool GetDUOIntrinsics(IntPtr duo, ref DUOIntrinsics val)
		{
            return DUOLibInternal.DUOGetIntrinsics(duo, ref val);
		}
		public static bool GetDUOExtrinsics(IntPtr duo, ref DUOExtrinsics val)
		{
            return DUOLibInternal.DUOGetExtrinsics(duo, ref val);
		}
        public static bool GetDUOStereoParameters(IntPtr duo, ref DUOStereo val)
        {
            return DUOLibInternal.DUOGetStereoParameters(duo, ref val);
        }
        #endregion
		#region [ DUO Set Parameters ]
		public static bool SetDUOResolutionInfo(IntPtr duo, DUOResolutionInfo val)
		{
            return DUOLibInternal.DUOSetResolutionInfo(duo, val);
		}
		public static bool SetDUOExposure(IntPtr duo, double val)
		{
            return DUOLibInternal.DUOSetExposure(duo, val);
		}
		public static bool SetDUOExposureMS(IntPtr duo, double val)
		{
            return DUOLibInternal.DUOSetExposureMS(duo, val);
		}
		public static bool SetDUOGain(IntPtr duo, double val)
		{
            return DUOLibInternal.DUOSetGain(duo, val);
		}
		public static bool SetDUOHFlip(IntPtr duo, bool val)
		{
            return DUOLibInternal.DUOSetHFlip(duo, val);
		}
		public static bool SetDUOVFlip(IntPtr duo, bool val)
		{
            return DUOLibInternal.DUOSetVFlip(duo, val);
		}
		public static bool SetDUOCameraSwap(IntPtr duo, bool val)
		{
            return DUOLibInternal.DUOSetCameraSwap(duo, val);
		}
		public static bool SetDUOLedPWM(IntPtr duo, double val)
		{
            return DUOLibInternal.DUOSetLedPWM(duo, val);
		}
		public static bool SetDUOLedPWMSeq(IntPtr duo, DUOLEDSeq[] seq, uint count)
		{
            return DUOLibInternal.DUOSetLedPWMSeq(duo, ref seq[0], count);
		}
		public static bool SetDUOUndistort(IntPtr duo, bool val)
		{
            return DUOLibInternal.DUOSetUndistort(duo, val);
		}
		#endregion
	}
	#endregion
}