using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200013D RID: 317
	internal class MetadataResolver : IMetadataResolver
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x00023347 File Offset: 0x00021547
		public IAssemblyResolver AssemblyResolver
		{
			get
			{
				return this.assembly_resolver;
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0002334F File Offset: 0x0002154F
		public MetadataResolver(IAssemblyResolver assemblyResolver)
		{
			if (assemblyResolver == null)
			{
				throw new ArgumentNullException("assemblyResolver");
			}
			this.assembly_resolver = assemblyResolver;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0002336C File Offset: 0x0002156C
		public virtual TypeDefinition Resolve(TypeReference type)
		{
			Mixin.CheckType(type);
			type = type.GetElementType();
			IMetadataScope scope = type.Scope;
			if (scope == null)
			{
				return null;
			}
			switch (scope.MetadataScopeType)
			{
			case MetadataScopeType.AssemblyNameReference:
			{
				AssemblyDefinition assemblyDefinition = this.assembly_resolver.Resolve((AssemblyNameReference)scope);
				if (assemblyDefinition == null)
				{
					return null;
				}
				return MetadataResolver.GetType(assemblyDefinition.MainModule, type);
			}
			case MetadataScopeType.ModuleReference:
			{
				Collection<ModuleDefinition> modules = type.Module.Assembly.Modules;
				ModuleReference moduleReference = (ModuleReference)scope;
				for (int i = 0; i < modules.Count; i++)
				{
					ModuleDefinition moduleDefinition = modules[i];
					if (moduleDefinition.Name == moduleReference.Name)
					{
						return MetadataResolver.GetType(moduleDefinition, type);
					}
				}
				break;
			}
			case MetadataScopeType.ModuleDefinition:
				return MetadataResolver.GetType((ModuleDefinition)scope, type);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00023440 File Offset: 0x00021640
		private static TypeDefinition GetType(ModuleDefinition module, TypeReference reference)
		{
			TypeDefinition typeDefinition = MetadataResolver.GetTypeDefinition(module, reference);
			if (typeDefinition != null)
			{
				return typeDefinition;
			}
			if (!module.HasExportedTypes)
			{
				return null;
			}
			Collection<ExportedType> exportedTypes = module.ExportedTypes;
			for (int i = 0; i < exportedTypes.Count; i++)
			{
				ExportedType exportedType = exportedTypes[i];
				if (!(exportedType.Name != reference.Name) && !(exportedType.Namespace != reference.Namespace))
				{
					return exportedType.Resolve();
				}
			}
			return null;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x000234B4 File Offset: 0x000216B4
		private static TypeDefinition GetTypeDefinition(ModuleDefinition module, TypeReference type)
		{
			if (!type.IsNested)
			{
				return module.GetType(type.Namespace, type.Name);
			}
			TypeDefinition typeDefinition = type.DeclaringType.Resolve();
			if (typeDefinition == null)
			{
				return null;
			}
			return typeDefinition.GetNestedType(type.TypeFullName());
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000234FC File Offset: 0x000216FC
		public virtual FieldDefinition Resolve(FieldReference field)
		{
			Mixin.CheckField(field);
			TypeDefinition typeDefinition = this.Resolve(field.DeclaringType);
			if (typeDefinition == null)
			{
				return null;
			}
			if (!typeDefinition.HasFields)
			{
				return null;
			}
			return this.GetField(typeDefinition, field);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00023534 File Offset: 0x00021734
		private FieldDefinition GetField(TypeDefinition type, FieldReference reference)
		{
			while (type != null)
			{
				FieldDefinition field = MetadataResolver.GetField(type.Fields, reference);
				if (field != null)
				{
					return field;
				}
				if (type.BaseType == null)
				{
					return null;
				}
				type = this.Resolve(type.BaseType);
			}
			return null;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00023574 File Offset: 0x00021774
		private static FieldDefinition GetField(Collection<FieldDefinition> fields, FieldReference reference)
		{
			for (int i = 0; i < fields.Count; i++)
			{
				FieldDefinition fieldDefinition = fields[i];
				if (!(fieldDefinition.Name != reference.Name) && MetadataResolver.AreSame(fieldDefinition.FieldType, reference.FieldType))
				{
					return fieldDefinition;
				}
			}
			return null;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x000235C4 File Offset: 0x000217C4
		public virtual MethodDefinition Resolve(MethodReference method)
		{
			Mixin.CheckMethod(method);
			TypeDefinition typeDefinition = this.Resolve(method.DeclaringType);
			if (typeDefinition == null)
			{
				return null;
			}
			method = method.GetElementMethod();
			if (!typeDefinition.HasMethods)
			{
				return null;
			}
			return this.GetMethod(typeDefinition, method);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00023604 File Offset: 0x00021804
		private MethodDefinition GetMethod(TypeDefinition type, MethodReference reference)
		{
			while (type != null)
			{
				MethodDefinition method = MetadataResolver.GetMethod(type.Methods, reference);
				if (method != null)
				{
					return method;
				}
				if (type.BaseType == null)
				{
					return null;
				}
				type = this.Resolve(type.BaseType);
			}
			return null;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00023644 File Offset: 0x00021844
		public static MethodDefinition GetMethod(Collection<MethodDefinition> methods, MethodReference reference)
		{
			for (int i = 0; i < methods.Count; i++)
			{
				MethodDefinition methodDefinition = methods[i];
				if (!(methodDefinition.Name != reference.Name) && methodDefinition.HasGenericParameters == reference.HasGenericParameters && (!methodDefinition.HasGenericParameters || methodDefinition.GenericParameters.Count == reference.GenericParameters.Count) && MetadataResolver.AreSame(methodDefinition.ReturnType, reference.ReturnType) && methodDefinition.IsVarArg() == reference.IsVarArg())
				{
					if (methodDefinition.IsVarArg() && MetadataResolver.IsVarArgCallTo(methodDefinition, reference))
					{
						return methodDefinition;
					}
					if (methodDefinition.HasParameters == reference.HasParameters)
					{
						if (!methodDefinition.HasParameters && !reference.HasParameters)
						{
							return methodDefinition;
						}
						if (MetadataResolver.AreSame(methodDefinition.Parameters, reference.Parameters))
						{
							return methodDefinition;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00023724 File Offset: 0x00021924
		private static bool AreSame(Collection<ParameterDefinition> a, Collection<ParameterDefinition> b)
		{
			int count = a.Count;
			if (count != b.Count)
			{
				return false;
			}
			if (count == 0)
			{
				return true;
			}
			for (int i = 0; i < count; i++)
			{
				if (!MetadataResolver.AreSame(a[i].ParameterType, b[i].ParameterType))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00023778 File Offset: 0x00021978
		private static bool IsVarArgCallTo(MethodDefinition method, MethodReference reference)
		{
			if (method.Parameters.Count >= reference.Parameters.Count)
			{
				return false;
			}
			if (reference.GetSentinelPosition() != method.Parameters.Count)
			{
				return false;
			}
			for (int i = 0; i < method.Parameters.Count; i++)
			{
				if (!MetadataResolver.AreSame(method.Parameters[i].ParameterType, reference.Parameters[i].ParameterType))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x000237F8 File Offset: 0x000219F8
		private static bool AreSame(TypeSpecification a, TypeSpecification b)
		{
			if (!MetadataResolver.AreSame(a.ElementType, b.ElementType))
			{
				return false;
			}
			if (a.IsGenericInstance)
			{
				return MetadataResolver.AreSame((GenericInstanceType)a, (GenericInstanceType)b);
			}
			if (a.IsRequiredModifier || a.IsOptionalModifier)
			{
				return MetadataResolver.AreSame((IModifierType)a, (IModifierType)b);
			}
			return !a.IsArray || MetadataResolver.AreSame((ArrayType)a, (ArrayType)b);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00023871 File Offset: 0x00021A71
		private static bool AreSame(ArrayType a, ArrayType b)
		{
			return a.Rank == b.Rank;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00023884 File Offset: 0x00021A84
		private static bool AreSame(IModifierType a, IModifierType b)
		{
			return MetadataResolver.AreSame(a.ModifierType, b.ModifierType);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00023898 File Offset: 0x00021A98
		private static bool AreSame(GenericInstanceType a, GenericInstanceType b)
		{
			if (a.GenericArguments.Count != b.GenericArguments.Count)
			{
				return false;
			}
			for (int i = 0; i < a.GenericArguments.Count; i++)
			{
				if (!MetadataResolver.AreSame(a.GenericArguments[i], b.GenericArguments[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x000238F7 File Offset: 0x00021AF7
		private static bool AreSame(GenericParameter a, GenericParameter b)
		{
			return a.Position == b.Position;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00023908 File Offset: 0x00021B08
		private static bool AreSame(TypeReference a, TypeReference b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.etype != b.etype)
			{
				return false;
			}
			if (a.IsGenericParameter)
			{
				return MetadataResolver.AreSame((GenericParameter)a, (GenericParameter)b);
			}
			if (a.IsTypeSpecification())
			{
				return MetadataResolver.AreSame((TypeSpecification)a, (TypeSpecification)b);
			}
			return !(a.Name != b.Name) && !(a.Namespace != b.Namespace) && MetadataResolver.AreSame(a.DeclaringType, b.DeclaringType);
		}

		// Token: 0x04000330 RID: 816
		private readonly IAssemblyResolver assembly_resolver;
	}
}
