using System.Runtime.InteropServices;

namespace Fuse.Loader
{
    public class Exports
    {
        [DllExport]
        public static void Load()
        {
            Entry.Load();
        }

        [DllExport]
        public static void Unload()
        {
            Entry.Unload();
        }
    }
}
