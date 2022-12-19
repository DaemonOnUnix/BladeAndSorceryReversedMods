using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001DE RID: 478
	public struct InstructionOffset
	{
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0003446C File Offset: 0x0003266C
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

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x000344A0 File Offset: 0x000326A0
		public bool IsEndOfMethod
		{
			get
			{
				return this.instruction == null && this.offset == null;
			}
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x000344BA File Offset: 0x000326BA
		public InstructionOffset(Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.instruction = instruction;
			this.offset = null;
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x000344DD File Offset: 0x000326DD
		public InstructionOffset(int offset)
		{
			this.instruction = null;
			this.offset = new int?(offset);
		}

		// Token: 0x0400091A RID: 2330
		private readonly Instruction instruction;

		// Token: 0x0400091B RID: 2331
		private readonly int? offset;
	}
}
