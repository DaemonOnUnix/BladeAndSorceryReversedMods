using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001F2 RID: 498
	internal abstract class BaseAssemblyResolver : IAssemblyResolver, IDisposable
	{
		// Token: 0x060009FF RID: 2559 RVA: 0x000255D4 File Offset: 0x000237D4
		public void AddSearchDirectory(string directory)
		{
			this.directories.Add(directory);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x000255E2 File Offset: 0x000237E2
		public void RemoveSearchDirectory(string directory)
		{
			this.directories.Remove(directory);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x000255F4 File Offset: 0x000237F4
		public string[] GetSearchDirectories()
		{
			string[] array = new string[this.directories.size];
			Array.Copy(this.directories.items, array, array.Length);
			return array;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000A02 RID: 2562 RVA: 0x00025628 File Offset: 0x00023828
		// (remove) Token: 0x06000A03 RID: 2563 RVA: 0x00025660 File Offset: 0x00023860
		public event AssemblyResolveEventHandler ResolveFailure;

		// Token: 0x06000A04 RID: 2564 RVA: 0x00025695 File Offset: 0x00023895
		protected BaseAssemblyResolver()
		{
			this.directories = new Collection<string>(2) { ".", "bin" };
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x000256BF File Offset: 0x000238BF
		private AssemblyDefinition GetAssembly(string file, ReaderParameters parameters)
		{
			if (parameters.AssemblyResolver == null)
			{
				parameters.AssemblyResolver = this;
			}
			return ModuleDefinition.ReadModule(file, parameters).Assembly;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x000256DC File Offset: 0x000238DC
		public virtual AssemblyDefinition Resolve(AssemblyNameReference name)
		{
			return this.Resolve(name, new ReaderParameters());
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x000256EC File Offset: 0x000238EC
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

		// Token: 0x06000A08 RID: 2568 RVA: 0x00025800 File Offset: 0x00023A00
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

		// Token: 0x06000A09 RID: 2569 RVA: 0x000258D4 File Offset: 0x00023AD4
		private static bool IsZero(Version version)
		{
			return version.Major == 0 && version.Minor == 0 && version.Build == 0 && version.Revision == 0;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x000258FC File Offset: 0x00023AFC
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

		// Token: 0x06000A0B RID: 2571 RVA: 0x00025AF0 File Offset: 0x00023CF0
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

		// Token: 0x06000A0C RID: 2572 RVA: 0x00025B50 File Offset: 0x00023D50
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

		// Token: 0x06000A0D RID: 2573 RVA: 0x00025BFC File Offset: 0x00023DFC
		private static string GetCurrentMonoGac()
		{
			return Path.Combine(Directory.GetParent(Path.GetDirectoryName(typeof(object).Module.FullyQualifiedName)).FullName, "gac");
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00025C2B File Offset: 0x00023E2B
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

		// Token: 0x06000A0F RID: 2575 RVA: 0x00025C6C File Offset: 0x00023E6C
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

		// Token: 0x06000A10 RID: 2576 RVA: 0x00025CBC File Offset: 0x00023EBC
		private AssemblyDefinition GetAssemblyInNetGac(AssemblyNameReference reference, ReaderParameters parameters)
		{
			string[] array = new string[] { "GAC_MSIL", "GAC_32", "GAC_64", "GAC" };
			string[] array2 = new string[]
			{
				string.Empty,
				"v4.0_"
			};
			for (int i = 0; i < this.gac_paths.Count; i++)
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

		// Token: 0x06000A11 RID: 2577 RVA: 0x00025D6C File Offset: 0x00023F6C
		private static string GetAssemblyFile(AssemblyNameReference reference, string prefix, string gac)
		{
			StringBuilder stringBuilder = new StringBuilder().Append(prefix).Append(reference.Version).Append("__");
			for (int i = 0; i < reference.PublicKeyToken.Length; i++)
			{
				stringBuilder.Append(reference.PublicKeyToken[i].ToString("x2"));
			}
			return Path.Combine(Path.Combine(Path.Combine(gac, reference.Name), stringBuilder.ToString()), reference.Name + ".dll");
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00025DF6 File Offset: 0x00023FF6
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00018105 File Offset: 0x00016305
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x040002D2 RID: 722
		private static readonly bool on_mono = Type.GetType("Mono.Runtime") != null;

		// Token: 0x040002D3 RID: 723
		private readonly Collection<string> directories;

		// Token: 0x040002D4 RID: 724
		private Collection<string> gac_paths;
	}
}
