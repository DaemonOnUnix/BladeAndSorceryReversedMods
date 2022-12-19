using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200018E RID: 398
	public static class CodeInstructionExtensions
	{
		// Token: 0x06000663 RID: 1635 RVA: 0x00015A3F File Offset: 0x00013C3F
		public static bool IsValid(this OpCode code)
		{
			return code.Size > 0;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00015A4C File Offset: 0x00013C4C
		public static bool OperandIs(this CodeInstruction code, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (code.operand == null)
			{
				return false;
			}
			Type type = value.GetType();
			Type type2 = code.operand.GetType();
			if (AccessTools.IsInteger(type) && AccessTools.IsNumber(type2))
			{
				return Convert.ToInt64(code.operand) == Convert.ToInt64(value);
			}
			if (AccessTools.IsFloatingPoint(type) && AccessTools.IsNumber(type2))
			{
				return Convert.ToDouble(code.operand) == Convert.ToDouble(value);
			}
			return object.Equals(code.operand, value);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00015AD8 File Offset: 0x00013CD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool OperandIs(this CodeInstruction code, MemberInfo value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return object.Equals(code.operand, value);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00015AF4 File Offset: 0x00013CF4
		public static bool Is(this CodeInstruction code, OpCode opcode, object operand)
		{
			return code.opcode == opcode && code.OperandIs(operand);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00015B0D File Offset: 0x00013D0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool Is(this CodeInstruction code, OpCode opcode, MemberInfo operand)
		{
			return code.opcode == opcode && code.OperandIs(operand);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00015B28 File Offset: 0x00013D28
		public static bool IsLdarg(this CodeInstruction code, int? n = null)
		{
			return ((n == null || n.Value == 0) && code.opcode == OpCodes.Ldarg_0) || ((n == null || n.Value == 1) && code.opcode == OpCodes.Ldarg_1) || ((n == null || n.Value == 2) && code.opcode == OpCodes.Ldarg_2) || ((n == null || n.Value == 3) && code.opcode == OpCodes.Ldarg_3) || (code.opcode == OpCodes.Ldarg && (n == null || n.Value == Convert.ToInt32(code.operand))) || (code.opcode == OpCodes.Ldarg_S && (n == null || n.Value == Convert.ToInt32(code.operand)));
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00015C34 File Offset: 0x00013E34
		public static bool IsLdarga(this CodeInstruction code, int? n = null)
		{
			return (!(code.opcode != OpCodes.Ldarga) || !(code.opcode != OpCodes.Ldarga_S)) && (n == null || n.Value == Convert.ToInt32(code.operand));
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00015C88 File Offset: 0x00013E88
		public static bool IsStarg(this CodeInstruction code, int? n = null)
		{
			return (!(code.opcode != OpCodes.Starg) || !(code.opcode != OpCodes.Starg_S)) && (n == null || n.Value == Convert.ToInt32(code.operand));
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00015CDA File Offset: 0x00013EDA
		public static bool IsLdloc(this CodeInstruction code, LocalBuilder variable = null)
		{
			return CodeInstructionExtensions.loadVarCodes.Contains(code.opcode) && (variable == null || object.Equals(variable, code.operand));
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00015D01 File Offset: 0x00013F01
		public static bool IsStloc(this CodeInstruction code, LocalBuilder variable = null)
		{
			return CodeInstructionExtensions.storeVarCodes.Contains(code.opcode) && (variable == null || object.Equals(variable, code.operand));
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00015D28 File Offset: 0x00013F28
		public static bool Branches(this CodeInstruction code, out Label? label)
		{
			if (CodeInstructionExtensions.branchCodes.Contains(code.opcode))
			{
				label = new Label?((Label)code.operand);
				return true;
			}
			label = null;
			return false;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00015D5C File Offset: 0x00013F5C
		public static bool Calls(this CodeInstruction code, MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			return (!(code.opcode != OpCodes.Call) || !(code.opcode != OpCodes.Callvirt)) && object.Equals(code.operand, method);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00015DA9 File Offset: 0x00013FA9
		public static bool LoadsConstant(this CodeInstruction code)
		{
			return CodeInstructionExtensions.constantLoadingCodes.Contains(code.opcode);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00015DBC File Offset: 0x00013FBC
		public static bool LoadsConstant(this CodeInstruction code, long number)
		{
			OpCode opcode = code.opcode;
			return (number == -1L && opcode == OpCodes.Ldc_I4_M1) || (number == 0L && opcode == OpCodes.Ldc_I4_0) || (number == 1L && opcode == OpCodes.Ldc_I4_1) || (number == 2L && opcode == OpCodes.Ldc_I4_2) || (number == 3L && opcode == OpCodes.Ldc_I4_3) || (number == 4L && opcode == OpCodes.Ldc_I4_4) || (number == 5L && opcode == OpCodes.Ldc_I4_5) || (number == 6L && opcode == OpCodes.Ldc_I4_6) || (number == 7L && opcode == OpCodes.Ldc_I4_7) || (number == 8L && opcode == OpCodes.Ldc_I4_8) || ((!(opcode != OpCodes.Ldc_I4) || !(opcode != OpCodes.Ldc_I4_S) || !(opcode != OpCodes.Ldc_I8)) && Convert.ToInt64(code.operand) == number);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00015ECD File Offset: 0x000140CD
		public static bool LoadsConstant(this CodeInstruction code, double number)
		{
			return (!(code.opcode != OpCodes.Ldc_R4) || !(code.opcode != OpCodes.Ldc_R8)) && Convert.ToDouble(code.operand) == number;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00015F03 File Offset: 0x00014103
		public static bool LoadsConstant(this CodeInstruction code, Enum e)
		{
			return code.LoadsConstant(Convert.ToInt64(e));
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00015F14 File Offset: 0x00014114
		public static bool LoadsField(this CodeInstruction code, FieldInfo field, bool byAddress = false)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			OpCode opCode = (field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld);
			if (!byAddress && code.opcode == opCode && object.Equals(code.operand, field))
			{
				return true;
			}
			OpCode opCode2 = (field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda);
			return byAddress && code.opcode == opCode2 && object.Equals(code.operand, field);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00015F9C File Offset: 0x0001419C
		public static bool StoresField(this CodeInstruction code, FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			OpCode opCode = (field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld);
			return code.opcode == opCode && object.Equals(code.operand, field);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00015FE8 File Offset: 0x000141E8
		public static CodeInstruction WithLabels(this CodeInstruction code, params Label[] labels)
		{
			code.labels.AddRange(labels);
			return code;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00015FE8 File Offset: 0x000141E8
		public static CodeInstruction WithLabels(this CodeInstruction code, IEnumerable<Label> labels)
		{
			code.labels.AddRange(labels);
			return code;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00015FF7 File Offset: 0x000141F7
		public static List<Label> ExtractLabels(this CodeInstruction code)
		{
			List<Label> list = new List<Label>(code.labels);
			code.labels.Clear();
			return list;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001600F File Offset: 0x0001420F
		public static CodeInstruction MoveLabelsTo(this CodeInstruction code, CodeInstruction other)
		{
			other.WithLabels(code.ExtractLabels());
			return code;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001601F File Offset: 0x0001421F
		public static CodeInstruction MoveLabelsFrom(this CodeInstruction code, CodeInstruction other)
		{
			return code.WithLabels(other.ExtractLabels());
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001602D File Offset: 0x0001422D
		public static CodeInstruction WithBlocks(this CodeInstruction code, params ExceptionBlock[] blocks)
		{
			code.blocks.AddRange(blocks);
			return code;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001602D File Offset: 0x0001422D
		public static CodeInstruction WithBlocks(this CodeInstruction code, IEnumerable<ExceptionBlock> blocks)
		{
			code.blocks.AddRange(blocks);
			return code;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001603C File Offset: 0x0001423C
		public static List<ExceptionBlock> ExtractBlocks(this CodeInstruction code)
		{
			List<ExceptionBlock> list = new List<ExceptionBlock>(code.blocks);
			code.blocks.Clear();
			return list;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00016054 File Offset: 0x00014254
		public static CodeInstruction MoveBlocksTo(this CodeInstruction code, CodeInstruction other)
		{
			other.WithBlocks(code.ExtractBlocks());
			return code;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00016064 File Offset: 0x00014264
		public static CodeInstruction MoveBlocksFrom(this CodeInstruction code, CodeInstruction other)
		{
			return code.WithBlocks(other.ExtractBlocks());
		}

		// Token: 0x04000201 RID: 513
		private static readonly HashSet<OpCode> loadVarCodes = new HashSet<OpCode>
		{
			OpCodes.Ldloc_0,
			OpCodes.Ldloc_1,
			OpCodes.Ldloc_2,
			OpCodes.Ldloc_3,
			OpCodes.Ldloc,
			OpCodes.Ldloca,
			OpCodes.Ldloc_S,
			OpCodes.Ldloca_S
		};

		// Token: 0x04000202 RID: 514
		private static readonly HashSet<OpCode> storeVarCodes = new HashSet<OpCode>
		{
			OpCodes.Stloc_0,
			OpCodes.Stloc_1,
			OpCodes.Stloc_2,
			OpCodes.Stloc_3,
			OpCodes.Stloc,
			OpCodes.Stloc_S
		};

		// Token: 0x04000203 RID: 515
		private static readonly HashSet<OpCode> branchCodes = new HashSet<OpCode>
		{
			OpCodes.Br_S,
			OpCodes.Brfalse_S,
			OpCodes.Brtrue_S,
			OpCodes.Beq_S,
			OpCodes.Bge_S,
			OpCodes.Bgt_S,
			OpCodes.Ble_S,
			OpCodes.Blt_S,
			OpCodes.Bne_Un_S,
			OpCodes.Bge_Un_S,
			OpCodes.Bgt_Un_S,
			OpCodes.Ble_Un_S,
			OpCodes.Blt_Un_S,
			OpCodes.Br,
			OpCodes.Brfalse,
			OpCodes.Brtrue,
			OpCodes.Beq,
			OpCodes.Bge,
			OpCodes.Bgt,
			OpCodes.Ble,
			OpCodes.Blt,
			OpCodes.Bne_Un,
			OpCodes.Bge_Un,
			OpCodes.Bgt_Un,
			OpCodes.Ble_Un,
			OpCodes.Blt_Un
		};

		// Token: 0x04000204 RID: 516
		private static readonly HashSet<OpCode> constantLoadingCodes = new HashSet<OpCode>
		{
			OpCodes.Ldc_I4_M1,
			OpCodes.Ldc_I4_0,
			OpCodes.Ldc_I4_1,
			OpCodes.Ldc_I4_2,
			OpCodes.Ldc_I4_3,
			OpCodes.Ldc_I4_4,
			OpCodes.Ldc_I4_5,
			OpCodes.Ldc_I4_6,
			OpCodes.Ldc_I4_7,
			OpCodes.Ldc_I4_8,
			OpCodes.Ldc_I4,
			OpCodes.Ldc_I4_S,
			OpCodes.Ldc_I8,
			OpCodes.Ldc_R4,
			OpCodes.Ldc_R8
		};
	}
}
