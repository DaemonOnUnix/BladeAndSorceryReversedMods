using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x0200018F RID: 399
	internal abstract class GrammarBuilderBase
	{
		// Token: 0x06000A01 RID: 2561
		internal abstract GrammarBuilderBase Clone();

		// Token: 0x06000A02 RID: 2562
		internal abstract IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds);

		// Token: 0x06000A03 RID: 2563 RVA: 0x0002B5A8 File Offset: 0x0002A5A8
		internal virtual int CalcCount(BuilderElements parent)
		{
			this.Marked = false;
			this.Parent = parent;
			return this.Count;
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0002B5BE File Offset: 0x0002A5BE
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x0002B5C6 File Offset: 0x0002A5C6
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

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0002B5CF File Offset: 0x0002A5CF
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0002B5D7 File Offset: 0x0002A5D7
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

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0002B5E0 File Offset: 0x0002A5E0
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x0002B5E8 File Offset: 0x0002A5E8
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

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000A0A RID: 2570
		internal abstract string DebugSummary { get; }

		// Token: 0x04000904 RID: 2308
		private int _count = 1;

		// Token: 0x04000905 RID: 2309
		private bool _marker;

		// Token: 0x04000906 RID: 2310
		private BuilderElements _parent;
	}
}
