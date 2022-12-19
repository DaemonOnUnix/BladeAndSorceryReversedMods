using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002D9 RID: 729
	public sealed class ConstantDebugInformation : DebugInformation
	{
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x0003C4FA File Offset: 0x0003A6FA
		// (set) Token: 0x0600129A RID: 4762 RVA: 0x0003C502 File Offset: 0x0003A702
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

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x0600129B RID: 4763 RVA: 0x0003C50B File Offset: 0x0003A70B
		// (set) Token: 0x0600129C RID: 4764 RVA: 0x0003C513 File Offset: 0x0003A713
		public TypeReference ConstantType
		{
			get
			{
				return this.constant_type;
			}
			set
			{
				this.constant_type = value;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x0003C51C File Offset: 0x0003A71C
		// (set) Token: 0x0600129E RID: 4766 RVA: 0x0003C524 File Offset: 0x0003A724
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0003C52D File Offset: 0x0003A72D
		public ConstantDebugInformation(string name, TypeReference constant_type, object value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.name = name;
			this.constant_type = constant_type;
			this.value = value;
			this.token = new MetadataToken(TokenType.LocalConstant);
		}

		// Token: 0x04000962 RID: 2402
		private string name;

		// Token: 0x04000963 RID: 2403
		private TypeReference constant_type;

		// Token: 0x04000964 RID: 2404
		private object value;
	}
}
