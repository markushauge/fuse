using System;

namespace Fuse.Native
{
    public abstract class NativeObject
    {
        public class Factory<T> where T : NativeObject
        {
            private readonly int _size;
            private readonly Func<IntPtr, T> _constructor;

            public Factory(int size, Func<IntPtr, T> constructor)
            {
                _size = size;
                _constructor = constructor;
            }

            public T Create(IntPtr pointer, int offset = 0) =>
                _constructor(pointer + offset * _size);
        }

        public IntPtr Pointer { get; }

        protected NativeObject(IntPtr pointer)
        {
            Pointer = pointer;
        }

        public override string ToString() =>
            $"{GetType().Name}({Pointer.ToInt64():X8})";
    }
}
