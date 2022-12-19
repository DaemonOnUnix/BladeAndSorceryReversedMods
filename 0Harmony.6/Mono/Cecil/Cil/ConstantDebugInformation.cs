using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E3 RID: 483
	public sealed class ConstantDebugInformation : DebugInformation
	{
		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0003466E File Offset: 0x0003286E
		// (set) Token: 0x06000F2D RID: 3885 RVA: 0x00034676 File Offset: 0x00032876
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

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0003467F File Offset: 0x0003287F
		// (set) Token: 0x06000F2F RID: 3887 RVA: 0x00034687 File Offset: 0x00032887
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

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x00034690 File Offset: 0x00032890
		// (set) Token: 0x06000F31 RID: 3889 RVA: 0x00034698 File Offset: 0x00032898
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

		// Token: 0x06000F32 RID: 3890 RVA: 0x000346A1 File Offset: 0x000328A1
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

		// Token: 0x04000926 RID: 2342
		private string name;

		// Token: 0x04000927 RID: 2343
		private TypeReference constant_type;

		// Token: 0x04000928 RID: 2344
		private object value;
	}
}
