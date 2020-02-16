using System;

namespace Fuse.Extensions
{
    public static class IntPtrExtensions
    {
        public static bool IsZero(this IntPtr value) =>
            value == IntPtr.Zero;

        public static IntPtr ThrowIfZero(this IntPtr value, string message) =>
            value.IsZero() ? throw new Exception(message) : value;

        public static byte ReadUInt8(this IntPtr address) =>
            address.ReadUInt8(0);

        public static unsafe byte ReadUInt8(this IntPtr address, int offset) =>
            *(byte*)(address + offset).ToPointer();

        public static sbyte ReadInt8(this IntPtr address) =>
            address.ReadInt8(0);

        public static unsafe sbyte ReadInt8(this IntPtr address, int offset) =>
            *(sbyte*)(address + offset).ToPointer();

        public static ushort ReadUInt16(this IntPtr address) =>
            address.ReadUInt16(0);

        public static unsafe ushort ReadUInt16(this IntPtr address, int offset) =>
            *(ushort*)(address + offset).ToPointer();

        public static short ReadInt16(this IntPtr address) =>
            address.ReadInt16(0);

        public static unsafe short ReadInt16(this IntPtr address, int offset) =>
            *(short*)(address + offset).ToPointer();

        public static uint ReadUInt32(this IntPtr address) =>
            address.ReadUInt32(0);

        public static unsafe uint ReadUInt32(this IntPtr address, int offset) =>
            *(uint*)(address + offset).ToPointer();

        public static int ReadInt32(this IntPtr address) =>
            address.ReadInt32(0);

        public static unsafe int ReadInt32(this IntPtr address, int offset) =>
            *(int*)(address + offset).ToPointer();

        public static ulong ReadUInt64(this IntPtr address) =>
            address.ReadUInt64(0);

        public static unsafe ulong ReadUInt64(this IntPtr address, int offset) =>
            *(ulong*)(address + offset).ToPointer();

        public static long ReadInt64(this IntPtr address) =>
            address.ReadInt64(0);

        public static unsafe long ReadInt64(this IntPtr address, int offset) =>
            *(long*)(address + offset).ToPointer();

        public static IntPtr ReadIntPtr(this IntPtr address) =>
            address.ReadIntPtr(0);

        public static unsafe IntPtr ReadIntPtr(this IntPtr address, int offset) =>
            *(IntPtr*)(address + offset).ToPointer();
    }
}
