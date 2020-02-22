using System;
using System.Collections.Generic;

namespace Fuse.Plugin
{
    public abstract class AutoDisposingPlugin : IPlugin
    {
        private readonly ICollection<IDisposable> _disposables = new List<IDisposable>();

        protected abstract void Configure(IReadOnlyCollection<IPlugin> plugins, ICollection<IDisposable> disposables);

        public void OnEnable(IReadOnlyCollection<IPlugin> plugins)
        {
            Configure(plugins, _disposables);
        }

        public void OnDisable(IReadOnlyCollection<IPlugin> plugins)
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}