using System;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000101 RID: 257
	public sealed class CallSite : IMethodSignature, IMetadataTokenProvider
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0001FF74 File Offset: 0x0001E174
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x0001FF81 File Offset: 0x0001E181
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x0001FF8F File Offset: 0x0001E18F
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x0001FF9C File Offset: 0x0001E19C
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

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0001FFAA File Offset: 0x0001E1AA
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x0001FFB7 File Offset: 0x0001E1B7
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

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0001FFC5 File Offset: 0x0001E1C5
		public bool HasParameters
		{
			get
			{
				return this.signature.HasParameters;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001FFD2 File Offset: 0x0001E1D2
		public Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.signature.Parameters;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0001FFDF File Offset: 0x0001E1DF
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x0001FFF1 File Offset: 0x0001E1F1
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

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00020004 File Offset: 0x0001E204
		public MethodReturnType MethodReturnType
		{
			get
			{
				return this.signature.MethodReturnType;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00020011 File Offset: 0x0001E211
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00020011 File Offset: 0x0001E211
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00020018 File Offset: 0x0001E218
		public ModuleDefinition Module
		{
			get
			{
				return this.ReturnType.Module;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00020025 File Offset: 0x0001E225
		public IMetadataScope Scope
		{
			get
			{
				return this.signature.ReturnType.Scope;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x00020037 File Offset: 0x0001E237
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x00020044 File Offset: 0x0001E244
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00020054 File Offset: 0x0001E254
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

		// Token: 0x060006F1 RID: 1777 RVA: 0x00020086 File Offset: 0x0001E286
		internal CallSite()
		{
			this.signature = new MethodReference();
			this.signature.token = new MetadataToken(TokenType.Signature, 0);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x000200AF File Offset: 0x0001E2AF
		public CallSite(TypeReference returnType)
			: this()
		{
			if (returnType == null)
			{
				throw new ArgumentNullException("returnType");
			}
			this.signature.ReturnType = returnType;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x000200D1 File Offset: 0x0001E2D1
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x040002A4 RID: 676
		private readonly MethodReference signature;
	}
}
