using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200017C RID: 380
	internal class TypeParser
	{
		// Token: 0x06000C11 RID: 3089 RVA: 0x00028732 File Offset: 0x00026932
		private TypeParser(string fullname)
		{
			this.fullname = fullname;
			this.length = fullname.Length;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00028750 File Offset: 0x00026950
		private TypeParser.Type ParseType(bool fq_name)
		{
			TypeParser.Type type = new TypeParser.Type();
			type.type_fullname = this.ParsePart();
			type.nested_names = this.ParseNestedNames();
			if (TypeParser.TryGetArity(type))
			{
				type.generic_arguments = this.ParseGenericArguments(type.arity);
			}
			type.specs = this.ParseSpecs();
			if (fq_name)
			{
				type.assembly = this.ParseAssemblyName();
			}
			return type;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x000287B4 File Offset: 0x000269B4
		private static bool TryGetArity(TypeParser.Type type)
		{
			int num = 0;
			TypeParser.TryAddArity(type.type_fullname, ref num);
			string[] nested_names = type.nested_names;
			if (!nested_names.IsNullOrEmpty<string>())
			{
				for (int i = 0; i < nested_names.Length; i++)
				{
					TypeParser.TryAddArity(nested_names[i], ref num);
				}
			}
			type.arity = num;
			return num > 0;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00028804 File Offset: 0x00026A04
		private static bool TryGetArity(string name, out int arity)
		{
			arity = 0;
			int num = name.LastIndexOf('`');
			return num != -1 && TypeParser.ParseInt32(name.Substring(num + 1), out arity);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00028832 File Offset: 0x00026A32
		private static bool ParseInt32(string value, out int result)
		{
			return int.TryParse(value, out result);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0002883C File Offset: 0x00026A3C
		private static void TryAddArity(string name, ref int arity)
		{
			int num;
			if (!TypeParser.TryGetArity(name, out num))
			{
				return;
			}
			arity += num;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0002885C File Offset: 0x00026A5C
		private string ParsePart()
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (this.position < this.length && !TypeParser.IsDelimiter(this.fullname[this.position]))
			{
				if (this.fullname[this.position] == '\\')
				{
					this.position++;
				}
				StringBuilder stringBuilder2 = stringBuilder;
				string text = this.fullname;
				int num = this.position;
				this.position = num + 1;
				stringBuilder2.Append(text[num]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x000288E3 File Offset: 0x00026AE3
		private static bool IsDelimiter(char chr)
		{
			return "+,[]*&".IndexOf(chr) != -1;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x000288F6 File Offset: 0x00026AF6
		private void TryParseWhiteSpace()
		{
			while (this.position < this.length && char.IsWhiteSpace(this.fullname[this.position]))
			{
				this.position++;
			}
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00028930 File Offset: 0x00026B30
		private string[] ParseNestedNames()
		{
			string[] array = null;
			while (this.TryParse('+'))
			{
				TypeParser.Add<string>(ref array, this.ParsePart());
			}
			return array;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00028959 File Offset: 0x00026B59
		private bool TryParse(char chr)
		{
			if (this.position < this.length && this.fullname[this.position] == chr)
			{
				this.position++;
				return true;
			}
			return false;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0002898E File Offset: 0x00026B8E
		private static void Add<T>(ref T[] array, T item)
		{
			array = array.Add(item);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0002899C File Offset: 0x00026B9C
		private int[] ParseSpecs()
		{
			int[] array = null;
			while (this.position < this.length)
			{
				char c = this.fullname[this.position];
				if (c != '&')
				{
					if (c != '*')
					{
						if (c != '[')
						{
							return array;
						}
						this.position++;
						char c2 = this.fullname[this.position];
						if (c2 != '*')
						{
							if (c2 == ']')
							{
								this.position++;
								TypeParser.Add<int>(ref array, -3);
							}
							else
							{
								int num = 1;
								while (this.TryParse(','))
								{
									num++;
								}
								TypeParser.Add<int>(ref array, num);
								this.TryParse(']');
							}
						}
						else
						{
							this.position++;
							TypeParser.Add<int>(ref array, 1);
						}
					}
					else
					{
						this.position++;
						TypeParser.Add<int>(ref array, -1);
					}
				}
				else
				{
					this.position++;
					TypeParser.Add<int>(ref array, -2);
				}
			}
			return array;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00028AA4 File Offset: 0x00026CA4
		private TypeParser.Type[] ParseGenericArguments(int arity)
		{
			TypeParser.Type[] array = null;
			if (this.position == this.length || this.fullname[this.position] != '[')
			{
				return array;
			}
			this.TryParse('[');
			for (int i = 0; i < arity; i++)
			{
				bool flag = this.TryParse('[');
				TypeParser.Add<TypeParser.Type>(ref array, this.ParseType(flag));
				if (flag)
				{
					this.TryParse(']');
				}
				this.TryParse(',');
				this.TryParseWhiteSpace();
			}
			this.TryParse(']');
			return array;
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00028B2C File Offset: 0x00026D2C
		private string ParseAssemblyName()
		{
			if (!this.TryParse(','))
			{
				return string.Empty;
			}
			this.TryParseWhiteSpace();
			int num = this.position;
			while (this.position < this.length)
			{
				char c = this.fullname[this.position];
				if (c == '[' || c == ']')
				{
					break;
				}
				this.position++;
			}
			return this.fullname.Substring(num, this.position - num);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00028BA4 File Offset: 0x00026DA4
		public static TypeReference ParseType(ModuleDefinition module, string fullname, bool typeDefinitionOnly = false)
		{
			if (string.IsNullOrEmpty(fullname))
			{
				return null;
			}
			TypeParser typeParser = new TypeParser(fullname);
			return TypeParser.GetTypeReference(module, typeParser.ParseType(true), typeDefinitionOnly);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00028BD0 File Offset: 0x00026DD0
		private static TypeReference GetTypeReference(ModuleDefinition module, TypeParser.Type type_info, bool type_def_only)
		{
			TypeReference typeReference;
			if (!TypeParser.TryGetDefinition(module, type_info, out typeReference))
			{
				if (type_def_only)
				{
					return null;
				}
				typeReference = TypeParser.CreateReference(type_info, module, TypeParser.GetMetadataScope(module, type_info));
			}
			return TypeParser.CreateSpecs(typeReference, type_info);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00028C04 File Offset: 0x00026E04
		private static TypeReference CreateSpecs(TypeReference type, TypeParser.Type type_info)
		{
			type = TypeParser.TryCreateGenericInstanceType(type, type_info);
			int[] specs = type_info.specs;
			if (specs.IsNullOrEmpty<int>())
			{
				return type;
			}
			for (int i = 0; i < specs.Length; i++)
			{
				switch (specs[i])
				{
				case -3:
					type = new ArrayType(type);
					break;
				case -2:
					type = new ByReferenceType(type);
					break;
				case -1:
					type = new PointerType(type);
					break;
				default:
				{
					ArrayType arrayType = new ArrayType(type);
					arrayType.Dimensions.Clear();
					for (int j = 0; j < specs[i]; j++)
					{
						arrayType.Dimensions.Add(default(ArrayDimension));
					}
					type = arrayType;
					break;
				}
				}
			}
			return type;
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00028CB0 File Offset: 0x00026EB0
		private static TypeReference TryCreateGenericInstanceType(TypeReference type, TypeParser.Type type_info)
		{
			TypeParser.Type[] generic_arguments = type_info.generic_arguments;
			if (generic_arguments.IsNullOrEmpty<TypeParser.Type>())
			{
				return type;
			}
			GenericInstanceType genericInstanceType = new GenericInstanceType(type, generic_arguments.Length);
			Collection<TypeReference> genericArguments = genericInstanceType.GenericArguments;
			for (int i = 0; i < generic_arguments.Length; i++)
			{
				genericArguments.Add(TypeParser.GetTypeReference(type.Module, generic_arguments[i], false));
			}
			return genericInstanceType;
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00028D04 File Offset: 0x00026F04
		public static void SplitFullName(string fullname, out string @namespace, out string name)
		{
			int num = fullname.LastIndexOf('.');
			if (num == -1)
			{
				@namespace = string.Empty;
				name = fullname;
				return;
			}
			@namespace = fullname.Substring(0, num);
			name = fullname.Substring(num + 1);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00028D40 File Offset: 0x00026F40
		private static TypeReference CreateReference(TypeParser.Type type_info, ModuleDefinition module, IMetadataScope scope)
		{
			string text;
			string text2;
			TypeParser.SplitFullName(type_info.type_fullname, out text, out text2);
			TypeReference typeReference = new TypeReference(text, text2, module, scope);
			MetadataSystem.TryProcessPrimitiveTypeReference(typeReference);
			TypeParser.AdjustGenericParameters(typeReference);
			string[] nested_names = type_info.nested_names;
			if (nested_names.IsNullOrEmpty<string>())
			{
				return typeReference;
			}
			for (int i = 0; i < nested_names.Length; i++)
			{
				typeReference = new TypeReference(string.Empty, nested_names[i], module, null)
				{
					DeclaringType = typeReference
				};
				TypeParser.AdjustGenericParameters(typeReference);
			}
			return typeReference;
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00028DB4 File Offset: 0x00026FB4
		private static void AdjustGenericParameters(TypeReference type)
		{
			int num;
			if (!TypeParser.TryGetArity(type.Name, out num))
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				type.GenericParameters.Add(new GenericParameter(type));
			}
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00028DF0 File Offset: 0x00026FF0
		private static IMetadataScope GetMetadataScope(ModuleDefinition module, TypeParser.Type type_info)
		{
			if (string.IsNullOrEmpty(type_info.assembly))
			{
				return module.TypeSystem.CoreLibrary;
			}
			AssemblyNameReference assemblyNameReference = AssemblyNameReference.Parse(type_info.assembly);
			AssemblyNameReference assemblyNameReference2;
			if (!module.TryGetAssemblyNameReference(assemblyNameReference, out assemblyNameReference2))
			{
				return assemblyNameReference;
			}
			return assemblyNameReference2;
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00028E30 File Offset: 0x00027030
		private static bool TryGetDefinition(ModuleDefinition module, TypeParser.Type type_info, out TypeReference type)
		{
			type = null;
			if (!TypeParser.TryCurrentModule(module, type_info))
			{
				return false;
			}
			TypeDefinition typeDefinition = module.GetType(type_info.type_fullname);
			if (typeDefinition == null)
			{
				return false;
			}
			string[] nested_names = type_info.nested_names;
			if (!nested_names.IsNullOrEmpty<string>())
			{
				for (int i = 0; i < nested_names.Length; i++)
				{
					TypeDefinition nestedType = typeDefinition.GetNestedType(nested_names[i]);
					if (nestedType == null)
					{
						return false;
					}
					typeDefinition = nestedType;
				}
			}
			type = typeDefinition;
			return true;
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00028E8F File Offset: 0x0002708F
		private static bool TryCurrentModule(ModuleDefinition module, TypeParser.Type type_info)
		{
			return string.IsNullOrEmpty(type_info.assembly) || (module.assembly != null && module.assembly.Name.FullName == type_info.assembly);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00028EC8 File Offset: 0x000270C8
		public static string ToParseable(TypeReference type, bool top_level = true)
		{
			if (type == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			TypeParser.AppendType(type, stringBuilder, true, top_level);
			return stringBuilder.ToString();
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00028EF0 File Offset: 0x000270F0
		private static void AppendNamePart(string part, StringBuilder name)
		{
			foreach (char c in part)
			{
				if (TypeParser.IsDelimiter(c))
				{
					name.Append('\\');
				}
				name.Append(c);
			}
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00028F34 File Offset: 0x00027134
		private static void AppendType(TypeReference type, StringBuilder name, bool fq_name, bool top_level)
		{
			TypeReference elementType = type.GetElementType();
			TypeReference declaringType = elementType.DeclaringType;
			if (declaringType != null)
			{
				TypeParser.AppendType(declaringType, name, false, top_level);
				name.Append('+');
			}
			string @namespace = type.Namespace;
			if (!string.IsNullOrEmpty(@namespace))
			{
				TypeParser.AppendNamePart(@namespace, name);
				name.Append('.');
			}
			TypeParser.AppendNamePart(elementType.Name, name);
			if (!fq_name)
			{
				return;
			}
			if (type.IsTypeSpecification())
			{
				TypeParser.AppendTypeSpecification((TypeSpecification)type, name);
			}
			if (TypeParser.RequiresFullyQualifiedName(type, top_level))
			{
				name.Append(", ");
				name.Append(TypeParser.GetScopeFullName(type));
			}
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00028FC8 File Offset: 0x000271C8
		private static string GetScopeFullName(TypeReference type)
		{
			IMetadataScope scope = type.Scope;
			MetadataScopeType metadataScopeType = scope.MetadataScopeType;
			if (metadataScopeType == MetadataScopeType.AssemblyNameReference)
			{
				return ((AssemblyNameReference)scope).FullName;
			}
			if (metadataScopeType != MetadataScopeType.ModuleDefinition)
			{
				throw new ArgumentException();
			}
			return ((ModuleDefinition)scope).Assembly.Name.FullName;
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00029014 File Offset: 0x00027214
		private static void AppendTypeSpecification(TypeSpecification type, StringBuilder name)
		{
			if (type.ElementType.IsTypeSpecification())
			{
				TypeParser.AppendTypeSpecification((TypeSpecification)type.ElementType, name);
			}
			ElementType etype = type.etype;
			switch (etype)
			{
			case ElementType.Ptr:
				name.Append('*');
				return;
			case ElementType.ByRef:
				name.Append('&');
				return;
			case ElementType.ValueType:
			case ElementType.Class:
			case ElementType.Var:
				return;
			case ElementType.Array:
				break;
			case ElementType.GenericInst:
			{
				Collection<TypeReference> genericArguments = ((GenericInstanceType)type).GenericArguments;
				name.Append('[');
				for (int i = 0; i < genericArguments.Count; i++)
				{
					if (i > 0)
					{
						name.Append(',');
					}
					TypeReference typeReference = genericArguments[i];
					bool flag = typeReference.Scope != typeReference.Module;
					if (flag)
					{
						name.Append('[');
					}
					TypeParser.AppendType(typeReference, name, true, false);
					if (flag)
					{
						name.Append(']');
					}
				}
				name.Append(']');
				return;
			}
			default:
				if (etype != ElementType.SzArray)
				{
					return;
				}
				break;
			}
			ArrayType arrayType = (ArrayType)type;
			if (arrayType.IsVector)
			{
				name.Append("[]");
				return;
			}
			name.Append('[');
			for (int j = 1; j < arrayType.Rank; j++)
			{
				name.Append(',');
			}
			name.Append(']');
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0002914E File Offset: 0x0002734E
		private static bool RequiresFullyQualifiedName(TypeReference type, bool top_level)
		{
			return type.Scope != type.Module && (!(type.Scope.Name == "mscorlib") || !top_level);
		}

		// Token: 0x0400050A RID: 1290
		private readonly string fullname;

		// Token: 0x0400050B RID: 1291
		private readonly int length;

		// Token: 0x0400050C RID: 1292
		private int position;

		// Token: 0x0200017D RID: 381
		private class Type
		{
			// Token: 0x0400050D RID: 1293
			public const int Ptr = -1;

			// Token: 0x0400050E RID: 1294
			public const int ByRef = -2;

			// Token: 0x0400050F RID: 1295
			public const int SzArray = -3;

			// Token: 0x04000510 RID: 1296
			public string type_fullname;

			// Token: 0x04000511 RID: 1297
			public string[] nested_names;

			// Token: 0x04000512 RID: 1298
			public int arity;

			// Token: 0x04000513 RID: 1299
			public int[] specs;

			// Token: 0x04000514 RID: 1300
			public TypeParser.Type[] generic_arguments;

			// Token: 0x04000515 RID: 1301
			public string assembly;
		}
	}
}
