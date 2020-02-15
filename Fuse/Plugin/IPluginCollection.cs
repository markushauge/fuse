using System.Collections.Generic;

namespace Fuse.Plugin
{
    public interface IPluginCollection
    {
        T? FindPlugin<T>() where T : class;
        IEnumerable<T> FindPlugins<T>() where T : class;
    }
}
