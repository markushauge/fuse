using System;
using System.Linq;

namespace Fuse.Unsafe
{
    public static class AssemblyExtensions
    {
        public static IntPtr AppendJmp(this IntPtr pointer, IntPtr target) =>
            pointer
                .Append<byte>(0x48, 0xB8).Append(target) // mov rax, {target}
                .Append<byte>(0xFF, 0xE0);               // jmp rax

        public static IntPtr AppendJmp2(this IntPtr pointer, IntPtr target) =>
            pointer
                .Append<byte>(0xFF, 0x25, 0x00, 0x00, 0x00, 0x00) // jmp qword ptr [rip]
                .Append(target);                                  // {target}

        public static IntPtr AppendNop(this IntPtr pointer, int count) =>
            Enumerable
                .Repeat<byte>(0x90, count)
                .Aggregate(pointer, IntPtrExtensions.Append);
    }
}
