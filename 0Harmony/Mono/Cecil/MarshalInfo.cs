using System;

namespace Mono.Cecil
{
	// Token: 0x02000225 RID: 549
	public class MarshalInfo
	{
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0002923B File Offset: 0x0002743B
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x00029243 File Offset: 0x00027443
		public NativeType NativeType
		{
			get
			{
				return this.native;
			}
			set
			{
				this.native = value;
			}
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0002924C File Offset: 0x0002744C
		public MarshalInfo(NativeType native)
		{
			this.native = native;
		}

		// Token: 0x0400034F RID: 847
		internal NativeType native;
	}
}
