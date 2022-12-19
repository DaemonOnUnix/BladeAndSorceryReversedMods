using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000100 RID: 256
	internal abstract class BaseAssemblyResolver : IAssemblyResolver, IDisposable
	{
		// Token: 0x060006C7 RID: 1735 RVA: 0x0001F734 File Offset: 0x0001D934
		public void AddSearchDirectory(string directory)
		{
			this.directories.Add(directory);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001F742 File Offset: 0x0001D942
		public void RemoveSearchDirectory(string directory)
		{
			this.directories.Remove(directory);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001F754 File Offset: 0x0001D954
		public string[] GetSearchDirectories()
		{
			string[] array = new string[this.directories.size];
			Array.Copy(this.directories.items, array, array.Length);
			return array;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060006CA RID: 1738 RVA: 0x0001F788 File Offset: 0x0001D988
		// (remove) Token: 0x060006CB RID: 1739 RVA: 0x0001F7C0 File Offset: 0x0001D9C0
		public event AssemblyResolveEventHandler ResolveFailure;

		// Token: 0x060006CC RID: 1740 RVA: 0x0001F7F5 File Offset: 0x0001D9F5
		protected BaseAssemblyResolver()
		{
			this.directories = new Collection<string>(2) { ".", "bin" };
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001F81F File Offset: 0x0001DA1F
		private AssemblyDefinition GetAssembly(string file, ReaderParameters parameters)
		{
			if (parameters.AssemblyResolver == null)
			{
				parameters.AssemblyResolver = this;
			}
			return ModuleDefinition.ReadModule(file, parameters).Assembly;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001F83C File Offset: 0x0001DA3C
		public virtual AssemblyDefinition Resolve(AssemblyNameReference name)
		{
			return this.Resolve(name, new ReaderParameters());
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001F84C File Offset: 0x0001DA4C
		public virtual AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
		{
			Mixin.CheckName(name);
			Mixin.CheckParameters(parameters);
			AssemblyDefinition assemblyDefinition = this.SearchDirectory(name, this.directories, parameters);
			if (assemblyDefinition != null)
			{
				return assemblyDefinition;
			}
			if (name.IsRetargetable)
			{
				name = new AssemblyNameReference(name.Name, Mixin.ZeroVersion)
				{
					PublicKeyToken = Empty<byte>.Array
				};
			}
			string directoryName = Path.GetDirectoryName(typeof(object).Module.FullyQualifiedName);
			string[] array;
			if (!BaseAssemblyResolver.on_mono)
			{
				(array = new string[1])[0] = directoryName;
			}
			else
			{
				string[] array2 = new string[2];
				array2[0] = directoryName;
				array = array2;
				array2[1] = Path.Combine(directoryName, "Facades");
			}
			string[] array3 = array;
			if (BaseAssemblyResolver.IsZero(name.Version))
			{
				assemblyDefinition = this.SearchDirectory(name, array3, parameters);
				if (assemblyDefinition != null)
				{
					return assemblyDefinition;
				}
			}
			if (name.Name == "mscorlib")
			{
				assemblyDefinition = this.GetCorlib(name, parameters);
				if (assemblyDefinition != null)
				{
					return assemblyDefinition;
				}
			}
			assemblyDefinition = this.GetAssemblyInGac(name, parameters);
			if (assemblyDefinition != null)
			{
				return assemblyDefinition;
			}
			assemblyDefinition = this.SearchDirectory(name, array3, parameters);
			if (assemblyDefinition != null)
			{
				return assemblyDefinition;
			}
			if (this.ResolveFailure != null)
			{
				assemblyDefinition = this.ResolveFailure(this, name);
				if (assemblyDefinition != null)
				{
					return assemblyDefinition;
				}
			}
			throw new AssemblyResolutionException(name);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001F960 File Offset: 0x0001DB60
		protected virtual AssemblyDefinition SearchDirectory(AssemblyNameReference name, IEnumerable<string> directories, ReaderParameters parameters)
		{
			string[] array2;
			if (!name.IsWindowsRuntime)
			{
				string[] array = new string[2];
				array[0] = ".exe";
				array2 = array;
				array[1] = ".dll";
			}
			else
			{
				string[] array3 = new string[2];
				array3[0] = ".winmd";
				array2 = array3;
				array3[1] = ".dll";
			}
			string[] array4 = array2;
			foreach (string text in directories)
			{
				foreach (string text2 in array4)
				{
					string text3 = Path.Combine(text, name.Name + text2);
					if (File.Exists(text3))
					{
						try
						{
							return this.GetAssembly(text3, parameters);
						}
						catch (BadImageFormatException)
						{
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001FA34 File Offset: 0x0001DC34
		private static bool IsZero(Version version)
		{
			return version.Major == 0 && version.Minor == 0 && version.Build == 0 && version.Revision == 0;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001FA5C File Offset: 0x0001DC5C
		private AssemblyDefinition GetCorlib(AssemblyNameReference reference, ReaderParameters parameters)
		{
			Version version = reference.Version;
			if (typeof(object).Assembly.GetName().Version == version || BaseAssemblyResolver.IsZero(version))
			{
				return this.GetAssembly(typeof(object).Module.FullyQualifiedName, parameters);
			}
			string text = Directory.GetParent(Directory.GetParent(typeof(object).Module.FullyQualifiedName).FullName).FullName;
			if (!BaseAssemblyResolver.on_mono)
			{
				switch (version.Major)
				{
				case 1:
					if (version.MajorRevision == 3300)
					{
						text = Path.Combine(text, "v1.0.3705");
						goto IL_187;
					}
					text = Path.Combine(text, "v1.1.4322");
					goto IL_187;
				case 2:
					text = Path.Combine(text, "v2.0.50727");
					goto IL_187;
				case 4:
					text = Path.Combine(text, "v4.0.30319");
					goto IL_187;
				}
				string text2 = "Version not supported: ";
				Version version2 = version;
				throw new NotSupportedException(text2 + ((version2 != null) ? version2.ToString() : null));
			}
			if (version.Major == 1)
			{
				text = Path.Combine(text, "1.0");
			}
			else if (version.Major == 2)
			{
				if (version.MajorRevision == 5)
				{
					text = Path.Combine(text, "2.1");
				}
				else
				{
					text = Path.Combine(text, "2.0");
				}
			}
			else
			{
				if (version.Major != 4)
				{
					string text3 = "Version not supported: ";
					Version version3 = version;
					throw new NotSupportedException(text3 + ((version3 != null) ? version3.ToString() : null));
				}
				text = Path.Combine(text, "4.0");
			}
			IL_187:
			string text4 = Path.Combine(text, "mscorlib.dll");
			if (File.Exists(text4))
			{
				return this.GetAssembly(text4, parameters);
			}
			if (BaseAssemblyResolver.on_mono && Directory.Exists(text + "-api"))
			{
				text4 = Path.Combine(text + "-api", "mscorlib.dll");
				if (File.Exists(text4))
				{
					return this.GetAssembly(text4, parameters);
				}
			}
			return null;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001FC50 File Offset: 0x0001DE50
		private static Collection<string> GetGacPaths()
		{
			if (BaseAssemblyResolver.on_mono)
			{
				return BaseAssemblyResolver.GetDefaultMonoGacPaths();
			}
			Collection<string> collection = new Collection<string>(2);
			string environmentVariable = Environment.GetEnvironmentVariable("WINDIR");
			if (environmentVariable == null)
			{
				return collection;
			}
			collection.Add(Path.Combine(environmentVariable, "assembly"));
			collection.Add(Path.Combine(environmentVariable, Path.Combine("Microsoft.NET", "assembly")));
			return collection;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001FCB0 File Offset: 0x0001DEB0
		private static Collection<string> GetDefaultMonoGacPaths()
		{
			Collection<string> collection = new Collection<string>(1);
			string currentMonoGac = BaseAssemblyResolver.GetCurrentMonoGac();
			if (currentMonoGac != null)
			{
				collection.Add(currentMonoGac);
			}
			string environmentVariable = Environment.GetEnvironmentVariable("MONO_GAC_PREFIX");
			if (string.IsNullOrEmpty(environmentVariable))
			{
				return collection;
			}
			foreach (string text in environmentVariable.Split(new char[] { Path.PathSeparator }))
			{
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = Path.Combine(Path.Combine(Path.Combine(text, "lib"), "mono"), "gac");
					if (Directory.Exists(text2) && !collection.Contains(currentMonoGac))
					{
						collection.Add(text2);
					}
				}
			}
			return collection;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001FD5C File Offset: 0x0001DF5C
		private static string GetCurrentMonoGac()
		{
			return Path.Combine(Directory.GetParent(Path.GetDirectoryName(typeof(object).Module.FullyQualifiedName)).FullName, "gac");
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001FD8B File Offset: 0x0001DF8B
		private AssemblyDefinition GetAssemblyInGac(AssemblyNameReference reference, ReaderParameters parameters)
		{
			if (reference.PublicKeyToken == null || reference.PublicKeyToken.Length == 0)
			{
				return null;
			}
			if (this.gac_paths == null)
			{
				this.gac_paths = BaseAssemblyResolver.GetGacPaths();
			}
			if (BaseAssemblyResolver.on_mono)
			{
				return this.GetAssemblyInMonoGac(reference, parameters);
			}
			return this.GetAssemblyInNetGac(reference, parameters);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001FDCC File Offset: 0x0001DFCC
		private AssemblyDefinition GetAssemblyInMonoGac(AssemblyNameReference reference, ReaderParameters parameters)
		{
			for (int i = 0; i < this.gac_paths.Count; i++)
			{
				string text = this.gac_paths[i];
				string assemblyFile = BaseAssemblyResolver.GetAssemblyFile(reference, string.Empty, text);
				if (File.Exists(assemblyFile))
				{
					return this.GetAssembly(assemblyFile, parameters);
				}
			}
			return null;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001FE1C File Offset: 0x0001E01C
		private AssemblyDefinition GetAssemblyInNetGac(AssemblyNameReference reference, ReaderParameters parameters)
		{
			string[] array = new string[] { "GAC_MSIL", "GAC_32", "GAC_64", "GAC" };
			string[] array2 = new string[]
			{
				string.Empty,
				"v4.0_"
			};
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < array.Length; j++)
				{
					string text = Path.Combine(this.gac_paths[i], array[j]);
					string assemblyFile = BaseAssemblyResolver.GetAssemblyFile(reference, array2[i], text);
					if (Directory.Exists(text) && File.Exists(assemblyFile))
					{
						return this.GetAssembly(assemblyFile, parameters);
					}
				}
			}
			return null;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001FEC4 File Offset: 0x0001E0C4
		private static string GetAssemblyFile(AssemblyNameReference reference, string prefix, string gac)
		{
			StringBuilder stringBuilder = new StringBuilder().Append(prefix).Append(reference.Version).Append("__");
			for (int i = 0; i < reference.PublicKeyToken.Length; i++)
			{
				stringBuilder.Append(reference.PublicKeyToken[i].ToString("x2"));
			}
			return Path.Combine(Path.Combine(Path.Combine(gac, reference.Name), stringBuilder.ToString()), reference.Name + ".dll");
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001FF4E File Offset: 0x0001E14E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00012279 File Offset: 0x00010479
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x040002A0 RID: 672
		private static readonly bool on_mono = Type.GetType("Mono.Runtime") != null;

		// Token: 0x040002A1 RID: 673
		private readonly Collection<string> directories;

		// Token: 0x040002A2 RID: 674
		private Collection<string> gac_paths;
	}
}
