using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace MonoMod.Utils.Cil
{
	// Token: 0x02000360 RID: 864
	public abstract class ILGeneratorShim
	{
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600144D RID: 5197
		public abstract int ILOffset { get; }

		// Token: 0x0600144E RID: 5198
		public abstract void BeginCatchBlock(Type exceptionType);

		// Token: 0x0600144F RID: 5199
		public abstract void BeginExceptFilterBlock();

		// Token: 0x06001450 RID: 5200
		public abstract Label BeginExceptionBlock();

		// Token: 0x06001451 RID: 5201
		public abstract void BeginFaultBlock();

		// Token: 0x06001452 RID: 5202
		public abstract void BeginFinallyBlock();

		// Token: 0x06001453 RID: 5203
		public abstract void BeginScope();

		// Token: 0x06001454 RID: 5204
		public abstract LocalBuilder DeclareLocal(Type localType);

		// Token: 0x06001455 RID: 5205
		public abstract LocalBuilder DeclareLocal(Type localType, bool pinned);

		// Token: 0x06001456 RID: 5206
		public abstract Label DefineLabel();

		// Token: 0x06001457 RID: 5207
		public abstract void Emit(System.Reflection.Emit.OpCode opcode);

		// Token: 0x06001458 RID: 5208
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, byte arg);

		// Token: 0x06001459 RID: 5209
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, double arg);

		// Token: 0x0600145A RID: 5210
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, short arg);

		// Token: 0x0600145B RID: 5211
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, int arg);

		// Token: 0x0600145C RID: 5212
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, long arg);

		// Token: 0x0600145D RID: 5213
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, ConstructorInfo con);

		// Token: 0x0600145E RID: 5214
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, Label label);

		// Token: 0x0600145F RID: 5215
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, Label[] labels);

		// Token: 0x06001460 RID: 5216
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, LocalBuilder local);

		// Token: 0x06001461 RID: 5217
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, SignatureHelper signature);

		// Token: 0x06001462 RID: 5218
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, FieldInfo field);

		// Token: 0x06001463 RID: 5219
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, MethodInfo meth);

		// Token: 0x06001464 RID: 5220
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, sbyte arg);

		// Token: 0x06001465 RID: 5221
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, float arg);

		// Token: 0x06001466 RID: 5222
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, string str);

		// Token: 0x06001467 RID: 5223
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, Type cls);

		// Token: 0x06001468 RID: 5224
		public abstract void EmitCall(System.Reflection.Emit.OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes);

		// Token: 0x06001469 RID: 5225
		public abstract void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes);

		// Token: 0x0600146A RID: 5226
		public abstract void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes);

		// Token: 0x0600146B RID: 5227
		public abstract void EmitWriteLine(LocalBuilder localBuilder);

		// Token: 0x0600146C RID: 5228
		public abstract void EmitWriteLine(FieldInfo fld);

		// Token: 0x0600146D RID: 5229
		public abstract void EmitWriteLine(string value);

		// Token: 0x0600146E RID: 5230
		public abstract void EndExceptionBlock();

		// Token: 0x0600146F RID: 5231
		public abstract void EndScope();

		// Token: 0x06001470 RID: 5232
		public abstract void MarkLabel(Label loc);

		// Token: 0x06001471 RID: 5233
		public abstract void ThrowException(Type excType);

		// Token: 0x06001472 RID: 5234
		public abstract void UsingNamespace(string usingNamespace);

		// Token: 0x06001473 RID: 5235 RVA: 0x0004AF20 File Offset: 0x00049120
		public ILGenerator GetProxy()
		{
			return (ILGenerator)ILGeneratorShim.ILGeneratorBuilder.GenerateProxy().MakeGenericType(new Type[] { base.GetType() }).GetConstructors()[0].Invoke(new object[] { this });
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0004AF56 File Offset: 0x00049156
		public static Type GetProxyType<TShim>() where TShim : ILGeneratorShim
		{
			return ILGeneratorShim.GetProxyType(typeof(TShim));
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0004AF67 File Offset: 0x00049167
		public static Type GetProxyType(Type tShim)
		{
			return ILGeneratorShim.ProxyType.MakeGenericType(new Type[] { tShim });
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x0004AF7D File Offset: 0x0004917D
		public static Type ProxyType
		{
			get
			{
				return ILGeneratorShim.ILGeneratorBuilder.GenerateProxy();
			}
		}

		// Token: 0x02000361 RID: 865
		internal static class ILGeneratorBuilder
		{
			// Token: 0x06001478 RID: 5240 RVA: 0x0004AF84 File Offset: 0x00049184
			public static Type GenerateProxy()
			{
				if (ILGeneratorShim.ILGeneratorBuilder.ProxyType != null)
				{
					return ILGeneratorShim.ILGeneratorBuilder.ProxyType;
				}
				Type typeFromHandle = typeof(ILGenerator);
				Type typeFromHandle2 = typeof(ILGeneratorShim);
				Assembly assembly;
				using (ModuleDefinition moduleDefinition = ModuleDefinition.CreateModule("MonoMod.Utils.Cil.ILGeneratorProxy", new ModuleParameters
				{
					Kind = ModuleKind.Dll
				}))
				{
					CustomAttribute customAttribute = new CustomAttribute(moduleDefinition.ImportReference(DynamicMethodDefinition.c_IgnoresAccessChecksToAttribute));
					customAttribute.ConstructorArguments.Add(new CustomAttributeArgument(moduleDefinition.TypeSystem.String, typeof(ILGeneratorShim).Assembly.GetName().Name));
					moduleDefinition.Assembly.CustomAttributes.Add(customAttribute);
					TypeDefinition typeDefinition = new TypeDefinition("MonoMod.Utils.Cil", "ILGeneratorProxy", Mono.Cecil.TypeAttributes.Public)
					{
						BaseType = moduleDefinition.ImportReference(typeFromHandle)
					};
					moduleDefinition.Types.Add(typeDefinition);
					TypeReference typeReference = moduleDefinition.ImportReference(typeFromHandle2);
					GenericParameter genericParameter = new GenericParameter("TTarget", typeDefinition);
					genericParameter.Constraints.Add(new GenericParameterConstraint(typeReference));
					typeDefinition.GenericParameters.Add(genericParameter);
					FieldDefinition fieldDefinition = new FieldDefinition("Target", Mono.Cecil.FieldAttributes.Public, genericParameter);
					typeDefinition.Fields.Add(fieldDefinition);
					FieldReference fieldReference = new FieldReference("Target", genericParameter, new GenericInstanceType(typeDefinition)
					{
						GenericArguments = { genericParameter }
					});
					MethodDefinition methodDefinition = new MethodDefinition(".ctor", Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.HideBySig | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.RTSpecialName, moduleDefinition.TypeSystem.Void);
					methodDefinition.Parameters.Add(new ParameterDefinition(genericParameter));
					typeDefinition.Methods.Add(methodDefinition);
					ILProcessor ilprocessor = methodDefinition.Body.GetILProcessor();
					ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
					ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_1);
					ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Stfld, fieldReference);
					ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ret);
					foreach (MethodInfo methodInfo in typeFromHandle.GetMethods(BindingFlags.Instance | BindingFlags.Public))
					{
						MethodInfo method = typeFromHandle2.GetMethod(methodInfo.Name, (from p in methodInfo.GetParameters()
							select p.ParameterType).ToArray<Type>());
						if (!(method == null))
						{
							MethodDefinition methodDefinition2 = new MethodDefinition(methodInfo.Name, Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.Virtual | Mono.Cecil.MethodAttributes.HideBySig, moduleDefinition.ImportReference(methodInfo.ReturnType))
							{
								HasThis = true
							};
							foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
							{
								methodDefinition2.Parameters.Add(new ParameterDefinition(moduleDefinition.ImportReference(parameterInfo.ParameterType)));
							}
							typeDefinition.Methods.Add(methodDefinition2);
							ilprocessor = methodDefinition2.Body.GetILProcessor();
							ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
							ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldfld, fieldReference);
							foreach (ParameterDefinition parameterDefinition in methodDefinition2.Parameters)
							{
								ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldarg, parameterDefinition);
							}
							ilprocessor.Emit(method.IsVirtual ? Mono.Cecil.Cil.OpCodes.Callvirt : Mono.Cecil.Cil.OpCodes.Call, ilprocessor.Body.Method.Module.ImportReference(method));
							ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ret);
						}
					}
					assembly = ReflectionHelper.Load(moduleDefinition);
					assembly.SetMonoCorlibInternal(true);
				}
				ResolveEventHandler resolveEventHandler = delegate(object asmSender, ResolveEventArgs asmArgs)
				{
					if (new AssemblyName(asmArgs.Name).Name == typeof(ILGeneratorShim.ILGeneratorBuilder).Assembly.GetName().Name)
					{
						return typeof(ILGeneratorShim.ILGeneratorBuilder).Assembly;
					}
					return null;
				};
				AppDomain.CurrentDomain.AssemblyResolve += resolveEventHandler;
				try
				{
					ILGeneratorShim.ILGeneratorBuilder.ProxyType = assembly.GetType("MonoMod.Utils.Cil.ILGeneratorProxy");
				}
				finally
				{
					AppDomain.CurrentDomain.AssemblyResolve -= resolveEventHandler;
				}
				if (ILGeneratorShim.ILGeneratorBuilder.ProxyType == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("Couldn't find ILGeneratorShim proxy \"").Append("MonoMod.Utils.Cil.ILGeneratorProxy").Append("\" in autogenerated \"")
						.Append(assembly.FullName)
						.AppendLine("\"");
					Type[] array;
					Exception[] array2;
					try
					{
						array = assembly.GetTypes();
						array2 = null;
					}
					catch (ReflectionTypeLoadException ex)
					{
						array = ex.Types;
						array2 = new Exception[ex.LoaderExceptions.Length + 1];
						array2[0] = ex;
						for (int k = 0; k < ex.LoaderExceptions.Length; k++)
						{
							array2[k + 1] = ex.LoaderExceptions[k];
						}
					}
					stringBuilder.AppendLine("Listing all types in autogenerated assembly:");
					foreach (Type type in array)
					{
						stringBuilder.AppendLine(((type != null) ? type.FullName : null) ?? "<NULL>");
					}
					if (((array2 != null) ? array2.Length : 0) > 0)
					{
						stringBuilder.AppendLine("Listing all exceptions:");
						for (int l = 0; l < array2.Length; l++)
						{
							StringBuilder stringBuilder2 = stringBuilder.Append("#").Append(l).Append(": ");
							Exception ex2 = array2[l];
							stringBuilder2.AppendLine(((ex2 != null) ? ex2.ToString() : null) ?? "NULL");
						}
					}
					throw new Exception(stringBuilder.ToString());
				}
				return ILGeneratorShim.ILGeneratorBuilder.ProxyType;
			}

			// Token: 0x04001020 RID: 4128
			public const string Namespace = "MonoMod.Utils.Cil";

			// Token: 0x04001021 RID: 4129
			public const string Name = "ILGeneratorProxy";

			// Token: 0x04001022 RID: 4130
			public const string FullName = "MonoMod.Utils.Cil.ILGeneratorProxy";

			// Token: 0x04001023 RID: 4131
			public const string TargetName = "Target";

			// Token: 0x04001024 RID: 4132
			private static Type ProxyType;
		}
	}
}
