using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilZ.Dotnet.Ex.NativeMethod
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ICMP_ECHO_REPLY
    {
        public int Address;
        public int Status;
        public int RoundTripTime;
        public short DataSize;
        public short Reserved;
        public IntPtr DataPtr;
        public ICMP_OPTIONS Options;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 250)]
        public string Data;
    }
}
