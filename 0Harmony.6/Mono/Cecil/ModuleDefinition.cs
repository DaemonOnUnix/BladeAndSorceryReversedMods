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
	// Token: 0x02000150 RID: 336
	public sealed class ModuleDefinition : ModuleReference, ICustomAttributeProvider, IMetadataTokenProvider, ICustomDebugInformationProvider, IDisposable
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x000255B2 File Offset: 0x000237B2
		public bool IsMain
		{
			get
			{
				return this.kind != ModuleKind.NetModule;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x000255C0 File Offset: 0x000237C0
		// (set) Token: 0x06000A47 RID: 2631 RVA: 0x000255C8 File Offset: 0x000237C8
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

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x000255D1 File Offset: 0x000237D1
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x000255D9 File Offset: 0x000237D9
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

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x000255E2 File Offset: 0x000237E2
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

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x00025605 File Offset: 0x00023805
		// (set) Token: 0x06000A4C RID: 2636 RVA: 0x0002560D File Offset: 0x0002380D
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

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x00025627 File Offset: 0x00023827
		// (set) Token: 0x06000A4E RID: 2638 RVA: 0x0002562F File Offset: 0x0002382F
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

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x00025649 File Offset: 0x00023849
		// (set) Token: 0x06000A50 RID: 2640 RVA: 0x00025651 File Offset: 0x00023851
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

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0002565A File Offset: 0x0002385A
		// (set) Token: 0x06000A52 RID: 2642 RVA: 0x00025662 File Offset: 0x00023862
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

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0002566B File Offset: 0x0002386B
		// (set) Token: 0x06000A54 RID: 2644 RVA: 0x00025673 File Offset: 0x00023873
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

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0002567C File Offset: 0x0002387C
		[Obsolete("Use FileName")]
		public string FullyQualifiedName
		{
			get
			{
				return this.file_name;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0002567C File Offset: 0x0002387C
		public string FileName
		{
			get
			{
				return this.file_name;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x00025684 File Offset: 0x00023884
		// (set) Token: 0x06000A58 RID: 2648 RVA: 0x0002568C File Offset: 0x0002388C
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

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00025695 File Offset: 0x00023895
		internal bool HasImage
		{
			get
			{
				return this.Image != null;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x000256A0 File Offset: 0x000238A0
		public bool HasSymbols
		{
			get
			{
				return this.symbol_reader != null;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x000256AB File Offset: 0x000238AB
		public ISymbolReader SymbolReader
		{
			get
			{
				return this.symbol_reader;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x00014AA3 File Offset: 0x00012CA3
		public override MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.ModuleDefinition;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x000256B3 File Offset: 0x000238B3
		public AssemblyDefinition Assembly
		{
			get
			{
				return this.assembly;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x000256BB File Offset: 0x000238BB
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

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x000256DE File Offset: 0x000238DE
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

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x00025704 File Offset: 0x00023904
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

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00025768 File Offset: 0x00023968
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

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00025790 File Offset: 0x00023990
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

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x000257B3 File Offset: 0x000239B3
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

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x000257E4 File Offset: 0x000239E4
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

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0002584D File Offset: 0x00023A4D
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

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x00025880 File Offset: 0x00023A80
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

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x000258EC File Offset: 0x00023AEC
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

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x00025954 File Offset: 0x00023B54
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

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x000259BD File Offset: 0x00023BBD
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

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x000259DD File Offset: 0x00023BDD
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this);
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x000259F6 File Offset: 0x00023BF6
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

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00025A28 File Offset: 0x00023C28
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

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00025A92 File Offset: 0x00023C92
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

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x00025AC4 File Offset: 0x00023CC4
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

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x00025B30 File Offset: 0x00023D30
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x00025B8B File Offset: 0x00023D8B
		public MethodDefinition EntryPoint
		{
			get
			{
				if (this.entry_point != null)
				{
					return this.entry_point;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, MethodDefinition>(ref this.entry_point, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadEntryPoint());
				}
				return this.entry_point = null;
			}
			set
			{
				this.entry_point = value;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00025B94 File Offset: 0x00023D94
		public bool HasCustomDebugInformations
		{
			get
			{
				return this.custom_infos != null && this.custom_infos.Count > 0;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x00025BAE File Offset: 0x00023DAE
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

		// Token: 0x06000A73 RID: 2675 RVA: 0x00025BD0 File Offset: 0x00023DD0
		internal ModuleDefinition()
		{
			this.MetadataSystem = new MetadataSystem();
			this.token = new MetadataToken(TokenType.Module, 1);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00025C0C File Offset: 0x00023E0C
		internal ModuleDefinition(Image image)
			: this()
		{
			this.Image = image;
			this.kind = image.Kind;
			this.RuntimeVersion = image.RuntimeVersion;
			this.architecture = image.Architecture;
			this.attributes = image.Attributes;
			this.characteristics = image.Characteristics;
			this.linker_version = image.LinkerVersion;
			this.subsystem_major = image.SubSystemMajor;
			this.subsystem_minor = image.SubSystemMinor;
			this.file_name = image.FileName;
			this.timestamp = image.Timestamp;
			this.reader = new MetadataReader(this);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00025CAA File Offset: 0x00023EAA
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

		// Token: 0x06000A76 RID: 2678 RVA: 0x00025CEA File Offset: 0x00023EEA
		public bool HasTypeReference(string fullName)
		{
			return this.HasTypeReference(string.Empty, fullName);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00025CF8 File Offset: 0x00023EF8
		public bool HasTypeReference(string scope, string fullName)
		{
			Mixin.CheckFullName(fullName);
			return this.HasImage && this.GetTypeReference(scope, fullName) != null;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00025D15 File Offset: 0x00023F15
		public bool TryGetTypeReference(string fullName, out TypeReference type)
		{
			return this.TryGetTypeReference(string.Empty, fullName, out type);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00025D24 File Offset: 0x00023F24
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

		// Token: 0x06000A7A RID: 2682 RVA: 0x00025D54 File Offset: 0x00023F54
		private TypeReference GetTypeReference(string scope, string fullname)
		{
			return this.Read<Row<string, string>, TypeReference>(new Row<string, string>(scope, fullname), (Row<string, string> row, MetadataReader reader) => reader.GetTypeReference(row.Col1, row.Col2));
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00025D82 File Offset: 0x00023F82
		public IEnumerable<TypeReference> GetTypeReferences()
		{
			if (!this.HasImage)
			{
				return Empty<TypeReference>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<TypeReference>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetTypeReferences());
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00025DB8 File Offset: 0x00023FB8
		public IEnumerable<MemberReference> GetMemberReferences()
		{
			if (!this.HasImage)
			{
				return Empty<MemberReference>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<MemberReference>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetMemberReferences());
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00025DEE File Offset: 0x00023FEE
		public IEnumerable<CustomAttribute> GetCustomAttributes()
		{
			if (!this.HasImage)
			{
				return Empty<CustomAttribute>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<CustomAttribute>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetCustomAttributes());
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00025E24 File Offset: 0x00024024
		public TypeReference GetType(string fullName, bool runtimeName)
		{
			if (!runtimeName)
			{
				return this.GetType(fullName);
			}
			return TypeParser.ParseType(this, fullName, true);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00025E39 File Offset: 0x00024039
		public TypeDefinition GetType(string fullName)
		{
			Mixin.CheckFullName(fullName);
			if (fullName.IndexOf('/') > 0)
			{
				return this.GetNestedType(fullName);
			}
			return ((TypeDefinitionCollection)this.Types).GetType(fullName);
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00025E65 File Offset: 0x00024065
		public TypeDefinition GetType(string @namespace, string name)
		{
			Mixin.CheckName(name);
			return ((TypeDefinitionCollection)this.Types).GetType(@namespace ?? string.Empty, name);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00025E88 File Offset: 0x00024088
		public IEnumerable<TypeDefinition> GetTypes()
		{
			return ModuleDefinition.GetTypes(this.Types);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00025E95 File Offset: 0x00024095
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

		// Token: 0x06000A83 RID: 2691 RVA: 0x00025EA8 File Offset: 0x000240A8
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

		// Token: 0x06000A84 RID: 2692 RVA: 0x00025EF6 File Offset: 0x000240F6
		internal FieldDefinition Resolve(FieldReference field)
		{
			return this.MetadataResolver.Resolve(field);
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00025F04 File Offset: 0x00024104
		internal MethodDefinition Resolve(MethodReference method)
		{
			return this.MetadataResolver.Resolve(method);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00025F12 File Offset: 0x00024112
		internal TypeDefinition Resolve(TypeReference type)
		{
			return this.MetadataResolver.Resolve(type);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00025F20 File Offset: 0x00024120
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

		// Token: 0x06000A88 RID: 2696 RVA: 0x00025F35 File Offset: 0x00024135
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(Type type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00025F35 File Offset: 0x00024135
		public TypeReference ImportReference(Type type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00025F3F File Offset: 0x0002413F
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(Type type, IGenericParameterProvider context)
		{
			return this.ImportReference(type, context);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00025F49 File Offset: 0x00024149
		public TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			Mixin.CheckType(type);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(type, context);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00025F65 File Offset: 0x00024165
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldInfo field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00025F6F File Offset: 0x0002416F
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldInfo field, IGenericParameterProvider context)
		{
			return this.ImportReference(field, context);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00025F65 File Offset: 0x00024165
		public FieldReference ImportReference(FieldInfo field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00025F79 File Offset: 0x00024179
		public FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			Mixin.CheckField(field);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(field, context);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00025F95 File Offset: 0x00024195
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodBase method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00025F9F File Offset: 0x0002419F
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodBase method, IGenericParameterProvider context)
		{
			return this.ImportReference(method, context);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00025F95 File Offset: 0x00024195
		public MethodReference ImportReference(MethodBase method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00025FA9 File Offset: 0x000241A9
		public MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			Mixin.CheckMethod(method);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(method, context);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00025FC5 File Offset: 0x000241C5
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(TypeReference type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00025FCF File Offset: 0x000241CF
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(TypeReference type, IGenericParameterProvider context)
		{
			return this.ImportReference(type, context);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00025FC5 File Offset: 0x000241C5
		public TypeReference ImportReference(TypeReference type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00025FD9 File Offset: 0x000241D9
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

		// Token: 0x06000A98 RID: 2712 RVA: 0x00026000 File Offset: 0x00024200
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldReference field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0002600A File Offset: 0x0002420A
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldReference field, IGenericParameterProvider context)
		{
			return this.ImportReference(field, context);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00026000 File Offset: 0x00024200
		public FieldReference ImportReference(FieldReference field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00026014 File Offset: 0x00024214
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

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002603B File Offset: 0x0002423B
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodReference method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00026045 File Offset: 0x00024245
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodReference method, IGenericParameterProvider context)
		{
			return this.ImportReference(method, context);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0002603B File Offset: 0x0002423B
		public MethodReference ImportReference(MethodReference method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0002604F File Offset: 0x0002424F
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

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00026076 File Offset: 0x00024276
		public IMetadataTokenProvider LookupToken(int token)
		{
			return this.LookupToken(new MetadataToken((uint)token));
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00026084 File Offset: 0x00024284
		public IMetadataTokenProvider LookupToken(MetadataToken token)
		{
			return this.Read<MetadataToken, IMetadataTokenProvider>(token, (MetadataToken t, MetadataReader reader) => reader.LookupToken(t));
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x000260AC File Offset: 0x000242AC
		internal object SyncRoot
		{
			get
			{
				return this.module_lock;
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000260B4 File Offset: 0x000242B4
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

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002612C File Offset: 0x0002432C
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

		// Token: 0x06000AA5 RID: 2725 RVA: 0x000261A8 File Offset: 0x000243A8
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

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x0002624C File Offset: 0x0002444C
		public bool HasDebugHeader
		{
			get
			{
				return this.Image != null && this.Image.DebugHeader != null;
			}
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00026266 File Offset: 0x00024466
		public ImageDebugHeader GetDebugHeader()
		{
			return this.Image.DebugHeader ?? new ImageDebugHeader();
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0002627C File Offset: 0x0002447C
		public static ModuleDefinition CreateModule(string name, ModuleKind kind)
		{
			return ModuleDefinition.CreateModule(name, new ModuleParameters
			{
				Kind = kind
			});
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00026290 File Offset: 0x00024490
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

		// Token: 0x06000AAA RID: 2730 RVA: 0x000263C6 File Offset: 0x000245C6
		private static AssemblyNameDefinition CreateAssemblyName(string name)
		{
			if (name.EndsWith(".dll") || name.EndsWith(".exe"))
			{
				name = name.Substring(0, name.Length - 4);
			}
			return new AssemblyNameDefinition(name, Mixin.ZeroVersion);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00026400 File Offset: 0x00024600
		public void ReadSymbols()
		{
			if (string.IsNullOrEmpty(this.file_name))
			{
				throw new InvalidOperationException();
			}
			DefaultSymbolReaderProvider defaultSymbolReaderProvider = new DefaultSymbolReaderProvider(true);
			this.ReadSymbols(defaultSymbolReaderProvider.GetSymbolReader(this, this.file_name), true);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0002643B File Offset: 0x0002463B
		public void ReadSymbols(ISymbolReader reader)
		{
			this.ReadSymbols(reader, true);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00026448 File Offset: 0x00024648
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

		// Token: 0x06000AAE RID: 2734 RVA: 0x000264B5 File Offset: 0x000246B5
		public static ModuleDefinition ReadModule(string fileName)
		{
			return ModuleDefinition.ReadModule(fileName, new ReaderParameters(ReadingMode.Deferred));
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000264C4 File Offset: 0x000246C4
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

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002655C File Offset: 0x0002475C
		private static Stream GetFileStream(string fileName, FileMode mode, FileAccess access, FileShare share)
		{
			Mixin.CheckFileName(fileName);
			return new FileStream(fileName, mode, access, share);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002656D File Offset: 0x0002476D
		public static ModuleDefinition ReadModule(Stream stream)
		{
			return ModuleDefinition.ReadModule(stream, new ReaderParameters(ReadingMode.Deferred));
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0002657B File Offset: 0x0002477B
		public static ModuleDefinition ReadModule(Stream stream, ReaderParameters parameters)
		{
			Mixin.CheckStream(stream);
			Mixin.CheckReadSeek(stream);
			return ModuleDefinition.ReadModule(Disposable.NotOwned<Stream>(stream), stream.GetFileName(), parameters);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0002659B File Offset: 0x0002479B
		private static ModuleDefinition ReadModule(Disposable<Stream> stream, string fileName, ReaderParameters parameters)
		{
			Mixin.CheckParameters(parameters);
			return ModuleReader.CreateModule(ImageReader.ReadImage(stream, fileName), parameters);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x000265B0 File Offset: 0x000247B0
		public void Write(string fileName)
		{
			this.Write(fileName, new WriterParameters());
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x000265C0 File Offset: 0x000247C0
		public void Write(string fileName, WriterParameters parameters)
		{
			Mixin.CheckParameters(parameters);
			Stream fileStream = ModuleDefinition.GetFileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
			ModuleWriter.WriteModule(this, Disposable.Owned<Stream>(fileStream), parameters);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000265EA File Offset: 0x000247EA
		public void Write()
		{
			this.Write(new WriterParameters());
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000265F7 File Offset: 0x000247F7
		public void Write(WriterParameters parameters)
		{
			if (!this.HasImage)
			{
				throw new InvalidOperationException();
			}
			this.Write(this.Image.Stream.value, parameters);
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0002661E File Offset: 0x0002481E
		public void Write(Stream stream)
		{
			this.Write(stream, new WriterParameters());
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0002662C File Offset: 0x0002482C
		public void Write(Stream stream, WriterParameters parameters)
		{
			Mixin.CheckStream(stream);
			Mixin.CheckWriteSeek(stream);
			Mixin.CheckParameters(parameters);
			ModuleWriter.WriteModule(this, Disposable.NotOwned<Stream>(stream), parameters);
		}

		// Token: 0x040003C6 RID: 966
		internal Image Image;

		// Token: 0x040003C7 RID: 967
		internal MetadataSystem MetadataSystem;

		// Token: 0x040003C8 RID: 968
		internal ReadingMode ReadingMode;

		// Token: 0x040003C9 RID: 969
		internal ISymbolReaderProvider SymbolReaderProvider;

		// Token: 0x040003CA RID: 970
		internal ISymbolReader symbol_reader;

		// Token: 0x040003CB RID: 971
		internal Disposable<IAssemblyResolver> assembly_resolver;

		// Token: 0x040003CC RID: 972
		internal IMetadataResolver metadata_resolver;

		// Token: 0x040003CD RID: 973
		internal TypeSystem type_system;

		// Token: 0x040003CE RID: 974
		internal readonly MetadataReader reader;

		// Token: 0x040003CF RID: 975
		private readonly string file_name;

		// Token: 0x040003D0 RID: 976
		internal string runtime_version;

		// Token: 0x040003D1 RID: 977
		internal ModuleKind kind;

		// Token: 0x040003D2 RID: 978
		private WindowsRuntimeProjections projections;

		// Token: 0x040003D3 RID: 979
		private MetadataKind metadata_kind;

		// Token: 0x040003D4 RID: 980
		private TargetRuntime runtime;

		// Token: 0x040003D5 RID: 981
		private TargetArchitecture architecture;

		// Token: 0x040003D6 RID: 982
		private ModuleAttributes attributes;

		// Token: 0x040003D7 RID: 983
		private ModuleCharacteristics characteristics;

		// Token: 0x040003D8 RID: 984
		private Guid mvid;

		// Token: 0x040003D9 RID: 985
		internal ushort linker_version = 8;

		// Token: 0x040003DA RID: 986
		internal ushort subsystem_major = 4;

		// Token: 0x040003DB RID: 987
		internal ushort subsystem_minor;

		// Token: 0x040003DC RID: 988
		internal uint timestamp;

		// Token: 0x040003DD RID: 989
		internal AssemblyDefinition assembly;

		// Token: 0x040003DE RID: 990
		private MethodDefinition entry_point;

		// Token: 0x040003DF RID: 991
		internal IReflectionImporter reflection_importer;

		// Token: 0x040003E0 RID: 992
		internal IMetadataImporter metadata_importer;

		// Token: 0x040003E1 RID: 993
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040003E2 RID: 994
		private Collection<AssemblyNameReference> references;

		// Token: 0x040003E3 RID: 995
		private Collection<ModuleReference> modules;

		// Token: 0x040003E4 RID: 996
		private Collection<Resource> resources;

		// Token: 0x040003E5 RID: 997
		private Collection<ExportedType> exported_types;

		// Token: 0x040003E6 RID: 998
		private TypeDefinitionCollection types;

		// Token: 0x040003E7 RID: 999
		internal Collection<CustomDebugInformation> custom_infos;

		// Token: 0x040003E8 RID: 1000
		internal MetadataBuilder metadata_builder;

		// Token: 0x040003E9 RID: 1001
		private readonly object module_lock = new object();
	}
}
