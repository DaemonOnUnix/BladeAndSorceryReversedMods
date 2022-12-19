using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002CE RID: 718
	public sealed class SequencePoint
	{
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x0003BFE8 File Offset: 0x0003A1E8
		public int Offset
		{
			get
			{
				return this.offset.Offset;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600125B RID: 4699 RVA: 0x0003BFF5 File Offset: 0x0003A1F5
		// (set) Token: 0x0600125C RID: 4700 RVA: 0x0003BFFD File Offset: 0x0003A1FD
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

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600125D RID: 4701 RVA: 0x0003C006 File Offset: 0x0003A206
		// (set) Token: 0x0600125E RID: 4702 RVA: 0x0003C00E File Offset: 0x0003A20E
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

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x0003C017 File Offset: 0x0003A217
		// (set) Token: 0x06001260 RID: 4704 RVA: 0x0003C01F File Offset: 0x0003A21F
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

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x0003C028 File Offset: 0x0003A228
		// (set) Token: 0x06001262 RID: 4706 RVA: 0x0003C030 File Offset: 0x0003A230
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

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x0003C039 File Offset: 0x0003A239
		public bool IsHidden
		{
			get
			{
				return this.start_line == 16707566 && this.start_line == this.end_line;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0003C058 File Offset: 0x0003A258
		// (set) Token: 0x06001265 RID: 4709 RVA: 0x0003C060 File Offset: 0x0003A260
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

		// Token: 0x06001266 RID: 4710 RVA: 0x0003C069 File Offset: 0x0003A269
		internal SequencePoint(int offset, Document document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.offset = new InstructionOffset(offset);
			this.document = document;
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0003C092 File Offset: 0x0003A292
		public SequencePoint(Instruction instruction, Document document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.offset = new InstructionOffset(instruction);
			this.document = document;
		}

		// Token: 0x0400093A RID: 2362
		internal InstructionOffset offset;

		// Token: 0x0400093B RID: 2363
		private Document document;

		// Token: 0x0400093C RID: 2364
		private int start_line;

		// Token: 0x0400093D RID: 2365
		private int start_column;

		// Token: 0x0400093E RID: 2366
		private int end_line;

		// Token: 0x0400093F RID: 2367
		private int end_column;
	}
}
