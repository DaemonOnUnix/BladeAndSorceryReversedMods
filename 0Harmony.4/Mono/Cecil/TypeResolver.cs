using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil
{
	// Token: 0x02000275 RID: 629
	internal sealed class TypeResolver
	{
		// Token: 0x06000FA9 RID: 4009 RVA: 0x0003009F File Offset: 0x0002E29F
		public static TypeResolver For(TypeReference typeReference)
		{
			if (!typeReference.IsGenericInstance)
			{
				return new TypeResolver();
			}
			return new TypeResolver((GenericInstanceType)typeReference);
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x000300BA File Offset: 0x0002E2BA
		public static TypeResolver For(TypeReference typeReference, MethodReference methodReference)
		{
			return new TypeResolver(typeReference as GenericInstanceType, methodReference as GenericInstanceMethod);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00002AED File Offset: 0x00000CED
		public TypeResolver()
		{
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x000300CD File Offset: 0x0002E2CD
		public TypeResolver(GenericInstanceType typeDefinitionContext)
		{
			this._typeDefinitionContext = typeDefinitionContext;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x000300DC File Offset: 0x0002E2DC
		public TypeResolver(GenericInstanceMethod methodDefinitionContext)
		{
			this._methodDefinitionContext = methodDefinitionContext;
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x000300EB File Offset: 0x0002E2EB
		public TypeResolver(GenericInstanceType typeDefinitionContext, GenericInstanceMethod methodDefinitionContext)
		{
			this._typeDefinitionContext = typeDefinitionContext;
			this._methodDefinitionContext = methodDefinitionContext;
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00030104 File Offset: 0x0002E304
		public MethodReference Resolve(MethodReference method)
		{
			MethodReference methodReference = method;
			if (this.IsDummy())
			{
				return methodReference;
			}
			TypeReference typeReference = this.Resolve(method.DeclaringType);
			GenericInstanceMethod genericInstanceMethod = method as GenericInstanceMethod;
			if (genericInstanceMethod != null)
			{
				methodReference = new MethodReference(method.Name, method.ReturnType, typeReference);
				foreach (ParameterDefinition parameterDefinition in method.Parameters)
				{
					methodReference.Parameters.Add(new ParameterDefinition(parameterDefinition.Name, parameterDefinition.Attributes, parameterDefinition.ParameterType));
				}
				foreach (GenericParameter genericParameter in genericInstanceMethod.ElementMethod.GenericParameters)
				{
					methodReference.GenericParameters.Add(new GenericParameter(genericParameter.Name, methodReference));
				}
				methodReference.HasThis = method.HasThis;
				GenericInstanceMethod genericInstanceMethod2 = new GenericInstanceMethod(methodReference);
				foreach (TypeReference typeReference2 in genericInstanceMethod.GenericArguments)
				{
					genericInstanceMethod2.GenericArguments.Add(this.Resolve(typeReference2));
				}
				methodReference = genericInstanceMethod2;
			}
			else
			{
				methodReference = new MethodReference(method.Name, method.ReturnType, typeReference);
				foreach (GenericParameter genericParameter2 in method.GenericParameters)
				{
					methodReference.GenericParameters.Add(new GenericParameter(genericParameter2.Name, methodReference));
				}
				foreach (ParameterDefinition parameterDefinition2 in method.Parameters)
				{
					methodReference.Parameters.Add(new ParameterDefinition(parameterDefinition2.Name, parameterDefinition2.Attributes, parameterDefinition2.ParameterType));
				}
				methodReference.HasThis = method.HasThis;
			}
			return methodReference;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0003034C File Offset: 0x0002E54C
		public FieldReference Resolve(FieldReference field)
		{
			TypeReference typeReference = this.Resolve(field.DeclaringType);
			if (typeReference == field.DeclaringType)
			{
				return field;
			}
			return new FieldReference(field.Name, field.FieldType, typeReference);
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00030383 File Offset: 0x0002E583
		public TypeReference ResolveReturnType(MethodReference method)
		{
			return this.Resolve(GenericParameterResolver.ResolveReturnTypeIfNeeded(method));
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00030391 File Offset: 0x0002E591
		public TypeReference ResolveParameterType(MethodReference method, ParameterReference parameter)
		{
			return this.Resolve(GenericParameterResolver.ResolveParameterTypeIfNeeded(method, parameter));
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x000303A0 File Offset: 0x0002E5A0
		public TypeReference ResolveVariableType(MethodReference method, VariableReference variable)
		{
			return this.Resolve(GenericParameterResolver.ResolveVariableTypeIfNeeded(method, variable));
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x000303AF File Offset: 0x0002E5AF
		public TypeReference ResolveFieldType(FieldReference field)
		{
			return this.Resolve(GenericParameterResolver.ResolveFieldTypeIfNeeded(field));
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x000303BD File Offset: 0x0002E5BD
		public TypeReference Resolve(TypeReference typeReference)
		{
			return this.Resolve(typeReference, true);
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x000303C8 File Offset: 0x0002E5C8
		public TypeReference Resolve(TypeReference typeReference, bool includeTypeDefinitions)
		{
			if (this.IsDummy())
			{
				return typeReference;
			}
			if (this._typeDefinitionContext != null && this._typeDefinitionContext.GenericArguments.Contains(typeReference))
			{
				return typeReference;
			}
			if (this._methodDefinitionContext != null && this._methodDefinitionContext.GenericArguments.Contains(typeReference))
			{
				return typeReference;
			}
			GenericParameter genericParameter = typeReference as GenericParameter;
			if (genericParameter != null)
			{
				if (this._typeDefinitionContext != null && this._typeDefinitionContext.GenericArguments.Contains(genericParameter))
				{
					return genericParameter;
				}
				if (this._methodDefinitionContext != null && this._methodDefinitionContext.GenericArguments.Contains(genericParameter))
				{
					return genericParameter;
				}
				return this.ResolveGenericParameter(genericParameter);
			}
			else
			{
				ArrayType arrayType = typeReference as ArrayType;
				if (arrayType != null)
				{
					return new ArrayType(this.Resolve(arrayType.ElementType), arrayType.Rank);
				}
				PointerType pointerType = typeReference as PointerType;
				if (pointerType != null)
				{
					return new PointerType(this.Resolve(pointerType.ElementType));
				}
				ByReferenceType byReferenceType = typeReference as ByReferenceType;
				if (byReferenceType != null)
				{
					return new ByReferenceType(this.Resolve(byReferenceType.ElementType));
				}
				PinnedType pinnedType = typeReference as PinnedType;
				if (pinnedType != null)
				{
					return new PinnedType(this.Resolve(pinnedType.ElementType));
				}
				GenericInstanceType genericInstanceType = typeReference as GenericInstanceType;
				if (genericInstanceType != null)
				{
					GenericInstanceType genericInstanceType2 = new GenericInstanceType(genericInstanceType.ElementType);
					foreach (TypeReference typeReference2 in genericInstanceType.GenericArguments)
					{
						genericInstanceType2.GenericArguments.Add(this.Resolve(typeReference2));
					}
					return genericInstanceType2;
				}
				RequiredModifierType requiredModifierType = typeReference as RequiredModifierType;
				if (requiredModifierType != null)
				{
					return this.Resolve(requiredModifierType.ElementType, includeTypeDefinitions);
				}
				if (includeTypeDefinitions)
				{
					TypeDefinition typeDefinition = typeReference as TypeDefinition;
					if (typeDefinition != null && typeDefinition.HasGenericParameters)
					{
						GenericInstanceType genericInstanceType3 = new GenericInstanceType(typeDefinition);
						foreach (GenericParameter genericParameter2 in typeDefinition.GenericParameters)
						{
							genericInstanceType3.GenericArguments.Add(this.Resolve(genericParameter2));
						}
						return genericInstanceType3;
					}
				}
				if (typeReference is TypeSpecification)
				{
					throw new NotSupportedException(string.Format("The type {0} cannot be resolved correctly.", typeReference.FullName));
				}
				return typeReference;
			}
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00030604 File Offset: 0x0002E804
		internal TypeResolver Nested(GenericInstanceMethod genericInstanceMethod)
		{
			return new TypeResolver(this._typeDefinitionContext as GenericInstanceType, genericInstanceMethod);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00030618 File Offset: 0x0002E818
		private TypeReference ResolveGenericParameter(GenericParameter genericParameter)
		{
			if (genericParameter.Owner == null)
			{
				return this.HandleOwnerlessInvalidILCode(genericParameter);
			}
			if (!(genericParameter.Owner is MemberReference))
			{
				throw new NotSupportedException();
			}
			if (genericParameter.Type == GenericParameterType.Type)
			{
				return this._typeDefinitionContext.GenericArguments[genericParameter.Position];
			}
			if (this._methodDefinitionContext == null)
			{
				return genericParameter;
			}
			return this._methodDefinitionContext.GenericArguments[genericParameter.Position];
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00030688 File Offset: 0x0002E888
		private TypeReference HandleOwnerlessInvalidILCode(GenericParameter genericParameter)
		{
			if (genericParameter.Type == GenericParameterType.Method && this._typeDefinitionContext != null && genericParameter.Position < this._typeDefinitionContext.GenericArguments.Count)
			{
				return this._typeDefinitionContext.GenericArguments[genericParameter.Position];
			}
			return genericParameter.Module.TypeSystem.Object;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x000306E5 File Offset: 0x0002E8E5
		private bool IsDummy()
		{
			return this._typeDefinitionContext == null && this._methodDefinitionContext == null;
		}

		// Token: 0x04000573 RID: 1395
		private readonly IGenericInstance _typeDefinitionContext;

		// Token: 0x04000574 RID: 1396
		private readonly IGenericInstance _methodDefinitionContext;
	}
}
