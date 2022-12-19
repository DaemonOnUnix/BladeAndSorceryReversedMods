using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E6 RID: 486
	public sealed class ImportDebugInformation : DebugInformation
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x00034740 File Offset: 0x00032940
		public bool HasTargets
		{
			get
			{
				return !this.targets.IsNullOrEmpty<ImportTarget>();
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x00034750 File Offset: 0x00032950
		public Collection<ImportTarget> Targets
		{
			get
			{
				if (this.targets == null)
				{
					Interlocked.CompareExchange<Collection<ImportTarget>>(ref this.targets, new Collection<ImportTarget>(), null);
				}
				return this.targets;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x00034772 File Offset: 0x00032972
		// (set) Token: 0x06000F41 RID: 3905 RVA: 0x0003477A File Offset: 0x0003297A
		public ImportDebugInformation Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x00034783 File Offset: 0x00032983
		public ImportDebugInformation()
		{
			this.token = new MetadataToken(TokenType.ImportScope);
		}

		// Token: 0x04000938 RID: 2360
		internal ImportDebugInformation parent;

		// Token: 0x04000939 RID: 2361
		internal Collection<ImportTarget> targets;
	}
}
