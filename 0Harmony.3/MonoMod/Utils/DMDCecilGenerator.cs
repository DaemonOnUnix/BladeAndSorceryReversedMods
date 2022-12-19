﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace MonoMod.Utils
{
	// Token: 0x02000334 RID: 820
	internal sealed class DMDCecilGenerator : DMDGenerator<DMDCecilGenerator>
	{
		// Token: 0x06001304 RID: 4868 RVA: 0x00040710 File Offset: 0x0003E910
		protected override MethodInfo _Generate(DynamicMethodDefinition dmd, object context)
		{
			MethodDefinition definition = dmd.Definition;
			TypeDefinition typeDefinition = context as TypeDefinition;
			bool flag = false;
			ModuleDefinition module = ((typeDefinition != null) ? typeDefinition.Module : null);
			HashSet<string> hashSet = null;
			if (typeDefinition == null)
			{
				flag = true;
				hashSet = new HashSet<string>();
				string dumpName = dmd.GetDumpName("Cecil");
				module = ModuleDefinition.CreateModule(dumpName, new ModuleParameters
				{
					Kind = ModuleKind.Dll,
					ReflectionImporterProvider = MMReflectionImporter.ProviderNoDefault
				});
				module.Assembly.CustomAttributes.Add(new CustomAttribute(module.ImportReference(DynamicMethodDefinition.c_UnverifiableCodeAttribute)));
				if (dmd.Debug)
				{
					CustomAttribute customAttribute = new CustomAttribute(module.ImportReference(DynamicMethodDefinition.c_DebuggableAttribute));
					customAttribute.ConstructorArguments.Add(new CustomAttributeArgument(module.ImportReference(typeof(DebuggableAttribute.DebuggingModes)), DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations));
					module.Assembly.CustomAttributes.Add(customAttribute);
				}
				string text = "";
				string text2 = "DMD<{0}>?{1}";
				MethodBase originalMethod = dmd.OriginalMethod;
				object obj;
				if (originalMethod == null)
				{
					obj = null;
				}
				else
				{
					string name = originalMethod.Name;
					obj = ((name != null) ? name.Replace('.', '_') : null);
				}
				typeDefinition = new TypeDefinition(text, string.Format(text2, obj, this.GetHashCode()), Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Abstract | Mono.Cecil.TypeAttributes.Sealed)
				{
					BaseType = module.TypeSystem.Object
				};
				module.Types.Add(typeDefinition);
			}
			MethodInfo method;
			try
			{
				TypeReference typeReference = new TypeReference("System.Runtime.CompilerServices", "IsVolatile", module, module.TypeSystem.CoreLibrary);
				Relinker relinker = (IMetadataTokenProvider mtp, IGenericParameterProvider ctx) => module.ImportReference(mtp);
				MethodDefinition methodDefinition = new MethodDefinition(dmd.Name ?? ("_" + definition.Name.Replace('.', '_')), definition.Attributes, module.TypeSystem.Void)
				{
					MethodReturnType = definition.MethodReturnType,
					Attributes = (Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.Static | Mono.Cecil.MethodAttributes.HideBySig),
					ImplAttributes = Mono.Cecil.MethodImplAttributes.IL,
					DeclaringType = typeDefinition,
					NoInlining = true
				};
				foreach (ParameterDefinition parameterDefinition in definition.Parameters)
				{
					methodDefinition.Parameters.Add(parameterDefinition.Clone().Relink(relinker, methodDefinition));
				}
				methodDefinition.ReturnType = definition.ReturnType.Relink(relinker, methodDefinition);
				typeDefinition.Methods.Add(methodDefinition);
				methodDefinition.HasThis = definition.HasThis;
				Mono.Cecil.Cil.MethodBody methodBody = (methodDefinition.Body = definition.Body.Clone(methodDefinition));
				foreach (VariableDefinition variableDefinition in methodDefinition.Body.Variables)
				{
					variableDefinition.VariableType = variableDefinition.VariableType.Relink(relinker, methodDefinition);
				}
				foreach (ExceptionHandler exceptionHandler in methodDefinition.Body.ExceptionHandlers)
				{
					if (exceptionHandler.CatchType != null)
					{
						exceptionHandler.CatchType = exceptionHandler.CatchType.Relink(relinker, methodDefinition);
					}
				}
				for (int i = 0; i < methodBody.Instructions.Count; i++)
				{
					Instruction instruction = methodBody.Instructions[i];
					object obj2 = instruction.Operand;
					ParameterDefinition parameterDefinition2 = obj2 as ParameterDefinition;
					if (parameterDefinition2 != null)
					{
						obj2 = methodDefinition.Parameters[parameterDefinition2.Index];
					}
					else
					{
						IMetadataTokenProvider metadataTokenProvider = obj2 as IMetadataTokenProvider;
						if (metadataTokenProvider != null)
						{
							obj2 = metadataTokenProvider.Relink(relinker, methodDefinition);
						}
					}
					Instruction previous = instruction.Previous;
					OpCode? opCode = ((previous != null) ? new OpCode?(previous.OpCode) : null);
					OpCode @volatile = OpCodes.Volatile;
					if (opCode != null && (opCode == null || opCode.GetValueOrDefault() == @volatile))
					{
						FieldReference fieldReference = obj2 as FieldReference;
						if (fieldReference != null)
						{
							RequiredModifierType requiredModifierType = fieldReference.FieldType as RequiredModifierType;
							if (((requiredModifierType != null) ? requiredModifierType.ModifierType : null) != typeReference)
							{
								fieldReference.FieldType = new RequiredModifierType(typeReference, fieldReference.FieldType);
							}
						}
					}
					DynamicMethodReference dynamicMethodReference = obj2 as DynamicMethodReference;
					if (hashSet != null)
					{
						MemberReference memberReference = obj2 as MemberReference;
						if (memberReference != null)
						{
							TypeReference typeReference2 = memberReference as TypeReference;
							IMetadataScope metadataScope = ((typeReference2 != null) ? typeReference2.Scope : null) ?? memberReference.DeclaringType.Scope;
							if (!hashSet.Contains(metadataScope.Name))
							{
								CustomAttribute customAttribute2 = new CustomAttribute(module.ImportReference(DynamicMethodDefinition.c_IgnoresAccessChecksToAttribute));
								customAttribute2.ConstructorArguments.Add(new CustomAttributeArgument(module.ImportReference(typeof(DebuggableAttribute.DebuggingModes)), metadataScope.Name));
								module.Assembly.CustomAttributes.Add(customAttribute2);
								hashSet.Add(metadataScope.Name);
							}
						}
					}
					instruction.Operand = obj2;
				}
				methodDefinition.HasThis = false;
				if (definition.HasThis)
				{
					TypeReference typeReference3 = definition.DeclaringType;
					if (typeReference3.IsValueType)
					{
						typeReference3 = new ByReferenceType(typeReference3);
					}
					methodDefinition.Parameters.Insert(0, new ParameterDefinition("<>_this", Mono.Cecil.ParameterAttributes.None, typeReference3.Relink(relinker, methodDefinition)));
				}
				if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONOMOD_DMD_DUMP")))
				{
					string text3 = Path.GetFullPath(Environment.GetEnvironmentVariable("MONOMOD_DMD_DUMP"));
					string text4 = module.Name + ".dll";
					string text5 = Path.Combine(text3, text4);
					text3 = Path.GetDirectoryName(text5);
					if (!string.IsNullOrEmpty(text3) && !Directory.Exists(text3))
					{
						Directory.CreateDirectory(text3);
					}
					if (File.Exists(text5))
					{
						File.Delete(text5);
					}
					using (Stream stream = File.OpenWrite(text5))
					{
						module.Write(stream);
					}
				}
				method = ReflectionHelper.Load(module).GetType(typeDefinition.FullName.Replace("+", "\\+"), false, false).GetMethod(methodDefinition.Name, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			finally
			{
				if (flag)
				{
					module.Dispose();
				}
			}
			return method;
		}
	}
}
