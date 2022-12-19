using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A1 RID: 417
	internal abstract class HeapBuffer : ByteBuffer
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x0002EF2F File Offset: 0x0002D12F
		public bool IsLarge
		{
			get
			{
				return this.length > 65535;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000D90 RID: 3472
		public abstract bool IsEmpty { get; }

		// Token: 0x06000D91 RID: 3473 RVA: 0x0002EF3E File Offset: 0x0002D13E
		protected HeapBuffer(int length)
			: base(length)
		{
		}
	}
}
