using System;
using System.Runtime.InteropServices;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000A2 RID: 162
	[StructLayout(2)]
	internal struct CfgSemanticTag
	{
		// Token: 0x0600037A RID: 890 RVA: 0x0000D678 File Offset: 0x0000C678
		internal CfgSemanticTag(CfgSemanticTag cfgTag)
		{
			this._flag1 = cfgTag._flag1;
			this._flag2 = cfgTag._flag2;
			this._flag3 = cfgTag._flag3;
			this._propId = cfgTag._propId;
			this._nameOffset = cfgTag._nameOffset;
			this._varInt = 0;
			this._valueOffset = cfgTag._valueOffset;
			this._varDouble = cfgTag._varDouble;
			this.StartArcIndex = 4194303U;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000D6F4 File Offset: 0x0000C6F4
		internal CfgSemanticTag(StringBlob symbols, CfgGrammar.CfgProperty property)
		{
			this._flag1 = (this._flag2 = (this._flag3 = 0U));
			this._valueOffset = 0;
			this._varInt = 0;
			this._varDouble = 0.0;
			this._propId = property._ulId;
			if (property._pszName != null)
			{
				int num;
				this._nameOffset = symbols.Add(property._pszName, out num);
			}
			else
			{
				this._nameOffset = 0;
			}
			VarEnum comType = property._comType;
			if (comType <= 5)
			{
				if (comType != 0)
				{
					switch (comType)
					{
					case 3:
						this._varInt = (int)property._comValue;
						goto IL_104;
					case 4:
						goto IL_104;
					case 5:
						this._varDouble = (double)property._comValue;
						goto IL_104;
					default:
						goto IL_104;
					}
				}
			}
			else if (comType != 8)
			{
				if (comType != 11)
				{
					goto IL_104;
				}
				this._varInt = (((bool)property._comValue) ? 65535 : 0);
				goto IL_104;
			}
			if (property._comValue != null)
			{
				int num;
				this._valueOffset = symbols.Add((string)property._comValue, out num);
			}
			else
			{
				this._valueOffset = 0;
			}
			IL_104:
			this.PropVariantType = property._comType;
			this.ArcIndex = 0U;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000D818 File Offset: 0x0000C818
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0000D826 File Offset: 0x0000C826
		internal uint StartArcIndex
		{
			get
			{
				return this._flag1 & 4194303U;
			}
			set
			{
				if (value > 4194303U)
				{
					XmlParser.ThrowSrgsException(SRID.TooManyArcs, new object[0]);
				}
				this._flag1 &= 4290772992U;
				this._flag1 |= value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0000D85D File Offset: 0x0000C85D
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0000D871 File Offset: 0x0000C871
		internal bool StartParallelEpsilonArc
		{
			get
			{
				return (this._flag1 & 4194304U) != 0U;
			}
			set
			{
				if (value)
				{
					this._flag1 |= 4194304U;
					return;
				}
				this._flag1 &= 4290772991U;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000D89B File Offset: 0x0000C89B
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000D8A9 File Offset: 0x0000C8A9
		internal uint EndArcIndex
		{
			get
			{
				return this._flag2 & 4194303U;
			}
			set
			{
				if (value > 4194303U)
				{
					XmlParser.ThrowSrgsException(SRID.TooManyArcs, new object[0]);
				}
				this._flag2 &= 4290772992U;
				this._flag2 |= value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000D8E0 File Offset: 0x0000C8E0
		// (set) Token: 0x06000383 RID: 899 RVA: 0x0000D8F4 File Offset: 0x0000C8F4
		internal bool EndParallelEpsilonArc
		{
			get
			{
				return (this._flag2 & 4194304U) != 0U;
			}
			set
			{
				if (value)
				{
					this._flag2 |= 4194304U;
					return;
				}
				this._flag2 &= 4290772991U;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0000D91E File Offset: 0x0000C91E
		// (set) Token: 0x06000385 RID: 901 RVA: 0x0000D92C File Offset: 0x0000C92C
		internal VarEnum PropVariantType
		{
			get
			{
				return this._flag3 & 255U;
			}
			set
			{
				if (value > 255)
				{
					XmlParser.ThrowSrgsException(SRID.TooManyArcs, new object[0]);
				}
				this._flag3 &= 4294967040U;
				this._flag3 |= value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000D970 File Offset: 0x0000C970
		// (set) Token: 0x06000387 RID: 903 RVA: 0x0000D980 File Offset: 0x0000C980
		internal uint ArcIndex
		{
			get
			{
				return (this._flag3 >> 8) & 4194303U;
			}
			set
			{
				if (value > 4194303U)
				{
					XmlParser.ThrowSrgsException(SRID.TooManyArcs, new object[0]);
				}
				this._flag3 &= 3221225727U;
				this._flag3 |= value << 8;
			}
		}

		// Token: 0x04000345 RID: 837
		[FieldOffset(0)]
		private uint _flag1;

		// Token: 0x04000346 RID: 838
		[FieldOffset(4)]
		private uint _flag2;

		// Token: 0x04000347 RID: 839
		[FieldOffset(8)]
		private uint _flag3;

		// Token: 0x04000348 RID: 840
		[FieldOffset(12)]
		internal int _nameOffset;

		// Token: 0x04000349 RID: 841
		[FieldOffset(16)]
		internal uint _propId;

		// Token: 0x0400034A RID: 842
		[FieldOffset(20)]
		internal int _valueOffset;

		// Token: 0x0400034B RID: 843
		[FieldOffset(24)]
		internal int _varInt;

		// Token: 0x0400034C RID: 844
		[FieldOffset(24)]
		internal double _varDouble;
	}
}
