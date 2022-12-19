using System;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000177 RID: 375
	public sealed class TypeDefinition : TypeReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, ISecurityDeclarationProvider
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x000277CE File Offset: 0x000259CE
		// (set) Token: 0x06000B8E RID: 2958 RVA: 0x000277D6 File Offset: 0x000259D6
		public TypeAttributes Attributes
		{
			get
			{
				return (TypeAttributes)this.attributes;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && (uint)((ushort)value) != this.attributes)
				{
					throw new InvalidOperationException();
				}
				this.attributes = (uint)value;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x000277F7 File Offset: 0x000259F7
		// (set) Token: 0x06000B90 RID: 2960 RVA: 0x000277FF File Offset: 0x000259FF
		public TypeReference BaseType
		{
			get
			{
				return this.base_type;
			}
			set
			{
				this.base_type = value;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x00027808 File Offset: 0x00025A08
		// (set) Token: 0x06000B92 RID: 2962 RVA: 0x00027810 File Offset: 0x00025A10
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != base.Name)
				{
					throw new InvalidOperationException();
				}
				base.Name = value;
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00027838 File Offset: 0x00025A38
		private void ResolveLayout()
		{
			if (!base.HasImage)
			{
				this.packing_size = -1;
				this.class_size = -1;
				return;
			}
			object syncRoot = this.Module.SyncRoot;
			lock (syncRoot)
			{
				if (this.packing_size == -2 && this.class_size == -2)
				{
					Row<short, int> row = this.Module.Read<TypeDefinition, Row<short, int>>(this, (TypeDefinition type, MetadataReader reader) => reader.ReadTypeLayout(type));
					this.packing_size = row.Col1;
					this.class_size = row.Col2;
				}
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x000278E8 File Offset: 0x00025AE8
		public bool HasLayoutInfo
		{
			get
			{
				if (this.packing_size >= 0 || this.class_size >= 0)
				{
					return true;
				}
				this.ResolveLayout();
				return this.packing_size >= 0 || this.class_size >= 0;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x0002791B File Offset: 0x00025B1B
		// (set) Token: 0x06000B96 RID: 2966 RVA: 0x00027944 File Offset: 0x00025B44
		public short PackingSize
		{
			get
			{
				if (this.packing_size >= 0)
				{
					return this.packing_size;
				}
				this.ResolveLayout();
				if (this.packing_size < 0)
				{
					return -1;
				}
				return this.packing_size;
			}
			set
			{
				this.packing_size = value;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x0002794D File Offset: 0x00025B4D
		// (set) Token: 0x06000B98 RID: 2968 RVA: 0x00027976 File Offset: 0x00025B76
		public int ClassSize
		{
			get
			{
				if (this.class_size >= 0)
				{
					return this.class_size;
				}
				this.ResolveLayout();
				if (this.class_size < 0)
				{
					return -1;
				}
				return this.class_size;
			}
			set
			{
				this.class_size = value;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x00027980 File Offset: 0x00025B80
		public bool HasInterfaces
		{
			get
			{
				if (this.interfaces != null)
				{
					return this.interfaces.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, bool>(this, (TypeDefinition type, MetadataReader reader) => reader.HasInterfaces(type));
				}
				return false;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x000279DC File Offset: 0x00025BDC
		public Collection<InterfaceImplementation> Interfaces
		{
			get
			{
				if (this.interfaces != null)
				{
					return this.interfaces;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, InterfaceImplementationCollection>(ref this.interfaces, this, (TypeDefinition type, MetadataReader reader) => reader.ReadInterfaces(type));
				}
				Interlocked.CompareExchange<InterfaceImplementationCollection>(ref this.interfaces, new InterfaceImplementationCollection(this), null);
				return this.interfaces;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x00027A4C File Offset: 0x00025C4C
		public bool HasNestedTypes
		{
			get
			{
				if (this.nested_types != null)
				{
					return this.nested_types.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, bool>(this, (TypeDefinition type, MetadataReader reader) => reader.HasNestedTypes(type));
				}
				return false;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00027AA8 File Offset: 0x00025CA8
		public Collection<TypeDefinition> NestedTypes
		{
			get
			{
				if (this.nested_types != null)
				{
					return this.nested_types;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, Collection<TypeDefinition>>(ref this.nested_types, this, (TypeDefinition type, MetadataReader reader) => reader.ReadNestedTypes(type));
				}
				Interlocked.CompareExchange<Collection<TypeDefinition>>(ref this.nested_types, new MemberDefinitionCollection<TypeDefinition>(this), null);
				return this.nested_types;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x00027B17 File Offset: 0x00025D17
		public bool HasMethods
		{
			get
			{
				if (this.methods != null)
				{
					return this.methods.Count > 0;
				}
				return base.HasImage && this.methods_range.Length > 0U;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00027B48 File Offset: 0x00025D48
		public Collection<MethodDefinition> Methods
		{
			get
			{
				if (this.methods != null)
				{
					return this.methods;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, Collection<MethodDefinition>>(ref this.methods, this, (TypeDefinition type, MetadataReader reader) => reader.ReadMethods(type));
				}
				Interlocked.CompareExchange<Collection<MethodDefinition>>(ref this.methods, new MemberDefinitionCollection<MethodDefinition>(this), null);
				return this.methods;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00027BB7 File Offset: 0x00025DB7
		public bool HasFields
		{
			get
			{
				if (this.fields != null)
				{
					return this.fields.Count > 0;
				}
				return base.HasImage && this.fields_range.Length > 0U;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00027BE8 File Offset: 0x00025DE8
		public Collection<FieldDefinition> Fields
		{
			get
			{
				if (this.fields != null)
				{
					return this.fields;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, Collection<FieldDefinition>>(ref this.fields, this, (TypeDefinition type, MetadataReader reader) => reader.ReadFields(type));
				}
				Interlocked.CompareExchange<Collection<FieldDefinition>>(ref this.fields, new MemberDefinitionCollection<FieldDefinition>(this), null);
				return this.fields;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x00027C58 File Offset: 0x00025E58
		public bool HasEvents
		{
			get
			{
				if (this.events != null)
				{
					return this.events.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, bool>(this, (TypeDefinition type, MetadataReader reader) => reader.HasEvents(type));
				}
				return false;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00027CB4 File Offset: 0x00025EB4
		public Collection<EventDefinition> Events
		{
			get
			{
				if (this.events != null)
				{
					return this.events;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, Collection<EventDefinition>>(ref this.events, this, (TypeDefinition type, MetadataReader reader) => reader.ReadEvents(type));
				}
				Interlocked.CompareExchange<Collection<EventDefinition>>(ref this.events, new MemberDefinitionCollection<EventDefinition>(this), null);
				return this.events;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x00027D24 File Offset: 0x00025F24
		public bool HasProperties
		{
			get
			{
				if (this.properties != null)
				{
					return this.properties.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, bool>(this, (TypeDefinition type, MetadataReader reader) => reader.HasProperties(type));
				}
				return false;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x00027D80 File Offset: 0x00025F80
		public Collection<PropertyDefinition> Properties
		{
			get
			{
				if (this.properties != null)
				{
					return this.properties;
				}
				if (base.HasImage)
				{
					return this.Module.Read<TypeDefinition, Collection<PropertyDefinition>>(ref this.properties, this, (TypeDefinition type, MetadataReader reader) => reader.ReadProperties(type));
				}
				Interlocked.CompareExchange<Collection<PropertyDefinition>>(ref this.properties, new MemberDefinitionCollection<PropertyDefinition>(this), null);
				return this.properties;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x00027DEF File Offset: 0x00025FEF
		public bool HasSecurityDeclarations
		{
			get
			{
				if (this.security_declarations != null)
				{
					return this.security_declarations.Count > 0;
				}
				return this.GetHasSecurityDeclarations(this.Module);
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x00027E14 File Offset: 0x00026014
		public Collection<SecurityDeclaration> SecurityDeclarations
		{
			get
			{
				return this.security_declarations ?? this.GetSecurityDeclarations(ref this.security_declarations, this.Module);
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x00027E32 File Offset: 0x00026032
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

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x00027E57 File Offset: 0x00026057
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x00027E75 File Offset: 0x00026075
		public override bool HasGenericParameters
		{
			get
			{
				if (this.generic_parameters != null)
				{
					return this.generic_parameters.Count > 0;
				}
				return this.GetHasGenericParameters(this.Module);
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x00027E9A File Offset: 0x0002609A
		public override Collection<GenericParameter> GenericParameters
		{
			get
			{
				return this.generic_parameters ?? this.GetGenericParameters(ref this.generic_parameters, this.Module);
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x00027EB8 File Offset: 0x000260B8
		// (set) Token: 0x06000BAC RID: 2988 RVA: 0x00027EC7 File Offset: 0x000260C7
		public bool IsNotPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 0U, value);
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x00027EDD File Offset: 0x000260DD
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x00027EEC File Offset: 0x000260EC
		public bool IsPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 1U, value);
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00027F02 File Offset: 0x00026102
		// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x00027F11 File Offset: 0x00026111
		public bool IsNestedPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 2U, value);
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00027F27 File Offset: 0x00026127
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x00027F36 File Offset: 0x00026136
		public bool IsNestedPrivate
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 3U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 3U, value);
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x00027F4C File Offset: 0x0002614C
		// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x00027F5B File Offset: 0x0002615B
		public bool IsNestedFamily
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 4U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 4U, value);
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x00027F71 File Offset: 0x00026171
		// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x00027F80 File Offset: 0x00026180
		public bool IsNestedAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 5U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 5U, value);
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x00027F96 File Offset: 0x00026196
		// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x00027FA5 File Offset: 0x000261A5
		public bool IsNestedFamilyAndAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 6U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 6U, value);
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x00027FBB File Offset: 0x000261BB
		// (set) Token: 0x06000BBA RID: 3002 RVA: 0x00027FCA File Offset: 0x000261CA
		public bool IsNestedFamilyOrAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 7U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 7U, value);
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x00027FE0 File Offset: 0x000261E0
		// (set) Token: 0x06000BBC RID: 3004 RVA: 0x00027FF0 File Offset: 0x000261F0
		public bool IsAutoLayout
		{
			get
			{
				return this.attributes.GetMaskedAttributes(24U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(24U, 0U, value);
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00028007 File Offset: 0x00026207
		// (set) Token: 0x06000BBE RID: 3006 RVA: 0x00028017 File Offset: 0x00026217
		public bool IsSequentialLayout
		{
			get
			{
				return this.attributes.GetMaskedAttributes(24U, 8U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(24U, 8U, value);
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0002802E File Offset: 0x0002622E
		// (set) Token: 0x06000BC0 RID: 3008 RVA: 0x0002803F File Offset: 0x0002623F
		public bool IsExplicitLayout
		{
			get
			{
				return this.attributes.GetMaskedAttributes(24U, 16U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(24U, 16U, value);
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x00028057 File Offset: 0x00026257
		// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x00028067 File Offset: 0x00026267
		public bool IsClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(32U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(32U, 0U, value);
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x0002807E File Offset: 0x0002627E
		// (set) Token: 0x06000BC4 RID: 3012 RVA: 0x0002808F File Offset: 0x0002628F
		public bool IsInterface
		{
			get
			{
				return this.attributes.GetMaskedAttributes(32U, 32U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(32U, 32U, value);
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x000280A7 File Offset: 0x000262A7
		// (set) Token: 0x06000BC6 RID: 3014 RVA: 0x000280B9 File Offset: 0x000262B9
		public bool IsAbstract
		{
			get
			{
				return this.attributes.GetAttributes(128U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(128U, value);
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x000280D2 File Offset: 0x000262D2
		// (set) Token: 0x06000BC8 RID: 3016 RVA: 0x000280E4 File Offset: 0x000262E4
		public bool IsSealed
		{
			get
			{
				return this.attributes.GetAttributes(256U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(256U, value);
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x000280FD File Offset: 0x000262FD
		// (set) Token: 0x06000BCA RID: 3018 RVA: 0x0002810F File Offset: 0x0002630F
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(1024U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024U, value);
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x00028128 File Offset: 0x00026328
		// (set) Token: 0x06000BCC RID: 3020 RVA: 0x0002813A File Offset: 0x0002633A
		public bool IsImport
		{
			get
			{
				return this.attributes.GetAttributes(4096U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4096U, value);
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x00028153 File Offset: 0x00026353
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x00028165 File Offset: 0x00026365
		public bool IsSerializable
		{
			get
			{
				return this.attributes.GetAttributes(8192U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8192U, value);
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x0002817E File Offset: 0x0002637E
		// (set) Token: 0x06000BD0 RID: 3024 RVA: 0x00028190 File Offset: 0x00026390
		public bool IsWindowsRuntime
		{
			get
			{
				return this.attributes.GetAttributes(16384U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16384U, value);
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x000281A9 File Offset: 0x000263A9
		// (set) Token: 0x06000BD2 RID: 3026 RVA: 0x000281BC File Offset: 0x000263BC
		public bool IsAnsiClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(196608U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(196608U, 0U, value);
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x000281D6 File Offset: 0x000263D6
		// (set) Token: 0x06000BD4 RID: 3028 RVA: 0x000281ED File Offset: 0x000263ED
		public bool IsUnicodeClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(196608U, 65536U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(196608U, 65536U, value);
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x0002820B File Offset: 0x0002640B
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x00028222 File Offset: 0x00026422
		public bool IsAutoClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(196608U, 131072U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(196608U, 131072U, value);
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x00028240 File Offset: 0x00026440
		// (set) Token: 0x06000BD8 RID: 3032 RVA: 0x00028252 File Offset: 0x00026452
		public bool IsBeforeFieldInit
		{
			get
			{
				return this.attributes.GetAttributes(1048576U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1048576U, value);
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0002826B File Offset: 0x0002646B
		// (set) Token: 0x06000BDA RID: 3034 RVA: 0x0002827D File Offset: 0x0002647D
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(2048U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(2048U, value);
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x00028296 File Offset: 0x00026496
		// (set) Token: 0x06000BDC RID: 3036 RVA: 0x000282A8 File Offset: 0x000264A8
		public bool HasSecurity
		{
			get
			{
				return this.attributes.GetAttributes(262144U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(262144U, value);
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x000282C1 File Offset: 0x000264C1
		public bool IsEnum
		{
			get
			{
				return this.base_type != null && this.base_type.IsTypeOf("System", "Enum");
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x000282E4 File Offset: 0x000264E4
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x000039F6 File Offset: 0x00001BF6
		public override bool IsValueType
		{
			get
			{
				return this.base_type != null && (this.base_type.IsTypeOf("System", "Enum") || (this.base_type.IsTypeOf("System", "ValueType") && !this.IsTypeOf("System", "Enum")));
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x00028340 File Offset: 0x00026540
		public override bool IsPrimitive
		{
			get
			{
				ElementType elementType;
				return MetadataSystem.TryGetPrimitiveElementType(this, out elementType) && elementType.IsPrimitive();
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x00028360 File Offset: 0x00026560
		public override MetadataType MetadataType
		{
			get
			{
				ElementType elementType;
				if (MetadataSystem.TryGetPrimitiveElementType(this, out elementType))
				{
					return (MetadataType)elementType;
				}
				return base.MetadataType;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x0002837F File Offset: 0x0002657F
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x0002838C File Offset: 0x0002658C
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

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00028395 File Offset: 0x00026595
		// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x00020F72 File Offset: 0x0001F172
		internal new TypeDefinitionProjection WindowsRuntimeProjection
		{
			get
			{
				return (TypeDefinitionProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x000283A2 File Offset: 0x000265A2
		public TypeDefinition(string @namespace, string name, TypeAttributes attributes)
			: base(@namespace, name)
		{
			this.attributes = (uint)attributes;
			this.token = new MetadataToken(TokenType.TypeDef);
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000283D3 File Offset: 0x000265D3
		public TypeDefinition(string @namespace, string name, TypeAttributes attributes, TypeReference baseType)
			: this(@namespace, name, attributes)
		{
			this.BaseType = baseType;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x000283E8 File Offset: 0x000265E8
		protected override void ClearFullName()
		{
			base.ClearFullName();
			if (!this.HasNestedTypes)
			{
				return;
			}
			Collection<TypeDefinition> nestedTypes = this.NestedTypes;
			for (int i = 0; i < nestedTypes.Count; i++)
			{
				nestedTypes[i].ClearFullName();
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00011FA0 File Offset: 0x000101A0
		public override TypeDefinition Resolve()
		{
			return this;
		}

		// Token: 0x040004E9 RID: 1257
		private uint attributes;

		// Token: 0x040004EA RID: 1258
		private TypeReference base_type;

		// Token: 0x040004EB RID: 1259
		internal Range fields_range;

		// Token: 0x040004EC RID: 1260
		internal Range methods_range;

		// Token: 0x040004ED RID: 1261
		private short packing_size = -2;

		// Token: 0x040004EE RID: 1262
		private int class_size = -2;

		// Token: 0x040004EF RID: 1263
		private InterfaceImplementationCollection interfaces;

		// Token: 0x040004F0 RID: 1264
		private Collection<TypeDefinition> nested_types;

		// Token: 0x040004F1 RID: 1265
		private Collection<MethodDefinition> methods;

		// Token: 0x040004F2 RID: 1266
		private Collection<FieldDefinition> fields;

		// Token: 0x040004F3 RID: 1267
		private Collection<EventDefinition> events;

		// Token: 0x040004F4 RID: 1268
		private Collection<PropertyDefinition> properties;

		// Token: 0x040004F5 RID: 1269
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040004F6 RID: 1270
		private Collection<SecurityDeclaration> security_declarations;
	}
}
