using System;
using System.IO;
using System.Reflection;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002EE RID: 750
	internal static class SymbolProvider
	{
		// Token: 0x060012FF RID: 4863 RVA: 0x0003D030 File Offset: 0x0003B230
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

		// Token: 0x06001300 RID: 4864 RVA: 0x0003D0A4 File Offset: 0x0003B2A4
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

		// Token: 0x06001301 RID: 4865 RVA: 0x0003D130 File Offset: 0x0003B330
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

		// Token: 0x06001302 RID: 4866 RVA: 0x0003D188 File Offset: 0x0003B388
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

		// Token: 0x06001303 RID: 4867 RVA: 0x0003D1C2 File Offset: 0x0003B3C2
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
