using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x0200009B RID: 155
	internal struct CfgArc
	{
		// Token: 0x06000348 RID: 840 RVA: 0x0000C877 File Offset: 0x0000B877
		internal CfgArc(CfgArc arc)
		{
			this._flag1 = arc._flag1;
			this._flag2 = arc._flag2;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000C893 File Offset: 0x0000B893
		// (set) Token: 0x0600034A RID: 842 RVA: 0x0000C8A3 File Offset: 0x0000B8A3
		internal bool RuleRef
		{
			get
			{
				return (this._flag1 & 1U) != 0U;
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000C8C6 File Offset: 0x0000B8C6
		// (set) Token: 0x0600034C RID: 844 RVA: 0x0000C8D6 File Offset: 0x0000B8D6
		internal bool LastArc
		{
			get
			{
				return (this._flag1 & 2U) != 0U;
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

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000C8F9 File Offset: 0x0000B8F9
		// (set) Token: 0x0600034E RID: 846 RVA: 0x0000C909 File Offset: 0x0000B909
		internal bool HasSemanticTag
		{
			get
			{
				return (this._flag1 & 4U) != 0U;
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

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000C92C File Offset: 0x0000B92C
		// (set) Token: 0x06000350 RID: 848 RVA: 0x0000C93C File Offset: 0x0000B93C
		internal bool LowConfRequired
		{
			get
			{
				return (this._flag1 & 8U) != 0U;
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

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000C95F File Offset: 0x0000B95F
		// (set) Token: 0x06000352 RID: 850 RVA: 0x0000C970 File Offset: 0x0000B970
		internal bool HighConfRequired
		{
			get
			{
				return (this._flag1 & 16U) != 0U;
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000C994 File Offset: 0x0000B994
		// (set) Token: 0x06000354 RID: 852 RVA: 0x0000C9A4 File Offset: 0x0000B9A4
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

		// Token: 0x17000057 RID: 87
		// (set) Token: 0x06000355 RID: 853 RVA: 0x0000C9DD File Offset: 0x0000B9DD
		internal uint MatchMode
		{
			set
			{
				this._flag1 &= 3355443199U;
				this._flag1 |= value << 27;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000CA02 File Offset: 0x0000BA02
		// (set) Token: 0x06000357 RID: 855 RVA: 0x0000CA12 File Offset: 0x0000BA12
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

		// Token: 0x040002F3 RID: 755
		private uint _flag1;

		// Token: 0x040002F4 RID: 756
		private uint _flag2;
	}
}
