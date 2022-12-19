using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x0200005B RID: 91
	public class CodeInstruction
	{
		// Token: 0x0600019D RID: 413 RVA: 0x0000ADD2 File Offset: 0x00008FD2
		internal CodeInstruction()
		{
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000ADF0 File Offset: 0x00008FF0
		public CodeInstruction(OpCode opcode, object operand = null)
		{
			this.opcode = opcode;
			this.operand = operand;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000AE1C File Offset: 0x0000901C
		public CodeInstruction(CodeInstruction instruction)
		{
			this.opcode = instruction.opcode;
			this.operand = instruction.operand;
			this.labels = instruction.labels.ToList<Label>();
			this.blocks = instruction.blocks.ToList<ExceptionBlock>();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000AE7F File Offset: 0x0000907F
		public CodeInstruction Clone()
		{
			return new CodeInstruction(this)
			{
				labels = new List<Label>(),
				blocks = new List<ExceptionBlock>()
			};
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000AE9D File Offset: 0x0000909D
		public CodeInstruction Clone(OpCode opcode)
		{
			CodeInstruction codeInstruction = this.Clone();
			codeInstruction.opcode = opcode;
			return codeInstruction;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000AEAC File Offset: 0x000090AC
		public CodeInstruction Clone(object operand)
		{
			CodeInstruction codeInstruction = this.Clone();
			codeInstruction.operand = operand;
			return codeInstruction;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000AEBC File Offset: 0x000090BC
		public static CodeInstruction Call(Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			MethodInfo methodInfo = AccessTools.Method(type, name, parameters, generics);
			if (methodInfo == null)
			{
				throw new ArgumentException(string.Format("No method found for type={0}, name={1}, parameters={2}, generics={3}", new object[]
				{
					type,
					name,
					parameters.Description(),
					generics.Description()
				}));
			}
			return new CodeInstruction(OpCodes.Call, methodInfo);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000AF14 File Offset: 0x00009114
		public static CodeInstruction Call(string typeColonMethodname, Type[] parameters = null, Type[] generics = null)
		{
			MethodInfo methodInfo = AccessTools.Method(typeColonMethodname, parameters, generics);
			if (methodInfo == null)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"No method found for ",
					typeColonMethodname,
					", parameters=",
					parameters.Description(),
					", generics=",
					generics.Description()
				}));
			}
			return new CodeInstruction(OpCodes.Call, methodInfo);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000AF77 File Offset: 0x00009177
		public static CodeInstruction Call(Expression<Action> expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(expression));
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000AF89 File Offset: 0x00009189
		public static CodeInstruction Call<T>(Expression<Action<T>> expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo<T>(expression));
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000AF9B File Offset: 0x0000919B
		public static CodeInstruction Call<T, TResult>(Expression<Func<T, TResult>> expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo<T, TResult>(expression));
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000AFAD File Offset: 0x000091AD
		public static CodeInstruction Call(LambdaExpression expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(expression));
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000AFC0 File Offset: 0x000091C0
		public static CodeInstruction CallClosure<T>(T closure) where T : Delegate
		{
			if (closure.Method.IsStatic && closure.Target == null)
			{
				return new CodeInstruction(OpCodes.Call, closure.Method);
			}
			Type[] array = (from x in closure.Method.GetParameters()
				select x.ParameterType).ToArray<Type>();
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(closure.Method.Name, closure.Method.ReturnType, array);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			Type type = closure.Target.GetType();
			bool flag;
			if (closure.Target != null)
			{
				flag = type.GetFields().Any((FieldInfo x) => !x.IsStatic);
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				int count = CodeInstruction.State.closureCache.Count;
				CodeInstruction.State.closureCache[count] = closure;
				ilgenerator.Emit(OpCodes.Ldsfld, AccessTools.Field(typeof(Transpilers), "closureCache"));
				ilgenerator.Emit(OpCodes.Ldc_I4, count);
				ilgenerator.Emit(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Dictionary<int, Delegate>), "Item"));
			}
			else
			{
				if (closure.Target == null)
				{
					ilgenerator.Emit(OpCodes.Ldnull);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Newobj, AccessTools.FirstConstructor(type, (ConstructorInfo x) => !x.IsStatic && x.GetParameters().Length == 0));
				}
				ilgenerator.Emit(OpCodes.Ldftn, closure.Method);
				ilgenerator.Emit(OpCodes.Newobj, AccessTools.Constructor(typeof(T), new Type[]
				{
					typeof(object),
					typeof(IntPtr)
				}, false));
			}
			for (int i = 0; i < array.Length; i++)
			{
				ilgenerator.Emit(OpCodes.Ldarg, i);
			}
			ilgenerator.Emit(OpCodes.Callvirt, AccessTools.Method(typeof(T), "Invoke", null, null));
			ilgenerator.Emit(OpCodes.Ret);
			return new CodeInstruction(OpCodes.Call, dynamicMethodDefinition.Generate());
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000B21C File Offset: 0x0000941C
		public static CodeInstruction LoadField(Type type, string name, bool useAddress = false)
		{
			FieldInfo fieldInfo = AccessTools.Field(type, name);
			if (fieldInfo == null)
			{
				throw new ArgumentException(string.Format("No field found for {0} and {1}", type, name));
			}
			return new CodeInstruction(useAddress ? (fieldInfo.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda) : (fieldInfo.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld), fieldInfo);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000B27C File Offset: 0x0000947C
		public static CodeInstruction StoreField(Type type, string name)
		{
			FieldInfo fieldInfo = AccessTools.Field(type, name);
			if (fieldInfo == null)
			{
				throw new ArgumentException(string.Format("No field found for {0} and {1}", type, name));
			}
			return new CodeInstruction(fieldInfo.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fieldInfo);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000B2C0 File Offset: 0x000094C0
		public override string ToString()
		{
			List<string> list = new List<string>();
			foreach (Label label in this.labels)
			{
				list.Add(string.Format("Label{0}", label.GetHashCode()));
			}
			foreach (ExceptionBlock exceptionBlock in this.blocks)
			{
				list.Add("EX_" + exceptionBlock.blockType.ToString().Replace("Block", ""));
			}
			string text = ((list.Count > 0) ? (" [" + string.Join(", ", list.ToArray()) + "]") : "");
			string text2 = Emitter.FormatArgument(this.operand, null);
			if (text2.Length > 0)
			{
				text2 = " " + text2;
			}
			OpCode opCode = this.opcode;
			return opCode.ToString() + text2 + text;
		}

		// Token: 0x0400010B RID: 267
		public OpCode opcode;

		// Token: 0x0400010C RID: 268
		public object operand;

		// Token: 0x0400010D RID: 269
		public List<Label> labels = new List<Label>();

		// Token: 0x0400010E RID: 270
		public List<ExceptionBlock> blocks = new List<ExceptionBlock>();

		// Token: 0x0200005C RID: 92
		internal static class State
		{
			// Token: 0x0400010F RID: 271
			internal static readonly Dictionary<int, Delegate> closureCache = new Dictionary<int, Delegate>();
		}
	}
}
