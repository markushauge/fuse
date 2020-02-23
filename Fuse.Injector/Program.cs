using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Fuse.Injector
{
    public static class Program
    {
        private const string Dll = "Fuse.Loader.dll";

        private static Process FindOrCreateProcess(string name, IEnumerable<string> args)
        {
            var processes = Process.GetProcessesByName(name);

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

            return Process.Start($"{name}.exe", string.Join(" ", arguments));
        }

        public static void Main(string[] args)
        {
            var process = FindOrCreateProcess(args.First(), args.Skip(1));

            try
            {
                Injector.Inject(process, Dll);
                Injector.SuspendProcess(process);
                Injector.CallExport(process, Dll, "Load");
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
    }
}
