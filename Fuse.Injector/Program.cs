using System;
using System.Diagnostics;
using System.Linq;

namespace Fuse.Injector
{
    public static class Program
    {
        private const string DLL = "Fuse.dll";
        private const string TARGET = "Gw2-64";

        private static Process FindOrCreateProcess(string name, string[] args)
        {
            Process[] processes = Process.GetProcessesByName(name);

            if (processes.Length > 0)
            {
                return processes[0];
            }

            var arguments = args.Select(arg =>
            {
                if (!arg.Contains(" "))
                {
                    return arg;
                }

                return '"' + arg + '"';
            });

            return Process.Start(name + ".exe", string.Join(" ", arguments));
        }

        public static void Main(string[] args)
        {
            Process process = FindOrCreateProcess(TARGET, args);

            try
            {
                Injector.Inject(process, DLL);
                Injector.SuspendProcess(process);
                Injector.CallExport(process, DLL, "Load");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Injector.ResumeProcess(process);
            }
        }
    }
}
