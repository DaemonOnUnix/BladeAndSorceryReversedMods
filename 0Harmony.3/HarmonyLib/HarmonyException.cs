using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace HarmonyLib
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public class HarmonyException : Exception
	{
		// Token: 0x060001BC RID: 444 RVA: 0x0000A7EC File Offset: 0x000089EC
		internal HarmonyException()
		{
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000A7F4 File Offset: 0x000089F4
		internal HarmonyException(string message)
			: base(message)
		{
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000A7FD File Offset: 0x000089FD
		internal HarmonyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000A807 File Offset: 0x00008A07
		protected HarmonyException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000A814 File Offset: 0x00008A14
		internal HarmonyException(Exception innerException, Dictionary<int, CodeInstruction> instructions, int errorOffset)
			: base("IL Compile Error", innerException)
		{
			this.instructions = instructions;
			this.errorOffset = errorOffset;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000A830 File Offset: 0x00008A30
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

		// Token: 0x060001C2 RID: 450 RVA: 0x0000A8C1 File Offset: 0x00008AC1
		public List<KeyValuePair<int, CodeInstruction>> GetInstructionsWithOffsets()
		{
			return this.instructions.OrderBy((KeyValuePair<int, CodeInstruction> ins) => ins.Key).ToList<KeyValuePair<int, CodeInstruction>>();
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000A8F4 File Offset: 0x00008AF4
		public List<CodeInstruction> GetInstructions()
		{
			return (from ins in this.instructions
				orderby ins.Key
				select ins.Value).ToList<CodeInstruction>();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000A954 File Offset: 0x00008B54
		public int GetErrorOffset()
		{
			return this.errorOffset;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000A95C File Offset: 0x00008B5C
		public int GetErrorIndex()
		{
			CodeInstruction codeInstruction;
			if (this.instructions.TryGetValue(this.errorOffset, out codeInstruction))
			{
				return this.GetInstructions().IndexOf(codeInstruction);
			}
			return -1;
		}

		// Token: 0x04000115 RID: 277
		private Dictionary<int, CodeInstruction> instructions;

		// Token: 0x04000116 RID: 278
		private int errorOffset;
	}
}
