using System;
using System.Text;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002B9 RID: 697
	public sealed class Instruction
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x00039471 File Offset: 0x00037671
		// (set) Token: 0x060011CB RID: 4555 RVA: 0x00039479 File Offset: 0x00037679
		public int Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x00039482 File Offset: 0x00037682
		// (set) Token: 0x060011CD RID: 4557 RVA: 0x0003948A File Offset: 0x0003768A
		public OpCode OpCode
		{
			get
			{
				return this.opcode;
			}
			set
			{
				this.opcode = value;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x00039493 File Offset: 0x00037693
		// (set) Token: 0x060011CF RID: 4559 RVA: 0x0003949B File Offset: 0x0003769B
		public object Operand
		{
			get
			{
				return this.operand;
			}
			set
			{
				this.operand = value;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x000394A4 File Offset: 0x000376A4
		// (set) Token: 0x060011D1 RID: 4561 RVA: 0x000394AC File Offset: 0x000376AC
		public Instruction Previous
		{
			get
			{
				return this.previous;
			}
			set
			{
				this.previous = value;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x000394B5 File Offset: 0x000376B5
		// (set) Token: 0x060011D3 RID: 4563 RVA: 0x000394BD File Offset: 0x000376BD
		public Instruction Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x000394C6 File Offset: 0x000376C6
		internal Instruction(int offset, OpCode opCode)
		{
			this.offset = offset;
			this.opcode = opCode;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x000394DC File Offset: 0x000376DC
		internal Instruction(OpCode opcode, object operand)
		{
			this.opcode = opcode;
			this.operand = operand;
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x000394F4 File Offset: 0x000376F4
		public int GetSize()
		{
			int size = this.opcode.Size;
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
				return size + 4;
			case OperandType.InlineI8:
			case OperandType.InlineR:
				return size + 8;
			case OperandType.InlineSwitch:
				return size + (1 + ((Instruction[])this.operand).Length) * 4;
			case OperandType.InlineVar:
			case OperandType.InlineArg:
				return size + 2;
			case OperandType.ShortInlineBrTarget:
			case OperandType.ShortInlineI:
			case OperandType.ShortInlineVar:
			case OperandType.ShortInlineArg:
				return size + 1;
			}
			return size;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00039598 File Offset: 0x00037798
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Instruction.AppendLabel(stringBuilder, this);
			stringBuilder.Append(':');
			stringBuilder.Append(' ');
			stringBuilder.Append(this.opcode.Name);
			if (this.operand == null)
			{
				return stringBuilder.ToString();
			}
			stringBuilder.Append(' ');
			OperandType operandType = this.opcode.OperandType;
			if (operandType <= OperandType.InlineString)
			{
				if (operandType != OperandType.InlineBrTarget)
				{
					if (operandType != OperandType.InlineString)
					{
						goto IL_D4;
					}
					stringBuilder.Append('"');
					stringBuilder.Append(this.operand);
					stringBuilder.Append('"');
					goto IL_E1;
				}
			}
			else
			{
				if (operandType == OperandType.InlineSwitch)
				{
					Instruction[] array = (Instruction[])this.operand;
					for (int i = 0; i < array.Length; i++)
					{
						if (i > 0)
						{
							stringBuilder.Append(',');
						}
						Instruction.AppendLabel(stringBuilder, array[i]);
					}
					goto IL_E1;
				}
				if (operandType != OperandType.ShortInlineBrTarget)
				{
					goto IL_D4;
				}
			}
			Instruction.AppendLabel(stringBuilder, (Instruction)this.operand);
			goto IL_E1;
			IL_D4:
			stringBuilder.Append(this.operand);
			IL_E1:
			return stringBuilder.ToString();
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0003968C File Offset: 0x0003788C
		private static void AppendLabel(StringBuilder builder, Instruction instruction)
		{
			builder.Append("IL_");
			builder.Append(instruction.offset.ToString("x4"));
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x000396B1 File Offset: 0x000378B1
		public static Instruction Create(OpCode opcode)
		{
			if (opcode.OperandType != OperandType.InlineNone)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, null);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x000396CF File Offset: 0x000378CF
		public static Instruction Create(OpCode opcode, TypeReference type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (opcode.OperandType != OperandType.InlineType && opcode.OperandType != OperandType.InlineTok)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, type);
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00039707 File Offset: 0x00037907
		public static Instruction Create(OpCode opcode, CallSite site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			if (opcode.Code != Code.Calli)
			{
				throw new ArgumentException("code");
			}
			return new Instruction(opcode, site);
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00039734 File Offset: 0x00037934
		public static Instruction Create(OpCode opcode, MethodReference method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (opcode.OperandType != OperandType.InlineMethod && opcode.OperandType != OperandType.InlineTok)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, method);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0003976B File Offset: 0x0003796B
		public static Instruction Create(OpCode opcode, FieldReference field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			if (opcode.OperandType != OperandType.InlineField && opcode.OperandType != OperandType.InlineTok)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, field);
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x000397A2 File Offset: 0x000379A2
		public static Instruction Create(OpCode opcode, string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (opcode.OperandType != OperandType.InlineString)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x000397CF File Offset: 0x000379CF
		public static Instruction Create(OpCode opcode, sbyte value)
		{
			if (opcode.OperandType != OperandType.ShortInlineI && opcode != OpCodes.Ldc_I4_S)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00039800 File Offset: 0x00037A00
		public static Instruction Create(OpCode opcode, byte value)
		{
			if (opcode.OperandType != OperandType.ShortInlineI || opcode == OpCodes.Ldc_I4_S)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00039831 File Offset: 0x00037A31
		public static Instruction Create(OpCode opcode, int value)
		{
			if (opcode.OperandType != OperandType.InlineI)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x00039854 File Offset: 0x00037A54
		public static Instruction Create(OpCode opcode, long value)
		{
			if (opcode.OperandType != OperandType.InlineI8)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00039877 File Offset: 0x00037A77
		public static Instruction Create(OpCode opcode, float value)
		{
			if (opcode.OperandType != OperandType.ShortInlineR)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0003989B File Offset: 0x00037A9B
		public static Instruction Create(OpCode opcode, double value)
		{
			if (opcode.OperandType != OperandType.InlineR)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x000398BE File Offset: 0x00037ABE
		public static Instruction Create(OpCode opcode, Instruction target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (opcode.OperandType != OperandType.InlineBrTarget && opcode.OperandType != OperandType.ShortInlineBrTarget)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, target);
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x000398F4 File Offset: 0x00037AF4
		public static Instruction Create(OpCode opcode, Instruction[] targets)
		{
			if (targets == null)
			{
				throw new ArgumentNullException("targets");
			}
			if (opcode.OperandType != OperandType.InlineSwitch)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, targets);
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00039921 File Offset: 0x00037B21
		public static Instruction Create(OpCode opcode, VariableDefinition variable)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			if (opcode.OperandType != OperandType.ShortInlineVar && opcode.OperandType != OperandType.InlineVar)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, variable);
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00039959 File Offset: 0x00037B59
		public static Instruction Create(OpCode opcode, ParameterDefinition parameter)
		{
			if (parameter == null)
			{
				throw new ArgumentNullException("parameter");
			}
			if (opcode.OperandType != OperandType.ShortInlineArg && opcode.OperandType != OperandType.InlineArg)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, parameter);
		}

		// Token: 0x040007EB RID: 2027
		internal int offset;

		// Token: 0x040007EC RID: 2028
		internal OpCode opcode;

		// Token: 0x040007ED RID: 2029
		internal object operand;

		// Token: 0x040007EE RID: 2030
		internal Instruction previous;

		// Token: 0x040007EF RID: 2031
		internal Instruction next;
	}
}
