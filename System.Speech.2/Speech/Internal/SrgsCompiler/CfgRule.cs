using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000EB RID: 235
	internal struct CfgRule
	{
		// Token: 0x0600083D RID: 2109 RVA: 0x0002529C File Offset: 0x0002349C
		internal CfgRule(int id, int nameOffset, uint flag)
		{
			this._flag = flag;
			this._nameOffset = nameOffset;
			this._id = id;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x000252B4 File Offset: 0x000234B4
		internal CfgRule(int id, int nameOffset, SPCFGRULEATTRIBUTES attributes)
		{
			this._flag = 0U;
			this._nameOffset = nameOffset;
			this._id = id;
			this.TopLevel = (attributes & SPCFGRULEATTRIBUTES.SPRAF_TopLevel) > (SPCFGRULEATTRIBUTES)0;
			this.DefaultActive = (attributes & SPCFGRULEATTRIBUTES.SPRAF_Active) > (SPCFGRULEATTRIBUTES)0;
			this.PropRule = (attributes & SPCFGRULEATTRIBUTES.SPRAF_Interpreter) > (SPCFGRULEATTRIBUTES)0;
			this.Export = (attributes & SPCFGRULEATTRIBUTES.SPRAF_Export) > (SPCFGRULEATTRIBUTES)0;
			this.Dynamic = (attributes & SPCFGRULEATTRIBUTES.SPRAF_Dynamic) > (SPCFGRULEATTRIBUTES)0;
			this.Import = (attributes & SPCFGRULEATTRIBUTES.SPRAF_Import) > (SPCFGRULEATTRIBUTES)0;
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x00025320 File Offset: 0x00023520
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x0002532D File Offset: 0x0002352D
		internal bool TopLevel
		{
			get
			{
				return (this._flag & 1U) > 0U;
			}
			set
			{
				if (value)
				{
					this._flag |= 1U;
					return;
				}
				this._flag &= 4294967294U;
			}
		}

		// Token: 0x170001AE RID: 430
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x00025350 File Offset: 0x00023550
		internal bool DefaultActive
		{
			set
			{
				if (value)
				{
					this._flag |= 2U;
					return;
				}
				this._flag &= 4294967293U;
			}
		}

		// Token: 0x170001AF RID: 431
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x00025373 File Offset: 0x00023573
		internal bool PropRule
		{
			set
			{
				if (value)
				{
					this._flag |= 4U;
					return;
				}
				this._flag &= 4294967291U;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x00025396 File Offset: 0x00023596
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x000253A3 File Offset: 0x000235A3
		internal bool Import
		{
			get
			{
				return (this._flag & 8U) > 0U;
			}
			set
			{
				if (value)
				{
					this._flag |= 8U;
					return;
				}
				this._flag &= 4294967287U;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x000253C6 File Offset: 0x000235C6
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x000253D4 File Offset: 0x000235D4
		internal bool Export
		{
			get
			{
				return (this._flag & 16U) > 0U;
			}
			set
			{
				if (value)
				{
					this._flag |= 16U;
					return;
				}
				this._flag &= 4294967279U;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x000253F8 File Offset: 0x000235F8
		internal bool HasResources
		{
			get
			{
				return (this._flag & 32U) > 0U;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x00025406 File Offset: 0x00023606
		// (set) Token: 0x06000849 RID: 2121 RVA: 0x00025414 File Offset: 0x00023614
		internal bool Dynamic
		{
			get
			{
				return (this._flag & 64U) > 0U;
			}
			set
			{
				if (value)
				{
					this._flag |= 64U;
					return;
				}
				this._flag &= 4294967231U;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x00025438 File Offset: 0x00023638
		// (set) Token: 0x0600084B RID: 2123 RVA: 0x00025449 File Offset: 0x00023649
		internal bool HasDynamicRef
		{
			get
			{
				return (this._flag & 128U) > 0U;
			}
			set
			{
				if (value)
				{
					this._flag |= 128U;
					return;
				}
				this._flag &= 4294967167U;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x00025473 File Offset: 0x00023673
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x00025483 File Offset: 0x00023683
		internal uint FirstArcIndex
		{
			get
			{
				return (this._flag >> 8) & 4194303U;
			}
			set
			{
				if (value > 4194303U)
				{
					XmlParser.ThrowSrgsException(SRID.TooManyArcs, new object[0]);
				}
				this._flag &= 3221225727U;
				this._flag |= value << 8;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x000254BC File Offset: 0x000236BC
		internal bool DirtyRule
		{
			set
			{
				if (value)
				{
					this._flag |= 2147483648U;
					return;
				}
				this._flag &= 2147483647U;
			}
		}

		// Token: 0x040005E9 RID: 1513
		internal uint _flag;

		// Token: 0x040005EA RID: 1514
		internal int _nameOffset;

		// Token: 0x040005EB RID: 1515
		internal int _id;
	}
}
