using System.Collections.Generic;

namespace Fuse.Plugin
{
    public interface IPlugin
    {
        void OnEnable(IReadOnlyCollection<IPlugin> plugins);
        void OnDisable(IReadOnlyCollection<IPlugin> plugins);
    }
}
