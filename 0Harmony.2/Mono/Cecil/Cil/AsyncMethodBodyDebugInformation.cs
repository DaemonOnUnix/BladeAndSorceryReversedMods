using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002E1 RID: 737
	internal sealed class AsyncMethodBodyDebugInformation : CustomDebugInformation
	{
		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x060012B9 RID: 4793 RVA: 0x0003C66F File Offset: 0x0003A86F
		// (set) Token: 0x060012BA RID: 4794 RVA: 0x0003C677 File Offset: 0x0003A877
		public InstructionOffset CatchHandler
		{
			get
			{
				return this.catch_handler;
			}
			set
			{
				this.catch_handler = value;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x0003C680 File Offset: 0x0003A880
		public Collection<InstructionOffset> Yields
		{
			get
			{
				if (this.yields == null)
				{
					Interlocked.CompareExchange<Collection<InstructionOffset>>(ref this.yields, new Collection<InstructionOffset>(), null);
				}
				return this.yields;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060012BC RID: 4796 RVA: 0x0003C6A2 File Offset: 0x0003A8A2
		public Collection<InstructionOffset> Resumes
		{
			get
			{
				if (this.resumes == null)
				{
					Interlocked.CompareExchange<Collection<InstructionOffset>>(ref this.resumes, new Collection<InstructionOffset>(), null);
				}
				return this.resumes;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x0003C6C4 File Offset: 0x0003A8C4
		public Collection<MethodDefinition> ResumeMethods
		{
			get
			{
				Collection<MethodDefinition> collection;
				if ((collection = this.resume_methods) == null)
				{
					collection = (this.resume_methods = new Collection<MethodDefinition>());
				}
				return collection;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x0003C6E9 File Offset: 0x0003A8E9
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.AsyncMethodBody;
			}
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0003C6EC File Offset: 0x0003A8EC
		internal AsyncMethodBodyDebugInformation(int catchHandler)
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(catchHandler);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0003C705 File Offset: 0x0003A905
		public AsyncMethodBodyDebugInformation(Instruction catchHandler)
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(catchHandler);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0003C71E File Offset: 0x0003A91E
		public AsyncMethodBodyDebugInformation()
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(-1);
		}

		// Token: 0x04000980 RID: 2432
		internal InstructionOffset catch_handler;

		// Token: 0x04000981 RID: 2433
		internal Collection<InstructionOffset> yields;

		// Token: 0x04000982 RID: 2434
		internal Collection<InstructionOffset> resumes;

		// Token: 0x04000983 RID: 2435
		internal Collection<MethodDefinition> resume_methods;

		// Token: 0x04000984 RID: 2436
		public static Guid KindIdentifier = new Guid("{54FD2AC5-E925-401A-9C2A-F94F171072F8}");
	}
}
