using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Mono.Cecil
{
	// Token: 0x020001B5 RID: 437
	public class AssemblyNameReference : IMetadataScope, IMetadataTokenProvider
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x0001A988 File Offset: 0x00018B88
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x0001A990 File Offset: 0x00018B90
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.full_name = null;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0001A9A0 File Offset: 0x00018BA0
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x0001A9A8 File Offset: 0x00018BA8
		public string Culture
		{
			get
			{
				return this.culture;
			}
			set
			{
				this.culture = value;
				this.full_name = null;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0001A9B8 File Offset: 0x00018BB8
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x0001A9C0 File Offset: 0x00018BC0
		public Version Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = Mixin.CheckVersion(value);
				this.full_name = null;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0001A9D5 File Offset: 0x00018BD5
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x0001A9DD File Offset: 0x00018BDD
		public AssemblyAttributes Attributes
		{
			get
			{
				return (AssemblyAttributes)this.attributes;
			}
			set
			{
				this.attributes = (uint)value;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0001A9E6 File Offset: 0x00018BE6
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x0001A9F4 File Offset: 0x00018BF4
		public bool HasPublicKey
		{
			get
			{
				return this.attributes.GetAttributes(1U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1U, value);
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0001AA09 File Offset: 0x00018C09
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x0001AA17 File Offset: 0x00018C17
		public bool IsSideBySideCompatible
		{
			get
			{
				return this.attributes.GetAttributes(0U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(0U, value);
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x0001AA2C File Offset: 0x00018C2C
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x0001AA3E File Offset: 0x00018C3E
		public bool IsRetargetable
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

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0001AA57 File Offset: 0x00018C57
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x0001AA69 File Offset: 0x00018C69
		public bool IsWindowsRuntime
		{
			get
			{
				return this.attributes.GetAttributes(512U);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(512U, value);
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0001AA82 File Offset: 0x00018C82
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x0001AA93 File Offset: 0x00018C93
		public byte[] PublicKey
		{
			get
			{
				return this.public_key ?? Empty<byte>.Array;
			}
			set
			{
				this.public_key = value;
				this.HasPublicKey = !this.public_key.IsNullOrEmpty<byte>();
				this.public_key_token = null;
				this.full_name = null;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x0001AAC0 File Offset: 0x00018CC0
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x0001AB23 File Offset: 0x00018D23
		public byte[] PublicKeyToken
		{
			get
			{
				if (this.public_key_token == null && !this.public_key.IsNullOrEmpty<byte>())
				{
					byte[] array = this.HashPublicKey();
					byte[] array2 = new byte[8];
					Array.Copy(array, array.Length - 8, array2, 0, 8);
					Array.Reverse(array2, 0, 8);
					Interlocked.CompareExchange<byte[]>(ref this.public_key_token, array2, null);
				}
				return this.public_key_token ?? Empty<byte>.Array;
			}
			set
			{
				this.public_key_token = value;
				this.full_name = null;
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001AB34 File Offset: 0x00018D34
		private byte[] HashPublicKey()
		{
			HashAlgorithm hashAlgorithm;
			if (this.hash_algorithm == AssemblyHashAlgorithm.MD5)
			{
				hashAlgorithm = MD5.Create();
			}
			else
			{
				hashAlgorithm = SHA1.Create();
			}
			byte[] array;
			using (hashAlgorithm)
			{
				array = hashAlgorithm.ComputeHash(this.public_key);
			}
			return array;
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public virtual MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.AssemblyNameReference;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0001AB88 File Offset: 0x00018D88
		public string FullName
		{
			get
			{
				if (this.full_name != null)
				{
					return this.full_name;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.name);
				stringBuilder.Append(", ");
				stringBuilder.Append("Version=");
				stringBuilder.Append(this.version.ToString(4));
				stringBuilder.Append(", ");
				stringBuilder.Append("Culture=");
				stringBuilder.Append(string.IsNullOrEmpty(this.culture) ? "neutral" : this.culture);
				stringBuilder.Append(", ");
				stringBuilder.Append("PublicKeyToken=");
				byte[] publicKeyToken = this.PublicKeyToken;
				if (!publicKeyToken.IsNullOrEmpty<byte>() && publicKeyToken.Length != 0)
				{
					for (int i = 0; i < publicKeyToken.Length; i++)
					{
						stringBuilder.Append(publicKeyToken[i].ToString("x2"));
					}
				}
				else
				{
					stringBuilder.Append("null");
				}
				if (this.IsRetargetable)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append("Retargetable=Yes");
				}
				Interlocked.CompareExchange<string>(ref this.full_name, stringBuilder.ToString(), null);
				return this.full_name;
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001ACB4 File Offset: 0x00018EB4
		public static AssemblyNameReference Parse(string fullName)
		{
			if (fullName == null)
			{
				throw new ArgumentNullException("fullName");
			}
			if (fullName.Length == 0)
			{
				throw new ArgumentException("Name can not be empty");
			}
			AssemblyNameReference assemblyNameReference = new AssemblyNameReference();
			string[] array = fullName.Split(new char[] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (i == 0)
				{
					assemblyNameReference.Name = text;
				}
				else
				{
					string[] array2 = text.Split(new char[] { '=' });
					if (array2.Length != 2)
					{
						throw new ArgumentException("Malformed name");
					}
					string text2 = array2[0].ToLowerInvariant();
					if (!(text2 == "version"))
					{
						if (!(text2 == "culture"))
						{
							if (text2 == "publickeytoken")
							{
								string text3 = array2[1];
								if (!(text3 == "null"))
								{
									assemblyNameReference.PublicKeyToken = new byte[text3.Length / 2];
									for (int j = 0; j < assemblyNameReference.PublicKeyToken.Length; j++)
									{
										assemblyNameReference.PublicKeyToken[j] = byte.Parse(text3.Substring(j * 2, 2), NumberStyles.HexNumber);
									}
								}
							}
						}
						else
						{
							assemblyNameReference.Culture = ((array2[1] == "neutral") ? "" : array2[1]);
						}
					}
					else
					{
						assemblyNameReference.Version = new Version(array2[1]);
					}
				}
			}
			return assemblyNameReference;
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0001AE1B File Offset: 0x0001901B
		// (set) Token: 0x060007E5 RID: 2021 RVA: 0x0001AE23 File Offset: 0x00019023
		public AssemblyHashAlgorithm HashAlgorithm
		{
			get
			{
				return this.hash_algorithm;
			}
			set
			{
				this.hash_algorithm = value;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0001AE2C File Offset: 0x0001902C
		// (set) Token: 0x060007E7 RID: 2023 RVA: 0x0001AE34 File Offset: 0x00019034
		public virtual byte[] Hash
		{
			get
			{
				return this.hash;
			}
			set
			{
				this.hash = value;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0001AE3D File Offset: 0x0001903D
		// (set) Token: 0x060007E9 RID: 2025 RVA: 0x0001AE45 File Offset: 0x00019045
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

		// Token: 0x060007EA RID: 2026 RVA: 0x0001AE4E File Offset: 0x0001904E
		internal AssemblyNameReference()
		{
			this.version = Mixin.ZeroVersion;
			this.token = new MetadataToken(TokenType.AssemblyRef);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001AE71 File Offset: 0x00019071
		public AssemblyNameReference(string name, Version version)
		{
			Mixin.CheckName(name);
			this.name = name;
			this.version = Mixin.CheckVersion(version);
			this.hash_algorithm = AssemblyHashAlgorithm.None;
			this.token = new MetadataToken(TokenType.AssemblyRef);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001AEA9 File Offset: 0x000190A9
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x04000278 RID: 632
		private string name;

		// Token: 0x04000279 RID: 633
		private string culture;

		// Token: 0x0400027A RID: 634
		private Version version;

		// Token: 0x0400027B RID: 635
		private uint attributes;

		// Token: 0x0400027C RID: 636
		private byte[] public_key;

		// Token: 0x0400027D RID: 637
		private byte[] public_key_token;

		// Token: 0x0400027E RID: 638
		private AssemblyHashAlgorithm hash_algorithm;

		// Token: 0x0400027F RID: 639
		private byte[] hash;

		// Token: 0x04000280 RID: 640
		internal MetadataToken token;

		// Token: 0x04000281 RID: 641
		private string full_name;
	}
}
