using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200012D RID: 301
	internal class DefaultReflectionImporter : IReflectionImporter
	{
		// Token: 0x06000874 RID: 2164 RVA: 0x00021F30 File Offset: 0x00020130
		public DefaultReflectionImporter(ModuleDefinition module)
		{
			Mixin.CheckModule(module);
			this.module = module;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00021F45 File Offset: 0x00020145
		private TypeReference ImportType(Type type, ImportGenericContext context)
		{
			return this.ImportType(type, context, DefaultReflectionImporter.ImportGenericKind.Open);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00021F50 File Offset: 0x00020150
		private TypeReference ImportType(Type type, ImportGenericContext context, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			if (DefaultReflectionImporter.IsTypeSpecification(type) || DefaultReflectionImporter.ImportOpenGenericType(type, import_kind))
			{
				return this.ImportTypeSpecification(type, context);
			}
			TypeReference typeReference = new TypeReference(string.Empty, type.Name, this.module, this.ImportScope(type), type.IsValueType);
			typeReference.etype = DefaultReflectionImporter.ImportElementType(type);
			if (DefaultReflectionImporter.IsNestedType(type))
			{
				typeReference.DeclaringType = this.ImportType(type.DeclaringType, context, import_kind);
			}
			else
			{
				typeReference.Namespace = type.Namespace ?? string.Empty;
			}
			if (type.IsGenericType)
			{
				DefaultReflectionImporter.ImportGenericParameters(typeReference, type.GetGenericArguments());
			}
			return typeReference;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00021FEF File Offset: 0x000201EF
		protected virtual IMetadataScope ImportScope(Type type)
		{
			return this.ImportScope(type.Assembly);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00021FFD File Offset: 0x000201FD
		private static bool ImportOpenGenericType(Type type, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			return type.IsGenericType && type.IsGenericTypeDefinition && import_kind == DefaultReflectionImporter.ImportGenericKind.Open;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00022015 File Offset: 0x00020215
		private static bool ImportOpenGenericMethod(MethodBase method, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			return method.IsGenericMethod && method.IsGenericMethodDefinition && import_kind == DefaultReflectionImporter.ImportGenericKind.Open;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0002202D File Offset: 0x0002022D
		private static bool IsNestedType(Type type)
		{
			return type.IsNested;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00022038 File Offset: 0x00020238
		private TypeReference ImportTypeSpecification(Type type, ImportGenericContext context)
		{
			if (type.IsByRef)
			{
				return new ByReferenceType(this.ImportType(type.GetElementType(), context));
			}
			if (type.IsPointer)
			{
				return new PointerType(this.ImportType(type.GetElementType(), context));
			}
			if (type.IsArray)
			{
				return new ArrayType(this.ImportType(type.GetElementType(), context), type.GetArrayRank());
			}
			if (type.IsGenericType)
			{
				return this.ImportGenericInstance(type, context);
			}
			if (type.IsGenericParameter)
			{
				return DefaultReflectionImporter.ImportGenericParameter(type, context);
			}
			throw new NotSupportedException(type.FullName);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000220C8 File Offset: 0x000202C8
		private static TypeReference ImportGenericParameter(Type type, ImportGenericContext context)
		{
			if (context.IsEmpty)
			{
				throw new InvalidOperationException();
			}
			if (type.DeclaringMethod != null)
			{
				return context.MethodParameter(DefaultReflectionImporter.NormalizeMethodName(type.DeclaringMethod), type.GenericParameterPosition);
			}
			if (type.DeclaringType != null)
			{
				return context.TypeParameter(DefaultReflectionImporter.NormalizeTypeFullName(type.DeclaringType), type.GenericParameterPosition);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00022137 File Offset: 0x00020337
		private static string NormalizeMethodName(MethodBase method)
		{
			return DefaultReflectionImporter.NormalizeTypeFullName(method.DeclaringType) + "." + method.Name;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00022154 File Offset: 0x00020354
		private static string NormalizeTypeFullName(Type type)
		{
			if (DefaultReflectionImporter.IsNestedType(type))
			{
				return DefaultReflectionImporter.NormalizeTypeFullName(type.DeclaringType) + "/" + type.Name;
			}
			return type.FullName;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00022180 File Offset: 0x00020380
		private TypeReference ImportGenericInstance(Type type, ImportGenericContext context)
		{
			TypeReference typeReference = this.ImportType(type.GetGenericTypeDefinition(), context, DefaultReflectionImporter.ImportGenericKind.Definition);
			Type[] genericArguments = type.GetGenericArguments();
			GenericInstanceType genericInstanceType = new GenericInstanceType(typeReference, genericArguments.Length);
			Collection<TypeReference> genericArguments2 = genericInstanceType.GenericArguments;
			context.Push(typeReference);
			TypeReference typeReference2;
			try
			{
				for (int i = 0; i < genericArguments.Length; i++)
				{
					genericArguments2.Add(this.ImportType(genericArguments[i], context));
				}
				typeReference2 = genericInstanceType;
			}
			finally
			{
				context.Pop();
			}
			return typeReference2;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00022200 File Offset: 0x00020400
		private static bool IsTypeSpecification(Type type)
		{
			return type.HasElementType || DefaultReflectionImporter.IsGenericInstance(type) || type.IsGenericParameter;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0002221A File Offset: 0x0002041A
		private static bool IsGenericInstance(Type type)
		{
			return type.IsGenericType && !type.IsGenericTypeDefinition;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00022230 File Offset: 0x00020430
		private static ElementType ImportElementType(Type type)
		{
			ElementType elementType;
			if (!DefaultReflectionImporter.type_etype_mapping.TryGetValue(type, out elementType))
			{
				return ElementType.None;
			}
			return elementType;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0002224F File Offset: 0x0002044F
		protected AssemblyNameReference ImportScope(Assembly assembly)
		{
			return this.ImportReference(assembly.GetName());
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00022260 File Offset: 0x00020460
		public virtual AssemblyNameReference ImportReference(AssemblyName name)
		{
			Mixin.CheckName(name);
			AssemblyNameReference assemblyNameReference;
			if (this.TryGetAssemblyNameReference(name, out assemblyNameReference))
			{
				return assemblyNameReference;
			}
			assemblyNameReference = new AssemblyNameReference(name.Name, name.Version)
			{
				PublicKeyToken = name.GetPublicKeyToken(),
				Culture = name.CultureInfo.Name,
				HashAlgorithm = (AssemblyHashAlgorithm)name.HashAlgorithm
			};
			this.module.AssemblyReferences.Add(assemblyNameReference);
			return assemblyNameReference;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x000222D0 File Offset: 0x000204D0
		private bool TryGetAssemblyNameReference(AssemblyName name, out AssemblyNameReference assembly_reference)
		{
			Collection<AssemblyNameReference> assemblyReferences = this.module.AssemblyReferences;
			for (int i = 0; i < assemblyReferences.Count; i++)
			{
				AssemblyNameReference assemblyNameReference = assemblyReferences[i];
				if (!(name.FullName != assemblyNameReference.FullName))
				{
					assembly_reference = assemblyNameReference;
					return true;
				}
			}
			assembly_reference = null;
			return false;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00022320 File Offset: 0x00020520
		private FieldReference ImportField(FieldInfo field, ImportGenericContext context)
		{
			TypeReference typeReference = this.ImportType(field.DeclaringType, context);
			if (DefaultReflectionImporter.IsGenericInstance(field.DeclaringType))
			{
				field = DefaultReflectionImporter.ResolveFieldDefinition(field);
			}
			context.Push(typeReference);
			FieldReference fieldReference;
			try
			{
				fieldReference = new FieldReference
				{
					Name = field.Name,
					DeclaringType = typeReference,
					FieldType = this.ImportType(field.FieldType, context)
				};
			}
			finally
			{
				context.Pop();
			}
			return fieldReference;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x000223A0 File Offset: 0x000205A0
		private static FieldInfo ResolveFieldDefinition(FieldInfo field)
		{
			return field.Module.ResolveField(field.MetadataToken);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000223B3 File Offset: 0x000205B3
		private static MethodBase ResolveMethodDefinition(MethodBase method)
		{
			return method.Module.ResolveMethod(method.MetadataToken);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x000223C8 File Offset: 0x000205C8
		private MethodReference ImportMethod(MethodBase method, ImportGenericContext context, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			if (DefaultReflectionImporter.IsMethodSpecification(method) || DefaultReflectionImporter.ImportOpenGenericMethod(method, import_kind))
			{
				return this.ImportMethodSpecification(method, context);
			}
			TypeReference typeReference = this.ImportType(method.DeclaringType, context);
			if (DefaultReflectionImporter.IsGenericInstance(method.DeclaringType))
			{
				method = DefaultReflectionImporter.ResolveMethodDefinition(method);
			}
			MethodReference methodReference = new MethodReference
			{
				Name = method.Name,
				HasThis = DefaultReflectionImporter.HasCallingConvention(method, CallingConventions.HasThis),
				ExplicitThis = DefaultReflectionImporter.HasCallingConvention(method, CallingConventions.ExplicitThis),
				DeclaringType = this.ImportType(method.DeclaringType, context, DefaultReflectionImporter.ImportGenericKind.Definition)
			};
			if (DefaultReflectionImporter.HasCallingConvention(method, CallingConventions.VarArgs))
			{
				methodReference.CallingConvention &= MethodCallingConvention.VarArg;
			}
			if (method.IsGenericMethod)
			{
				DefaultReflectionImporter.ImportGenericParameters(methodReference, method.GetGenericArguments());
			}
			context.Push(methodReference);
			MethodReference methodReference2;
			try
			{
				MethodInfo methodInfo = method as MethodInfo;
				methodReference.ReturnType = ((methodInfo != null) ? this.ImportType(methodInfo.ReturnType, context) : this.ImportType(typeof(void), default(ImportGenericContext)));
				ParameterInfo[] parameters = method.GetParameters();
				Collection<ParameterDefinition> parameters2 = methodReference.Parameters;
				for (int i = 0; i < parameters.Length; i++)
				{
					parameters2.Add(new ParameterDefinition(this.ImportType(parameters[i].ParameterType, context)));
				}
				methodReference.DeclaringType = typeReference;
				methodReference2 = methodReference;
			}
			finally
			{
				context.Pop();
			}
			return methodReference2;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0002252C File Offset: 0x0002072C
		private static void ImportGenericParameters(IGenericParameterProvider provider, Type[] arguments)
		{
			Collection<GenericParameter> genericParameters = provider.GenericParameters;
			for (int i = 0; i < arguments.Length; i++)
			{
				genericParameters.Add(new GenericParameter(arguments[i].Name, provider));
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00022562 File Offset: 0x00020762
		private static bool IsMethodSpecification(MethodBase method)
		{
			return method.IsGenericMethod && !method.IsGenericMethodDefinition;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00022578 File Offset: 0x00020778
		private MethodReference ImportMethodSpecification(MethodBase method, ImportGenericContext context)
		{
			MethodInfo methodInfo = method as MethodInfo;
			if (methodInfo == null)
			{
				throw new InvalidOperationException();
			}
			MethodReference methodReference = this.ImportMethod(methodInfo.GetGenericMethodDefinition(), context, DefaultReflectionImporter.ImportGenericKind.Definition);
			GenericInstanceMethod genericInstanceMethod = new GenericInstanceMethod(methodReference);
			Type[] genericArguments = method.GetGenericArguments();
			Collection<TypeReference> genericArguments2 = genericInstanceMethod.GenericArguments;
			context.Push(methodReference);
			MethodReference methodReference2;
			try
			{
				for (int i = 0; i < genericArguments.Length; i++)
				{
					genericArguments2.Add(this.ImportType(genericArguments[i], context));
				}
				methodReference2 = genericInstanceMethod;
			}
			finally
			{
				context.Pop();
			}
			return methodReference2;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0002260C File Offset: 0x0002080C
		private static bool HasCallingConvention(MethodBase method, CallingConventions conventions)
		{
			return (method.CallingConvention & conventions) > (CallingConventions)0;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00022619 File Offset: 0x00020819
		public virtual TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			Mixin.CheckType(type);
			return this.ImportType(type, ImportGenericContext.For(context), (context != null) ? DefaultReflectionImporter.ImportGenericKind.Open : DefaultReflectionImporter.ImportGenericKind.Definition);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00022635 File Offset: 0x00020835
		public virtual FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			Mixin.CheckField(field);
			return this.ImportField(field, ImportGenericContext.For(context));
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0002264A File Offset: 0x0002084A
		public virtual MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			Mixin.CheckMethod(method);
			return this.ImportMethod(method, ImportGenericContext.For(context), (context != null) ? DefaultReflectionImporter.ImportGenericKind.Open : DefaultReflectionImporter.ImportGenericKind.Definition);
		}

		// Token: 0x04000311 RID: 785
		protected readonly ModuleDefinition module;

		// Token: 0x04000312 RID: 786
		private static readonly Dictionary<Type, ElementType> type_etype_mapping = new Dictionary<Type, ElementType>(18)
		{
			{
				typeof(void),
				ElementType.Void
			},
			{
				typeof(bool),
				ElementType.Boolean
			},
			{
				typeof(char),
				ElementType.Char
			},
			{
				typeof(sbyte),
				ElementType.I1
			},
			{
				typeof(byte),
				ElementType.U1
			},
			{
				typeof(short),
				ElementType.I2
			},
			{
				typeof(ushort),
				ElementType.U2
			},
			{
				typeof(int),
				ElementType.I4
			},
			{
				typeof(uint),
				ElementType.U4
			},
			{
				typeof(long),
				ElementType.I8
			},
			{
				typeof(ulong),
				ElementType.U8
			},
			{
				typeof(float),
				ElementType.R4
			},
			{
				typeof(double),
				ElementType.R8
			},
			{
				typeof(string),
				ElementType.String
			},
			{
				typeof(TypedReference),
				ElementType.TypedByRef
			},
			{
				typeof(IntPtr),
				ElementType.I
			},
			{
				typeof(UIntPtr),
				ElementType.U
			},
			{
				typeof(object),
				ElementType.Object
			}
		};

		// Token: 0x0200012E RID: 302
		private enum ImportGenericKind
		{
			// Token: 0x04000314 RID: 788
			Definition,
			// Token: 0x04000315 RID: 789
			Open
		}
	}
}
