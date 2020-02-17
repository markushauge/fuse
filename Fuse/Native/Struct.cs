using System;
using Fuse.Compose;
using Fuse.Extensions;

namespace Fuse.Native
{
    public static class Struct
    {
        public class Factory<T>
        {
            public int Size { get; }
            public Func<IntPtr, T> Constructor { get; }

            public Factory(int size, Func<IntPtr, T> constructor)
            {
                Size = size;
                Constructor = constructor;
            }

            public T Create(IntPtr pointer, int offset = 0) => Constructor(pointer + offset * Size);
        }

        public class Builder
        {
            public IntPtr Pointer { get; }
            public int Offset { get; private set; }

            public Builder(IntPtr pointer)
            {
                Pointer = pointer;
            }

            public Builder Pad(int padding)
            {
                Offset += padding;
                return this;
            }

            public Builder AddUInt8(out Property<byte> property)
            {
                var offset = Offset;
                property = new Property<byte>(() => Pointer.ReadUInt8(offset));
                return Pad(sizeof(byte));
            }

            public Builder AddInt8(out Property<sbyte> property)
            {
                var offset = Offset;
                property = new Property<sbyte>(() => Pointer.ReadInt8(offset));
                return Pad(sizeof(sbyte));
            }

            public Builder AddUInt16(out Property<ushort> property)
            {
                var offset = Offset;
                property = new Property<ushort>(() => Pointer.ReadUInt16(offset));
                return Pad(sizeof(ushort));
            }

            public Builder AddInt16(out Property<short> property)
            {
                var offset = Offset;
                property = new Property<short>(() => Pointer.ReadInt16(offset));
                return Pad(sizeof(short));
            }

            public Builder AddUInt32(out Property<uint> property)
            {
                var offset = Offset;
                property = new Property<uint>(() => Pointer.ReadUInt32(offset));
                return Pad(sizeof(uint));
            }

            public Builder AddInt32(out Property<int> property)
            {
                var offset = Offset;
                property = new Property<int>(() => Pointer.ReadInt32(offset));
                return Pad(sizeof(int));
            }

            public Builder AddUInt64(out Property<ulong> property)
            {
                var offset = Offset;
                property = new Property<ulong>(() => Pointer.ReadUInt64(offset));
                return Pad(sizeof(ulong));
            }

            public Builder AddInt64(out Property<long> property)
            {
                var offset = Offset;
                property = new Property<long>(() => Pointer.ReadInt64(offset));
                return Pad(sizeof(long));
            }

            public Builder AddIntPtr(out Property<IntPtr> property)
            {
                var offset = Offset;
                property = new Property<IntPtr>(() => Pointer.ReadIntPtr(offset));
                return Pad(IntPtr.Size);
            }
            
            public Builder AddReference<T>(out Property<T> property, Func<IntPtr, T> constructor)
            {
                var offset = Offset;
                property = new Property<T>(() => constructor(Pointer.ReadIntPtr(offset)));
                return Pad(IntPtr.Size);
            }

            public Builder AddStruct<T>(out Property<T> property, int size, Func<IntPtr, T> constructor)
            {
                var offset = Offset;
                property = new Property<T>(() => constructor(Pointer.Offset(offset)));
                return Pad(size);
            }
        }
    }
}
