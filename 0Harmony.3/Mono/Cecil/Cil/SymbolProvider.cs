using System;
using System.IO;
using System.Reflection;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F8 RID: 504
	internal static class SymbolProvider
	{
		// Token: 0x06000F8F RID: 3983 RVA: 0x000350E4 File Offset: 0x000332E4
		private static AssemblyName GetSymbolAssemblyName(SymbolKind kind)
		{
			if (kind == SymbolKind.PortablePdb)
			{
				throw new ArgumentException();
			}
			string symbolNamespace = SymbolProvider.GetSymbolNamespace(kind);
			AssemblyName name = typeof(SymbolProvider).Assembly.GetName();
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = name.Name + "." + symbolNamespace;
			assemblyName.Version = name.Version;
			assemblyName.CultureInfo = name.CultureInfo;
			assemblyName.SetPublicKeyToken(name.GetPublicKeyToken());
			return assemblyName;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00035158 File Offset: 0x00033358
		private static Type GetSymbolType(SymbolKind kind, string fullname)
		{
			Type type = Type.GetType(fullname);
			if (type != null)
			{
				return type;
			}
			AssemblyName symbolAssemblyName = SymbolProvider.GetSymbolAssemblyName(kind);
			type = Type.GetType(fullname + ", " + symbolAssemblyName.FullName);
			if (type != null)
			{
				return type;
			}
			try
			{
				Assembly assembly = Assembly.Load(symbolAssemblyName);
				if (assembly != null)
				{
					return assembly.GetType(fullname);
				}
			}
			catch (FileNotFoundException)
			{
			}
			catch (FileLoadException)
			{
			}
			return null;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x000351E4 File Offset: 0x000333E4
		public static ISymbolReaderProvider GetReaderProvider(SymbolKind kind)
		{
			if (kind == SymbolKind.PortablePdb)
			{
				return new PortablePdbReaderProvider();
			}
			if (kind == SymbolKind.EmbeddedPortablePdb)
			{
				return new EmbeddedPortablePdbReaderProvider();
			}
			string symbolTypeName = SymbolProvider.GetSymbolTypeName(kind, "ReaderProvider");
			Type symbolType = SymbolProvider.GetSymbolType(kind, symbolTypeName);
			if (symbolType == null)
			{
				throw new TypeLoadException("Could not find symbol provider type " + symbolTypeName);
			}
			return (ISymbolReaderProvider)Activator.CreateInstance(symbolType);
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0003523C File Offset: 0x0003343C
		private static string GetSymbolTypeName(SymbolKind kind, string name)
		{
			return string.Concat(new string[]
			{
				"Mono.Cecil.",
				SymbolProvider.GetSymbolNamespace(kind),
				".",
				kind.ToString(),
				name
			});
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00035276 File Offset: 0x00033476
		private static string GetSymbolNamespace(SymbolKind kind)
		{
			if (kind == SymbolKind.PortablePdb || kind == SymbolKind.EmbeddedPortablePdb)
			{
				return "Cil";
			}
			if (kind == SymbolKind.NativePdb)
			{
				return "Pdb";
			}
			if (kind == SymbolKind.Mdb)
			{
				return "Mdb";
			}
			throw new ArgumentException();
		}
	}
}
