using System;
using System.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D7 RID: 471
	internal static class PdbGuidMapping
	{
		// Token: 0x06000EE7 RID: 3815 RVA: 0x00033F38 File Offset: 0x00032138
		static PdbGuidMapping()
		{
			PdbGuidMapping.AddMapping(DocumentLanguage.C, new Guid("63a08714-fc37-11d2-904c-00c04fa302a1"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Cpp, new Guid("3a12d0b7-c26c-11d0-b442-00a0244a1dd2"));
			PdbGuidMapping.AddMapping(DocumentLanguage.CSharp, new Guid("3f5162f8-07c6-11d3-9053-00c04fa302a1"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Basic, new Guid("3a12d0b8-c26c-11d0-b442-00a0244a1dd2"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Java, new Guid("3a12d0b4-c26c-11d0-b442-00a0244a1dd2"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Cobol, new Guid("af046cd1-d0e1-11d2-977c-00a0c9b4d50c"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Pascal, new Guid("af046cd2-d0e1-11d2-977c-00a0c9b4d50c"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Cil, new Guid("af046cd3-d0e1-11d2-977c-00a0c9b4d50c"));
			PdbGuidMapping.AddMapping(DocumentLanguage.JScript, new Guid("3a12d0b6-c26c-11d0-b442-00a0244a1dd2"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Smc, new Guid("0d9b9f7b-6611-11d3-bd2a-0000f80849bd"));
			PdbGuidMapping.AddMapping(DocumentLanguage.MCpp, new Guid("4b35fde8-07c6-11d3-9053-00c04fa302a1"));
			PdbGuidMapping.AddMapping(DocumentLanguage.FSharp, new Guid("ab4f38c9-b6e6-43ba-be3b-58080b2ccce3"));
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00034068 File Offset: 0x00032268
		private static void AddMapping(DocumentLanguage language, Guid guid)
		{
			PdbGuidMapping.guid_language.Add(guid, language);
			PdbGuidMapping.language_guid.Add(language, guid);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00034082 File Offset: 0x00032282
		public static DocumentType ToType(this Guid guid)
		{
			if (guid == PdbGuidMapping.type_text)
			{
				return DocumentType.Text;
			}
			return DocumentType.Other;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00034094 File Offset: 0x00032294
		public static Guid ToGuid(this DocumentType type)
		{
			if (type == DocumentType.Text)
			{
				return PdbGuidMapping.type_text;
			}
			return default(Guid);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x000340B4 File Offset: 0x000322B4
		public static DocumentHashAlgorithm ToHashAlgorithm(this Guid guid)
		{
			if (guid == PdbGuidMapping.hash_md5)
			{
				return DocumentHashAlgorithm.MD5;
			}
			if (guid == PdbGuidMapping.hash_sha1)
			{
				return DocumentHashAlgorithm.SHA1;
			}
			if (guid == PdbGuidMapping.hash_sha256)
			{
				return DocumentHashAlgorithm.SHA256;
			}
			return DocumentHashAlgorithm.None;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x000340E4 File Offset: 0x000322E4
		public static Guid ToGuid(this DocumentHashAlgorithm hash_algo)
		{
			if (hash_algo == DocumentHashAlgorithm.MD5)
			{
				return PdbGuidMapping.hash_md5;
			}
			if (hash_algo == DocumentHashAlgorithm.SHA1)
			{
				return PdbGuidMapping.hash_sha1;
			}
			if (hash_algo == DocumentHashAlgorithm.SHA256)
			{
				return PdbGuidMapping.hash_sha256;
			}
			return default(Guid);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00034118 File Offset: 0x00032318
		public static DocumentLanguage ToLanguage(this Guid guid)
		{
			DocumentLanguage documentLanguage;
			if (!PdbGuidMapping.guid_language.TryGetValue(guid, out documentLanguage))
			{
				return DocumentLanguage.Other;
			}
			return documentLanguage;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00034138 File Offset: 0x00032338
		public static Guid ToGuid(this DocumentLanguage language)
		{
			Guid guid;
			if (!PdbGuidMapping.language_guid.TryGetValue(language, out guid))
			{
				return default(Guid);
			}
			return guid;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0003415F File Offset: 0x0003235F
		public static DocumentLanguageVendor ToVendor(this Guid guid)
		{
			if (guid == PdbGuidMapping.vendor_ms)
			{
				return DocumentLanguageVendor.Microsoft;
			}
			return DocumentLanguageVendor.Other;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00034174 File Offset: 0x00032374
		public static Guid ToGuid(this DocumentLanguageVendor vendor)
		{
			if (vendor == DocumentLanguageVendor.Microsoft)
			{
				return PdbGuidMapping.vendor_ms;
			}
			return default(Guid);
		}

		// Token: 0x040008F7 RID: 2295
		private static readonly Dictionary<Guid, DocumentLanguage> guid_language = new Dictionary<Guid, DocumentLanguage>();

		// Token: 0x040008F8 RID: 2296
		private static readonly Dictionary<DocumentLanguage, Guid> language_guid = new Dictionary<DocumentLanguage, Guid>();

		// Token: 0x040008F9 RID: 2297
		private static readonly Guid type_text = new Guid("5a869d0b-6611-11d3-bd2a-0000f80849bd");

		// Token: 0x040008FA RID: 2298
		private static readonly Guid hash_md5 = new Guid("406ea660-64cf-4c82-b6f0-42d48172a799");

		// Token: 0x040008FB RID: 2299
		private static readonly Guid hash_sha1 = new Guid("ff1816ec-aa5e-4d10-87f7-6f4963833460");

		// Token: 0x040008FC RID: 2300
		private static readonly Guid hash_sha256 = new Guid("8829d00f-11b8-4213-878b-770e8597ac16");

		// Token: 0x040008FD RID: 2301
		private static readonly Guid vendor_ms = new Guid("994b45c4-e6e9-11d2-903f-00c04fa302a1");
	}
}
