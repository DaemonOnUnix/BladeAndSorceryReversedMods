using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200005A RID: 90
	public class CodeInstruction
	{
		// Token: 0x0600018D RID: 397 RVA: 0x00009F56 File Offset: 0x00008156
		public CodeInstruction(OpCode opcode, object operand = null)
		{
			this.opcode = opcode;
			this.operand = operand;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00009F84 File Offset: 0x00008184
		public CodeInstruction(CodeInstruction instruction)
		{
			this.opcode = instruction.opcode;
			this.operand = instruction.operand;
			this.labels = instruction.labels.ToList<Label>();
			this.blocks = instruction.blocks.ToList<ExceptionBlock>();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00009FE7 File Offset: 0x000081E7
		public CodeInstruction Clone()
		{
			return new CodeInstruction(this)
			{
				labels = new List<Label>(),
				blocks = new List<ExceptionBlock>()
			};
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000A005 File Offset: 0x00008205
		public CodeInstruction Clone(OpCode opcode)
		{
			CodeInstruction codeInstruction = this.Clone();
			codeInstruction.opcode = opcode;
			return codeInstruction;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000A014 File Offset: 0x00008214
		public CodeInstruction Clone(object operand)
		{
			CodeInstruction codeInstruction = this.Clone();
			codeInstruction.operand = operand;
			return codeInstruction;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000A024 File Offset: 0x00008224
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

		// Token: 0x06000193 RID: 403 RVA: 0x0000A07C File Offset: 0x0000827C
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

		// Token: 0x06000194 RID: 404 RVA: 0x0000A0DF File Offset: 0x000082DF
		public static CodeInstruction Call(Expression<Action> expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(expression));
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000A0F1 File Offset: 0x000082F1
		public static CodeInstruction Call<T>(Expression<Action<T>> expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo<T>(expression));
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000A103 File Offset: 0x00008303
		public static CodeInstruction Call<T, TResult>(Expression<Func<T, TResult>> expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo<T, TResult>(expression));
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000A115 File Offset: 0x00008315
		public static CodeInstruction Call(LambdaExpression expression)
		{
			return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(expression));
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000A128 File Offset: 0x00008328
		public static CodeInstruction LoadField(Type type, string name, bool useAddress = false)
		{
			FieldInfo fieldInfo = AccessTools.Field(type, name);
			if (fieldInfo == null)
			{
				throw new ArgumentException(string.Format("No field found for {0} and {1}", type, name));
			}
			return new CodeInstruction(useAddress ? (fieldInfo.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda) : (fieldInfo.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld), fieldInfo);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000A188 File Offset: 0x00008388
		public static CodeInstruction StoreField(Type type, string name)
		{
			FieldInfo fieldInfo = AccessTools.Field(type, name);
			if (fieldInfo == null)
			{
				throw new ArgumentException(string.Format("No field found for {0} and {1}", type, name));
			}
			return new CodeInstruction(fieldInfo.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fieldInfo);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000A1CC File Offset: 0x000083CC
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

		// Token: 0x040000FF RID: 255
		public OpCode opcode;

		// Token: 0x04000100 RID: 256
		public object operand;

		// Token: 0x04000101 RID: 257
		public List<Label> labels = new List<Label>();

		// Token: 0x04000102 RID: 258
		public List<ExceptionBlock> blocks = new List<ExceptionBlock>();
	}
}
