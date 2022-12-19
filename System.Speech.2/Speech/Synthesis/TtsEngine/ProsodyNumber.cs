using System;
using System.ComponentModel;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000034 RID: 52
	[ImmutableObject(true)]
	public struct ProsodyNumber : IEquatable<ProsodyNumber>
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00004F50 File Offset: 0x00003150
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00004F58 File Offset: 0x00003158
		public int SsmlAttributeId
		{
			get
			{
				return this._ssmlAttributeId;
			}
			internal set
			{
				this._ssmlAttributeId = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00004F61 File Offset: 0x00003161
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00004F69 File Offset: 0x00003169
		public bool IsNumberPercent
		{
			get
			{
				return this._isPercent;
			}
			internal set
			{
				this._isPercent = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00004F72 File Offset: 0x00003172
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00004F7A File Offset: 0x0000317A
		public float Number
		{
			get
			{
				return this._number;
			}
			internal set
			{
				this._number = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00004F83 File Offset: 0x00003183
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00004F8B File Offset: 0x0000318B
		public ProsodyUnit Unit
		{
			get
			{
				return this._unit;
			}
			internal set
			{
				this._unit = value;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004F94 File Offset: 0x00003194
		public ProsodyNumber(int ssmlAttributeId)
		{
			this._ssmlAttributeId = ssmlAttributeId;
			this._number = 1f;
			this._isPercent = true;
			this._unit = ProsodyUnit.Default;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00004FB6 File Offset: 0x000031B6
		public ProsodyNumber(float number)
		{
			this._ssmlAttributeId = int.MaxValue;
			this._number = number;
			this._isPercent = false;
			this._unit = ProsodyUnit.Default;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004FD8 File Offset: 0x000031D8
		public static bool operator ==(ProsodyNumber prosodyNumber1, ProsodyNumber prosodyNumber2)
		{
			return prosodyNumber1._ssmlAttributeId == prosodyNumber2._ssmlAttributeId && prosodyNumber1.Number.Equals(prosodyNumber2.Number) && prosodyNumber1.IsNumberPercent == prosodyNumber2.IsNumberPercent && prosodyNumber1.Unit == prosodyNumber2.Unit;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000502D File Offset: 0x0000322D
		public static bool operator !=(ProsodyNumber prosodyNumber1, ProsodyNumber prosodyNumber2)
		{
			return !(prosodyNumber1 == prosodyNumber2);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005039 File Offset: 0x00003239
		public bool Equals(ProsodyNumber other)
		{
			return this == other;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005047 File Offset: 0x00003247
		public override bool Equals(object obj)
		{
			return obj is ProsodyNumber && this.Equals((ProsodyNumber)obj);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000505F File Offset: 0x0000325F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000266 RID: 614
		public const int AbsoluteNumber = 2147483647;

		// Token: 0x04000267 RID: 615
		private int _ssmlAttributeId;

		// Token: 0x04000268 RID: 616
		private bool _isPercent;

		// Token: 0x04000269 RID: 617
		private float _number;

		// Token: 0x0400026A RID: 618
		private ProsodyUnit _unit;
	}
}
