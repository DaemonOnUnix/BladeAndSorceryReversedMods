using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002DB RID: 731
	public sealed class ImportTarget
	{
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x0003C568 File Offset: 0x0003A768
		// (set) Token: 0x060012A1 RID: 4769 RVA: 0x0003C570 File Offset: 0x0003A770
		public string Namespace
		{
			get
			{
				return this.@namespace;
			}
			set
			{
				this.@namespace = value;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060012A2 RID: 4770 RVA: 0x0003C579 File Offset: 0x0003A779
		// (set) Token: 0x060012A3 RID: 4771 RVA: 0x0003C581 File Offset: 0x0003A781
		public TypeReference Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x0003C58A File Offset: 0x0003A78A
		// (set) Token: 0x060012A5 RID: 4773 RVA: 0x0003C592 File Offset: 0x0003A792
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
			set
			{
				this.reference = value;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x0003C59B File Offset: 0x0003A79B
		// (set) Token: 0x060012A7 RID: 4775 RVA: 0x0003C5A3 File Offset: 0x0003A7A3
		public string Alias
		{
			get
			{
				return this.alias;
			}
			set
			{
				this.alias = value;
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0003C5AC File Offset: 0x0003A7AC
		// (set) Token: 0x060012A9 RID: 4777 RVA: 0x0003C5B4 File Offset: 0x0003A7B4
		public ImportTargetKind Kind
		{
			get
			{
				return this.kind;
			}
			set
			{
				this.kind = value;
			}
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0003C5BD File Offset: 0x0003A7BD
		public ImportTarget(ImportTargetKind kind)
		{
			this.kind = kind;
		}

		// Token: 0x0400096F RID: 2415
		internal ImportTargetKind kind;

		// Token: 0x04000970 RID: 2416
		internal string @namespace;

		// Token: 0x04000971 RID: 2417
		internal TypeReference type;

		// Token: 0x04000972 RID: 2418
		internal AssemblyNameReference reference;

		// Token: 0x04000973 RID: 2419
		internal string alias;
	}
}
