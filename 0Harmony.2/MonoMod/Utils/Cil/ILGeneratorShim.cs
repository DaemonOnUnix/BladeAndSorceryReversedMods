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
	// Token: 0x0200045F RID: 1119
	public abstract class ILGeneratorShim
	{
		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x060017F6 RID: 6134
		public abstract int ILOffset { get; }

		// Token: 0x060017F7 RID: 6135
		public abstract void BeginCatchBlock(Type exceptionType);

		// Token: 0x060017F8 RID: 6136
		public abstract void BeginExceptFilterBlock();

		// Token: 0x060017F9 RID: 6137
		public abstract Label BeginExceptionBlock();

		// Token: 0x060017FA RID: 6138
		public abstract void BeginFaultBlock();

		// Token: 0x060017FB RID: 6139
		public abstract void BeginFinallyBlock();

		// Token: 0x060017FC RID: 6140
		public abstract void BeginScope();

		// Token: 0x060017FD RID: 6141
		public abstract LocalBuilder DeclareLocal(Type localType);

		// Token: 0x060017FE RID: 6142
		public abstract LocalBuilder DeclareLocal(Type localType, bool pinned);

		// Token: 0x060017FF RID: 6143
		public abstract Label DefineLabel();

		// Token: 0x06001800 RID: 6144
		public abstract void Emit(System.Reflection.Emit.OpCode opcode);

		// Token: 0x06001801 RID: 6145
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, byte arg);

		// Token: 0x06001802 RID: 6146
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, double arg);

		// Token: 0x06001803 RID: 6147
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, short arg);

		// Token: 0x06001804 RID: 6148
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, int arg);

		// Token: 0x06001805 RID: 6149
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, long arg);

		// Token: 0x06001806 RID: 6150
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, ConstructorInfo con);

		// Token: 0x06001807 RID: 6151
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, Label label);

		// Token: 0x06001808 RID: 6152
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, Label[] labels);

		// Token: 0x06001809 RID: 6153
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, LocalBuilder local);

		// Token: 0x0600180A RID: 6154
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, SignatureHelper signature);

		// Token: 0x0600180B RID: 6155
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, FieldInfo field);

		// Token: 0x0600180C RID: 6156
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, MethodInfo meth);

		// Token: 0x0600180D RID: 6157
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, sbyte arg);

		// Token: 0x0600180E RID: 6158
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, float arg);

		// Token: 0x0600180F RID: 6159
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, string str);

		// Token: 0x06001810 RID: 6160
		public abstract void Emit(System.Reflection.Emit.OpCode opcode, Type cls);

		// Token: 0x06001811 RID: 6161
		public abstract void EmitCall(System.Reflection.Emit.OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes);

		// Token: 0x06001812 RID: 6162
		public abstract void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes);

		// Token: 0x06001813 RID: 6163
		public abstract void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes);

		// Token: 0x06001814 RID: 6164
		public abstract void EmitWriteLine(LocalBuilder localBuilder);

		// Token: 0x06001815 RID: 6165
		public abstract void EmitWriteLine(FieldInfo fld);

		// Token: 0x06001816 RID: 6166
		public abstract void EmitWriteLine(string value);

		// Token: 0x06001817 RID: 6167
		public abstract void EndExceptionBlock();

		// Token: 0x06001818 RID: 6168
		public abstract void EndScope();

		// Token: 0x06001819 RID: 6169
		public abstract void MarkLabel(Label loc);

		// Token: 0x0600181A RID: 6170
		public abstract void ThrowException(Type excType);

		// Token: 0x0600181B RID: 6171
		public abstract void UsingNamespace(string usingNamespace);

		// Token: 0x0600181C RID: 6172 RVA: 0x00053EA0 File Offset: 0x000520A0
		public ILGenerator GetProxy()
		{
			return (ILGenerator)ILGeneratorShim.ILGeneratorBuilder.GenerateProxy().MakeGenericType(new Type[] { base.GetType() }).GetConstructors()[0].Invoke(new object[] { this });
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x00053ED6 File Offset: 0x000520D6
		public static Type GetProxyType<TShim>() where TShim : ILGeneratorShim
		{
			return ILGeneratorShim.GetProxyType(typeof(TShim));
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x00053EE7 File Offset: 0x000520E7
		public static Type GetProxyType(Type tShim)
		{
			return ILGeneratorShim.ProxyType.MakeGenericType(new Type[] { tShim });
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x0600181F RID: 6175 RVA: 0x00053EFD File Offset: 0x000520FD
		public static Type ProxyType
		{
			get
			{
				return ILGeneratorShim.ILGeneratorBuilder.GenerateProxy();
			}
		}

		// Token: 0x02000460 RID: 1120
		internal static class ILGeneratorBuilder
		{
			// Token: 0x06001821 RID: 6177 RVA: 0x00053F04 File Offset: 0x00052104
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

			// Token: 0x04001082 RID: 4226
			public const string Namespace = "MonoMod.Utils.Cil";

			// Token: 0x04001083 RID: 4227
			public const string Name = "ILGeneratorProxy";

			// Token: 0x04001084 RID: 4228
			public const string FullName = "MonoMod.Utils.Cil.ILGeneratorProxy";

			// Token: 0x04001085 RID: 4229
			public const string TargetName = "Target";

			// Token: 0x04001086 RID: 4230
			private static Type ProxyType;
		}
	}
}
