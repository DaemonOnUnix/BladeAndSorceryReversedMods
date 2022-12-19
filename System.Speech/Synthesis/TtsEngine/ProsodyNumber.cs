using System;
using System.ComponentModel;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000169 RID: 361
	[ImmutableObject(true)]
	public struct ProsodyNumber : IEquatable<ProsodyNumber>
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x000286FC File Offset: 0x000276FC
		// (set) Token: 0x0600095C RID: 2396 RVA: 0x00028704 File Offset: 0x00027704
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

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0002870D File Offset: 0x0002770D
		// (set) Token: 0x0600095E RID: 2398 RVA: 0x00028715 File Offset: 0x00027715
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

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x0002871E File Offset: 0x0002771E
		// (set) Token: 0x06000960 RID: 2400 RVA: 0x00028726 File Offset: 0x00027726
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

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x0002872F File Offset: 0x0002772F
		// (set) Token: 0x06000962 RID: 2402 RVA: 0x00028737 File Offset: 0x00027737
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

		// Token: 0x06000963 RID: 2403 RVA: 0x00028740 File Offset: 0x00027740
		public ProsodyNumber(int ssmlAttributeId)
		{
			this._ssmlAttributeId = ssmlAttributeId;
			this._number = 1f;
			this._isPercent = true;
			this._unit = ProsodyUnit.Default;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00028762 File Offset: 0x00027762
		public ProsodyNumber(float number)
		{
			this._ssmlAttributeId = int.MaxValue;
			this._number = number;
			this._isPercent = false;
			this._unit = ProsodyUnit.Default;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00028784 File Offset: 0x00027784
		public static bool operator ==(ProsodyNumber prosodyNumber1, ProsodyNumber prosodyNumber2)
		{
			return prosodyNumber1._ssmlAttributeId == prosodyNumber2._ssmlAttributeId && prosodyNumber1.Number.Equals(prosodyNumber2.Number) && prosodyNumber1.IsNumberPercent == prosodyNumber2.IsNumberPercent && prosodyNumber1.Unit == prosodyNumber2.Unit;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x000287DB File Offset: 0x000277DB
		public static bool operator !=(ProsodyNumber prosodyNumber1, ProsodyNumber prosodyNumber2)
		{
			return !(prosodyNumber1 == prosodyNumber2);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x000287E7 File Offset: 0x000277E7
		public bool Equals(ProsodyNumber other)
		{
			return this == other;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x000287F5 File Offset: 0x000277F5
		public override bool Equals(object obj)
		{
			return obj is ProsodyNumber && this.Equals((ProsodyNumber)obj);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0002880D File Offset: 0x0002780D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040006E0 RID: 1760
		public const int AbsoluteNumber = 2147483647;

		// Token: 0x040006E1 RID: 1761
		private int _ssmlAttributeId;

		// Token: 0x040006E2 RID: 1762
		private bool _isPercent;

		// Token: 0x040006E3 RID: 1763
		private float _number;

		// Token: 0x040006E4 RID: 1764
		private ProsodyUnit _unit;
	}
}
