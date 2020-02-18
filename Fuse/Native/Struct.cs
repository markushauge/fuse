using System;
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

            public T Create(IntPtr pointer) => Constructor(pointer);
            public T Create(IntPtr pointer, int index) => Constructor(pointer + Size * index);
        }
        
        public class Builder
        {
            public IntPtr Pointer { get; }
            public int Offset { get; private set; }

            public Builder(IntPtr pointer)
            {
                Pointer = pointer;
            }

            public Builder AddPadding(int padding)
            {
                Offset += padding;
                return this;
            }

            public Builder AddProperty<T>(out Property<T> property, int size, Func<IntPtr, T> get)
            {
                var pointer = Pointer.Offset(Offset);
                T Get() => get(pointer);
                property = new Property<T>(Get);
                return AddPadding(size);
            }

            public Builder AddUInt8(out Property<byte> property) =>
                AddProperty(out property, sizeof(byte), IntPtrExtensions.ReadUInt8);

            public Builder AddInt8(out Property<sbyte> property) =>
                AddProperty(out property, sizeof(sbyte), IntPtrExtensions.ReadInt8);

            public Builder AddUInt16(out Property<ushort> property) =>
                AddProperty(out property, sizeof(ushort), IntPtrExtensions.ReadUInt16);

            public Builder AddInt16(out Property<short> property) =>
                AddProperty(out property, sizeof(short), IntPtrExtensions.ReadInt16);

            public Builder AddUInt32(out Property<uint> property) =>
                AddProperty(out property, sizeof(uint), IntPtrExtensions.ReadUInt32);

            public Builder AddInt32(out Property<int> property) =>
                AddProperty(out property, sizeof(int), IntPtrExtensions.ReadInt32);

            public Builder AddUInt64(out Property<ulong> property) =>
                AddProperty(out property, sizeof(ulong), IntPtrExtensions.ReadUInt64);

            public Builder AddInt64(out Property<long> property) =>
                AddProperty(out property, sizeof(long), IntPtrExtensions.ReadInt64);

            public Builder AddIntPtr(out Property<IntPtr> property) =>
                AddProperty(out property, IntPtr.Size, IntPtrExtensions.ReadIntPtr);

            public Builder AddReference<T>(out Property<T> property, Func<IntPtr, T> constructor) =>
                AddProperty(out property, IntPtr.Size, pointer => constructor(pointer.ReadIntPtr()));

            public Builder AddStruct<T>(out Property<T> property, int size, Func<IntPtr, T> constructor) =>
                AddProperty(out property, size, constructor);
        }
    }
}
