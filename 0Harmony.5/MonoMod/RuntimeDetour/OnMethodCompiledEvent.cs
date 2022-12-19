using System;
using System.Reflection;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x02000467 RID: 1127
	// (Invoke) Token: 0x0600185C RID: 6236
	public delegate void OnMethodCompiledEvent(MethodBase method, IntPtr codeStart, ulong codeSize);
}
