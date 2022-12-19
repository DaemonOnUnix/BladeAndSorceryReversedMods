using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200000F RID: 15
	internal class CodeTranspiler
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002EA4 File Offset: 0x000010A4
		internal CodeTranspiler(List<ILInstruction> ilInstructions, bool argumentShift)
		{
			this.argumentShift = argumentShift;
			this.codeInstructions = ilInstructions.Select((ILInstruction ilInstruction) => ilInstruction.GetCodeInstruction()).ToList<CodeInstruction>().AsEnumerable<CodeInstruction>();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002EFE File Offset: 0x000010FE
		internal void Add(MethodInfo transpiler)
		{
			this.transpilers.Add(transpiler);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002F0C File Offset: 0x0000110C
		internal static object ConvertInstruction(Type type, object instruction, out Dictionary<string, object> unassigned)
		{
			Dictionary<string, object> nonExisting = new Dictionary<string, object>();
			object obj = AccessTools.MakeDeepCopy(instruction, type, delegate(string namePath, Traverse trvSrc, Traverse trvDest)
			{
				object value = trvSrc.GetValue();
				if (!trvDest.FieldExists())
				{
					nonExisting[namePath] = value;
					return null;
				}
				if (namePath == "opcode")
				{
					return CodeTranspiler.ReplaceShortJumps((OpCode)value);
				}
				return value;
			}, "");
			unassigned = nonExisting;
			return obj;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002F4C File Offset: 0x0000114C
		internal static bool ShouldAddExceptionInfo(object op, int opIndex, List<object> originalInstructions, List<object> newInstructions, Dictionary<object, Dictionary<string, object>> unassignedValues)
		{
			int num = originalInstructions.IndexOf(op);
			if (num == -1)
			{
				return false;
			}
			Dictionary<string, object> unassigned;
			if (!unassignedValues.TryGetValue(op, out unassigned))
			{
				return false;
			}
			object blocksObject;
			if (!unassigned.TryGetValue("blocks", out blocksObject))
			{
				return false;
			}
			List<ExceptionBlock> blocks = blocksObject as List<ExceptionBlock>;
			if (newInstructions.Count((object instr) => instr == op) <= 1)
			{
				return true;
			}
			ExceptionBlock exceptionBlock = blocks.FirstOrDefault((ExceptionBlock block) => block.blockType != ExceptionBlockType.EndExceptionBlock);
			ExceptionBlock exceptionBlock2 = blocks.FirstOrDefault((ExceptionBlock block) => block.blockType == ExceptionBlockType.EndExceptionBlock);
			if (exceptionBlock != null && exceptionBlock2 == null)
			{
				object obj = originalInstructions.Skip(num + 1).FirstOrDefault(delegate(object instr)
				{
					if (!unassignedValues.TryGetValue(instr, out unassigned))
					{
						return false;
					}
					if (!unassigned.TryGetValue("blocks", out blocksObject))
					{
						return false;
					}
					blocks = blocksObject as List<ExceptionBlock>;
					return blocks.Any<ExceptionBlock>();
				});
				if (obj != null)
				{
					int num2 = num + 1;
					int num3 = num2 + originalInstructions.Skip(num2).ToList<object>().IndexOf(obj) - 1;
					IEnumerable<object> enumerable = originalInstructions.GetRange(num2, num3 - num2).Intersect(newInstructions);
					obj = newInstructions.Skip(opIndex + 1).FirstOrDefault(delegate(object instr)
					{
						if (!unassignedValues.TryGetValue(instr, out unassigned))
						{
							return false;
						}
						if (!unassigned.TryGetValue("blocks", out blocksObject))
						{
							return false;
						}
						blocks = blocksObject as List<ExceptionBlock>;
						return blocks.Any<ExceptionBlock>();
					});
					if (obj != null)
					{
						num2 = opIndex + 1;
						num3 = num2 + newInstructions.Skip(opIndex + 1).ToList<object>().IndexOf(obj) - 1;
						List<object> range = newInstructions.GetRange(num2, num3 - num2);
						return !enumerable.Except(range).ToList<object>().Any<object>();
					}
				}
			}
			if (exceptionBlock == null && exceptionBlock2 != null)
			{
				object obj2 = originalInstructions.GetRange(0, num).LastOrDefault(delegate(object instr)
				{
					if (!unassignedValues.TryGetValue(instr, out unassigned))
					{
						return false;
					}
					if (!unassigned.TryGetValue("blocks", out blocksObject))
					{
						return false;
					}
					blocks = blocksObject as List<ExceptionBlock>;
					return blocks.Any<ExceptionBlock>();
				});
				if (obj2 != null)
				{
					int num4 = originalInstructions.GetRange(0, num).LastIndexOf(obj2);
					int num5 = num;
					IEnumerable<object> enumerable2 = originalInstructions.GetRange(num4, num5 - num4).Intersect(newInstructions);
					obj2 = newInstructions.GetRange(0, opIndex).LastOrDefault(delegate(object instr)
					{
						if (!unassignedValues.TryGetValue(instr, out unassigned))
						{
							return false;
						}
						if (!unassigned.TryGetValue("blocks", out blocksObject))
						{
							return false;
						}
						blocks = blocksObject as List<ExceptionBlock>;
						return blocks.Any<ExceptionBlock>();
					});
					if (obj2 != null)
					{
						num4 = newInstructions.GetRange(0, opIndex).LastIndexOf(obj2);
						List<object> range2 = newInstructions.GetRange(num4, opIndex - num4);
						return !enumerable2.Except(range2).Any<object>();
					}
				}
			}
			return true;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000031A8 File Offset: 0x000013A8
		internal static IEnumerable ConvertInstructionsAndUnassignedValues(Type type, IEnumerable enumerable, out Dictionary<object, Dictionary<string, object>> unassignedValues)
		{
			Assembly assembly = type.GetGenericTypeDefinition().Assembly;
			Type type2 = assembly.GetType(typeof(List<>).FullName);
			Type type3 = type.GetGenericArguments()[0];
			Type type4 = type2.MakeGenericType(new Type[] { type3 });
			object obj = Activator.CreateInstance(assembly.GetType(type4.FullName));
			MethodInfo method = obj.GetType().GetMethod("Add");
			unassignedValues = new Dictionary<object, Dictionary<string, object>>();
			foreach (object obj2 in enumerable)
			{
				Dictionary<string, object> dictionary;
				object obj3 = CodeTranspiler.ConvertInstruction(type3, obj2, out dictionary);
				unassignedValues.Add(obj3, dictionary);
				method.Invoke(obj, new object[] { obj3 });
			}
			return obj as IEnumerable;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003290 File Offset: 0x00001490
		internal static IEnumerable ConvertToOurInstructions(IEnumerable instructions, Type codeInstructionType, List<object> originalInstructions, Dictionary<object, Dictionary<string, object>> unassignedValues)
		{
			List<object> newInstructions = instructions.Cast<object>().ToList<object>();
			int index = -1;
			foreach (object obj in newInstructions)
			{
				int num = index;
				index = num + 1;
				object obj2 = AccessTools.MakeDeepCopy(obj, codeInstructionType, null, "");
				Dictionary<string, object> dictionary;
				if (unassignedValues.TryGetValue(obj, out dictionary))
				{
					bool flag = CodeTranspiler.ShouldAddExceptionInfo(obj, index, originalInstructions, newInstructions, unassignedValues);
					Traverse traverse = Traverse.Create(obj2);
					foreach (KeyValuePair<string, object> keyValuePair in dictionary)
					{
						if (flag || keyValuePair.Key != "blocks")
						{
							traverse.Field(keyValuePair.Key).SetValue(keyValuePair.Value);
						}
					}
				}
				yield return obj2;
			}
			List<object>.Enumerator enumerator = default(List<object>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000032B5 File Offset: 0x000014B5
		private static bool IsCodeInstructionsParameter(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition().Name.StartsWith("IEnumerable", StringComparison.Ordinal);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000032D8 File Offset: 0x000014D8
		internal static IEnumerable ConvertToGeneralInstructions(MethodInfo transpiler, IEnumerable enumerable, out Dictionary<object, Dictionary<string, object>> unassignedValues)
		{
			Type type = (from p in transpiler.GetParameters()
				select p.ParameterType).FirstOrDefault((Type t) => CodeTranspiler.IsCodeInstructionsParameter(t));
			if (type == typeof(IEnumerable<CodeInstruction>))
			{
				unassignedValues = null;
				IList<CodeInstruction> list;
				if ((list = enumerable as IList<CodeInstruction>) == null)
				{
					list = ((enumerable as IEnumerable<CodeInstruction>) ?? enumerable.Cast<CodeInstruction>()).ToList<CodeInstruction>();
				}
				return list;
			}
			return CodeTranspiler.ConvertInstructionsAndUnassignedValues(type, enumerable, out unassignedValues);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003374 File Offset: 0x00001574
		internal static List<object> GetTranspilerCallParameters(ILGenerator generator, MethodInfo transpiler, MethodBase method, IEnumerable instructions)
		{
			List<object> parameter = new List<object>();
			(from param in transpiler.GetParameters()
				select param.ParameterType).Do(delegate(Type type)
			{
				if (type.IsAssignableFrom(typeof(ILGenerator)))
				{
					parameter.Add(generator);
					return;
				}
				if (type.IsAssignableFrom(typeof(MethodBase)))
				{
					parameter.Add(method);
					return;
				}
				if (CodeTranspiler.IsCodeInstructionsParameter(type))
				{
					parameter.Add(instructions);
				}
			});
			return parameter;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000033E8 File Offset: 0x000015E8
		internal List<CodeInstruction> GetResult(ILGenerator generator, MethodBase method)
		{
			IEnumerable instructions = this.codeInstructions;
			this.transpilers.ForEach(delegate(MethodInfo transpiler)
			{
				Dictionary<object, Dictionary<string, object>> dictionary;
				instructions = CodeTranspiler.ConvertToGeneralInstructions(transpiler, instructions, out dictionary);
				List<object> list2 = null;
				if (dictionary != null)
				{
					list2 = instructions.Cast<object>().ToList<object>();
				}
				List<object> transpilerCallParameters = CodeTranspiler.GetTranspilerCallParameters(generator, transpiler, method, instructions);
				IEnumerable enumerable = transpiler.Invoke(null, transpilerCallParameters.ToArray()) as IEnumerable;
				if (enumerable != null)
				{
					instructions = enumerable;
				}
				if (dictionary != null)
				{
					instructions = CodeTranspiler.ConvertToOurInstructions(instructions, typeof(CodeInstruction), list2, dictionary);
				}
			});
			List<CodeInstruction> list = (instructions as List<CodeInstruction>) ?? instructions.Cast<CodeInstruction>().ToList<CodeInstruction>();
			if (this.argumentShift)
			{
				StructReturnBuffer.ArgumentShifter(list, method.IsStatic && AccessTools.IsMonoRuntime);
			}
			return list;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003470 File Offset: 0x00001670
		private static OpCode ReplaceShortJumps(OpCode opcode)
		{
			foreach (KeyValuePair<OpCode, OpCode> keyValuePair in CodeTranspiler.allJumpCodes)
			{
				if (opcode == keyValuePair.Key)
				{
					return keyValuePair.Value;
				}
			}
			return opcode;
		}

		// Token: 0x0400001D RID: 29
		private readonly IEnumerable<CodeInstruction> codeInstructions;

		// Token: 0x0400001E RID: 30
		private readonly bool argumentShift;

		// Token: 0x0400001F RID: 31
		private readonly List<MethodInfo> transpilers = new List<MethodInfo>();

		// Token: 0x04000020 RID: 32
		private static readonly Dictionary<OpCode, OpCode> allJumpCodes = new Dictionary<OpCode, OpCode>
		{
			{
				OpCodes.Beq_S,
				OpCodes.Beq
			},
			{
				OpCodes.Bge_S,
				OpCodes.Bge
			},
			{
				OpCodes.Bge_Un_S,
				OpCodes.Bge_Un
			},
			{
				OpCodes.Bgt_S,
				OpCodes.Bgt
			},
			{
				OpCodes.Bgt_Un_S,
				OpCodes.Bgt_Un
			},
			{
				OpCodes.Ble_S,
				OpCodes.Ble
			},
			{
				OpCodes.Ble_Un_S,
				OpCodes.Ble_Un
			},
			{
				OpCodes.Blt_S,
				OpCodes.Blt
			},
			{
				OpCodes.Blt_Un_S,
				OpCodes.Blt_Un
			},
			{
				OpCodes.Bne_Un_S,
				OpCodes.Bne_Un
			},
			{
				OpCodes.Brfalse_S,
				OpCodes.Brfalse
			},
			{
				OpCodes.Brtrue_S,
				OpCodes.Brtrue
			},
			{
				OpCodes.Br_S,
				OpCodes.Br
			},
			{
				OpCodes.Leave_S,
				OpCodes.Leave
			}
		};
	}
}
