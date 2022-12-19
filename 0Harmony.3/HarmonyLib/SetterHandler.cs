using System;

namespace HarmonyLib
{
	// Token: 0x02000004 RID: 4
	// (Invoke) Token: 0x06000008 RID: 8
	[Obsolete("Use AccessTools.FieldRefAccess<T, S> for fields and AccessTools.MethodDelegate<Action<T, S>> for property setters")]
	public delegate void SetterHandler<in T, in S>(T source, S value);
}
