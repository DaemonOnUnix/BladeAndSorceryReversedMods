using System;
using System.Collections.Generic;
using System.Threading;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200013F RID: 319
	internal sealed class MetadataSystem
	{
		// Token: 0x06000900 RID: 2304 RVA: 0x000239B0 File Offset: 0x00021BB0
		private static void InitializePrimitives()
		{
			Dictionary<string, Row<ElementType, bool>> dictionary = new Dictionary<string, Row<ElementType, bool>>(18, StringComparer.Ordinal)
			{
				{
					"Void",
					new Row<ElementType, bool>(ElementType.Void, false)
				},
				{
					"Boolean",
					new Row<ElementType, bool>(ElementType.Boolean, true)
				},
				{
					"Char",
					new Row<ElementType, bool>(ElementType.Char, true)
				},
				{
					"SByte",
					new Row<ElementType, bool>(ElementType.I1, true)
				},
				{
					"Byte",
					new Row<ElementType, bool>(ElementType.U1, true)
				},
				{
					"Int16",
					new Row<ElementType, bool>(ElementType.I2, true)
				},
				{
					"UInt16",
					new Row<ElementType, bool>(ElementType.U2, true)
				},
				{
					"Int32",
					new Row<ElementType, bool>(ElementType.I4, true)
				},
				{
					"UInt32",
					new Row<ElementType, bool>(ElementType.U4, true)
				},
				{
					"Int64",
					new Row<ElementType, bool>(ElementType.I8, true)
				},
				{
					"UInt64",
					new Row<ElementType, bool>(ElementType.U8, true)
				},
				{
					"Single",
					new Row<ElementType, bool>(ElementType.R4, true)
				},
				{
					"Double",
					new Row<ElementType, bool>(ElementType.R8, true)
				},
				{
					"String",
					new Row<ElementType, bool>(ElementType.String, false)
				},
				{
					"TypedReference",
					new Row<ElementType, bool>(ElementType.TypedByRef, false)
				},
				{
					"IntPtr",
					new Row<ElementType, bool>(ElementType.I, true)
				},
				{
					"UIntPtr",
					new Row<ElementType, bool>(ElementType.U, true)
				},
				{
					"Object",
					new Row<ElementType, bool>(ElementType.Object, false)
				}
			};
			Interlocked.CompareExchange<Dictionary<string, Row<ElementType, bool>>>(ref MetadataSystem.primitive_value_types, dictionary, null);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00023B28 File Offset: 0x00021D28
		public static void TryProcessPrimitiveTypeReference(TypeReference type)
		{
			if (type.Namespace != "System")
			{
				return;
			}
			IMetadataScope scope = type.scope;
			if (scope == null || scope.MetadataScopeType != MetadataScopeType.AssemblyNameReference)
			{
				return;
			}
			Row<ElementType, bool> row;
			if (!MetadataSystem.TryGetPrimitiveData(type, out row))
			{
				return;
			}
			type.etype = row.Col1;
			type.IsValueType = row.Col2;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00023B80 File Offset: 0x00021D80
		public static bool TryGetPrimitiveElementType(TypeDefinition type, out ElementType etype)
		{
			etype = ElementType.None;
			if (type.Namespace != "System")
			{
				return false;
			}
			Row<ElementType, bool> row;
			if (MetadataSystem.TryGetPrimitiveData(type, out row))
			{
				etype = row.Col1;
				return true;
			}
			return false;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00023BB9 File Offset: 0x00021DB9
		private static bool TryGetPrimitiveData(TypeReference type, out Row<ElementType, bool> primitive_data)
		{
			if (MetadataSystem.primitive_value_types == null)
			{
				MetadataSystem.InitializePrimitives();
			}
			return MetadataSystem.primitive_value_types.TryGetValue(type.Name, out primitive_data);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00023BD8 File Offset: 0x00021DD8
		public void Clear()
		{
			if (this.NestedTypes != null)
			{
				this.NestedTypes = new Dictionary<uint, Collection<uint>>(0);
			}
			if (this.ReverseNestedTypes != null)
			{
				this.ReverseNestedTypes = new Dictionary<uint, uint>(0);
			}
			if (this.Interfaces != null)
			{
				this.Interfaces = new Dictionary<uint, Collection<Row<uint, MetadataToken>>>(0);
			}
			if (this.ClassLayouts != null)
			{
				this.ClassLayouts = new Dictionary<uint, Row<ushort, uint>>(0);
			}
			if (this.FieldLayouts != null)
			{
				this.FieldLayouts = new Dictionary<uint, uint>(0);
			}
			if (this.FieldRVAs != null)
			{
				this.FieldRVAs = new Dictionary<uint, uint>(0);
			}
			if (this.FieldMarshals != null)
			{
				this.FieldMarshals = new Dictionary<MetadataToken, uint>(0);
			}
			if (this.Constants != null)
			{
				this.Constants = new Dictionary<MetadataToken, Row<ElementType, uint>>(0);
			}
			if (this.Overrides != null)
			{
				this.Overrides = new Dictionary<uint, Collection<MetadataToken>>(0);
			}
			if (this.CustomAttributes != null)
			{
				this.CustomAttributes = new Dictionary<MetadataToken, Range[]>(0);
			}
			if (this.SecurityDeclarations != null)
			{
				this.SecurityDeclarations = new Dictionary<MetadataToken, Range[]>(0);
			}
			if (this.Events != null)
			{
				this.Events = new Dictionary<uint, Range>(0);
			}
			if (this.Properties != null)
			{
				this.Properties = new Dictionary<uint, Range>(0);
			}
			if (this.Semantics != null)
			{
				this.Semantics = new Dictionary<uint, Row<MethodSemanticsAttributes, MetadataToken>>(0);
			}
			if (this.PInvokes != null)
			{
				this.PInvokes = new Dictionary<uint, Row<PInvokeAttributes, uint, uint>>(0);
			}
			if (this.GenericParameters != null)
			{
				this.GenericParameters = new Dictionary<MetadataToken, Range[]>(0);
			}
			if (this.GenericConstraints != null)
			{
				this.GenericConstraints = new Dictionary<uint, Collection<Row<uint, MetadataToken>>>(0);
			}
			this.Documents = Empty<Document>.Array;
			this.ImportScopes = Empty<ImportDebugInformation>.Array;
			if (this.LocalScopes != null)
			{
				this.LocalScopes = new Dictionary<uint, Collection<Row<uint, Range, Range, uint, uint, uint>>>(0);
			}
			if (this.StateMachineMethods != null)
			{
				this.StateMachineMethods = new Dictionary<uint, uint>(0);
			}
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00023D77 File Offset: 0x00021F77
		public AssemblyNameReference GetAssemblyNameReference(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.AssemblyReferences.Length))
			{
				return null;
			}
			return this.AssemblyReferences[(int)(rid - 1U)];
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00023D96 File Offset: 0x00021F96
		public TypeDefinition GetTypeDefinition(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Types.Length))
			{
				return null;
			}
			return this.Types[(int)(rid - 1U)];
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00023DB5 File Offset: 0x00021FB5
		public void AddTypeDefinition(TypeDefinition type)
		{
			this.Types[(int)(type.token.RID - 1U)] = type;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00023DCC File Offset: 0x00021FCC
		public TypeReference GetTypeReference(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.TypeReferences.Length))
			{
				return null;
			}
			return this.TypeReferences[(int)(rid - 1U)];
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00023DEB File Offset: 0x00021FEB
		public void AddTypeReference(TypeReference type)
		{
			this.TypeReferences[(int)(type.token.RID - 1U)] = type;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00023E02 File Offset: 0x00022002
		public FieldDefinition GetFieldDefinition(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Fields.Length))
			{
				return null;
			}
			return this.Fields[(int)(rid - 1U)];
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00023E21 File Offset: 0x00022021
		public void AddFieldDefinition(FieldDefinition field)
		{
			this.Fields[(int)(field.token.RID - 1U)] = field;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00023E38 File Offset: 0x00022038
		public MethodDefinition GetMethodDefinition(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Methods.Length))
			{
				return null;
			}
			return this.Methods[(int)(rid - 1U)];
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00023E57 File Offset: 0x00022057
		public void AddMethodDefinition(MethodDefinition method)
		{
			this.Methods[(int)(method.token.RID - 1U)] = method;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00023E6E File Offset: 0x0002206E
		public MemberReference GetMemberReference(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.MemberReferences.Length))
			{
				return null;
			}
			return this.MemberReferences[(int)(rid - 1U)];
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00023E8D File Offset: 0x0002208D
		public void AddMemberReference(MemberReference member)
		{
			this.MemberReferences[(int)(member.token.RID - 1U)] = member;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00023EA4 File Offset: 0x000220A4
		public bool TryGetNestedTypeMapping(TypeDefinition type, out Collection<uint> mapping)
		{
			return this.NestedTypes.TryGetValue(type.token.RID, out mapping);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00023EBD File Offset: 0x000220BD
		public void SetNestedTypeMapping(uint type_rid, Collection<uint> mapping)
		{
			this.NestedTypes[type_rid] = mapping;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00023ECC File Offset: 0x000220CC
		public void RemoveNestedTypeMapping(TypeDefinition type)
		{
			this.NestedTypes.Remove(type.token.RID);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00023EE5 File Offset: 0x000220E5
		public bool TryGetReverseNestedTypeMapping(TypeDefinition type, out uint declaring)
		{
			return this.ReverseNestedTypes.TryGetValue(type.token.RID, out declaring);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00023EFE File Offset: 0x000220FE
		public void SetReverseNestedTypeMapping(uint nested, uint declaring)
		{
			this.ReverseNestedTypes[nested] = declaring;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00023F0D File Offset: 0x0002210D
		public void RemoveReverseNestedTypeMapping(TypeDefinition type)
		{
			this.ReverseNestedTypes.Remove(type.token.RID);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00023F26 File Offset: 0x00022126
		public bool TryGetInterfaceMapping(TypeDefinition type, out Collection<Row<uint, MetadataToken>> mapping)
		{
			return this.Interfaces.TryGetValue(type.token.RID, out mapping);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00023F3F File Offset: 0x0002213F
		public void SetInterfaceMapping(uint type_rid, Collection<Row<uint, MetadataToken>> mapping)
		{
			this.Interfaces[type_rid] = mapping;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00023F4E File Offset: 0x0002214E
		public void RemoveInterfaceMapping(TypeDefinition type)
		{
			this.Interfaces.Remove(type.token.RID);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00023F67 File Offset: 0x00022167
		public void AddPropertiesRange(uint type_rid, Range range)
		{
			this.Properties.Add(type_rid, range);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00023F76 File Offset: 0x00022176
		public bool TryGetPropertiesRange(TypeDefinition type, out Range range)
		{
			return this.Properties.TryGetValue(type.token.RID, out range);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00023F8F File Offset: 0x0002218F
		public void RemovePropertiesRange(TypeDefinition type)
		{
			this.Properties.Remove(type.token.RID);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00023FA8 File Offset: 0x000221A8
		public void AddEventsRange(uint type_rid, Range range)
		{
			this.Events.Add(type_rid, range);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00023FB7 File Offset: 0x000221B7
		public bool TryGetEventsRange(TypeDefinition type, out Range range)
		{
			return this.Events.TryGetValue(type.token.RID, out range);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00023FD0 File Offset: 0x000221D0
		public void RemoveEventsRange(TypeDefinition type)
		{
			this.Events.Remove(type.token.RID);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00023FE9 File Offset: 0x000221E9
		public bool TryGetGenericParameterRanges(IGenericParameterProvider owner, out Range[] ranges)
		{
			return this.GenericParameters.TryGetValue(owner.MetadataToken, out ranges);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00023FFD File Offset: 0x000221FD
		public void RemoveGenericParameterRange(IGenericParameterProvider owner)
		{
			this.GenericParameters.Remove(owner.MetadataToken);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00024011 File Offset: 0x00022211
		public bool TryGetCustomAttributeRanges(ICustomAttributeProvider owner, out Range[] ranges)
		{
			return this.CustomAttributes.TryGetValue(owner.MetadataToken, out ranges);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00024025 File Offset: 0x00022225
		public void RemoveCustomAttributeRange(ICustomAttributeProvider owner)
		{
			this.CustomAttributes.Remove(owner.MetadataToken);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00024039 File Offset: 0x00022239
		public bool TryGetSecurityDeclarationRanges(ISecurityDeclarationProvider owner, out Range[] ranges)
		{
			return this.SecurityDeclarations.TryGetValue(owner.MetadataToken, out ranges);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0002404D File Offset: 0x0002224D
		public void RemoveSecurityDeclarationRange(ISecurityDeclarationProvider owner)
		{
			this.SecurityDeclarations.Remove(owner.MetadataToken);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00024061 File Offset: 0x00022261
		public bool TryGetGenericConstraintMapping(GenericParameter generic_parameter, out Collection<Row<uint, MetadataToken>> mapping)
		{
			return this.GenericConstraints.TryGetValue(generic_parameter.token.RID, out mapping);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0002407A File Offset: 0x0002227A
		public void SetGenericConstraintMapping(uint gp_rid, Collection<Row<uint, MetadataToken>> mapping)
		{
			this.GenericConstraints[gp_rid] = mapping;
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00024089 File Offset: 0x00022289
		public void RemoveGenericConstraintMapping(GenericParameter generic_parameter)
		{
			this.GenericConstraints.Remove(generic_parameter.token.RID);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x000240A2 File Offset: 0x000222A2
		public bool TryGetOverrideMapping(MethodDefinition method, out Collection<MetadataToken> mapping)
		{
			return this.Overrides.TryGetValue(method.token.RID, out mapping);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x000240BB File Offset: 0x000222BB
		public void SetOverrideMapping(uint rid, Collection<MetadataToken> mapping)
		{
			this.Overrides[rid] = mapping;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x000240CA File Offset: 0x000222CA
		public void RemoveOverrideMapping(MethodDefinition method)
		{
			this.Overrides.Remove(method.token.RID);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x000240E3 File Offset: 0x000222E3
		public Document GetDocument(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Documents.Length))
			{
				return null;
			}
			return this.Documents[(int)(rid - 1U)];
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00024104 File Offset: 0x00022304
		public bool TryGetLocalScopes(MethodDefinition method, out Collection<Row<uint, Range, Range, uint, uint, uint>> scopes)
		{
			return this.LocalScopes.TryGetValue(method.MetadataToken.RID, out scopes);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0002412B File Offset: 0x0002232B
		public void SetLocalScopes(uint method_rid, Collection<Row<uint, Range, Range, uint, uint, uint>> records)
		{
			this.LocalScopes[method_rid] = records;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002413A File Offset: 0x0002233A
		public ImportDebugInformation GetImportScope(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.ImportScopes.Length))
			{
				return null;
			}
			return this.ImportScopes[(int)(rid - 1U)];
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002415C File Offset: 0x0002235C
		public bool TryGetStateMachineKickOffMethod(MethodDefinition method, out uint rid)
		{
			return this.StateMachineMethods.TryGetValue(method.MetadataToken.RID, out rid);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00024183 File Offset: 0x00022383
		public TypeDefinition GetFieldDeclaringType(uint field_rid)
		{
			return MetadataSystem.BinaryRangeSearch(this.Types, field_rid, true);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00024192 File Offset: 0x00022392
		public TypeDefinition GetMethodDeclaringType(uint method_rid)
		{
			return MetadataSystem.BinaryRangeSearch(this.Types, method_rid, false);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x000241A4 File Offset: 0x000223A4
		private static TypeDefinition BinaryRangeSearch(TypeDefinition[] types, uint rid, bool field)
		{
			int i = 0;
			int num = types.Length - 1;
			while (i <= num)
			{
				int num2 = i + (num - i) / 2;
				TypeDefinition typeDefinition = types[num2];
				Range range = (field ? typeDefinition.fields_range : typeDefinition.methods_range);
				if (rid < range.Start)
				{
					num = num2 - 1;
				}
				else
				{
					if (rid < range.Start + range.Length)
					{
						return typeDefinition;
					}
					i = num2 + 1;
				}
			}
			return null;
		}

		// Token: 0x04000333 RID: 819
		internal AssemblyNameReference[] AssemblyReferences;

		// Token: 0x04000334 RID: 820
		internal ModuleReference[] ModuleReferences;

		// Token: 0x04000335 RID: 821
		internal TypeDefinition[] Types;

		// Token: 0x04000336 RID: 822
		internal TypeReference[] TypeReferences;

		// Token: 0x04000337 RID: 823
		internal FieldDefinition[] Fields;

		// Token: 0x04000338 RID: 824
		internal MethodDefinition[] Methods;

		// Token: 0x04000339 RID: 825
		internal MemberReference[] MemberReferences;

		// Token: 0x0400033A RID: 826
		internal Dictionary<uint, Collection<uint>> NestedTypes;

		// Token: 0x0400033B RID: 827
		internal Dictionary<uint, uint> ReverseNestedTypes;

		// Token: 0x0400033C RID: 828
		internal Dictionary<uint, Collection<Row<uint, MetadataToken>>> Interfaces;

		// Token: 0x0400033D RID: 829
		internal Dictionary<uint, Row<ushort, uint>> ClassLayouts;

		// Token: 0x0400033E RID: 830
		internal Dictionary<uint, uint> FieldLayouts;

		// Token: 0x0400033F RID: 831
		internal Dictionary<uint, uint> FieldRVAs;

		// Token: 0x04000340 RID: 832
		internal Dictionary<MetadataToken, uint> FieldMarshals;

		// Token: 0x04000341 RID: 833
		internal Dictionary<MetadataToken, Row<ElementType, uint>> Constants;

		// Token: 0x04000342 RID: 834
		internal Dictionary<uint, Collection<MetadataToken>> Overrides;

		// Token: 0x04000343 RID: 835
		internal Dictionary<MetadataToken, Range[]> CustomAttributes;

		// Token: 0x04000344 RID: 836
		internal Dictionary<MetadataToken, Range[]> SecurityDeclarations;

		// Token: 0x04000345 RID: 837
		internal Dictionary<uint, Range> Events;

		// Token: 0x04000346 RID: 838
		internal Dictionary<uint, Range> Properties;

		// Token: 0x04000347 RID: 839
		internal Dictionary<uint, Row<MethodSemanticsAttributes, MetadataToken>> Semantics;

		// Token: 0x04000348 RID: 840
		internal Dictionary<uint, Row<PInvokeAttributes, uint, uint>> PInvokes;

		// Token: 0x04000349 RID: 841
		internal Dictionary<MetadataToken, Range[]> GenericParameters;

		// Token: 0x0400034A RID: 842
		internal Dictionary<uint, Collection<Row<uint, MetadataToken>>> GenericConstraints;

		// Token: 0x0400034B RID: 843
		internal Document[] Documents;

		// Token: 0x0400034C RID: 844
		internal Dictionary<uint, Collection<Row<uint, Range, Range, uint, uint, uint>>> LocalScopes;

		// Token: 0x0400034D RID: 845
		internal ImportDebugInformation[] ImportScopes;

		// Token: 0x0400034E RID: 846
		internal Dictionary<uint, uint> StateMachineMethods;

		// Token: 0x0400034F RID: 847
		internal Dictionary<MetadataToken, Row<Guid, uint, uint>[]> CustomDebugInformations;

		// Token: 0x04000350 RID: 848
		private static Dictionary<string, Row<ElementType, bool>> primitive_value_types;
	}
}
