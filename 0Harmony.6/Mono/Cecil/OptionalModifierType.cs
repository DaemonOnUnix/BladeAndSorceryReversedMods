using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200014A RID: 330
	internal sealed class OptionalModifierType : TypeSpecification, IModifierType
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x000251F0 File Offset: 0x000233F0
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x000251F8 File Offset: 0x000233F8
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

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x00025201 File Offset: 0x00023401
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x00025214 File Offset: 0x00023414
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x00025227 File Offset: 0x00023427
		private string Suffix
		{
			get
			{
				string text = " modopt(";
				TypeReference typeReference = this.modifier_type;
				return text + ((typeReference != null) ? typeReference.ToString() : null) + ")";
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x00011F38 File Offset: 0x00010138
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x000125CE File Offset: 0x000107CE
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

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsOptionalModifier
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x0002524A File Offset: 0x0002344A
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.modifier_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00025264 File Offset: 0x00023464
		public OptionalModifierType(TypeReference modifierType, TypeReference type)
			: base(type)
		{
			if (modifierType == null)
			{
				throw new ArgumentNullException(Mixin.Argument.modifierType.ToString());
			}
			Mixin.CheckType(type);
			this.modifier_type = modifierType;
			this.etype = Mono.Cecil.Metadata.ElementType.CModOpt;
		}

		// Token: 0x040003A5 RID: 933
		private TypeReference modifier_type;
	}
}
