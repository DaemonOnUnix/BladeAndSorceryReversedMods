using System;
using System.Runtime.InteropServices;
using System.Speech.Internal;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200016A RID: 362
	[StructLayout(0)]
	public class SayAs
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0002881F File Offset: 0x0002781F
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x00028827 File Offset: 0x00027827
		public string InterpretAs
		{
			get
			{
				return this._interpretAs;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				this._interpretAs = value;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x0002883B File Offset: 0x0002783B
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x00028843 File Offset: 0x00027843
		public string Format
		{
			get
			{
				return this._format;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				this._format = value;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x00028857 File Offset: 0x00027857
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x0002885F File Offset: 0x0002785F
		public string Detail
		{
			get
			{
				return this._detail;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				this._detail = value;
			}
		}

		// Token: 0x040006E5 RID: 1765
		[MarshalAs(21)]
		private string _interpretAs;

		// Token: 0x040006E6 RID: 1766
		[MarshalAs(21)]
		private string _format;

		// Token: 0x040006E7 RID: 1767
		[MarshalAs(21)]
		private string _detail;
	}
}
