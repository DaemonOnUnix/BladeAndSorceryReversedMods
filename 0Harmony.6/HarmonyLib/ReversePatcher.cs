using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000081 RID: 129
	public class ReversePatcher
	{
		// Token: 0x0600027C RID: 636 RVA: 0x0000D09B File Offset: 0x0000B29B
		public ReversePatcher(Harmony instance, MethodBase original, HarmonyMethod standin)
		{
			this.instance = instance;
			this.original = original;
			this.standin = standin;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000D0B8 File Offset: 0x0000B2B8
		public MethodInfo Patch(HarmonyReversePatchType type = HarmonyReversePatchType.Original)
		{
			if (this.original == null)
			{
				throw new NullReferenceException("Null method for " + this.instance.Id);
			}
			MethodInfo transpiler = ReversePatcher.GetTranspiler(this.standin.method);
			return PatchFunctions.ReversePatch(this.standin, this.original, transpiler);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000D10C File Offset: 0x0000B30C
		internal static MethodInfo GetTranspiler(MethodInfo method)
		{
			string methodName = method.Name;
			IEnumerable<MethodInfo> declaredMethods = AccessTools.GetDeclaredMethods(method.DeclaringType);
			Type ici = typeof(IEnumerable<CodeInstruction>);
			return declaredMethods.FirstOrDefault((MethodInfo m) => !(m.ReturnType != ici) && m.Name.StartsWith("<" + methodName + ">"));
		}

		// Token: 0x0400018E RID: 398
		private readonly Harmony instance;

		// Token: 0x0400018F RID: 399
		private readonly MethodBase original;

		// Token: 0x04000190 RID: 400
		private readonly HarmonyMethod standin;
	}
}
