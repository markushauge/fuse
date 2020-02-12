using System;

namespace Fuse.Extensions
{
    public static class IntPtrExtensions
    {
        public static bool IsZero(this IntPtr value) => value == IntPtr.Zero;
        public static IntPtr ThrowIfZero(this IntPtr value, string message) => value.IsZero() ? throw new Exception(message) : value;
    }
}
