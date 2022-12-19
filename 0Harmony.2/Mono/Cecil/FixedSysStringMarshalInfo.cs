using System;

namespace Mono.Cecil
{
	// Token: 0x0200022A RID: 554
	internal sealed class FixedSysStringMarshalInfo : MarshalInfo
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x0002936A File Offset: 0x0002756A
		// (set) Token: 0x06000C08 RID: 3080 RVA: 0x00029372 File Offset: 0x00027572
		public int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0002937B File Offset: 0x0002757B
		public FixedSysStringMarshalInfo()
			: base(NativeType.FixedSysString)
		{
			this.size = -1;
		}

		// Token: 0x0400035B RID: 859
		internal int size;
	}
}
