using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001EB RID: 491
	internal sealed class AsyncMethodBodyDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000F4C RID: 3916 RVA: 0x000347E3 File Offset: 0x000329E3
		// (set) Token: 0x06000F4D RID: 3917 RVA: 0x000347EB File Offset: 0x000329EB
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

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000F4E RID: 3918 RVA: 0x000347F4 File Offset: 0x000329F4
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

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000F4F RID: 3919 RVA: 0x00034816 File Offset: 0x00032A16
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

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x00034838 File Offset: 0x00032A38
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

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x0003485D File Offset: 0x00032A5D
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.AsyncMethodBody;
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00034860 File Offset: 0x00032A60
		internal AsyncMethodBodyDebugInformation(int catchHandler)
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(catchHandler);
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00034879 File Offset: 0x00032A79
		public AsyncMethodBodyDebugInformation(Instruction catchHandler)
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(catchHandler);
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x00034892 File Offset: 0x00032A92
		public AsyncMethodBodyDebugInformation()
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(-1);
		}

		// Token: 0x04000944 RID: 2372
		internal InstructionOffset catch_handler;

		// Token: 0x04000945 RID: 2373
		internal Collection<InstructionOffset> yields;

		// Token: 0x04000946 RID: 2374
		internal Collection<InstructionOffset> resumes;

		// Token: 0x04000947 RID: 2375
		internal Collection<MethodDefinition> resume_methods;

		// Token: 0x04000948 RID: 2376
		public static Guid KindIdentifier = new Guid("{54FD2AC5-E925-401A-9C2A-F94F171072F8}");
	}
}
