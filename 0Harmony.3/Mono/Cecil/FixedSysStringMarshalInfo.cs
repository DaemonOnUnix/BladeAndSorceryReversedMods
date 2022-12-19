using System;

namespace Mono.Cecil
{
	// Token: 0x02000137 RID: 311
	internal sealed class FixedSysStringMarshalInfo : MarshalInfo
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00023092 File Offset: 0x00021292
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x0002309A File Offset: 0x0002129A
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

		// Token: 0x060008C4 RID: 2244 RVA: 0x000230A3 File Offset: 0x000212A3
		public FixedSysStringMarshalInfo()
			: base(NativeType.FixedSysString)
		{
			this.size = -1;
		}

		// Token: 0x04000329 RID: 809
		internal int size;
	}
}
