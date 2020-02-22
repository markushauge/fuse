using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Fuse.Plugin
{
    internal class Proxy : MarshalByRefObject
    {
        public static Proxy Create(AppDomain domain)
        {
            AssemblyResolver.Initialize(domain);

            return (Proxy)domain.CreateInstanceAndUnwrap(
                typeof(Proxy).Assembly.FullName,
                typeof(Proxy).FullName ?? throw new InvalidOperationException()
            );
        }

        private readonly List<IPlugin> _plugins = new List<IPlugin>();

        private void LoadPlugin(string file)
        {
            var assembly = Assembly.LoadFile(Path.GetFullPath(file));
            var type = assembly
                .GetTypes()
                .First(x => typeof(IPlugin).IsAssignableFrom(x));

            _plugins.Add((IPlugin)Activator.CreateInstance(type));
        }

        public void LoadPlugins(string directory)
        {
            foreach (var path in Directory.GetFiles(directory))
            {
                var file = Path.GetFileName(path);

                if (file.StartsWith("Fuse.Plugins") && file.EndsWith(".dll"))
                {
                    LoadPlugin(path);
                }
            }
        }

        public void EnablePlugins()
        {
            foreach (var plugin in _plugins)
            {
                var pluginName = plugin.GetType().Name;

                try
                {
                    plugin.OnEnable(_plugins);
                    Console.WriteLine($"[Proxy] Enabled {pluginName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Proxy] Failed to enable {pluginName}:\n{ex}");
                }
            }
        }

        public void DisablePlugins()
        {
            foreach (var plugin in _plugins)
            {
                var pluginName = plugin.GetType().Name;

                try
                {
                    plugin.OnEnable(_plugins);
                    Console.WriteLine($"[Proxy] Enabled {pluginName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Proxy] Failed to disable {pluginName}:\n{ex}");
                }
            }
        }
    }
}
