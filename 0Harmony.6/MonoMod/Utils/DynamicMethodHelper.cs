using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace MonoMod.Utils
{
	// Token: 0x02000341 RID: 833
	internal static class DynamicMethodHelper
	{
		// Token: 0x06001346 RID: 4934 RVA: 0x00043CEC File Offset: 0x00041EEC
		public static object GetReference(int id)
		{
			return DynamicMethodHelper.References[id];
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00043CF9 File Offset: 0x00041EF9
		public static void SetReference(int id, object obj)
		{
			DynamicMethodHelper.References[id] = obj;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00043D08 File Offset: 0x00041F08
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

		// Token: 0x06001349 RID: 4937 RVA: 0x00043D5C File Offset: 0x00041F5C
		public static void FreeReference(int id)
		{
			DynamicMethodHelper.References[id] = null;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00043D6C File Offset: 0x00041F6C
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

		// Token: 0x0600134B RID: 4939 RVA: 0x00043DF0 File Offset: 0x00041FF0
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

		// Token: 0x0600134C RID: 4940 RVA: 0x00043E98 File Offset: 0x00042098
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

		// Token: 0x0600134D RID: 4941 RVA: 0x00043EF0 File Offset: 0x000420F0
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

		// Token: 0x0600134E RID: 4942 RVA: 0x00043F64 File Offset: 0x00042164
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

		// Token: 0x0600134F RID: 4943 RVA: 0x00043FB0 File Offset: 0x000421B0
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

		// Token: 0x04000FA7 RID: 4007
		private static List<object> References = new List<object>();

		// Token: 0x04000FA8 RID: 4008
		private static readonly MethodInfo _GetMethodFromHandle = typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[] { typeof(RuntimeMethodHandle) });

		// Token: 0x04000FA9 RID: 4009
		private static readonly MethodInfo _GetReference = typeof(DynamicMethodHelper).GetMethod("GetReference");
	}
}
