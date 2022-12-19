using System;
using System.IO;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000BD RID: 189
	public sealed class AssemblyDefinition : ICustomAttributeProvider, IMetadataTokenProvider, ISecurityDeclarationProvider, IDisposable
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x000147C3 File Offset: 0x000129C3
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x000147CB File Offset: 0x000129CB
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x000147D4 File Offset: 0x000129D4
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

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x000147EF File Offset: 0x000129EF
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x00012279 File Offset: 0x00010479
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

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x000147FC File Offset: 0x000129FC
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0001487C File Offset: 0x00012A7C
		public ModuleDefinition MainModule
		{
			get
			{
				return this.main_module;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00014884 File Offset: 0x00012A84
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x00014891 File Offset: 0x00012A91
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x0001489F File Offset: 0x00012A9F
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x000148C4 File Offset: 0x00012AC4
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.main_module);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x000148E2 File Offset: 0x00012AE2
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x00014907 File Offset: 0x00012B07
		public Collection<SecurityDeclaration> SecurityDeclarations
		{
			get
			{
				return this.security_declarations ?? this.GetSecurityDeclarations(ref this.security_declarations, this.main_module);
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00002AED File Offset: 0x00000CED
		internal AssemblyDefinition()
		{
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00014928 File Offset: 0x00012B28
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

		// Token: 0x0600047D RID: 1149 RVA: 0x0001496D File Offset: 0x00012B6D
		public static AssemblyDefinition CreateAssembly(AssemblyNameDefinition assemblyName, string moduleName, ModuleKind kind)
		{
			return AssemblyDefinition.CreateAssembly(assemblyName, moduleName, new ModuleParameters
			{
				Kind = kind
			});
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00014984 File Offset: 0x00012B84
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

		// Token: 0x0600047F RID: 1151 RVA: 0x000149DA File Offset: 0x00012BDA
		public static AssemblyDefinition ReadAssembly(string fileName)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(fileName));
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000149E7 File Offset: 0x00012BE7
		public static AssemblyDefinition ReadAssembly(string fileName, ReaderParameters parameters)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(fileName, parameters));
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000149F5 File Offset: 0x00012BF5
		public static AssemblyDefinition ReadAssembly(Stream stream)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(stream));
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00014A02 File Offset: 0x00012C02
		public static AssemblyDefinition ReadAssembly(Stream stream, ReaderParameters parameters)
		{
			return AssemblyDefinition.ReadAssembly(ModuleDefinition.ReadModule(stream, parameters));
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00014A10 File Offset: 0x00012C10
		private static AssemblyDefinition ReadAssembly(ModuleDefinition module)
		{
			AssemblyDefinition assembly = module.Assembly;
			if (assembly == null)
			{
				throw new ArgumentException();
			}
			return assembly;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00014A21 File Offset: 0x00012C21
		public void Write(string fileName)
		{
			this.Write(fileName, new WriterParameters());
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00014A2F File Offset: 0x00012C2F
		public void Write(string fileName, WriterParameters parameters)
		{
			this.main_module.Write(fileName, parameters);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00014A3E File Offset: 0x00012C3E
		public void Write()
		{
			this.main_module.Write();
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00014A4B File Offset: 0x00012C4B
		public void Write(WriterParameters parameters)
		{
			this.main_module.Write(parameters);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00014A59 File Offset: 0x00012C59
		public void Write(Stream stream)
		{
			this.Write(stream, new WriterParameters());
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00014A67 File Offset: 0x00012C67
		public void Write(Stream stream, WriterParameters parameters)
		{
			this.main_module.Write(stream, parameters);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00014A76 File Offset: 0x00012C76
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x04000233 RID: 563
		private AssemblyNameDefinition name;

		// Token: 0x04000234 RID: 564
		internal ModuleDefinition main_module;

		// Token: 0x04000235 RID: 565
		private Collection<ModuleDefinition> modules;

		// Token: 0x04000236 RID: 566
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000237 RID: 567
		private Collection<SecurityDeclaration> security_declarations;
	}
}
