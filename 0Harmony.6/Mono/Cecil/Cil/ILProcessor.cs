using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C3 RID: 451
	public sealed class ILProcessor
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x0003164B File Offset: 0x0002F84B
		public MethodBody Body
		{
			get
			{
				return this.body;
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00031653 File Offset: 0x0002F853
		internal ILProcessor(MethodBody body)
		{
			this.body = body;
			this.instructions = body.Instructions;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0003166E File Offset: 0x0002F86E
		public Instruction Create(OpCode opcode)
		{
			return Instruction.Create(opcode);
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00031676 File Offset: 0x0002F876
		public Instruction Create(OpCode opcode, TypeReference type)
		{
			return Instruction.Create(opcode, type);
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0003167F File Offset: 0x0002F87F
		public Instruction Create(OpCode opcode, CallSite site)
		{
			return Instruction.Create(opcode, site);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00031688 File Offset: 0x0002F888
		public Instruction Create(OpCode opcode, MethodReference method)
		{
			return Instruction.Create(opcode, method);
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00031691 File Offset: 0x0002F891
		public Instruction Create(OpCode opcode, FieldReference field)
		{
			return Instruction.Create(opcode, field);
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0003169A File Offset: 0x0002F89A
		public Instruction Create(OpCode opcode, string value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x000316A3 File Offset: 0x0002F8A3
		public Instruction Create(OpCode opcode, sbyte value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x000316AC File Offset: 0x0002F8AC
		public Instruction Create(OpCode opcode, byte value)
		{
			if (opcode.OperandType == OperandType.ShortInlineVar)
			{
				return Instruction.Create(opcode, this.body.Variables[(int)value]);
			}
			if (opcode.OperandType == OperandType.ShortInlineArg)
			{
				return Instruction.Create(opcode, this.body.GetParameter((int)value));
			}
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00031704 File Offset: 0x0002F904
		public Instruction Create(OpCode opcode, int value)
		{
			if (opcode.OperandType == OperandType.InlineVar)
			{
				return Instruction.Create(opcode, this.body.Variables[value]);
			}
			if (opcode.OperandType == OperandType.InlineArg)
			{
				return Instruction.Create(opcode, this.body.GetParameter(value));
			}
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00031759 File Offset: 0x0002F959
		public Instruction Create(OpCode opcode, long value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00031762 File Offset: 0x0002F962
		public Instruction Create(OpCode opcode, float value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x0003176B File Offset: 0x0002F96B
		public Instruction Create(OpCode opcode, double value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00031774 File Offset: 0x0002F974
		public Instruction Create(OpCode opcode, Instruction target)
		{
			return Instruction.Create(opcode, target);
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0003177D File Offset: 0x0002F97D
		public Instruction Create(OpCode opcode, Instruction[] targets)
		{
			return Instruction.Create(opcode, targets);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00031786 File Offset: 0x0002F986
		public Instruction Create(OpCode opcode, VariableDefinition variable)
		{
			return Instruction.Create(opcode, variable);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0003178F File Offset: 0x0002F98F
		public Instruction Create(OpCode opcode, ParameterDefinition parameter)
		{
			return Instruction.Create(opcode, parameter);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00031798 File Offset: 0x0002F998
		public void Emit(OpCode opcode)
		{
			this.Append(this.Create(opcode));
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x000317A7 File Offset: 0x0002F9A7
		public void Emit(OpCode opcode, TypeReference type)
		{
			this.Append(this.Create(opcode, type));
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x000317B7 File Offset: 0x0002F9B7
		public void Emit(OpCode opcode, MethodReference method)
		{
			this.Append(this.Create(opcode, method));
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x000317C7 File Offset: 0x0002F9C7
		public void Emit(OpCode opcode, CallSite site)
		{
			this.Append(this.Create(opcode, site));
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x000317D7 File Offset: 0x0002F9D7
		public void Emit(OpCode opcode, FieldReference field)
		{
			this.Append(this.Create(opcode, field));
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x000317E7 File Offset: 0x0002F9E7
		public void Emit(OpCode opcode, string value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x000317F7 File Offset: 0x0002F9F7
		public void Emit(OpCode opcode, byte value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00031807 File Offset: 0x0002FA07
		public void Emit(OpCode opcode, sbyte value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00031817 File Offset: 0x0002FA17
		public void Emit(OpCode opcode, int value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00031827 File Offset: 0x0002FA27
		public void Emit(OpCode opcode, long value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00031837 File Offset: 0x0002FA37
		public void Emit(OpCode opcode, float value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00031847 File Offset: 0x0002FA47
		public void Emit(OpCode opcode, double value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00031857 File Offset: 0x0002FA57
		public void Emit(OpCode opcode, Instruction target)
		{
			this.Append(this.Create(opcode, target));
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00031867 File Offset: 0x0002FA67
		public void Emit(OpCode opcode, Instruction[] targets)
		{
			this.Append(this.Create(opcode, targets));
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00031877 File Offset: 0x0002FA77
		public void Emit(OpCode opcode, VariableDefinition variable)
		{
			this.Append(this.Create(opcode, variable));
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00031887 File Offset: 0x0002FA87
		public void Emit(OpCode opcode, ParameterDefinition parameter)
		{
			this.Append(this.Create(opcode, parameter));
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00031898 File Offset: 0x0002FA98
		public void InsertBefore(Instruction target, Instruction instruction)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			int num = this.instructions.IndexOf(target);
			if (num == -1)
			{
				throw new ArgumentOutOfRangeException("target");
			}
			this.instructions.Insert(num, instruction);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000318EC File Offset: 0x0002FAEC
		public void InsertAfter(Instruction target, Instruction instruction)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			int num = this.instructions.IndexOf(target);
			if (num == -1)
			{
				throw new ArgumentOutOfRangeException("target");
			}
			this.instructions.Insert(num + 1, instruction);
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00031940 File Offset: 0x0002FB40
		public void InsertAfter(int index, Instruction instruction)
		{
			if (index < 0 || index >= this.instructions.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.instructions.Insert(index + 1, instruction);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0003197C File Offset: 0x0002FB7C
		public void Append(Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.instructions.Add(instruction);
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00031998 File Offset: 0x0002FB98
		public void Replace(Instruction target, Instruction instruction)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.InsertAfter(target, instruction);
			this.Remove(target);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x000319C5 File Offset: 0x0002FBC5
		public void Replace(int index, Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.InsertAfter(index, instruction);
			this.RemoveAt(index);
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x000319E4 File Offset: 0x0002FBE4
		public void Remove(Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			if (!this.instructions.Remove(instruction))
			{
				throw new ArgumentOutOfRangeException("instruction");
			}
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00031A0D File Offset: 0x0002FC0D
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.instructions.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.instructions.RemoveAt(index);
		}

		// Token: 0x040007B1 RID: 1969
		private readonly MethodBody body;

		// Token: 0x040007B2 RID: 1970
		private readonly Collection<Instruction> instructions;
	}
}
