using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000113 RID: 275
	internal sealed class FunctionPointerType : TypeSpecification, IMethodSignature, IMetadataTokenProvider
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x000214BE File Offset: 0x0001F6BE
		// (set) Token: 0x060007CB RID: 1995 RVA: 0x000214CB File Offset: 0x0001F6CB
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x000214D9 File Offset: 0x0001F6D9
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x000214E6 File Offset: 0x0001F6E6
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

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x000214F4 File Offset: 0x0001F6F4
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x00021501 File Offset: 0x0001F701
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0002150F File Offset: 0x0001F70F
		public bool HasParameters
		{
			get
			{
				return this.function.HasParameters;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0002151C File Offset: 0x0001F71C
		public Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.function.Parameters;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x00021529 File Offset: 0x0001F729
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x0002153B File Offset: 0x0001F73B
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0002154E File Offset: 0x0001F74E
		public MethodReturnType MethodReturnType
		{
			get
			{
				return this.function.MethodReturnType;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0002155B File Offset: 0x0001F75B
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x00020011 File Offset: 0x0001E211
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00021568 File Offset: 0x0001F768
		public override ModuleDefinition Module
		{
			get
			{
				return this.ReturnType.Module;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x00021575 File Offset: 0x0001F775
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsFunctionPointer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00021587 File Offset: 0x0001F787
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.function.ContainsGenericParameter;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00021594 File Offset: 0x0001F794
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

		// Token: 0x060007DF RID: 2015 RVA: 0x000215F5 File Offset: 0x0001F7F5
		public FunctionPointerType()
			: base(null)
		{
			this.function = new MethodReference();
			this.function.Name = "method";
			this.etype = Mono.Cecil.Metadata.ElementType.FnPtr;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00021621 File Offset: 0x0001F821
		public override TypeDefinition Resolve()
		{
			return null;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00011FA0 File Offset: 0x000101A0
		public override TypeReference GetElementType()
		{
			return this;
		}

		// Token: 0x040002EE RID: 750
		private readonly MethodReference function;
	}
}
