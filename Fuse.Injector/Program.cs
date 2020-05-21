using Fuse.Extensions;
using Fuse.Unsafe.Win32;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Windows.Forms;
using Process = System.Diagnostics.Process;

namespace Fuse.Injector
{
    public static class Program
    {
        private const string Dll = "Fuse.Loader.dll";

        private static Process FindOrCreateProcess(string application, IEnumerable<string> args, bool suspended)
        {
            var processes = Process.GetProcessesByName(application);

            if (processes.Length > 0)
            {
                return processes[0];
            }

            var startupInfo = new StartupInfo();
            var arguments = args.Select(arg => arg.Contains(" ") ? $"\"{arg}\"" : arg);

            Kernel32.CreateProcess(
                $"{application}.exe",
                $"{application}.exe {string.Join(" ", arguments)}",
                IntPtr.Zero,
                IntPtr.Zero,
                false,
                suspended
                    ? ProcessCreationFlags.CreateSuspended
                    : ProcessCreationFlags.None,
                IntPtr.Zero,
                null,
                ref startupInfo,
                out var processInformation
            );

            return Process.GetProcessById(processInformation.dwProcessId);
        }

        private static void HandleRootCommand(
            string application, 
            bool suspended,
            string function,
            IEnumerable<string> targetArgs
        ) {
            var process = FindOrCreateProcess(application, targetArgs, suspended);

            try
            {
                Injector.Inject(process, Dll);
                Injector.CallExport(process, Dll, function);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Fuse.Injector", MessageBoxButtons.OK, MessageBoxIcon.Error);
                process.Kill();
            }
            finally
            {
                Injector.ResumeProcess(process);
            }
        }

        public static void Main(string[] args)
        {
            var (injectorArgs, targetArgs) = args.Split("--");

            var rootCommand = new RootCommand
            {
                new Option<string>("--application"),
                new Option<bool>("--suspended"),
                new Option<string>("--function", getDefaultValue: () => "Load")
            };

            rootCommand.Handler = CommandHandler.Create<string, bool, string>(
                (application, suspended, function) =>
                    HandleRootCommand(application, suspended, function, targetArgs)
            );

            rootCommand.Invoke(injectorArgs.ToArray());
        }
    }
}
