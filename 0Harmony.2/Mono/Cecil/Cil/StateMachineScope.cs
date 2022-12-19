using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002E2 RID: 738
	internal sealed class StateMachineScope
	{
		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x0003C748 File Offset: 0x0003A948
		// (set) Token: 0x060012C4 RID: 4804 RVA: 0x0003C750 File Offset: 0x0003A950
		public InstructionOffset Start
		{
			get
			{
				return this.start;
			}
			set
			{
				this.start = value;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x0003C759 File Offset: 0x0003A959
		// (set) Token: 0x060012C6 RID: 4806 RVA: 0x0003C761 File Offset: 0x0003A961
		public InstructionOffset End
		{
			get
			{
				return this.end;
			}
			set
			{
				this.end = value;
			}
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0003C76A File Offset: 0x0003A96A
		internal StateMachineScope(int start, int end)
		{
			this.start = new InstructionOffset(start);
			this.end = new InstructionOffset(end);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0003C78C File Offset: 0x0003A98C
		public StateMachineScope(Instruction start, Instruction end)
		{
			this.start = new InstructionOffset(start);
			this.end = ((end != null) ? new InstructionOffset(end) : default(InstructionOffset));
		}

		// Token: 0x04000985 RID: 2437
		internal InstructionOffset start;

		// Token: 0x04000986 RID: 2438
		internal InstructionOffset end;
	}
}
