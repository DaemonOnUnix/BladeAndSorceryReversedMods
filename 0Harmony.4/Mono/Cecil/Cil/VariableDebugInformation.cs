using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002D8 RID: 728
	public sealed class VariableDebugInformation : DebugInformation
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x0003C42A File Offset: 0x0003A62A
		public int Index
		{
			get
			{
				return this.index.Index;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x0003C437 File Offset: 0x0003A637
		// (set) Token: 0x06001292 RID: 4754 RVA: 0x0003C43F File Offset: 0x0003A63F
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001293 RID: 4755 RVA: 0x0003C448 File Offset: 0x0003A648
		// (set) Token: 0x06001294 RID: 4756 RVA: 0x0003C450 File Offset: 0x0003A650
		public VariableAttributes Attributes
		{
			get
			{
				return (VariableAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x0003C459 File Offset: 0x0003A659
		// (set) Token: 0x06001296 RID: 4758 RVA: 0x0003C467 File Offset: 0x0003A667
		public bool IsDebuggerHidden
		{
			get
			{
				return this.attributes.GetAttributes(1);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1, value);
			}
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0003C47C File Offset: 0x0003A67C
		internal VariableDebugInformation(int index, string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.index = new VariableIndex(index);
			this.name = name;
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0003C4A8 File Offset: 0x0003A6A8
		public VariableDebugInformation(VariableDefinition variable, string name)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.index = new VariableIndex(variable);
			this.name = name;
			this.token = new MetadataToken(TokenType.LocalVariable);
		}

		// Token: 0x0400095F RID: 2399
		private string name;

		// Token: 0x04000960 RID: 2400
		private ushort attributes;

		// Token: 0x04000961 RID: 2401
		internal VariableIndex index;
	}
}
