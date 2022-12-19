using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001EC RID: 492
	internal sealed class StateMachineScope
	{
		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x000348BC File Offset: 0x00032ABC
		// (set) Token: 0x06000F57 RID: 3927 RVA: 0x000348C4 File Offset: 0x00032AC4
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

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000F58 RID: 3928 RVA: 0x000348CD File Offset: 0x00032ACD
		// (set) Token: 0x06000F59 RID: 3929 RVA: 0x000348D5 File Offset: 0x00032AD5
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

		// Token: 0x06000F5A RID: 3930 RVA: 0x000348DE File Offset: 0x00032ADE
		internal StateMachineScope(int start, int end)
		{
			this.start = new InstructionOffset(start);
			this.end = new InstructionOffset(end);
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00034900 File Offset: 0x00032B00
		public StateMachineScope(Instruction start, Instruction end)
		{
			this.start = new InstructionOffset(start);
			this.end = ((end != null) ? new InstructionOffset(end) : default(InstructionOffset));
		}

		// Token: 0x04000949 RID: 2377
		internal InstructionOffset start;

		// Token: 0x0400094A RID: 2378
		internal InstructionOffset end;
	}
}
