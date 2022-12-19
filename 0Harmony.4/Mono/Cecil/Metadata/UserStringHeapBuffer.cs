using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200029B RID: 667
	internal sealed class UserStringHeapBuffer : StringHeapBuffer
	{
		// Token: 0x06001106 RID: 4358 RVA: 0x00036C5C File Offset: 0x00034E5C
		public override uint GetStringIndex(string @string)
		{
			uint position;
			if (this.strings.TryGetValue(@string, out position))
			{
				return position;
			}
			position = (uint)this.position;
			this.WriteString(@string);
			this.strings.Add(@string, position);
			return position;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00036C98 File Offset: 0x00034E98
		protected override void WriteString(string @string)
		{
			base.WriteCompressedUInt32((uint)(@string.Length * 2 + 1));
			byte b = 0;
			foreach (char c in @string)
			{
				base.WriteUInt16((ushort)c);
				if (b != 1 && (c < ' ' || c > '~') && (c > '~' || (c >= '\u0001' && c <= '\b') || (c >= '\u000e' && c <= '\u001f') || c == '\'' || c == '-'))
				{
					b = 1;
				}
			}
			base.WriteByte(b);
		}
	}
}
