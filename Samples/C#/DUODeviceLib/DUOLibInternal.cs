using System;
using System.Runtime.InteropServices;
using DUODeviceLib;

namespace DUODeviceLib
{
    internal static class DUOLibInternal
    {
        #region [ DUO internal const]
        internal const int CL_STRING_MAX_LENGTH = 252;
        internal const string UNKNOWN_STRING = "Unknown";
        internal static DUOResolutionInfo DUO_DEFAULT_RESOLUTION_INFO
        {
            get
            {
                DUOResolutionInfo res = new DUOResolutionInfo();
                EnumerateDUOResolutions(ref res, 1, DUOLib.DUO_DEFAULT_RESOLUTION_WIDTH, DUOLib.DUO_DEFAULT_RESOLUTION_HEIGHT, 
                                        DUOLib.DUO_DEFAULT_RESOLUTION_BINNING, DUOLib.DUO_DEFAULT_RESOLUTION_FRAMERATE);
                return res;
            }
        }
        #endregion
        #region [ DUO library imported generic logic ]
        [DllImport("DUOLib", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOLibVersion")]
        internal static extern IntPtr GetDUOLibVersion();
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnumerateDUOResolutions")]
        [return: MarshalAsAttribute(UnmanagedType.I4)]
        internal static extern int EnumerateDUOResolutions(ref DUOResolutionInfo resolutions, int resListSize, int width, int height, DUOBinning binning, float fps);
        #endregion
        #region [ DUO Set/Get imported logic ]
        // Get
        [DllImport("DUOLib", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUODeviceName")]
        internal static extern bool DUOGetDeviceName(IntPtr duo, IntPtr val);
        [DllImport("DUOLib", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOSerialNumber")]
        internal static extern bool DUOGetSerialNumber(IntPtr duo, IntPtr val);
        [DllImport("DUOLib", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOFirmwareVersion")]
        internal static extern bool DUOGetFirmwareVersion(IntPtr duo, IntPtr val);
        [DllImport("DUOLib", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOFirmwareBuild")]
        internal static extern bool DUOGetFirmwareBuild(IntPtr duo, IntPtr val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOResolutionInfo")]
        internal static extern bool DUOGetResolutionInfo(IntPtr duo, ref DUOResolutionInfo val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOFrameDimension")]
        internal static extern bool DUOGetFrameDimension(IntPtr duo, ref uint w, ref uint h);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOExposure")]
        internal static extern bool DUOGetExposure(IntPtr duo, ref double val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOExposureMS")]
        internal static extern bool DUOGetExposureMS(IntPtr duo, ref double val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOGain")]
        internal static extern bool DUOGetGain(IntPtr duo, ref double val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOHFlip")]
        internal static extern bool DUOGetHFlip(IntPtr duo, ref bool val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOVFlip")]
        internal static extern bool DUOGetVFlip(IntPtr duo, ref bool val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOCameraSwap")]
        internal static extern bool DUOGetameraSwap(IntPtr duo, ref bool val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOLedPWM")]
        internal static extern bool DUOGetLedPWM(IntPtr duo, ref double val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOCalibrationPresent")]
        internal static extern bool DUOGetCalibrationPresent(IntPtr duo, ref bool val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOFOV")]
        internal static extern bool DUOGetFOV(IntPtr duo, ref double[] val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOUndistort")]
        internal static extern bool DUOGetUndistort(IntPtr duo, ref bool val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOIntrinsics")]
        internal static extern bool DUOGetIntrinsics(IntPtr duo, ref DUOIntrinsics val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOExtrinsics")]
        internal static extern bool DUOGetExtrinsics(IntPtr duo, ref DUOExtrinsics val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetDUOStereoParameters")]
        internal static extern bool DUOGetStereoParameters(IntPtr duo, ref DUOStereo val);
        // Set
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDUOResolutionInfo")]
        internal static extern bool DUOSetResolutionInfo(IntPtr duo, DUOResolutionInfo val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDUOExposure")]
        internal static extern bool DUOSetExposure(IntPtr duo, double val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDUOExposureMS")]
        internal static extern bool DUOSetExposureMS(IntPtr duo, double val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDUOGain")]
        internal static extern bool DUOSetGain(IntPtr duo, double val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDUOHFlip")]
        internal static extern bool DUOSetHFlip(IntPtr duo, bool val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDUOVFlip")]
        internal static extern bool DUOSetVFlip(IntPtr duo, bool val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDUOCameraSwap")]
        internal static extern bool DUOSetCameraSwap(IntPtr duo, bool val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDUOLedPWM")]
        internal static extern bool DUOSetLedPWM(IntPtr duo, double val);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDUOLedPWMSeq")]
        internal static extern bool DUOSetLedPWMSeq(IntPtr duo, ref DUOLEDSeq seq, uint count);
        [DllImport("DUOLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetDUOUndistort")]
        internal static extern bool DUOSetUndistort(IntPtr duo, bool val);
        #endregion
    }
}
