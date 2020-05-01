using System;
using Fuse.Plugin;

namespace Fuse
{
    public static class Entry
    {
        private static readonly PluginManager PluginManager = new PluginManager("Plugins");

        public static void Load()
        {
            ExternalConsole.Allocate();

            try
            {
                PluginManager.LoadPlugins();
                PluginManager.EnablePlugins();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void Unload()
        {
            try
            {
                PluginManager.DisablePlugins();
                PluginManager.UnloadPlugins();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
