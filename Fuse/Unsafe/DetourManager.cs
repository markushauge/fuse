using System;
using System.Collections.Generic;

namespace Fuse.Unsafe
{
    public class DetourManager : IDisposable
    {
        public ICollection<IDetour> Detours { get; } = new List<IDetour>();

        public void Dispose()
        {
            foreach (var detour in Detours)
            {
                detour.Dispose();
            }
        }
    }
}
