using System;

namespace Mono.Cecil
{
	// Token: 0x02000176 RID: 374
	[Flags]
	public enum TypeAttributes : uint
	{
		// Token: 0x040004CB RID: 1227
		VisibilityMask = 7U,
		// Token: 0x040004CC RID: 1228
		NotPublic = 0U,
		// Token: 0x040004CD RID: 1229
		Public = 1U,
		// Token: 0x040004CE RID: 1230
		NestedPublic = 2U,
		// Token: 0x040004CF RID: 1231
		NestedPrivate = 3U,
		// Token: 0x040004D0 RID: 1232
		NestedFamily = 4U,
		// Token: 0x040004D1 RID: 1233
		NestedAssembly = 5U,
		// Token: 0x040004D2 RID: 1234
		NestedFamANDAssem = 6U,
		// Token: 0x040004D3 RID: 1235
		NestedFamORAssem = 7U,
		// Token: 0x040004D4 RID: 1236
		LayoutMask = 24U,
		// Token: 0x040004D5 RID: 1237
		AutoLayout = 0U,
		// Token: 0x040004D6 RID: 1238
		SequentialLayout = 8U,
		// Token: 0x040004D7 RID: 1239
		ExplicitLayout = 16U,
		// Token: 0x040004D8 RID: 1240
		ClassSemanticMask = 32U,
		// Token: 0x040004D9 RID: 1241
		Class = 0U,
		// Token: 0x040004DA RID: 1242
		Interface = 32U,
		// Token: 0x040004DB RID: 1243
		Abstract = 128U,
		// Token: 0x040004DC RID: 1244
		Sealed = 256U,
		// Token: 0x040004DD RID: 1245
		SpecialName = 1024U,
		// Token: 0x040004DE RID: 1246
		Import = 4096U,
		// Token: 0x040004DF RID: 1247
		Serializable = 8192U,
		// Token: 0x040004E0 RID: 1248
		WindowsRuntime = 16384U,
		// Token: 0x040004E1 RID: 1249
		StringFormatMask = 196608U,
		// Token: 0x040004E2 RID: 1250
		AnsiClass = 0U,
		// Token: 0x040004E3 RID: 1251
		UnicodeClass = 65536U,
		// Token: 0x040004E4 RID: 1252
		AutoClass = 131072U,
		// Token: 0x040004E5 RID: 1253
		BeforeFieldInit = 1048576U,
		// Token: 0x040004E6 RID: 1254
		RTSpecialName = 2048U,
		// Token: 0x040004E7 RID: 1255
		HasSecurity = 262144U,
		// Token: 0x040004E8 RID: 1256
		Forwarder = 2097152U
	}
}
