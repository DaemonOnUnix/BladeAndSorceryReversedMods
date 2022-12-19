using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
using MonoMod.Utils.Cil;

namespace HarmonyLib
{
	// Token: 0x02000017 RID: 23
	internal class Emitter
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00003B74 File Offset: 0x00001D74
		internal Emitter(ILGenerator il, bool debug)
		{
			this.il = il.GetProxiedShim<CecilILGenerator>();
			this.debug = debug;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003B9A File Offset: 0x00001D9A
		internal Dictionary<int, CodeInstruction> GetInstructions()
		{
			return this.instructions;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003BA2 File Offset: 0x00001DA2
		internal void AddInstruction(System.Reflection.Emit.OpCode opcode, object operand)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, operand));
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003BBC File Offset: 0x00001DBC
		internal int CurrentPos()
		{
			return this.il.ILOffset;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003BC9 File Offset: 0x00001DC9
		internal static string CodePos(int offset)
		{
			return string.Format("IL_{0:X4}: ", offset);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003BDB File Offset: 0x00001DDB
		internal string CodePos()
		{
			return Emitter.CodePos(this.CurrentPos());
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003BE8 File Offset: 0x00001DE8
		internal void LogComment(string comment)
		{
			if (this.debug)
			{
				FileLog.LogBuffered(string.Format("{0}// {1}", this.CodePos(), comment));
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003C08 File Offset: 0x00001E08
		internal void LogIL(System.Reflection.Emit.OpCode opcode)
		{
			if (this.debug)
			{
				FileLog.LogBuffered(string.Format("{0}{1}", this.CodePos(), opcode));
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003C30 File Offset: 0x00001E30
		internal void LogIL(System.Reflection.Emit.OpCode opcode, object arg, string extra = null)
		{
			if (this.debug)
			{
				string text = Emitter.FormatArgument(arg, extra);
				string text2 = ((text.Length > 0) ? " " : "");
				string text3 = opcode.ToString();
				if (opcode.FlowControl == System.Reflection.Emit.FlowControl.Branch || opcode.FlowControl == System.Reflection.Emit.FlowControl.Cond_Branch)
				{
					text3 += " =>";
				}
				text3 = text3.PadRight(10);
				FileLog.LogBuffered(string.Format("{0}{1}{2}{3}", new object[]
				{
					this.CodePos(),
					text3,
					text2,
					text
				}));
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003CC4 File Offset: 0x00001EC4
		internal void LogAllLocalVariables()
		{
			if (!this.debug)
			{
				return;
			}
			this.il.IL.Body.Variables.Do(delegate(VariableDefinition v)
			{
				FileLog.LogBuffered(string.Format("{0}Local var {1}: {2}{3}", new object[]
				{
					Emitter.CodePos(0),
					v.Index,
					v.VariableType.FullName,
					v.IsPinned ? "(pinned)" : ""
				}));
			});
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003D14 File Offset: 0x00001F14
		internal static string FormatArgument(object argument, string extra = null)
		{
			if (argument == null)
			{
				return "NULL";
			}
			Type type = argument.GetType();
			MethodBase methodBase = argument as MethodBase;
			if (methodBase != null)
			{
				return methodBase.FullDescription() + ((extra != null) ? (" " + extra) : "");
			}
			FieldInfo fieldInfo = argument as FieldInfo;
			if (fieldInfo != null)
			{
				return string.Concat(new string[]
				{
					fieldInfo.FieldType.FullDescription(),
					" ",
					fieldInfo.DeclaringType.FullDescription(),
					"::",
					fieldInfo.Name
				});
			}
			if (type == typeof(Label))
			{
				return string.Format("Label{0}", ((Label)argument).GetHashCode());
			}
			if (type == typeof(Label[]))
			{
				return "Labels" + string.Join(",", ((Label[])argument).Select((Label l) => l.GetHashCode().ToString()).ToArray<string>());
			}
			if (type == typeof(LocalBuilder))
			{
				return string.Format("{0} ({1})", ((LocalBuilder)argument).LocalIndex, ((LocalBuilder)argument).LocalType);
			}
			if (type == typeof(string))
			{
				return argument.ToString().ToLiteral("\"");
			}
			return argument.ToString().Trim();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003E99 File Offset: 0x00002099
		internal void MarkLabel(Label label)
		{
			if (this.debug)
			{
				FileLog.LogBuffered(this.CodePos() + Emitter.FormatArgument(label, null));
			}
			this.il.MarkLabel(label);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003ECC File Offset: 0x000020CC
		internal void MarkBlockBefore(ExceptionBlock block, out Label? label)
		{
			label = null;
			switch (block.blockType)
			{
			case ExceptionBlockType.BeginExceptionBlock:
				if (this.debug)
				{
					FileLog.LogBuffered(".try");
					FileLog.LogBuffered("{");
					FileLog.ChangeIndent(1);
				}
				label = new Label?(this.il.BeginExceptionBlock());
				return;
			case ExceptionBlockType.BeginCatchBlock:
				if (this.debug)
				{
					this.LogIL(System.Reflection.Emit.OpCodes.Leave, new LeaveTry(), null);
					FileLog.ChangeIndent(-1);
					FileLog.LogBuffered("} // end try");
					FileLog.LogBuffered(string.Format(".catch {0}", block.catchType));
					FileLog.LogBuffered("{");
					FileLog.ChangeIndent(1);
				}
				this.il.BeginCatchBlock(block.catchType);
				return;
			case ExceptionBlockType.BeginExceptFilterBlock:
				if (this.debug)
				{
					this.LogIL(System.Reflection.Emit.OpCodes.Leave, new LeaveTry(), null);
					FileLog.ChangeIndent(-1);
					FileLog.LogBuffered("} // end try");
					FileLog.LogBuffered(".filter");
					FileLog.LogBuffered("{");
					FileLog.ChangeIndent(1);
				}
				this.il.BeginExceptFilterBlock();
				return;
			case ExceptionBlockType.BeginFaultBlock:
				if (this.debug)
				{
					this.LogIL(System.Reflection.Emit.OpCodes.Leave, new LeaveTry(), null);
					FileLog.ChangeIndent(-1);
					FileLog.LogBuffered("} // end try");
					FileLog.LogBuffered(".fault");
					FileLog.LogBuffered("{");
					FileLog.ChangeIndent(1);
				}
				this.il.BeginFaultBlock();
				return;
			case ExceptionBlockType.BeginFinallyBlock:
				if (this.debug)
				{
					this.LogIL(System.Reflection.Emit.OpCodes.Leave, new LeaveTry(), null);
					FileLog.ChangeIndent(-1);
					FileLog.LogBuffered("} // end try");
					FileLog.LogBuffered(".finally");
					FileLog.LogBuffered("{");
					FileLog.ChangeIndent(1);
				}
				this.il.BeginFinallyBlock();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004087 File Offset: 0x00002287
		internal void MarkBlockAfter(ExceptionBlock block)
		{
			if (block.blockType == ExceptionBlockType.EndExceptionBlock)
			{
				if (this.debug)
				{
					this.LogIL(System.Reflection.Emit.OpCodes.Leave, new LeaveTry(), null);
					FileLog.ChangeIndent(-1);
					FileLog.LogBuffered("} // end handler");
				}
				this.il.EndExceptionBlock();
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000040C6 File Offset: 0x000022C6
		internal void Emit(System.Reflection.Emit.OpCode opcode)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, null));
			this.LogIL(opcode);
			this.il.Emit(opcode);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000040F3 File Offset: 0x000022F3
		internal void Emit(System.Reflection.Emit.OpCode opcode, LocalBuilder local)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, local));
			this.LogIL(opcode, local, null);
			this.il.Emit(opcode, local);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004123 File Offset: 0x00002323
		internal void Emit(System.Reflection.Emit.OpCode opcode, FieldInfo field)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, field));
			this.LogIL(opcode, field, null);
			this.il.Emit(opcode, field);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004153 File Offset: 0x00002353
		internal void Emit(System.Reflection.Emit.OpCode opcode, Label[] labels)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, labels));
			this.LogIL(opcode, labels, null);
			this.il.Emit(opcode, labels);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004183 File Offset: 0x00002383
		internal void Emit(System.Reflection.Emit.OpCode opcode, Label label)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, label));
			this.LogIL(opcode, label, null);
			this.il.Emit(opcode, label);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000041BD File Offset: 0x000023BD
		internal void Emit(System.Reflection.Emit.OpCode opcode, string str)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, str));
			this.LogIL(opcode, str, null);
			this.il.Emit(opcode, str);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000041ED File Offset: 0x000023ED
		internal void Emit(System.Reflection.Emit.OpCode opcode, float arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004227 File Offset: 0x00002427
		internal void Emit(System.Reflection.Emit.OpCode opcode, byte arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004261 File Offset: 0x00002461
		internal void Emit(System.Reflection.Emit.OpCode opcode, sbyte arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000429B File Offset: 0x0000249B
		internal void Emit(System.Reflection.Emit.OpCode opcode, double arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000042D5 File Offset: 0x000024D5
		internal void Emit(System.Reflection.Emit.OpCode opcode, int arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004310 File Offset: 0x00002510
		internal void Emit(System.Reflection.Emit.OpCode opcode, MethodInfo meth)
		{
			if (opcode.Equals(System.Reflection.Emit.OpCodes.Call) || opcode.Equals(System.Reflection.Emit.OpCodes.Callvirt) || opcode.Equals(System.Reflection.Emit.OpCodes.Newobj))
			{
				this.EmitCall(opcode, meth, null);
				return;
			}
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, meth));
			this.LogIL(opcode, meth, null);
			this.il.Emit(opcode, meth);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000437F File Offset: 0x0000257F
		internal void Emit(System.Reflection.Emit.OpCode opcode, short arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000043B9 File Offset: 0x000025B9
		internal void Emit(System.Reflection.Emit.OpCode opcode, SignatureHelper signature)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, signature));
			this.LogIL(opcode, signature, null);
			this.il.Emit(opcode, signature);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000043E9 File Offset: 0x000025E9
		internal void Emit(System.Reflection.Emit.OpCode opcode, ConstructorInfo con)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, con));
			this.LogIL(opcode, con, null);
			this.il.Emit(opcode, con);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004419 File Offset: 0x00002619
		internal void Emit(System.Reflection.Emit.OpCode opcode, Type cls)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, cls));
			this.LogIL(opcode, cls, null);
			this.il.Emit(opcode, cls);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004449 File Offset: 0x00002649
		internal void Emit(System.Reflection.Emit.OpCode opcode, long arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004484 File Offset: 0x00002684
		internal void EmitCall(System.Reflection.Emit.OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, methodInfo));
			string text = ((optionalParameterTypes != null && optionalParameterTypes.Length != 0) ? optionalParameterTypes.Description() : null);
			this.LogIL(opcode, methodInfo, text);
			this.il.EmitCall(opcode, methodInfo, optionalParameterTypes);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000044D4 File Offset: 0x000026D4
		internal void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, unmanagedCallConv));
			string text = returnType.FullName + " " + parameterTypes.Description();
			this.LogIL(opcode, unmanagedCallConv, text);
			this.il.EmitCalli(opcode, unmanagedCallConv, returnType, parameterTypes);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004534 File Offset: 0x00002734
		internal void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, callingConvention));
			string text = string.Concat(new string[]
			{
				returnType.FullName,
				" ",
				parameterTypes.Description(),
				" ",
				optionalParameterTypes.Description()
			});
			this.LogIL(opcode, callingConvention, text);
			this.il.EmitCalli(opcode, callingConvention, returnType, parameterTypes, optionalParameterTypes);
		}

		// Token: 0x04000043 RID: 67
		private readonly CecilILGenerator il;

		// Token: 0x04000044 RID: 68
		private readonly Dictionary<int, CodeInstruction> instructions = new Dictionary<int, CodeInstruction>();

		// Token: 0x04000045 RID: 69
		private readonly bool debug;
	}
}
