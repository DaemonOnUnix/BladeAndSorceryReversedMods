using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000201 RID: 513
	public sealed class FieldDefinition : FieldReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider, IMarshalInfoProvider
	{
		// Token: 0x06000ABD RID: 2749 RVA: 0x00026D24 File Offset: 0x00024F24
		private void ResolveLayout()
		{
			if (this.offset != -2)
			{
				return;
			}
			if (!base.HasImage)
			{
				this.offset = -1;
				return;
			}
			object syncRoot = this.Module.SyncRoot;
			lock (syncRoot)
			{
				if (this.offset == -2)
				{
					this.offset = this.Module.Read<FieldDefinition, int>(this, (FieldDefinition field, MetadataReader reader) => reader.ReadFieldLayout(field));
				}
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x00026DBC File Offset: 0x00024FBC
		public bool HasLayoutInfo
		{
			get
			{
				if (this.offset >= 0)
				{
					return true;
				}
				this.ResolveLayout();
				return this.offset >= 0;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00026DDB File Offset: 0x00024FDB
		// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x00026E04 File Offset: 0x00025004
		public int Offset
		{
			get
			{
				if (this.offset >= 0)
				{
					return this.offset;
				}
				this.ResolveLayout();
				if (this.offset < 0)
				{
					return -1;
				}
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00026E0D File Offset: 0x0002500D
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x00026E1A File Offset: 0x0002501A
		internal FieldDefinitionProjection WindowsRuntimeProjection
		{
			get
			{
				return (FieldDefinitionProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00026E24 File Offset: 0x00025024
		private void ResolveRVA()
		{
			if (this.rva != -2)
			{
				return;
			}
			if (!base.HasImage)
			{
				return;
			}
			object syncRoot = this.Module.SyncRoot;
			lock (syncRoot)
			{
				if (this.rva == -2)
				{
					this.rva = this.Module.Read<FieldDefinition, int>(this, (FieldDefinition field, MetadataReader reader) => reader.ReadFieldRVA(field));
				}
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00026EB4 File Offset: 0x000250B4
		public int RVA
		{
			get
			{
				if (this.rva > 0)
				{
					return this.rva;
				}
				this.ResolveRVA();
				if (this.rva <= 0)
				{
					return 0;
				}
				return this.rva;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x00026EDD File Offset: 0x000250DD
		// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x00026F0D File Offset: 0x0002510D
		public byte[] InitialValue
		{
			get
			{
				if (this.initial_value != null)
				{
					return this.initial_value;
				}
				this.ResolveRVA();
				if (this.initial_value == null)
				{
					this.initial_value = Empty<byte>.Array;
				}
				return this.initial_value;
			}
			set
			{
				this.initial_value = value;
				this.HasFieldRVA = !this.initial_value.IsNullOrEmpty<byte>();
				this.rva = 0;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x00026F31 File Offset: 0x00025131
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x00026F39 File Offset: 0x00025139
		public FieldAttributes Attributes
		{
			get
			{
				return (FieldAttributes)this.attributes;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != (FieldAttributes)this.attributes)
				{
					throw new InvalidOperationException();
				}
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00026F59 File Offset: 0x00025159
		// (set) Token: 0x06000ACA RID: 2762 RVA: 0x00026F7D File Offset: 0x0002517D
		public bool HasConstant
		{
			get
			{
				this.ResolveConstant(ref this.constant, this.Module);
				return this.constant != Mixin.NoValue;
			}
			set
			{
				if (!value)
				{
					this.constant = Mixin.NoValue;
				}
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x00026F8D File Offset: 0x0002518D
		// (set) Token: 0x06000ACC RID: 2764 RVA: 0x00026F9F File Offset: 0x0002519F
		public object Constant
		{
			get
			{
				if (!this.HasConstant)
				{
					return null;
				}
				return this.constant;
			}
			set
			{
				this.constant = value;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00026FA8 File Offset: 0x000251A8
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.Module);
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x00026FCD File Offset: 0x000251CD
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x00026FEB File Offset: 0x000251EB
		public bool HasMarshalInfo
		{
			get
			{
				return this.marshal_info != null || this.GetHasMarshalInfo(this.Module);
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x00027003 File Offset: 0x00025203
		// (set) Token: 0x06000AD1 RID: 2769 RVA: 0x00027021 File Offset: 0x00025221
		public MarshalInfo MarshalInfo
		{
			get
			{
				return this.marshal_info ?? this.GetMarshalInfo(ref this.marshal_info, this.Module);
			}
			set
			{
				this.marshal_info = value;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x0002702A File Offset: 0x0002522A
		// (set) Token: 0x06000AD3 RID: 2771 RVA: 0x00027039 File Offset: 0x00025239
		public bool IsCompilerControlled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 0U, value);
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0002704F File Offset: 0x0002524F
		// (set) Token: 0x06000AD5 RID: 2773 RVA: 0x0002705E File Offset: 0x0002525E
		public bool IsPrivate
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 1U, value);
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00027074 File Offset: 0x00025274
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x00027083 File Offset: 0x00025283
		public bool IsFamilyAndAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 2U, value);
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00027099 File Offset: 0x00025299
		// (set) Token: 0x06000AD9 RID: 2777 RVA: 0x000270A8 File Offset: 0x000252A8
		public bool IsAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 3U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 3U, value);
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x000270BE File Offset: 0x000252BE
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x000270CD File Offset: 0x000252CD
		public bool IsFamily
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 4U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 4U, value);
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x000270E3 File Offset: 0x000252E3
		// (set) Token: 0x06000ADD RID: 2781 RVA: 0x000270F2 File Offset: 0x000252F2
		public bool IsFamilyOrAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 5U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 5U, value);
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x00027108 File Offset: 0x00025308
		// (set) Token: 0x06000ADF RID: 2783 RVA: 0x00027117 File Offset: 0x00025317
		public bool IsPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 6U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 6U, value);
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0002712D File Offset: 0x0002532D
		// (set) Token: 0x06000AE1 RID: 2785 RVA: 0x0002713C File Offset: 0x0002533C
		public bool IsStatic
		{
			get
			{
				return this.attributes.GetAttributes(16);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16, value);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x00027152 File Offset: 0x00025352
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x00027161 File Offset: 0x00025361
		public bool IsInitOnly
		{
			get
			{
				return this.attributes.GetAttributes(32);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(32, value);
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00027177 File Offset: 0x00025377
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x00027186 File Offset: 0x00025386
		public bool IsLiteral
		{
			get
			{
				return this.attributes.GetAttributes(64);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(64, value);
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0002719C File Offset: 0x0002539C
		// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x000271AE File Offset: 0x000253AE
		public bool IsNotSerialized
		{
			get
			{
				return this.attributes.GetAttributes(128);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(128, value);
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x000271C7 File Offset: 0x000253C7
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x000271D9 File Offset: 0x000253D9
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(512);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(512, value);
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x000271F2 File Offset: 0x000253F2
		// (set) Token: 0x06000AEB RID: 2795 RVA: 0x00027204 File Offset: 0x00025404
		public bool IsPInvokeImpl
		{
			get
			{
				return this.attributes.GetAttributes(8192);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8192, value);
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0002721D File Offset: 0x0002541D
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x0002722F File Offset: 0x0002542F
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(1024);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024, value);
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x00027248 File Offset: 0x00025448
		// (set) Token: 0x06000AEF RID: 2799 RVA: 0x0002725A File Offset: 0x0002545A
		public bool HasDefault
		{
			get
			{
				return this.attributes.GetAttributes(32768);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(32768, value);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x00027273 File Offset: 0x00025473
		// (set) Token: 0x06000AF1 RID: 2801 RVA: 0x00027285 File Offset: 0x00025485
		public bool HasFieldRVA
		{
			get
			{
				return this.attributes.GetAttributes(256);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(256, value);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0002667C File Offset: 0x0002487C
		// (set) Token: 0x06000AF4 RID: 2804 RVA: 0x00026689 File Offset: 0x00024889
		public new TypeDefinition DeclaringType
		{
			get
			{
				return (TypeDefinition)base.DeclaringType;
			}
			set
			{
				base.DeclaringType = value;
			}
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0002729E File Offset: 0x0002549E
		public FieldDefinition(string name, FieldAttributes attributes, TypeReference fieldType)
			: base(name, fieldType)
		{
			this.attributes = (ushort)attributes;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00017E2C File Offset: 0x0001602C
		public override FieldDefinition Resolve()
		{
			return this;
		}

		// Token: 0x04000312 RID: 786
		private ushort attributes;

		// Token: 0x04000313 RID: 787
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000314 RID: 788
		private int offset = -2;

		// Token: 0x04000315 RID: 789
		internal int rva = -2;

		// Token: 0x04000316 RID: 790
		private byte[] initial_value;

		// Token: 0x04000317 RID: 791
		private object constant = Mixin.NotResolved;

		// Token: 0x04000318 RID: 792
		private MarshalInfo marshal_info;
	}
}
