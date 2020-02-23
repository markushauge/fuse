using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Fuse.Extensions;

namespace Fuse.Unsafe
{
    public class PatternScanner
    {
        private struct ByteMask
        {
            private static ByteMask None => new ByteMask(0x00, 0x00);
            private static ByteMask Some(byte @byte) => new ByteMask(@byte, 0xFF);

            private static ByteMask From(string part) =>
                part == "??" ? None : Some(byte.Parse(part, NumberStyles.HexNumber));

            public static IEnumerable<ByteMask> FromPattern(string pattern) =>
                pattern.Replace(" ", "").Chunk(2).Select(From);

            public readonly byte Byte;
            public readonly byte Mask;

            private ByteMask(byte @byte, byte mask)
            {
                Byte = @byte;
                Mask = mask;
            }

            public void Deconstruct(out byte @byte, out byte mask)
            {
                @byte = Byte;
                mask = Mask;
            }
        }

        private static unsafe bool Compare(byte* buffer, IEnumerable<ByteMask> byteMasks)
        {
            foreach (var (i, (@byte, mask)) in byteMasks.Enumerate())
            {
                if ((buffer[i] & mask) != @byte)
                {
                    return false;
                }
            }

            return true;
        }

        public IntPtr Begin { get; }
        public IntPtr End { get; }

        public PatternScanner(ProcessModule module) :
            this(module.BaseAddress, module.BaseAddress + module.ModuleMemorySize) { }

        public PatternScanner(IntPtr begin, IntPtr end)
        {
            Begin = begin;
            End = end;
        }

        public unsafe IntPtr Scan(string pattern)
        {
            var byteMasks = ByteMask.FromPattern(pattern).ToArray();
            var begin = (byte*)Begin.ToPointer();
            var end = (byte*)End.ToPointer();

            for (var buffer = begin; buffer < end; buffer++)
            {
                if (Compare(buffer, byteMasks))
                {
                    return new IntPtr(buffer);
                }
            }

            return IntPtr.Zero;
        }

        public IntPtr Scan(string pattern, int offset) => Scan(pattern) + offset;
    }
}
