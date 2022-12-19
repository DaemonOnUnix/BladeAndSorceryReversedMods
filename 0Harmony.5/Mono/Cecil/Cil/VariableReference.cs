using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002F3 RID: 755
	public abstract class VariableReference
	{
		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x0600130F RID: 4879 RVA: 0x0003D243 File Offset: 0x0003B443
		// (set) Token: 0x06001310 RID: 4880 RVA: 0x0003D24B File Offset: 0x0003B44B
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

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001311 RID: 4881 RVA: 0x0003D254 File Offset: 0x0003B454
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0003D25C File Offset: 0x0003B45C
		internal VariableReference(TypeReference variable_type)
		{
			this.variable_type = variable_type;
		}

		// Token: 0x06001313 RID: 4883
		public abstract VariableDefinition Resolve();

		// Token: 0x06001314 RID: 4884 RVA: 0x0003D272 File Offset: 0x0003B472
		public override string ToString()
		{
			if (this.index >= 0)
			{
				return "V_" + this.index.ToString();
			}
			return string.Empty;
		}

		// Token: 0x040009A5 RID: 2469
		internal int index = -1;

		// Token: 0x040009A6 RID: 2470
		protected TypeReference variable_type;
	}
}
