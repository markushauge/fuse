using System;
using System.Runtime.InteropServices;
using Fuse.Native.Win32;
using static Fuse.Native.Win32.AllocationType;
using static Fuse.Native.Win32.MemoryProtection;

namespace Fuse.Native
{
    public class TrampolineDetour<T> : IDetour
    {
        private const int JmpSize = 0x0C;

        private readonly IntPtr _pointer;
        private readonly uint _size;
        private readonly IntPtr _trampoline;
        private GCHandle _handle;
        
        public TrampolineDetour(IntPtr pointer, uint size, Transform<T> detourTransform)
        {
            _pointer = pointer;
            _size = size;
            var trampolineSize = _size + JmpSize;
            _trampoline = Kernel32.VirtualAlloc(IntPtr.Zero, trampolineSize, Commit | Reserve, ExecuteRead);

            using (_trampoline.Protect(trampolineSize, ReadWrite))
            {
                Kernel32.CopyMemory(_trampoline, _pointer, _size);

                _trampoline
                    .Offset((int)_size)
                    .AppendJmp(_pointer.Offset((int)_size));
            }

            var original = _trampoline.ToDelegate<T>();
            var detour = detourTransform(original);
            _handle = GCHandle.Alloc(detour, GCHandleType.Normal);
            var detourPointer = detour.ToIntPtr();

            using (_pointer.Protect(_size, ReadWrite))
            {
                _pointer
                    .AppendJmp(detourPointer)
                    .AppendNop((int)_size - JmpSize);
            }
        }

        public void Dispose()
        {
            using (_pointer.Protect(_size, ReadWrite))
            {
                Kernel32.CopyMemory(_pointer, _trampoline, _size);
            }

            Kernel32.VirtualFree(_trampoline, 0, FreeType.Release);
            _handle.Free();
        }
    }
}
