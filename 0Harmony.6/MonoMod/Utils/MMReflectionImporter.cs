using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x0200034E RID: 846
	internal sealed class MMReflectionImporter : IReflectionImporter
	{
		// Token: 0x060013C8 RID: 5064 RVA: 0x00047E78 File Offset: 0x00046078
		public MMReflectionImporter(ModuleDefinition module)
		{
			this.Module = module;
			this.Default = new DefaultReflectionImporter(module);
			this.ElementTypes = new Dictionary<Type, TypeReference>
			{
				{
					typeof(void),
					module.TypeSystem.Void
				},
				{
					typeof(bool),
					module.TypeSystem.Boolean
				},
				{
					typeof(char),
					module.TypeSystem.Char
				},
				{
					typeof(sbyte),
					module.TypeSystem.SByte
				},
				{
					typeof(byte),
					module.TypeSystem.Byte
				},
				{
					typeof(short),
					module.TypeSystem.Int16
				},
				{
					typeof(ushort),
					module.TypeSystem.UInt16
				},
				{
					typeof(int),
					module.TypeSystem.Int32
				},
				{
					typeof(uint),
					module.TypeSystem.UInt32
				},
				{
					typeof(long),
					module.TypeSystem.Int64
				},
				{
					typeof(ulong),
					module.TypeSystem.UInt64
				},
				{
					typeof(float),
					module.TypeSystem.Single
				},
				{
					typeof(double),
					module.TypeSystem.Double
				},
				{
					typeof(string),
					module.TypeSystem.String
				},
				{
					typeof(TypedReference),
					module.TypeSystem.TypedReference
				},
				{
					typeof(IntPtr),
					module.TypeSystem.IntPtr
				},
				{
					typeof(UIntPtr),
					module.TypeSystem.UIntPtr
				},
				{
					typeof(object),
					module.TypeSystem.Object
				}
			};
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x000480C8 File Offset: 0x000462C8
		public AssemblyNameReference ImportReference(AssemblyName asm)
		{
			AssemblyNameReference assemblyNameReference;
			if (this.CachedAsms.TryGetValue(asm, out assemblyNameReference))
			{
				return assemblyNameReference;
			}
			return this.CachedAsms[asm] = this.Default.ImportReference(asm);
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00048104 File Offset: 0x00046304
		public TypeReference ImportModuleType(Module module, IGenericParameterProvider context)
		{
			TypeReference typeReference;
			if (this.CachedModuleTypes.TryGetValue(module, out typeReference))
			{
				return typeReference;
			}
			return this.CachedModuleTypes[module] = new TypeReference(string.Empty, "<Module>", this.Module, this.ImportReference(module.Assembly.GetName()));
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00048158 File Offset: 0x00046358
		public TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			TypeReference typeReference;
			if (this.CachedTypes.TryGetValue(type, out typeReference))
			{
				return typeReference;
			}
			if (this.UseDefault)
			{
				return this.CachedTypes[type] = this.Default.ImportReference(type, context);
			}
			if (type.HasElementType)
			{
				if (type.IsByRef)
				{
					return this.CachedTypes[type] = new ByReferenceType(this.ImportReference(type.GetElementType(), context));
				}
				if (type.IsPointer)
				{
					return this.CachedTypes[type] = new PointerType(this.ImportReference(type.GetElementType(), context));
				}
				if (type.IsArray)
				{
					ArrayType arrayType = new ArrayType(this.ImportReference(type.GetElementType(), context), type.GetArrayRank());
					if (type != type.GetElementType().MakeArrayType())
					{
						for (int i = 0; i < arrayType.Rank; i++)
						{
							arrayType.Dimensions[i] = new ArrayDimension(new int?(0), null);
						}
					}
					return this.CachedTypes[type] = arrayType;
				}
			}
			if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				GenericInstanceType genericInstanceType = new GenericInstanceType(this.ImportReference(type.GetGenericTypeDefinition(), context));
				foreach (Type type2 in type.GetGenericArguments())
				{
					genericInstanceType.GenericArguments.Add(this.ImportReference(type2, context));
				}
				return genericInstanceType;
			}
			if (type.IsGenericParameter)
			{
				return this.CachedTypes[type] = MMReflectionImporter.ImportGenericParameter(type, context);
			}
			if (this.ElementTypes.TryGetValue(type, out typeReference))
			{
				return this.CachedTypes[type] = typeReference;
			}
			typeReference = new TypeReference(string.Empty, type.Name, this.Module, this.ImportReference(type.Assembly.GetName()), type.IsValueType);
			if (type.IsNested)
			{
				typeReference.DeclaringType = this.ImportReference(type.DeclaringType, context);
			}
			else if (type.Namespace != null)
			{
				typeReference.Namespace = type.Namespace;
			}
			if (type.IsGenericType)
			{
				foreach (Type type3 in type.GetGenericArguments())
				{
					typeReference.GenericParameters.Add(new GenericParameter(type3.Name, typeReference));
				}
			}
			return this.CachedTypes[type] = typeReference;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x000483C4 File Offset: 0x000465C4
		private static TypeReference ImportGenericParameter(Type type, IGenericParameterProvider context)
		{
			MethodReference methodReference = context as MethodReference;
			if (methodReference != null)
			{
				if (type.DeclaringMethod != null)
				{
					return methodReference.GenericParameters[type.GenericParameterPosition];
				}
				context = methodReference.DeclaringType;
			}
			Type declaringType = type.DeclaringType;
			if (declaringType == null)
			{
				throw new InvalidOperationException();
			}
			TypeReference typeReference = context as TypeReference;
			if (typeReference != null)
			{
				while (typeReference != null)
				{
					TypeReference elementType = typeReference.GetElementType();
					if (elementType.Is(declaringType))
					{
						return elementType.GenericParameters[type.GenericParameterPosition];
					}
					if (typeReference.Is(declaringType))
					{
						return typeReference.GenericParameters[type.GenericParameterPosition];
					}
					typeReference = typeReference.DeclaringType;
				}
			}
			throw new NotSupportedException();
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00048474 File Offset: 0x00046674
		public FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			FieldReference fieldReference;
			if (this.CachedFields.TryGetValue(field, out fieldReference))
			{
				return fieldReference;
			}
			if (this.UseDefault)
			{
				return this.CachedFields[field] = this.Default.ImportReference(field, context);
			}
			Type declaringType = field.DeclaringType;
			TypeReference typeReference = ((declaringType != null) ? this.ImportReference(declaringType, context) : this.ImportModuleType(field.Module, context));
			FieldInfo fieldInfo = field;
			if (declaringType != null && declaringType.IsGenericType)
			{
				field = field.Module.ResolveField(field.MetadataToken);
			}
			return this.CachedFields[fieldInfo] = new FieldReference(field.Name, this.ImportReference(field.FieldType, typeReference), typeReference);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00048534 File Offset: 0x00046734
		public MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			MethodReference methodReference;
			if (this.CachedMethods.TryGetValue(method, out methodReference))
			{
				return methodReference;
			}
			DynamicMethod dynamicMethod = method as DynamicMethod;
			if (dynamicMethod != null)
			{
				return new DynamicMethodReference(this.Module, dynamicMethod);
			}
			if (this.UseDefault)
			{
				return this.CachedMethods[method] = this.Default.ImportReference(method, context);
			}
			if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
			{
				GenericInstanceMethod genericInstanceMethod = new GenericInstanceMethod(this.ImportReference((method as MethodInfo).GetGenericMethodDefinition(), context));
				foreach (Type type in method.GetGenericArguments())
				{
					genericInstanceMethod.GenericArguments.Add(this.ImportReference(type, context));
				}
				return this.CachedMethods[method] = genericInstanceMethod;
			}
			Type declaringType = method.DeclaringType;
			methodReference = new MethodReference(method.Name, this.ImportReference(typeof(void), context), (declaringType != null) ? this.ImportReference(declaringType, context) : this.ImportModuleType(method.Module, context));
			methodReference.HasThis = (method.CallingConvention & CallingConventions.HasThis) > (CallingConventions)0;
			methodReference.ExplicitThis = (method.CallingConvention & CallingConventions.ExplicitThis) > (CallingConventions)0;
			if ((method.CallingConvention & CallingConventions.VarArgs) != (CallingConventions)0)
			{
				methodReference.CallingConvention = MethodCallingConvention.VarArg;
			}
			MethodBase methodBase = method;
			if (declaringType != null && declaringType.IsGenericType)
			{
				method = method.Module.ResolveMethod(method.MetadataToken);
			}
			if (method.IsGenericMethodDefinition)
			{
				foreach (Type type2 in method.GetGenericArguments())
				{
					methodReference.GenericParameters.Add(new GenericParameter(type2.Name, methodReference));
				}
			}
			MethodReference methodReference2 = methodReference;
			MethodInfo methodInfo = method as MethodInfo;
			methodReference2.ReturnType = this.ImportReference(((methodInfo != null) ? methodInfo.ReturnType : null) ?? typeof(void), methodReference);
			foreach (ParameterInfo parameterInfo in method.GetParameters())
			{
				methodReference.Parameters.Add(new ParameterDefinition(parameterInfo.Name, (Mono.Cecil.ParameterAttributes)parameterInfo.Attributes, this.ImportReference(parameterInfo.ParameterType, methodReference)));
			}
			return this.CachedMethods[methodBase] = methodReference;
		}

		// Token: 0x04000FC9 RID: 4041
		public static readonly IReflectionImporterProvider Provider = new MMReflectionImporter._Provider();

		// Token: 0x04000FCA RID: 4042
		public static readonly IReflectionImporterProvider ProviderNoDefault = new MMReflectionImporter._Provider
		{
			UseDefault = new bool?(false)
		};

		// Token: 0x04000FCB RID: 4043
		private readonly ModuleDefinition Module;

		// Token: 0x04000FCC RID: 4044
		private readonly DefaultReflectionImporter Default;

		// Token: 0x04000FCD RID: 4045
		private readonly Dictionary<AssemblyName, AssemblyNameReference> CachedAsms = new Dictionary<AssemblyName, AssemblyNameReference>();

		// Token: 0x04000FCE RID: 4046
		private readonly Dictionary<Module, TypeReference> CachedModuleTypes = new Dictionary<Module, TypeReference>();

		// Token: 0x04000FCF RID: 4047
		private readonly Dictionary<Type, TypeReference> CachedTypes = new Dictionary<Type, TypeReference>();

		// Token: 0x04000FD0 RID: 4048
		private readonly Dictionary<FieldInfo, FieldReference> CachedFields = new Dictionary<FieldInfo, FieldReference>();

		// Token: 0x04000FD1 RID: 4049
		private readonly Dictionary<MethodBase, MethodReference> CachedMethods = new Dictionary<MethodBase, MethodReference>();

		// Token: 0x04000FD2 RID: 4050
		public bool UseDefault;

		// Token: 0x04000FD3 RID: 4051
		private readonly Dictionary<Type, TypeReference> ElementTypes;

		// Token: 0x0200034F RID: 847
		private class _Provider : IReflectionImporterProvider
		{
			// Token: 0x060013D0 RID: 5072 RVA: 0x00048798 File Offset: 0x00046998
			public IReflectionImporter GetReflectionImporter(ModuleDefinition module)
			{
				MMReflectionImporter mmreflectionImporter = new MMReflectionImporter(module);
				if (this.UseDefault != null)
				{
					mmreflectionImporter.UseDefault = this.UseDefault.Value;
				}
				return mmreflectionImporter;
			}

			// Token: 0x04000FD4 RID: 4052
			public bool? UseDefault;
		}
	}
}
