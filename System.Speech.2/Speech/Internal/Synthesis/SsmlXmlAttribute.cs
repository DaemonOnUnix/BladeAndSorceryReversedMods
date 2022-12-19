using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000B1 RID: 177
	internal class SsmlXmlAttribute
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x00017E53 File Offset: 0x00016053
		internal SsmlXmlAttribute(string prefix, string name, string value, string ns)
		{
			this._prefix = prefix;
			this._name = name;
			this._value = value;
			this._ns = ns;
		}

		// Token: 0x04000496 RID: 1174
		internal string _prefix;

		// Token: 0x04000497 RID: 1175
		internal string _name;

		// Token: 0x04000498 RID: 1176
		internal string _value;

		// Token: 0x04000499 RID: 1177
		internal string _ns;
	}
}
