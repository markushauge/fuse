using System;

namespace Fuse.Unsafe.Win32
{
    [Flags]
    public enum ProcessCreationFlags
    {
        None = 0,
        CreateSuspended = 0x00000004
    }
}
