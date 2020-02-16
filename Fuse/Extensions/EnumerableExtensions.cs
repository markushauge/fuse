using System.Collections.Generic;
using System.Linq;

namespace Fuse.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> enumerable) =>
            enumerable.Select((value, index) => (index, value));
    }
}
