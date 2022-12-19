using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E2 RID: 482
	public sealed class VariableDebugInformation : DebugInformation
	{
		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x000345A1 File Offset: 0x000327A1
		public int Index
		{
			get
			{
				return this.index.Index;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x000345AE File Offset: 0x000327AE
		// (set) Token: 0x06000F25 RID: 3877 RVA: 0x000345B6 File Offset: 0x000327B6
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

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x000345BF File Offset: 0x000327BF
		// (set) Token: 0x06000F27 RID: 3879 RVA: 0x000345C7 File Offset: 0x000327C7
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

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x000345D0 File Offset: 0x000327D0
		// (set) Token: 0x06000F29 RID: 3881 RVA: 0x000345DE File Offset: 0x000327DE
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

		// Token: 0x06000F2A RID: 3882 RVA: 0x000345F3 File Offset: 0x000327F3
		internal VariableDebugInformation(int index, string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.index = new VariableIndex(index);
			this.name = name;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0003461C File Offset: 0x0003281C
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

		// Token: 0x04000923 RID: 2339
		private string name;

		// Token: 0x04000924 RID: 2340
		private ushort attributes;

		// Token: 0x04000925 RID: 2341
		internal VariableIndex index;
	}
}
