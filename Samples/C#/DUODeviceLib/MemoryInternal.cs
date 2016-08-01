using System;
using System.Runtime.InteropServices;

namespace DUODeviceLib
{
    internal static class MemoryInternal
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpFileMappingAttributes, uint flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool UnmapViewOfFile(IntPtr hMap);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr hHandle);
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        internal static extern void CopyMemory(IntPtr Destination, IntPtr Source, int Length);
    }
}
