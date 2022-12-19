﻿using System;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x02000002 RID: 2
	public class DelegateTypeFactory
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public DelegateTypeFactory()
		{
			DelegateTypeFactory.counter++;
			AssemblyBuilder assemblyBuilder = PatchTools.DefineDynamicAssembly(string.Format("HarmonyDTFAssembly{0}", DelegateTypeFactory.counter));
			this.module = assemblyBuilder.DefineDynamicModule(string.Format("HarmonyDTFModule{0}", DelegateTypeFactory.counter));
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020AC File Offset: 0x000002AC
		public Type CreateDelegateType(MethodInfo method)
		{
			TypeAttributes typeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
			TypeBuilder typeBuilder = this.module.DefineType(string.Format("HarmonyDTFType{0}", DelegateTypeFactory.counter), typeAttributes, typeof(MulticastDelegate));
			typeBuilder.DefineConstructor(MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[]
			{
				typeof(object),
				typeof(IntPtr)
			}).SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
			ParameterInfo[] parameters = method.GetParameters();
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("Invoke", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig, method.ReturnType, parameters.Types());
			methodBuilder.SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
			for (int i = 0; i < parameters.Length; i++)
			{
				methodBuilder.DefineParameter(i + 1, ParameterAttributes.None, parameters[i].Name);
			}
			return typeBuilder.CreateType();
		}

		// Token: 0x04000001 RID: 1
		private readonly ModuleBuilder module;

		// Token: 0x04000002 RID: 2
		private static int counter;
	}
}
