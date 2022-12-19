using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000220 RID: 544
	internal class DefaultReflectionImporter : IReflectionImporter
	{
		// Token: 0x06000BB9 RID: 3001 RVA: 0x00028208 File Offset: 0x00026408
		public DefaultReflectionImporter(ModuleDefinition module)
		{
			Mixin.CheckModule(module);
			this.module = module;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0002821D File Offset: 0x0002641D
		private TypeReference ImportType(Type type, ImportGenericContext context)
		{
			return this.ImportType(type, context, DefaultReflectionImporter.ImportGenericKind.Open);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00028228 File Offset: 0x00026428
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

		// Token: 0x06000BBC RID: 3004 RVA: 0x000282C7 File Offset: 0x000264C7
		protected virtual IMetadataScope ImportScope(Type type)
		{
			return this.ImportScope(type.Assembly);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x000282D5 File Offset: 0x000264D5
		private static bool ImportOpenGenericType(Type type, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			return type.IsGenericType && type.IsGenericTypeDefinition && import_kind == DefaultReflectionImporter.ImportGenericKind.Open;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000282ED File Offset: 0x000264ED
		private static bool ImportOpenGenericMethod(MethodBase method, DefaultReflectionImporter.ImportGenericKind import_kind)
		{
			return method.IsGenericMethod && method.IsGenericMethodDefinition && import_kind == DefaultReflectionImporter.ImportGenericKind.Open;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00028305 File Offset: 0x00026505
		private static bool IsNestedType(Type type)
		{
			return type.IsNested;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00028310 File Offset: 0x00026510
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

		// Token: 0x06000BC1 RID: 3009 RVA: 0x000283A0 File Offset: 0x000265A0
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

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0002840F File Offset: 0x0002660F
		private static string NormalizeMethodName(MethodBase method)
		{
			return DefaultReflectionImporter.NormalizeTypeFullName(method.DeclaringType) + "." + method.Name;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002842C File Offset: 0x0002662C
		private static string NormalizeTypeFullName(Type type)
		{
			if (DefaultReflectionImporter.IsNestedType(type))
			{
				return DefaultReflectionImporter.NormalizeTypeFullName(type.DeclaringType) + "/" + type.Name;
			}
			return type.FullName;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00028458 File Offset: 0x00026658
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

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000284D8 File Offset: 0x000266D8
		private static bool IsTypeSpecification(Type type)
		{
			return type.HasElementType || DefaultReflectionImporter.IsGenericInstance(type) || type.IsGenericParameter;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000284F2 File Offset: 0x000266F2
		private static bool IsGenericInstance(Type type)
		{
			return type.IsGenericType && !type.IsGenericTypeDefinition;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00028508 File Offset: 0x00026708
		private static ElementType ImportElementType(Type type)
		{
			ElementType elementType;
			if (!DefaultReflectionImporter.type_etype_mapping.TryGetValue(type, out elementType))
			{
				return ElementType.None;
			}
			return elementType;
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00028527 File Offset: 0x00026727
		protected AssemblyNameReference ImportScope(Assembly assembly)
		{
			return this.ImportReference(assembly.GetName());
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00028538 File Offset: 0x00026738
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

		// Token: 0x06000BCA RID: 3018 RVA: 0x000285A8 File Offset: 0x000267A8
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

		// Token: 0x06000BCB RID: 3019 RVA: 0x000285F8 File Offset: 0x000267F8
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

		// Token: 0x06000BCC RID: 3020 RVA: 0x00028678 File Offset: 0x00026878
		private static FieldInfo ResolveFieldDefinition(FieldInfo field)
		{
			return field.Module.ResolveField(field.MetadataToken);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0002868B File Offset: 0x0002688B
		private static MethodBase ResolveMethodDefinition(MethodBase method)
		{
			return method.Module.ResolveMethod(method.MetadataToken);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000286A0 File Offset: 0x000268A0
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

		// Token: 0x06000BCF RID: 3023 RVA: 0x00028804 File Offset: 0x00026A04
		private static void ImportGenericParameters(IGenericParameterProvider provider, Type[] arguments)
		{
			Collection<GenericParameter> genericParameters = provider.GenericParameters;
			for (int i = 0; i < arguments.Length; i++)
			{
				genericParameters.Add(new GenericParameter(arguments[i].Name, provider));
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0002883A File Offset: 0x00026A3A
		private static bool IsMethodSpecification(MethodBase method)
		{
			return method.IsGenericMethod && !method.IsGenericMethodDefinition;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00028850 File Offset: 0x00026A50
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

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000288E4 File Offset: 0x00026AE4
		private static bool HasCallingConvention(MethodBase method, CallingConventions conventions)
		{
			return (method.CallingConvention & conventions) > (CallingConventions)0;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x000288F1 File Offset: 0x00026AF1
		public virtual TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			Mixin.CheckType(type);
			return this.ImportType(type, ImportGenericContext.For(context), (context != null) ? DefaultReflectionImporter.ImportGenericKind.Open : DefaultReflectionImporter.ImportGenericKind.Definition);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0002890D File Offset: 0x00026B0D
		public virtual FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			Mixin.CheckField(field);
			return this.ImportField(field, ImportGenericContext.For(context));
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00028922 File Offset: 0x00026B22
		public virtual MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			Mixin.CheckMethod(method);
			return this.ImportMethod(method, ImportGenericContext.For(context), (context != null) ? DefaultReflectionImporter.ImportGenericKind.Open : DefaultReflectionImporter.ImportGenericKind.Definition);
		}

		// Token: 0x04000343 RID: 835
		protected readonly ModuleDefinition module;

		// Token: 0x04000344 RID: 836
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

		// Token: 0x02000221 RID: 545
		private enum ImportGenericKind
		{
			// Token: 0x04000346 RID: 838
			Definition,
			// Token: 0x04000347 RID: 839
			Open
		}
	}
}
