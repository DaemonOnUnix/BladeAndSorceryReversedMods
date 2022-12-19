using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000270 RID: 624
	internal class TypeParser
	{
		// Token: 0x06000F5B RID: 3931 RVA: 0x0002ED9A File Offset: 0x0002CF9A
		private TypeParser(string fullname)
		{
			this.fullname = fullname;
			this.length = fullname.Length;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0002EDB8 File Offset: 0x0002CFB8
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

		// Token: 0x06000F5D RID: 3933 RVA: 0x0002EE1C File Offset: 0x0002D01C
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

		// Token: 0x06000F5E RID: 3934 RVA: 0x0002EE6C File Offset: 0x0002D06C
		private static bool TryGetArity(string name, out int arity)
		{
			arity = 0;
			int num = name.LastIndexOf('`');
			return num != -1 && TypeParser.ParseInt32(name.Substring(num + 1), out arity);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0002EE9A File Offset: 0x0002D09A
		private static bool ParseInt32(string value, out int result)
		{
			return int.TryParse(value, out result);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0002EEA4 File Offset: 0x0002D0A4
		private static void TryAddArity(string name, ref int arity)
		{
			int num;
			if (!TypeParser.TryGetArity(name, out num))
			{
				return;
			}
			arity += num;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0002EEC4 File Offset: 0x0002D0C4
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

		// Token: 0x06000F62 RID: 3938 RVA: 0x0002EF4B File Offset: 0x0002D14B
		private static bool IsDelimiter(char chr)
		{
			return "+,[]*&".IndexOf(chr) != -1;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0002EF5E File Offset: 0x0002D15E
		private void TryParseWhiteSpace()
		{
			while (this.position < this.length && char.IsWhiteSpace(this.fullname[this.position]))
			{
				this.position++;
			}
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0002EF98 File Offset: 0x0002D198
		private string[] ParseNestedNames()
		{
			string[] array = null;
			while (this.TryParse('+'))
			{
				TypeParser.Add<string>(ref array, this.ParsePart());
			}
			return array;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0002EFC1 File Offset: 0x0002D1C1
		private bool TryParse(char chr)
		{
			if (this.position < this.length && this.fullname[this.position] == chr)
			{
				this.position++;
				return true;
			}
			return false;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0002EFF6 File Offset: 0x0002D1F6
		private static void Add<T>(ref T[] array, T item)
		{
			array = array.Add(item);
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0002F004 File Offset: 0x0002D204
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

		// Token: 0x06000F68 RID: 3944 RVA: 0x0002F10C File Offset: 0x0002D30C
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

		// Token: 0x06000F69 RID: 3945 RVA: 0x0002F194 File Offset: 0x0002D394
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

		// Token: 0x06000F6A RID: 3946 RVA: 0x0002F20C File Offset: 0x0002D40C
		public static TypeReference ParseType(ModuleDefinition module, string fullname, bool typeDefinitionOnly = false)
		{
			if (string.IsNullOrEmpty(fullname))
			{
				return null;
			}
			TypeParser typeParser = new TypeParser(fullname);
			return TypeParser.GetTypeReference(module, typeParser.ParseType(true), typeDefinitionOnly);
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0002F238 File Offset: 0x0002D438
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

		// Token: 0x06000F6C RID: 3948 RVA: 0x0002F26C File Offset: 0x0002D46C
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

		// Token: 0x06000F6D RID: 3949 RVA: 0x0002F318 File Offset: 0x0002D518
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

		// Token: 0x06000F6E RID: 3950 RVA: 0x0002F36C File Offset: 0x0002D56C
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

		// Token: 0x06000F6F RID: 3951 RVA: 0x0002F3A8 File Offset: 0x0002D5A8
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

		// Token: 0x06000F70 RID: 3952 RVA: 0x0002F41C File Offset: 0x0002D61C
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

		// Token: 0x06000F71 RID: 3953 RVA: 0x0002F458 File Offset: 0x0002D658
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

		// Token: 0x06000F72 RID: 3954 RVA: 0x0002F498 File Offset: 0x0002D698
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

		// Token: 0x06000F73 RID: 3955 RVA: 0x0002F4F7 File Offset: 0x0002D6F7
		private static bool TryCurrentModule(ModuleDefinition module, TypeParser.Type type_info)
		{
			return string.IsNullOrEmpty(type_info.assembly) || (module.assembly != null && module.assembly.Name.FullName == type_info.assembly);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0002F530 File Offset: 0x0002D730
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

		// Token: 0x06000F75 RID: 3957 RVA: 0x0002F558 File Offset: 0x0002D758
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

		// Token: 0x06000F76 RID: 3958 RVA: 0x0002F59C File Offset: 0x0002D79C
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

		// Token: 0x06000F77 RID: 3959 RVA: 0x0002F630 File Offset: 0x0002D830
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

		// Token: 0x06000F78 RID: 3960 RVA: 0x0002F67C File Offset: 0x0002D87C
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

		// Token: 0x06000F79 RID: 3961 RVA: 0x0002F7B6 File Offset: 0x0002D9B6
		private static bool RequiresFullyQualifiedName(TypeReference type, bool top_level)
		{
			return type.Scope != type.Module && (!(type.Scope.Name == "mscorlib") || !top_level);
		}

		// Token: 0x04000540 RID: 1344
		private readonly string fullname;

		// Token: 0x04000541 RID: 1345
		private readonly int length;

		// Token: 0x04000542 RID: 1346
		private int position;

		// Token: 0x02000271 RID: 625
		private class Type
		{
			// Token: 0x04000543 RID: 1347
			public const int Ptr = -1;

			// Token: 0x04000544 RID: 1348
			public const int ByRef = -2;

			// Token: 0x04000545 RID: 1349
			public const int SzArray = -3;

			// Token: 0x04000546 RID: 1350
			public string type_fullname;

			// Token: 0x04000547 RID: 1351
			public string[] nested_names;

			// Token: 0x04000548 RID: 1352
			public int arity;

			// Token: 0x04000549 RID: 1353
			public int[] specs;

			// Token: 0x0400054A RID: 1354
			public TypeParser.Type[] generic_arguments;

			// Token: 0x0400054B RID: 1355
			public string assembly;
		}
	}
}
