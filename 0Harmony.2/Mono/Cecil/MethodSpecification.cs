using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200023C RID: 572
	internal abstract class MethodSpecification : MethodReference
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x0002B769 File Offset: 0x00029969
		public MethodReference ElementMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x0002B771 File Offset: 0x00029971
		// (set) Token: 0x06000D29 RID: 3369 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0002B77E File Offset: 0x0002997E
		// (set) Token: 0x06000D2B RID: 3371 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x0002B78B File Offset: 0x0002998B
		// (set) Token: 0x06000D2D RID: 3373 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x0002B798 File Offset: 0x00029998
		// (set) Token: 0x06000D2F RID: 3375 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x0002B7A5 File Offset: 0x000299A5
		// (set) Token: 0x06000D31 RID: 3377 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000D32 RID: 3378 RVA: 0x0002B7B2 File Offset: 0x000299B2
		// (set) Token: 0x06000D33 RID: 3379 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x0002B7BF File Offset: 0x000299BF
		public override ModuleDefinition Module
		{
			get
			{
				return this.method.Module;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x0002B7CC File Offset: 0x000299CC
		public override bool HasParameters
		{
			get
			{
				return this.method.HasParameters;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000D36 RID: 3382 RVA: 0x0002B7D9 File Offset: 0x000299D9
		public override Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.method.Parameters;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x0002B7E6 File Offset: 0x000299E6
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.method.ContainsGenericParameter;
			}
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0002B7F3 File Offset: 0x000299F3
		internal MethodSpecification(MethodReference method)
		{
			Mixin.CheckMethod(method);
			this.method = method;
			this.token = new MetadataToken(TokenType.MethodSpec);
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0002B818 File Offset: 0x00029A18
		public sealed override MethodReference GetElementMethod()
		{
			return this.method.GetElementMethod();
		}

		// Token: 0x040003D8 RID: 984
		private readonly MethodReference method;
	}
}
