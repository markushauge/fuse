using System;
using System.Runtime.InteropServices;
using Fuse.Plugin;

namespace Fuse
{
    // ReSharper disable once UnusedMember.Global
    public static class Exports
    {
        private static readonly PluginManager PluginManager = new PluginManager("Plugins");

        [DllExport]
        // ReSharper disable once UnusedMember.Global
        public static void Load()
        {
            ExternalConsole.Allocate();

            try
            {
                PluginManager.LoadPlugins();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        [DllExport]
        // ReSharper disable once UnusedMember.Global
        public static void Unload()
        {
            try
            {
                PluginManager.UnloadPlugins();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
