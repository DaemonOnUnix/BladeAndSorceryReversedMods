using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Mono.Cecil
{
	// Token: 0x020000C3 RID: 195
	public class AssemblyNameReference : IMetadataScope, IMetadataTokenProvider
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00014AFC File Offset: 0x00012CFC
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x00014B04 File Offset: 0x00012D04
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00014B14 File Offset: 0x00012D14
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x00014B1C File Offset: 0x00012D1C
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x00014B2C File Offset: 0x00012D2C
		// (set) Token: 0x0600049B RID: 1179 RVA: 0x00014B34 File Offset: 0x00012D34
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x00014B49 File Offset: 0x00012D49
		// (set) Token: 0x0600049D RID: 1181 RVA: 0x00014B51 File Offset: 0x00012D51
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00014B5A File Offset: 0x00012D5A
		// (set) Token: 0x0600049F RID: 1183 RVA: 0x00014B68 File Offset: 0x00012D68
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x00014B7D File Offset: 0x00012D7D
		// (set) Token: 0x060004A1 RID: 1185 RVA: 0x00014B8B File Offset: 0x00012D8B
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00014BA0 File Offset: 0x00012DA0
		// (set) Token: 0x060004A3 RID: 1187 RVA: 0x00014BB2 File Offset: 0x00012DB2
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x00014BCB File Offset: 0x00012DCB
		// (set) Token: 0x060004A5 RID: 1189 RVA: 0x00014BDD File Offset: 0x00012DDD
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00014BF6 File Offset: 0x00012DF6
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x00014C07 File Offset: 0x00012E07
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

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00014C34 File Offset: 0x00012E34
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x00014C97 File Offset: 0x00012E97
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

		// Token: 0x060004AA RID: 1194 RVA: 0x00014CA8 File Offset: 0x00012EA8
		private byte[] HashPublicKey()
		{
			HashAlgorithm hashAlgorithm;
			if (this.hash_algorithm == AssemblyHashAlgorithm.Reserved)
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.AssemblyNameReference;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00014CFC File Offset: 0x00012EFC
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

		// Token: 0x060004AD RID: 1197 RVA: 0x00014E28 File Offset: 0x00013028
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
					if (text2 != null)
					{
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
			}
			return assemblyNameReference;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00014F96 File Offset: 0x00013196
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x00014F9E File Offset: 0x0001319E
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00014FA7 File Offset: 0x000131A7
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x00014FAF File Offset: 0x000131AF
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00014FB8 File Offset: 0x000131B8
		// (set) Token: 0x060004B3 RID: 1203 RVA: 0x00014FC0 File Offset: 0x000131C0
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

		// Token: 0x060004B4 RID: 1204 RVA: 0x00014FC9 File Offset: 0x000131C9
		internal AssemblyNameReference()
		{
			this.version = Mixin.ZeroVersion;
			this.token = new MetadataToken(TokenType.AssemblyRef);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00014FEC File Offset: 0x000131EC
		public AssemblyNameReference(string name, Version version)
		{
			Mixin.CheckName(name);
			this.name = name;
			this.version = Mixin.CheckVersion(version);
			this.hash_algorithm = AssemblyHashAlgorithm.None;
			this.token = new MetadataToken(TokenType.AssemblyRef);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00015024 File Offset: 0x00013224
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x04000246 RID: 582
		private string name;

		// Token: 0x04000247 RID: 583
		private string culture;

		// Token: 0x04000248 RID: 584
		private Version version;

		// Token: 0x04000249 RID: 585
		private uint attributes;

		// Token: 0x0400024A RID: 586
		private byte[] public_key;

		// Token: 0x0400024B RID: 587
		private byte[] public_key_token;

		// Token: 0x0400024C RID: 588
		private AssemblyHashAlgorithm hash_algorithm;

		// Token: 0x0400024D RID: 589
		private byte[] hash;

		// Token: 0x0400024E RID: 590
		internal MetadataToken token;

		// Token: 0x0400024F RID: 591
		private string full_name;
	}
}
