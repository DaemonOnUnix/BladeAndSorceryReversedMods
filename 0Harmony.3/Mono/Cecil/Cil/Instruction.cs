using System;
using System.Text;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C4 RID: 452
	public sealed class Instruction
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x00031A38 File Offset: 0x0002FC38
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x00031A40 File Offset: 0x0002FC40
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

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x00031A49 File Offset: 0x0002FC49
		// (set) Token: 0x06000E69 RID: 3689 RVA: 0x00031A51 File Offset: 0x0002FC51
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

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x00031A5A File Offset: 0x0002FC5A
		// (set) Token: 0x06000E6B RID: 3691 RVA: 0x00031A62 File Offset: 0x0002FC62
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

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x00031A6B File Offset: 0x0002FC6B
		// (set) Token: 0x06000E6D RID: 3693 RVA: 0x00031A73 File Offset: 0x0002FC73
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

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x00031A7C File Offset: 0x0002FC7C
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x00031A84 File Offset: 0x0002FC84
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

		// Token: 0x06000E70 RID: 3696 RVA: 0x00031A8D File Offset: 0x0002FC8D
		internal Instruction(int offset, OpCode opCode)
		{
			this.offset = offset;
			this.opcode = opCode;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00031AA3 File Offset: 0x0002FCA3
		internal Instruction(OpCode opcode, object operand)
		{
			this.opcode = opcode;
			this.operand = operand;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00031ABC File Offset: 0x0002FCBC
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

		// Token: 0x06000E73 RID: 3699 RVA: 0x00031B60 File Offset: 0x0002FD60
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

		// Token: 0x06000E74 RID: 3700 RVA: 0x00031C54 File Offset: 0x0002FE54
		private static void AppendLabel(StringBuilder builder, Instruction instruction)
		{
			builder.Append("IL_");
			builder.Append(instruction.offset.ToString("x4"));
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00031C79 File Offset: 0x0002FE79
		public static Instruction Create(OpCode opcode)
		{
			if (opcode.OperandType != OperandType.InlineNone)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, null);
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00031C97 File Offset: 0x0002FE97
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

		// Token: 0x06000E77 RID: 3703 RVA: 0x00031CCF File Offset: 0x0002FECF
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

		// Token: 0x06000E78 RID: 3704 RVA: 0x00031CFC File Offset: 0x0002FEFC
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

		// Token: 0x06000E79 RID: 3705 RVA: 0x00031D33 File Offset: 0x0002FF33
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

		// Token: 0x06000E7A RID: 3706 RVA: 0x00031D6A File Offset: 0x0002FF6A
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

		// Token: 0x06000E7B RID: 3707 RVA: 0x00031D97 File Offset: 0x0002FF97
		public static Instruction Create(OpCode opcode, sbyte value)
		{
			if (opcode.OperandType != OperandType.ShortInlineI && opcode != OpCodes.Ldc_I4_S)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00031DC8 File Offset: 0x0002FFC8
		public static Instruction Create(OpCode opcode, byte value)
		{
			if (opcode.OperandType != OperandType.ShortInlineI || opcode == OpCodes.Ldc_I4_S)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00031DF9 File Offset: 0x0002FFF9
		public static Instruction Create(OpCode opcode, int value)
		{
			if (opcode.OperandType != OperandType.InlineI)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00031E1C File Offset: 0x0003001C
		public static Instruction Create(OpCode opcode, long value)
		{
			if (opcode.OperandType != OperandType.InlineI8)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00031E3F File Offset: 0x0003003F
		public static Instruction Create(OpCode opcode, float value)
		{
			if (opcode.OperandType != OperandType.ShortInlineR)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00031E63 File Offset: 0x00030063
		public static Instruction Create(OpCode opcode, double value)
		{
			if (opcode.OperandType != OperandType.InlineR)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00031E86 File Offset: 0x00030086
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

		// Token: 0x06000E82 RID: 3714 RVA: 0x00031EBC File Offset: 0x000300BC
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

		// Token: 0x06000E83 RID: 3715 RVA: 0x00031EE9 File Offset: 0x000300E9
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

		// Token: 0x06000E84 RID: 3716 RVA: 0x00031F21 File Offset: 0x00030121
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

		// Token: 0x040007B3 RID: 1971
		internal int offset;

		// Token: 0x040007B4 RID: 1972
		internal OpCode opcode;

		// Token: 0x040007B5 RID: 1973
		internal object operand;

		// Token: 0x040007B6 RID: 1974
		internal Instruction previous;

		// Token: 0x040007B7 RID: 1975
		internal Instruction next;
	}
}
