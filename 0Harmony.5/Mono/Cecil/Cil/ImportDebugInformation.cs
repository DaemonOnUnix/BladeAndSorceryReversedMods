using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002DC RID: 732
	public sealed class ImportDebugInformation : DebugInformation
	{
		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x0003C5CC File Offset: 0x0003A7CC
		public bool HasTargets
		{
			get
			{
				return !this.targets.IsNullOrEmpty<ImportTarget>();
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x0003C5DC File Offset: 0x0003A7DC
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

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060012AD RID: 4781 RVA: 0x0003C5FE File Offset: 0x0003A7FE
		// (set) Token: 0x060012AE RID: 4782 RVA: 0x0003C606 File Offset: 0x0003A806
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

		// Token: 0x060012AF RID: 4783 RVA: 0x0003C60F File Offset: 0x0003A80F
		public ImportDebugInformation()
		{
			this.token = new MetadataToken(TokenType.ImportScope);
		}

		// Token: 0x04000974 RID: 2420
		internal ImportDebugInformation parent;

		// Token: 0x04000975 RID: 2421
		internal Collection<ImportTarget> targets;
	}
}
