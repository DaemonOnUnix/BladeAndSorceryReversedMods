using System;

namespace Mono.Cecil
{
	// Token: 0x0200018F RID: 399
	public enum TokenType : uint
	{
		// Token: 0x0400058C RID: 1420
		Module,
		// Token: 0x0400058D RID: 1421
		TypeRef = 16777216U,
		// Token: 0x0400058E RID: 1422
		TypeDef = 33554432U,
		// Token: 0x0400058F RID: 1423
		Field = 67108864U,
		// Token: 0x04000590 RID: 1424
		Method = 100663296U,
		// Token: 0x04000591 RID: 1425
		Param = 134217728U,
		// Token: 0x04000592 RID: 1426
		InterfaceImpl = 150994944U,
		// Token: 0x04000593 RID: 1427
		MemberRef = 167772160U,
		// Token: 0x04000594 RID: 1428
		CustomAttribute = 201326592U,
		// Token: 0x04000595 RID: 1429
		Permission = 234881024U,
		// Token: 0x04000596 RID: 1430
		Signature = 285212672U,
		// Token: 0x04000597 RID: 1431
		Event = 335544320U,
		// Token: 0x04000598 RID: 1432
		Property = 385875968U,
		// Token: 0x04000599 RID: 1433
		ModuleRef = 436207616U,
		// Token: 0x0400059A RID: 1434
		TypeSpec = 452984832U,
		// Token: 0x0400059B RID: 1435
		Assembly = 536870912U,
		// Token: 0x0400059C RID: 1436
		AssemblyRef = 587202560U,
		// Token: 0x0400059D RID: 1437
		File = 637534208U,
		// Token: 0x0400059E RID: 1438
		ExportedType = 654311424U,
		// Token: 0x0400059F RID: 1439
		ManifestResource = 671088640U,
		// Token: 0x040005A0 RID: 1440
		GenericParam = 704643072U,
		// Token: 0x040005A1 RID: 1441
		MethodSpec = 721420288U,
		// Token: 0x040005A2 RID: 1442
		GenericParamConstraint = 738197504U,
		// Token: 0x040005A3 RID: 1443
		Document = 805306368U,
		// Token: 0x040005A4 RID: 1444
		MethodDebugInformation = 822083584U,
		// Token: 0x040005A5 RID: 1445
		LocalScope = 838860800U,
		// Token: 0x040005A6 RID: 1446
		LocalVariable = 855638016U,
		// Token: 0x040005A7 RID: 1447
		LocalConstant = 872415232U,
		// Token: 0x040005A8 RID: 1448
		ImportScope = 889192448U,
		// Token: 0x040005A9 RID: 1449
		StateMachineMethod = 905969664U,
		// Token: 0x040005AA RID: 1450
		CustomDebugInformation = 922746880U,
		// Token: 0x040005AB RID: 1451
		String = 1879048192U
	}
}
