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

        public static IntPtr AppendNop(this IntPtr pointer, int count) =>
            Enumerable
                .Repeat<byte>(0x90, count)
                .Aggregate(pointer, IntPtrExtensions.Append);
    }
}
