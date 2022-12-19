using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000A0 RID: 160
	internal struct CfgRule
	{
		// Token: 0x06000368 RID: 872 RVA: 0x0000D407 File Offset: 0x0000C407
		internal CfgRule(int id, int nameOffset, uint flag)
		{
			this._flag = flag;
			this._nameOffset = nameOffset;
			this._id = id;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000D420 File Offset: 0x0000C420
		internal CfgRule(int id, int nameOffset, SPCFGRULEATTRIBUTES attributes)
		{
			this._flag = 0U;
			this._nameOffset = nameOffset;
			this._id = id;
			this.TopLevel = (attributes & SPCFGRULEATTRIBUTES.SPRAF_TopLevel) != (SPCFGRULEATTRIBUTES)0;
			this.DefaultActive = (attributes & SPCFGRULEATTRIBUTES.SPRAF_Active) != (SPCFGRULEATTRIBUTES)0;
			this.PropRule = (attributes & SPCFGRULEATTRIBUTES.SPRAF_Interpreter) != (SPCFGRULEATTRIBUTES)0;
			this.Export = (attributes & SPCFGRULEATTRIBUTES.SPRAF_Export) != (SPCFGRULEATTRIBUTES)0;
			this.Dynamic = (attributes & SPCFGRULEATTRIBUTES.SPRAF_Dynamic) != (SPCFGRULEATTRIBUTES)0;
			this.Import = (attributes & SPCFGRULEATTRIBUTES.SPRAF_Import) != (SPCFGRULEATTRIBUTES)0;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0000D49E File Offset: 0x0000C49E
		// (set) Token: 0x0600036B RID: 875 RVA: 0x0000D4AE File Offset: 0x0000C4AE
		internal bool TopLevel
		{
			get
			{
				return (this._flag & 1U) != 0U;
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

		// Token: 0x1700005B RID: 91
		// (set) Token: 0x0600036C RID: 876 RVA: 0x0000D4D1 File Offset: 0x0000C4D1
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

		// Token: 0x1700005C RID: 92
		// (set) Token: 0x0600036D RID: 877 RVA: 0x0000D4F4 File Offset: 0x0000C4F4
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

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0000D517 File Offset: 0x0000C517
		// (set) Token: 0x0600036F RID: 879 RVA: 0x0000D527 File Offset: 0x0000C527
		internal bool Import
		{
			get
			{
				return (this._flag & 8U) != 0U;
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

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000D54A File Offset: 0x0000C54A
		// (set) Token: 0x06000371 RID: 881 RVA: 0x0000D55B File Offset: 0x0000C55B
		internal bool Export
		{
			get
			{
				return (this._flag & 16U) != 0U;
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

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000D57F File Offset: 0x0000C57F
		internal bool HasResources
		{
			get
			{
				return (this._flag & 32U) != 0U;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0000D590 File Offset: 0x0000C590
		// (set) Token: 0x06000374 RID: 884 RVA: 0x0000D5A1 File Offset: 0x0000C5A1
		internal bool Dynamic
		{
			get
			{
				return (this._flag & 64U) != 0U;
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000375 RID: 885 RVA: 0x0000D5C5 File Offset: 0x0000C5C5
		// (set) Token: 0x06000376 RID: 886 RVA: 0x0000D5D9 File Offset: 0x0000C5D9
		internal bool HasDynamicRef
		{
			get
			{
				return (this._flag & 128U) != 0U;
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

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000377 RID: 887 RVA: 0x0000D603 File Offset: 0x0000C603
		// (set) Token: 0x06000378 RID: 888 RVA: 0x0000D613 File Offset: 0x0000C613
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

		// Token: 0x17000063 RID: 99
		// (set) Token: 0x06000379 RID: 889 RVA: 0x0000D64C File Offset: 0x0000C64C
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

		// Token: 0x04000338 RID: 824
		internal uint _flag;

		// Token: 0x04000339 RID: 825
		internal int _nameOffset;

		// Token: 0x0400033A RID: 826
		internal int _id;
	}
}
