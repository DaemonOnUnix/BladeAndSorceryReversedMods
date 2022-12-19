using System;

namespace Mono.Cecil
{
	// Token: 0x02000284 RID: 644
	public enum TokenType : uint
	{
		// Token: 0x040005C3 RID: 1475
		Module,
		// Token: 0x040005C4 RID: 1476
		TypeRef = 16777216U,
		// Token: 0x040005C5 RID: 1477
		TypeDef = 33554432U,
		// Token: 0x040005C6 RID: 1478
		Field = 67108864U,
		// Token: 0x040005C7 RID: 1479
		Method = 100663296U,
		// Token: 0x040005C8 RID: 1480
		Param = 134217728U,
		// Token: 0x040005C9 RID: 1481
		InterfaceImpl = 150994944U,
		// Token: 0x040005CA RID: 1482
		MemberRef = 167772160U,
		// Token: 0x040005CB RID: 1483
		CustomAttribute = 201326592U,
		// Token: 0x040005CC RID: 1484
		Permission = 234881024U,
		// Token: 0x040005CD RID: 1485
		Signature = 285212672U,
		// Token: 0x040005CE RID: 1486
		Event = 335544320U,
		// Token: 0x040005CF RID: 1487
		Property = 385875968U,
		// Token: 0x040005D0 RID: 1488
		ModuleRef = 436207616U,
		// Token: 0x040005D1 RID: 1489
		TypeSpec = 452984832U,
		// Token: 0x040005D2 RID: 1490
		Assembly = 536870912U,
		// Token: 0x040005D3 RID: 1491
		AssemblyRef = 587202560U,
		// Token: 0x040005D4 RID: 1492
		File = 637534208U,
		// Token: 0x040005D5 RID: 1493
		ExportedType = 654311424U,
		// Token: 0x040005D6 RID: 1494
		ManifestResource = 671088640U,
		// Token: 0x040005D7 RID: 1495
		GenericParam = 704643072U,
		// Token: 0x040005D8 RID: 1496
		MethodSpec = 721420288U,
		// Token: 0x040005D9 RID: 1497
		GenericParamConstraint = 738197504U,
		// Token: 0x040005DA RID: 1498
		Document = 805306368U,
		// Token: 0x040005DB RID: 1499
		MethodDebugInformation = 822083584U,
		// Token: 0x040005DC RID: 1500
		LocalScope = 838860800U,
		// Token: 0x040005DD RID: 1501
		LocalVariable = 855638016U,
		// Token: 0x040005DE RID: 1502
		LocalConstant = 872415232U,
		// Token: 0x040005DF RID: 1503
		ImportScope = 889192448U,
		// Token: 0x040005E0 RID: 1504
		StateMachineMethod = 905969664U,
		// Token: 0x040005E1 RID: 1505
		CustomDebugInformation = 922746880U,
		// Token: 0x040005E2 RID: 1506
		String = 1879048192U
	}
}
