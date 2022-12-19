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
	// Token: 0x0200035B RID: 859
	public sealed class CecilILGenerator : ILGeneratorShim
	{
		// Token: 0x06001411 RID: 5137 RVA: 0x0004A08C File Offset: 0x0004828C
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

		// Token: 0x06001412 RID: 5138 RVA: 0x0004A170 File Offset: 0x00048370
		public CecilILGenerator(ILProcessor il)
		{
			this.IL = il;
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0004A1C1 File Offset: 0x000483C1
		private Mono.Cecil.Cil.OpCode _(System.Reflection.Emit.OpCode opcode)
		{
			return CecilILGenerator._MCCOpCodes[opcode.Value];
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0004A1D4 File Offset: 0x000483D4
		private CecilILGenerator.LabelInfo _(Label handle)
		{
			CecilILGenerator.LabelInfo labelInfo;
			if (!this._LabelInfos.TryGetValue(handle, out labelInfo))
			{
				return null;
			}
			return labelInfo;
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0004A1F4 File Offset: 0x000483F4
		private VariableDefinition _(LocalBuilder handle)
		{
			return this._Variables[handle];
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0004A202 File Offset: 0x00048402
		private TypeReference _(Type info)
		{
			return this.IL.Body.Method.Module.ImportReference(info);
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0004A21F File Offset: 0x0004841F
		private FieldReference _(FieldInfo info)
		{
			return this.IL.Body.Method.Module.ImportReference(info);
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0004A23C File Offset: 0x0004843C
		private MethodReference _(MethodBase info)
		{
			return this.IL.Body.Method.Module.ImportReference(info);
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x0004A259 File Offset: 0x00048459
		public override int ILOffset
		{
			get
			{
				return this._ILOffset;
			}
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0004A264 File Offset: 0x00048464
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

		// Token: 0x0600141B RID: 5147 RVA: 0x0004A494 File Offset: 0x00048694
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

		// Token: 0x0600141C RID: 5148 RVA: 0x0004A4D0 File Offset: 0x000486D0
		public override void MarkLabel(Label loc)
		{
			CecilILGenerator.LabelInfo labelInfo;
			if (!this._LabelInfos.TryGetValue(loc, out labelInfo) || labelInfo.Emitted)
			{
				return;
			}
			this._LabelsToMark.Add(labelInfo);
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0004A502 File Offset: 0x00048702
		public override LocalBuilder DeclareLocal(Type type)
		{
			return this.DeclareLocal(type, false);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0004A50C File Offset: 0x0004870C
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

		// Token: 0x0600141F RID: 5151 RVA: 0x0004A633 File Offset: 0x00048833
		private void Emit(Instruction ins)
		{
			ins.Offset = this._ILOffset;
			this._ILOffset += ins.GetSize();
			this.IL.Append(this.ProcessLabels(ins));
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0004A666 File Offset: 0x00048866
		public override void Emit(System.Reflection.Emit.OpCode opcode)
		{
			this.Emit(this.IL.Create(this._(opcode)));
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0004A680 File Offset: 0x00048880
		public override void Emit(System.Reflection.Emit.OpCode opcode, byte arg)
		{
			if (opcode.OperandType == System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), (int)arg);
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0004A6C0 File Offset: 0x000488C0
		public override void Emit(System.Reflection.Emit.OpCode opcode, sbyte arg)
		{
			if (opcode.OperandType == System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), (int)arg);
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0004A700 File Offset: 0x00048900
		public override void Emit(System.Reflection.Emit.OpCode opcode, short arg)
		{
			if (opcode.OperandType == System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), (int)arg);
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), (int)arg));
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0004A740 File Offset: 0x00048940
		public override void Emit(System.Reflection.Emit.OpCode opcode, int arg)
		{
			if (opcode.OperandType == System.Reflection.Emit.OperandType.ShortInlineVar || opcode.OperandType == System.Reflection.Emit.OperandType.InlineVar)
			{
				this._EmitInlineVar(this._(opcode), arg);
				return;
			}
			if (opcode.Name.EndsWith(".s"))
			{
				this.Emit(this.IL.Create(this._(opcode), (sbyte)arg));
				return;
			}
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0004A7B9 File Offset: 0x000489B9
		public override void Emit(System.Reflection.Emit.OpCode opcode, long arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0004A7D4 File Offset: 0x000489D4
		public override void Emit(System.Reflection.Emit.OpCode opcode, float arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0004A7EF File Offset: 0x000489EF
		public override void Emit(System.Reflection.Emit.OpCode opcode, double arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0004A80A File Offset: 0x00048A0A
		public override void Emit(System.Reflection.Emit.OpCode opcode, string arg)
		{
			this.Emit(this.IL.Create(this._(opcode), arg));
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0004A825 File Offset: 0x00048A25
		public override void Emit(System.Reflection.Emit.OpCode opcode, Type arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0004A846 File Offset: 0x00048A46
		public override void Emit(System.Reflection.Emit.OpCode opcode, FieldInfo arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0004A867 File Offset: 0x00048A67
		public override void Emit(System.Reflection.Emit.OpCode opcode, ConstructorInfo arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0004A867 File Offset: 0x00048A67
		public override void Emit(System.Reflection.Emit.OpCode opcode, MethodInfo arg)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(arg)));
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0004A888 File Offset: 0x00048A88
		public override void Emit(System.Reflection.Emit.OpCode opcode, Label label)
		{
			CecilILGenerator.LabelInfo labelInfo = this._(label);
			Instruction instruction = this.IL.Create(this._(opcode), this._(label).Instruction);
			labelInfo.Branches.Add(instruction);
			this.Emit(this.ProcessLabels(instruction));
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0004A8D4 File Offset: 0x00048AD4
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

		// Token: 0x0600142F RID: 5167 RVA: 0x0004A984 File Offset: 0x00048B84
		public override void Emit(System.Reflection.Emit.OpCode opcode, LocalBuilder local)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(local)));
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0004A9A5 File Offset: 0x00048BA5
		public override void Emit(System.Reflection.Emit.OpCode opcode, SignatureHelper signature)
		{
			this.Emit(this.IL.Create(this._(opcode), this.IL.Body.Method.Module.ImportCallSite(signature)));
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0004A9DA File Offset: 0x00048BDA
		public void Emit(System.Reflection.Emit.OpCode opcode, ICallSiteGenerator signature)
		{
			this.Emit(this.IL.Create(this._(opcode), this.IL.Body.Method.Module.ImportCallSite(signature)));
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0004AA10 File Offset: 0x00048C10
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

		// Token: 0x06001433 RID: 5171 RVA: 0x0004A867 File Offset: 0x00048A67
		public override void EmitCall(System.Reflection.Emit.OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			this.Emit(this.IL.Create(this._(opcode), this._(methodInfo)));
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x000039F6 File Offset: 0x00001BF6
		public override void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x000039F6 File Offset: 0x00001BF6
		public override void EmitCalli(System.Reflection.Emit.OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0004AACC File Offset: 0x00048CCC
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

		// Token: 0x06001437 RID: 5175 RVA: 0x0004AB74 File Offset: 0x00048D74
		public override void EmitWriteLine(LocalBuilder localBuilder)
		{
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldloc, this._(localBuilder)));
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Call, this._(typeof(Console).GetMethod("WriteLine", new Type[] { localBuilder.LocalType }))));
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0004ABE0 File Offset: 0x00048DE0
		public override void EmitWriteLine(string value)
		{
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Ldstr, value));
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Call, this._(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }))));
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0004AC47 File Offset: 0x00048E47
		public override void ThrowException(Type type)
		{
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Newobj, this._(type.GetConstructor(Type.EmptyTypes))));
			this.Emit(this.IL.Create(Mono.Cecil.Cil.OpCodes.Throw));
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0004AC88 File Offset: 0x00048E88
		public override Label BeginExceptionBlock()
		{
			CecilILGenerator.ExceptionHandlerChain exceptionHandlerChain = new CecilILGenerator.ExceptionHandlerChain(this);
			this._ExceptionHandlers.Push(exceptionHandlerChain);
			return exceptionHandlerChain.SkipAll;
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0004ACAE File Offset: 0x00048EAE
		public override void BeginCatchBlock(Type exceptionType)
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Catch).ExceptionType = ((exceptionType == null) ? null : this._(exceptionType));
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0004ACD9 File Offset: 0x00048ED9
		public override void BeginExceptFilterBlock()
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Filter);
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0004ACED File Offset: 0x00048EED
		public override void BeginFaultBlock()
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Fault);
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0004AD01 File Offset: 0x00048F01
		public override void BeginFinallyBlock()
		{
			this._ExceptionHandlers.Peek().BeginHandler(ExceptionHandlerType.Finally);
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0004AD15 File Offset: 0x00048F15
		public override void EndExceptionBlock()
		{
			this._ExceptionHandlers.Pop().End();
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x00012279 File Offset: 0x00010479
		public override void BeginScope()
		{
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x00012279 File Offset: 0x00010479
		public override void EndScope()
		{
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00012279 File Offset: 0x00010479
		public override void UsingNamespace(string usingNamespace)
		{
		}

		// Token: 0x04001000 RID: 4096
		private static readonly ConstructorInfo c_LocalBuilder = (from c in typeof(LocalBuilder).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			orderby c.GetParameters().Length descending
			select c).First<ConstructorInfo>();

		// Token: 0x04001001 RID: 4097
		private static readonly FieldInfo f_LocalBuilder_position = typeof(LocalBuilder).GetField("position", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001002 RID: 4098
		private static readonly FieldInfo f_LocalBuilder_is_pinned = typeof(LocalBuilder).GetField("is_pinned", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001003 RID: 4099
		private static int c_LocalBuilder_params = CecilILGenerator.c_LocalBuilder.GetParameters().Length;

		// Token: 0x04001004 RID: 4100
		private static readonly Dictionary<short, Mono.Cecil.Cil.OpCode> _MCCOpCodes = new Dictionary<short, Mono.Cecil.Cil.OpCode>();

		// Token: 0x04001005 RID: 4101
		private static Label NullLabel;

		// Token: 0x04001006 RID: 4102
		public readonly ILProcessor IL;

		// Token: 0x04001007 RID: 4103
		private readonly Dictionary<Label, CecilILGenerator.LabelInfo> _LabelInfos = new Dictionary<Label, CecilILGenerator.LabelInfo>();

		// Token: 0x04001008 RID: 4104
		private readonly List<CecilILGenerator.LabelInfo> _LabelsToMark = new List<CecilILGenerator.LabelInfo>();

		// Token: 0x04001009 RID: 4105
		private readonly List<CecilILGenerator.LabelledExceptionHandler> _ExceptionHandlersToMark = new List<CecilILGenerator.LabelledExceptionHandler>();

		// Token: 0x0400100A RID: 4106
		private readonly Dictionary<LocalBuilder, VariableDefinition> _Variables = new Dictionary<LocalBuilder, VariableDefinition>();

		// Token: 0x0400100B RID: 4107
		private readonly Stack<CecilILGenerator.ExceptionHandlerChain> _ExceptionHandlers = new Stack<CecilILGenerator.ExceptionHandlerChain>();

		// Token: 0x0400100C RID: 4108
		private int labelCounter;

		// Token: 0x0400100D RID: 4109
		private int _ILOffset;

		// Token: 0x0200035C RID: 860
		private class LabelInfo
		{
			// Token: 0x0400100E RID: 4110
			public bool Emitted;

			// Token: 0x0400100F RID: 4111
			public Instruction Instruction = Instruction.Create(Mono.Cecil.Cil.OpCodes.Nop);

			// Token: 0x04001010 RID: 4112
			public readonly List<Instruction> Branches = new List<Instruction>();
		}

		// Token: 0x0200035D RID: 861
		private class LabelledExceptionHandler
		{
			// Token: 0x04001011 RID: 4113
			public Label TryStart = CecilILGenerator.NullLabel;

			// Token: 0x04001012 RID: 4114
			public Label TryEnd = CecilILGenerator.NullLabel;

			// Token: 0x04001013 RID: 4115
			public Label HandlerStart = CecilILGenerator.NullLabel;

			// Token: 0x04001014 RID: 4116
			public Label HandlerEnd = CecilILGenerator.NullLabel;

			// Token: 0x04001015 RID: 4117
			public Label FilterStart = CecilILGenerator.NullLabel;

			// Token: 0x04001016 RID: 4118
			public ExceptionHandlerType HandlerType;

			// Token: 0x04001017 RID: 4119
			public TypeReference ExceptionType;
		}

		// Token: 0x0200035E RID: 862
		private class ExceptionHandlerChain
		{
			// Token: 0x06001445 RID: 5189 RVA: 0x0004AD89 File Offset: 0x00048F89
			public ExceptionHandlerChain(CecilILGenerator il)
			{
				this.IL = il;
				this._Start = il.DefineLabel();
				il.MarkLabel(this._Start);
				this.SkipAll = il.DefineLabel();
			}

			// Token: 0x06001446 RID: 5190 RVA: 0x0004ADBC File Offset: 0x00048FBC
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

			// Token: 0x06001447 RID: 5191 RVA: 0x0004AE6C File Offset: 0x0004906C
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

			// Token: 0x06001448 RID: 5192 RVA: 0x0004AEE3 File Offset: 0x000490E3
			public void End()
			{
				this.EndHandler(this._Handler);
				this.IL.MarkLabel(this.SkipAll);
			}

			// Token: 0x04001018 RID: 4120
			private readonly CecilILGenerator IL;

			// Token: 0x04001019 RID: 4121
			private readonly Label _Start;

			// Token: 0x0400101A RID: 4122
			public readonly Label SkipAll;

			// Token: 0x0400101B RID: 4123
			private Label _SkipHandler;

			// Token: 0x0400101C RID: 4124
			private CecilILGenerator.LabelledExceptionHandler _Prev;

			// Token: 0x0400101D RID: 4125
			private CecilILGenerator.LabelledExceptionHandler _Handler;
		}
	}
}
