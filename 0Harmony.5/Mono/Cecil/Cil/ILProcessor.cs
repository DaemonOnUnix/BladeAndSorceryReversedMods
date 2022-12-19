using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002B8 RID: 696
	public sealed class ILProcessor
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x00039077 File Offset: 0x00037277
		public MethodBody Body
		{
			get
			{
				return this.body;
			}
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0003907F File Offset: 0x0003727F
		internal ILProcessor(MethodBody body)
		{
			this.body = body;
			this.instructions = body.Instructions;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0003909A File Offset: 0x0003729A
		public Instruction Create(OpCode opcode)
		{
			return Instruction.Create(opcode);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x000390A2 File Offset: 0x000372A2
		public Instruction Create(OpCode opcode, TypeReference type)
		{
			return Instruction.Create(opcode, type);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x000390AB File Offset: 0x000372AB
		public Instruction Create(OpCode opcode, CallSite site)
		{
			return Instruction.Create(opcode, site);
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x000390B4 File Offset: 0x000372B4
		public Instruction Create(OpCode opcode, MethodReference method)
		{
			return Instruction.Create(opcode, method);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x000390BD File Offset: 0x000372BD
		public Instruction Create(OpCode opcode, FieldReference field)
		{
			return Instruction.Create(opcode, field);
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x000390C6 File Offset: 0x000372C6
		public Instruction Create(OpCode opcode, string value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x000390CF File Offset: 0x000372CF
		public Instruction Create(OpCode opcode, sbyte value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x000390D8 File Offset: 0x000372D8
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

		// Token: 0x060011A9 RID: 4521 RVA: 0x00039130 File Offset: 0x00037330
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

		// Token: 0x060011AA RID: 4522 RVA: 0x00039185 File Offset: 0x00037385
		public Instruction Create(OpCode opcode, long value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0003918E File Offset: 0x0003738E
		public Instruction Create(OpCode opcode, float value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00039197 File Offset: 0x00037397
		public Instruction Create(OpCode opcode, double value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x000391A0 File Offset: 0x000373A0
		public Instruction Create(OpCode opcode, Instruction target)
		{
			return Instruction.Create(opcode, target);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x000391A9 File Offset: 0x000373A9
		public Instruction Create(OpCode opcode, Instruction[] targets)
		{
			return Instruction.Create(opcode, targets);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x000391B2 File Offset: 0x000373B2
		public Instruction Create(OpCode opcode, VariableDefinition variable)
		{
			return Instruction.Create(opcode, variable);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000391BB File Offset: 0x000373BB
		public Instruction Create(OpCode opcode, ParameterDefinition parameter)
		{
			return Instruction.Create(opcode, parameter);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x000391C4 File Offset: 0x000373C4
		public void Emit(OpCode opcode)
		{
			this.Append(this.Create(opcode));
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x000391D3 File Offset: 0x000373D3
		public void Emit(OpCode opcode, TypeReference type)
		{
			this.Append(this.Create(opcode, type));
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x000391E3 File Offset: 0x000373E3
		public void Emit(OpCode opcode, MethodReference method)
		{
			this.Append(this.Create(opcode, method));
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x000391F3 File Offset: 0x000373F3
		public void Emit(OpCode opcode, CallSite site)
		{
			this.Append(this.Create(opcode, site));
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00039203 File Offset: 0x00037403
		public void Emit(OpCode opcode, FieldReference field)
		{
			this.Append(this.Create(opcode, field));
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00039213 File Offset: 0x00037413
		public void Emit(OpCode opcode, string value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00039223 File Offset: 0x00037423
		public void Emit(OpCode opcode, byte value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00039233 File Offset: 0x00037433
		public void Emit(OpCode opcode, sbyte value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00039243 File Offset: 0x00037443
		public void Emit(OpCode opcode, int value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00039253 File Offset: 0x00037453
		public void Emit(OpCode opcode, long value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00039263 File Offset: 0x00037463
		public void Emit(OpCode opcode, float value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00039273 File Offset: 0x00037473
		public void Emit(OpCode opcode, double value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00039283 File Offset: 0x00037483
		public void Emit(OpCode opcode, Instruction target)
		{
			this.Append(this.Create(opcode, target));
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00039293 File Offset: 0x00037493
		public void Emit(OpCode opcode, Instruction[] targets)
		{
			this.Append(this.Create(opcode, targets));
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x000392A3 File Offset: 0x000374A3
		public void Emit(OpCode opcode, VariableDefinition variable)
		{
			this.Append(this.Create(opcode, variable));
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x000392B3 File Offset: 0x000374B3
		public void Emit(OpCode opcode, ParameterDefinition parameter)
		{
			this.Append(this.Create(opcode, parameter));
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x000392C4 File Offset: 0x000374C4
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

		// Token: 0x060011C2 RID: 4546 RVA: 0x00039318 File Offset: 0x00037518
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

		// Token: 0x060011C3 RID: 4547 RVA: 0x0003936C File Offset: 0x0003756C
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

		// Token: 0x060011C4 RID: 4548 RVA: 0x000393A8 File Offset: 0x000375A8
		public void Append(Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.instructions.Add(instruction);
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x000393C4 File Offset: 0x000375C4
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

		// Token: 0x060011C6 RID: 4550 RVA: 0x000393F1 File Offset: 0x000375F1
		public void Replace(int index, Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.InsertAfter(index, instruction);
			this.RemoveAt(index);
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00039410 File Offset: 0x00037610
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

		// Token: 0x060011C8 RID: 4552 RVA: 0x00039439 File Offset: 0x00037639
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.instructions.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.instructions.RemoveAt(index);
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00039464 File Offset: 0x00037664
		public void Clear()
		{
			this.instructions.Clear();
		}

		// Token: 0x040007E9 RID: 2025
		private readonly MethodBody body;

		// Token: 0x040007EA RID: 2026
		private readonly Collection<Instruction> instructions;
	}
}
