using System;
using System.Runtime.InteropServices;
using System.Speech.Internal;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000167 RID: 359
	[StructLayout(0)]
	public class Prosody
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x00028526 File Offset: 0x00027526
		// (set) Token: 0x06000945 RID: 2373 RVA: 0x0002852E File Offset: 0x0002752E
		public ProsodyNumber Pitch
		{
			get
			{
				return this._pitch;
			}
			set
			{
				this._pitch = value;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x00028537 File Offset: 0x00027537
		// (set) Token: 0x06000947 RID: 2375 RVA: 0x0002853F File Offset: 0x0002753F
		public ProsodyNumber Range
		{
			get
			{
				return this._range;
			}
			set
			{
				this._range = value;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x00028548 File Offset: 0x00027548
		// (set) Token: 0x06000949 RID: 2377 RVA: 0x00028550 File Offset: 0x00027550
		public ProsodyNumber Rate
		{
			get
			{
				return this._rate;
			}
			set
			{
				this._rate = value;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x00028559 File Offset: 0x00027559
		// (set) Token: 0x0600094B RID: 2379 RVA: 0x00028561 File Offset: 0x00027561
		public int Duration
		{
			get
			{
				return this._duration;
			}
			set
			{
				this._duration = value;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0002856A File Offset: 0x0002756A
		// (set) Token: 0x0600094D RID: 2381 RVA: 0x00028572 File Offset: 0x00027572
		public ProsodyNumber Volume
		{
			get
			{
				return this._volume;
			}
			set
			{
				this._volume = value;
			}
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002857B File Offset: 0x0002757B
		public ContourPoint[] GetContourPoints()
		{
			return this._contourPoints;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00028583 File Offset: 0x00027583
		public void SetContourPoints(ContourPoint[] points)
		{
			Helpers.ThrowIfNull(points, "points");
			this._contourPoints = (ContourPoint[])points.Clone();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x000285A1 File Offset: 0x000275A1
		public Prosody()
		{
			this.Pitch = new ProsodyNumber(0);
			this.Range = new ProsodyNumber(0);
			this.Rate = new ProsodyNumber(0);
			this.Volume = new ProsodyNumber(-1);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x000285DC File Offset: 0x000275DC
		internal Prosody Clone()
		{
			return new Prosody
			{
				_pitch = this._pitch,
				_range = this._range,
				_rate = this._rate,
				_duration = this._duration,
				_volume = this._volume
			};
		}

		// Token: 0x040006D7 RID: 1751
		internal ProsodyNumber _pitch;

		// Token: 0x040006D8 RID: 1752
		internal ProsodyNumber _range;

		// Token: 0x040006D9 RID: 1753
		internal ProsodyNumber _rate;

		// Token: 0x040006DA RID: 1754
		internal int _duration;

		// Token: 0x040006DB RID: 1755
		internal ProsodyNumber _volume;

		// Token: 0x040006DC RID: 1756
		internal ContourPoint[] _contourPoints;
	}
}
