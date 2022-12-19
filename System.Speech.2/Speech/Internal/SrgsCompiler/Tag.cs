using System;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000FE RID: 254
	internal sealed class Tag : IComparable<Tag>
	{
		// Token: 0x060008F3 RID: 2291 RVA: 0x00028B16 File Offset: 0x00026D16
		internal Tag(Tag tag)
		{
			this._be = tag._be;
			this._cfgTag = tag._cfgTag;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00028B36 File Offset: 0x00026D36
		internal Tag(Backend be, CfgSemanticTag cfgTag)
		{
			this._be = be;
			this._cfgTag = cfgTag;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00028B4C File Offset: 0x00026D4C
		internal Tag(Backend be, CfgGrammar.CfgProperty property)
		{
			this._be = be;
			this._cfgTag = new CfgSemanticTag(be.Symbols, property);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00028B6D File Offset: 0x00026D6D
		int IComparable<Tag>.CompareTo(Tag tag)
		{
			return (int)(this._cfgTag.ArcIndex - tag._cfgTag.ArcIndex);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00028B86 File Offset: 0x00026D86
		internal void Serialize(StreamMarshaler streamBuffer)
		{
			streamBuffer.WriteStream(this._cfgTag);
		}

		// Token: 0x04000639 RID: 1593
		internal CfgSemanticTag _cfgTag;

		// Token: 0x0400063A RID: 1594
		internal Backend _be;
	}
}
