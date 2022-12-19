using System;
using System.Runtime.InteropServices;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000ED RID: 237
	[StructLayout(LayoutKind.Explicit)]
	internal struct CfgSemanticTag
	{
		// Token: 0x0600084F RID: 2127 RVA: 0x000254E8 File Offset: 0x000236E8
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

		// Token: 0x06000850 RID: 2128 RVA: 0x0002555C File Offset: 0x0002375C
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
			if (comType <= VarEnum.VT_I4)
			{
				if (comType != VarEnum.VT_EMPTY)
				{
					if (comType != VarEnum.VT_I4)
					{
						goto IL_F7;
					}
					this._varInt = (int)property._comValue;
					goto IL_F7;
				}
			}
			else
			{
				if (comType == VarEnum.VT_R8)
				{
					this._varDouble = (double)property._comValue;
					goto IL_F7;
				}
				if (comType != VarEnum.VT_BSTR)
				{
					if (comType != VarEnum.VT_BOOL)
					{
						goto IL_F7;
					}
					this._varInt = (((bool)property._comValue) ? 65535 : 0);
					goto IL_F7;
				}
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
			IL_F7:
			this.PropVariantType = property._comType;
			this.ArcIndex = 0U;
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x00025673 File Offset: 0x00023873
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x00025681 File Offset: 0x00023881
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

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x000256B8 File Offset: 0x000238B8
		// (set) Token: 0x06000854 RID: 2132 RVA: 0x000256C9 File Offset: 0x000238C9
		internal bool StartParallelEpsilonArc
		{
			get
			{
				return (this._flag1 & 4194304U) > 0U;
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

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x000256F3 File Offset: 0x000238F3
		// (set) Token: 0x06000856 RID: 2134 RVA: 0x00025701 File Offset: 0x00023901
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

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x00025738 File Offset: 0x00023938
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x00025749 File Offset: 0x00023949
		internal bool EndParallelEpsilonArc
		{
			get
			{
				return (this._flag2 & 4194304U) > 0U;
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

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x00025773 File Offset: 0x00023973
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x00025784 File Offset: 0x00023984
		internal VarEnum PropVariantType
		{
			get
			{
				return (VarEnum)(this._flag3 & 255U);
			}
			set
			{
				if (value > (VarEnum)255)
				{
					XmlParser.ThrowSrgsException(SRID.TooManyArcs, new object[0]);
				}
				this._flag3 &= 4294967040U;
				this._flag3 |= (uint)value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x000257C8 File Offset: 0x000239C8
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x000257D8 File Offset: 0x000239D8
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

		// Token: 0x040005F6 RID: 1526
		[FieldOffset(0)]
		private uint _flag1;

		// Token: 0x040005F7 RID: 1527
		[FieldOffset(4)]
		private uint _flag2;

		// Token: 0x040005F8 RID: 1528
		[FieldOffset(8)]
		private uint _flag3;

		// Token: 0x040005F9 RID: 1529
		[FieldOffset(12)]
		internal int _nameOffset;

		// Token: 0x040005FA RID: 1530
		[FieldOffset(16)]
		internal uint _propId;

		// Token: 0x040005FB RID: 1531
		[FieldOffset(20)]
		internal int _valueOffset;

		// Token: 0x040005FC RID: 1532
		[FieldOffset(24)]
		internal int _varInt;

		// Token: 0x040005FD RID: 1533
		[FieldOffset(24)]
		internal double _varDouble;
	}
}
