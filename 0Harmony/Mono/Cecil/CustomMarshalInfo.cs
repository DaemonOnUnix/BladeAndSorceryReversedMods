using System;

namespace Mono.Cecil
{
	// Token: 0x02000227 RID: 551
	internal sealed class CustomMarshalInfo : MarshalInfo
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x000292C6 File Offset: 0x000274C6
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x000292CE File Offset: 0x000274CE
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

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x000292D7 File Offset: 0x000274D7
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x000292DF File Offset: 0x000274DF
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

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x000292E8 File Offset: 0x000274E8
		// (set) Token: 0x06000BFB RID: 3067 RVA: 0x000292F0 File Offset: 0x000274F0
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

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x000292F9 File Offset: 0x000274F9
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x00029301 File Offset: 0x00027501
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

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002930A File Offset: 0x0002750A
		public CustomMarshalInfo()
			: base(NativeType.CustomMarshaler)
		{
		}

		// Token: 0x04000354 RID: 852
		internal Guid guid;

		// Token: 0x04000355 RID: 853
		internal string unmanaged_type;

		// Token: 0x04000356 RID: 854
		internal TypeReference managed_type;

		// Token: 0x04000357 RID: 855
		internal string cookie;
	}
}
