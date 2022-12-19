using System;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001F3 RID: 499
	public sealed class CallSite : IMethodSignature, IMetadataTokenProvider
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x00025E1C File Offset: 0x0002401C
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x00025E29 File Offset: 0x00024029
		public bool HasThis
		{
			get
			{
				return this.signature.HasThis;
			}
			set
			{
				this.signature.HasThis = value;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00025E37 File Offset: 0x00024037
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x00025E44 File Offset: 0x00024044
		public bool ExplicitThis
		{
			get
			{
				return this.signature.ExplicitThis;
			}
			set
			{
				this.signature.ExplicitThis = value;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00025E52 File Offset: 0x00024052
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x00025E5F File Offset: 0x0002405F
		public MethodCallingConvention CallingConvention
		{
			get
			{
				return this.signature.CallingConvention;
			}
			set
			{
				this.signature.CallingConvention = value;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00025E6D File Offset: 0x0002406D
		public bool HasParameters
		{
			get
			{
				return this.signature.HasParameters;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x00025E7A File Offset: 0x0002407A
		public Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.signature.Parameters;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x00025E87 File Offset: 0x00024087
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x00025E99 File Offset: 0x00024099
		public TypeReference ReturnType
		{
			get
			{
				return this.signature.MethodReturnType.ReturnType;
			}
			set
			{
				this.signature.MethodReturnType.ReturnType = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x00025EAC File Offset: 0x000240AC
		public MethodReturnType MethodReturnType
		{
			get
			{
				return this.signature.MethodReturnType;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x00025EB9 File Offset: 0x000240B9
		// (set) Token: 0x06000A21 RID: 2593 RVA: 0x0001845A File Offset: 0x0001665A
		public string Name
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x00025EB9 File Offset: 0x000240B9
		// (set) Token: 0x06000A23 RID: 2595 RVA: 0x0001845A File Offset: 0x0001665A
		public string Namespace
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x00025EC0 File Offset: 0x000240C0
		public ModuleDefinition Module
		{
			get
			{
				return this.ReturnType.Module;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x00025ECD File Offset: 0x000240CD
		public IMetadataScope Scope
		{
			get
			{
				return this.signature.ReturnType.Scope;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x00025EDF File Offset: 0x000240DF
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x00025EEC File Offset: 0x000240EC
		public MetadataToken MetadataToken
		{
			get
			{
				return this.signature.token;
			}
			set
			{
				this.signature.token = value;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x00025EFC File Offset: 0x000240FC
		public string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.ReturnType.FullName);
				this.MethodSignatureFullName(stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00025F2E File Offset: 0x0002412E
		internal CallSite()
		{
			this.signature = new MethodReference();
			this.signature.token = new MetadataToken(TokenType.Signature, 0);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00025F57 File Offset: 0x00024157
		public CallSite(TypeReference returnType)
			: this()
		{
			if (returnType == null)
			{
				throw new ArgumentNullException("returnType");
			}
			this.signature.ReturnType = returnType;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x00025F79 File Offset: 0x00024179
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x040002D6 RID: 726
		private readonly MethodReference signature;
	}
}
