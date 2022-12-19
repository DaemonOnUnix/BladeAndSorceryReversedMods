using System;

namespace Mono.Cecil
{
	// Token: 0x0200010D RID: 269
	public sealed class ExportedType : IMetadataTokenProvider
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x000208FF File Offset: 0x0001EAFF
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x00020907 File Offset: 0x0001EB07
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

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x00020910 File Offset: 0x0001EB10
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x00020918 File Offset: 0x0001EB18
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

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x00020921 File Offset: 0x0001EB21
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x00020929 File Offset: 0x0001EB29
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

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x00020932 File Offset: 0x0001EB32
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x0002094E File Offset: 0x0001EB4E
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

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0002096C File Offset: 0x0001EB6C
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x00020974 File Offset: 0x0001EB74
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

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x0002097D File Offset: 0x0001EB7D
		// (set) Token: 0x0600074B RID: 1867 RVA: 0x00020985 File Offset: 0x0001EB85
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

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0002098E File Offset: 0x0001EB8E
		// (set) Token: 0x0600074D RID: 1869 RVA: 0x00020996 File Offset: 0x0001EB96
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x0002099F File Offset: 0x0001EB9F
		// (set) Token: 0x0600074F RID: 1871 RVA: 0x000209AE File Offset: 0x0001EBAE
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

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x000209C4 File Offset: 0x0001EBC4
		// (set) Token: 0x06000751 RID: 1873 RVA: 0x000209D3 File Offset: 0x0001EBD3
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

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x000209E9 File Offset: 0x0001EBE9
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x000209F8 File Offset: 0x0001EBF8
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

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x00020A0E File Offset: 0x0001EC0E
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x00020A1D File Offset: 0x0001EC1D
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

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00020A33 File Offset: 0x0001EC33
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x00020A42 File Offset: 0x0001EC42
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

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x00020A58 File Offset: 0x0001EC58
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x00020A67 File Offset: 0x0001EC67
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

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x00020A7D File Offset: 0x0001EC7D
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x00020A8C File Offset: 0x0001EC8C
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

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x00020AA2 File Offset: 0x0001ECA2
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x00020AB1 File Offset: 0x0001ECB1
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

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x00020AC7 File Offset: 0x0001ECC7
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x00020AD7 File Offset: 0x0001ECD7
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

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00020AEE File Offset: 0x0001ECEE
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x00020AFE File Offset: 0x0001ECFE
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

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00020B15 File Offset: 0x0001ED15
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x00020B26 File Offset: 0x0001ED26
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

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x00020B3E File Offset: 0x0001ED3E
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x00020B4E File Offset: 0x0001ED4E
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

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00020B65 File Offset: 0x0001ED65
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x00020B76 File Offset: 0x0001ED76
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00020B8E File Offset: 0x0001ED8E
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x00020BA0 File Offset: 0x0001EDA0
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00020BB9 File Offset: 0x0001EDB9
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x00020BCB File Offset: 0x0001EDCB
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00020BE4 File Offset: 0x0001EDE4
		// (set) Token: 0x0600076D RID: 1901 RVA: 0x00020BF6 File Offset: 0x0001EDF6
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

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x00020C0F File Offset: 0x0001EE0F
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x00020C21 File Offset: 0x0001EE21
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

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x00020C3A File Offset: 0x0001EE3A
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x00020C4C File Offset: 0x0001EE4C
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

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x00020C65 File Offset: 0x0001EE65
		// (set) Token: 0x06000773 RID: 1907 RVA: 0x00020C78 File Offset: 0x0001EE78
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

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x00020C92 File Offset: 0x0001EE92
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x00020CA9 File Offset: 0x0001EEA9
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

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x00020CC7 File Offset: 0x0001EEC7
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x00020CDE File Offset: 0x0001EEDE
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00020CFC File Offset: 0x0001EEFC
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x00020D0E File Offset: 0x0001EF0E
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

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00020D27 File Offset: 0x0001EF27
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x00020D39 File Offset: 0x0001EF39
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

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x00020D52 File Offset: 0x0001EF52
		// (set) Token: 0x0600077D RID: 1917 RVA: 0x00020D64 File Offset: 0x0001EF64
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

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x00020D7D File Offset: 0x0001EF7D
		// (set) Token: 0x0600077F RID: 1919 RVA: 0x00020D8F File Offset: 0x0001EF8F
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00020DA8 File Offset: 0x0001EFA8
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

		// Token: 0x06000781 RID: 1921 RVA: 0x00020E01 File Offset: 0x0001F001
		public ExportedType(string @namespace, string name, ModuleDefinition module, IMetadataScope scope)
		{
			this.@namespace = @namespace;
			this.name = name;
			this.scope = scope;
			this.module = module;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00020E26 File Offset: 0x0001F026
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00020E2E File Offset: 0x0001F02E
		public TypeDefinition Resolve()
		{
			return this.module.Resolve(this.CreateReference());
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00020E41 File Offset: 0x0001F041
		internal TypeReference CreateReference()
		{
			return new TypeReference(this.@namespace, this.name, this.module, this.scope)
			{
				DeclaringType = ((this.declaring_type != null) ? this.declaring_type.CreateReference() : null)
			};
		}

		// Token: 0x040002C5 RID: 709
		private string @namespace;

		// Token: 0x040002C6 RID: 710
		private string name;

		// Token: 0x040002C7 RID: 711
		private uint attributes;

		// Token: 0x040002C8 RID: 712
		private IMetadataScope scope;

		// Token: 0x040002C9 RID: 713
		private ModuleDefinition module;

		// Token: 0x040002CA RID: 714
		private int identifier;

		// Token: 0x040002CB RID: 715
		private ExportedType declaring_type;

		// Token: 0x040002CC RID: 716
		internal MetadataToken token;
	}
}
