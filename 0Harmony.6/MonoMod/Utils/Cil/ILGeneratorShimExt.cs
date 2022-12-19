using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MonoMod.Utils.Cil
{
	// Token: 0x02000363 RID: 867
	internal static class ILGeneratorShimExt
	{
		// Token: 0x0600147D RID: 5245 RVA: 0x0004B584 File Offset: 0x00049784
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

		// Token: 0x0600147E RID: 5246 RVA: 0x0004B687 File Offset: 0x00049887
		public static ILGeneratorShim GetProxiedShim(this ILGenerator il)
		{
			FieldInfo field = il.GetType().GetField("Target", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			return ((field != null) ? field.GetValue(il) : null) as ILGeneratorShim;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0004B6AD File Offset: 0x000498AD
		public static T GetProxiedShim<T>(this ILGenerator il) where T : ILGeneratorShim
		{
			return il.GetProxiedShim() as T;
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0004B6BF File Offset: 0x000498BF
		public static object DynEmit(this ILGenerator il, OpCode opcode, object operand)
		{
			return il.DynEmit(new object[] { opcode, operand });
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0004B6DC File Offset: 0x000498DC
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

		// Token: 0x04001028 RID: 4136
		private static readonly Dictionary<Type, MethodInfo> _Emitters = new Dictionary<Type, MethodInfo>();

		// Token: 0x04001029 RID: 4137
		private static readonly Dictionary<Type, MethodInfo> _EmittersShim = new Dictionary<Type, MethodInfo>();
	}
}
