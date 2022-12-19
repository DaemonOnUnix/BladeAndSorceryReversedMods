using System;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000181 RID: 385
	public abstract class TypeSystem
	{
		// Token: 0x06000C65 RID: 3173 RVA: 0x000294AD File Offset: 0x000276AD
		private TypeSystem(ModuleDefinition module)
		{
			this.module = module;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x000294BC File Offset: 0x000276BC
		internal static TypeSystem CreateTypeSystem(ModuleDefinition module)
		{
			if (module.IsCoreLibrary())
			{
				return new TypeSystem.CoreTypeSystem(module);
			}
			return new TypeSystem.CommonTypeSystem(module);
		}

		// Token: 0x06000C67 RID: 3175
		internal abstract TypeReference LookupType(string @namespace, string name);

		// Token: 0x06000C68 RID: 3176 RVA: 0x000294D4 File Offset: 0x000276D4
		private TypeReference LookupSystemType(ref TypeReference reference, string name, ElementType element_type)
		{
			object syncRoot = this.module.SyncRoot;
			TypeReference typeReference;
			lock (syncRoot)
			{
				if (reference != null)
				{
					typeReference = reference;
				}
				else
				{
					TypeReference typeReference2 = this.LookupType("System", name);
					typeReference2.etype = element_type;
					TypeReference typeReference3;
					reference = (typeReference3 = typeReference2);
					typeReference = typeReference3;
				}
			}
			return typeReference;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0002953C File Offset: 0x0002773C
		private TypeReference LookupSystemValueType(ref TypeReference typeRef, string name, ElementType element_type)
		{
			object syncRoot = this.module.SyncRoot;
			TypeReference typeReference;
			lock (syncRoot)
			{
				if (typeRef != null)
				{
					typeReference = typeRef;
				}
				else
				{
					TypeReference typeReference2 = this.LookupType("System", name);
					typeReference2.etype = element_type;
					typeReference2.KnownValueType();
					TypeReference typeReference3;
					typeRef = (typeReference3 = typeReference2);
					typeReference = typeReference3;
				}
			}
			return typeReference;
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x000295A8 File Offset: 0x000277A8
		[Obsolete("Use CoreLibrary")]
		public IMetadataScope Corlib
		{
			get
			{
				return this.CoreLibrary;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x000295B0 File Offset: 0x000277B0
		public IMetadataScope CoreLibrary
		{
			get
			{
				TypeSystem.CommonTypeSystem commonTypeSystem = this as TypeSystem.CommonTypeSystem;
				if (commonTypeSystem == null)
				{
					return this.module;
				}
				return commonTypeSystem.GetCoreLibraryReference();
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x000295D4 File Offset: 0x000277D4
		public TypeReference Object
		{
			get
			{
				return this.type_object ?? this.LookupSystemType(ref this.type_object, "Object", ElementType.Object);
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x000295F3 File Offset: 0x000277F3
		public TypeReference Void
		{
			get
			{
				return this.type_void ?? this.LookupSystemType(ref this.type_void, "Void", ElementType.Void);
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x00029611 File Offset: 0x00027811
		public TypeReference Boolean
		{
			get
			{
				return this.type_bool ?? this.LookupSystemValueType(ref this.type_bool, "Boolean", ElementType.Boolean);
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0002962F File Offset: 0x0002782F
		public TypeReference Char
		{
			get
			{
				return this.type_char ?? this.LookupSystemValueType(ref this.type_char, "Char", ElementType.Char);
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x0002964D File Offset: 0x0002784D
		public TypeReference SByte
		{
			get
			{
				return this.type_sbyte ?? this.LookupSystemValueType(ref this.type_sbyte, "SByte", ElementType.I1);
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x0002966B File Offset: 0x0002786B
		public TypeReference Byte
		{
			get
			{
				return this.type_byte ?? this.LookupSystemValueType(ref this.type_byte, "Byte", ElementType.U1);
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00029689 File Offset: 0x00027889
		public TypeReference Int16
		{
			get
			{
				return this.type_int16 ?? this.LookupSystemValueType(ref this.type_int16, "Int16", ElementType.I2);
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x000296A7 File Offset: 0x000278A7
		public TypeReference UInt16
		{
			get
			{
				return this.type_uint16 ?? this.LookupSystemValueType(ref this.type_uint16, "UInt16", ElementType.U2);
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x000296C5 File Offset: 0x000278C5
		public TypeReference Int32
		{
			get
			{
				return this.type_int32 ?? this.LookupSystemValueType(ref this.type_int32, "Int32", ElementType.I4);
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x000296E3 File Offset: 0x000278E3
		public TypeReference UInt32
		{
			get
			{
				return this.type_uint32 ?? this.LookupSystemValueType(ref this.type_uint32, "UInt32", ElementType.U4);
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00029702 File Offset: 0x00027902
		public TypeReference Int64
		{
			get
			{
				return this.type_int64 ?? this.LookupSystemValueType(ref this.type_int64, "Int64", ElementType.I8);
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00029721 File Offset: 0x00027921
		public TypeReference UInt64
		{
			get
			{
				return this.type_uint64 ?? this.LookupSystemValueType(ref this.type_uint64, "UInt64", ElementType.U8);
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00029740 File Offset: 0x00027940
		public TypeReference Single
		{
			get
			{
				return this.type_single ?? this.LookupSystemValueType(ref this.type_single, "Single", ElementType.R4);
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x0002975F File Offset: 0x0002795F
		public TypeReference Double
		{
			get
			{
				return this.type_double ?? this.LookupSystemValueType(ref this.type_double, "Double", ElementType.R8);
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x0002977E File Offset: 0x0002797E
		public TypeReference IntPtr
		{
			get
			{
				return this.type_intptr ?? this.LookupSystemValueType(ref this.type_intptr, "IntPtr", ElementType.I);
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x0002979D File Offset: 0x0002799D
		public TypeReference UIntPtr
		{
			get
			{
				return this.type_uintptr ?? this.LookupSystemValueType(ref this.type_uintptr, "UIntPtr", ElementType.U);
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x000297BC File Offset: 0x000279BC
		public TypeReference String
		{
			get
			{
				return this.type_string ?? this.LookupSystemType(ref this.type_string, "String", ElementType.String);
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x000297DB File Offset: 0x000279DB
		public TypeReference TypedReference
		{
			get
			{
				return this.type_typedref ?? this.LookupSystemValueType(ref this.type_typedref, "TypedReference", ElementType.TypedByRef);
			}
		}

		// Token: 0x0400053E RID: 1342
		private readonly ModuleDefinition module;

		// Token: 0x0400053F RID: 1343
		private TypeReference type_object;

		// Token: 0x04000540 RID: 1344
		private TypeReference type_void;

		// Token: 0x04000541 RID: 1345
		private TypeReference type_bool;

		// Token: 0x04000542 RID: 1346
		private TypeReference type_char;

		// Token: 0x04000543 RID: 1347
		private TypeReference type_sbyte;

		// Token: 0x04000544 RID: 1348
		private TypeReference type_byte;

		// Token: 0x04000545 RID: 1349
		private TypeReference type_int16;

		// Token: 0x04000546 RID: 1350
		private TypeReference type_uint16;

		// Token: 0x04000547 RID: 1351
		private TypeReference type_int32;

		// Token: 0x04000548 RID: 1352
		private TypeReference type_uint32;

		// Token: 0x04000549 RID: 1353
		private TypeReference type_int64;

		// Token: 0x0400054A RID: 1354
		private TypeReference type_uint64;

		// Token: 0x0400054B RID: 1355
		private TypeReference type_single;

		// Token: 0x0400054C RID: 1356
		private TypeReference type_double;

		// Token: 0x0400054D RID: 1357
		private TypeReference type_intptr;

		// Token: 0x0400054E RID: 1358
		private TypeReference type_uintptr;

		// Token: 0x0400054F RID: 1359
		private TypeReference type_string;

		// Token: 0x04000550 RID: 1360
		private TypeReference type_typedref;

		// Token: 0x02000182 RID: 386
		private sealed class CoreTypeSystem : TypeSystem
		{
			// Token: 0x06000C7E RID: 3198 RVA: 0x000297FA File Offset: 0x000279FA
			public CoreTypeSystem(ModuleDefinition module)
				: base(module)
			{
			}

			// Token: 0x06000C7F RID: 3199 RVA: 0x00029804 File Offset: 0x00027A04
			internal override TypeReference LookupType(string @namespace, string name)
			{
				TypeReference typeReference = this.LookupTypeDefinition(@namespace, name) ?? this.LookupTypeForwarded(@namespace, name);
				if (typeReference != null)
				{
					return typeReference;
				}
				throw new NotSupportedException();
			}

			// Token: 0x06000C80 RID: 3200 RVA: 0x00029830 File Offset: 0x00027A30
			private TypeReference LookupTypeDefinition(string @namespace, string name)
			{
				if (this.module.MetadataSystem.Types == null)
				{
					TypeSystem.CoreTypeSystem.Initialize(this.module.Types);
				}
				return this.module.Read<Row<string, string>, TypeDefinition>(new Row<string, string>(@namespace, name), delegate(Row<string, string> row, MetadataReader reader)
				{
					TypeDefinition[] types = reader.metadata.Types;
					for (int i = 0; i < types.Length; i++)
					{
						if (types[i] == null)
						{
							types[i] = reader.GetTypeDefinition((uint)(i + 1));
						}
						TypeDefinition typeDefinition = types[i];
						if (typeDefinition.Name == row.Col2 && typeDefinition.Namespace == row.Col1)
						{
							return typeDefinition;
						}
					}
					return null;
				});
			}

			// Token: 0x06000C81 RID: 3201 RVA: 0x00029890 File Offset: 0x00027A90
			private TypeReference LookupTypeForwarded(string @namespace, string name)
			{
				if (!this.module.HasExportedTypes)
				{
					return null;
				}
				Collection<ExportedType> exportedTypes = this.module.ExportedTypes;
				for (int i = 0; i < exportedTypes.Count; i++)
				{
					ExportedType exportedType = exportedTypes[i];
					if (exportedType.Name == name && exportedType.Namespace == @namespace)
					{
						return exportedType.CreateReference();
					}
				}
				return null;
			}

			// Token: 0x06000C82 RID: 3202 RVA: 0x00012279 File Offset: 0x00010479
			private static void Initialize(object obj)
			{
			}
		}

		// Token: 0x02000184 RID: 388
		private sealed class CommonTypeSystem : TypeSystem
		{
			// Token: 0x06000C86 RID: 3206 RVA: 0x000297FA File Offset: 0x000279FA
			public CommonTypeSystem(ModuleDefinition module)
				: base(module)
			{
			}

			// Token: 0x06000C87 RID: 3207 RVA: 0x00029969 File Offset: 0x00027B69
			internal override TypeReference LookupType(string @namespace, string name)
			{
				return this.CreateTypeReference(@namespace, name);
			}

			// Token: 0x06000C88 RID: 3208 RVA: 0x00029974 File Offset: 0x00027B74
			public AssemblyNameReference GetCoreLibraryReference()
			{
				if (this.core_library != null)
				{
					return this.core_library;
				}
				if (this.module.TryGetCoreLibraryReference(out this.core_library))
				{
					return this.core_library;
				}
				this.core_library = new AssemblyNameReference
				{
					Name = "mscorlib",
					Version = this.GetCorlibVersion(),
					PublicKeyToken = new byte[] { 183, 122, 92, 86, 25, 52, 224, 137 }
				};
				this.module.AssemblyReferences.Add(this.core_library);
				return this.core_library;
			}

			// Token: 0x06000C89 RID: 3209 RVA: 0x00029A04 File Offset: 0x00027C04
			private Version GetCorlibVersion()
			{
				switch (this.module.Runtime)
				{
				case TargetRuntime.Net_1_0:
				case TargetRuntime.Net_1_1:
					return new Version(1, 0, 0, 0);
				case TargetRuntime.Net_2_0:
					return new Version(2, 0, 0, 0);
				case TargetRuntime.Net_4_0:
					return new Version(4, 0, 0, 0);
				default:
					throw new NotSupportedException();
				}
			}

			// Token: 0x06000C8A RID: 3210 RVA: 0x00029A58 File Offset: 0x00027C58
			private TypeReference CreateTypeReference(string @namespace, string name)
			{
				return new TypeReference(@namespace, name, this.module, this.GetCoreLibraryReference());
			}

			// Token: 0x04000553 RID: 1363
			private AssemblyNameReference core_library;
		}
	}
}
