using System;

namespace Mono.Cecil
{
	// Token: 0x0200017E RID: 382
	public enum MetadataType : byte
	{
		// Token: 0x04000517 RID: 1303
		Void = 1,
		// Token: 0x04000518 RID: 1304
		Boolean,
		// Token: 0x04000519 RID: 1305
		Char,
		// Token: 0x0400051A RID: 1306
		SByte,
		// Token: 0x0400051B RID: 1307
		Byte,
		// Token: 0x0400051C RID: 1308
		Int16,
		// Token: 0x0400051D RID: 1309
		UInt16,
		// Token: 0x0400051E RID: 1310
		Int32,
		// Token: 0x0400051F RID: 1311
		UInt32,
		// Token: 0x04000520 RID: 1312
		Int64,
		// Token: 0x04000521 RID: 1313
		UInt64,
		// Token: 0x04000522 RID: 1314
		Single,
		// Token: 0x04000523 RID: 1315
		Double,
		// Token: 0x04000524 RID: 1316
		String,
		// Token: 0x04000525 RID: 1317
		Pointer,
		// Token: 0x04000526 RID: 1318
		ByReference,
		// Token: 0x04000527 RID: 1319
		ValueType,
		// Token: 0x04000528 RID: 1320
		Class,
		// Token: 0x04000529 RID: 1321
		Var,
		// Token: 0x0400052A RID: 1322
		Array,
		// Token: 0x0400052B RID: 1323
		GenericInstance,
		// Token: 0x0400052C RID: 1324
		TypedByReference,
		// Token: 0x0400052D RID: 1325
		IntPtr = 24,
		// Token: 0x0400052E RID: 1326
		UIntPtr,
		// Token: 0x0400052F RID: 1327
		FunctionPointer = 27,
		// Token: 0x04000530 RID: 1328
		Object,
		// Token: 0x04000531 RID: 1329
		MVar = 30,
		// Token: 0x04000532 RID: 1330
		RequiredModifier,
		// Token: 0x04000533 RID: 1331
		OptionalModifier,
		// Token: 0x04000534 RID: 1332
		Sentinel = 65,
		// Token: 0x04000535 RID: 1333
		Pinned = 69
	}
}
