using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000148 RID: 328
	internal abstract class MethodSpecification : MethodReference
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x00025134 File Offset: 0x00023334
		public MethodReference ElementMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0002513C File Offset: 0x0002333C
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x000125CE File Offset: 0x000107CE
		public override string Name
		{
			get
			{
				return this.method.Name;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00025149 File Offset: 0x00023349
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x000125CE File Offset: 0x000107CE
		public override MethodCallingConvention CallingConvention
		{
			get
			{
				return this.method.CallingConvention;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00025156 File Offset: 0x00023356
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x000125CE File Offset: 0x000107CE
		public override bool HasThis
		{
			get
			{
				return this.method.HasThis;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00025163 File Offset: 0x00023363
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x000125CE File Offset: 0x000107CE
		public override bool ExplicitThis
		{
			get
			{
				return this.method.ExplicitThis;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x00025170 File Offset: 0x00023370
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x000125CE File Offset: 0x000107CE
		public override MethodReturnType MethodReturnType
		{
			get
			{
				return this.method.MethodReturnType;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0002517D File Offset: 0x0002337D
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x000125CE File Offset: 0x000107CE
		public override TypeReference DeclaringType
		{
			get
			{
				return this.method.DeclaringType;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x0002518A File Offset: 0x0002338A
		public override ModuleDefinition Module
		{
			get
			{
				return this.method.Module;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x00025197 File Offset: 0x00023397
		public override bool HasParameters
		{
			get
			{
				return this.method.HasParameters;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x000251A4 File Offset: 0x000233A4
		public override Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.method.Parameters;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x000251B1 File Offset: 0x000233B1
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.method.ContainsGenericParameter;
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x000251BE File Offset: 0x000233BE
		internal MethodSpecification(MethodReference method)
		{
			Mixin.CheckMethod(method);
			this.method = method;
			this.token = new MetadataToken(TokenType.MethodSpec);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x000251E3 File Offset: 0x000233E3
		public sealed override MethodReference GetElementMethod()
		{
			return this.method.GetElementMethod();
		}

		// Token: 0x040003A4 RID: 932
		private readonly MethodReference method;
	}
}
