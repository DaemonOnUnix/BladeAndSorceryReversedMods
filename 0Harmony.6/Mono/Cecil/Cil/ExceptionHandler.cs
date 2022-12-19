using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C2 RID: 450
	public sealed class ExceptionHandler
	{
		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x000315C5 File Offset: 0x0002F7C5
		// (set) Token: 0x06000E2E RID: 3630 RVA: 0x000315CD File Offset: 0x0002F7CD
		public Instruction TryStart
		{
			get
			{
				return this.try_start;
			}
			set
			{
				this.try_start = value;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000E2F RID: 3631 RVA: 0x000315D6 File Offset: 0x0002F7D6
		// (set) Token: 0x06000E30 RID: 3632 RVA: 0x000315DE File Offset: 0x0002F7DE
		public Instruction TryEnd
		{
			get
			{
				return this.try_end;
			}
			set
			{
				this.try_end = value;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000E31 RID: 3633 RVA: 0x000315E7 File Offset: 0x0002F7E7
		// (set) Token: 0x06000E32 RID: 3634 RVA: 0x000315EF File Offset: 0x0002F7EF
		public Instruction FilterStart
		{
			get
			{
				return this.filter_start;
			}
			set
			{
				this.filter_start = value;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000E33 RID: 3635 RVA: 0x000315F8 File Offset: 0x0002F7F8
		// (set) Token: 0x06000E34 RID: 3636 RVA: 0x00031600 File Offset: 0x0002F800
		public Instruction HandlerStart
		{
			get
			{
				return this.handler_start;
			}
			set
			{
				this.handler_start = value;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x00031609 File Offset: 0x0002F809
		// (set) Token: 0x06000E36 RID: 3638 RVA: 0x00031611 File Offset: 0x0002F811
		public Instruction HandlerEnd
		{
			get
			{
				return this.handler_end;
			}
			set
			{
				this.handler_end = value;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x0003161A File Offset: 0x0002F81A
		// (set) Token: 0x06000E38 RID: 3640 RVA: 0x00031622 File Offset: 0x0002F822
		public TypeReference CatchType
		{
			get
			{
				return this.catch_type;
			}
			set
			{
				this.catch_type = value;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x0003162B File Offset: 0x0002F82B
		// (set) Token: 0x06000E3A RID: 3642 RVA: 0x00031633 File Offset: 0x0002F833
		public ExceptionHandlerType HandlerType
		{
			get
			{
				return this.handler_type;
			}
			set
			{
				this.handler_type = value;
			}
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0003163C File Offset: 0x0002F83C
		public ExceptionHandler(ExceptionHandlerType handlerType)
		{
			this.handler_type = handlerType;
		}

		// Token: 0x040007AA RID: 1962
		private Instruction try_start;

		// Token: 0x040007AB RID: 1963
		private Instruction try_end;

		// Token: 0x040007AC RID: 1964
		private Instruction filter_start;

		// Token: 0x040007AD RID: 1965
		private Instruction handler_start;

		// Token: 0x040007AE RID: 1966
		private Instruction handler_end;

		// Token: 0x040007AF RID: 1967
		private TypeReference catch_type;

		// Token: 0x040007B0 RID: 1968
		private ExceptionHandlerType handler_type;
	}
}
