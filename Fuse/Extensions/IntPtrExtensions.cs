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

        public static unsafe T* To<T>(this IntPtr pointer) where T : unmanaged =>
            (T*)pointer;

        public static unsafe T Read<T>(this IntPtr pointer) where T : unmanaged =>
            *(T*)pointer;

        public static unsafe T* ReadPointer<T>(this IntPtr pointer) where T : unmanaged =>
            *(T**)pointer;

        public static unsafe void Write<T>(this IntPtr pointer, T value) where T : unmanaged
        {
            *(T*) pointer = value;
        }

        public static unsafe void WritePointer<T>(this IntPtr pointer, T* value) where T : unmanaged
        {
            *(T**)pointer = value;
        }
    }
}
