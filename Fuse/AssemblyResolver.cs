﻿using System;
using System.IO;
using System.Reflection;

namespace Fuse
{
    public static class AssemblyResolver
    {
        private static Assembly? OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = args.Name.Substring(0, args.Name.IndexOf(","));

            if (name.EndsWith(".resources"))
            {
                return null;
            }

            return Assembly.LoadFile(Path.GetFullPath($"{name}.dll"));
        }

        public static void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }
    }
}