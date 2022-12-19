using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x02000183 RID: 387
	public class CodeMatch : CodeInstruction
	{
		// Token: 0x060005F9 RID: 1529 RVA: 0x00014716 File Offset: 0x00012916
		internal CodeMatch Set(object operand, string name)
		{
			if (this.operand == null)
			{
				this.operand = operand;
			}
			if (this.name == null)
			{
				this.name = name;
			}
			return this;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00014738 File Offset: 0x00012938
		public CodeMatch(OpCode? opcode = null, object operand = null, string name = null)
		{
			if (opcode != null)
			{
				OpCode valueOrDefault = opcode.GetValueOrDefault();
				this.opcode = valueOrDefault;
				this.opcodes.Add(valueOrDefault);
			}
			if (operand != null)
			{
				this.operands.Add(operand);
			}
			this.operand = operand;
			this.name = name;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000147B8 File Offset: 0x000129B8
		public CodeMatch(CodeInstruction instruction, string name = null)
			: this(new OpCode?(instruction.opcode), instruction.operand, name)
		{
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000147D4 File Offset: 0x000129D4
		public CodeMatch(Func<CodeInstruction, bool> predicate, string name = null)
		{
			this.predicate = predicate;
			this.name = name;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00014824 File Offset: 0x00012A24
		internal bool Matches(List<CodeInstruction> codes, CodeInstruction instruction)
		{
			if (this.predicate != null)
			{
				return this.predicate(instruction);
			}
			if (this.opcodes.Count > 0 && !this.opcodes.Contains(instruction.opcode))
			{
				return false;
			}
			if (this.operands.Count > 0 && !this.operands.Contains(instruction.operand))
			{
				return false;
			}
			if (this.labels.Count > 0 && !this.labels.Intersect(instruction.labels).Any<Label>())
			{
				return false;
			}
			if (this.blocks.Count > 0 && !this.blocks.Intersect(instruction.blocks).Any<ExceptionBlock>())
			{
				return false;
			}
			if (this.jumpsFrom.Count > 0 && !this.jumpsFrom.Select((int index) => codes[index].operand).OfType<Label>().Intersect(instruction.labels)
				.Any<Label>())
			{
				return false;
			}
			if (this.jumpsTo.Count > 0)
			{
				object operand = instruction.operand;
				if (operand == null || operand.GetType() != typeof(Label))
				{
					return false;
				}
				Label label = (Label)operand;
				IEnumerable<int> enumerable = from idx in Enumerable.Range(0, codes.Count)
					where codes[idx].labels.Contains(label)
					select idx;
				if (!this.jumpsTo.Intersect(enumerable).Any<int>())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000149B0 File Offset: 0x00012BB0
		public override string ToString()
		{
			string text = "[";
			if (this.name != null)
			{
				text = text + this.name + ": ";
			}
			if (this.opcodes.Count > 0)
			{
				text = text + "opcodes=" + this.opcodes.Join(null, ", ") + " ";
			}
			if (this.operands.Count > 0)
			{
				text = text + "operands=" + this.operands.Join(null, ", ") + " ";
			}
			if (this.labels.Count > 0)
			{
				text = text + "labels=" + this.labels.Join(null, ", ") + " ";
			}
			if (this.blocks.Count > 0)
			{
				text = text + "blocks=" + this.blocks.Join(null, ", ") + " ";
			}
			if (this.jumpsFrom.Count > 0)
			{
				text = text + "jumpsFrom=" + this.jumpsFrom.Join(null, ", ") + " ";
			}
			if (this.jumpsTo.Count > 0)
			{
				text = text + "jumpsTo=" + this.jumpsTo.Join(null, ", ") + " ";
			}
			if (this.predicate != null)
			{
				text += "predicate=yes ";
			}
			return text.TrimEnd(Array.Empty<char>()) + "]";
		}

		// Token: 0x040001E1 RID: 481
		public string name;

		// Token: 0x040001E2 RID: 482
		public List<OpCode> opcodes = new List<OpCode>();

		// Token: 0x040001E3 RID: 483
		public List<object> operands = new List<object>();

		// Token: 0x040001E4 RID: 484
		public List<int> jumpsFrom = new List<int>();

		// Token: 0x040001E5 RID: 485
		public List<int> jumpsTo = new List<int>();

		// Token: 0x040001E6 RID: 486
		public Func<CodeInstruction, bool> predicate;
	}
}
