using System;
using System.Runtime.InteropServices;
using Fuse.Http;

namespace Fuse
{
    public static class Exports
    {
        [DllExport]
        public static void Load()
        {
            AssemblyResolver.Initialize();
            ExternalConsole.Allocate();

            try
            {
                Server.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
