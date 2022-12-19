using System;

namespace Mono.Cecil
{
	// Token: 0x02000272 RID: 626
	public enum MetadataType : byte
	{
		// Token: 0x0400054D RID: 1357
		Void = 1,
		// Token: 0x0400054E RID: 1358
		Boolean,
		// Token: 0x0400054F RID: 1359
		Char,
		// Token: 0x04000550 RID: 1360
		SByte,
		// Token: 0x04000551 RID: 1361
		Byte,
		// Token: 0x04000552 RID: 1362
		Int16,
		// Token: 0x04000553 RID: 1363
		UInt16,
		// Token: 0x04000554 RID: 1364
		Int32,
		// Token: 0x04000555 RID: 1365
		UInt32,
		// Token: 0x04000556 RID: 1366
		Int64,
		// Token: 0x04000557 RID: 1367
		UInt64,
		// Token: 0x04000558 RID: 1368
		Single,
		// Token: 0x04000559 RID: 1369
		Double,
		// Token: 0x0400055A RID: 1370
		String,
		// Token: 0x0400055B RID: 1371
		Pointer,
		// Token: 0x0400055C RID: 1372
		ByReference,
		// Token: 0x0400055D RID: 1373
		ValueType,
		// Token: 0x0400055E RID: 1374
		Class,
		// Token: 0x0400055F RID: 1375
		Var,
		// Token: 0x04000560 RID: 1376
		Array,
		// Token: 0x04000561 RID: 1377
		GenericInstance,
		// Token: 0x04000562 RID: 1378
		TypedByReference,
		// Token: 0x04000563 RID: 1379
		IntPtr = 24,
		// Token: 0x04000564 RID: 1380
		UIntPtr,
		// Token: 0x04000565 RID: 1381
		FunctionPointer = 27,
		// Token: 0x04000566 RID: 1382
		Object,
		// Token: 0x04000567 RID: 1383
		MVar = 30,
		// Token: 0x04000568 RID: 1384
		RequiredModifier,
		// Token: 0x04000569 RID: 1385
		OptionalModifier,
		// Token: 0x0400056A RID: 1386
		Sentinel = 65,
		// Token: 0x0400056B RID: 1387
		Pinned = 69
	}
}
