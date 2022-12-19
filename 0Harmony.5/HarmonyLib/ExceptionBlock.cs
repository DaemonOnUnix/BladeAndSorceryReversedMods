using System;

namespace HarmonyLib
{
	// Token: 0x0200005F RID: 95
	public class ExceptionBlock
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x0000B44D File Offset: 0x0000964D
		public ExceptionBlock(ExceptionBlockType blockType, Type catchType = null)
		{
			this.blockType = blockType;
			this.catchType = catchType ?? typeof(object);
		}

		// Token: 0x0400011B RID: 283
		public ExceptionBlockType blockType;

		// Token: 0x0400011C RID: 284
		public Type catchType;
	}
}
