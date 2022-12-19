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
		// Token: 0x06000068 RID: 104 RVA: 0x00003BB0 File Offset: 0x00001DB0
		internal Emitter(ILGenerator il, bool debug)
		{
			this.il = il.GetProxiedShim<CecilILGenerator>();
			this.debug = debug;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003BD6 File Offset: 0x00001DD6
		internal Dictionary<int, CodeInstruction> GetInstructions()
		{
			return this.instructions;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003BDE File Offset: 0x00001DDE
		internal void AddInstruction(System.Reflection.Emit.OpCode opcode, object operand)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, operand));
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003BF8 File Offset: 0x00001DF8
		internal int CurrentPos()
		{
			return this.il.ILOffset;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003C05 File Offset: 0x00001E05
		internal static string CodePos(int offset)
		{
			return string.Format("IL_{0:X4}: ", offset);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003C17 File Offset: 0x00001E17
		internal string CodePos()
		{
			return Emitter.CodePos(this.CurrentPos());
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003C24 File Offset: 0x00001E24
		internal void LogComment(string comment)
		{
			if (this.debug)
			{
				FileLog.LogBuffered(string.Format("{0}// {1}", this.CodePos(), comment));
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003C44 File Offset: 0x00001E44
		internal void LogIL(System.Reflection.Emit.OpCode opcode)
		{
			if (this.debug)
			{
				FileLog.LogBuffered(string.Format("{0}{1}", this.CodePos(), opcode));
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003C6C File Offset: 0x00001E6C
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

		// Token: 0x06000071 RID: 113 RVA: 0x00003D00 File Offset: 0x00001F00
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

		// Token: 0x06000072 RID: 114 RVA: 0x00003D50 File Offset: 0x00001F50
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

		// Token: 0x06000073 RID: 115 RVA: 0x00003ED5 File Offset: 0x000020D5
		internal void MarkLabel(Label label)
		{
			if (this.debug)
			{
				FileLog.LogBuffered(this.CodePos() + Emitter.FormatArgument(label, null));
			}
			this.il.MarkLabel(label);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003F08 File Offset: 0x00002108
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

		// Token: 0x06000075 RID: 117 RVA: 0x000040C3 File Offset: 0x000022C3
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

		// Token: 0x06000076 RID: 118 RVA: 0x00004102 File Offset: 0x00002302
		internal void Emit(System.Reflection.Emit.OpCode opcode)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, null));
			this.LogIL(opcode);
			this.il.Emit(opcode);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000412F File Offset: 0x0000232F
		internal void Emit(System.Reflection.Emit.OpCode opcode, LocalBuilder local)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, local));
			this.LogIL(opcode, local, null);
			this.il.Emit(opcode, local);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000415F File Offset: 0x0000235F
		internal void Emit(System.Reflection.Emit.OpCode opcode, FieldInfo field)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, field));
			this.LogIL(opcode, field, null);
			this.il.Emit(opcode, field);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000418F File Offset: 0x0000238F
		internal void Emit(System.Reflection.Emit.OpCode opcode, Label[] labels)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, labels));
			this.LogIL(opcode, labels, null);
			this.il.Emit(opcode, labels);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000041BF File Offset: 0x000023BF
		internal void Emit(System.Reflection.Emit.OpCode opcode, Label label)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, label));
			this.LogIL(opcode, label, null);
			this.il.Emit(opcode, label);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000041F9 File Offset: 0x000023F9
		internal void Emit(System.Reflection.Emit.OpCode opcode, string str)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, str));
			this.LogIL(opcode, str, null);
			this.il.Emit(opcode, str);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004229 File Offset: 0x00002429
		internal void Emit(System.Reflection.Emit.OpCode opcode, float arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004263 File Offset: 0x00002463
		internal void Emit(System.Reflection.Emit.OpCode opcode, byte arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000429D File Offset: 0x0000249D
		internal void Emit(System.Reflection.Emit.OpCode opcode, sbyte arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000042D7 File Offset: 0x000024D7
		internal void Emit(System.Reflection.Emit.OpCode opcode, double arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004311 File Offset: 0x00002511
		internal void Emit(System.Reflection.Emit.OpCode opcode, int arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000434C File Offset: 0x0000254C
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

		// Token: 0x06000082 RID: 130 RVA: 0x000043BB File Offset: 0x000025BB
		internal void Emit(System.Reflection.Emit.OpCode opcode, short arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000043F5 File Offset: 0x000025F5
		internal void Emit(System.Reflection.Emit.OpCode opcode, SignatureHelper signature)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, signature));
			this.LogIL(opcode, signature, null);
			this.il.Emit(opcode, signature);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004425 File Offset: 0x00002625
		internal void Emit(System.Reflection.Emit.OpCode opcode, ConstructorInfo con)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, con));
			this.LogIL(opcode, con, null);
			this.il.Emit(opcode, con);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004455 File Offset: 0x00002655
		internal void Emit(System.Reflection.Emit.OpCode opcode, Type cls)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, cls));
			this.LogIL(opcode, cls, null);
			this.il.Emit(opcode, cls);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004485 File Offset: 0x00002685
		internal void Emit(System.Reflection.Emit.OpCode opcode, long arg)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, arg));
			this.LogIL(opcode, arg, null);
			this.il.Emit(opcode, arg);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000044C0 File Offset: 0x000026C0
		internal void EmitCall(System.Reflection.Emit.OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, methodInfo));
			string text = ((optionalParameterTypes != null && optionalParameterTypes.Length != 0) ? optionalParameterTypes.Description() : null);
			this.LogIL(opcode, methodInfo, text);
			this.il.EmitCall(opcode, methodInfo, optionalParameterTypes);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004510 File Offset: 0x00002710
		internal void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			this.instructions.Add(this.CurrentPos(), new CodeInstruction(opcode, unmanagedCallConv));
			string text = returnType.FullName + " " + parameterTypes.Description();
			this.LogIL(opcode, unmanagedCallConv, text);
			this.il.EmitCalli(opcode, unmanagedCallConv, returnType, parameterTypes);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004570 File Offset: 0x00002770
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
