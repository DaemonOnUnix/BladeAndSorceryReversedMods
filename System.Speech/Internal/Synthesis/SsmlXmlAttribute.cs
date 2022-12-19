using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000DF RID: 223
	internal class SsmlXmlAttribute
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x00016B03 File Offset: 0x00015B03
		internal SsmlXmlAttribute(string prefix, string name, string value, string ns)
		{
			this._prefix = prefix;
			this._name = name;
			this._value = value;
			this._ns = ns;
		}

		// Token: 0x04000403 RID: 1027
		internal string _prefix;

		// Token: 0x04000404 RID: 1028
		internal string _name;

		// Token: 0x04000405 RID: 1029
		internal string _value;

		// Token: 0x04000406 RID: 1030
		internal string _ns;
	}
}
