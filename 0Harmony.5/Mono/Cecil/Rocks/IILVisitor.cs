using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000416 RID: 1046
	public interface IILVisitor
	{
		// Token: 0x0600160F RID: 5647
		void OnInlineNone(OpCode opcode);

		// Token: 0x06001610 RID: 5648
		void OnInlineSByte(OpCode opcode, sbyte value);

		// Token: 0x06001611 RID: 5649
		void OnInlineByte(OpCode opcode, byte value);

		// Token: 0x06001612 RID: 5650
		void OnInlineInt32(OpCode opcode, int value);

		// Token: 0x06001613 RID: 5651
		void OnInlineInt64(OpCode opcode, long value);

		// Token: 0x06001614 RID: 5652
		void OnInlineSingle(OpCode opcode, float value);

		// Token: 0x06001615 RID: 5653
		void OnInlineDouble(OpCode opcode, double value);

		// Token: 0x06001616 RID: 5654
		void OnInlineString(OpCode opcode, string value);

		// Token: 0x06001617 RID: 5655
		void OnInlineBranch(OpCode opcode, int offset);

		// Token: 0x06001618 RID: 5656
		void OnInlineSwitch(OpCode opcode, int[] offsets);

		// Token: 0x06001619 RID: 5657
		void OnInlineVariable(OpCode opcode, VariableDefinition variable);

		// Token: 0x0600161A RID: 5658
		void OnInlineArgument(OpCode opcode, ParameterDefinition parameter);

		// Token: 0x0600161B RID: 5659
		void OnInlineSignature(OpCode opcode, CallSite callSite);

		// Token: 0x0600161C RID: 5660
		void OnInlineType(OpCode opcode, TypeReference type);

		// Token: 0x0600161D RID: 5661
		void OnInlineField(OpCode opcode, FieldReference field);

		// Token: 0x0600161E RID: 5662
		void OnInlineMethod(OpCode opcode, MethodReference method);
	}
}
