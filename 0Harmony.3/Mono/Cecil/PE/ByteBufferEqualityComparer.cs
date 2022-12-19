using System;
using System.Collections.Generic;

namespace Mono.Cecil.PE
{
	// Token: 0x02000194 RID: 404
	internal sealed class ByteBufferEqualityComparer : IEqualityComparer<ByteBuffer>
	{
		// Token: 0x06000D07 RID: 3335 RVA: 0x0002C0CC File Offset: 0x0002A2CC
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

		// Token: 0x06000D08 RID: 3336 RVA: 0x0002C114 File Offset: 0x0002A314
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
