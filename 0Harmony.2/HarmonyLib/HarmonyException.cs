using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace HarmonyLib
{
	// Token: 0x02000065 RID: 101
	[Serializable]
	public class HarmonyException : Exception
	{
		// Token: 0x060001D7 RID: 471 RVA: 0x0000B9A0 File Offset: 0x00009BA0
		internal HarmonyException()
		{
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000B9A8 File Offset: 0x00009BA8
		internal HarmonyException(string message)
			: base(message)
		{
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000B9B1 File Offset: 0x00009BB1
		internal HarmonyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000B9BB File Offset: 0x00009BBB
		protected HarmonyException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000B9C8 File Offset: 0x00009BC8
		internal HarmonyException(Exception innerException, Dictionary<int, CodeInstruction> instructions, int errorOffset)
			: base("IL Compile Error", innerException)
		{
			this.instructions = instructions;
			this.errorOffset = errorOffset;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000B9E4 File Offset: 0x00009BE4
		internal static Exception Create(Exception ex, Dictionary<int, CodeInstruction> finalInstructions)
		{
			Match match = Regex.Match(ex.Message.TrimEnd(Array.Empty<char>()), "Reason: Invalid IL code in.+: IL_(\\d{4}): (.+)$");
			if (!match.Success)
			{
				return ex;
			}
			int num = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
			Regex.Replace(match.Groups[2].Value, " {2,}", " ");
			HarmonyException ex2 = ex as HarmonyException;
			if (ex2 != null)
			{
				ex2.instructions = finalInstructions;
				ex2.errorOffset = num;
				return ex2;
			}
			return new HarmonyException(ex, finalInstructions, num);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000BA75 File Offset: 0x00009C75
		public List<KeyValuePair<int, CodeInstruction>> GetInstructionsWithOffsets()
		{
			return this.instructions.OrderBy((KeyValuePair<int, CodeInstruction> ins) => ins.Key).ToList<KeyValuePair<int, CodeInstruction>>();
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000BAA8 File Offset: 0x00009CA8
		public List<CodeInstruction> GetInstructions()
		{
			return (from ins in this.instructions
				orderby ins.Key
				select ins.Value).ToList<CodeInstruction>();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000BB08 File Offset: 0x00009D08
		public int GetErrorOffset()
		{
			return this.errorOffset;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000BB10 File Offset: 0x00009D10
		public int GetErrorIndex()
		{
			CodeInstruction codeInstruction;
			if (this.instructions.TryGetValue(this.errorOffset, out codeInstruction))
			{
				return this.GetInstructions().IndexOf(codeInstruction);
			}
			return -1;
		}

		// Token: 0x04000126 RID: 294
		private Dictionary<int, CodeInstruction> instructions;

		// Token: 0x04000127 RID: 295
		private int errorOffset;
	}
}
