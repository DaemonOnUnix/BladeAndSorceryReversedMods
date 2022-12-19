using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000084 RID: 132
	public class ReversePatcher
	{
		// Token: 0x06000299 RID: 665 RVA: 0x0000E32B File Offset: 0x0000C52B
		public ReversePatcher(Harmony instance, MethodBase original, HarmonyMethod standin)
		{
			this.instance = instance;
			this.original = original;
			this.standin = standin;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000E348 File Offset: 0x0000C548
		public MethodInfo Patch(HarmonyReversePatchType type = HarmonyReversePatchType.Original)
		{
			if (this.original == null)
			{
				throw new NullReferenceException("Null method for " + this.instance.Id);
			}
			this.standin.reversePatchType = new HarmonyReversePatchType?(type);
			MethodInfo transpiler = ReversePatcher.GetTranspiler(this.standin.method);
			return PatchFunctions.ReversePatch(this.standin, this.original, transpiler);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000E3AC File Offset: 0x0000C5AC
		internal static MethodInfo GetTranspiler(MethodInfo method)
		{
			string methodName = method.Name;
			IEnumerable<MethodInfo> declaredMethods = AccessTools.GetDeclaredMethods(method.DeclaringType);
			Type ici = typeof(IEnumerable<CodeInstruction>);
			return declaredMethods.FirstOrDefault((MethodInfo m) => !(m.ReturnType != ici) && m.Name.StartsWith("<" + methodName + ">"));
		}

		// Token: 0x040001A0 RID: 416
		private readonly Harmony instance;

		// Token: 0x040001A1 RID: 417
		private readonly MethodBase original;

		// Token: 0x040001A2 RID: 418
		private readonly HarmonyMethod standin;
	}
}
