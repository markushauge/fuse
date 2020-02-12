using System;

namespace Fuse.Extensions
{
    public static class BoolExtensions
    {
        public static bool ThrowIfFalse(this bool value, string message) => !value ? throw new Exception(message) : value;
        public static bool ThrowIfTrue(this bool value, string message) => value ? throw new Exception(message) : value;
    }
}
