using System;

namespace Fuse.Plugin
{
    public class PluginManager
    {
        private static AppDomain CreateDomain() => AppDomain.CreateDomain("PluginDomain");

        private readonly string _directory;
        private AppDomain _domain;
        private Proxy _proxy;

        public PluginManager(string directory)
        {
            _directory = directory;
            _domain = CreateDomain();
            _proxy = Proxy.Create(_domain);
        }

        public void LoadPlugins()
        {
            _proxy.LoadPlugins(_directory);
            _proxy.EnablePlugins();
        }

        public void UnloadPlugins()
        {
            _proxy.DisablePlugins();
            AppDomain.Unload(_domain);
            _domain = CreateDomain();
            _proxy = Proxy.Create(_domain);
        }

        public void ReloadPlugins()
        {
            UnloadPlugins();
            LoadPlugins();
        }
    }
}
