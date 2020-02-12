using System;

namespace Fuse.Extensions
{
    public static class NullableExtensions
    {
        public static T ThrowIfNull<T>(this T? value, string message) where T : class => value ?? throw new Exception(message);
    }
}
