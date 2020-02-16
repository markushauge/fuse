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

        public static unsafe byte ReadUInt8(this IntPtr pointer, int offset = 0) =>
            *(byte*)(pointer + offset).ToPointer();

        public static unsafe sbyte ReadInt8(this IntPtr pointer, int offset = 0) =>
            *(sbyte*)(pointer + offset).ToPointer();

        public static unsafe ushort ReadUInt16(this IntPtr pointer, int offset = 0) =>
            *(ushort*)(pointer + offset).ToPointer();

        public static unsafe short ReadInt16(this IntPtr pointer, int offset = 0) =>
            *(short*)(pointer + offset).ToPointer();

        public static unsafe uint ReadUInt32(this IntPtr pointer, int offset = 0) =>
            *(uint*)(pointer + offset).ToPointer();

        public static unsafe int ReadInt32(this IntPtr pointer, int offset = 0) =>
            *(int*)(pointer + offset).ToPointer();

        public static unsafe ulong ReadUInt64(this IntPtr pointer, int offset = 0) =>
            *(ulong*)(pointer + offset).ToPointer();

        public static unsafe long ReadInt64(this IntPtr pointer, int offset = 0) =>
            *(long*)(pointer + offset).ToPointer();

        public static unsafe IntPtr ReadIntPtr(this IntPtr pointer, int offset = 0) =>
            *(IntPtr*)(pointer + offset).ToPointer();
    }
}
