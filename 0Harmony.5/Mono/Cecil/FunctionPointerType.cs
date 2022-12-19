using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000205 RID: 517
	internal sealed class FunctionPointerType : TypeSpecification, IMethodSignature, IMetadataTokenProvider
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x000273A5 File Offset: 0x000255A5
		// (set) Token: 0x06000B05 RID: 2821 RVA: 0x000273B2 File Offset: 0x000255B2
		public bool HasThis
		{
			get
			{
				return this.function.HasThis;
			}
			set
			{
				this.function.HasThis = value;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x000273C0 File Offset: 0x000255C0
		// (set) Token: 0x06000B07 RID: 2823 RVA: 0x000273CD File Offset: 0x000255CD
		public bool ExplicitThis
		{
			get
			{
				return this.function.ExplicitThis;
			}
			set
			{
				this.function.ExplicitThis = value;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x000273DB File Offset: 0x000255DB
		// (set) Token: 0x06000B09 RID: 2825 RVA: 0x000273E8 File Offset: 0x000255E8
		public MethodCallingConvention CallingConvention
		{
			get
			{
				return this.function.CallingConvention;
			}
			set
			{
				this.function.CallingConvention = value;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x000273F6 File Offset: 0x000255F6
		public bool HasParameters
		{
			get
			{
				return this.function.HasParameters;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x00027403 File Offset: 0x00025603
		public Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.function.Parameters;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00027410 File Offset: 0x00025610
		// (set) Token: 0x06000B0D RID: 2829 RVA: 0x00027422 File Offset: 0x00025622
		public TypeReference ReturnType
		{
			get
			{
				return this.function.MethodReturnType.ReturnType;
			}
			set
			{
				this.function.MethodReturnType.ReturnType = value;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00027435 File Offset: 0x00025635
		public MethodReturnType MethodReturnType
		{
			get
			{
				return this.function.MethodReturnType;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x00027442 File Offset: 0x00025642
		// (set) Token: 0x06000B10 RID: 2832 RVA: 0x0001845A File Offset: 0x0001665A
		public override string Name
		{
			get
			{
				return this.function.Name;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x00025EB9 File Offset: 0x000240B9
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x0001845A File Offset: 0x0001665A
		public override string Namespace
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

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0002744F File Offset: 0x0002564F
		public override ModuleDefinition Module
		{
			get
			{
				return this.ReturnType.Module;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x0002745C File Offset: 0x0002565C
		// (set) Token: 0x06000B15 RID: 2837 RVA: 0x0001845A File Offset: 0x0001665A
		public override IMetadataScope Scope
		{
			get
			{
				return this.function.ReturnType.Scope;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsFunctionPointer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0002746E File Offset: 0x0002566E
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.function.ContainsGenericParameter;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x0002747C File Offset: 0x0002567C
		public override string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.function.Name);
				stringBuilder.Append(" ");
				stringBuilder.Append(this.function.ReturnType.FullName);
				stringBuilder.Append(" *");
				this.MethodSignatureFullName(stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x000274DD File Offset: 0x000256DD
		public FunctionPointerType()
			: base(null)
		{
			this.function = new MethodReference();
			this.function.Name = "method";
			this.etype = Mono.Cecil.Metadata.ElementType.FnPtr;
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00027509 File Offset: 0x00025709
		public override TypeDefinition Resolve()
		{
			return null;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00017E2C File Offset: 0x0001602C
		public override TypeReference GetElementType()
		{
			return this;
		}

		// Token: 0x04000320 RID: 800
		private readonly MethodReference function;
	}
}
