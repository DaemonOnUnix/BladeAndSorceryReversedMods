using System;
using System.Collections.Generic;

namespace Mono.Cecil.Metadata
{
	// Token: 0x02000297 RID: 663
	internal sealed class GuidHeapBuffer : HeapBuffer
	{
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x0003696F File Offset: 0x00034B6F
		public override bool IsEmpty
		{
			get
			{
				return this.length == 0;
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0003697A File Offset: 0x00034B7A
		public GuidHeapBuffer()
			: base(16)
		{
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00036990 File Offset: 0x00034B90
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

		// Token: 0x060010F8 RID: 4344 RVA: 0x000369D2 File Offset: 0x00034BD2
		private void WriteGuid(Guid guid)
		{
			base.WriteBytes(guid.ToByteArray());
		}

		// Token: 0x0400063F RID: 1599
		private readonly Dictionary<Guid, uint> guids = new Dictionary<Guid, uint>();
	}
}
