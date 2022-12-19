using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200023F RID: 575
	internal sealed class RequiredModifierType : TypeSpecification, IModifierType
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x0002B8DA File Offset: 0x00029ADA
		// (set) Token: 0x06000D47 RID: 3399 RVA: 0x0002B8E2 File Offset: 0x00029AE2
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

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x0002B8EB File Offset: 0x00029AEB
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x0002B8FE File Offset: 0x00029AFE
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x0002B911 File Offset: 0x00029B11
		private string Suffix
		{
			get
			{
				string text = " modreq(";
				TypeReference typeReference = this.modifier_type;
				return text + ((typeReference != null) ? typeReference.ToString() : null) + ")";
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00017DC4 File Offset: 0x00015FC4
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsRequiredModifier
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x0002B934 File Offset: 0x00029B34
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.modifier_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0002B94C File Offset: 0x00029B4C
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

		// Token: 0x040003DA RID: 986
		private TypeReference modifier_type;
	}
}
