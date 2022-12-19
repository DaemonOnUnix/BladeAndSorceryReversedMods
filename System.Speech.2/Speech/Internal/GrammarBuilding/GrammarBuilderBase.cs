using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x0200009B RID: 155
	internal abstract class GrammarBuilderBase
	{
		// Token: 0x0600052A RID: 1322
		internal abstract GrammarBuilderBase Clone();

		// Token: 0x0600052B RID: 1323
		internal abstract IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds);

		// Token: 0x0600052C RID: 1324 RVA: 0x000151D0 File Offset: 0x000133D0
		internal virtual int CalcCount(BuilderElements parent)
		{
			this.Marked = false;
			this.Parent = parent;
			return this.Count;
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x000151E6 File Offset: 0x000133E6
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x000151EE File Offset: 0x000133EE
		internal virtual int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				this._count = value;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x000151F7 File Offset: 0x000133F7
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x000151FF File Offset: 0x000133FF
		internal virtual bool Marked
		{
			get
			{
				return this._marker;
			}
			set
			{
				this._marker = value;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00015208 File Offset: 0x00013408
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x00015210 File Offset: 0x00013410
		internal virtual BuilderElements Parent
		{
			get
			{
				return this._parent;
			}
			set
			{
				this._parent = value;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000533 RID: 1331
		internal abstract string DebugSummary { get; }

		// Token: 0x0400044C RID: 1100
		private int _count = 1;

		// Token: 0x0400044D RID: 1101
		private bool _marker;

		// Token: 0x0400044E RID: 1102
		private BuilderElements _parent;
	}
}
