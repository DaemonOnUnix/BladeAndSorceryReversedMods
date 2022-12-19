using System;
using System.Collections.Generic;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001BB RID: 443
	internal sealed class CodeWriter : ByteBuffer
	{
		// Token: 0x06000DED RID: 3565 RVA: 0x000306E2 File Offset: 0x0002E8E2
		public CodeWriter(MetadataBuilder metadata)
			: base(0)
		{
			this.code_base = metadata.text_map.GetNextRVA(TextSegment.CLIHeader);
			this.metadata = metadata;
			this.standalone_signatures = new Dictionary<uint, MetadataToken>();
			this.tiny_method_bodies = new Dictionary<ByteBuffer, uint>(new ByteBufferEqualityComparer());
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00030720 File Offset: 0x0002E920
		public uint WriteMethodBody(MethodDefinition method)
		{
			uint num;
			if (CodeWriter.IsUnresolved(method))
			{
				if (method.rva == 0U)
				{
					return 0U;
				}
				num = this.WriteUnresolvedMethodBody(method);
			}
			else
			{
				if (CodeWriter.IsEmptyMethodBody(method.Body))
				{
					return 0U;
				}
				num = this.WriteResolvedMethodBody(method);
			}
			return num;
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00030761 File Offset: 0x0002E961
		private static bool IsEmptyMethodBody(MethodBody body)
		{
			return body.instructions.IsNullOrEmpty<Instruction>() && body.variables.IsNullOrEmpty<VariableDefinition>();
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0003077D File Offset: 0x0002E97D
		private static bool IsUnresolved(MethodDefinition method)
		{
			return method.HasBody && method.HasImage && method.body == null;
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0003079C File Offset: 0x0002E99C
		private uint WriteUnresolvedMethodBody(MethodDefinition method)
		{
			int num;
			MetadataToken metadataToken;
			ByteBuffer byteBuffer = this.metadata.module.reader.code.PatchRawMethodBody(method, this, out num, out metadataToken);
			bool flag = (byteBuffer.buffer[0] & 3) == 3;
			if (flag)
			{
				this.Align(4);
			}
			uint num2 = this.BeginMethod();
			if (flag || !this.GetOrMapTinyMethodBody(byteBuffer, ref num2))
			{
				base.WriteBytes(byteBuffer);
			}
			if (method.debug_info == null)
			{
				return num2;
			}
			ISymbolWriter symbol_writer = this.metadata.symbol_writer;
			if (symbol_writer != null)
			{
				method.debug_info.code_size = num;
				method.debug_info.local_var_token = metadataToken;
				symbol_writer.Write(method.debug_info);
			}
			return num2;
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00030840 File Offset: 0x0002EA40
		private uint WriteResolvedMethodBody(MethodDefinition method)
		{
			this.body = method.Body;
			this.ComputeHeader();
			uint num;
			if (this.RequiresFatHeader())
			{
				this.Align(4);
				num = this.BeginMethod();
				this.WriteFatHeader();
				this.WriteInstructions();
				if (this.body.HasExceptionHandlers)
				{
					this.WriteExceptionHandlers();
				}
			}
			else
			{
				num = this.BeginMethod();
				base.WriteByte((byte)(2 | (this.body.CodeSize << 2)));
				this.WriteInstructions();
				int num2 = (int)(num - this.code_base);
				int num3 = this.position - num2;
				byte[] array = new byte[num3];
				Array.Copy(this.buffer, num2, array, 0, num3);
				if (this.GetOrMapTinyMethodBody(new ByteBuffer(array), ref num))
				{
					this.position = num2;
				}
			}
			ISymbolWriter symbol_writer = this.metadata.symbol_writer;
			if (symbol_writer != null && method.debug_info != null)
			{
				method.debug_info.code_size = this.body.CodeSize;
				method.debug_info.local_var_token = this.body.local_var_token;
				symbol_writer.Write(method.debug_info);
			}
			return num;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0003094C File Offset: 0x0002EB4C
		private bool GetOrMapTinyMethodBody(ByteBuffer body, ref uint rva)
		{
			uint num;
			if (this.tiny_method_bodies.TryGetValue(body, out num))
			{
				rva = num;
				return true;
			}
			this.tiny_method_bodies.Add(body, rva);
			return false;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00030980 File Offset: 0x0002EB80
		private void WriteFatHeader()
		{
			MethodBody methodBody = this.body;
			byte b = 3;
			if (methodBody.InitLocals)
			{
				b |= 16;
			}
			if (methodBody.HasExceptionHandlers)
			{
				b |= 8;
			}
			base.WriteByte(b);
			base.WriteByte(48);
			base.WriteInt16((short)methodBody.max_stack_size);
			base.WriteInt32(methodBody.code_size);
			methodBody.local_var_token = (methodBody.HasVariables ? this.GetStandAloneSignature(methodBody.Variables) : MetadataToken.Zero);
			this.WriteMetadataToken(methodBody.local_var_token);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00030A08 File Offset: 0x0002EC08
		private void WriteInstructions()
		{
			Collection<Instruction> instructions = this.body.Instructions;
			Instruction[] items = instructions.items;
			int size = instructions.size;
			for (int i = 0; i < size; i++)
			{
				Instruction instruction = items[i];
				this.WriteOpCode(instruction.opcode);
				this.WriteOperand(instruction);
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00030A50 File Offset: 0x0002EC50
		private void WriteOpCode(OpCode opcode)
		{
			if (opcode.Size == 1)
			{
				base.WriteByte(opcode.Op2);
				return;
			}
			base.WriteByte(opcode.Op1);
			base.WriteByte(opcode.Op2);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00030A84 File Offset: 0x0002EC84
		private void WriteOperand(Instruction instruction)
		{
			OpCode opcode = instruction.opcode;
			OperandType operandType = opcode.OperandType;
			if (operandType == OperandType.InlineNone)
			{
				return;
			}
			object operand = instruction.operand;
			if (operand == null && operandType != OperandType.InlineBrTarget && operandType != OperandType.ShortInlineBrTarget)
			{
				throw new ArgumentException();
			}
			switch (operandType)
			{
			case OperandType.InlineBrTarget:
			{
				Instruction instruction2 = (Instruction)operand;
				int num = ((instruction2 != null) ? this.GetTargetOffset(instruction2) : this.body.code_size);
				base.WriteInt32(num - (instruction.Offset + opcode.Size + 4));
				return;
			}
			case OperandType.InlineField:
			case OperandType.InlineMethod:
			case OperandType.InlineTok:
			case OperandType.InlineType:
				this.WriteMetadataToken(this.metadata.LookupToken((IMetadataTokenProvider)operand));
				return;
			case OperandType.InlineI:
				base.WriteInt32((int)operand);
				return;
			case OperandType.InlineI8:
				base.WriteInt64((long)operand);
				return;
			case OperandType.InlineR:
				base.WriteDouble((double)operand);
				return;
			case OperandType.InlineSig:
				this.WriteMetadataToken(this.GetStandAloneSignature((CallSite)operand));
				return;
			case OperandType.InlineString:
				this.WriteMetadataToken(new MetadataToken(TokenType.String, this.GetUserStringIndex((string)operand)));
				return;
			case OperandType.InlineSwitch:
			{
				Instruction[] array = (Instruction[])operand;
				base.WriteInt32(array.Length);
				int num2 = instruction.Offset + opcode.Size + 4 * (array.Length + 1);
				for (int i = 0; i < array.Length; i++)
				{
					base.WriteInt32(this.GetTargetOffset(array[i]) - num2);
				}
				return;
			}
			case OperandType.InlineVar:
				base.WriteInt16((short)CodeWriter.GetVariableIndex((VariableDefinition)operand));
				return;
			case OperandType.InlineArg:
				base.WriteInt16((short)this.GetParameterIndex((ParameterDefinition)operand));
				return;
			case OperandType.ShortInlineBrTarget:
			{
				Instruction instruction3 = (Instruction)operand;
				int num3 = ((instruction3 != null) ? this.GetTargetOffset(instruction3) : this.body.code_size);
				base.WriteSByte((sbyte)(num3 - (instruction.Offset + opcode.Size + 1)));
				return;
			}
			case OperandType.ShortInlineI:
				if (opcode == OpCodes.Ldc_I4_S)
				{
					base.WriteSByte((sbyte)operand);
					return;
				}
				base.WriteByte((byte)operand);
				return;
			case OperandType.ShortInlineR:
				base.WriteSingle((float)operand);
				return;
			case OperandType.ShortInlineVar:
				base.WriteByte((byte)CodeWriter.GetVariableIndex((VariableDefinition)operand));
				return;
			case OperandType.ShortInlineArg:
				base.WriteByte((byte)this.GetParameterIndex((ParameterDefinition)operand));
				return;
			}
			throw new ArgumentException();
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00030CD8 File Offset: 0x0002EED8
		private int GetTargetOffset(Instruction instruction)
		{
			if (instruction == null)
			{
				Instruction instruction2 = this.body.instructions[this.body.instructions.size - 1];
				return instruction2.offset + instruction2.GetSize();
			}
			return instruction.offset;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00030D1F File Offset: 0x0002EF1F
		private uint GetUserStringIndex(string @string)
		{
			if (@string == null)
			{
				return 0U;
			}
			return this.metadata.user_string_heap.GetStringIndex(@string);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00030D37 File Offset: 0x0002EF37
		private static int GetVariableIndex(VariableDefinition variable)
		{
			return variable.Index;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00030D3F File Offset: 0x0002EF3F
		private int GetParameterIndex(ParameterDefinition parameter)
		{
			if (!this.body.method.HasThis)
			{
				return parameter.Index;
			}
			if (parameter == this.body.this_parameter)
			{
				return 0;
			}
			return parameter.Index + 1;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00030D74 File Offset: 0x0002EF74
		private bool RequiresFatHeader()
		{
			MethodBody methodBody = this.body;
			return methodBody.CodeSize >= 64 || methodBody.InitLocals || methodBody.HasVariables || methodBody.HasExceptionHandlers || methodBody.MaxStackSize > 8;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00030DB8 File Offset: 0x0002EFB8
		private void ComputeHeader()
		{
			int num = 0;
			Collection<Instruction> instructions = this.body.instructions;
			Instruction[] items = instructions.items;
			int size = instructions.size;
			int num2 = 0;
			int num3 = 0;
			Dictionary<Instruction, int> dictionary = null;
			if (this.body.HasExceptionHandlers)
			{
				this.ComputeExceptionHandlerStackSize(ref dictionary);
			}
			for (int i = 0; i < size; i++)
			{
				Instruction instruction = items[i];
				instruction.offset = num;
				num += instruction.GetSize();
				CodeWriter.ComputeStackSize(instruction, ref dictionary, ref num2, ref num3);
			}
			this.body.code_size = num;
			this.body.max_stack_size = num3;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00030E4C File Offset: 0x0002F04C
		private void ComputeExceptionHandlerStackSize(ref Dictionary<Instruction, int> stack_sizes)
		{
			Collection<ExceptionHandler> exceptionHandlers = this.body.ExceptionHandlers;
			for (int i = 0; i < exceptionHandlers.Count; i++)
			{
				ExceptionHandler exceptionHandler = exceptionHandlers[i];
				ExceptionHandlerType handlerType = exceptionHandler.HandlerType;
				if (handlerType != ExceptionHandlerType.Catch)
				{
					if (handlerType == ExceptionHandlerType.Filter)
					{
						CodeWriter.AddExceptionStackSize(exceptionHandler.FilterStart, ref stack_sizes);
						CodeWriter.AddExceptionStackSize(exceptionHandler.HandlerStart, ref stack_sizes);
					}
				}
				else
				{
					CodeWriter.AddExceptionStackSize(exceptionHandler.HandlerStart, ref stack_sizes);
				}
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00030EB4 File Offset: 0x0002F0B4
		private static void AddExceptionStackSize(Instruction handler_start, ref Dictionary<Instruction, int> stack_sizes)
		{
			if (handler_start == null)
			{
				return;
			}
			if (stack_sizes == null)
			{
				stack_sizes = new Dictionary<Instruction, int>();
			}
			stack_sizes[handler_start] = 1;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00030ED0 File Offset: 0x0002F0D0
		private static void ComputeStackSize(Instruction instruction, ref Dictionary<Instruction, int> stack_sizes, ref int stack_size, ref int max_stack)
		{
			int num;
			if (stack_sizes != null && stack_sizes.TryGetValue(instruction, out num))
			{
				stack_size = num;
			}
			max_stack = Math.Max(max_stack, stack_size);
			CodeWriter.ComputeStackDelta(instruction, ref stack_size);
			max_stack = Math.Max(max_stack, stack_size);
			CodeWriter.CopyBranchStackSize(instruction, ref stack_sizes, stack_size);
			CodeWriter.ComputeStackSize(instruction, ref stack_size);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00030F20 File Offset: 0x0002F120
		private static void CopyBranchStackSize(Instruction instruction, ref Dictionary<Instruction, int> stack_sizes, int stack_size)
		{
			if (stack_size == 0)
			{
				return;
			}
			OperandType operandType = instruction.opcode.OperandType;
			if (operandType != OperandType.InlineBrTarget)
			{
				if (operandType != OperandType.InlineSwitch)
				{
					if (operandType == OperandType.ShortInlineBrTarget)
					{
						goto IL_1D;
					}
				}
				else
				{
					Instruction[] array = (Instruction[])instruction.operand;
					for (int i = 0; i < array.Length; i++)
					{
						CodeWriter.CopyBranchStackSize(ref stack_sizes, array[i], stack_size);
					}
				}
				return;
			}
			IL_1D:
			CodeWriter.CopyBranchStackSize(ref stack_sizes, (Instruction)instruction.operand, stack_size);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00030F84 File Offset: 0x0002F184
		private static void CopyBranchStackSize(ref Dictionary<Instruction, int> stack_sizes, Instruction target, int stack_size)
		{
			if (stack_sizes == null)
			{
				stack_sizes = new Dictionary<Instruction, int>();
			}
			int num = stack_size;
			int num2;
			if (stack_sizes.TryGetValue(target, out num2))
			{
				num = Math.Max(num, num2);
			}
			stack_sizes[target] = num;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00030FBC File Offset: 0x0002F1BC
		private static void ComputeStackSize(Instruction instruction, ref int stack_size)
		{
			FlowControl flowControl = instruction.opcode.FlowControl;
			if (flowControl <= FlowControl.Break || flowControl - FlowControl.Return <= 1)
			{
				stack_size = 0;
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00030FE4 File Offset: 0x0002F1E4
		private static void ComputeStackDelta(Instruction instruction, ref int stack_size)
		{
			if (instruction.opcode.FlowControl == FlowControl.Call)
			{
				IMethodSignature methodSignature = (IMethodSignature)instruction.operand;
				if (methodSignature.HasImplicitThis() && instruction.opcode.Code != Code.Newobj)
				{
					stack_size--;
				}
				if (methodSignature.HasParameters)
				{
					stack_size -= methodSignature.Parameters.Count;
				}
				if (instruction.opcode.Code == Code.Calli)
				{
					stack_size--;
				}
				if (methodSignature.ReturnType.etype != ElementType.Void || instruction.opcode.Code == Code.Newobj)
				{
					stack_size++;
					return;
				}
			}
			else
			{
				CodeWriter.ComputePopDelta(instruction.opcode.StackBehaviourPop, ref stack_size);
				CodeWriter.ComputePushDelta(instruction.opcode.StackBehaviourPush, ref stack_size);
			}
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0003109C File Offset: 0x0002F29C
		private static void ComputePopDelta(StackBehaviour pop_behavior, ref int stack_size)
		{
			switch (pop_behavior)
			{
			case StackBehaviour.Pop1:
			case StackBehaviour.Popi:
			case StackBehaviour.Popref:
				stack_size--;
				return;
			case StackBehaviour.Pop1_pop1:
			case StackBehaviour.Popi_pop1:
			case StackBehaviour.Popi_popi:
			case StackBehaviour.Popi_popi8:
			case StackBehaviour.Popi_popr4:
			case StackBehaviour.Popi_popr8:
			case StackBehaviour.Popref_pop1:
			case StackBehaviour.Popref_popi:
				stack_size -= 2;
				return;
			case StackBehaviour.Popi_popi_popi:
			case StackBehaviour.Popref_popi_popi:
			case StackBehaviour.Popref_popi_popi8:
			case StackBehaviour.Popref_popi_popr4:
			case StackBehaviour.Popref_popi_popr8:
			case StackBehaviour.Popref_popi_popref:
				stack_size -= 3;
				return;
			case StackBehaviour.PopAll:
				stack_size = 0;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00031112 File Offset: 0x0002F312
		private static void ComputePushDelta(StackBehaviour push_behaviour, ref int stack_size)
		{
			switch (push_behaviour)
			{
			case StackBehaviour.Push1:
			case StackBehaviour.Pushi:
			case StackBehaviour.Pushi8:
			case StackBehaviour.Pushr4:
			case StackBehaviour.Pushr8:
			case StackBehaviour.Pushref:
				stack_size++;
				return;
			case StackBehaviour.Push1_push1:
				stack_size += 2;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00031148 File Offset: 0x0002F348
		private void WriteExceptionHandlers()
		{
			this.Align(4);
			Collection<ExceptionHandler> exceptionHandlers = this.body.ExceptionHandlers;
			if (exceptionHandlers.Count < 21 && !CodeWriter.RequiresFatSection(exceptionHandlers))
			{
				this.WriteSmallSection(exceptionHandlers);
				return;
			}
			this.WriteFatSection(exceptionHandlers);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0003118C File Offset: 0x0002F38C
		private static bool RequiresFatSection(Collection<ExceptionHandler> handlers)
		{
			for (int i = 0; i < handlers.Count; i++)
			{
				ExceptionHandler exceptionHandler = handlers[i];
				if (CodeWriter.IsFatRange(exceptionHandler.TryStart, exceptionHandler.TryEnd))
				{
					return true;
				}
				if (CodeWriter.IsFatRange(exceptionHandler.HandlerStart, exceptionHandler.HandlerEnd))
				{
					return true;
				}
				if (exceptionHandler.HandlerType == ExceptionHandlerType.Filter && CodeWriter.IsFatRange(exceptionHandler.FilterStart, exceptionHandler.HandlerStart))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x000311FB File Offset: 0x0002F3FB
		private static bool IsFatRange(Instruction start, Instruction end)
		{
			if (start == null)
			{
				throw new ArgumentException();
			}
			return end == null || end.Offset - start.Offset > 255 || start.Offset > 65535;
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00031230 File Offset: 0x0002F430
		private void WriteSmallSection(Collection<ExceptionHandler> handlers)
		{
			base.WriteByte(1);
			base.WriteByte((byte)(handlers.Count * 12 + 4));
			base.WriteBytes(2);
			this.WriteExceptionHandlers(handlers, delegate(int i)
			{
				base.WriteUInt16((ushort)i);
			}, delegate(int i)
			{
				base.WriteByte((byte)i);
			});
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0003127C File Offset: 0x0002F47C
		private void WriteFatSection(Collection<ExceptionHandler> handlers)
		{
			base.WriteByte(65);
			int num = handlers.Count * 24 + 4;
			base.WriteByte((byte)(num & 255));
			base.WriteByte((byte)((num >> 8) & 255));
			base.WriteByte((byte)((num >> 16) & 255));
			this.WriteExceptionHandlers(handlers, new Action<int>(base.WriteInt32), new Action<int>(base.WriteInt32));
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x000312EC File Offset: 0x0002F4EC
		private void WriteExceptionHandlers(Collection<ExceptionHandler> handlers, Action<int> write_entry, Action<int> write_length)
		{
			for (int i = 0; i < handlers.Count; i++)
			{
				ExceptionHandler exceptionHandler = handlers[i];
				write_entry((int)exceptionHandler.HandlerType);
				write_entry(exceptionHandler.TryStart.Offset);
				write_length(this.GetTargetOffset(exceptionHandler.TryEnd) - exceptionHandler.TryStart.Offset);
				write_entry(exceptionHandler.HandlerStart.Offset);
				write_length(this.GetTargetOffset(exceptionHandler.HandlerEnd) - exceptionHandler.HandlerStart.Offset);
				this.WriteExceptionHandlerSpecific(exceptionHandler);
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00031388 File Offset: 0x0002F588
		private void WriteExceptionHandlerSpecific(ExceptionHandler handler)
		{
			ExceptionHandlerType handlerType = handler.HandlerType;
			if (handlerType == ExceptionHandlerType.Catch)
			{
				this.WriteMetadataToken(this.metadata.LookupToken(handler.CatchType));
				return;
			}
			if (handlerType != ExceptionHandlerType.Filter)
			{
				base.WriteInt32(0);
				return;
			}
			base.WriteInt32(handler.FilterStart.Offset);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x000313D8 File Offset: 0x0002F5D8
		public MetadataToken GetStandAloneSignature(Collection<VariableDefinition> variables)
		{
			uint localVariableBlobIndex = this.metadata.GetLocalVariableBlobIndex(variables);
			return this.GetStandAloneSignatureToken(localVariableBlobIndex);
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000313FC File Offset: 0x0002F5FC
		public MetadataToken GetStandAloneSignature(CallSite call_site)
		{
			uint callSiteBlobIndex = this.metadata.GetCallSiteBlobIndex(call_site);
			MetadataToken standAloneSignatureToken = this.GetStandAloneSignatureToken(callSiteBlobIndex);
			call_site.MetadataToken = standAloneSignatureToken;
			return standAloneSignatureToken;
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00031428 File Offset: 0x0002F628
		private MetadataToken GetStandAloneSignatureToken(uint signature)
		{
			MetadataToken metadataToken;
			if (this.standalone_signatures.TryGetValue(signature, out metadataToken))
			{
				return metadataToken;
			}
			metadataToken = new MetadataToken(TokenType.Signature, this.metadata.AddStandAloneSignature(signature));
			this.standalone_signatures.Add(signature, metadataToken);
			return metadataToken;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0003146D File Offset: 0x0002F66D
		private uint BeginMethod()
		{
			return (uint)((ulong)this.code_base + (ulong)((long)this.position));
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0003147F File Offset: 0x0002F67F
		private void WriteMetadataToken(MetadataToken token)
		{
			base.WriteUInt32(token.ToUInt32());
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0003148E File Offset: 0x0002F68E
		private void Align(int align)
		{
			align--;
			base.WriteBytes(((this.position + align) & ~align) - this.position);
		}

		// Token: 0x04000780 RID: 1920
		private readonly uint code_base;

		// Token: 0x04000781 RID: 1921
		internal readonly MetadataBuilder metadata;

		// Token: 0x04000782 RID: 1922
		private readonly Dictionary<uint, MetadataToken> standalone_signatures;

		// Token: 0x04000783 RID: 1923
		private readonly Dictionary<ByteBuffer, uint> tiny_method_bodies;

		// Token: 0x04000784 RID: 1924
		private MethodBody body;
	}
}
