using System;
using System.Collections.Generic;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A2 RID: 418
	internal sealed class GuidHeapBuffer : HeapBuffer
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x0002EF47 File Offset: 0x0002D147
		public override bool IsEmpty
		{
			get
			{
				return this.length == 0;
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0002EF52 File Offset: 0x0002D152
		public GuidHeapBuffer()
			: base(16)
		{
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0002EF68 File Offset: 0x0002D168
		public uint GetGuidIndex(Guid guid)
		{
			uint num;
			if (this.guids.TryGetValue(guid, out num))
			{
				return num;
			}
			num = (uint)(this.guids.Count + 1);
			this.WriteGuid(guid);
			this.guids.Add(guid, num);
			return num;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0002EFAA File Offset: 0x0002D1AA
		private void WriteGuid(Guid guid)
		{
			base.WriteBytes(guid.ToByteArray());
		}

		// Token: 0x04000607 RID: 1543
		private readonly Dictionary<Guid, uint> guids = new Dictionary<Guid, uint>();
	}
}
