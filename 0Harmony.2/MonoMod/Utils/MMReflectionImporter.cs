﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x02000448 RID: 1096
	internal sealed class MMReflectionImporter : IReflectionImporter
	{
		// Token: 0x0600174F RID: 5967 RVA: 0x000503E8 File Offset: 0x0004E5E8
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

		// Token: 0x06001750 RID: 5968 RVA: 0x00050636 File Offset: 0x0004E836
		private bool TryGetCachedType(Type type, out TypeReference typeRef, MMReflectionImporter.GenericImportKind importKind)
		{
			if (importKind == MMReflectionImporter.GenericImportKind.Definition)
			{
				typeRef = null;
				return false;
			}
			return this.CachedTypes.TryGetValue(type, out typeRef);
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00050650 File Offset: 0x0004E850
		private TypeReference SetCachedType(Type type, TypeReference typeRef, MMReflectionImporter.GenericImportKind importKind)
		{
			if (importKind == MMReflectionImporter.GenericImportKind.Definition)
			{
				return typeRef;
			}
			this.CachedTypes[type] = typeRef;
			return typeRef;
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x00050673 File Offset: 0x0004E873
		[Obsolete("Please use the Assembly overload instead.")]
		public AssemblyNameReference ImportReference(AssemblyName asm)
		{
			return this.Default.ImportReference(asm);
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00050684 File Offset: 0x0004E884
		public AssemblyNameReference ImportReference(Assembly asm)
		{
			AssemblyNameReference assemblyNameReference;
			if (this.CachedAsms.TryGetValue(asm, out assemblyNameReference))
			{
				return assemblyNameReference;
			}
			assemblyNameReference = this.Default.ImportReference(asm.GetName());
			assemblyNameReference.ApplyRuntimeHash(asm);
			return this.CachedAsms[asm] = assemblyNameReference;
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x000506CC File Offset: 0x0004E8CC
		public TypeReference ImportModuleType(Module module, IGenericParameterProvider context)
		{
			TypeReference typeReference;
			if (this.CachedModuleTypes.TryGetValue(module, out typeReference))
			{
				return typeReference;
			}
			return this.CachedModuleTypes[module] = new TypeReference(string.Empty, "<Module>", this.Module, this.ImportReference(module.Assembly));
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0005071B File Offset: 0x0004E91B
		public TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			return this._ImportReference(type, context, (context != null) ? MMReflectionImporter.GenericImportKind.Open : MMReflectionImporter.GenericImportKind.Definition);
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x0005072C File Offset: 0x0004E92C
		private bool _IsGenericInstance(Type type, MMReflectionImporter.GenericImportKind importKind)
		{
			return (type.IsGenericType && !type.IsGenericTypeDefinition) || (type.IsGenericType && type.IsGenericTypeDefinition && importKind == MMReflectionImporter.GenericImportKind.Open);
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00050758 File Offset: 0x0004E958
		private GenericInstanceType _ImportGenericInstance(Type type, IGenericParameterProvider context, TypeReference typeRef)
		{
			GenericInstanceType genericInstanceType = new GenericInstanceType(typeRef);
			foreach (Type type2 in type.GetGenericArguments())
			{
				genericInstanceType.GenericArguments.Add(this._ImportReference(type2, context, MMReflectionImporter.GenericImportKind.Open));
			}
			return genericInstanceType;
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x0005079C File Offset: 0x0004E99C
		private TypeReference _ImportReference(Type type, IGenericParameterProvider context, MMReflectionImporter.GenericImportKind importKind = MMReflectionImporter.GenericImportKind.Open)
		{
			TypeReference typeReference;
			if (this.TryGetCachedType(type, out typeReference, importKind))
			{
				if (!this._IsGenericInstance(type, importKind))
				{
					return typeReference;
				}
				return this._ImportGenericInstance(type, context, typeReference);
			}
			else
			{
				if (this.UseDefault)
				{
					return this.SetCachedType(type, this.Default.ImportReference(type, context), importKind);
				}
				if (type.HasElementType)
				{
					if (type.IsByRef)
					{
						return this.SetCachedType(type, new ByReferenceType(this._ImportReference(type.GetElementType(), context, MMReflectionImporter.GenericImportKind.Open)), importKind);
					}
					if (type.IsPointer)
					{
						return this.SetCachedType(type, new PointerType(this._ImportReference(type.GetElementType(), context, MMReflectionImporter.GenericImportKind.Open)), importKind);
					}
					if (type.IsArray)
					{
						ArrayType arrayType = new ArrayType(this._ImportReference(type.GetElementType(), context, MMReflectionImporter.GenericImportKind.Open), type.GetArrayRank());
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
				if (this._IsGenericInstance(type, importKind))
				{
					return this._ImportGenericInstance(type, context, this._ImportReference(type.GetGenericTypeDefinition(), context, MMReflectionImporter.GenericImportKind.Definition));
				}
				if (type.IsGenericParameter)
				{
					return this.SetCachedType(type, MMReflectionImporter.ImportGenericParameter(type, context), importKind);
				}
				if (this.ElementTypes.TryGetValue(type, out typeReference))
				{
					return this.SetCachedType(type, typeReference, importKind);
				}
				typeReference = new TypeReference(string.Empty, type.Name, this.Module, this.ImportReference(type.Assembly), type.IsValueType);
				if (type.IsNested)
				{
					typeReference.DeclaringType = this._ImportReference(type.DeclaringType, context, importKind);
				}
				else if (type.Namespace != null)
				{
					typeReference.Namespace = type.Namespace;
				}
				if (type.IsGenericType)
				{
					foreach (Type type2 in type.GetGenericArguments())
					{
						typeReference.GenericParameters.Add(new GenericParameter(type2.Name, typeReference));
					}
				}
				return this.SetCachedType(type, typeReference, importKind);
			}
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x000509B0 File Offset: 0x0004EBB0
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

		// Token: 0x0600175A RID: 5978 RVA: 0x00050A60 File Offset: 0x0004EC60
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
			return this.CachedFields[fieldInfo] = new FieldReference(field.Name, this._ImportReference(field.FieldType, typeReference, MMReflectionImporter.GenericImportKind.Open), typeReference);
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x00050B1F File Offset: 0x0004ED1F
		public MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			return this._ImportReference(method, context, (context != null) ? MMReflectionImporter.GenericImportKind.Open : MMReflectionImporter.GenericImportKind.Definition);
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00050B30 File Offset: 0x0004ED30
		private MethodReference _ImportReference(MethodBase method, IGenericParameterProvider context, MMReflectionImporter.GenericImportKind importKind)
		{
			MethodReference methodReference;
			if (this.CachedMethods.TryGetValue(method, out methodReference) && importKind == MMReflectionImporter.GenericImportKind.Open)
			{
				return methodReference;
			}
			MethodInfo methodInfo = method as MethodInfo;
			if (methodInfo != null && methodInfo.IsDynamicMethod())
			{
				return new DynamicMethodReference(this.Module, methodInfo);
			}
			if (this.UseDefault)
			{
				return this.CachedMethods[method] = this.Default.ImportReference(method, context);
			}
			if ((method.IsGenericMethod && !method.IsGenericMethodDefinition) || (method.IsGenericMethod && method.IsGenericMethodDefinition && importKind == MMReflectionImporter.GenericImportKind.Open))
			{
				GenericInstanceMethod genericInstanceMethod = new GenericInstanceMethod(this._ImportReference((method as MethodInfo).GetGenericMethodDefinition(), context, MMReflectionImporter.GenericImportKind.Definition));
				foreach (Type type in method.GetGenericArguments())
				{
					genericInstanceMethod.GenericArguments.Add(this._ImportReference(type, context, MMReflectionImporter.GenericImportKind.Open));
				}
				return this.CachedMethods[method] = genericInstanceMethod;
			}
			Type declaringType = method.DeclaringType;
			methodReference = new MethodReference(method.Name, this._ImportReference(typeof(void), context, MMReflectionImporter.GenericImportKind.Open), (declaringType != null) ? this._ImportReference(declaringType, context, MMReflectionImporter.GenericImportKind.Definition) : this.ImportModuleType(method.Module, context));
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
			MethodInfo methodInfo2 = method as MethodInfo;
			methodReference2.ReturnType = this._ImportReference(((methodInfo2 != null) ? methodInfo2.ReturnType : null) ?? typeof(void), methodReference, MMReflectionImporter.GenericImportKind.Open);
			foreach (ParameterInfo parameterInfo in method.GetParameters())
			{
				methodReference.Parameters.Add(new ParameterDefinition(parameterInfo.Name, (Mono.Cecil.ParameterAttributes)parameterInfo.Attributes, this._ImportReference(parameterInfo.ParameterType, methodReference, MMReflectionImporter.GenericImportKind.Open)));
			}
			return this.CachedMethods[methodBase] = methodReference;
		}

		// Token: 0x0400100C RID: 4108
		public static readonly IReflectionImporterProvider Provider = new MMReflectionImporter._Provider();

		// Token: 0x0400100D RID: 4109
		public static readonly IReflectionImporterProvider ProviderNoDefault = new MMReflectionImporter._Provider
		{
			UseDefault = new bool?(false)
		};

		// Token: 0x0400100E RID: 4110
		private readonly ModuleDefinition Module;

		// Token: 0x0400100F RID: 4111
		private readonly DefaultReflectionImporter Default;

		// Token: 0x04001010 RID: 4112
		private readonly Dictionary<Assembly, AssemblyNameReference> CachedAsms = new Dictionary<Assembly, AssemblyNameReference>();

		// Token: 0x04001011 RID: 4113
		private readonly Dictionary<Module, TypeReference> CachedModuleTypes = new Dictionary<Module, TypeReference>();

		// Token: 0x04001012 RID: 4114
		private readonly Dictionary<Type, TypeReference> CachedTypes = new Dictionary<Type, TypeReference>();

		// Token: 0x04001013 RID: 4115
		private readonly Dictionary<FieldInfo, FieldReference> CachedFields = new Dictionary<FieldInfo, FieldReference>();

		// Token: 0x04001014 RID: 4116
		private readonly Dictionary<MethodBase, MethodReference> CachedMethods = new Dictionary<MethodBase, MethodReference>();

		// Token: 0x04001015 RID: 4117
		public bool UseDefault;

		// Token: 0x04001016 RID: 4118
		private readonly Dictionary<Type, TypeReference> ElementTypes;

		// Token: 0x02000449 RID: 1097
		private class _Provider : IReflectionImporterProvider
		{
			// Token: 0x0600175E RID: 5982 RVA: 0x00050DB8 File Offset: 0x0004EFB8
			public IReflectionImporter GetReflectionImporter(ModuleDefinition module)
			{
				MMReflectionImporter mmreflectionImporter = new MMReflectionImporter(module);
				if (this.UseDefault != null)
				{
					mmreflectionImporter.UseDefault = this.UseDefault.Value;
				}
				return mmreflectionImporter;
			}

			// Token: 0x04001017 RID: 4119
			public bool? UseDefault;
		}

		// Token: 0x0200044A RID: 1098
		private enum GenericImportKind
		{
			// Token: 0x04001019 RID: 4121
			Open,
			// Token: 0x0400101A RID: 4122
			Definition
		}
	}
}
