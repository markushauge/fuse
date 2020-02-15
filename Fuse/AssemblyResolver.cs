using System;
using System.IO;
using System.Reflection;

namespace Fuse
{
    public static class AssemblyResolver
    {
        private static Assembly? OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = args.Name.Substring(0, args.Name.IndexOf(",", StringComparison.Ordinal));

            if (name.EndsWith(".resources"))
            {
                return null;
            }

            var codeBase = args.RequestingAssembly?.CodeBase ?? throw new InvalidOperationException();
            var path = Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
            var directory = Path.GetDirectoryName(path) ?? throw new InvalidOperationException();

            return Assembly.LoadFile(Path.Combine(directory, $"{name}.dll"));
        }

        public static void Initialize(AppDomain domain)
        {
            domain.AssemblyResolve += OnAssemblyResolve;
        }
    }
}
