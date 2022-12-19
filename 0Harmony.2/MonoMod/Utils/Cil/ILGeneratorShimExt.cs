using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MonoMod.Utils.Cil
{
	// Token: 0x02000462 RID: 1122
	internal static class ILGeneratorShimExt
	{
		// Token: 0x06001826 RID: 6182 RVA: 0x00054504 File Offset: 0x00052704
		static ILGeneratorShimExt()
		{
			foreach (MethodInfo methodInfo in typeof(ILGenerator).GetMethods())
			{
				if (!(methodInfo.Name != "Emit"))
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters.Length == 2 && !(parameters[0].ParameterType != typeof(OpCode)))
					{
						ILGeneratorShimExt._Emitters[parameters[1].ParameterType] = methodInfo;
					}
				}
			}
			foreach (MethodInfo methodInfo2 in typeof(ILGeneratorShim).GetMethods())
			{
				if (!(methodInfo2.Name != "Emit"))
				{
					ParameterInfo[] parameters2 = methodInfo2.GetParameters();
					if (parameters2.Length == 2 && !(parameters2[0].ParameterType != typeof(OpCode)))
					{
						ILGeneratorShimExt._EmittersShim[parameters2[1].ParameterType] = methodInfo2;
					}
				}
			}
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x00054607 File Offset: 0x00052807
		public static ILGeneratorShim GetProxiedShim(this ILGenerator il)
		{
			FieldInfo field = il.GetType().GetField("Target", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			return ((field != null) ? field.GetValue(il) : null) as ILGeneratorShim;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0005462D File Offset: 0x0005282D
		public static T GetProxiedShim<T>(this ILGenerator il) where T : ILGeneratorShim
		{
			return il.GetProxiedShim() as T;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0005463F File Offset: 0x0005283F
		public static object DynEmit(this ILGenerator il, OpCode opcode, object operand)
		{
			return il.DynEmit(new object[] { opcode, operand });
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0005465C File Offset: 0x0005285C
		public static object DynEmit(this ILGenerator il, object[] emitArgs)
		{
			Type operandType = emitArgs[1].GetType();
			object obj = il.GetProxiedShim() ?? il;
			Dictionary<Type, MethodInfo> dictionary = ((obj is ILGeneratorShim) ? ILGeneratorShimExt._EmittersShim : ILGeneratorShimExt._Emitters);
			MethodInfo value;
			if (!dictionary.TryGetValue(operandType, out value))
			{
				value = dictionary.FirstOrDefault((KeyValuePair<Type, MethodInfo> kvp) => kvp.Key.IsAssignableFrom(operandType)).Value;
			}
			if (value == null)
			{
				throw new InvalidOperationException("Unexpected unemittable operand type " + operandType.FullName);
			}
			return value.Invoke(obj, emitArgs);
		}

		// Token: 0x0400108A RID: 4234
		private static readonly Dictionary<Type, MethodInfo> _Emitters = new Dictionary<Type, MethodInfo>();

		// Token: 0x0400108B RID: 4235
		private static readonly Dictionary<Type, MethodInfo> _EmittersShim = new Dictionary<Type, MethodInfo>();
	}
}
