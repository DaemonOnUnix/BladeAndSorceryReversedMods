﻿using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000325 RID: 805
	internal static class MethodBodyRocks
	{
		// Token: 0x060012C6 RID: 4806 RVA: 0x0003F188 File Offset: 0x0003D388
		public static void SimplifyMacros(this MethodBody self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			foreach (Instruction instruction in self.Instructions)
			{
				if (instruction.OpCode.OpCodeType == OpCodeType.Macro)
				{
					Code code = instruction.OpCode.Code;
					switch (code)
					{
					case Code.Ldarg_0:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldarg, self.GetParameter(0));
						break;
					case Code.Ldarg_1:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldarg, self.GetParameter(1));
						break;
					case Code.Ldarg_2:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldarg, self.GetParameter(2));
						break;
					case Code.Ldarg_3:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldarg, self.GetParameter(3));
						break;
					case Code.Ldloc_0:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldloc, self.Variables[0]);
						break;
					case Code.Ldloc_1:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldloc, self.Variables[1]);
						break;
					case Code.Ldloc_2:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldloc, self.Variables[2]);
						break;
					case Code.Ldloc_3:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldloc, self.Variables[3]);
						break;
					case Code.Stloc_0:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Stloc, self.Variables[0]);
						break;
					case Code.Stloc_1:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Stloc, self.Variables[1]);
						break;
					case Code.Stloc_2:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Stloc, self.Variables[2]);
						break;
					case Code.Stloc_3:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Stloc, self.Variables[3]);
						break;
					case Code.Ldarg_S:
						instruction.OpCode = OpCodes.Ldarg;
						break;
					case Code.Ldarga_S:
						instruction.OpCode = OpCodes.Ldarga;
						break;
					case Code.Starg_S:
						instruction.OpCode = OpCodes.Starg;
						break;
					case Code.Ldloc_S:
						instruction.OpCode = OpCodes.Ldloc;
						break;
					case Code.Ldloca_S:
						instruction.OpCode = OpCodes.Ldloca;
						break;
					case Code.Stloc_S:
						instruction.OpCode = OpCodes.Stloc;
						break;
					case Code.Ldnull:
					case Code.Ldc_I4:
					case Code.Ldc_I8:
					case Code.Ldc_R4:
					case Code.Ldc_R8:
					case Code.Dup:
					case Code.Pop:
					case Code.Jmp:
					case Code.Call:
					case Code.Calli:
					case Code.Ret:
						break;
					case Code.Ldc_I4_M1:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, -1);
						break;
					case Code.Ldc_I4_0:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, 0);
						break;
					case Code.Ldc_I4_1:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, 1);
						break;
					case Code.Ldc_I4_2:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, 2);
						break;
					case Code.Ldc_I4_3:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, 3);
						break;
					case Code.Ldc_I4_4:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, 4);
						break;
					case Code.Ldc_I4_5:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, 5);
						break;
					case Code.Ldc_I4_6:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, 6);
						break;
					case Code.Ldc_I4_7:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, 7);
						break;
					case Code.Ldc_I4_8:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, 8);
						break;
					case Code.Ldc_I4_S:
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, (int)((sbyte)instruction.Operand));
						break;
					case Code.Br_S:
						instruction.OpCode = OpCodes.Br;
						break;
					case Code.Brfalse_S:
						instruction.OpCode = OpCodes.Brfalse;
						break;
					case Code.Brtrue_S:
						instruction.OpCode = OpCodes.Brtrue;
						break;
					case Code.Beq_S:
						instruction.OpCode = OpCodes.Beq;
						break;
					case Code.Bge_S:
						instruction.OpCode = OpCodes.Bge;
						break;
					case Code.Bgt_S:
						instruction.OpCode = OpCodes.Bgt;
						break;
					case Code.Ble_S:
						instruction.OpCode = OpCodes.Ble;
						break;
					case Code.Blt_S:
						instruction.OpCode = OpCodes.Blt;
						break;
					case Code.Bne_Un_S:
						instruction.OpCode = OpCodes.Bne_Un;
						break;
					case Code.Bge_Un_S:
						instruction.OpCode = OpCodes.Bge_Un;
						break;
					case Code.Bgt_Un_S:
						instruction.OpCode = OpCodes.Bgt_Un;
						break;
					case Code.Ble_Un_S:
						instruction.OpCode = OpCodes.Ble_Un;
						break;
					case Code.Blt_Un_S:
						instruction.OpCode = OpCodes.Blt_Un;
						break;
					default:
						if (code == Code.Leave_S)
						{
							instruction.OpCode = OpCodes.Leave;
						}
						break;
					}
				}
			}
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0003F65C File Offset: 0x0003D85C
		private static void ExpandMacro(Instruction instruction, OpCode opcode, object operand)
		{
			instruction.OpCode = opcode;
			instruction.Operand = operand;
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0003F66C File Offset: 0x0003D86C
		private static void MakeMacro(Instruction instruction, OpCode opcode)
		{
			instruction.OpCode = opcode;
			instruction.Operand = null;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x0003F67C File Offset: 0x0003D87C
		public static void Optimize(this MethodBody self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			self.OptimizeLongs();
			self.OptimizeMacros();
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0003F698 File Offset: 0x0003D898
		private static void OptimizeLongs(this MethodBody self)
		{
			for (int i = 0; i < self.Instructions.Count; i++)
			{
				Instruction instruction = self.Instructions[i];
				if (instruction.OpCode.Code == Code.Ldc_I8)
				{
					long num = (long)instruction.Operand;
					if (num < 2147483647L && num > -2147483648L)
					{
						MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4, (int)num);
						self.Instructions.Insert(++i, Instruction.Create(OpCodes.Conv_I8));
					}
				}
			}
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0003F724 File Offset: 0x0003D924
		public static void OptimizeMacros(this MethodBody self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			MethodDefinition method = self.Method;
			foreach (Instruction instruction in self.Instructions)
			{
				Code code = instruction.OpCode.Code;
				if (code != Code.Ldc_I4)
				{
					switch (code)
					{
					case Code.Ldarg:
					{
						int num = ((ParameterDefinition)instruction.Operand).Index;
						if (num == -1 && instruction.Operand == self.ThisParameter)
						{
							num = 0;
						}
						else if (method.HasThis)
						{
							num++;
						}
						switch (num)
						{
						case 0:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldarg_0);
							break;
						case 1:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldarg_1);
							break;
						case 2:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldarg_2);
							break;
						case 3:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldarg_3);
							break;
						default:
							if (num < 256)
							{
								MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldarg_S, instruction.Operand);
							}
							break;
						}
						break;
					}
					case Code.Ldarga:
					{
						int num = ((ParameterDefinition)instruction.Operand).Index;
						if (num == -1 && instruction.Operand == self.ThisParameter)
						{
							num = 0;
						}
						else if (method.HasThis)
						{
							num++;
						}
						if (num < 256)
						{
							MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldarga_S, instruction.Operand);
						}
						break;
					}
					case Code.Ldloc:
					{
						int num = ((VariableDefinition)instruction.Operand).Index;
						switch (num)
						{
						case 0:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldloc_0);
							break;
						case 1:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldloc_1);
							break;
						case 2:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldloc_2);
							break;
						case 3:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldloc_3);
							break;
						default:
							if (num < 256)
							{
								MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldloc_S, instruction.Operand);
							}
							break;
						}
						break;
					}
					case Code.Ldloca:
						if (((VariableDefinition)instruction.Operand).Index < 256)
						{
							MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldloca_S, instruction.Operand);
						}
						break;
					case Code.Stloc:
					{
						int num = ((VariableDefinition)instruction.Operand).Index;
						switch (num)
						{
						case 0:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Stloc_0);
							break;
						case 1:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Stloc_1);
							break;
						case 2:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Stloc_2);
							break;
						case 3:
							MethodBodyRocks.MakeMacro(instruction, OpCodes.Stloc_3);
							break;
						default:
							if (num < 256)
							{
								MethodBodyRocks.ExpandMacro(instruction, OpCodes.Stloc_S, instruction.Operand);
							}
							break;
						}
						break;
					}
					}
				}
				else
				{
					int num2 = (int)instruction.Operand;
					switch (num2)
					{
					case -1:
						MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldc_I4_M1);
						break;
					case 0:
						MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldc_I4_0);
						break;
					case 1:
						MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldc_I4_1);
						break;
					case 2:
						MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldc_I4_2);
						break;
					case 3:
						MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldc_I4_3);
						break;
					case 4:
						MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldc_I4_4);
						break;
					case 5:
						MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldc_I4_5);
						break;
					case 6:
						MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldc_I4_6);
						break;
					case 7:
						MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldc_I4_7);
						break;
					case 8:
						MethodBodyRocks.MakeMacro(instruction, OpCodes.Ldc_I4_8);
						break;
					default:
						if (num2 >= -128 && num2 < 128)
						{
							MethodBodyRocks.ExpandMacro(instruction, OpCodes.Ldc_I4_S, (sbyte)num2);
						}
						break;
					}
				}
			}
			MethodBodyRocks.OptimizeBranches(self);
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0003FB14 File Offset: 0x0003DD14
		private static void OptimizeBranches(MethodBody body)
		{
			MethodBodyRocks.ComputeOffsets(body);
			foreach (Instruction instruction in body.Instructions)
			{
				if (instruction.OpCode.OperandType == OperandType.InlineBrTarget && MethodBodyRocks.OptimizeBranch(instruction))
				{
					MethodBodyRocks.ComputeOffsets(body);
				}
			}
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0003FB84 File Offset: 0x0003DD84
		private static bool OptimizeBranch(Instruction instruction)
		{
			int num = ((Instruction)instruction.Operand).Offset - (instruction.Offset + instruction.OpCode.Size + 4);
			if (num < -128 || num > 127)
			{
				return false;
			}
			Code code = instruction.OpCode.Code;
			switch (code)
			{
			case Code.Br:
				instruction.OpCode = OpCodes.Br_S;
				break;
			case Code.Brfalse:
				instruction.OpCode = OpCodes.Brfalse_S;
				break;
			case Code.Brtrue:
				instruction.OpCode = OpCodes.Brtrue_S;
				break;
			case Code.Beq:
				instruction.OpCode = OpCodes.Beq_S;
				break;
			case Code.Bge:
				instruction.OpCode = OpCodes.Bge_S;
				break;
			case Code.Bgt:
				instruction.OpCode = OpCodes.Bgt_S;
				break;
			case Code.Ble:
				instruction.OpCode = OpCodes.Ble_S;
				break;
			case Code.Blt:
				instruction.OpCode = OpCodes.Blt_S;
				break;
			case Code.Bne_Un:
				instruction.OpCode = OpCodes.Bne_Un_S;
				break;
			case Code.Bge_Un:
				instruction.OpCode = OpCodes.Bge_Un_S;
				break;
			case Code.Bgt_Un:
				instruction.OpCode = OpCodes.Bgt_Un_S;
				break;
			case Code.Ble_Un:
				instruction.OpCode = OpCodes.Ble_Un_S;
				break;
			case Code.Blt_Un:
				instruction.OpCode = OpCodes.Blt_Un_S;
				break;
			default:
				if (code == Code.Leave)
				{
					instruction.OpCode = OpCodes.Leave_S;
				}
				break;
			}
			return true;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0003FCE4 File Offset: 0x0003DEE4
		private static void ComputeOffsets(MethodBody body)
		{
			int num = 0;
			foreach (Instruction instruction in body.Instructions)
			{
				instruction.Offset = num;
				num += instruction.GetSize();
			}
		}
	}
}
