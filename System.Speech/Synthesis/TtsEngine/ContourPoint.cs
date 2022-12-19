using System;
using System.ComponentModel;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000168 RID: 360
	[ImmutableObject(true)]
	public struct ContourPoint : IEquatable<ContourPoint>
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0002862C File Offset: 0x0002762C
		public float Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00028634 File Offset: 0x00027634
		public float Change
		{
			get
			{
				return this._change;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x0002863C File Offset: 0x0002763C
		public ContourPointChangeType ChangeType
		{
			get
			{
				return this._changeType;
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00028644 File Offset: 0x00027644
		public ContourPoint(float start, float change, ContourPointChangeType changeType)
		{
			this._start = start;
			this._change = change;
			this._changeType = changeType;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002865C File Offset: 0x0002765C
		public static bool operator ==(ContourPoint point1, ContourPoint point2)
		{
			return point1.Start.Equals(point2.Start) && point1.Change.Equals(point2.Change) && point1.ChangeType.Equals(point2.ChangeType);
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x000286B8 File Offset: 0x000276B8
		public static bool operator !=(ContourPoint point1, ContourPoint point2)
		{
			return !(point1 == point2);
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x000286C4 File Offset: 0x000276C4
		public bool Equals(ContourPoint other)
		{
			return this == other;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x000286D2 File Offset: 0x000276D2
		public override bool Equals(object obj)
		{
			return obj is ContourPoint && this.Equals((ContourPoint)obj);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000286EA File Offset: 0x000276EA
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040006DD RID: 1757
		private float _start;

		// Token: 0x040006DE RID: 1758
		private float _change;

		// Token: 0x040006DF RID: 1759
		private ContourPointChangeType _changeType;
	}
}
