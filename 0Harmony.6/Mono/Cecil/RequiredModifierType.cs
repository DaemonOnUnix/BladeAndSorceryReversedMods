using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200014B RID: 331
	internal sealed class RequiredModifierType : TypeSpecification, IModifierType
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x000252A6 File Offset: 0x000234A6
		// (set) Token: 0x060009FE RID: 2558 RVA: 0x000252AE File Offset: 0x000234AE
		public TypeReference ModifierType
		{
			get
			{
				return this.modifier_type;
			}
			set
			{
				this.modifier_type = value;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x000252B7 File Offset: 0x000234B7
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x000252CA File Offset: 0x000234CA
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x000252DD File Offset: 0x000234DD
		private string Suffix
		{
			get
			{
				string text = " modreq(";
				TypeReference typeReference = this.modifier_type;
				return text + ((typeReference != null) ? typeReference.ToString() : null) + ")";
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00011F38 File Offset: 0x00010138
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x000125CE File Offset: 0x000107CE
		public override bool IsValueType
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsRequiredModifier
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x00025300 File Offset: 0x00023500
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.modifier_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00025318 File Offset: 0x00023518
		public RequiredModifierType(TypeReference modifierType, TypeReference type)
			: base(type)
		{
			if (modifierType == null)
			{
				throw new ArgumentNullException(Mixin.Argument.modifierType.ToString());
			}
			Mixin.CheckType(type);
			this.modifier_type = modifierType;
			this.etype = Mono.Cecil.Metadata.ElementType.CModReqD;
		}

		// Token: 0x040003A6 RID: 934
		private TypeReference modifier_type;
	}
}
