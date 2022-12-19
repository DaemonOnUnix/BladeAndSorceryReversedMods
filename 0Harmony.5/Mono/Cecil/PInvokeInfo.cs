using System;

namespace Mono.Cecil
{
	// Token: 0x02000254 RID: 596
	public sealed class PInvokeInfo
	{
		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0002D344 File Offset: 0x0002B544
		// (set) Token: 0x06000E57 RID: 3671 RVA: 0x0002D34C File Offset: 0x0002B54C
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

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000E58 RID: 3672 RVA: 0x0002D355 File Offset: 0x0002B555
		// (set) Token: 0x06000E59 RID: 3673 RVA: 0x0002D35D File Offset: 0x0002B55D
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

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0002D366 File Offset: 0x0002B566
		// (set) Token: 0x06000E5B RID: 3675 RVA: 0x0002D36E File Offset: 0x0002B56E
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

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x0002D377 File Offset: 0x0002B577
		// (set) Token: 0x06000E5D RID: 3677 RVA: 0x0002D385 File Offset: 0x0002B585
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

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0002D39A File Offset: 0x0002B59A
		// (set) Token: 0x06000E5F RID: 3679 RVA: 0x0002D3A9 File Offset: 0x0002B5A9
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

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x0002D3BF File Offset: 0x0002B5BF
		// (set) Token: 0x06000E61 RID: 3681 RVA: 0x0002D3CE File Offset: 0x0002B5CE
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

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x0002D3E4 File Offset: 0x0002B5E4
		// (set) Token: 0x06000E63 RID: 3683 RVA: 0x0002D3F3 File Offset: 0x0002B5F3
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

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0002D409 File Offset: 0x0002B609
		// (set) Token: 0x06000E65 RID: 3685 RVA: 0x0002D418 File Offset: 0x0002B618
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

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0002D42E File Offset: 0x0002B62E
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x0002D43D File Offset: 0x0002B63D
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

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0002D453 File Offset: 0x0002B653
		// (set) Token: 0x06000E69 RID: 3689 RVA: 0x0002D46A File Offset: 0x0002B66A
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

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0002D488 File Offset: 0x0002B688
		// (set) Token: 0x06000E6B RID: 3691 RVA: 0x0002D49F File Offset: 0x0002B69F
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

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0002D4BD File Offset: 0x0002B6BD
		// (set) Token: 0x06000E6D RID: 3693 RVA: 0x0002D4D4 File Offset: 0x0002B6D4
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

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0002D4F2 File Offset: 0x0002B6F2
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x0002D509 File Offset: 0x0002B709
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

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0002D527 File Offset: 0x0002B727
		// (set) Token: 0x06000E71 RID: 3697 RVA: 0x0002D53E File Offset: 0x0002B73E
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

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0002D55C File Offset: 0x0002B75C
		// (set) Token: 0x06000E73 RID: 3699 RVA: 0x0002D56D File Offset: 0x0002B76D
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

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0002D585 File Offset: 0x0002B785
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x0002D596 File Offset: 0x0002B796
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

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0002D5AE File Offset: 0x0002B7AE
		// (set) Token: 0x06000E77 RID: 3703 RVA: 0x0002D5C5 File Offset: 0x0002B7C5
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

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0002D5E3 File Offset: 0x0002B7E3
		// (set) Token: 0x06000E79 RID: 3705 RVA: 0x0002D5FA File Offset: 0x0002B7FA
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

		// Token: 0x06000E7A RID: 3706 RVA: 0x0002D618 File Offset: 0x0002B818
		public PInvokeInfo(PInvokeAttributes attributes, string entryPoint, ModuleReference module)
		{
			this.attributes = (ushort)attributes;
			this.entry_point = entryPoint;
			this.module = module;
		}

		// Token: 0x040004A1 RID: 1185
		private ushort attributes;

		// Token: 0x040004A2 RID: 1186
		private string entry_point;

		// Token: 0x040004A3 RID: 1187
		private ModuleReference module;
	}
}
