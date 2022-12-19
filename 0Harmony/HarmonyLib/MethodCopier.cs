using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x02000020 RID: 32
	internal class MethodCopier
	{
		// Token: 0x060000AD RID: 173 RVA: 0x000054A4 File Offset: 0x000036A4
		internal MethodCopier(MethodBase fromMethod, ILGenerator toILGenerator, LocalBuilder[] existingVariables = null)
		{
			if (fromMethod == null)
			{
				throw new ArgumentNullException("fromMethod");
			}
			this.reader = new MethodBodyReader(fromMethod, toILGenerator);
			this.reader.DeclareVariables(existingVariables);
			this.reader.ReadInstructions();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000054F4 File Offset: 0x000036F4
		internal void SetDebugging(bool debug)
		{
			this.reader.SetDebugging(debug);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005502 File Offset: 0x00003702
		internal void SetArgumentShift(bool useShift)
		{
			this.reader.SetArgumentShift(useShift);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005510 File Offset: 0x00003710
		internal void AddTranspiler(MethodInfo transpiler)
		{
			this.transpilers.Add(transpiler);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000551E File Offset: 0x0000371E
		internal List<CodeInstruction> Finalize(Emitter emitter, List<Label> endLabels, out bool hasReturnCode)
		{
			return this.reader.FinalizeILCodes(emitter, this.transpilers, endLabels, out hasReturnCode);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005534 File Offset: 0x00003734
		internal static List<CodeInstruction> GetInstructions(ILGenerator generator, MethodBase method, int maxTranspilers)
		{
			if (generator == null)
			{
				throw new ArgumentNullException("generator");
			}
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			LocalBuilder[] array = MethodPatcher.DeclareLocalVariables(generator, method);
			bool flag = StructReturnBuffer.NeedsFix(method);
			MethodCopier methodCopier = new MethodCopier(method, generator, array);
			methodCopier.SetArgumentShift(flag);
			Patches patchInfo = Harmony.GetPatchInfo(method);
			if (patchInfo != null)
			{
				List<MethodInfo> sortedPatchMethods = PatchFunctions.GetSortedPatchMethods(method, patchInfo.Transpilers.ToArray<Patch>(), false);
				int num = 0;
				while (num < maxTranspilers && num < sortedPatchMethods.Count)
				{
					methodCopier.AddTranspiler(sortedPatchMethods[num]);
					num++;
				}
			}
			bool flag2;
			return methodCopier.Finalize(null, null, out flag2);
		}

		// Token: 0x0400005A RID: 90
		private readonly MethodBodyReader reader;

		// Token: 0x0400005B RID: 91
		private readonly List<MethodInfo> transpilers = new List<MethodInfo>();
	}
}
