using System;
using System.Collections.Generic;
using System.Linq;

namespace Fuse.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> enumerable) =>
            enumerable.Select((value, index) => (index, value));

        public static (IEnumerable<T>, IEnumerable<T>) Split<T>(
            this IEnumerable<T> enumerable,
            T delimiter
        ) where T : IEquatable<T> => (
            enumerable.TakeWhile(value => !value.Equals(delimiter)),
            enumerable.SkipWhile(value => !value.Equals(delimiter)).Skip(1)
        );
    }
}
