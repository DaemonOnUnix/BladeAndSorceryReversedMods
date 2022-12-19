using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001FD RID: 509
	public abstract class VariableReference
	{
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x000352F7 File Offset: 0x000334F7
		// (set) Token: 0x06000FA0 RID: 4000 RVA: 0x000352FF File Offset: 0x000334FF
		public TypeReference VariableType
		{
			get
			{
				return this.variable_type;
			}
			set
			{
				this.variable_type = value;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x00035308 File Offset: 0x00033508
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00035310 File Offset: 0x00033510
		internal VariableReference(TypeReference variable_type)
		{
			this.variable_type = variable_type;
		}

		// Token: 0x06000FA3 RID: 4003
		public abstract VariableDefinition Resolve();

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00035326 File Offset: 0x00033526
		public override string ToString()
		{
			if (this.index >= 0)
			{
				return "V_" + this.index.ToString();
			}
			return string.Empty;
		}

		// Token: 0x04000966 RID: 2406
		internal int index = -1;

		// Token: 0x04000967 RID: 2407
		protected TypeReference variable_type;
	}
}
