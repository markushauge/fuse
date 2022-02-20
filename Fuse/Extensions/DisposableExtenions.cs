using System;
using System.Collections.Generic;

namespace Fuse.Extensions
{
    public static class DisposableExtensions
    {
        public static T DisposeWith<T>(this T value, ICollection<IDisposable> disposables)
            where T : IDisposable
        {
            disposables.Add(value);
            return value;
        }
    }
}
