using System;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000C0 RID: 192
	internal sealed class Tag : IComparable<Tag>
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x00010E28 File Offset: 0x0000FE28
		internal Tag(Tag tag)
		{
			this._be = tag._be;
			this._cfgTag = tag._cfgTag;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00010E54 File Offset: 0x0000FE54
		internal Tag(Backend be, CfgSemanticTag cfgTag)
		{
			this._be = be;
			this._cfgTag = cfgTag;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00010E76 File Offset: 0x0000FE76
		internal Tag(Backend be, CfgGrammar.CfgProperty property)
		{
			this._be = be;
			this._cfgTag = new CfgSemanticTag(be.Symbols, property);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00010EA3 File Offset: 0x0000FEA3
		int IComparable<Tag>.CompareTo(Tag tag)
		{
			return (int)(this._cfgTag.ArcIndex - tag._cfgTag.ArcIndex);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00010EBC File Offset: 0x0000FEBC
		internal void Serialize(StreamMarshaler streamBuffer)
		{
			streamBuffer.WriteStream(this._cfgTag);
		}

		// Token: 0x04000394 RID: 916
		internal CfgSemanticTag _cfgTag = default(CfgSemanticTag);

		// Token: 0x04000395 RID: 917
		internal Backend _be;
	}
}
