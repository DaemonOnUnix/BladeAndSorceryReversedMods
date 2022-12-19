using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002C2 RID: 706
	public struct OpCode : IEquatable<OpCode>
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x0003A12E File Offset: 0x0003832E
		public string Name
		{
			get
			{
				return OpCodeNames.names[(int)this.Code];
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x0003A13C File Offset: 0x0003833C
		public int Size
		{
			get
			{
				if (this.op1 != 255)
				{
					return 2;
				}
				return 1;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x0003A14E File Offset: 0x0003834E
		public byte Op1
		{
			get
			{
				return this.op1;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x0003A156 File Offset: 0x00038356
		public byte Op2
		{
			get
			{
				return this.op2;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x0003A15E File Offset: 0x0003835E
		public short Value
		{
			get
			{
				if (this.op1 != 255)
				{
					return (short)(((int)this.op1 << 8) | (int)this.op2);
				}
				return (short)this.op2;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x0003A184 File Offset: 0x00038384
		public Code Code
		{
			get
			{
				return (Code)this.code;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x0003A18C File Offset: 0x0003838C
		public FlowControl FlowControl
		{
			get
			{
				return (FlowControl)this.flow_control;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x0003A194 File Offset: 0x00038394
		public OpCodeType OpCodeType
		{
			get
			{
				return (OpCodeType)this.opcode_type;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x0003A19C File Offset: 0x0003839C
		public OperandType OperandType
		{
			get
			{
				return (OperandType)this.operand_type;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001214 RID: 4628 RVA: 0x0003A1A4 File Offset: 0x000383A4
		public StackBehaviour StackBehaviourPop
		{
			get
			{
				return (StackBehaviour)this.stack_behavior_pop;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x0003A1AC File Offset: 0x000383AC
		public StackBehaviour StackBehaviourPush
		{
			get
			{
				return (StackBehaviour)this.stack_behavior_push;
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0003A1B4 File Offset: 0x000383B4
		internal OpCode(int x, int y)
		{
			this.op1 = (byte)(x & 255);
			this.op2 = (byte)((x >> 8) & 255);
			this.code = (byte)((x >> 16) & 255);
			this.flow_control = (byte)((x >> 24) & 255);
			this.opcode_type = (byte)(y & 255);
			this.operand_type = (byte)((y >> 8) & 255);
			this.stack_behavior_pop = (byte)((y >> 16) & 255);
			this.stack_behavior_push = (byte)((y >> 24) & 255);
			if (this.op1 == 255)
			{
				OpCodes.OneByteOpCode[(int)this.op2] = this;
				return;
			}
			OpCodes.TwoBytesOpCode[(int)this.op2] = this;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0003A27B File Offset: 0x0003847B
		public override int GetHashCode()
		{
			return (int)this.Value;
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0003A284 File Offset: 0x00038484
		public override bool Equals(object obj)
		{
			if (!(obj is OpCode))
			{
				return false;
			}
			OpCode opCode = (OpCode)obj;
			return this.op1 == opCode.op1 && this.op2 == opCode.op2;
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0003A2C0 File Offset: 0x000384C0
		public bool Equals(OpCode opcode)
		{
			return this.op1 == opcode.op1 && this.op2 == opcode.op2;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0003A2C0 File Offset: 0x000384C0
		public static bool operator ==(OpCode one, OpCode other)
		{
			return one.op1 == other.op1 && one.op2 == other.op2;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0003A2E0 File Offset: 0x000384E0
		public static bool operator !=(OpCode one, OpCode other)
		{
			return one.op1 != other.op1 || one.op2 != other.op2;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x0003A303 File Offset: 0x00038503
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04000842 RID: 2114
		private readonly byte op1;

		// Token: 0x04000843 RID: 2115
		private readonly byte op2;

		// Token: 0x04000844 RID: 2116
		private readonly byte code;

		// Token: 0x04000845 RID: 2117
		private readonly byte flow_control;

		// Token: 0x04000846 RID: 2118
		private readonly byte opcode_type;

		// Token: 0x04000847 RID: 2119
		private readonly byte operand_type;

		// Token: 0x04000848 RID: 2120
		private readonly byte stack_behavior_pop;

		// Token: 0x04000849 RID: 2121
		private readonly byte stack_behavior_push;
	}
}
