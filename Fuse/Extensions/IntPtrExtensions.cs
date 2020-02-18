using System;

namespace Fuse.Extensions
{
    public static class IntPtrExtensions
    {
        public static bool IsZero(this IntPtr pointer) =>
            pointer == IntPtr.Zero;

        public static IntPtr ThrowIfZero(this IntPtr pointer, string message) =>
            pointer.IsZero() ? throw new Exception(message) : pointer;

        public static IntPtr Offset(this IntPtr pointer, int offset) =>
            pointer + offset;

        public static unsafe byte ReadUInt8(this IntPtr pointer) =>
            *(byte*)pointer;

        public static unsafe sbyte ReadInt8(this IntPtr pointer) =>
            *(sbyte*)pointer;

        public static unsafe ushort ReadUInt16(this IntPtr pointer) =>
            *(ushort*)pointer;

        public static unsafe short ReadInt16(this IntPtr pointer) =>
            *(short*)pointer;

        public static unsafe uint ReadUInt32(this IntPtr pointer) =>
            *(uint*)pointer;

        public static unsafe int ReadInt32(this IntPtr pointer) =>
            *(int*)pointer;

        public static unsafe ulong ReadUInt64(this IntPtr pointer) =>
            *(ulong*)pointer;

        public static unsafe long ReadInt64(this IntPtr pointer) =>
            *(long*)pointer;

        public static unsafe IntPtr ReadIntPtr(this IntPtr pointer) =>
            *(IntPtr*)pointer;
    }
}
