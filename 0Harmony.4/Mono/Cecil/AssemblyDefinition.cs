using System;
using System.IO;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001AF RID: 431
	public sealed class AssemblyDefinition : ICustomAttributeProvider, IMetadataTokenProvider, ISecurityDeclarationProvider, IDisposable
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0001A64F File Offset: 0x0001884F
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x0001A657 File Offset: 0x00018857
		public AssemblyNameDefinition Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0001A660 File Offset: 0x00018860
		public string FullName
		{
			get
			{
				if (this.name == null)
				{
					return string.Empty;
				}
				return this.name.FullName;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0001A67B File Offset: 0x0001887B
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x00018105 File Offset: 0x00016305
		public MetadataToken MetadataToken
		{
			get
			{
				return new MetadataToken(TokenType.Assembly, 1);
			}
			set
			{
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0001A688 File Offset: 0x00018888
		public Collection<ModuleDefinition> Modules
		{
			get
			{
				if (this.modules != null)
				{
					return this.modules;
				}
				if (this.main_module.HasImage)
				{
					return this.main_module.Read<AssemblyDefinition, Collection<ModuleDefinition>>(ref this.modules, this, (AssemblyDefinition _, MetadataReader reader) => reader.ReadModules());
				}
				Interlocked.CompareExchange<Collection<ModuleDefinition>>(ref this.modules, new Collection<ModuleDefinition>(1) { this.main_module }, null);
				return this.modules;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0001A708 File Offset: 0x00018908
		public ModuleDefinition MainModule
		{
			get
			{
				return this.main_module;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0001A710 File Offset: 0x00018910
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x0001A71D File Offset: 0x0001891D
		public MethodDefinition EntryPoint
		{
			get
			{
				return this.main_module.EntryPoint;
			}
			set
			{
				this.main_module.EntryPoint = value;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0001A72B File Offset: 0x0001892B
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.main_module);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x0001A750 File Offset: 0x00018950
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.main_module);
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0001A76E File Offset: 0x0001896E
		public bool HasSecurityDeclarations
		{
			get
			{
				if (this.security_declarations != null)
				{
					return this.security_declarations.Count > 0;
				}
				return this.GetHasSecurityDeclarations(this.main_module);
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0001A793 File Offset: 0x00018993
		public Collection<SecurityDeclaration> SecurityDeclarations
		{
			get
			{
				return this.security_declarations ?? this.GetSecurityDeclarations(ref this.security_declarations, this.main_module);
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00002AED File Offset: 0x00000CED
		internal AssemblyDefinition()
		{
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001A7B4 File Offset: 0x000189B4
		public void Dispose()
		{
			if (this.modules == null)
			{
				this.main_module.Dispose();
				return;
			}
			Collection<ModuleDefinition> collection = this.Modules;
			for (int i = 0; i < collection.Count; i++)
			{
				collection[i].Dispose();
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001A7F9 File Offset: 0x000189F9
		public static AssemblyDefinition CreateAssembly(AssemblyNameDefinition assemblyName, string moduleName, ModuleKind kind)
		{
			return AssemblyDefinition.CreateAssembly(assemblyName, moduleName, new ModuleParameters
			{
				Kind = kind
			});
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0001A810 File Offset: 0x00018A10
		public static AssemblyDefinition CreateAssembly(AssemblyNameDefinition assemblyName, string moduleName, ModuleParameters parameters)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (moduleName == null)
			{
				throw new ArgumentNullException("moduleName");
			}
			Mixin.CheckParameters(parameters);
			if (parameters.Kind == ModuleKind.NetModule)
			{
				throw new ArgumentException("kind");
			}
			AssemblyDefinition assembly = ModuleDefinition.CreateModule(moduleName, parameters).Assembly;
			assembly.Name = assemblyName;
			return assembly;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001A866 File Offset: 0x00018A66
		public static AssemblyDefinition ReadAssembly(string fileName)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(fileName));
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001A873 File Offset: 0x00018A73
		public static AssemblyDefinition ReadAssembly(string fileName, ReaderParameters parameters)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(fileName, parameters));
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001A881 File Offset: 0x00018A81
		public static AssemblyDefinition ReadAssembly(Stream stream)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(stream));
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001A88E File Offset: 0x00018A8E
		public static AssemblyDefinition ReadAssembly(Stream stream, ReaderParameters parameters)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(stream, parameters));
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001A89C File Offset: 0x00018A9C
		private static AssemblyDefinition ReadAssembly(ModuleDefinition module)
		{
			AssemblyDefinition assembly = module.Assembly;
			if (assembly == null)
			{
				throw new ArgumentException();
			}
			return assembly;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001A8AD File Offset: 0x00018AAD
		public void Write(string fileName)
		{
			this.Write(fileName, new WriterParameters());
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001A8BB File Offset: 0x00018ABB
		public void Write(string fileName, WriterParameters parameters)
		{
			this.main_module.Write(fileName, parameters);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001A8CA File Offset: 0x00018ACA
		public void Write()
		{
			this.main_module.Write();
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001A8D7 File Offset: 0x00018AD7
		public void Write(WriterParameters parameters)
		{
			this.main_module.Write(parameters);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001A8E5 File Offset: 0x00018AE5
		public void Write(Stream stream)
		{
			this.Write(stream, new WriterParameters());
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001A8F3 File Offset: 0x00018AF3
		public void Write(Stream stream, WriterParameters parameters)
		{
			this.main_module.Write(stream, parameters);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001A902 File Offset: 0x00018B02
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x04000261 RID: 609
		private AssemblyNameDefinition name;

		// Token: 0x04000262 RID: 610
		internal ModuleDefinition main_module;

		// Token: 0x04000263 RID: 611
		private Collection<ModuleDefinition> modules;

		// Token: 0x04000264 RID: 612
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000265 RID: 613
		private Collection<SecurityDeclaration> security_declarations;
	}
}
