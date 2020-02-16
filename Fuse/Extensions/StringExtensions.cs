using System.Collections.Generic;
using System.Linq;

namespace Fuse.Extensions
{
    public static class StringExtensions
    {
        // Splitting a string into chunks of a certain size
        // https://stackoverflow.com/a/1450797
        public static IEnumerable<string> Chunk(this string str, int chunkSize) =>
            Enumerable
                .Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
    }
}