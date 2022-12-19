using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000E9 RID: 233
	internal struct CfgArc
	{
		// Token: 0x06000821 RID: 2081 RVA: 0x000249B6 File Offset: 0x00022BB6
		internal CfgArc(CfgArc arc)
		{
			this._flag1 = arc._flag1;
			this._flag2 = arc._flag2;
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x000249D0 File Offset: 0x00022BD0
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x000249DD File Offset: 0x00022BDD
		internal bool RuleRef
		{
			get
			{
				return (this._flag1 & 1U) > 0U;
			}
			set
			{
				if (value)
				{
					this._flag1 |= 1U;
					return;
				}
				this._flag1 &= 4294967294U;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00024A00 File Offset: 0x00022C00
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x00024A0D File Offset: 0x00022C0D
		internal bool LastArc
		{
			get
			{
				return (this._flag1 & 2U) > 0U;
			}
			set
			{
				if (value)
				{
					this._flag1 |= 2U;
					return;
				}
				this._flag1 &= 4294967293U;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x00024A30 File Offset: 0x00022C30
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x00024A3D File Offset: 0x00022C3D
		internal bool HasSemanticTag
		{
			get
			{
				return (this._flag1 & 4U) > 0U;
			}
			set
			{
				if (value)
				{
					this._flag1 |= 4U;
					return;
				}
				this._flag1 &= 4294967291U;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00024A60 File Offset: 0x00022C60
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x00024A6D File Offset: 0x00022C6D
		internal bool LowConfRequired
		{
			get
			{
				return (this._flag1 & 8U) > 0U;
			}
			set
			{
				if (value)
				{
					this._flag1 |= 8U;
					return;
				}
				this._flag1 &= 4294967287U;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00024A90 File Offset: 0x00022C90
		// (set) Token: 0x0600082B RID: 2091 RVA: 0x00024A9E File Offset: 0x00022C9E
		internal bool HighConfRequired
		{
			get
			{
				return (this._flag1 & 16U) > 0U;
			}
			set
			{
				if (value)
				{
					this._flag1 |= 16U;
					return;
				}
				this._flag1 &= 4294967279U;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x00024AC2 File Offset: 0x00022CC2
		// (set) Token: 0x0600082D RID: 2093 RVA: 0x00024AD2 File Offset: 0x00022CD2
		internal uint TransitionIndex
		{
			get
			{
				return (this._flag1 >> 5) & 4194303U;
			}
			set
			{
				if (value > 4194303U)
				{
					XmlParser.ThrowSrgsException(SRID.TooManyArcs, new object[0]);
				}
				this._flag1 &= 4160749599U;
				this._flag1 |= value << 5;
			}
		}

		// Token: 0x170001AA RID: 426
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x00024B0B File Offset: 0x00022D0B
		internal uint MatchMode
		{
			set
			{
				this._flag1 &= 3355443199U;
				this._flag1 |= value << 27;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x00024B30 File Offset: 0x00022D30
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x00024B40 File Offset: 0x00022D40
		internal uint NextStartArcIndex
		{
			get
			{
				return (this._flag2 >> 8) & 4194303U;
			}
			set
			{
				if (value > 4194303U)
				{
					XmlParser.ThrowSrgsException(SRID.TooManyArcs, new object[0]);
				}
				this._flag2 &= 3221225727U;
				this._flag2 |= value << 8;
			}
		}

		// Token: 0x040005D8 RID: 1496
		private uint _flag1;

		// Token: 0x040005D9 RID: 1497
		private uint _flag2;
	}
}
