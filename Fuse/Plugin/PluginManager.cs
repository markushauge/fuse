using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Fuse.Plugin
{
    public class PluginManager
    {
        private static Assembly? OnResolving(AssemblyLoadContext context, AssemblyName name)
        {
            var fuse = Assembly.GetExecutingAssembly();

            if (fuse.GetName().Name == name.Name)
            {
                return fuse;
            }

            return null;
        }

        private static AssemblyLoadContext CreateContext()
        {
            var context = new AssemblyLoadContext(null, true);
            context.Resolving += OnResolving;
            return context;
        }

        private readonly List<IPlugin> _plugins = new List<IPlugin>();
        private readonly string _directory;
        private AssemblyLoadContext _context = CreateContext();

        public PluginManager(string directory)
        {
            _directory = directory;
        }

        private void LoadPlugin(string file)
        {
            var type = _context
                .LoadFromAssemblyPath(Path.GetFullPath(file))
                .GetTypes()
                .First(x => typeof(IPlugin).IsAssignableFrom(x));

            _plugins.Add((IPlugin)Activator.CreateInstance(type)!);
        }

        public void LoadPlugins()
        {
            foreach (var path in Directory.GetFiles(_directory))
            {
                var file = Path.GetFileName(path);

                if (file.StartsWith("Fuse.Plugins") && file.EndsWith(".dll"))
                {
                    LoadPlugin(path);
                }
            }
        }

        public void UnloadPlugins()
        {
            _context.Unload();
            _context = CreateContext();
        }

        public void EnablePlugins()
        {
            foreach (var plugin in _plugins)
            {
                var pluginName = plugin.GetType().Name;

                try
                {
                    plugin.OnEnable(_plugins);
                    Console.WriteLine($"[PluginManager] Enabled {pluginName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PluginManager] Failed to enable {pluginName}:\n{ex}");
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
                    plugin.OnDisable(_plugins);
                    Console.WriteLine($"[PluginManager] Disabled {pluginName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PluginManager] Failed to disable {pluginName}:\n{ex}");
                }
            }
        }
    }
}
