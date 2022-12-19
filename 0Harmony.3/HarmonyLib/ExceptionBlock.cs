using System;

namespace HarmonyLib
{
	// Token: 0x0200005C RID: 92
	public class ExceptionBlock
	{
		// Token: 0x0600019B RID: 411 RVA: 0x0000A320 File Offset: 0x00008520
		public ExceptionBlock(ExceptionBlockType blockType, Type catchType = null)
		{
			this.blockType = blockType;
			this.catchType = catchType ?? typeof(object);
		}

		// Token: 0x0400010A RID: 266
		public ExceptionBlockType blockType;

		// Token: 0x0400010B RID: 267
		public Type catchType;
	}
}
