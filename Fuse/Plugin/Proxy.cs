using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Fuse.Plugin
{
    internal class Proxy : MarshalByRefObject, IPluginCollection
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
            foreach (var subdirectory in Directory.GetDirectories(directory))
            {
                var file = Path.Combine(subdirectory, $"{Path.GetFileName(subdirectory)}.dll");

                if (!File.Exists(file))
                {
                    throw new Exception("Directory does not contain a plugin with matching name");
                }

                LoadPlugin(file);
            }

            foreach (var file in Directory.GetFiles(directory))
            {
                if (file.EndsWith(".dll"))
                {
                    LoadPlugin(file);
                }
            }
        }

        public void EnablePlugins() => _plugins.ForEach(plugin => plugin.OnEnable(this));
        public void DisablePlugins() => _plugins.ForEach(plugin => plugin.OnDisable(this));

        public T? FindPlugin<T>() where T : class => FindPlugins<T>().FirstOrDefault();
        public IEnumerable<T> FindPlugins<T>() where T : class => _plugins.OfType<T>();
    }
}
