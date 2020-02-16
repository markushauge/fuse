using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Fuse.Extensions;

namespace Fuse.Native
{
    public class PatternScanner
    {
        private struct ByteMask
        {
            private static ByteMask None => new ByteMask(0x00, 0x00);
            private static ByteMask Some(byte @byte) => new ByteMask(@byte, 0xFF);

            public static ByteMask From(string part) =>
                part == "??" ? None : Some(byte.Parse(part, NumberStyles.HexNumber));

            public readonly byte Byte;
            public readonly byte Mask;

            private ByteMask(byte @byte, byte mask)
            {
                Byte = @byte;
                Mask = mask;
            }
        }

        private static unsafe bool Compare(byte* buffer, IList<ByteMask> byteMasks, long length)
        {
            for (var i = 0; i < length; i++)
            {
                if ((buffer[i] & byteMasks[i].Mask) != byteMasks[i].Byte)
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
            var parts = pattern.Replace(" ", "").Chunk(2);
            var byteMasks = parts.Select(ByteMask.From).ToArray();
            var begin = (byte*)Begin.ToPointer();
            var end = (byte*)End.ToPointer();

            for (var buffer = begin; buffer < end; buffer++)
            {
                if (Compare(buffer, byteMasks, byteMasks.Length))
                {
                    return new IntPtr(buffer);
                }
            }

            return IntPtr.Zero;
        }

        public IntPtr Scan(string pattern, int offset) => Scan(pattern) + offset;
    }
}
