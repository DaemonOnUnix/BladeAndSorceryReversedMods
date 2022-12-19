using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000146 RID: 326
	public sealed class MethodReturnType : IConstantProvider, IMetadataTokenProvider, ICustomAttributeProvider, IMarshalInfoProvider
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00024FAD File Offset: 0x000231AD
		public IMethodSignature Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x00024FB5 File Offset: 0x000231B5
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x00024FBD File Offset: 0x000231BD
		public TypeReference ReturnType
		{
			get
			{
				return this.return_type;
			}
			set
			{
				this.return_type = value;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x00024FC6 File Offset: 0x000231C6
		internal ParameterDefinition Parameter
		{
			get
			{
				if (this.parameter == null)
				{
					Interlocked.CompareExchange<ParameterDefinition>(ref this.parameter, new ParameterDefinition(this.return_type, this.method), null);
				}
				return this.parameter;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x00024FF4 File Offset: 0x000231F4
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x00025001 File Offset: 0x00023201
		public MetadataToken MetadataToken
		{
			get
			{
				return this.Parameter.MetadataToken;
			}
			set
			{
				this.Parameter.MetadataToken = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0002500F File Offset: 0x0002320F
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x0002501C File Offset: 0x0002321C
		public ParameterAttributes Attributes
		{
			get
			{
				return this.Parameter.Attributes;
			}
			set
			{
				this.Parameter.Attributes = value;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0002502A File Offset: 0x0002322A
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x00025037 File Offset: 0x00023237
		public string Name
		{
			get
			{
				return this.Parameter.Name;
			}
			set
			{
				this.Parameter.Name = value;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00025045 File Offset: 0x00023245
		public bool HasCustomAttributes
		{
			get
			{
				return this.parameter != null && this.parameter.HasCustomAttributes;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0002505C File Offset: 0x0002325C
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.Parameter.CustomAttributes;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x00025069 File Offset: 0x00023269
		// (set) Token: 0x060009D3 RID: 2515 RVA: 0x00025080 File Offset: 0x00023280
		public bool HasDefault
		{
			get
			{
				return this.parameter != null && this.parameter.HasDefault;
			}
			set
			{
				this.Parameter.HasDefault = value;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0002508E File Offset: 0x0002328E
		// (set) Token: 0x060009D5 RID: 2517 RVA: 0x000250A5 File Offset: 0x000232A5
		public bool HasConstant
		{
			get
			{
				return this.parameter != null && this.parameter.HasConstant;
			}
			set
			{
				this.Parameter.HasConstant = value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x000250B3 File Offset: 0x000232B3
		// (set) Token: 0x060009D7 RID: 2519 RVA: 0x000250C0 File Offset: 0x000232C0
		public object Constant
		{
			get
			{
				return this.Parameter.Constant;
			}
			set
			{
				this.Parameter.Constant = value;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x000250CE File Offset: 0x000232CE
		// (set) Token: 0x060009D9 RID: 2521 RVA: 0x000250E5 File Offset: 0x000232E5
		public bool HasFieldMarshal
		{
			get
			{
				return this.parameter != null && this.parameter.HasFieldMarshal;
			}
			set
			{
				this.Parameter.HasFieldMarshal = value;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x000250F3 File Offset: 0x000232F3
		public bool HasMarshalInfo
		{
			get
			{
				return this.parameter != null && this.parameter.HasMarshalInfo;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0002510A File Offset: 0x0002330A
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x00025117 File Offset: 0x00023317
		public MarshalInfo MarshalInfo
		{
			get
			{
				return this.Parameter.MarshalInfo;
			}
			set
			{
				this.Parameter.MarshalInfo = value;
			}
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00025125 File Offset: 0x00023325
		public MethodReturnType(IMethodSignature method)
		{
			this.method = method;
		}

		// Token: 0x04000399 RID: 921
		internal IMethodSignature method;

		// Token: 0x0400039A RID: 922
		internal ParameterDefinition parameter;

		// Token: 0x0400039B RID: 923
		private TypeReference return_type;
	}
}
