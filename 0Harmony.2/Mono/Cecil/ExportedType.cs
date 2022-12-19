using System;

namespace Mono.Cecil
{
	// Token: 0x020001FF RID: 511
	public sealed class ExportedType : IMetadataTokenProvider
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x000267A7 File Offset: 0x000249A7
		// (set) Token: 0x06000A79 RID: 2681 RVA: 0x000267AF File Offset: 0x000249AF
		public string Namespace
		{
			get
			{
				return this.@namespace;
			}
			set
			{
				this.@namespace = value;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x000267B8 File Offset: 0x000249B8
		// (set) Token: 0x06000A7B RID: 2683 RVA: 0x000267C0 File Offset: 0x000249C0
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x000267C9 File Offset: 0x000249C9
		// (set) Token: 0x06000A7D RID: 2685 RVA: 0x000267D1 File Offset: 0x000249D1
		public TypeAttributes Attributes
		{
			get
			{
				return (TypeAttributes)this.attributes;
			}
			set
			{
				this.attributes = (uint)value;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x000267DA File Offset: 0x000249DA
		// (set) Token: 0x06000A7F RID: 2687 RVA: 0x000267F6 File Offset: 0x000249F6
		public IMetadataScope Scope
		{
			get
			{
				if (this.declaring_type != null)
				{
					return this.declaring_type.Scope;
				}
				return this.scope;
			}
			set
			{
				if (this.declaring_type != null)
				{
					this.declaring_type.Scope = value;
					return;
				}
				this.scope = value;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x00026814 File Offset: 0x00024A14
		// (set) Token: 0x06000A81 RID: 2689 RVA: 0x0002681C File Offset: 0x00024A1C
		public ExportedType DeclaringType
		{
			get
			{
				return this.declaring_type;
			}
			set
			{
				this.declaring_type = value;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x00026825 File Offset: 0x00024A25
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x0002682D File Offset: 0x00024A2D
		public MetadataToken MetadataToken
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x00026836 File Offset: 0x00024A36
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x0002683E File Offset: 0x00024A3E
		public int Identifier
		{
			get
			{
				return this.identifier;
			}
			set
			{
				this.identifier = value;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x00026847 File Offset: 0x00024A47
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x00026856 File Offset: 0x00024A56
		public bool IsNotPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 0U, value);
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0002686C File Offset: 0x00024A6C
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x0002687B File Offset: 0x00024A7B
		public bool IsPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 1U, value);
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x00026891 File Offset: 0x00024A91
		// (set) Token: 0x06000A8B RID: 2699 RVA: 0x000268A0 File Offset: 0x00024AA0
		public bool IsNestedPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 2U, value);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x000268B6 File Offset: 0x00024AB6
		// (set) Token: 0x06000A8D RID: 2701 RVA: 0x000268C5 File Offset: 0x00024AC5
		public bool IsNestedPrivate
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 3U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 3U, value);
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x000268DB File Offset: 0x00024ADB
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x000268EA File Offset: 0x00024AEA
		public bool IsNestedFamily
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 4U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 4U, value);
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00026900 File Offset: 0x00024B00
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x0002690F File Offset: 0x00024B0F
		public bool IsNestedAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 5U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 5U, value);
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x00026925 File Offset: 0x00024B25
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x00026934 File Offset: 0x00024B34
		public bool IsNestedFamilyAndAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 6U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 6U, value);
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0002694A File Offset: 0x00024B4A
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x00026959 File Offset: 0x00024B59
		public bool IsNestedFamilyOrAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7U, 7U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7U, 7U, value);
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0002696F File Offset: 0x00024B6F
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x0002697F File Offset: 0x00024B7F
		public bool IsAutoLayout
		{
			get
			{
				return this.attributes.GetMaskedAttributes(24U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(24U, 0U, value);
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x00026996 File Offset: 0x00024B96
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x000269A6 File Offset: 0x00024BA6
		public bool IsSequentialLayout
		{
			get
			{
				return this.attributes.GetMaskedAttributes(24U, 8U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(24U, 8U, value);
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x000269BD File Offset: 0x00024BBD
		// (set) Token: 0x06000A9B RID: 2715 RVA: 0x000269CE File Offset: 0x00024BCE
		public bool IsExplicitLayout
		{
			get
			{
				return this.attributes.GetMaskedAttributes(24U, 16U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(24U, 16U, value);
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x000269E6 File Offset: 0x00024BE6
		// (set) Token: 0x06000A9D RID: 2717 RVA: 0x000269F6 File Offset: 0x00024BF6
		public bool IsClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(32U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(32U, 0U, value);
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x00026A0D File Offset: 0x00024C0D
		// (set) Token: 0x06000A9F RID: 2719 RVA: 0x00026A1E File Offset: 0x00024C1E
		public bool IsInterface
		{
			get
			{
				return this.attributes.GetMaskedAttributes(32U, 32U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(32U, 32U, value);
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x00026A36 File Offset: 0x00024C36
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x00026A48 File Offset: 0x00024C48
		public bool IsAbstract
		{
			get
			{
				return this.attributes.GetAttributes(128U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(128U, value);
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x00026A61 File Offset: 0x00024C61
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x00026A73 File Offset: 0x00024C73
		public bool IsSealed
		{
			get
			{
				return this.attributes.GetAttributes(256U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(256U, value);
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x00026A8C File Offset: 0x00024C8C
		// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x00026A9E File Offset: 0x00024C9E
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(1024U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024U, value);
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00026AB7 File Offset: 0x00024CB7
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x00026AC9 File Offset: 0x00024CC9
		public bool IsImport
		{
			get
			{
				return this.attributes.GetAttributes(4096U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4096U, value);
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00026AE2 File Offset: 0x00024CE2
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x00026AF4 File Offset: 0x00024CF4
		public bool IsSerializable
		{
			get
			{
				return this.attributes.GetAttributes(8192U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8192U, value);
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00026B0D File Offset: 0x00024D0D
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x00026B20 File Offset: 0x00024D20
		public bool IsAnsiClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(196608U, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(196608U, 0U, value);
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x00026B3A File Offset: 0x00024D3A
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x00026B51 File Offset: 0x00024D51
		public bool IsUnicodeClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(196608U, 65536U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(196608U, 65536U, value);
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00026B6F File Offset: 0x00024D6F
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x00026B86 File Offset: 0x00024D86
		public bool IsAutoClass
		{
			get
			{
				return this.attributes.GetMaskedAttributes(196608U, 131072U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(196608U, 131072U, value);
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00026BA4 File Offset: 0x00024DA4
		// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x00026BB6 File Offset: 0x00024DB6
		public bool IsBeforeFieldInit
		{
			get
			{
				return this.attributes.GetAttributes(1048576U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1048576U, value);
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00026BCF File Offset: 0x00024DCF
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x00026BE1 File Offset: 0x00024DE1
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(2048U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(2048U, value);
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x00026BFA File Offset: 0x00024DFA
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x00026C0C File Offset: 0x00024E0C
		public bool HasSecurity
		{
			get
			{
				return this.attributes.GetAttributes(262144U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(262144U, value);
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00026C25 File Offset: 0x00024E25
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x00026C37 File Offset: 0x00024E37
		public bool IsForwarder
		{
			get
			{
				return this.attributes.GetAttributes(2097152U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(2097152U, value);
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00026C50 File Offset: 0x00024E50
		public string FullName
		{
			get
			{
				string text = (string.IsNullOrEmpty(this.@namespace) ? this.name : (this.@namespace + "." + this.name));
				if (this.declaring_type != null)
				{
					return this.declaring_type.FullName + "/" + text;
				}
				return text;
			}
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00026CA9 File Offset: 0x00024EA9
		public ExportedType(string @namespace, string name, ModuleDefinition module, IMetadataScope scope)
		{
			this.@namespace = @namespace;
			this.name = name;
			this.scope = scope;
			this.module = module;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00026CCE File Offset: 0x00024ECE
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00026CD6 File Offset: 0x00024ED6
		public TypeDefinition Resolve()
		{
			return this.module.Resolve(this.CreateReference());
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00026CE9 File Offset: 0x00024EE9
		internal TypeReference CreateReference()
		{
			return new TypeReference(this.@namespace, this.name, this.module, this.scope)
			{
				DeclaringType = ((this.declaring_type != null) ? this.declaring_type.CreateReference() : null)
			};
		}

		// Token: 0x040002F7 RID: 759
		private string @namespace;

		// Token: 0x040002F8 RID: 760
		private string name;

		// Token: 0x040002F9 RID: 761
		private uint attributes;

		// Token: 0x040002FA RID: 762
		private IMetadataScope scope;

		// Token: 0x040002FB RID: 763
		private ModuleDefinition module;

		// Token: 0x040002FC RID: 764
		private int identifier;

		// Token: 0x040002FD RID: 765
		private ExportedType declaring_type;

		// Token: 0x040002FE RID: 766
		internal MetadataToken token;
	}
}
