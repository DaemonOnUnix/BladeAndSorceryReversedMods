using System;

namespace Mono.Cecil
{
	// Token: 0x02000134 RID: 308
	internal sealed class CustomMarshalInfo : MarshalInfo
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00022FEE File Offset: 0x000211EE
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x00022FF6 File Offset: 0x000211F6
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
			set
			{
				this.guid = value;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00022FFF File Offset: 0x000211FF
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x00023007 File Offset: 0x00021207
		public string UnmanagedType
		{
			get
			{
				return this.unmanaged_type;
			}
			set
			{
				this.unmanaged_type = value;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x00023010 File Offset: 0x00021210
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x00023018 File Offset: 0x00021218
		public TypeReference ManagedType
		{
			get
			{
				return this.managed_type;
			}
			set
			{
				this.managed_type = value;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00023021 File Offset: 0x00021221
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x00023029 File Offset: 0x00021229
		public string Cookie
		{
			get
			{
				return this.cookie;
			}
			set
			{
				this.cookie = value;
			}
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00023032 File Offset: 0x00021232
		public CustomMarshalInfo()
			: base(NativeType.CustomMarshaler)
		{
		}

		// Token: 0x04000322 RID: 802
		internal Guid guid;

		// Token: 0x04000323 RID: 803
		internal string unmanaged_type;

		// Token: 0x04000324 RID: 804
		internal TypeReference managed_type;

		// Token: 0x04000325 RID: 805
		internal string cookie;
	}
}
