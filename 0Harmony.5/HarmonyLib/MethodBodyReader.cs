﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using MonoMod.Utils;
using MonoMod.Utils.Cil;

namespace HarmonyLib
{
	// Token: 0x02000021 RID: 33
	internal class MethodBodyReader
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x000055CE File Offset: 0x000037CE
		internal static List<ILInstruction> GetInstructions(ILGenerator generator, MethodBase method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			MethodBodyReader methodBodyReader = new MethodBodyReader(method, generator);
			methodBodyReader.DeclareVariables(null);
			methodBodyReader.ReadInstructions();
			return methodBodyReader.ilInstructions;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000055F8 File Offset: 0x000037F8
		internal MethodBodyReader(MethodBase method, ILGenerator generator)
		{
			this.generator = generator;
			this.method = method;
			this.module = method.Module;
			MethodBody methodBody = method.GetMethodBody();
			int? num;
			if (methodBody == null)
			{
				num = null;
			}
			else
			{
				byte[] ilasByteArray = methodBody.GetILAsByteArray();
				num = ((ilasByteArray != null) ? new int?(ilasByteArray.Length) : null);
			}
			int? num2 = num;
			if (num2.GetValueOrDefault() == 0)
			{
				this.ilBytes = new ByteBuffer(new byte[0]);
				this.ilInstructions = new List<ILInstruction>();
			}
			else
			{
				byte[] ilasByteArray2 = methodBody.GetILAsByteArray();
				if (ilasByteArray2 == null)
				{
					throw new ArgumentException("Can not get IL bytes of method " + method.FullDescription());
				}
				this.ilBytes = new ByteBuffer(ilasByteArray2);
				this.ilInstructions = new List<ILInstruction>((ilasByteArray2.Length + 1) / 2);
			}
			Type declaringType = method.DeclaringType;
			if (declaringType != null && declaringType.IsGenericType)
			{
				try
				{
					this.typeArguments = declaringType.GetGenericArguments();
				}
				catch
				{
					this.typeArguments = null;
				}
			}
			if (method.IsGenericMethod)
			{
				try
				{
					this.methodArguments = method.GetGenericArguments();
				}
				catch
				{
					this.methodArguments = null;
				}
			}
			if (!method.IsStatic)
			{
				this.this_parameter = new MethodBodyReader.ThisParameter(method);
			}
			this.parameters = method.GetParameters();
			List<LocalVariableInfo> list;
			if (methodBody == null)
			{
				list = null;
			}
			else
			{
				IList<LocalVariableInfo> list2 = methodBody.LocalVariables;
				list = ((list2 != null) ? list2.ToList<LocalVariableInfo>() : null);
			}
			this.localVariables = list ?? new List<LocalVariableInfo>();
			this.exceptions = ((methodBody != null) ? methodBody.ExceptionHandlingClauses : null) ?? new List<ExceptionHandlingClause>();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005784 File Offset: 0x00003984
		internal void SetDebugging(bool debug)
		{
			this.debug = debug;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000578D File Offset: 0x0000398D
		internal void SetArgumentShift(bool argumentShift)
		{
			this.argumentShift = argumentShift;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005798 File Offset: 0x00003998
		internal void ReadInstructions()
		{
			while (this.ilBytes.position < this.ilBytes.buffer.Length)
			{
				int position = this.ilBytes.position;
				ILInstruction ilinstruction = new ILInstruction(this.ReadOpCode(), null)
				{
					offset = position
				};
				this.ReadOperand(ilinstruction);
				this.ilInstructions.Add(ilinstruction);
			}
			this.ResolveBranches();
			this.ParseExceptions();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005800 File Offset: 0x00003A00
		internal void DeclareVariables(LocalBuilder[] existingVariables)
		{
			if (this.generator == null)
			{
				return;
			}
			if (existingVariables != null)
			{
				this.variables = existingVariables;
				return;
			}
			this.variables = this.localVariables.Select((LocalVariableInfo lvi) => this.generator.DeclareLocal(lvi.LocalType, lvi.IsPinned)).ToArray<LocalBuilder>();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005838 File Offset: 0x00003A38
		private void ResolveBranches()
		{
			foreach (ILInstruction ilinstruction in this.ilInstructions)
			{
				OperandType operandType = ilinstruction.opcode.OperandType;
				if (operandType != OperandType.InlineBrTarget)
				{
					if (operandType == OperandType.InlineSwitch)
					{
						int[] array = (int[])ilinstruction.operand;
						ILInstruction[] array2 = new ILInstruction[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array2[i] = this.GetInstruction(array[i], false);
						}
						ilinstruction.operand = array2;
						continue;
					}
					if (operandType != OperandType.ShortInlineBrTarget)
					{
						continue;
					}
				}
				ilinstruction.operand = this.GetInstruction((int)ilinstruction.operand, false);
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000058FC File Offset: 0x00003AFC
		private void ParseExceptions()
		{
			foreach (ExceptionHandlingClause exceptionHandlingClause in this.exceptions)
			{
				int tryOffset = exceptionHandlingClause.TryOffset;
				int handlerOffset = exceptionHandlingClause.HandlerOffset;
				int num = exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength - 1;
				this.GetInstruction(tryOffset, false).blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock, null));
				this.GetInstruction(num, true).blocks.Add(new ExceptionBlock(ExceptionBlockType.EndExceptionBlock, null));
				switch (exceptionHandlingClause.Flags)
				{
				case ExceptionHandlingClauseOptions.Clause:
					this.GetInstruction(handlerOffset, false).blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginCatchBlock, exceptionHandlingClause.CatchType));
					break;
				case ExceptionHandlingClauseOptions.Filter:
					this.GetInstruction(exceptionHandlingClause.FilterOffset, false).blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptFilterBlock, null));
					break;
				case ExceptionHandlingClauseOptions.Finally:
					this.GetInstruction(handlerOffset, false).blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginFinallyBlock, null));
					break;
				case ExceptionHandlingClauseOptions.Fault:
					this.GetInstruction(handlerOffset, false).blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginFaultBlock, null));
					break;
				}
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005A40 File Offset: 0x00003C40
		internal List<CodeInstruction> FinalizeILCodes(Emitter emitter, List<MethodInfo> transpilers, List<Label> endLabels, out bool hasReturnCode)
		{
			hasReturnCode = false;
			if (this.generator == null)
			{
				return null;
			}
			Label label;
			foreach (ILInstruction ilinstruction in this.ilInstructions)
			{
				OperandType operandType = ilinstruction.opcode.OperandType;
				if (operandType != OperandType.InlineBrTarget)
				{
					if (operandType != OperandType.InlineSwitch)
					{
						if (operandType != OperandType.ShortInlineBrTarget)
						{
							continue;
						}
					}
					else
					{
						ILInstruction[] array = ilinstruction.operand as ILInstruction[];
						if (array != null)
						{
							List<Label> list = new List<Label>();
							foreach (ILInstruction ilinstruction2 in array)
							{
								label = this.generator.DefineLabel();
								ilinstruction2.labels.Add(label);
								list.Add(label);
							}
							ilinstruction.argument = list.ToArray();
							continue;
						}
						continue;
					}
				}
				ILInstruction ilinstruction3 = ilinstruction.operand as ILInstruction;
				if (ilinstruction3 != null)
				{
					Label label2 = this.generator.DefineLabel();
					ilinstruction3.labels.Add(label2);
					ilinstruction.argument = label2;
				}
			}
			CodeTranspiler codeTranspiler = new CodeTranspiler(this.ilInstructions, this.argumentShift);
			transpilers.Do(delegate(MethodInfo transpiler)
			{
				codeTranspiler.Add(transpiler);
			});
			List<CodeInstruction> result = codeTranspiler.GetResult(this.generator, this.method);
			if (emitter == null)
			{
				return result;
			}
			emitter.LogComment("start original");
			if (this.debug)
			{
				List<string> buffer = FileLog.GetBuffer(true);
				emitter.LogAllLocalVariables();
				FileLog.LogBuffered(buffer);
			}
			hasReturnCode = result.Any((CodeInstruction code) => code.opcode == OpCodes.Ret);
			for (;;)
			{
				CodeInstruction codeInstruction2 = result.LastOrDefault<CodeInstruction>();
				if (codeInstruction2 == null || codeInstruction2.opcode != OpCodes.Ret)
				{
					break;
				}
				endLabels.AddRange(codeInstruction2.labels);
				result.RemoveAt(result.Count - 1);
			}
			Action<Label> <>9__3;
			Action<ExceptionBlock> <>9__4;
			Action<ExceptionBlock> <>9__5;
			result.Do(delegate(CodeInstruction codeInstruction)
			{
				IEnumerable<Label> labels = codeInstruction.labels;
				Action<Label> action;
				if ((action = <>9__3) == null)
				{
					action = (<>9__3 = delegate(Label label)
					{
						emitter.MarkLabel(label);
					});
				}
				labels.Do(action);
				IEnumerable<ExceptionBlock> blocks = codeInstruction.blocks;
				Action<ExceptionBlock> action2;
				if ((action2 = <>9__4) == null)
				{
					action2 = (<>9__4 = delegate(ExceptionBlock block)
					{
						Label? label5;
						emitter.MarkBlockBefore(block, out label5);
					});
				}
				blocks.Do(action2);
				OpCode opCode = codeInstruction.opcode;
				object obj = codeInstruction.operand;
				if (opCode == OpCodes.Ret)
				{
					Label label3 = this.generator.DefineLabel();
					opCode = OpCodes.Br;
					obj = label3;
					endLabels.Add(label3);
				}
				OpCode opCode2;
				if (MethodBodyReader.shortJumps.TryGetValue(opCode, out opCode2))
				{
					opCode = opCode2;
				}
				OperandType operandType2 = opCode.OperandType;
				if (operandType2 != OperandType.InlineNone)
				{
					if (operandType2 != OperandType.InlineSig)
					{
						if (obj == null)
						{
							throw new Exception(string.Format("Wrong null argument: {0}", codeInstruction));
						}
						emitter.AddInstruction(opCode, obj);
						emitter.LogIL(opCode, obj, null);
						this.generator.DynEmit(opCode, obj);
					}
					else
					{
						CecilILGenerator proxiedShim = this.generator.GetProxiedShim<CecilILGenerator>();
						if (proxiedShim == null)
						{
							throw new NotSupportedException();
						}
						if (obj == null)
						{
							throw new Exception(string.Format("Wrong null argument: {0}", codeInstruction));
						}
						if (!(obj is ICallSiteGenerator))
						{
							throw new Exception(string.Format("Wrong Emit argument type {0} in {1}", obj.GetType(), codeInstruction));
						}
						emitter.AddInstruction(opCode, obj);
						emitter.LogIL(opCode, obj, null);
						proxiedShim.Emit(opCode, (ICallSiteGenerator)obj);
					}
				}
				else
				{
					emitter.Emit(opCode);
				}
				IEnumerable<ExceptionBlock> blocks2 = codeInstruction.blocks;
				Action<ExceptionBlock> action3;
				if ((action3 = <>9__5) == null)
				{
					action3 = (<>9__5 = delegate(ExceptionBlock block)
					{
						emitter.MarkBlockAfter(block);
					});
				}
				blocks2.Do(action3);
			});
			emitter.LogComment("end original");
			return result;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005C84 File Offset: 0x00003E84
		private static void GetMemberInfoValue(MemberInfo info, out object result)
		{
			result = null;
			MemberTypes memberType = info.MemberType;
			if (memberType <= MemberTypes.Method)
			{
				switch (memberType)
				{
				case MemberTypes.Constructor:
					result = (ConstructorInfo)info;
					return;
				case MemberTypes.Event:
					result = (EventInfo)info;
					return;
				case MemberTypes.Constructor | MemberTypes.Event:
					break;
				case MemberTypes.Field:
					result = (FieldInfo)info;
					return;
				default:
					if (memberType != MemberTypes.Method)
					{
						return;
					}
					result = (MethodInfo)info;
					return;
				}
			}
			else if (memberType != MemberTypes.Property)
			{
				if (memberType != MemberTypes.TypeInfo && memberType != MemberTypes.NestedType)
				{
					return;
				}
				result = (Type)info;
				return;
			}
			else
			{
				result = (PropertyInfo)info;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005D04 File Offset: 0x00003F04
		private void ReadOperand(ILInstruction instruction)
		{
			switch (instruction.opcode.OperandType)
			{
			case OperandType.InlineBrTarget:
			{
				int num = this.ilBytes.ReadInt32();
				instruction.operand = num + this.ilBytes.position;
				return;
			}
			case OperandType.InlineField:
			{
				int num2 = this.ilBytes.ReadInt32();
				instruction.operand = this.module.ResolveField(num2, this.typeArguments, this.methodArguments);
				Type declaringType = ((MemberInfo)instruction.operand).DeclaringType;
				if (declaringType != null)
				{
					declaringType.FixReflectionCacheAuto();
				}
				instruction.argument = (FieldInfo)instruction.operand;
				return;
			}
			case OperandType.InlineI:
			{
				int num3 = this.ilBytes.ReadInt32();
				instruction.operand = num3;
				instruction.argument = (int)instruction.operand;
				return;
			}
			case OperandType.InlineI8:
			{
				long num4 = this.ilBytes.ReadInt64();
				instruction.operand = num4;
				instruction.argument = (long)instruction.operand;
				return;
			}
			case OperandType.InlineMethod:
			{
				int num5 = this.ilBytes.ReadInt32();
				instruction.operand = this.module.ResolveMethod(num5, this.typeArguments, this.methodArguments);
				Type declaringType2 = ((MemberInfo)instruction.operand).DeclaringType;
				if (declaringType2 != null)
				{
					declaringType2.FixReflectionCacheAuto();
				}
				if (instruction.operand is ConstructorInfo)
				{
					instruction.argument = (ConstructorInfo)instruction.operand;
					return;
				}
				instruction.argument = (MethodInfo)instruction.operand;
				return;
			}
			case OperandType.InlineNone:
				instruction.argument = null;
				return;
			case OperandType.InlineR:
			{
				double num6 = this.ilBytes.ReadDouble();
				instruction.operand = num6;
				instruction.argument = (double)instruction.operand;
				return;
			}
			case OperandType.InlineSig:
			{
				int num7 = this.ilBytes.ReadInt32();
				byte[] array = this.module.ResolveSignature(num7);
				InlineSignature inlineSignature = InlineSignatureParser.ImportCallSite(this.module, array);
				instruction.operand = inlineSignature;
				instruction.argument = inlineSignature;
				Debugger.Log(0, "TEST", "METHOD " + this.method.FullDescription() + "\n");
				Debugger.Log(0, "TEST", "Signature Blob = " + array.Select((byte b) => string.Format("0x{0:x02}", b)).Aggregate((string a, string b) => a + " " + b) + "\n");
				Debugger.Log(0, "TEST", string.Format("Signature = {0}\n", inlineSignature));
				Debugger.Break();
				return;
			}
			case OperandType.InlineString:
			{
				int num8 = this.ilBytes.ReadInt32();
				instruction.operand = this.module.ResolveString(num8);
				instruction.argument = (string)instruction.operand;
				return;
			}
			case OperandType.InlineSwitch:
			{
				int num9 = this.ilBytes.ReadInt32();
				int num10 = this.ilBytes.position + 4 * num9;
				int[] array2 = new int[num9];
				for (int i = 0; i < num9; i++)
				{
					array2[i] = this.ilBytes.ReadInt32() + num10;
				}
				instruction.operand = array2;
				return;
			}
			case OperandType.InlineTok:
			{
				int num11 = this.ilBytes.ReadInt32();
				instruction.operand = this.module.ResolveMember(num11, this.typeArguments, this.methodArguments);
				Type declaringType3 = ((MemberInfo)instruction.operand).DeclaringType;
				if (declaringType3 != null)
				{
					declaringType3.FixReflectionCacheAuto();
				}
				MethodBodyReader.GetMemberInfoValue((MemberInfo)instruction.operand, out instruction.argument);
				return;
			}
			case OperandType.InlineType:
			{
				int num12 = this.ilBytes.ReadInt32();
				instruction.operand = this.module.ResolveType(num12, this.typeArguments, this.methodArguments);
				((Type)instruction.operand).FixReflectionCacheAuto();
				instruction.argument = (Type)instruction.operand;
				return;
			}
			case OperandType.InlineVar:
			{
				short num13 = this.ilBytes.ReadInt16();
				if (!MethodBodyReader.TargetsLocalVariable(instruction.opcode))
				{
					instruction.operand = this.GetParameter((int)num13);
					instruction.argument = num13;
					return;
				}
				LocalVariableInfo localVariable = this.GetLocalVariable((int)num13);
				if (localVariable == null)
				{
					instruction.argument = num13;
					return;
				}
				instruction.operand = localVariable;
				LocalBuilder[] array3 = this.variables;
				instruction.argument = ((array3 != null) ? array3[localVariable.LocalIndex] : null) ?? localVariable;
				return;
			}
			case OperandType.ShortInlineBrTarget:
			{
				sbyte b5 = (sbyte)this.ilBytes.ReadByte();
				instruction.operand = (int)b5 + this.ilBytes.position;
				return;
			}
			case OperandType.ShortInlineI:
			{
				if (instruction.opcode == OpCodes.Ldc_I4_S)
				{
					sbyte b2 = (sbyte)this.ilBytes.ReadByte();
					instruction.operand = b2;
					instruction.argument = (sbyte)instruction.operand;
					return;
				}
				byte b3 = this.ilBytes.ReadByte();
				instruction.operand = b3;
				instruction.argument = (byte)instruction.operand;
				return;
			}
			case OperandType.ShortInlineR:
			{
				float num14 = this.ilBytes.ReadSingle();
				instruction.operand = num14;
				instruction.argument = (float)instruction.operand;
				return;
			}
			case OperandType.ShortInlineVar:
			{
				byte b4 = this.ilBytes.ReadByte();
				if (!MethodBodyReader.TargetsLocalVariable(instruction.opcode))
				{
					instruction.operand = this.GetParameter((int)b4);
					instruction.argument = b4;
					return;
				}
				LocalVariableInfo localVariable2 = this.GetLocalVariable((int)b4);
				if (localVariable2 == null)
				{
					instruction.argument = b4;
					return;
				}
				instruction.operand = localVariable2;
				LocalBuilder[] array4 = this.variables;
				instruction.argument = ((array4 != null) ? array4[localVariable2.LocalIndex] : null) ?? localVariable2;
				return;
			}
			}
			throw new NotSupportedException();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000062E0 File Offset: 0x000044E0
		private ILInstruction GetInstruction(int offset, bool isEndOfInstruction)
		{
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", offset, string.Format("Instruction offset {0} is less than 0", offset));
			}
			int num = this.ilInstructions.Count - 1;
			ILInstruction ilinstruction = this.ilInstructions[num];
			if (offset > ilinstruction.offset + ilinstruction.GetSize() - 1)
			{
				throw new ArgumentOutOfRangeException("offset", offset, string.Format("Instruction offset {0} is outside valid range 0 - {1}", offset, ilinstruction.offset + ilinstruction.GetSize() - 1));
			}
			int i = 0;
			int num2 = num;
			while (i <= num2)
			{
				int num3 = i + (num2 - i) / 2;
				ilinstruction = this.ilInstructions[num3];
				if (isEndOfInstruction)
				{
					if (offset == ilinstruction.offset + ilinstruction.GetSize() - 1)
					{
						return ilinstruction;
					}
				}
				else if (offset == ilinstruction.offset)
				{
					return ilinstruction;
				}
				if (offset < ilinstruction.offset)
				{
					num2 = num3 - 1;
				}
				else
				{
					i = num3 + 1;
				}
			}
			throw new Exception(string.Format("Cannot find instruction for {0:X4}", offset));
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000063E1 File Offset: 0x000045E1
		private static bool TargetsLocalVariable(OpCode opcode)
		{
			return opcode.Name.Contains("loc");
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000063F4 File Offset: 0x000045F4
		private LocalVariableInfo GetLocalVariable(int index)
		{
			List<LocalVariableInfo> list = this.localVariables;
			if (list == null)
			{
				return null;
			}
			return list[index];
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006408 File Offset: 0x00004608
		private ParameterInfo GetParameter(int index)
		{
			if (index == 0)
			{
				return this.this_parameter;
			}
			return this.parameters[index - 1];
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006420 File Offset: 0x00004620
		private OpCode ReadOpCode()
		{
			byte b = this.ilBytes.ReadByte();
			if (b == 254)
			{
				return MethodBodyReader.two_bytes_opcodes[(int)this.ilBytes.ReadByte()];
			}
			return MethodBodyReader.one_byte_opcodes[(int)b];
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006464 File Offset: 0x00004664
		[MethodImpl(MethodImplOptions.Synchronized)]
		static MethodBodyReader()
		{
			FieldInfo[] fields = typeof(OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				OpCode opCode = (OpCode)fields[i].GetValue(null);
				if (opCode.OpCodeType != OpCodeType.Nternal)
				{
					if (opCode.Size == 1)
					{
						MethodBodyReader.one_byte_opcodes[(int)opCode.Value] = opCode;
					}
					else
					{
						MethodBodyReader.two_bytes_opcodes[(int)(opCode.Value & 255)] = opCode;
					}
				}
			}
		}

		// Token: 0x0400005C RID: 92
		private readonly ILGenerator generator;

		// Token: 0x0400005D RID: 93
		private readonly MethodBase method;

		// Token: 0x0400005E RID: 94
		private bool debug;

		// Token: 0x0400005F RID: 95
		private bool argumentShift;

		// Token: 0x04000060 RID: 96
		private readonly Module module;

		// Token: 0x04000061 RID: 97
		private readonly Type[] typeArguments;

		// Token: 0x04000062 RID: 98
		private readonly Type[] methodArguments;

		// Token: 0x04000063 RID: 99
		private readonly ByteBuffer ilBytes;

		// Token: 0x04000064 RID: 100
		private readonly ParameterInfo this_parameter;

		// Token: 0x04000065 RID: 101
		private readonly ParameterInfo[] parameters;

		// Token: 0x04000066 RID: 102
		private readonly IList<ExceptionHandlingClause> exceptions;

		// Token: 0x04000067 RID: 103
		private readonly List<ILInstruction> ilInstructions;

		// Token: 0x04000068 RID: 104
		private readonly List<LocalVariableInfo> localVariables;

		// Token: 0x04000069 RID: 105
		private LocalBuilder[] variables;

		// Token: 0x0400006A RID: 106
		private static readonly Dictionary<OpCode, OpCode> shortJumps = new Dictionary<OpCode, OpCode>
		{
			{
				OpCodes.Leave_S,
				OpCodes.Leave
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
				OpCodes.Beq_S,
				OpCodes.Beq
			},
			{
				OpCodes.Bge_S,
				OpCodes.Bge
			},
			{
				OpCodes.Bgt_S,
				OpCodes.Bgt
			},
			{
				OpCodes.Ble_S,
				OpCodes.Ble
			},
			{
				OpCodes.Blt_S,
				OpCodes.Blt
			},
			{
				OpCodes.Bne_Un_S,
				OpCodes.Bne_Un
			},
			{
				OpCodes.Bge_Un_S,
				OpCodes.Bge_Un
			},
			{
				OpCodes.Bgt_Un_S,
				OpCodes.Bgt_Un
			},
			{
				OpCodes.Ble_Un_S,
				OpCodes.Ble_Un
			},
			{
				OpCodes.Br_S,
				OpCodes.Br
			},
			{
				OpCodes.Blt_Un_S,
				OpCodes.Blt_Un
			}
		};

		// Token: 0x0400006B RID: 107
		private static readonly OpCode[] one_byte_opcodes = new OpCode[225];

		// Token: 0x0400006C RID: 108
		private static readonly OpCode[] two_bytes_opcodes = new OpCode[31];

		// Token: 0x02000022 RID: 34
		private class ThisParameter : ParameterInfo
		{
			// Token: 0x060000C5 RID: 197 RVA: 0x000065FE File Offset: 0x000047FE
			internal ThisParameter(MethodBase method)
			{
				this.MemberImpl = method;
				this.ClassImpl = method.DeclaringType;
				this.NameImpl = "this";
				this.PositionImpl = -1;
			}
		}
	}
}
