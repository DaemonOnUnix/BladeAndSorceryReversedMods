using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200010F RID: 271
	public sealed class FieldDefinition : FieldReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider, IMarshalInfoProvider
	{
		// Token: 0x06000785 RID: 1925 RVA: 0x00020E7C File Offset: 0x0001F07C
		private void ResolveLayout()
		{
			if (this.offset != -2)
			{
				return;
			}
			if (!base.HasImage)
			{
				this.offset = -1;
				return;
			}
			object syncRoot = this.Module.SyncRoot;
			lock (syncRoot)
			{
				if (this.offset == -2)
				{
					this.offset = this.Module.Read<FieldDefinition, int>(this, (FieldDefinition field, MetadataReader reader) => reader.ReadFieldLayout(field));
				}
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x00020F14 File Offset: 0x0001F114
		public bool HasLayoutInfo
		{
			get
			{
				if (this.offset >= 0)
				{
					return true;
				}
				this.ResolveLayout();
				return this.offset >= 0;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x00020F33 File Offset: 0x0001F133
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x00020F5C File Offset: 0x0001F15C
		public int Offset
		{
			get
			{
				if (this.offset >= 0)
				{
					return this.offset;
				}
				this.ResolveLayout();
				if (this.offset < 0)
				{
					return -1;
				}
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x00020F65 File Offset: 0x0001F165
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x00020F72 File Offset: 0x0001F172
		internal new FieldDefinitionProjection WindowsRuntimeProjection
		{
			get
			{
				return (FieldDefinitionProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00020F7C File Offset: 0x0001F17C
		private void ResolveRVA()
		{
			if (this.rva != -2)
			{
				return;
			}
			if (!base.HasImage)
			{
				return;
			}
			object syncRoot = this.Module.SyncRoot;
			lock (syncRoot)
			{
				if (this.rva == -2)
				{
					this.rva = this.Module.Read<FieldDefinition, int>(this, (FieldDefinition field, MetadataReader reader) => reader.ReadFieldRVA(field));
				}
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0002100C File Offset: 0x0001F20C
		public int RVA
		{
			get
			{
				if (this.rva > 0)
				{
					return this.rva;
				}
				this.ResolveRVA();
				if (this.rva <= 0)
				{
					return 0;
				}
				return this.rva;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x00021035 File Offset: 0x0001F235
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x00021065 File Offset: 0x0001F265
		public byte[] InitialValue
		{
			get
			{
				if (this.initial_value != null)
				{
					return this.initial_value;
				}
				this.ResolveRVA();
				if (this.initial_value == null)
				{
					this.initial_value = Empty<byte>.Array;
				}
				return this.initial_value;
			}
			set
			{
				this.initial_value = value;
				this.rva = 0;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x00021075 File Offset: 0x0001F275
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x0002107D File Offset: 0x0001F27D
		public FieldAttributes Attributes
		{
			get
			{
				return (FieldAttributes)this.attributes;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != (FieldAttributes)this.attributes)
				{
					throw new InvalidOperationException();
				}
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0002109D File Offset: 0x0001F29D
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x000210C1 File Offset: 0x0001F2C1
		public bool HasConstant
		{
			get
			{
				this.ResolveConstant(ref this.constant, this.Module);
				return this.constant != Mixin.NoValue;
			}
			set
			{
				if (!value)
				{
					this.constant = Mixin.NoValue;
				}
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x000210D1 File Offset: 0x0001F2D1
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x000210E3 File Offset: 0x0001F2E3
		public object Constant
		{
			get
			{
				if (!this.HasConstant)
				{
					return null;
				}
				return this.constant;
			}
			set
			{
				this.constant = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x000210EC File Offset: 0x0001F2EC
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.Module);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00021111 File Offset: 0x0001F311
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0002112F File Offset: 0x0001F32F
		public bool HasMarshalInfo
		{
			get
			{
				return this.marshal_info != null || this.GetHasMarshalInfo(this.Module);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00021147 File Offset: 0x0001F347
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x00021165 File Offset: 0x0001F365
		public MarshalInfo MarshalInfo
		{
			get
			{
				return this.marshal_info ?? this.GetMarshalInfo(ref this.marshal_info, this.Module);
			}
			set
			{
				this.marshal_info = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0002116E File Offset: 0x0001F36E
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x0002117D File Offset: 0x0001F37D
		public bool IsCompilerControlled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 0U, value);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x00021193 File Offset: 0x0001F393
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x000211A2 File Offset: 0x0001F3A2
		public bool IsPrivate
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 1U, value);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x000211B8 File Offset: 0x0001F3B8
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x000211C7 File Offset: 0x0001F3C7
		public bool IsFamilyAndAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 2U, value);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x000211DD File Offset: 0x0001F3DD
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x000211EC File Offset: 0x0001F3EC
		public bool IsAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 3U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 3U, value);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00021202 File Offset: 0x0001F402
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x00021211 File Offset: 0x0001F411
		public bool IsFamily
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 4U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 4U, value);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00021227 File Offset: 0x0001F427
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x00021236 File Offset: 0x0001F436
		public bool IsFamilyOrAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 5U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 5U, value);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0002124C File Offset: 0x0001F44C
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0002125B File Offset: 0x0001F45B
		public bool IsPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 6U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 6U, value);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00021271 File Offset: 0x0001F471
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x00021280 File Offset: 0x0001F480
		public bool IsStatic
		{
			get
			{
				return this.attributes.GetAttributes(16);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16, value);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x00021296 File Offset: 0x0001F496
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x000212A5 File Offset: 0x0001F4A5
		public bool IsInitOnly
		{
			get
			{
				return this.attributes.GetAttributes(32);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(32, value);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x000212BB File Offset: 0x0001F4BB
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x000212CA File Offset: 0x0001F4CA
		public bool IsLiteral
		{
			get
			{
				return this.attributes.GetAttributes(64);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(64, value);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x000212E0 File Offset: 0x0001F4E0
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x000212F2 File Offset: 0x0001F4F2
		public bool IsNotSerialized
		{
			get
			{
				return this.attributes.GetAttributes(128);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(128, value);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0002130B File Offset: 0x0001F50B
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x0002131D File Offset: 0x0001F51D
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(512);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(512, value);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x00021336 File Offset: 0x0001F536
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x00021348 File Offset: 0x0001F548
		public bool IsPInvokeImpl
		{
			get
			{
				return this.attributes.GetAttributes(8192);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8192, value);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00021361 File Offset: 0x0001F561
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x00021373 File Offset: 0x0001F573
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(1024);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024, value);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0002138C File Offset: 0x0001F58C
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x0002139E File Offset: 0x0001F59E
		public bool HasDefault
		{
			get
			{
				return this.attributes.GetAttributes(32768);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(32768, value);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x000207D4 File Offset: 0x0001E9D4
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x000207E1 File Offset: 0x0001E9E1
		public new TypeDefinition DeclaringType
		{
			get
			{
				return (TypeDefinition)base.DeclaringType;
			}
			set
			{
				base.DeclaringType = value;
			}
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x000213B7 File Offset: 0x0001F5B7
		public FieldDefinition(string name, FieldAttributes attributes, TypeReference fieldType)
			: base(name, fieldType)
		{
			this.attributes = (ushort)attributes;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00011FA0 File Offset: 0x000101A0
		public override FieldDefinition Resolve()
		{
			return this;
		}

		// Token: 0x040002E0 RID: 736
		private ushort attributes;

		// Token: 0x040002E1 RID: 737
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040002E2 RID: 738
		private int offset = -2;

		// Token: 0x040002E3 RID: 739
		internal int rva = -2;

		// Token: 0x040002E4 RID: 740
		private byte[] initial_value;

		// Token: 0x040002E5 RID: 741
		private object constant = Mixin.NotResolved;

		// Token: 0x040002E6 RID: 742
		private MarshalInfo marshal_info;
	}
}
