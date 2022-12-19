using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200023E RID: 574
	internal sealed class OptionalModifierType : TypeSpecification, IModifierType
	{
		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x0002B825 File Offset: 0x00029A25
		// (set) Token: 0x06000D3D RID: 3389 RVA: 0x0002B82D File Offset: 0x00029A2D
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

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x0002B836 File Offset: 0x00029A36
		public override string Name
		{
			get
			{
				return base.Name + this.Suffix;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x0002B849 File Offset: 0x00029A49
		public override string FullName
		{
			get
			{
				return base.FullName + this.Suffix;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x0002B85C File Offset: 0x00029A5C
		private string Suffix
		{
			get
			{
				string text = " modopt(";
				TypeReference typeReference = this.modifier_type;
				return text + ((typeReference != null) ? typeReference.ToString() : null) + ")";
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00017DC4 File Offset: 0x00015FC4
		// (set) Token: 0x06000D42 RID: 3394 RVA: 0x0001845A File Offset: 0x0001665A
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

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsOptionalModifier
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x0002B87F File Offset: 0x00029A7F
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.modifier_type.ContainsGenericParameter || base.ContainsGenericParameter;
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0002B898 File Offset: 0x00029A98
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

		// Token: 0x040003D9 RID: 985
		private TypeReference modifier_type;
	}
}
