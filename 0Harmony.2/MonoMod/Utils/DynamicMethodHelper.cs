using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace MonoMod.Utils
{
	// Token: 0x02000439 RID: 1081
	internal static class DynamicMethodHelper
	{
		// Token: 0x060016BD RID: 5821 RVA: 0x0004BD8A File Offset: 0x00049F8A
		public static object GetReference(int id)
		{
			return DynamicMethodHelper.References[id];
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x0004BD97 File Offset: 0x00049F97
		public static void SetReference(int id, object obj)
		{
			DynamicMethodHelper.References[id] = obj;
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x0004BDA8 File Offset: 0x00049FA8
		private static int AddReference(object obj)
		{
			List<object> references = DynamicMethodHelper.References;
			int num;
			lock (references)
			{
				DynamicMethodHelper.References.Add(obj);
				num = DynamicMethodHelper.References.Count - 1;
			}
			return num;
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x0004BDFC File Offset: 0x00049FFC
		public static void FreeReference(int id)
		{
			DynamicMethodHelper.References[id] = null;
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x0004BE0C File Offset: 0x0004A00C
		public static DynamicMethod Stub(this DynamicMethod dm)
		{
			ILGenerator ilgenerator = dm.GetILGenerator();
			for (int i = 0; i < 32; i++)
			{
				ilgenerator.Emit(System.Reflection.Emit.OpCodes.Nop);
			}
			if (dm.ReturnType != typeof(void))
			{
				ilgenerator.DeclareLocal(dm.ReturnType);
				ilgenerator.Emit(System.Reflection.Emit.OpCodes.Ldloca_S, 0);
				ilgenerator.Emit(System.Reflection.Emit.OpCodes.Initobj, dm.ReturnType);
				ilgenerator.Emit(System.Reflection.Emit.OpCodes.Ldloc_0);
			}
			ilgenerator.Emit(System.Reflection.Emit.OpCodes.Ret);
			return dm;
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x0004BE90 File Offset: 0x0004A090
		public static DynamicMethodDefinition Stub(this DynamicMethodDefinition dmd)
		{
			ILProcessor ilprocessor = dmd.GetILProcessor();
			for (int i = 0; i < 32; i++)
			{
				ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Nop);
			}
			if (dmd.Definition.ReturnType != dmd.Definition.Module.TypeSystem.Void)
			{
				ilprocessor.Body.Variables.Add(new VariableDefinition(dmd.Definition.ReturnType));
				ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloca_S, 0);
				ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Initobj, dmd.Definition.ReturnType);
				ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ldloc_0);
			}
			ilprocessor.Emit(Mono.Cecil.Cil.OpCodes.Ret);
			return dmd;
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x0004BF38 File Offset: 0x0004A138
		public static int EmitReference<T>(this ILGenerator il, T obj)
		{
			Type typeFromHandle = typeof(T);
			int num = DynamicMethodHelper.AddReference(obj);
			il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4, num);
			il.Emit(System.Reflection.Emit.OpCodes.Call, DynamicMethodHelper._GetReference);
			if (typeFromHandle.IsValueType)
			{
				il.Emit(System.Reflection.Emit.OpCodes.Unbox_Any, typeFromHandle);
			}
			return num;
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x0004BF90 File Offset: 0x0004A190
		public static int EmitReference<T>(this ILProcessor il, T obj)
		{
			ModuleDefinition module = il.Body.Method.Module;
			Type typeFromHandle = typeof(T);
			int num = DynamicMethodHelper.AddReference(obj);
			il.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, num);
			il.Emit(Mono.Cecil.Cil.OpCodes.Call, module.ImportReference(DynamicMethodHelper._GetReference));
			if (typeFromHandle.IsValueType)
			{
				il.Emit(Mono.Cecil.Cil.OpCodes.Unbox_Any, module.ImportReference(typeFromHandle));
			}
			return num;
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x0004C004 File Offset: 0x0004A204
		public static int EmitGetReference<T>(this ILGenerator il, int id)
		{
			Type typeFromHandle = typeof(T);
			il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4, id);
			il.Emit(System.Reflection.Emit.OpCodes.Call, DynamicMethodHelper._GetReference);
			if (typeFromHandle.IsValueType)
			{
				il.Emit(System.Reflection.Emit.OpCodes.Unbox_Any, typeFromHandle);
			}
			return id;
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x0004C050 File Offset: 0x0004A250
		public static int EmitGetReference<T>(this ILProcessor il, int id)
		{
			ModuleDefinition module = il.Body.Method.Module;
			Type typeFromHandle = typeof(T);
			il.Emit(Mono.Cecil.Cil.OpCodes.Ldc_I4, id);
			il.Emit(Mono.Cecil.Cil.OpCodes.Call, module.ImportReference(DynamicMethodHelper._GetReference));
			if (typeFromHandle.IsValueType)
			{
				il.Emit(Mono.Cecil.Cil.OpCodes.Unbox_Any, module.ImportReference(typeFromHandle));
			}
			return id;
		}

		// Token: 0x04000FE6 RID: 4070
		private static List<object> References = new List<object>();

		// Token: 0x04000FE7 RID: 4071
		private static readonly MethodInfo _GetMethodFromHandle = typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[] { typeof(RuntimeMethodHandle) });

		// Token: 0x04000FE8 RID: 4072
		private static readonly MethodInfo _GetReference = typeof(DynamicMethodHelper).GetMethod("GetReference");
	}
}
