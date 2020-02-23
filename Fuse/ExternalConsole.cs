using System;
using System.IO;
using Fuse.Unsafe.Win32;
using Microsoft.Win32.SafeHandles;

namespace Fuse
{
    public static class ExternalConsole
    {
        public static bool Allocate()
        {
            if (!Kernel32.AllocConsole())
            {
                return false;
            }

            var inputHandle = Kernel32.GetStdHandle(StdHandleType.Input);
            var inputStream = new FileStream(new SafeFileHandle(inputHandle, true), FileAccess.Read);
            Console.SetIn(new StreamReader(inputStream));

            var outputHandle = Kernel32.GetStdHandle(StdHandleType.Output);
            var outputStream = new FileStream(new SafeFileHandle(outputHandle, true), FileAccess.Write);
            Console.SetOut(new StreamWriter(outputStream) { AutoFlush = true });

            return true;
        }
    }
}
