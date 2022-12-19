using System;

namespace Mono.Cecil
{
	// Token: 0x02000160 RID: 352
	public sealed class PInvokeInfo
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00026CDC File Offset: 0x00024EDC
		// (set) Token: 0x06000B0D RID: 2829 RVA: 0x00026CE4 File Offset: 0x00024EE4
		public PInvokeAttributes Attributes
		{
			get
			{
				return (PInvokeAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00026CED File Offset: 0x00024EED
		// (set) Token: 0x06000B0F RID: 2831 RVA: 0x00026CF5 File Offset: 0x00024EF5
		public string EntryPoint
		{
			get
			{
				return this.entry_point;
			}
			set
			{
				this.entry_point = value;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00026CFE File Offset: 0x00024EFE
		// (set) Token: 0x06000B11 RID: 2833 RVA: 0x00026D06 File Offset: 0x00024F06
		public ModuleReference Module
		{
			get
			{
				return this.module;
			}
			set
			{
				this.module = value;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x00026D0F File Offset: 0x00024F0F
		// (set) Token: 0x06000B13 RID: 2835 RVA: 0x00026D1D File Offset: 0x00024F1D
		public bool IsNoMangle
		{
			get
			{
				return this.attributes.GetAttributes(1);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1, value);
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00026D32 File Offset: 0x00024F32
		// (set) Token: 0x06000B15 RID: 2837 RVA: 0x00026D41 File Offset: 0x00024F41
		public bool IsCharSetNotSpec
		{
			get
			{
				return this.attributes.GetMaskedAttributes(6, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(6, 0U, value);
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00026D57 File Offset: 0x00024F57
		// (set) Token: 0x06000B17 RID: 2839 RVA: 0x00026D66 File Offset: 0x00024F66
		public bool IsCharSetAnsi
		{
			get
			{
				return this.attributes.GetMaskedAttributes(6, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(6, 2U, value);
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00026D7C File Offset: 0x00024F7C
		// (set) Token: 0x06000B19 RID: 2841 RVA: 0x00026D8B File Offset: 0x00024F8B
		public bool IsCharSetUnicode
		{
			get
			{
				return this.attributes.GetMaskedAttributes(6, 4U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(6, 4U, value);
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00026DA1 File Offset: 0x00024FA1
		// (set) Token: 0x06000B1B RID: 2843 RVA: 0x00026DB0 File Offset: 0x00024FB0
		public bool IsCharSetAuto
		{
			get
			{
				return this.attributes.GetMaskedAttributes(6, 6U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(6, 6U, value);
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00026DC6 File Offset: 0x00024FC6
		// (set) Token: 0x06000B1D RID: 2845 RVA: 0x00026DD5 File Offset: 0x00024FD5
		public bool SupportsLastError
		{
			get
			{
				return this.attributes.GetAttributes(64);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(64, value);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x00026DEB File Offset: 0x00024FEB
		// (set) Token: 0x06000B1F RID: 2847 RVA: 0x00026E02 File Offset: 0x00025002
		public bool IsCallConvWinapi
		{
			get
			{
				return this.attributes.GetMaskedAttributes(1792, 256U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(1792, 256U, value);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00026E20 File Offset: 0x00025020
		// (set) Token: 0x06000B21 RID: 2849 RVA: 0x00026E37 File Offset: 0x00025037
		public bool IsCallConvCdecl
		{
			get
			{
				return this.attributes.GetMaskedAttributes(1792, 512U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(1792, 512U, value);
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00026E55 File Offset: 0x00025055
		// (set) Token: 0x06000B23 RID: 2851 RVA: 0x00026E6C File Offset: 0x0002506C
		public bool IsCallConvStdCall
		{
			get
			{
				return this.attributes.GetMaskedAttributes(1792, 768U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(1792, 768U, value);
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x00026E8A File Offset: 0x0002508A
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x00026EA1 File Offset: 0x000250A1
		public bool IsCallConvThiscall
		{
			get
			{
				return this.attributes.GetMaskedAttributes(1792, 1024U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(1792, 1024U, value);
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x00026EBF File Offset: 0x000250BF
		// (set) Token: 0x06000B27 RID: 2855 RVA: 0x00026ED6 File Offset: 0x000250D6
		public bool IsCallConvFastcall
		{
			get
			{
				return this.attributes.GetMaskedAttributes(1792, 1280U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(1792, 1280U, value);
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x00026EF4 File Offset: 0x000250F4
		// (set) Token: 0x06000B29 RID: 2857 RVA: 0x00026F05 File Offset: 0x00025105
		public bool IsBestFitEnabled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(48, 16U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(48, 16U, value);
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00026F1D File Offset: 0x0002511D
		// (set) Token: 0x06000B2B RID: 2859 RVA: 0x00026F2E File Offset: 0x0002512E
		public bool IsBestFitDisabled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(48, 32U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(48, 32U, value);
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00026F46 File Offset: 0x00025146
		// (set) Token: 0x06000B2D RID: 2861 RVA: 0x00026F5D File Offset: 0x0002515D
		public bool IsThrowOnUnmappableCharEnabled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(12288, 4096U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(12288, 4096U, value);
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x00026F7B File Offset: 0x0002517B
		// (set) Token: 0x06000B2F RID: 2863 RVA: 0x00026F92 File Offset: 0x00025192
		public bool IsThrowOnUnmappableCharDisabled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(12288, 8192U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(12288, 8192U, value);
			}
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00026FB0 File Offset: 0x000251B0
		public PInvokeInfo(PInvokeAttributes attributes, string entryPoint, ModuleReference module)
		{
			this.attributes = (ushort)attributes;
			this.entry_point = entryPoint;
			this.module = module;
		}

		// Token: 0x0400046C RID: 1132
		private ushort attributes;

		// Token: 0x0400046D RID: 1133
		private string entry_point;

		// Token: 0x0400046E RID: 1134
		private ModuleReference module;
	}
}
