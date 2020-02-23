using System;
using System.Linq;
using System.Runtime.InteropServices;
using Fuse.Native.Win32;

namespace Fuse.Native
{
    public static class IntPtrExtensions
    {
        public static IntPtr Offset(this IntPtr pointer, int offset) =>
            pointer + offset;

        public static unsafe T* To<T>(this IntPtr pointer) where T : unmanaged =>
            (T*)pointer;

        public static unsafe T Read<T>(this IntPtr pointer) where T : unmanaged =>
            *(T*)pointer;

        public static unsafe void Write<T>(this IntPtr pointer, T value) where T : unmanaged
        {
            *(T*)pointer = value;
        }

        public static unsafe IntPtr Append<T>(this IntPtr pointer, T value) where T : unmanaged
        {
            pointer.Write(value);
            return pointer.Offset(sizeof(T));
        }

        public static IntPtr Append<T>(this IntPtr pointer, params T[] values) where T : unmanaged =>
            values.Aggregate(pointer, Append);

        public static IntPtr FollowRelative(this IntPtr pointer) =>
            pointer.Offset(pointer.Read<int>() + sizeof(int));

        public static IDisposable Protect(this IntPtr pointer, uint size, MemoryProtection protection) =>
            new MemoryProtectionBlock(pointer, size, protection);

        public static T ToDelegate<T>(this IntPtr pointer) =>
            Marshal.GetDelegateForFunctionPointer<T>(pointer);

        public static IntPtr ToIntPtr<T>(this T @delegate) where T : Delegate =>
            Marshal.GetFunctionPointerForDelegate(@delegate);
    }
}