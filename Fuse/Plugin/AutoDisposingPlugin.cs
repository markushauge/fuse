using System;
using System.Collections.Generic;

namespace Fuse.Plugin
{
    public abstract class AutoDisposingPlugin : IPlugin
    {
        private readonly ICollection<IDisposable> _disposables = new List<IDisposable>();

        protected abstract void Configure(IPluginCollection plugins, ICollection<IDisposable> disposables);

        public void OnEnable(IPluginCollection plugins)
        {
            Configure(plugins, _disposables);
        }

        public void OnDisable(IPluginCollection plugins)
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}