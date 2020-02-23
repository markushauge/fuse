using System;

namespace Fuse.Unsafe
{
    public class DisposableBlock : IDisposable
    {
        private readonly Action _disposer;

        public DisposableBlock(Func<Action> createDisposer) => _disposer = createDisposer();
        public void Dispose() => _disposer();
    }
}