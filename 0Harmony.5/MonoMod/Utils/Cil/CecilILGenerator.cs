using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace MonoMod.Utils.Cil
{
	// Token: 0x0200045A RID: 1114
	public sealed class CecilILGenerator : ILGeneratorShim
	{
		// Token: 0x060017BA RID: 6074 RVA: 0x00053008 File Offset: 0x00051208
		unsafe static CecilILGenerator()
		{
			FieldInfo[] fields = typeof(Mono.Cecil.Cil.OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				Mono.Cecil.Cil.OpCode opCode = (Mono.Cecil.Cil.OpCode)fields[i].GetValue(null);
				CecilILGenerator._MCCOpCodes[opCode.Value] = opCode;
			}
			Label label = default(Label);
			*(int*)(&label) = -1;
			CecilILGenerator.NullLabel = label;
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x000530EC File Offset: 0x000512EC
		public CecilILGenerator(ILProcessor il)
		{
			this.IL = il;
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x0005313D File Offset: 0x0005133D
		private Mono.Cecil.Cil.OpCode _(System.Reflection.Emit.OpCode opcode)
		{
			return CecilILGenerator._MCCOpCodes[opcode.Value];
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x00053150 File Offset: 0x00051350
		private CecilILGenerator.LabelInfo _(Label handle)
		{
			CecilILGenerator.LabelInfo labelInfo;
			if (!this._LabelInfos.TryGetValue(handle, out labelInfo))
			{
				return null;
			}
			return labelInfo;
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00053170 File Offset: 0x00051370
		private VariableDefinition _(LocalBuilder handle)
		{
			return this._Variables[handle];
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0005317E File Offset: 0x0005137E
		private TypeReference _(Type info)
		{
			return this.IL.Body.Method.Module.ImportReference(info);
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0005319B File Offset: 0x0005139B
		private FieldReference _(FieldInfo info)
		{
			return this.IL.Body.Method.Module.ImportReference(info);
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x000531B8 File Offset: 0x000513B8
		private MethodReference _(MethodBase info)
		{
			return this.IL.Body.Method.Module.ImportReference(info);
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x060017C2 RID: 6082 RVA: 0x000531D5 File Offset: 0x000513D5
		public override int ILOffset
		{
			get
			{
				return this._ILOffset;
			}
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x000531E0 File Offset: 0x000513E0
		private Instruction ProcessLabels(Instruction ins)
		{
			if (this._LabelsToMark.Count != 0)
			{
				foreach (CecilILGenerator.LabelInfo labelInfo in this._LabelsToMark)
				{
					foreach (Instruction instruction in labelInfo.Branches)
					{
						object operand = instruction.Operand;
						if (!(operand is Instruction))
						{
							Instruction[] array = operand as Instruction[];
							if (array != null)
							{
								for (int i = 0; i < array.Length; i++)
								{
									if (array[i] == labelInfo.Instruction)
									{
										array[i] = ins;
										break;
									}
								}
							}
						}
						else
						{
							instruction.Operand = ins;
						}
					}
					labelInfo.Emitted = true;
					labelInfo.Instruction = ins;
				}
				this._LabelsToMark.Clear();
			}
			if (this._ExceptionHandlersToMark.Count != 0)
			{
				foreach (CecilILGenerator.LabelledExceptionHandler labelledExceptionHandler in this._ExceptionHandlersToMark)
				{
					Collection<Mono.Cecil.Cil.ExceptionHandler> exceptionHandlers = this.IL.Body.ExceptionHandlers;
					Mono.Cecil.Cil.ExceptionHandler exceptionHandler = new Mono.Cecil.Cil.ExceptionHandler(labelledExceptionHandler.HandlerType);
					CecilILGenerator.LabelInfo labelInfo2 = this._(labelledExceptionHandler.TryStart);
					exceptionHandler.TryStart = ((labelInfo2 != null) ? labelInfo2.Instruction : null);
					CecilILGenerator.LabelInfo labelInfo3 = this._(labelledExceptionHandler.TryEnd);
					exceptionHandler.TryEnd = ((labelInfo3 != null) ? labelInfo3.Instruction : null);
					CecilILGenerator.LabelInfo labelInfo4 = this._(labelledExceptionHandler.HandlerStart);
					exceptionHandler.HandlerStart = ((labelInfo4 != null) ? labelInfo4.Instruction : null);
					CecilILGenerator.LabelInfo labelInfo5 = this._(labelledExceptionHandler.HandlerEnd);
					exceptionHandler.HandlerEnd = ((labelInfo5 != null) ? labelInfo5.Instruction : null);
					CecilILGenerator.LabelInfo labelInfo6 = this._(labelledExceptionHandler.FilterStart);
					exceptionHandler.FilterStart = ((labelInfo6 != null) ? labelInfo6.Instruction : null);
					exceptionHandler.CatchType = labelledExceptionHandler.ExceptionType;
					exceptionHandlers.Add(exceptionHandler);
				}
				this._ExceptionHandlersToMark.Clear();
			}
			return ins;
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00053410 File Offset: 0x00051610
		public unsafe override Label DefineLabel()
		{
			Label label = default(Label);
			ref int ptr = ref *(int*)(&label);
			int num = this.labelCounter;
			this.labelCounter = num + 1;
			ptr = num;
			this._LabelInfos[label] = new CecilILGenerator.LabelInfo();
			return label;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0005344C File Offset: 0x0005164C
		public override void MarkLabel(Label loc)
		{
			CecilILGenerator.LabelInfo labelInfo;
			if (!this._LabelInfos.TryGetValue(loc, out labelInfo) || labelInfo.Emitted)
			{
				return;
			}
			this._LabelsToMark.Add(labelInfo);
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x0005347E File Offset: 0x0005167E
		public override LocalBuilder DeclareLocal(Type type)
		{
			return this.DeclareLocal(type, false);
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00053488 File Offset: 0x00051688
		public override LocalBuilder DeclareLocal(Type type, bool pinned)
		{
			int count = this.IL.Body.Variables.Count;
			object obj;
			if (CecilILGenerator.c_LocalBuilder_params != 4)
			{
				if (CecilILGenerator.c_LocalBuilder_params != 3)
				{
					if (CecilILGenerator.c_LocalBuilder_params != 2)
					{
						if (CecilILGenerator.c_LocalBuilder_params != 0)
						{
							throw new NotSupportedException();
						}
						obj = CecilILGenerator.c_LocalBuilder.Invoke(new object[0]);
					}
					else
					{
						ConstructorInfo constructorInfo = CecilILGenerator.c_LocalBuilder;
						object[] array = new object[2];
						array[0] = type;
						obj = constructorInfo.Invoke(array);
					}
				}
				else
				{
					ConstructorInfo constructorInfo2 = CecilILGenerator.c_LocalBuilder;
					object[] array2 = new object[3];
					array2[0] = count;
					array2[1] = type;
					obj = constructorInfo2.Invoke(array2);
				}
			}
			else
			{
				obj = CecilILGenerator.c_LocalBuilder.Invoke(new object[] { count, type, null, pinned });
			}
			LocalBuilder localBuilder = (LocalBuilder)obj;
			FieldInfo fieldInfo = CecilILGenerator.f_LocalBuilder_position;
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(localBuilder, (ushort)count);
			}
			FieldInfo fieldInfo2 = CecilILGenerator.f_LocalBuilder_is_pinned;
			if (fieldInfo2 != null)
			{
				fieldInfo2.SetValue(localBuilder, pinned);
			}
			TypeReference typeReference = this._(type);
			if (pinned)
			{
				typeReference = new PinnedType(typeReference);
			}
			VariableDefinition variableDefinition = new VariableDefinition(typeReference);
			this.IL.Body.Variables.Add(variableDefinition);
			this._Variables[localBuilder] = variableDefinition;
			return localBuilder;
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x000535AF File Offset: 0x000517AF
		private void Emit(Instruction ins)
		{
			ins.Offset = this._ILOffset;
			this._ILOffset += ins.GetSize();
			this.IL.Append(this.ProcessLabels(ins));
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x000535E2 File Offset: 0x000517E2
		public override void Emit(System.Reflection.Emit.OpCode opcode)
		{
			this.Emit(this.IL.Create(this._(opcode)));
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000535FC File Offset: 0x000517FC
		public override void Emit(System.Reflection.Emit.OpCode opcode, byte arg)
		{
			if (opcode.OperandType == System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), (int)arg);
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0005363C File Offset: 0x0005183C
		public override void Emit(System.Reflection.Emit.OpCode opcode, sbyte arg)
		{
			if (opcode.OperandType == System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), (int)arg);
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0005367C File Offset: 0x0005187C
		public override void Emit(System.Reflection.Emit.OpCode opcode, short arg)
		{
			if (opcode.OperandType == System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), (int)arg);
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), (int)arg));
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x000536BC File Offset: 0x000518BC
		public override void Emit(System.Reflection.Emit.OpCode opcode, int arg)
		{
			if (opcode.OperandType == System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), arg);
				return;
			}
			if (opcode.Name.EndsWith(".s", StringComparison.Ordinal))
			{
				this.Emit(this.IL.Create(this._(opcode), (sbyte)arg));
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00053736 File Offset: 0x00051936
		public override void Emit(System.Reflection.Emit.OpCode opcode, long arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00053751 File Offset: 0x00051951
		public override void Emit(System.Reflection.Emit.OpCode opcode, float arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x0005376C File Offset: 0x0005196C
		public override void Emit(System.Reflection.Emit.OpCode opcode, double arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00053787 File Offset: 0x00051987
		public override void Emit(System.Reflection.Emit.OpCode opcode, string arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x000537A2 File Offset: 0x000519A2
		public override void Emit(System.Reflection.Emit.OpCode opcode, Type arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000537C3 File Offset: 0x000519C3
		public override void Emit(System.Reflection.Emit.OpCode opcode, FieldInfo arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x000537E4 File Offset: 0x000519E4
		public override void Emit(System.Reflection.Emit.OpCode opcode, ConstructorInfo arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x000537E4 File Offset: 0x000519E4
		public override void Emit(System.Reflection.Emit.OpCode opcode, MethodInfo arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00053808 File Offset: 0x00051A08
		public override void Emit(System.Reflection.Emit.OpCode opcode, Label label)
		{
			CecilILGenerator.LabelInfo labelInfo = this._(label);
			Instruction instruction = this.IL.Create(this._(opcode), this._(label).Instruction);
			labelInfo.Branches.Add(instruction);
			this.Emit(this.ProcessLabels(instruction));
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00053854 File Offset: 0x00051A54
		public override void Emit(System.Reflection.Emit.OpCode opcode, Label[] labels)
		{
			IEnumerable<CecilILGenerator.LabelInfo> enumerable = labels.Distinct<Label>().Select(new Func<Label, CecilILGenerator.LabelInfo>(this._));
			Instruction instruction = this.IL.Create(this._(opcode), enumerable.Select((CecilILGenerator.LabelInfo labelInfo) => labelInfo.Instruction).ToArray<Instruction>());
			foreach (CecilILGenerator.LabelInfo labelInfo2 in enumerable)
			{
				labelInfo2.Branches.Add(instruction);
			}
			this.Emit(this.ProcessLabels(instruction));
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00053904 File Offset: 0x00051B04
		public override void Emit(System.Reflection.Emit.OpCode opcode, LocalBuilder local)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(local)));
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00053925 File Offset: 0x00051B25
		public override void Emit(System.Reflection.Emit.OpCode opcode, SignatureHelper signature)
		{
			this.Emit(this.IL.Create(this._(opcode), this.IL.Body.Method.Module.ImportCallSite(signature)));
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x0005395A File Offset: 0x00051B5A
		public void Emit(System.Reflection.Emit.OpCode opcode, ICallSiteGenerator signature)
		{
			this.Emit(this.IL.Create(this._(opcode), this.IL.Body.Method.Module.ImportCallSite(signature)));
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00053990 File Offset: 0x00051B90
		private void _EmitInlineVar(Mono.Cecil.Cil.OpCode opcode, int index)
		{
			switch (opcode.OperandType)
			{
			case Mono.Cecil.Cil.OperandType.InlineVar:
			case Mono.Cecil.Cil.OperandType.ShortInlineVar:
				this.Emit(this.IL.Create(opcode, this.IL.Body.Variables[index]));
				return;
			case Mono.Cecil.Cil.OperandType.InlineArg:
			case Mono.Cecil.Cil.OperandType.ShortInlineArg:
				this.Emit(this.IL.Create(opcode, this.IL.Body.Method.Parameters[index]));
				return;
			}
			throw new NotSupportedException(string.Format("Unsupported SRE InlineVar -> Cecil {0} for {1} {2}", opcode.OperandType, opcode, index));
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x000537E4 File Offset: 0x000519E4
		public override void EmitCall(System.Reflection.Emit.OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(methodInfo)));
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00003A32 File Offset: 0x00001C32
		public override void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00003A32 File Offset: 0x00001C32
		public override void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x00053A4C File Offset: 0x00051C4C
		public override void EmitWriteLine(FieldInfo field)
		{
			if (field.IsStatic)
			{
				this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldsfld, this._(field)));
			}
			else
			{
				this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldarg_0));
				this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldfld, this._(field)));
			}
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Call, this._(typeof(Console).GetMethod("WriteLine", new Type[] { field.FieldType }))));
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00053AF4 File Offset: 0x00051CF4
		public override void EmitWriteLine(LocalBuilder localBuilder)
		{
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldloc, this._(localBuilder)));
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Call, this._(typeof(Console).GetMethod("WriteLine", new Type[] { localBuilder.LocalType }))));
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00053B60 File Offset: 0x00051D60
		public override void EmitWriteLine(string value)
		{
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldstr, value));
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Call, this._(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }))));
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00053BC7 File Offset: 0x00051DC7
		public override void ThrowException(Type type)
		{
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Newobj, this._(type.GetConstructor(Type.EmptyTypes))));
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Throw));
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00053C08 File Offset: 0x00051E08
		public override Label BeginExceptionBlock()
		{
			CecilILGenerator.ExceptionHandlerChain exceptionHandlerChain = new CecilILGenerator.ExceptionHandlerChain(this);
			this._ExceptionHandlers.Push(exceptionHandlerChain);
			return exceptionHandlerChain.SkipAll;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00053C2E File Offset: 0x00051E2E
		public override void BeginCatchBlock(Type exceptionType)
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Catch).ExceptionType = ((exceptionType == null) ? null : this._(exceptionType));
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00053C59 File Offset: 0x00051E59
		public override void BeginExceptFilterBlock()
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Filter);
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00053C6D File Offset: 0x00051E6D
		public override void BeginFaultBlock()
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Fault);
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00053C81 File Offset: 0x00051E81
		public override void BeginFinallyBlock()
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Finally);
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00053C95 File Offset: 0x00051E95
		public override void EndExceptionBlock()
		{
			this._ExceptionHandlers.Pop().End();
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x00018105 File Offset: 0x00016305
		public override void BeginScope()
		{
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00018105 File Offset: 0x00016305
		public override void EndScope()
		{
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00018105 File Offset: 0x00016305
		public override void UsingNamespace(string usingNamespace)
		{
		}

		// Token: 0x04001062 RID: 4194
		private static readonly ConstructorInfo c_LocalBuilder = (from c in typeof(LocalBuilder).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			orderby c.GetParameters().Length descending
			select c).First<ConstructorInfo>();

		// Token: 0x04001063 RID: 4195
		private static readonly FieldInfo f_LocalBuilder_position = typeof(LocalBuilder).GetField("position", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001064 RID: 4196
		private static readonly FieldInfo f_LocalBuilder_is_pinned = typeof(LocalBuilder).GetField("is_pinned", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001065 RID: 4197
		private static int c_LocalBuilder_params = CecilILGenerator.c_LocalBuilder.GetParameters().Length;

		// Token: 0x04001066 RID: 4198
		private static readonly Dictionary<short, Mono.Cecil.Cil.OpCode> _MCCOpCodes = new Dictionary<short, Mono.Cecil.Cil.OpCode>();

		// Token: 0x04001067 RID: 4199
		private static Label NullLabel;

		// Token: 0x04001068 RID: 4200
		public readonly ILProcessor IL;

		// Token: 0x04001069 RID: 4201
		private readonly Dictionary<Label, CecilILGenerator.LabelInfo> _LabelInfos = new Dictionary<Label, CecilILGenerator.LabelInfo>();

		// Token: 0x0400106A RID: 4202
		private readonly List<CecilILGenerator.LabelInfo> _LabelsToMark = new List<CecilILGenerator.LabelInfo>();

		// Token: 0x0400106B RID: 4203
		private readonly List<CecilILGenerator.LabelledExceptionHandler> _ExceptionHandlersToMark = new List<CecilILGenerator.LabelledExceptionHandler>();

		// Token: 0x0400106C RID: 4204
		private readonly Dictionary<LocalBuilder, VariableDefinition> _Variables = new Dictionary<LocalBuilder, VariableDefinition>();

		// Token: 0x0400106D RID: 4205
		private readonly Stack<CecilILGenerator.ExceptionHandlerChain> _ExceptionHandlers = new Stack<CecilILGenerator.ExceptionHandlerChain>();

		// Token: 0x0400106E RID: 4206
		private int labelCounter;

		// Token: 0x0400106F RID: 4207
		private int _ILOffset;

		// Token: 0x0200045B RID: 1115
		private class LabelInfo
		{
			// Token: 0x04001070 RID: 4208
			public bool Emitted;

			// Token: 0x04001071 RID: 4209
			public Instruction Instruction = Instruction.Create(Mono.Cecil.Cil.OpCodes.Nop);

			// Token: 0x04001072 RID: 4210
			public readonly List<Instruction> Branches = new List<Instruction>();
		}

		// Token: 0x0200045C RID: 1116
		private class LabelledExceptionHandler
		{
			// Token: 0x04001073 RID: 4211
			public Label TryStart = CecilILGenerator.NullLabel;

			// Token: 0x04001074 RID: 4212
			public Label TryEnd = CecilILGenerator.NullLabel;

			// Token: 0x04001075 RID: 4213
			public Label HandlerStart = CecilILGenerator.NullLabel;

			// Token: 0x04001076 RID: 4214
			public Label HandlerEnd = CecilILGenerator.NullLabel;

			// Token: 0x04001077 RID: 4215
			public Label FilterStart = CecilILGenerator.NullLabel;

			// Token: 0x04001078 RID: 4216
			public ExceptionHandlerType HandlerType;

			// Token: 0x04001079 RID: 4217
			public TypeReference ExceptionType;
		}

		// Token: 0x0200045D RID: 1117
		private class ExceptionHandlerChain
		{
			// Token: 0x060017EE RID: 6126 RVA: 0x00053D09 File Offset: 0x00051F09
			public ExceptionHandlerChain(CecilILGenerator il)
			{
				this.IL = il;
				this._Start = il.DefineLabel();
				il.MarkLabel(this._Start);
				this.SkipAll = il.DefineLabel();
			}

			// Token: 0x060017EF RID: 6127 RVA: 0x00053D3C File Offset: 0x00051F3C
			public CecilILGenerator.LabelledExceptionHandler BeginHandler(ExceptionHandlerType type)
			{
				CecilILGenerator.LabelledExceptionHandler labelledExceptionHandler = (this._Prev = this._Handler);
				if (labelledExceptionHandler != null)
				{
					this.EndHandler(labelledExceptionHandler);
				}
				this.IL.Emit(System.Reflection.Emit.OpCodes.Leave, this._SkipHandler = this.IL.DefineLabel());
				Label label = this.IL.DefineLabel();
				this.IL.MarkLabel(label);
				CecilILGenerator.LabelledExceptionHandler labelledExceptionHandler2 = new CecilILGenerator.LabelledExceptionHandler();
				labelledExceptionHandler2.TryStart = this._Start;
				labelledExceptionHandler2.TryEnd = label;
				labelledExceptionHandler2.HandlerType = type;
				labelledExceptionHandler2.HandlerEnd = this._SkipHandler;
				CecilILGenerator.LabelledExceptionHandler labelledExceptionHandler3 = labelledExceptionHandler2;
				this._Handler = labelledExceptionHandler2;
				CecilILGenerator.LabelledExceptionHandler labelledExceptionHandler4 = labelledExceptionHandler3;
				if (type == ExceptionHandlerType.Filter)
				{
					labelledExceptionHandler4.FilterStart = label;
				}
				else
				{
					labelledExceptionHandler4.HandlerStart = label;
				}
				return labelledExceptionHandler4;
			}

			// Token: 0x060017F0 RID: 6128 RVA: 0x00053DEC File Offset: 0x00051FEC
			public void EndHandler(CecilILGenerator.LabelledExceptionHandler handler)
			{
				Label skipHandler = this._SkipHandler;
				ExceptionHandlerType handlerType = handler.HandlerType;
				if (handlerType != ExceptionHandlerType.Filter)
				{
					if (handlerType != ExceptionHandlerType.Finally)
					{
						this.IL.Emit(System.Reflection.Emit.OpCodes.Leave, skipHandler);
					}
					else
					{
						this.IL.Emit(System.Reflection.Emit.OpCodes.Endfinally);
					}
				}
				else
				{
					this.IL.Emit(System.Reflection.Emit.OpCodes.Endfilter);
				}
				this.IL.MarkLabel(skipHandler);
				this.IL._ExceptionHandlersToMark.Add(handler);
			}

			// Token: 0x060017F1 RID: 6129 RVA: 0x00053E63 File Offset: 0x00052063
			public void End()
			{
				this.EndHandler(this._Handler);
				this.IL.MarkLabel(this.SkipAll);
			}

			// Token: 0x0400107A RID: 4218
			private readonly CecilILGenerator IL;

			// Token: 0x0400107B RID: 4219
			private readonly Label _Start;

			// Token: 0x0400107C RID: 4220
			public readonly Label SkipAll;

			// Token: 0x0400107D RID: 4221
			private Label _SkipHandler;

			// Token: 0x0400107E RID: 4222
			private CecilILGenerator.LabelledExceptionHandler _Prev;

			// Token: 0x0400107F RID: 4223
			private CecilILGenerator.LabelledExceptionHandler _Handler;
		}
	}
}
