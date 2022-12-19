using System;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000277 RID: 631
	public abstract class TypeSystem
	{
		// Token: 0x06000FC8 RID: 4040 RVA: 0x0003077E File Offset: 0x0002E97E
		private TypeSystem(ModuleDefinition module)
		{
			this.module = module;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0003078D File Offset: 0x0002E98D
		internal static TypeSystem CreateTypeSystem(ModuleDefinition module)
		{
			if (module.IsCoreLibrary())
			{
				return new TypeSystem.CoreTypeSystem(module);
			}
			return new TypeSystem.CommonTypeSystem(module);
		}

		// Token: 0x06000FCA RID: 4042
		internal abstract TypeReference LookupType(string @namespace, string name);

		// Token: 0x06000FCB RID: 4043 RVA: 0x000307A4 File Offset: 0x0002E9A4
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

		// Token: 0x06000FCC RID: 4044 RVA: 0x0003080C File Offset: 0x0002EA0C
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

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00030878 File Offset: 0x0002EA78
		[Obsolete("Use CoreLibrary")]
		public IMetadataScope Corlib
		{
			get
			{
				return this.CoreLibrary;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00030880 File Offset: 0x0002EA80
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

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000FCF RID: 4047 RVA: 0x000308A4 File Offset: 0x0002EAA4
		public TypeReference Object
		{
			get
			{
				return this.type_object ?? this.LookupSystemType(ref this.type_object, "Object", ElementType.Object);
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x000308C3 File Offset: 0x0002EAC3
		public TypeReference Void
		{
			get
			{
				return this.type_void ?? this.LookupSystemType(ref this.type_void, "Void", ElementType.Void);
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x000308E1 File Offset: 0x0002EAE1
		public TypeReference Boolean
		{
			get
			{
				return this.type_bool ?? this.LookupSystemValueType(ref this.type_bool, "Boolean", ElementType.Boolean);
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x000308FF File Offset: 0x0002EAFF
		public TypeReference Char
		{
			get
			{
				return this.type_char ?? this.LookupSystemValueType(ref this.type_char, "Char", ElementType.Char);
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x0003091D File Offset: 0x0002EB1D
		public TypeReference SByte
		{
			get
			{
				return this.type_sbyte ?? this.LookupSystemValueType(ref this.type_sbyte, "SByte", ElementType.I1);
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0003093B File Offset: 0x0002EB3B
		public TypeReference Byte
		{
			get
			{
				return this.type_byte ?? this.LookupSystemValueType(ref this.type_byte, "Byte", ElementType.U1);
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x00030959 File Offset: 0x0002EB59
		public TypeReference Int16
		{
			get
			{
				return this.type_int16 ?? this.LookupSystemValueType(ref this.type_int16, "Int16", ElementType.I2);
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x00030977 File Offset: 0x0002EB77
		public TypeReference UInt16
		{
			get
			{
				return this.type_uint16 ?? this.LookupSystemValueType(ref this.type_uint16, "UInt16", ElementType.U2);
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00030995 File Offset: 0x0002EB95
		public TypeReference Int32
		{
			get
			{
				return this.type_int32 ?? this.LookupSystemValueType(ref this.type_int32, "Int32", ElementType.I4);
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x000309B3 File Offset: 0x0002EBB3
		public TypeReference UInt32
		{
			get
			{
				return this.type_uint32 ?? this.LookupSystemValueType(ref this.type_uint32, "UInt32", ElementType.U4);
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x000309D2 File Offset: 0x0002EBD2
		public TypeReference Int64
		{
			get
			{
				return this.type_int64 ?? this.LookupSystemValueType(ref this.type_int64, "Int64", ElementType.I8);
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x000309F1 File Offset: 0x0002EBF1
		public TypeReference UInt64
		{
			get
			{
				return this.type_uint64 ?? this.LookupSystemValueType(ref this.type_uint64, "UInt64", ElementType.U8);
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x00030A10 File Offset: 0x0002EC10
		public TypeReference Single
		{
			get
			{
				return this.type_single ?? this.LookupSystemValueType(ref this.type_single, "Single", ElementType.R4);
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x00030A2F File Offset: 0x0002EC2F
		public TypeReference Double
		{
			get
			{
				return this.type_double ?? this.LookupSystemValueType(ref this.type_double, "Double", ElementType.R8);
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x00030A4E File Offset: 0x0002EC4E
		public TypeReference IntPtr
		{
			get
			{
				return this.type_intptr ?? this.LookupSystemValueType(ref this.type_intptr, "IntPtr", ElementType.I);
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x00030A6D File Offset: 0x0002EC6D
		public TypeReference UIntPtr
		{
			get
			{
				return this.type_uintptr ?? this.LookupSystemValueType(ref this.type_uintptr, "UIntPtr", ElementType.U);
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000FDF RID: 4063 RVA: 0x00030A8C File Offset: 0x0002EC8C
		public TypeReference String
		{
			get
			{
				return this.type_string ?? this.LookupSystemType(ref this.type_string, "String", ElementType.String);
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x00030AAB File Offset: 0x0002ECAB
		public TypeReference TypedReference
		{
			get
			{
				return this.type_typedref ?? this.LookupSystemValueType(ref this.type_typedref, "TypedReference", ElementType.TypedByRef);
			}
		}

		// Token: 0x04000576 RID: 1398
		private readonly ModuleDefinition module;

		// Token: 0x04000577 RID: 1399
		private TypeReference type_object;

		// Token: 0x04000578 RID: 1400
		private TypeReference type_void;

		// Token: 0x04000579 RID: 1401
		private TypeReference type_bool;

		// Token: 0x0400057A RID: 1402
		private TypeReference type_char;

		// Token: 0x0400057B RID: 1403
		private TypeReference type_sbyte;

		// Token: 0x0400057C RID: 1404
		private TypeReference type_byte;

		// Token: 0x0400057D RID: 1405
		private TypeReference type_int16;

		// Token: 0x0400057E RID: 1406
		private TypeReference type_uint16;

		// Token: 0x0400057F RID: 1407
		private TypeReference type_int32;

		// Token: 0x04000580 RID: 1408
		private TypeReference type_uint32;

		// Token: 0x04000581 RID: 1409
		private TypeReference type_int64;

		// Token: 0x04000582 RID: 1410
		private TypeReference type_uint64;

		// Token: 0x04000583 RID: 1411
		private TypeReference type_single;

		// Token: 0x04000584 RID: 1412
		private TypeReference type_double;

		// Token: 0x04000585 RID: 1413
		private TypeReference type_intptr;

		// Token: 0x04000586 RID: 1414
		private TypeReference type_uintptr;

		// Token: 0x04000587 RID: 1415
		private TypeReference type_string;

		// Token: 0x04000588 RID: 1416
		private TypeReference type_typedref;

		// Token: 0x02000278 RID: 632
		private sealed class CoreTypeSystem : TypeSystem
		{
			// Token: 0x06000FE1 RID: 4065 RVA: 0x00030ACA File Offset: 0x0002ECCA
			public CoreTypeSystem(ModuleDefinition module)
				: base(module)
			{
			}

			// Token: 0x06000FE2 RID: 4066 RVA: 0x00030AD4 File Offset: 0x0002ECD4
			internal override TypeReference LookupType(string @namespace, string name)
			{
				TypeReference typeReference = this.LookupTypeDefinition(@namespace, name) ?? this.LookupTypeForwarded(@namespace, name);
				if (typeReference != null)
				{
					return typeReference;
				}
				throw new NotSupportedException();
			}

			// Token: 0x06000FE3 RID: 4067 RVA: 0x00030B00 File Offset: 0x0002ED00
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

			// Token: 0x06000FE4 RID: 4068 RVA: 0x00030B60 File Offset: 0x0002ED60
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

			// Token: 0x06000FE5 RID: 4069 RVA: 0x00018105 File Offset: 0x00016305
			private static void Initialize(object obj)
			{
			}
		}

		// Token: 0x0200027A RID: 634
		private sealed class CommonTypeSystem : TypeSystem
		{
			// Token: 0x06000FE9 RID: 4073 RVA: 0x00030ACA File Offset: 0x0002ECCA
			public CommonTypeSystem(ModuleDefinition module)
				: base(module)
			{
			}

			// Token: 0x06000FEA RID: 4074 RVA: 0x00030C39 File Offset: 0x0002EE39
			internal override TypeReference LookupType(string @namespace, string name)
			{
				return this.CreateTypeReference(@namespace, name);
			}

			// Token: 0x06000FEB RID: 4075 RVA: 0x00030C44 File Offset: 0x0002EE44
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

			// Token: 0x06000FEC RID: 4076 RVA: 0x00030CD4 File Offset: 0x0002EED4
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

			// Token: 0x06000FED RID: 4077 RVA: 0x00030D28 File Offset: 0x0002EF28
			private TypeReference CreateTypeReference(string @namespace, string name)
			{
				return new TypeReference(@namespace, name, this.module, this.GetCoreLibraryReference());
			}

			// Token: 0x0400058B RID: 1419
			private AssemblyNameReference core_library;
		}
	}
}
