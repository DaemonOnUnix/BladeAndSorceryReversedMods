using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000D6 RID: 214
	internal interface IPropertyTag : IElement
	{
		// Token: 0x06000783 RID: 1923
		void NameValue(IElement parent, string name, object value);
	}
}
