using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Fuse.Native
{
    public class PatternScanner
    {
        public IntPtr Begin { get; }
        public IntPtr End { get; }

        public PatternScanner(ProcessModule module) : this(module.BaseAddress, module.BaseAddress + module.ModuleMemorySize) { }

        public PatternScanner(IntPtr begin, IntPtr end)
        {
            Begin = begin;
            End = end;
        }

        private unsafe bool Compare(byte* pBuffer, byte* pBytes, byte* pMask, long length)
        {
            for (var i = 0; i < length; i++)
            {
                if ((pBuffer[i] & pMask[i]) != pBytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        public unsafe IntPtr Scan(string pattern)
        {
            var parts = pattern.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var bytes = parts
                .Select(part => part == "??" ? (byte)0x00 : byte.Parse(part, NumberStyles.HexNumber))
                .ToArray();

            var mask = parts
                .Select(part => part == "??" ? (byte)0x00 : (byte)0xFF)
                .ToArray();

            var begin = (byte*)Begin.ToPointer();
            var end = (byte*)End.ToPointer();

            fixed (byte* pBytes = bytes, pMask = mask)
            {
                for (var pBuffer = begin; pBuffer < end; pBuffer++)
                {
                    if (Compare(pBuffer, pBytes, pMask, bytes.Length))
                    {
                        return new IntPtr(pBuffer);
                    }
                }
            }

            return IntPtr.Zero;
        }

        public IntPtr Scan(string pattern, int offset) => Scan(pattern) + offset;
    }
}
