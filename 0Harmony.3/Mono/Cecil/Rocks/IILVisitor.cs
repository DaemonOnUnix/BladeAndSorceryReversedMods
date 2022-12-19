using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000320 RID: 800
	public interface IILVisitor
	{
		// Token: 0x060012A0 RID: 4768
		void OnInlineNone(OpCode opcode);

		// Token: 0x060012A1 RID: 4769
		void OnInlineSByte(OpCode opcode, sbyte value);

		// Token: 0x060012A2 RID: 4770
		void OnInlineByte(OpCode opcode, byte value);

		// Token: 0x060012A3 RID: 4771
		void OnInlineInt32(OpCode opcode, int value);

		// Token: 0x060012A4 RID: 4772
		void OnInlineInt64(OpCode opcode, long value);

		// Token: 0x060012A5 RID: 4773
		void OnInlineSingle(OpCode opcode, float value);

		// Token: 0x060012A6 RID: 4774
		void OnInlineDouble(OpCode opcode, double value);

		// Token: 0x060012A7 RID: 4775
		void OnInlineString(OpCode opcode, string value);

		// Token: 0x060012A8 RID: 4776
		void OnInlineBranch(OpCode opcode, int offset);

		// Token: 0x060012A9 RID: 4777
		void OnInlineSwitch(OpCode opcode, int[] offsets);

		// Token: 0x060012AA RID: 4778
		void OnInlineVariable(OpCode opcode, VariableDefinition variable);

		// Token: 0x060012AB RID: 4779
		void OnInlineArgument(OpCode opcode, ParameterDefinition parameter);

		// Token: 0x060012AC RID: 4780
		void OnInlineSignature(OpCode opcode, CallSite callSite);

		// Token: 0x060012AD RID: 4781
		void OnInlineType(OpCode opcode, TypeReference type);

		// Token: 0x060012AE RID: 4782
		void OnInlineField(OpCode opcode, FieldReference field);

		// Token: 0x060012AF RID: 4783
		void OnInlineMethod(OpCode opcode, MethodReference method);
	}
}
