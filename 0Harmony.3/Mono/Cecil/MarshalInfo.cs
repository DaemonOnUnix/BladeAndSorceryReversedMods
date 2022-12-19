using System;

namespace Mono.Cecil
{
	// Token: 0x02000132 RID: 306
	public class MarshalInfo
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x00022F63 File Offset: 0x00021163
		// (set) Token: 0x060008A6 RID: 2214 RVA: 0x00022F6B File Offset: 0x0002116B
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

		// Token: 0x060008A7 RID: 2215 RVA: 0x00022F74 File Offset: 0x00021174
		public MarshalInfo(NativeType native)
		{
			this.native = native;
		}

		// Token: 0x0400031D RID: 797
		internal NativeType native;
	}
}
