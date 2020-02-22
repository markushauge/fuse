using System;
using Fuse.Native.Win32;

namespace Fuse.Native
{
    internal class MemoryProtectionBlock : IDisposable
    {
        private readonly IntPtr _pointer;
        private readonly uint _size;
        private readonly MemoryProtection _oldProtection;

        public MemoryProtectionBlock(IntPtr pointer, uint size, MemoryProtection newProtection)
        {
            _pointer = pointer;
            _size = size;
            Protect(newProtection, out _oldProtection);
        }

        private void Protect(MemoryProtection newProtection, out MemoryProtection oldProtection) =>
            Kernel32.VirtualProtect(_pointer, _size, newProtection, out oldProtection);

        public void Dispose() =>
            Protect(_oldProtection, out _);
    }
}