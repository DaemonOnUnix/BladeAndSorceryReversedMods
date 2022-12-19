using System;
using System.ComponentModel;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000033 RID: 51
	[ImmutableObject(true)]
	public struct ContourPoint : IEquatable<ContourPoint>
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00004E7C File Offset: 0x0000307C
		public float Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00004E84 File Offset: 0x00003084
		public float Change
		{
			get
			{
				return this._change;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004E8C File Offset: 0x0000308C
		public ContourPointChangeType ChangeType
		{
			get
			{
				return this._changeType;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004E94 File Offset: 0x00003094
		public ContourPoint(float start, float change, ContourPointChangeType changeType)
		{
			this._start = start;
			this._change = change;
			this._changeType = changeType;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004EAC File Offset: 0x000030AC
		public static bool operator ==(ContourPoint point1, ContourPoint point2)
		{
			return point1.Start.Equals(point2.Start) && point1.Change.Equals(point2.Change) && point1.ChangeType.Equals(point2.ChangeType);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004F0C File Offset: 0x0000310C
		public static bool operator !=(ContourPoint point1, ContourPoint point2)
		{
			return !(point1 == point2);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004F18 File Offset: 0x00003118
		public bool Equals(ContourPoint other)
		{
			return this == other;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004F26 File Offset: 0x00003126
		public override bool Equals(object obj)
		{
			return obj is ContourPoint && this.Equals((ContourPoint)obj);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004F3E File Offset: 0x0000313E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000263 RID: 611
		private float _start;

		// Token: 0x04000264 RID: 612
		private float _change;

		// Token: 0x04000265 RID: 613
		private ContourPointChangeType _changeType;
	}
}
