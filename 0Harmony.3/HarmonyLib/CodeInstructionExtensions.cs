using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200009E RID: 158
	public static class CodeInstructionExtensions
	{
		// Token: 0x06000338 RID: 824 RVA: 0x0000FF6C File Offset: 0x0000E16C
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

		// Token: 0x06000339 RID: 825 RVA: 0x0000FFF8 File Offset: 0x0000E1F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool OperandIs(this CodeInstruction code, MemberInfo value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return object.Equals(code.operand, value);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00010014 File Offset: 0x0000E214
		public static bool Is(this CodeInstruction code, OpCode opcode, object operand)
		{
			return code.opcode == opcode && code.OperandIs(operand);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0001002D File Offset: 0x0000E22D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool Is(this CodeInstruction code, OpCode opcode, MemberInfo operand)
		{
			return code.opcode == opcode && code.OperandIs(operand);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00010048 File Offset: 0x0000E248
		public static bool IsLdarg(this CodeInstruction code, int? n = null)
		{
			return ((n == null || n.Value == 0) && code.opcode == OpCodes.Ldarg_0) || ((n == null || n.Value == 1) && code.opcode == OpCodes.Ldarg_1) || ((n == null || n.Value == 2) && code.opcode == OpCodes.Ldarg_2) || ((n == null || n.Value == 3) && code.opcode == OpCodes.Ldarg_3) || (code.opcode == OpCodes.Ldarg && (n == null || n.Value == Convert.ToInt32(code.operand))) || (code.opcode == OpCodes.Ldarg_S && (n == null || n.Value == Convert.ToInt32(code.operand)));
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00010154 File Offset: 0x0000E354
		public static bool IsLdarga(this CodeInstruction code, int? n = null)
		{
			return (!(code.opcode != OpCodes.Ldarga) || !(code.opcode != OpCodes.Ldarga_S)) && (n == null || n.Value == Convert.ToInt32(code.operand));
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000101A8 File Offset: 0x0000E3A8
		public static bool IsStarg(this CodeInstruction code, int? n = null)
		{
			return (!(code.opcode != OpCodes.Starg) || !(code.opcode != OpCodes.Starg_S)) && (n == null || n.Value == Convert.ToInt32(code.operand));
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000101FA File Offset: 0x0000E3FA
		public static bool IsLdloc(this CodeInstruction code, LocalBuilder variable = null)
		{
			return CodeInstructionExtensions.loadVarCodes.Contains(code.opcode) && (variable == null || object.Equals(variable, code.operand));
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00010221 File Offset: 0x0000E421
		public static bool IsStloc(this CodeInstruction code, LocalBuilder variable = null)
		{
			return CodeInstructionExtensions.storeVarCodes.Contains(code.opcode) && (variable == null || object.Equals(variable, code.operand));
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00010248 File Offset: 0x0000E448
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

		// Token: 0x06000342 RID: 834 RVA: 0x0001027C File Offset: 0x0000E47C
		public static bool Calls(this CodeInstruction code, MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			return (!(code.opcode != OpCodes.Call) || !(code.opcode != OpCodes.Callvirt)) && object.Equals(code.operand, method);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x000102C9 File Offset: 0x0000E4C9
		public static bool LoadsConstant(this CodeInstruction code)
		{
			return CodeInstructionExtensions.constantLoadingCodes.Contains(code.opcode);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x000102DC File Offset: 0x0000E4DC
		public static bool LoadsConstant(this CodeInstruction code, long number)
		{
			OpCode opcode = code.opcode;
			return (number == -1L && opcode == OpCodes.Ldc_I4_M1) || (number == 0L && opcode == OpCodes.Ldc_I4_0) || (number == 1L && opcode == OpCodes.Ldc_I4_1) || (number == 2L && opcode == OpCodes.Ldc_I4_2) || (number == 3L && opcode == OpCodes.Ldc_I4_3) || (number == 4L && opcode == OpCodes.Ldc_I4_4) || (number == 5L && opcode == OpCodes.Ldc_I4_5) || (number == 6L && opcode == OpCodes.Ldc_I4_6) || (number == 7L && opcode == OpCodes.Ldc_I4_7) || (number == 8L && opcode == OpCodes.Ldc_I4_8) || ((!(opcode != OpCodes.Ldc_I4) || !(opcode != OpCodes.Ldc_I4_S) || !(opcode != OpCodes.Ldc_I8)) && Convert.ToInt64(code.operand) == number);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000103ED File Offset: 0x0000E5ED
		public static bool LoadsConstant(this CodeInstruction code, double number)
		{
			return (!(code.opcode != OpCodes.Ldc_R4) || !(code.opcode != OpCodes.Ldc_R8)) && Convert.ToDouble(code.operand) == number;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00010423 File Offset: 0x0000E623
		public static bool LoadsConstant(this CodeInstruction code, Enum e)
		{
			return code.LoadsConstant(Convert.ToInt64(e));
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00010434 File Offset: 0x0000E634
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

		// Token: 0x06000348 RID: 840 RVA: 0x000104BC File Offset: 0x0000E6BC
		public static bool StoresField(this CodeInstruction code, FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			OpCode opCode = (field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld);
			return code.opcode == opCode && object.Equals(code.operand, field);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00010508 File Offset: 0x0000E708
		public static CodeInstruction WithLabels(this CodeInstruction code, params Label[] labels)
		{
			code.labels.AddRange(labels);
			return code;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00010508 File Offset: 0x0000E708
		public static CodeInstruction WithLabels(this CodeInstruction code, IEnumerable<Label> labels)
		{
			code.labels.AddRange(labels);
			return code;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00010517 File Offset: 0x0000E717
		public static List<Label> ExtractLabels(this CodeInstruction code)
		{
			List<Label> list = new List<Label>(code.labels);
			code.labels.Clear();
			return list;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0001052F File Offset: 0x0000E72F
		public static CodeInstruction MoveLabelsTo(this CodeInstruction code, CodeInstruction other)
		{
			other.WithLabels(code.ExtractLabels());
			return code;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0001053F File Offset: 0x0000E73F
		public static CodeInstruction MoveLabelsFrom(this CodeInstruction code, CodeInstruction other)
		{
			return code.WithLabels(other.ExtractLabels());
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0001054D File Offset: 0x0000E74D
		public static CodeInstruction WithBlocks(this CodeInstruction code, params ExceptionBlock[] blocks)
		{
			code.blocks.AddRange(blocks);
			return code;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0001054D File Offset: 0x0000E74D
		public static CodeInstruction WithBlocks(this CodeInstruction code, IEnumerable<ExceptionBlock> blocks)
		{
			code.blocks.AddRange(blocks);
			return code;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001055C File Offset: 0x0000E75C
		public static List<ExceptionBlock> ExtractBlocks(this CodeInstruction code)
		{
			List<ExceptionBlock> list = new List<ExceptionBlock>(code.blocks);
			code.blocks.Clear();
			return list;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00010574 File Offset: 0x0000E774
		public static CodeInstruction MoveBlocksTo(this CodeInstruction code, CodeInstruction other)
		{
			other.WithBlocks(code.ExtractBlocks());
			return code;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00010584 File Offset: 0x0000E784
		public static CodeInstruction MoveBlocksFrom(this CodeInstruction code, CodeInstruction other)
		{
			return code.WithBlocks(other.ExtractBlocks());
		}

		// Token: 0x040001D5 RID: 469
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

		// Token: 0x040001D6 RID: 470
		private static readonly HashSet<OpCode> storeVarCodes = new HashSet<OpCode>
		{
			OpCodes.Stloc_0,
			OpCodes.Stloc_1,
			OpCodes.Stloc_2,
			OpCodes.Stloc_3,
			OpCodes.Stloc,
			OpCodes.Stloc_S
		};

		// Token: 0x040001D7 RID: 471
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

		// Token: 0x040001D8 RID: 472
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
