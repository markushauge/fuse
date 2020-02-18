using System;

namespace Fuse.Native
{
    public class Property<T>
    {
        public Func<T> Get { get; }

        public Property(Func<T> get)
        {
            Get = get;
        }
    }
}