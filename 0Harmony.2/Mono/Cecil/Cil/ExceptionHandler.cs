using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002B7 RID: 695
	public sealed class ExceptionHandler
	{
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x00038FF1 File Offset: 0x000371F1
		// (set) Token: 0x06001191 RID: 4497 RVA: 0x00038FF9 File Offset: 0x000371F9
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

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x00039002 File Offset: 0x00037202
		// (set) Token: 0x06001193 RID: 4499 RVA: 0x0003900A File Offset: 0x0003720A
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

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x00039013 File Offset: 0x00037213
		// (set) Token: 0x06001195 RID: 4501 RVA: 0x0003901B File Offset: 0x0003721B
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

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x00039024 File Offset: 0x00037224
		// (set) Token: 0x06001197 RID: 4503 RVA: 0x0003902C File Offset: 0x0003722C
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

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x00039035 File Offset: 0x00037235
		// (set) Token: 0x06001199 RID: 4505 RVA: 0x0003903D File Offset: 0x0003723D
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

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x00039046 File Offset: 0x00037246
		// (set) Token: 0x0600119B RID: 4507 RVA: 0x0003904E File Offset: 0x0003724E
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

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x0600119C RID: 4508 RVA: 0x00039057 File Offset: 0x00037257
		// (set) Token: 0x0600119D RID: 4509 RVA: 0x0003905F File Offset: 0x0003725F
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

		// Token: 0x0600119E RID: 4510 RVA: 0x00039068 File Offset: 0x00037268
		public ExceptionHandler(ExceptionHandlerType handlerType)
		{
			this.handler_type = handlerType;
		}

		// Token: 0x040007E2 RID: 2018
		private Instruction try_start;

		// Token: 0x040007E3 RID: 2019
		private Instruction try_end;

		// Token: 0x040007E4 RID: 2020
		private Instruction filter_start;

		// Token: 0x040007E5 RID: 2021
		private Instruction handler_start;

		// Token: 0x040007E6 RID: 2022
		private Instruction handler_end;

		// Token: 0x040007E7 RID: 2023
		private TypeReference catch_type;

		// Token: 0x040007E8 RID: 2024
		private ExceptionHandlerType handler_type;
	}
}
