using System;
using System.Runtime.InteropServices;
using System.Speech.Internal;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000032 RID: 50
	[StructLayout(LayoutKind.Sequential)]
	public class Prosody
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004D76 File Offset: 0x00002F76
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00004D7E File Offset: 0x00002F7E
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004D87 File Offset: 0x00002F87
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00004D8F File Offset: 0x00002F8F
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00004D98 File Offset: 0x00002F98
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00004DA0 File Offset: 0x00002FA0
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00004DA9 File Offset: 0x00002FA9
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00004DB1 File Offset: 0x00002FB1
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

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00004DBA File Offset: 0x00002FBA
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00004DC2 File Offset: 0x00002FC2
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

		// Token: 0x0600010D RID: 269 RVA: 0x00004DCB File Offset: 0x00002FCB
		public ContourPoint[] GetContourPoints()
		{
			return this._contourPoints;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004DD3 File Offset: 0x00002FD3
		public void SetContourPoints(ContourPoint[] points)
		{
			Helpers.ThrowIfNull(points, "points");
			this._contourPoints = (ContourPoint[])points.Clone();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004DF1 File Offset: 0x00002FF1
		public Prosody()
		{
			this.Pitch = new ProsodyNumber(0);
			this.Range = new ProsodyNumber(0);
			this.Rate = new ProsodyNumber(0);
			this.Volume = new ProsodyNumber(-1);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004E2C File Offset: 0x0000302C
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

		// Token: 0x0400025D RID: 605
		internal ProsodyNumber _pitch;

		// Token: 0x0400025E RID: 606
		internal ProsodyNumber _range;

		// Token: 0x0400025F RID: 607
		internal ProsodyNumber _rate;

		// Token: 0x04000260 RID: 608
		internal int _duration;

		// Token: 0x04000261 RID: 609
		internal ProsodyNumber _volume;

		// Token: 0x04000262 RID: 610
		internal ContourPoint[] _contourPoints;
	}
}
