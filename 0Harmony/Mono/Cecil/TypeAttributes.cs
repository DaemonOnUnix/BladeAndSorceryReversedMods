using System;

namespace Mono.Cecil
{
	// Token: 0x02000269 RID: 617
	[Flags]
	public enum TypeAttributes : uint
	{
		// Token: 0x040004FD RID: 1277
		VisibilityMask = 7U,
		// Token: 0x040004FE RID: 1278
		NotPublic = 0U,
		// Token: 0x040004FF RID: 1279
		Public = 1U,
		// Token: 0x04000500 RID: 1280
		NestedPublic = 2U,
		// Token: 0x04000501 RID: 1281
		NestedPrivate = 3U,
		// Token: 0x04000502 RID: 1282
		NestedFamily = 4U,
		// Token: 0x04000503 RID: 1283
		NestedAssembly = 5U,
		// Token: 0x04000504 RID: 1284
		NestedFamANDAssem = 6U,
		// Token: 0x04000505 RID: 1285
		NestedFamORAssem = 7U,
		// Token: 0x04000506 RID: 1286
		LayoutMask = 24U,
		// Token: 0x04000507 RID: 1287
		AutoLayout = 0U,
		// Token: 0x04000508 RID: 1288
		SequentialLayout = 8U,
		// Token: 0x04000509 RID: 1289
		ExplicitLayout = 16U,
		// Token: 0x0400050A RID: 1290
		ClassSemanticMask = 32U,
		// Token: 0x0400050B RID: 1291
		Class = 0U,
		// Token: 0x0400050C RID: 1292
		Interface = 32U,
		// Token: 0x0400050D RID: 1293
		Abstract = 128U,
		// Token: 0x0400050E RID: 1294
		Sealed = 256U,
		// Token: 0x0400050F RID: 1295
		SpecialName = 1024U,
		// Token: 0x04000510 RID: 1296
		Import = 4096U,
		// Token: 0x04000511 RID: 1297
		Serializable = 8192U,
		// Token: 0x04000512 RID: 1298
		WindowsRuntime = 16384U,
		// Token: 0x04000513 RID: 1299
		StringFormatMask = 196608U,
		// Token: 0x04000514 RID: 1300
		AnsiClass = 0U,
		// Token: 0x04000515 RID: 1301
		UnicodeClass = 65536U,
		// Token: 0x04000516 RID: 1302
		AutoClass = 131072U,
		// Token: 0x04000517 RID: 1303
		BeforeFieldInit = 1048576U,
		// Token: 0x04000518 RID: 1304
		RTSpecialName = 2048U,
		// Token: 0x04000519 RID: 1305
		HasSecurity = 262144U,
		// Token: 0x0400051A RID: 1306
		Forwarder = 2097152U
	}
}
