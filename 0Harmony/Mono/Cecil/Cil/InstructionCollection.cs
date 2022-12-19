using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002BC RID: 700
	internal class InstructionCollection : Collection<Instruction>
	{
		// Token: 0x06001201 RID: 4609 RVA: 0x00039CF4 File Offset: 0x00037EF4
		internal InstructionCollection(MethodDefinition method)
		{
			this.method = method;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00039D03 File Offset: 0x00037F03
		internal InstructionCollection(MethodDefinition method, int capacity)
			: base(capacity)
		{
			this.method = method;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x00039D14 File Offset: 0x00037F14
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

		// Token: 0x06001204 RID: 4612 RVA: 0x00039D40 File Offset: 0x00037F40
		protected override void OnInsert(Instruction item, int index)
		{
			if (this.size != 0)
			{
				Instruction instruction = this.items[index];
				if (instruction == null)
				{
					Instruction instruction2 = this.items[index - 1];
					instruction2.next = item;
					item.previous = instruction2;
					return;
				}
				int offset = instruction.Offset;
				Instruction previous = instruction.previous;
				if (previous != null)
				{
					previous.next = item;
					item.previous = previous;
				}
				instruction.previous = item;
				item.next = instruction;
			}
			this.UpdateLocalScopes(null, null);
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00039DB0 File Offset: 0x00037FB0
		protected override void OnSet(Instruction item, int index)
		{
			Instruction instruction = this.items[index];
			item.previous = instruction.previous;
			item.next = instruction.next;
			instruction.previous = null;
			instruction.next = null;
			this.UpdateLocalScopes(item, instruction);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00039DF4 File Offset: 0x00037FF4
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
			this.UpdateLocalScopes(item, next ?? previous);
			item.previous = null;
			item.next = null;
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x00039E50 File Offset: 0x00038050
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

		// Token: 0x06001208 RID: 4616 RVA: 0x00039EAC File Offset: 0x000380AC
		private void UpdateLocalScopes(Instruction removedInstruction, Instruction existingInstruction)
		{
			MethodDebugInformation debug_info = this.method.debug_info;
			if (debug_info == null)
			{
				return;
			}
			InstructionCollection.InstructionOffsetCache instructionOffsetCache = new InstructionCollection.InstructionOffsetCache
			{
				Offset = 0,
				Index = 0,
				Instruction = this.items[0]
			};
			this.UpdateLocalScope(debug_info.Scope, removedInstruction, existingInstruction, ref instructionOffsetCache);
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00039F04 File Offset: 0x00038104
		private void UpdateLocalScope(ScopeDebugInformation scope, Instruction removedInstruction, Instruction existingInstruction, ref InstructionCollection.InstructionOffsetCache cache)
		{
			if (scope == null)
			{
				return;
			}
			if (!scope.Start.IsResolved)
			{
				scope.Start = this.ResolveInstructionOffset(scope.Start, ref cache);
			}
			if (!scope.Start.IsEndOfMethod && scope.Start.ResolvedInstruction == removedInstruction)
			{
				scope.Start = new InstructionOffset(existingInstruction);
			}
			if (scope.HasScopes)
			{
				foreach (ScopeDebugInformation scopeDebugInformation in scope.Scopes)
				{
					this.UpdateLocalScope(scopeDebugInformation, removedInstruction, existingInstruction, ref cache);
				}
			}
			if (!scope.End.IsResolved)
			{
				scope.End = this.ResolveInstructionOffset(scope.End, ref cache);
			}
			if (!scope.End.IsEndOfMethod && scope.End.ResolvedInstruction == removedInstruction)
			{
				scope.End = new InstructionOffset(existingInstruction);
			}
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x0003A00C File Offset: 0x0003820C
		private InstructionOffset ResolveInstructionOffset(InstructionOffset inputOffset, ref InstructionCollection.InstructionOffsetCache cache)
		{
			if (inputOffset.IsResolved)
			{
				return inputOffset;
			}
			int offset = inputOffset.Offset;
			if (cache.Offset == offset)
			{
				return new InstructionOffset(cache.Instruction);
			}
			if (cache.Offset > offset)
			{
				int num = 0;
				for (int i = 0; i < this.items.Length; i++)
				{
					if (num == offset)
					{
						return new InstructionOffset(this.items[i]);
					}
					if (num > offset)
					{
						return new InstructionOffset(this.items[i - 1]);
					}
					num += this.items[i].GetSize();
				}
				return default(InstructionOffset);
			}
			int num2 = cache.Offset;
			for (int j = cache.Index; j < this.items.Length; j++)
			{
				cache.Index = j;
				cache.Offset = num2;
				Instruction instruction = this.items[j];
				if (instruction == null)
				{
					break;
				}
				cache.Instruction = instruction;
				if (cache.Offset == offset)
				{
					return new InstructionOffset(cache.Instruction);
				}
				if (cache.Offset > offset)
				{
					return new InstructionOffset(this.items[j - 1]);
				}
				num2 += instruction.GetSize();
			}
			return default(InstructionOffset);
		}

		// Token: 0x040007FA RID: 2042
		private readonly MethodDefinition method;

		// Token: 0x020002BD RID: 701
		private struct InstructionOffsetCache
		{
			// Token: 0x040007FB RID: 2043
			public int Offset;

			// Token: 0x040007FC RID: 2044
			public int Index;

			// Token: 0x040007FD RID: 2045
			public Instruction Instruction;
		}
	}
}
