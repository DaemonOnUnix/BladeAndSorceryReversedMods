using System;
using System.Runtime.InteropServices;
using System.Speech.Internal;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000035 RID: 53
	[StructLayout(LayoutKind.Sequential)]
	public class SayAs
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00005071 File Offset: 0x00003271
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00005079 File Offset: 0x00003279
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

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000508D File Offset: 0x0000328D
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00005095 File Offset: 0x00003295
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000050A9 File Offset: 0x000032A9
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000050B1 File Offset: 0x000032B1
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

		// Token: 0x0400026B RID: 619
		[MarshalAs(UnmanagedType.LPWStr)]
		private string _interpretAs;

		// Token: 0x0400026C RID: 620
		[MarshalAs(UnmanagedType.LPWStr)]
		private string _format;

		// Token: 0x0400026D RID: 621
		[MarshalAs(UnmanagedType.LPWStr)]
		private string _detail;
	}
}
