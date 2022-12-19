using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002D7 RID: 727
	public abstract class DebugInformation : ICustomDebugInformationProvider, IMetadataTokenProvider
	{
		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x0003C3E7 File Offset: 0x0003A5E7
		// (set) Token: 0x0600128C RID: 4748 RVA: 0x0003C3EF File Offset: 0x0003A5EF
		public MetadataToken MetadataToken
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x0003C3F8 File Offset: 0x0003A5F8
		public bool HasCustomDebugInformations
		{
			get
			{
				return !this.custom_infos.IsNullOrEmpty<CustomDebugInformation>();
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x0003C408 File Offset: 0x0003A608
		public Collection<CustomDebugInformation> CustomDebugInformations
		{
			get
			{
				if (this.custom_infos == null)
				{
					Interlocked.CompareExchange<Collection<CustomDebugInformation>>(ref this.custom_infos, new Collection<CustomDebugInformation>(), null);
				}
				return this.custom_infos;
			}
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00002AED File Offset: 0x00000CED
		internal DebugInformation()
		{
		}

		// Token: 0x0400095D RID: 2397
		internal MetadataToken token;

		// Token: 0x0400095E RID: 2398
		internal Collection<CustomDebugInformation> custom_infos;
	}
}
