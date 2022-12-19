using System;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001B7 RID: 439
	internal sealed class ImmediateModuleReader : ModuleReader
	{
		// Token: 0x060007F6 RID: 2038 RVA: 0x0001B0F8 File Offset: 0x000192F8
		public ImmediateModuleReader(Image image)
			: base(image, ReadingMode.Immediate)
		{
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0001B102 File Offset: 0x00019302
		protected override void ReadModule()
		{
			this.module.Read<ModuleDefinition>(this.module, delegate(ModuleDefinition module, MetadataReader reader)
			{
				base.ReadModuleManifest(reader);
				this.ReadModule(module, true);
			});
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001B124 File Offset: 0x00019324
		public void ReadModule(ModuleDefinition module, bool resolve_attributes)
		{
			this.resolve_attributes = resolve_attributes;
			if (module.HasAssemblyReferences)
			{
				Mixin.Read(module.AssemblyReferences);
			}
			if (module.HasResources)
			{
				Mixin.Read(module.Resources);
			}
			if (module.HasModuleReferences)
			{
				Mixin.Read(module.ModuleReferences);
			}
			if (module.HasTypes)
			{
				this.ReadTypes(module.Types);
			}
			if (module.HasExportedTypes)
			{
				Mixin.Read(module.ExportedTypes);
			}
			this.ReadCustomAttributes(module);
			AssemblyDefinition assembly = module.Assembly;
			if (module.kind == ModuleKind.NetModule || assembly == null)
			{
				return;
			}
			this.ReadCustomAttributes(assembly);
			this.ReadSecurityDeclarations(assembly);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0001B1C4 File Offset: 0x000193C4
		private void ReadTypes(Collection<TypeDefinition> types)
		{
			for (int i = 0; i < types.Count; i++)
			{
				this.ReadType(types[i]);
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0001B1F0 File Offset: 0x000193F0
		private void ReadType(TypeDefinition type)
		{
			this.ReadGenericParameters(type);
			if (type.HasInterfaces)
			{
				this.ReadInterfaces(type);
			}
			if (type.HasNestedTypes)
			{
				this.ReadTypes(type.NestedTypes);
			}
			if (type.HasLayoutInfo)
			{
				Mixin.Read(type.ClassSize);
			}
			if (type.HasFields)
			{
				this.ReadFields(type);
			}
			if (type.HasMethods)
			{
				this.ReadMethods(type);
			}
			if (type.HasProperties)
			{
				this.ReadProperties(type);
			}
			if (type.HasEvents)
			{
				this.ReadEvents(type);
			}
			this.ReadSecurityDeclarations(type);
			this.ReadCustomAttributes(type);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0001B28C File Offset: 0x0001948C
		private void ReadInterfaces(TypeDefinition type)
		{
			Collection<InterfaceImplementation> interfaces = type.Interfaces;
			for (int i = 0; i < interfaces.Count; i++)
			{
				this.ReadCustomAttributes(interfaces[i]);
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001B2C0 File Offset: 0x000194C0
		private void ReadGenericParameters(IGenericParameterProvider provider)
		{
			if (!provider.HasGenericParameters)
			{
				return;
			}
			Collection<GenericParameter> genericParameters = provider.GenericParameters;
			for (int i = 0; i < genericParameters.Count; i++)
			{
				GenericParameter genericParameter = genericParameters[i];
				if (genericParameter.HasConstraints)
				{
					this.ReadGenericParameterConstraints(genericParameter);
				}
				this.ReadCustomAttributes(genericParameter);
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001B30C File Offset: 0x0001950C
		private void ReadGenericParameterConstraints(GenericParameter parameter)
		{
			Collection<GenericParameterConstraint> constraints = parameter.Constraints;
			for (int i = 0; i < constraints.Count; i++)
			{
				this.ReadCustomAttributes(constraints[i]);
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001B340 File Offset: 0x00019540
		private void ReadSecurityDeclarations(ISecurityDeclarationProvider provider)
		{
			if (!provider.HasSecurityDeclarations)
			{
				return;
			}
			Collection<SecurityDeclaration> securityDeclarations = provider.SecurityDeclarations;
			if (!this.resolve_attributes)
			{
				return;
			}
			for (int i = 0; i < securityDeclarations.Count; i++)
			{
				Mixin.Read(securityDeclarations[i].SecurityAttributes);
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001B388 File Offset: 0x00019588
		private void ReadCustomAttributes(ICustomAttributeProvider provider)
		{
			if (!provider.HasCustomAttributes)
			{
				return;
			}
			Collection<CustomAttribute> customAttributes = provider.CustomAttributes;
			if (!this.resolve_attributes)
			{
				return;
			}
			for (int i = 0; i < customAttributes.Count; i++)
			{
				Mixin.Read(customAttributes[i].ConstructorArguments);
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001B3D0 File Offset: 0x000195D0
		private void ReadFields(TypeDefinition type)
		{
			Collection<FieldDefinition> fields = type.Fields;
			for (int i = 0; i < fields.Count; i++)
			{
				FieldDefinition fieldDefinition = fields[i];
				if (fieldDefinition.HasConstant)
				{
					Mixin.Read(fieldDefinition.Constant);
				}
				if (fieldDefinition.HasLayoutInfo)
				{
					Mixin.Read(fieldDefinition.Offset);
				}
				if (fieldDefinition.RVA > 0)
				{
					Mixin.Read(fieldDefinition.InitialValue);
				}
				if (fieldDefinition.HasMarshalInfo)
				{
					Mixin.Read(fieldDefinition.MarshalInfo);
				}
				this.ReadCustomAttributes(fieldDefinition);
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001B458 File Offset: 0x00019658
		private void ReadMethods(TypeDefinition type)
		{
			Collection<MethodDefinition> methods = type.Methods;
			for (int i = 0; i < methods.Count; i++)
			{
				MethodDefinition methodDefinition = methods[i];
				this.ReadGenericParameters(methodDefinition);
				if (methodDefinition.HasParameters)
				{
					this.ReadParameters(methodDefinition);
				}
				if (methodDefinition.HasOverrides)
				{
					Mixin.Read(methodDefinition.Overrides);
				}
				if (methodDefinition.IsPInvokeImpl)
				{
					Mixin.Read(methodDefinition.PInvokeInfo);
				}
				this.ReadSecurityDeclarations(methodDefinition);
				this.ReadCustomAttributes(methodDefinition);
				MethodReturnType methodReturnType = methodDefinition.MethodReturnType;
				if (methodReturnType.HasConstant)
				{
					Mixin.Read(methodReturnType.Constant);
				}
				if (methodReturnType.HasMarshalInfo)
				{
					Mixin.Read(methodReturnType.MarshalInfo);
				}
				this.ReadCustomAttributes(methodReturnType);
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001B50C File Offset: 0x0001970C
		private void ReadParameters(MethodDefinition method)
		{
			Collection<ParameterDefinition> parameters = method.Parameters;
			for (int i = 0; i < parameters.Count; i++)
			{
				ParameterDefinition parameterDefinition = parameters[i];
				if (parameterDefinition.HasConstant)
				{
					Mixin.Read(parameterDefinition.Constant);
				}
				if (parameterDefinition.HasMarshalInfo)
				{
					Mixin.Read(parameterDefinition.MarshalInfo);
				}
				this.ReadCustomAttributes(parameterDefinition);
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001B568 File Offset: 0x00019768
		private void ReadProperties(TypeDefinition type)
		{
			Collection<PropertyDefinition> properties = type.Properties;
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDefinition propertyDefinition = properties[i];
				Mixin.Read(propertyDefinition.GetMethod);
				if (propertyDefinition.HasConstant)
				{
					Mixin.Read(propertyDefinition.Constant);
				}
				this.ReadCustomAttributes(propertyDefinition);
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001B5BC File Offset: 0x000197BC
		private void ReadEvents(TypeDefinition type)
		{
			Collection<EventDefinition> events = type.Events;
			for (int i = 0; i < events.Count; i++)
			{
				EventDefinition eventDefinition = events[i];
				Mixin.Read(eventDefinition.AddMethod);
				this.ReadCustomAttributes(eventDefinition);
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001B5FB File Offset: 0x000197FB
		public override void ReadSymbols(ModuleDefinition module)
		{
			if (module.symbol_reader == null)
			{
				return;
			}
			this.ReadTypesSymbols(module.Types, module.symbol_reader);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001B618 File Offset: 0x00019818
		private void ReadTypesSymbols(Collection<TypeDefinition> types, ISymbolReader symbol_reader)
		{
			for (int i = 0; i < types.Count; i++)
			{
				TypeDefinition typeDefinition = types[i];
				if (typeDefinition.HasNestedTypes)
				{
					this.ReadTypesSymbols(typeDefinition.NestedTypes, symbol_reader);
				}
				if (typeDefinition.HasMethods)
				{
					this.ReadMethodsSymbols(typeDefinition, symbol_reader);
				}
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001B664 File Offset: 0x00019864
		private void ReadMethodsSymbols(TypeDefinition type, ISymbolReader symbol_reader)
		{
			Collection<MethodDefinition> methods = type.Methods;
			for (int i = 0; i < methods.Count; i++)
			{
				MethodDefinition methodDefinition = methods[i];
				if (methodDefinition.HasBody && methodDefinition.token.RID != 0U && methodDefinition.debug_info == null)
				{
					methodDefinition.debug_info = symbol_reader.Read(methodDefinition);
				}
			}
		}

		// Token: 0x04000283 RID: 643
		private bool resolve_attributes;
	}
}
