using System.Collections.Generic;
using System.Linq;

namespace Fuse.Plugin
{
    public static class PluginExtensions
    {
        public static T? FindPlugin<T>(this IEnumerable<IPlugin> plugins) where T : class =>
            plugins.FindPlugins<T>().FirstOrDefault();

        public static IEnumerable<T> FindPlugins<T>(this IEnumerable<IPlugin> plugins) where T : class =>
            plugins.OfType<T>();
    }
}
