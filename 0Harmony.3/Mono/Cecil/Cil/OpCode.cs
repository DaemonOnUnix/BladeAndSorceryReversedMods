using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001CC RID: 460
	public struct OpCode : IEquatable<OpCode>
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x00032339 File Offset: 0x00030539
		public string Name
		{
			get
			{
				return OpCodeNames.names[(int)this.Code];
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x00032347 File Offset: 0x00030547
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

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x00032359 File Offset: 0x00030559
		public byte Op1
		{
			get
			{
				return this.op1;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x00032361 File Offset: 0x00030561
		public byte Op2
		{
			get
			{
				return this.op2;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x00032369 File Offset: 0x00030569
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

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x0003238F File Offset: 0x0003058F
		public Code Code
		{
			get
			{
				return (Code)this.code;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x00032397 File Offset: 0x00030597
		public FlowControl FlowControl
		{
			get
			{
				return (FlowControl)this.flow_control;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x0003239F File Offset: 0x0003059F
		public OpCodeType OpCodeType
		{
			get
			{
				return (OpCodeType)this.opcode_type;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x000323A7 File Offset: 0x000305A7
		public OperandType OperandType
		{
			get
			{
				return (OperandType)this.operand_type;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x000323AF File Offset: 0x000305AF
		public StackBehaviour StackBehaviourPop
		{
			get
			{
				return (StackBehaviour)this.stack_behavior_pop;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x000323B7 File Offset: 0x000305B7
		public StackBehaviour StackBehaviourPush
		{
			get
			{
				return (StackBehaviour)this.stack_behavior_push;
			}
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x000323C0 File Offset: 0x000305C0
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

		// Token: 0x06000EAF RID: 3759 RVA: 0x00032487 File Offset: 0x00030687
		public override int GetHashCode()
		{
			return (int)this.Value;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00032490 File Offset: 0x00030690
		public override bool Equals(object obj)
		{
			if (!(obj is OpCode))
			{
				return false;
			}
			OpCode opCode = (OpCode)obj;
			return this.op1 == opCode.op1 && this.op2 == opCode.op2;
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x000324CC File Offset: 0x000306CC
		public bool Equals(OpCode opcode)
		{
			return this.op1 == opcode.op1 && this.op2 == opcode.op2;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x000324CC File Offset: 0x000306CC
		public static bool operator ==(OpCode one, OpCode other)
		{
			return one.op1 == other.op1 && one.op2 == other.op2;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x000324EC File Offset: 0x000306EC
		public static bool operator !=(OpCode one, OpCode other)
		{
			return one.op1 != other.op1 || one.op2 != other.op2;
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0003250F File Offset: 0x0003070F
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04000806 RID: 2054
		private readonly byte op1;

		// Token: 0x04000807 RID: 2055
		private readonly byte op2;

		// Token: 0x04000808 RID: 2056
		private readonly byte code;

		// Token: 0x04000809 RID: 2057
		private readonly byte flow_control;

		// Token: 0x0400080A RID: 2058
		private readonly byte opcode_type;

		// Token: 0x0400080B RID: 2059
		private readonly byte operand_type;

		// Token: 0x0400080C RID: 2060
		private readonly byte stack_behavior_pop;

		// Token: 0x0400080D RID: 2061
		private readonly byte stack_behavior_push;
	}
}
