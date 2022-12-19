using System;
using System.Collections.Generic;

namespace Mono.Cecil.PE
{
	// Token: 0x02000289 RID: 649
	internal sealed class ByteBufferEqualityComparer : IEqualityComparer<ByteBuffer>
	{
		// Token: 0x0600106A RID: 4202 RVA: 0x00033AAC File Offset: 0x00031CAC
		public bool Equals(ByteBuffer x, ByteBuffer y)
		{
			if (x.length != y.length)
			{
				return false;
			}
			byte[] buffer = x.buffer;
			byte[] buffer2 = y.buffer;
			for (int i = 0; i < x.length; i++)
			{
				if (buffer[i] != buffer2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00033AF4 File Offset: 0x00031CF4
		public int GetHashCode(ByteBuffer buffer)
		{
			int num = -2128831035;
			byte[] buffer2 = buffer.buffer;
			for (int i = 0; i < buffer.length; i++)
			{
				num = (num ^ (int)buffer2[i]) * 16777619;
			}
			return num;
		}
	}
}
