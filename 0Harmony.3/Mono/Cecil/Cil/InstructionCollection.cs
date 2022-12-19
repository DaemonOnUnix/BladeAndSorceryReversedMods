using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C7 RID: 455
	internal class InstructionCollection : Collection<Instruction>
	{
		// Token: 0x06000E9C RID: 3740 RVA: 0x000321A3 File Offset: 0x000303A3
		internal InstructionCollection(MethodDefinition method)
		{
			this.method = method;
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x000321B2 File Offset: 0x000303B2
		internal InstructionCollection(MethodDefinition method, int capacity)
			: base(capacity)
		{
			this.method = method;
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x000321C4 File Offset: 0x000303C4
		protected override void OnAdd(Instruction item, int index)
		{
			if (index == 0)
			{
				return;
			}
			Instruction instruction = this.items[index - 1];
			instruction.next = item;
			item.previous = instruction;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x000321F0 File Offset: 0x000303F0
		protected override void OnInsert(Instruction item, int index)
		{
			if (this.size == 0)
			{
				return;
			}
			Instruction instruction = this.items[index];
			if (instruction == null)
			{
				Instruction instruction2 = this.items[index - 1];
				instruction2.next = item;
				item.previous = instruction2;
				return;
			}
			Instruction previous = instruction.previous;
			if (previous != null)
			{
				previous.next = item;
				item.previous = previous;
			}
			instruction.previous = item;
			item.next = instruction;
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00032254 File Offset: 0x00030454
		protected override void OnSet(Instruction item, int index)
		{
			Instruction instruction = this.items[index];
			item.previous = instruction.previous;
			item.next = instruction.next;
			instruction.previous = null;
			instruction.next = null;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00032290 File Offset: 0x00030490
		protected override void OnRemove(Instruction item, int index)
		{
			Instruction previous = item.previous;
			if (previous != null)
			{
				previous.next = item.next;
			}
			Instruction next = item.next;
			if (next != null)
			{
				next.previous = item.previous;
			}
			this.RemoveSequencePoint(item);
			item.previous = null;
			item.next = null;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x000322E0 File Offset: 0x000304E0
		private void RemoveSequencePoint(Instruction instruction)
		{
			MethodDebugInformation debug_info = this.method.debug_info;
			if (debug_info == null || !debug_info.HasSequencePoints)
			{
				return;
			}
			Collection<SequencePoint> sequence_points = debug_info.sequence_points;
			for (int i = 0; i < sequence_points.Count; i++)
			{
				if (sequence_points[i].Offset == instruction.offset)
				{
					sequence_points.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x040007C1 RID: 1985
		private readonly MethodDefinition method;
	}
}
