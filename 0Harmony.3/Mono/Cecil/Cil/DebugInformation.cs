using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E1 RID: 481
	public abstract class DebugInformation : ICustomDebugInformationProvider, IMetadataTokenProvider
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0003455E File Offset: 0x0003275E
		// (set) Token: 0x06000F1F RID: 3871 RVA: 0x00034566 File Offset: 0x00032766
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

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x0003456F File Offset: 0x0003276F
		public bool HasCustomDebugInformations
		{
			get
			{
				return !this.custom_infos.IsNullOrEmpty<CustomDebugInformation>();
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0003457F File Offset: 0x0003277F
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

		// Token: 0x06000F22 RID: 3874 RVA: 0x00002AED File Offset: 0x00000CED
		internal DebugInformation()
		{
		}

		// Token: 0x04000921 RID: 2337
		internal MetadataToken token;

		// Token: 0x04000922 RID: 2338
		internal Collection<CustomDebugInformation> custom_infos;
	}
}
