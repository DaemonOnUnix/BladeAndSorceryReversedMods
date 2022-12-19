using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D8 RID: 472
	public sealed class SequencePoint
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x00034194 File Offset: 0x00032394
		public int Offset
		{
			get
			{
				return this.offset.Offset;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x000341A1 File Offset: 0x000323A1
		// (set) Token: 0x06000EF3 RID: 3827 RVA: 0x000341A9 File Offset: 0x000323A9
		public int StartLine
		{
			get
			{
				return this.start_line;
			}
			set
			{
				this.start_line = value;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x000341B2 File Offset: 0x000323B2
		// (set) Token: 0x06000EF5 RID: 3829 RVA: 0x000341BA File Offset: 0x000323BA
		public int StartColumn
		{
			get
			{
				return this.start_column;
			}
			set
			{
				this.start_column = value;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x000341C3 File Offset: 0x000323C3
		// (set) Token: 0x06000EF7 RID: 3831 RVA: 0x000341CB File Offset: 0x000323CB
		public int EndLine
		{
			get
			{
				return this.end_line;
			}
			set
			{
				this.end_line = value;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x000341D4 File Offset: 0x000323D4
		// (set) Token: 0x06000EF9 RID: 3833 RVA: 0x000341DC File Offset: 0x000323DC
		public int EndColumn
		{
			get
			{
				return this.end_column;
			}
			set
			{
				this.end_column = value;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000EFA RID: 3834 RVA: 0x000341E5 File Offset: 0x000323E5
		public bool IsHidden
		{
			get
			{
				return this.start_line == 16707566 && this.start_line == this.end_line;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x00034204 File Offset: 0x00032404
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x0003420C File Offset: 0x0003240C
		public Document Document
		{
			get
			{
				return this.document;
			}
			set
			{
				this.document = value;
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00034215 File Offset: 0x00032415
		internal SequencePoint(int offset, Document document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.offset = new InstructionOffset(offset);
			this.document = document;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0003423E File Offset: 0x0003243E
		public SequencePoint(Instruction instruction, Document document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.offset = new InstructionOffset(instruction);
			this.document = document;
		}

		// Token: 0x040008FE RID: 2302
		internal InstructionOffset offset;

		// Token: 0x040008FF RID: 2303
		private Document document;

		// Token: 0x04000900 RID: 2304
		private int start_line;

		// Token: 0x04000901 RID: 2305
		private int start_column;

		// Token: 0x04000902 RID: 2306
		private int end_line;

		// Token: 0x04000903 RID: 2307
		private int end_column;
	}
}
