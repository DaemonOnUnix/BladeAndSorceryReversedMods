using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000B0 RID: 176
	internal sealed class PropertyTag : ParseElement, IPropertyTag, IElement
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x0000F4FE File Offset: 0x0000E4FE
		internal PropertyTag(ParseElement parent, Backend backend)
			: base(parent._rule)
		{
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000F518 File Offset: 0x0000E518
		void IPropertyTag.NameValue(IElement parent, string name, object value)
		{
			string text = value as string;
			if (string.IsNullOrEmpty(name) && (value == null || (text != null && string.IsNullOrEmpty(text.Trim()))))
			{
				return;
			}
			if (!string.IsNullOrEmpty(name))
			{
				this._propInfo._pszName = name;
			}
			else
			{
				this._propInfo._pszName = "=";
			}
			this._propInfo._comValue = value;
			if (value == null)
			{
				this._propInfo._comType = 0;
				return;
			}
			if (text != null)
			{
				this._propInfo._comType = 0;
				return;
			}
			if (value is int)
			{
				this._propInfo._comType = 3;
				return;
			}
			if (value is double)
			{
				this._propInfo._comType = 5;
				return;
			}
			if (value is bool)
			{
				this._propInfo._comType = 11;
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000F5DC File Offset: 0x0000E5DC
		void IElement.PostParse(IElement parentElement)
		{
			ParseElementCollection parseElementCollection = (ParseElementCollection)parentElement;
			this._propInfo._ulId = (uint)parseElementCollection._rule._iSerialize2;
			parseElementCollection.AddSementicPropertyTag(this._propInfo);
		}

		// Token: 0x0400036C RID: 876
		private CfgGrammar.CfgProperty _propInfo = new CfgGrammar.CfgProperty();
	}
}
