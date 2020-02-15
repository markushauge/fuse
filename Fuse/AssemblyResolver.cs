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
            var uri = new UriBuilder(args.RequestingAssembly.CodeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var directory = Path.GetDirectoryName(path) ?? throw new InvalidOperationException();

            if (name.EndsWith(".resources"))
            {
                return null;
            }

            return Assembly.LoadFile(Path.Combine(directory, $"{name}.dll"));
        }

        public static void Initialize(AppDomain domain)
        {
            domain.AssemblyResolve += OnAssemblyResolve;
        }
    }
}
