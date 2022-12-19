using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x02000083 RID: 131
	public static class Transpilers
	{
		// Token: 0x06000281 RID: 641 RVA: 0x0000D18E File Offset: 0x0000B38E
		public static IEnumerable<CodeInstruction> MethodReplacer(this IEnumerable<CodeInstruction> instructions, MethodBase from, MethodBase to)
		{
			if (from == null)
			{
				throw new ArgumentException("Unexpected null argument", "from");
			}
			if (to == null)
			{
				throw new ArgumentException("Unexpected null argument", "to");
			}
			foreach (CodeInstruction codeInstruction in instructions)
			{
				if (codeInstruction.operand as MethodBase == from)
				{
					codeInstruction.opcode = (to.IsConstructor ? OpCodes.Newobj : OpCodes.Call);
					codeInstruction.operand = to;
				}
				yield return codeInstruction;
			}
			IEnumerator<CodeInstruction> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000D1AC File Offset: 0x0000B3AC
		public static IEnumerable<CodeInstruction> Manipulator(this IEnumerable<CodeInstruction> instructions, Func<CodeInstruction, bool> predicate, Action<CodeInstruction> action)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			return instructions.Select(delegate(CodeInstruction instruction)
			{
				if (predicate(instruction))
				{
					action(instruction);
				}
				return instruction;
			}).AsEnumerable<CodeInstruction>();
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000D20A File Offset: 0x0000B40A
		public static IEnumerable<CodeInstruction> DebugLogger(this IEnumerable<CodeInstruction> instructions, string text)
		{
			yield return new CodeInstruction(OpCodes.Ldstr, text);
			yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(FileLog), "Log", null, null));
			foreach (CodeInstruction codeInstruction in instructions)
			{
				yield return codeInstruction;
			}
			IEnumerator<CodeInstruction> enumerator = null;
			yield break;
			yield break;
		}
	}
}
