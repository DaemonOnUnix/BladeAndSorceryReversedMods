using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x02000296 RID: 662
	internal abstract class HeapBuffer : ByteBuffer
	{
		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x00036957 File Offset: 0x00034B57
		public bool IsLarge
		{
			get
			{
				return this.length > 65535;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060010F3 RID: 4339
		public abstract bool IsEmpty { get; }

		// Token: 0x060010F4 RID: 4340 RVA: 0x00036966 File Offset: 0x00034B66
		protected HeapBuffer(int length)
			: base(length)
		{
		}
	}
}
