using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002D4 RID: 724
	public struct InstructionOffset
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x0003C2C0 File Offset: 0x0003A4C0
		public int Offset
		{
			get
			{
				if (this.instruction != null)
				{
					return this.instruction.Offset;
				}
				if (this.offset != null)
				{
					return this.offset.Value;
				}
				throw new NotSupportedException();
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x0003C2F4 File Offset: 0x0003A4F4
		public bool IsEndOfMethod
		{
			get
			{
				return this.instruction == null && this.offset == null;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x0003C30E File Offset: 0x0003A50E
		internal bool IsResolved
		{
			get
			{
				return this.instruction != null || this.offset == null;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x0003C328 File Offset: 0x0003A528
		internal Instruction ResolvedInstruction
		{
			get
			{
				return this.instruction;
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0003C330 File Offset: 0x0003A530
		public InstructionOffset(Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.instruction = instruction;
			this.offset = null;
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0003C353 File Offset: 0x0003A553
		public InstructionOffset(int offset)
		{
			this.instruction = null;
			this.offset = new int?(offset);
		}

		// Token: 0x04000956 RID: 2390
		private readonly Instruction instruction;

		// Token: 0x04000957 RID: 2391
		private readonly int? offset;
	}
}
