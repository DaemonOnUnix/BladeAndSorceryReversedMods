using System;

namespace HarmonyLib
{
	// Token: 0x02000003 RID: 3
	// (Invoke) Token: 0x06000004 RID: 4
	[Obsolete("Use AccessTools.FieldRefAccess<T, S> for fields and AccessTools.MethodDelegate<Func<T, S>> for property getters")]
	public delegate S GetterHandler<in T, out S>(T source);
}
