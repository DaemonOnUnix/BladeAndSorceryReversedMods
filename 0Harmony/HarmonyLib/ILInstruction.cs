using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200001C RID: 28
	internal class ILInstruction
	{
		// Token: 0x0600009A RID: 154 RVA: 0x00004B33 File Offset: 0x00002D33
		internal ILInstruction(OpCode opcode, object operand = null)
		{
			this.opcode = opcode;
			this.operand = operand;
			this.argument = operand;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004B68 File Offset: 0x00002D68
		internal CodeInstruction GetCodeInstruction()
		{
			CodeInstruction codeInstruction = new CodeInstruction(this.opcode, this.argument);
			if (this.opcode.OperandType == OperandType.InlineNone)
			{
				codeInstruction.operand = null;
			}
			codeInstruction.labels = this.labels;
			codeInstruction.blocks = this.blocks;
			return codeInstruction;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004BB8 File Offset: 0x00002DB8
		internal int GetSize()
		{
			int num = this.opcode.Size;
			switch (this.opcode.OperandType)
			{
			case OperandType.InlineBrTarget:
			case OperandType.InlineField:
			case OperandType.InlineI:
			case OperandType.InlineMethod:
			case OperandType.InlineSig:
			case OperandType.InlineString:
			case OperandType.InlineTok:
			case OperandType.InlineType:
			case OperandType.ShortInlineR:
				num += 4;
				break;
			case OperandType.InlineI8:
			case OperandType.InlineR:
				num += 8;
				break;
			case OperandType.InlineSwitch:
				num += (1 + ((Array)this.operand).Length) * 4;
				break;
			case OperandType.InlineVar:
				num += 2;
				break;
			case OperandType.ShortInlineBrTarget:
			case OperandType.ShortInlineI:
			case OperandType.ShortInlineVar:
				num++;
				break;
			}
			return num;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004C64 File Offset: 0x00002E64
		public override string ToString()
		{
			string text = "";
			ILInstruction.AppendLabel(ref text, this);
			text = text + ": " + this.opcode.Name;
			if (this.operand == null)
			{
				return text;
			}
			text += " ";
			OperandType operandType = this.opcode.OperandType;
			if (operandType <= OperandType.InlineString)
			{
				if (operandType != OperandType.InlineBrTarget)
				{
					if (operandType != OperandType.InlineString)
					{
						goto IL_BE;
					}
					return text + string.Format("\"{0}\"", this.operand);
				}
			}
			else
			{
				if (operandType == OperandType.InlineSwitch)
				{
					ILInstruction[] array = (ILInstruction[])this.operand;
					for (int i = 0; i < array.Length; i++)
					{
						if (i > 0)
						{
							text += ",";
						}
						ILInstruction.AppendLabel(ref text, array[i]);
					}
					return text;
				}
				if (operandType != OperandType.ShortInlineBrTarget)
				{
					goto IL_BE;
				}
			}
			ILInstruction.AppendLabel(ref text, this.operand);
			return text;
			IL_BE:
			string text2 = text;
			object obj = this.operand;
			text = text2 + ((obj != null) ? obj.ToString() : null);
			return text;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004D4C File Offset: 0x00002F4C
		private static void AppendLabel(ref string str, object argument)
		{
			ILInstruction ilinstruction = argument as ILInstruction;
			str += string.Format("IL_{0}", ((ilinstruction != null) ? ilinstruction.offset.ToString("X4") : null) ?? argument);
		}

		// Token: 0x04000051 RID: 81
		internal int offset;

		// Token: 0x04000052 RID: 82
		internal OpCode opcode;

		// Token: 0x04000053 RID: 83
		internal object operand;

		// Token: 0x04000054 RID: 84
		internal object argument;

		// Token: 0x04000055 RID: 85
		internal List<Label> labels = new List<Label>();

		// Token: 0x04000056 RID: 86
		internal List<ExceptionBlock> blocks = new List<ExceptionBlock>();
	}
}
