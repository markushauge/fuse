using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Fuse.Extensions;
using Fuse.Unsafe.Win32;
using static Fuse.Unsafe.Win32.AllocationType;
using static Fuse.Unsafe.Win32.MemoryProtection;
using static Fuse.Unsafe.Win32.FreeType;

namespace Fuse.Injector
{
    public static class Injector
    {
        private static ProcessModule? GetModule(Process process, string dll) =>
            process
                .Modules
                .Cast<ProcessModule>()
                .FirstOrDefault(module => Path.GetFileName(module.FileName) == dll);

        private static void IterateProcessThreads(Process process, Action<IntPtr> action)
        {
            foreach (ProcessThread processThread in process.Threads.Cast<ProcessThread>())
            {
                var threadHandle = Kernel32.OpenThread(2, false, processThread.Id);
                action(threadHandle);
                Kernel32.CloseHandle(threadHandle);
            }
        }

        public static void Inject(Process process, string dll)
        {
            var moduleHandle = Kernel32
                .LoadLibrary("kernel32.dll")
                .ThrowIfZero("Failed to load kernel32.dll");

            var procAddress = Kernel32
                .GetProcAddress(moduleHandle, "LoadLibraryA")
                .ThrowIfZero("Failed to find LoadLibraryA in kernel32.dll");

            var parameterBytes = Encoding.ASCII.GetBytes(dll + '\0');

            var remoteParameter = Kernel32
                .VirtualAllocEx(process.Handle, IntPtr.Zero, (uint)parameterBytes.Length, Commit | Reserve, ReadWrite)
                .ThrowIfZero("Failed to allocate memory in remote process");

            Kernel32
                .WriteProcessMemory(process.Handle, remoteParameter, parameterBytes, (uint)parameterBytes.Length, IntPtr.Zero)
                .ThrowIfFalse("Failed to write LoadLibraryA parameter to process");

            var remoteThread = Kernel32
                .CreateRemoteThread(process.Handle, IntPtr.Zero, IntPtr.Zero, procAddress, remoteParameter, 0, IntPtr.Zero)
                .ThrowIfZero("Failed to create remote thread");

            Kernel32.WaitForSingleObject(remoteThread, -1);
            Kernel32.VirtualFreeEx(process.Handle, remoteParameter, (uint)parameterBytes.Length, Release);
            Kernel32.CloseHandle(remoteThread);
            Kernel32.FreeLibrary(moduleHandle);
        }

        public static void CallExport(Process process, string dll, string function)
        {
            var moduleHandle = Kernel32
                .LoadLibrary(dll)
                .ThrowIfZero("Failed to get module handle");

            var procAddress = Kernel32
                .GetProcAddress(moduleHandle, function)
                .ThrowIfZero($"Failed to find {function}");

            var processModule = GetModule(process, dll)
                .ThrowIfNull("Failed to find module handle of injected dll");

            var startAddress = processModule.BaseAddress.ToInt64() + procAddress.ToInt64() - moduleHandle.ToInt64();

            var remoteThread = Kernel32
                .CreateRemoteThread(process.Handle, IntPtr.Zero, IntPtr.Zero, new IntPtr(startAddress), IntPtr.Zero, 0, IntPtr.Zero)
                .ThrowIfZero("Failed to create remote thread");

            Kernel32.WaitForSingleObject(remoteThread, -1);
            Kernel32.CloseHandle(remoteThread);
        }

        public static void SuspendProcess(Process process)
        {
            IterateProcessThreads(process, threadHandle =>
            {
                Kernel32.SuspendThread(threadHandle);
            });
        }

        public static void ResumeProcess(Process process)
        {
            IterateProcessThreads(process, threadHandle =>
            {
                Kernel32.ResumeThread(threadHandle);
            });
        }
    }
}
