using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000244 RID: 580
	public sealed class ModuleDefinition : ModuleReference, ICustomAttributeProvider, IMetadataTokenProvider, ICustomDebugInformationProvider, IDisposable
	{
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0002BBE6 File Offset: 0x00029DE6
		public bool IsMain
		{
			get
			{
				return this.kind != ModuleKind.NetModule;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x0002BBF4 File Offset: 0x00029DF4
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x0002BBFC File Offset: 0x00029DFC
		public ModuleKind Kind
		{
			get
			{
				return this.kind;
			}
			set
			{
				this.kind = value;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x0002BC05 File Offset: 0x00029E05
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x0002BC0D File Offset: 0x00029E0D
		public MetadataKind MetadataKind
		{
			get
			{
				return this.metadata_kind;
			}
			set
			{
				this.metadata_kind = value;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x0002BC16 File Offset: 0x00029E16
		internal WindowsRuntimeProjections Projections
		{
			get
			{
				if (this.projections == null)
				{
					Interlocked.CompareExchange<WindowsRuntimeProjections>(ref this.projections, new WindowsRuntimeProjections(this), null);
				}
				return this.projections;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0002BC39 File Offset: 0x00029E39
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x0002BC41 File Offset: 0x00029E41
		public TargetRuntime Runtime
		{
			get
			{
				return this.runtime;
			}
			set
			{
				this.runtime = value;
				this.runtime_version = this.runtime.RuntimeVersionString();
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0002BC5B File Offset: 0x00029E5B
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x0002BC63 File Offset: 0x00029E63
		public string RuntimeVersion
		{
			get
			{
				return this.runtime_version;
			}
			set
			{
				this.runtime_version = value;
				this.runtime = this.runtime_version.ParseRuntime();
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x0002BC7D File Offset: 0x00029E7D
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x0002BC85 File Offset: 0x00029E85
		public TargetArchitecture Architecture
		{
			get
			{
				return this.architecture;
			}
			set
			{
				this.architecture = value;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x0002BC8E File Offset: 0x00029E8E
		// (set) Token: 0x06000D9B RID: 3483 RVA: 0x0002BC96 File Offset: 0x00029E96
		public ModuleAttributes Attributes
		{
			get
			{
				return this.attributes;
			}
			set
			{
				this.attributes = value;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x0002BC9F File Offset: 0x00029E9F
		// (set) Token: 0x06000D9D RID: 3485 RVA: 0x0002BCA7 File Offset: 0x00029EA7
		public ModuleCharacteristics Characteristics
		{
			get
			{
				return this.characteristics;
			}
			set
			{
				this.characteristics = value;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x0002BCB0 File Offset: 0x00029EB0
		[Obsolete("Use FileName")]
		public string FullyQualifiedName
		{
			get
			{
				return this.file_name;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x0002BCB0 File Offset: 0x00029EB0
		public string FileName
		{
			get
			{
				return this.file_name;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x0002BCB8 File Offset: 0x00029EB8
		// (set) Token: 0x06000DA1 RID: 3489 RVA: 0x0002BCC0 File Offset: 0x00029EC0
		public Guid Mvid
		{
			get
			{
				return this.mvid;
			}
			set
			{
				this.mvid = value;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x0002BCC9 File Offset: 0x00029EC9
		internal bool HasImage
		{
			get
			{
				return this.Image != null;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0002BCD4 File Offset: 0x00029ED4
		public bool HasSymbols
		{
			get
			{
				return this.symbol_reader != null;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0002BCDF File Offset: 0x00029EDF
		public ISymbolReader SymbolReader
		{
			get
			{
				return this.symbol_reader;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x0001A92F File Offset: 0x00018B2F
		public override MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.ModuleDefinition;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x0002BCE7 File Offset: 0x00029EE7
		public AssemblyDefinition Assembly
		{
			get
			{
				return this.assembly;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x0002BCEF File Offset: 0x00029EEF
		internal IReflectionImporter ReflectionImporter
		{
			get
			{
				if (this.reflection_importer == null)
				{
					Interlocked.CompareExchange<IReflectionImporter>(ref this.reflection_importer, new DefaultReflectionImporter(this), null);
				}
				return this.reflection_importer;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x0002BD12 File Offset: 0x00029F12
		internal IMetadataImporter MetadataImporter
		{
			get
			{
				if (this.metadata_importer == null)
				{
					Interlocked.CompareExchange<IMetadataImporter>(ref this.metadata_importer, new DefaultMetadataImporter(this), null);
				}
				return this.metadata_importer;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x0002BD38 File Offset: 0x00029F38
		public IAssemblyResolver AssemblyResolver
		{
			get
			{
				if (this.assembly_resolver.value == null)
				{
					object obj = this.module_lock;
					lock (obj)
					{
						this.assembly_resolver = Disposable.Owned<IAssemblyResolver>(new DefaultAssemblyResolver());
					}
				}
				return this.assembly_resolver.value;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0002BD9C File Offset: 0x00029F9C
		public IMetadataResolver MetadataResolver
		{
			get
			{
				if (this.metadata_resolver == null)
				{
					Interlocked.CompareExchange<IMetadataResolver>(ref this.metadata_resolver, new MetadataResolver(this.AssemblyResolver), null);
				}
				return this.metadata_resolver;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x0002BDC4 File Offset: 0x00029FC4
		public TypeSystem TypeSystem
		{
			get
			{
				if (this.type_system == null)
				{
					Interlocked.CompareExchange<TypeSystem>(ref this.type_system, TypeSystem.CreateTypeSystem(this), null);
				}
				return this.type_system;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0002BDE7 File Offset: 0x00029FE7
		public bool HasAssemblyReferences
		{
			get
			{
				if (this.references != null)
				{
					return this.references.Count > 0;
				}
				return this.HasImage && this.Image.HasTable(Table.AssemblyRef);
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0002BE18 File Offset: 0x0002A018
		public Collection<AssemblyNameReference> AssemblyReferences
		{
			get
			{
				if (this.references != null)
				{
					return this.references;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, Collection<AssemblyNameReference>>(ref this.references, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadAssemblyReferences());
				}
				Interlocked.CompareExchange<Collection<AssemblyNameReference>>(ref this.references, new Collection<AssemblyNameReference>(), null);
				return this.references;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x0002BE81 File Offset: 0x0002A081
		public bool HasModuleReferences
		{
			get
			{
				if (this.modules != null)
				{
					return this.modules.Count > 0;
				}
				return this.HasImage && this.Image.HasTable(Table.ModuleRef);
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0002BEB4 File Offset: 0x0002A0B4
		public Collection<ModuleReference> ModuleReferences
		{
			get
			{
				if (this.modules != null)
				{
					return this.modules;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, Collection<ModuleReference>>(ref this.modules, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadModuleReferences());
				}
				Interlocked.CompareExchange<Collection<ModuleReference>>(ref this.modules, new Collection<ModuleReference>(), null);
				return this.modules;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0002BF20 File Offset: 0x0002A120
		public bool HasResources
		{
			get
			{
				if (this.resources != null)
				{
					return this.resources.Count > 0;
				}
				if (!this.HasImage)
				{
					return false;
				}
				if (!this.Image.HasTable(Table.ManifestResource))
				{
					return this.Read<ModuleDefinition, bool>(this, (ModuleDefinition _, MetadataReader reader) => reader.HasFileResource());
				}
				return true;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x0002BF88 File Offset: 0x0002A188
		public Collection<Resource> Resources
		{
			get
			{
				if (this.resources != null)
				{
					return this.resources;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, Collection<Resource>>(ref this.resources, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadResources());
				}
				Interlocked.CompareExchange<Collection<Resource>>(ref this.resources, new Collection<Resource>(), null);
				return this.resources;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0002BFF1 File Offset: 0x0002A1F1
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this);
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0002C011 File Offset: 0x0002A211
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this);
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x0002C02A File Offset: 0x0002A22A
		public bool HasTypes
		{
			get
			{
				if (this.types != null)
				{
					return this.types.Count > 0;
				}
				return this.HasImage && this.Image.HasTable(Table.TypeDef);
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0002C05C File Offset: 0x0002A25C
		public Collection<TypeDefinition> Types
		{
			get
			{
				if (this.types != null)
				{
					return this.types;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, TypeDefinitionCollection>(ref this.types, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadTypes());
				}
				Interlocked.CompareExchange<TypeDefinitionCollection>(ref this.types, new TypeDefinitionCollection(this), null);
				return this.types;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x0002C0C6 File Offset: 0x0002A2C6
		public bool HasExportedTypes
		{
			get
			{
				if (this.exported_types != null)
				{
					return this.exported_types.Count > 0;
				}
				return this.HasImage && this.Image.HasTable(Table.ExportedType);
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0002C0F8 File Offset: 0x0002A2F8
		public Collection<ExportedType> ExportedTypes
		{
			get
			{
				if (this.exported_types != null)
				{
					return this.exported_types;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, Collection<ExportedType>>(ref this.exported_types, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadExportedTypes());
				}
				Interlocked.CompareExchange<Collection<ExportedType>>(ref this.exported_types, new Collection<ExportedType>(), null);
				return this.exported_types;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x0002C164 File Offset: 0x0002A364
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x0002C1CB File Offset: 0x0002A3CB
		public MethodDefinition EntryPoint
		{
			get
			{
				if (this.entry_point_set)
				{
					return this.entry_point;
				}
				if (this.HasImage)
				{
					this.Read<ModuleDefinition, MethodDefinition>(ref this.entry_point, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadEntryPoint());
				}
				else
				{
					this.entry_point = null;
				}
				this.entry_point_set = true;
				return this.entry_point;
			}
			set
			{
				this.entry_point = value;
				this.entry_point_set = true;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0002C1DB File Offset: 0x0002A3DB
		public bool HasCustomDebugInformations
		{
			get
			{
				return this.custom_infos != null && this.custom_infos.Count > 0;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0002C1F5 File Offset: 0x0002A3F5
		public Collection<CustomDebugInformation> CustomDebugInformations
		{
			get
			{
				if (this.custom_infos == null)
				{
					Interlocked.CompareExchange<Collection<CustomDebugInformation>>(ref this.custom_infos, new Collection<CustomDebugInformation>(), null);
				}
				return this.custom_infos;
			}
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0002C217 File Offset: 0x0002A417
		internal ModuleDefinition()
		{
			this.MetadataSystem = new MetadataSystem();
			this.token = new MetadataToken(TokenType.Module, 1);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0002C250 File Offset: 0x0002A450
		internal ModuleDefinition(Image image)
			: this()
		{
			this.Image = image;
			this.kind = image.Kind;
			this.RuntimeVersion = image.RuntimeVersion;
			this.architecture = image.Architecture;
			this.attributes = image.Attributes;
			this.characteristics = image.DllCharacteristics;
			this.linker_version = image.LinkerVersion;
			this.subsystem_major = image.SubSystemMajor;
			this.subsystem_minor = image.SubSystemMinor;
			this.file_name = image.FileName;
			this.timestamp = image.Timestamp;
			this.reader = new MetadataReader(this);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0002C2EE File Offset: 0x0002A4EE
		public void Dispose()
		{
			if (this.Image != null)
			{
				this.Image.Dispose();
			}
			if (this.symbol_reader != null)
			{
				this.symbol_reader.Dispose();
			}
			if (this.assembly_resolver.value != null)
			{
				this.assembly_resolver.Dispose();
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0002C32E File Offset: 0x0002A52E
		public bool HasTypeReference(string fullName)
		{
			return this.HasTypeReference(string.Empty, fullName);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0002C33C File Offset: 0x0002A53C
		public bool HasTypeReference(string scope, string fullName)
		{
			Mixin.CheckFullName(fullName);
			return this.HasImage && this.GetTypeReference(scope, fullName) != null;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0002C359 File Offset: 0x0002A559
		public bool TryGetTypeReference(string fullName, out TypeReference type)
		{
			return this.TryGetTypeReference(string.Empty, fullName, out type);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0002C368 File Offset: 0x0002A568
		public bool TryGetTypeReference(string scope, string fullName, out TypeReference type)
		{
			Mixin.CheckFullName(fullName);
			if (!this.HasImage)
			{
				type = null;
				return false;
			}
			TypeReference typeReference;
			type = (typeReference = this.GetTypeReference(scope, fullName));
			return typeReference != null;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0002C398 File Offset: 0x0002A598
		private TypeReference GetTypeReference(string scope, string fullname)
		{
			return this.Read<Row<string, string>, TypeReference>(new Row<string, string>(scope, fullname), (Row<string, string> row, MetadataReader reader) => reader.GetTypeReference(row.Col1, row.Col2));
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0002C3C6 File Offset: 0x0002A5C6
		public IEnumerable<TypeReference> GetTypeReferences()
		{
			if (!this.HasImage)
			{
				return Empty<TypeReference>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<TypeReference>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetTypeReferences());
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0002C3FC File Offset: 0x0002A5FC
		public IEnumerable<MemberReference> GetMemberReferences()
		{
			if (!this.HasImage)
			{
				return Empty<MemberReference>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<MemberReference>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetMemberReferences());
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0002C432 File Offset: 0x0002A632
		public IEnumerable<CustomAttribute> GetCustomAttributes()
		{
			if (!this.HasImage)
			{
				return Empty<CustomAttribute>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<CustomAttribute>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetCustomAttributes());
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0002C468 File Offset: 0x0002A668
		public TypeReference GetType(string fullName, bool runtimeName)
		{
			if (!runtimeName)
			{
				return this.GetType(fullName);
			}
			return TypeParser.ParseType(this, fullName, true);
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0002C47D File Offset: 0x0002A67D
		public TypeDefinition GetType(string fullName)
		{
			Mixin.CheckFullName(fullName);
			if (fullName.IndexOf('/') > 0)
			{
				return this.GetNestedType(fullName);
			}
			return ((TypeDefinitionCollection)this.Types).GetType(fullName);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0002C4A9 File Offset: 0x0002A6A9
		public TypeDefinition GetType(string @namespace, string name)
		{
			Mixin.CheckName(name);
			return ((TypeDefinitionCollection)this.Types).GetType(@namespace ?? string.Empty, name);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0002C4CC File Offset: 0x0002A6CC
		public IEnumerable<TypeDefinition> GetTypes()
		{
			return ModuleDefinition.GetTypes(this.Types);
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0002C4D9 File Offset: 0x0002A6D9
		private static IEnumerable<TypeDefinition> GetTypes(Collection<TypeDefinition> types)
		{
			int num;
			for (int i = 0; i < types.Count; i = num + 1)
			{
				TypeDefinition type = types[i];
				yield return type;
				if (type.HasNestedTypes)
				{
					foreach (TypeDefinition typeDefinition in ModuleDefinition.GetTypes(type.NestedTypes))
					{
						yield return typeDefinition;
					}
					IEnumerator<TypeDefinition> enumerator = null;
					type = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0002C4EC File Offset: 0x0002A6EC
		private TypeDefinition GetNestedType(string fullname)
		{
			string[] array = fullname.Split(new char[] { '/' });
			TypeDefinition typeDefinition = this.GetType(array[0]);
			if (typeDefinition == null)
			{
				return null;
			}
			for (int i = 1; i < array.Length; i++)
			{
				TypeDefinition nestedType = typeDefinition.GetNestedType(array[i]);
				if (nestedType == null)
				{
					return null;
				}
				typeDefinition = nestedType;
			}
			return typeDefinition;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0002C53A File Offset: 0x0002A73A
		internal FieldDefinition Resolve(FieldReference field)
		{
			return this.MetadataResolver.Resolve(field);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0002C548 File Offset: 0x0002A748
		internal MethodDefinition Resolve(MethodReference method)
		{
			return this.MetadataResolver.Resolve(method);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0002C556 File Offset: 0x0002A756
		internal TypeDefinition Resolve(TypeReference type)
		{
			return this.MetadataResolver.Resolve(type);
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0002C564 File Offset: 0x0002A764
		private static void CheckContext(IGenericParameterProvider context, ModuleDefinition module)
		{
			if (context == null)
			{
				return;
			}
			if (context.Module != module)
			{
				throw new ArgumentException();
			}
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0002C579 File Offset: 0x0002A779
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(Type type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0002C579 File Offset: 0x0002A779
		public TypeReference ImportReference(Type type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0002C583 File Offset: 0x0002A783
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(Type type, IGenericParameterProvider context)
		{
			return this.ImportReference(type, context);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0002C58D File Offset: 0x0002A78D
		public TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			Mixin.CheckType(type);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(type, context);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0002C5A9 File Offset: 0x0002A7A9
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldInfo field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0002C5B3 File Offset: 0x0002A7B3
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldInfo field, IGenericParameterProvider context)
		{
			return this.ImportReference(field, context);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0002C5A9 File Offset: 0x0002A7A9
		public FieldReference ImportReference(FieldInfo field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0002C5BD File Offset: 0x0002A7BD
		public FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			Mixin.CheckField(field);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(field, context);
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0002C5D9 File Offset: 0x0002A7D9
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodBase method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0002C5E3 File Offset: 0x0002A7E3
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodBase method, IGenericParameterProvider context)
		{
			return this.ImportReference(method, context);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0002C5D9 File Offset: 0x0002A7D9
		public MethodReference ImportReference(MethodBase method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0002C5ED File Offset: 0x0002A7ED
		public MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			Mixin.CheckMethod(method);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(method, context);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0002C609 File Offset: 0x0002A809
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(TypeReference type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0002C613 File Offset: 0x0002A813
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(TypeReference type, IGenericParameterProvider context)
		{
			return this.ImportReference(type, context);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0002C609 File Offset: 0x0002A809
		public TypeReference ImportReference(TypeReference type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0002C61D File Offset: 0x0002A81D
		public TypeReference ImportReference(TypeReference type, IGenericParameterProvider context)
		{
			Mixin.CheckType(type);
			if (type.Module == this)
			{
				return type;
			}
			ModuleDefinition.CheckContext(context, this);
			return this.MetadataImporter.ImportReference(type, context);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0002C644 File Offset: 0x0002A844
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldReference field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0002C64E File Offset: 0x0002A84E
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldReference field, IGenericParameterProvider context)
		{
			return this.ImportReference(field, context);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0002C644 File Offset: 0x0002A844
		public FieldReference ImportReference(FieldReference field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0002C658 File Offset: 0x0002A858
		public FieldReference ImportReference(FieldReference field, IGenericParameterProvider context)
		{
			Mixin.CheckField(field);
			if (field.Module == this)
			{
				return field;
			}
			ModuleDefinition.CheckContext(context, this);
			return this.MetadataImporter.ImportReference(field, context);
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0002C67F File Offset: 0x0002A87F
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodReference method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0002C689 File Offset: 0x0002A889
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodReference method, IGenericParameterProvider context)
		{
			return this.ImportReference(method, context);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0002C67F File Offset: 0x0002A87F
		public MethodReference ImportReference(MethodReference method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0002C693 File Offset: 0x0002A893
		public MethodReference ImportReference(MethodReference method, IGenericParameterProvider context)
		{
			Mixin.CheckMethod(method);
			if (method.Module == this)
			{
				return method;
			}
			ModuleDefinition.CheckContext(context, this);
			return this.MetadataImporter.ImportReference(method, context);
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0002C6BA File Offset: 0x0002A8BA
		public IMetadataTokenProvider LookupToken(int token)
		{
			return this.LookupToken(new MetadataToken((uint)token));
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0002C6C8 File Offset: 0x0002A8C8
		public IMetadataTokenProvider LookupToken(MetadataToken token)
		{
			return this.Read<MetadataToken, IMetadataTokenProvider>(token, (MetadataToken t, MetadataReader reader) => reader.LookupToken(t));
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0002C6F0 File Offset: 0x0002A8F0
		public void ImmediateRead()
		{
			if (!this.HasImage)
			{
				return;
			}
			this.ReadingMode = ReadingMode.Immediate;
			new ImmediateModuleReader(this.Image).ReadModule(this, true);
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x0002C714 File Offset: 0x0002A914
		internal object SyncRoot
		{
			get
			{
				return this.module_lock;
			}
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0002C71C File Offset: 0x0002A91C
		internal void Read<TItem>(TItem item, Action<TItem, MetadataReader> read)
		{
			object obj = this.module_lock;
			lock (obj)
			{
				int position = this.reader.position;
				IGenericContext context = this.reader.context;
				read(item, this.reader);
				this.reader.position = position;
				this.reader.context = context;
			}
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0002C794 File Offset: 0x0002A994
		internal TRet Read<TItem, TRet>(TItem item, Func<TItem, MetadataReader, TRet> read)
		{
			object obj = this.module_lock;
			TRet tret2;
			lock (obj)
			{
				int position = this.reader.position;
				IGenericContext context = this.reader.context;
				TRet tret = read(item, this.reader);
				this.reader.position = position;
				this.reader.context = context;
				tret2 = tret;
			}
			return tret2;
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0002C810 File Offset: 0x0002AA10
		internal TRet Read<TItem, TRet>(ref TRet variable, TItem item, Func<TItem, MetadataReader, TRet> read) where TRet : class
		{
			object obj = this.module_lock;
			TRet tret;
			lock (obj)
			{
				if (variable != null)
				{
					tret = variable;
				}
				else
				{
					int position = this.reader.position;
					IGenericContext context = this.reader.context;
					TRet tret2 = read(item, this.reader);
					this.reader.position = position;
					this.reader.context = context;
					tret = (variable = tret2);
				}
			}
			return tret;
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x0002C8B4 File Offset: 0x0002AAB4
		public bool HasDebugHeader
		{
			get
			{
				return this.Image != null && this.Image.DebugHeader != null;
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0002C8CE File Offset: 0x0002AACE
		public ImageDebugHeader GetDebugHeader()
		{
			return this.Image.DebugHeader ?? new ImageDebugHeader();
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0002C8E4 File Offset: 0x0002AAE4
		public static ModuleDefinition CreateModule(string name, ModuleKind kind)
		{
			return ModuleDefinition.CreateModule(name, new ModuleParameters
			{
				Kind = kind
			});
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0002C8F8 File Offset: 0x0002AAF8
		public static ModuleDefinition CreateModule(string name, ModuleParameters parameters)
		{
			Mixin.CheckName(name);
			Mixin.CheckParameters(parameters);
			ModuleDefinition moduleDefinition = new ModuleDefinition
			{
				Name = name,
				kind = parameters.Kind,
				timestamp = (parameters.Timestamp ?? Mixin.GetTimestamp()),
				Runtime = parameters.Runtime,
				architecture = parameters.Architecture,
				mvid = Guid.NewGuid(),
				Attributes = ModuleAttributes.ILOnly,
				Characteristics = (ModuleCharacteristics.DynamicBase | ModuleCharacteristics.NoSEH | ModuleCharacteristics.NXCompat | ModuleCharacteristics.TerminalServerAware)
			};
			if (parameters.AssemblyResolver != null)
			{
				moduleDefinition.assembly_resolver = Disposable.NotOwned<IAssemblyResolver>(parameters.AssemblyResolver);
			}
			if (parameters.MetadataResolver != null)
			{
				moduleDefinition.metadata_resolver = parameters.MetadataResolver;
			}
			if (parameters.MetadataImporterProvider != null)
			{
				moduleDefinition.metadata_importer = parameters.MetadataImporterProvider.GetMetadataImporter(moduleDefinition);
			}
			if (parameters.ReflectionImporterProvider != null)
			{
				moduleDefinition.reflection_importer = parameters.ReflectionImporterProvider.GetReflectionImporter(moduleDefinition);
			}
			if (parameters.Kind != ModuleKind.NetModule)
			{
				AssemblyDefinition assemblyDefinition = new AssemblyDefinition();
				moduleDefinition.assembly = assemblyDefinition;
				moduleDefinition.assembly.Name = ModuleDefinition.CreateAssemblyName(name);
				assemblyDefinition.main_module = moduleDefinition;
			}
			moduleDefinition.Types.Add(new TypeDefinition(string.Empty, "<Module>", TypeAttributes.NotPublic));
			return moduleDefinition;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0002CA2E File Offset: 0x0002AC2E
		private static AssemblyNameDefinition CreateAssemblyName(string name)
		{
			if (name.EndsWith(".dll") || name.EndsWith(".exe"))
			{
				name = name.Substring(0, name.Length - 4);
			}
			return new AssemblyNameDefinition(name, Mixin.ZeroVersion);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0002CA68 File Offset: 0x0002AC68
		public void ReadSymbols()
		{
			if (string.IsNullOrEmpty(this.file_name))
			{
				throw new InvalidOperationException();
			}
			DefaultSymbolReaderProvider defaultSymbolReaderProvider = new DefaultSymbolReaderProvider(true);
			this.ReadSymbols(defaultSymbolReaderProvider.GetSymbolReader(this, this.file_name), true);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0002CAA3 File Offset: 0x0002ACA3
		public void ReadSymbols(ISymbolReader reader)
		{
			this.ReadSymbols(reader, true);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0002CAB0 File Offset: 0x0002ACB0
		public void ReadSymbols(ISymbolReader reader, bool throwIfSymbolsAreNotMaching)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.symbol_reader = reader;
			if (this.symbol_reader.ProcessDebugHeader(this.GetDebugHeader()))
			{
				if (this.HasImage && this.ReadingMode == ReadingMode.Immediate)
				{
					new ImmediateModuleReader(this.Image).ReadSymbols(this);
				}
				return;
			}
			this.symbol_reader = null;
			if (throwIfSymbolsAreNotMaching)
			{
				throw new SymbolsNotMatchingException("Symbols were found but are not matching the assembly");
			}
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0002CB1D File Offset: 0x0002AD1D
		public static ModuleDefinition ReadModule(string fileName)
		{
			return ModuleDefinition.ReadModule(fileName, new ReaderParameters(ReadingMode.Deferred));
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0002CB2C File Offset: 0x0002AD2C
		public static ModuleDefinition ReadModule(string fileName, ReaderParameters parameters)
		{
			Stream stream = ModuleDefinition.GetFileStream(fileName, FileMode.Open, parameters.ReadWrite ? FileAccess.ReadWrite : FileAccess.Read, FileShare.Read);
			if (parameters.InMemory)
			{
				MemoryStream memoryStream = new MemoryStream(stream.CanSeek ? ((int)stream.Length) : 0);
				using (stream)
				{
					stream.CopyTo(memoryStream);
				}
				memoryStream.Position = 0L;
				stream = memoryStream;
			}
			ModuleDefinition moduleDefinition;
			try
			{
				moduleDefinition = ModuleDefinition.ReadModule(Disposable.Owned<Stream>(stream), fileName, parameters);
			}
			catch (Exception)
			{
				stream.Dispose();
				throw;
			}
			return moduleDefinition;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0002CBC4 File Offset: 0x0002ADC4
		private static Stream GetFileStream(string fileName, FileMode mode, FileAccess access, FileShare share)
		{
			Mixin.CheckFileName(fileName);
			return new FileStream(fileName, mode, access, share);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0002CBD5 File Offset: 0x0002ADD5
		public static ModuleDefinition ReadModule(Stream stream)
		{
			return ModuleDefinition.ReadModule(stream, new ReaderParameters(ReadingMode.Deferred));
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0002CBE3 File Offset: 0x0002ADE3
		public static ModuleDefinition ReadModule(Stream stream, ReaderParameters parameters)
		{
			Mixin.CheckStream(stream);
			Mixin.CheckReadSeek(stream);
			return ModuleDefinition.ReadModule(Disposable.NotOwned<Stream>(stream), stream.GetFileName(), parameters);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0002CC03 File Offset: 0x0002AE03
		private static ModuleDefinition ReadModule(Disposable<Stream> stream, string fileName, ReaderParameters parameters)
		{
			Mixin.CheckParameters(parameters);
			return ModuleReader.CreateModule(ImageReader.ReadImage(stream, fileName), parameters);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0002CC18 File Offset: 0x0002AE18
		public void Write(string fileName)
		{
			this.Write(fileName, new WriterParameters());
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0002CC28 File Offset: 0x0002AE28
		public void Write(string fileName, WriterParameters parameters)
		{
			Mixin.CheckParameters(parameters);
			Stream fileStream = ModuleDefinition.GetFileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
			ModuleWriter.WriteModule(this, Disposable.Owned<Stream>(fileStream), parameters);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0002CC52 File Offset: 0x0002AE52
		public void Write()
		{
			this.Write(new WriterParameters());
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0002CC5F File Offset: 0x0002AE5F
		public void Write(WriterParameters parameters)
		{
			if (!this.HasImage)
			{
				throw new InvalidOperationException();
			}
			this.Write(this.Image.Stream.value, parameters);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0002CC86 File Offset: 0x0002AE86
		public void Write(Stream stream)
		{
			this.Write(stream, new WriterParameters());
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0002CC94 File Offset: 0x0002AE94
		public void Write(Stream stream, WriterParameters parameters)
		{
			Mixin.CheckStream(stream);
			Mixin.CheckWriteSeek(stream);
			Mixin.CheckParameters(parameters);
			ModuleWriter.WriteModule(this, Disposable.NotOwned<Stream>(stream), parameters);
		}

		// Token: 0x040003FA RID: 1018
		internal Image Image;

		// Token: 0x040003FB RID: 1019
		internal MetadataSystem MetadataSystem;

		// Token: 0x040003FC RID: 1020
		internal ReadingMode ReadingMode;

		// Token: 0x040003FD RID: 1021
		internal ISymbolReaderProvider SymbolReaderProvider;

		// Token: 0x040003FE RID: 1022
		internal ISymbolReader symbol_reader;

		// Token: 0x040003FF RID: 1023
		internal Disposable<IAssemblyResolver> assembly_resolver;

		// Token: 0x04000400 RID: 1024
		internal IMetadataResolver metadata_resolver;

		// Token: 0x04000401 RID: 1025
		internal TypeSystem type_system;

		// Token: 0x04000402 RID: 1026
		internal readonly MetadataReader reader;

		// Token: 0x04000403 RID: 1027
		private readonly string file_name;

		// Token: 0x04000404 RID: 1028
		internal string runtime_version;

		// Token: 0x04000405 RID: 1029
		internal ModuleKind kind;

		// Token: 0x04000406 RID: 1030
		private WindowsRuntimeProjections projections;

		// Token: 0x04000407 RID: 1031
		private MetadataKind metadata_kind;

		// Token: 0x04000408 RID: 1032
		private TargetRuntime runtime;

		// Token: 0x04000409 RID: 1033
		private TargetArchitecture architecture;

		// Token: 0x0400040A RID: 1034
		private ModuleAttributes attributes;

		// Token: 0x0400040B RID: 1035
		private ModuleCharacteristics characteristics;

		// Token: 0x0400040C RID: 1036
		private Guid mvid;

		// Token: 0x0400040D RID: 1037
		internal ushort linker_version = 8;

		// Token: 0x0400040E RID: 1038
		internal ushort subsystem_major = 4;

		// Token: 0x0400040F RID: 1039
		internal ushort subsystem_minor;

		// Token: 0x04000410 RID: 1040
		internal uint timestamp;

		// Token: 0x04000411 RID: 1041
		internal AssemblyDefinition assembly;

		// Token: 0x04000412 RID: 1042
		private MethodDefinition entry_point;

		// Token: 0x04000413 RID: 1043
		private bool entry_point_set;

		// Token: 0x04000414 RID: 1044
		internal IReflectionImporter reflection_importer;

		// Token: 0x04000415 RID: 1045
		internal IMetadataImporter metadata_importer;

		// Token: 0x04000416 RID: 1046
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000417 RID: 1047
		private Collection<AssemblyNameReference> references;

		// Token: 0x04000418 RID: 1048
		private Collection<ModuleReference> modules;

		// Token: 0x04000419 RID: 1049
		private Collection<Resource> resources;

		// Token: 0x0400041A RID: 1050
		private Collection<ExportedType> exported_types;

		// Token: 0x0400041B RID: 1051
		private TypeDefinitionCollection types;

		// Token: 0x0400041C RID: 1052
		internal Collection<CustomDebugInformation> custom_infos;

		// Token: 0x0400041D RID: 1053
		internal MetadataBuilder metadata_builder;

		// Token: 0x0400041E RID: 1054
		private readonly object module_lock = new object();
	}
}
