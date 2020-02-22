using System;

namespace Fuse.Extensions
{
    public static class IntPtrExtensions
    {
        public static bool IsZero(this IntPtr pointer) =>
            pointer == IntPtr.Zero;

        public static IntPtr ThrowIfZero(this IntPtr pointer, string message) =>
            pointer.IsZero() ? throw new Exception(message) : pointer;
    }
}
