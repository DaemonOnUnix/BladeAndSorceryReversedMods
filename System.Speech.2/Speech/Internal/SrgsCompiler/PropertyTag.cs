using System;
using System.Runtime.InteropServices;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000F5 RID: 245
	internal sealed class PropertyTag : ParseElement, IPropertyTag, IElement
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x000271F4 File Offset: 0x000253F4
		internal PropertyTag(ParseElement parent, Backend backend)
			: base(parent._rule)
		{
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00027210 File Offset: 0x00025410
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
				this._propInfo._comType = VarEnum.VT_EMPTY;
				return;
			}
			if (text != null)
			{
				this._propInfo._comType = VarEnum.VT_EMPTY;
				return;
			}
			if (value is int)
			{
				this._propInfo._comType = VarEnum.VT_I4;
				return;
			}
			if (value is double)
			{
				this._propInfo._comType = VarEnum.VT_R8;
				return;
			}
			if (value is bool)
			{
				this._propInfo._comType = VarEnum.VT_BOOL;
			}
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x000272D4 File Offset: 0x000254D4
		void IElement.PostParse(IElement parentElement)
		{
			ParseElementCollection parseElementCollection = (ParseElementCollection)parentElement;
			this._propInfo._ulId = (uint)parseElementCollection._rule._iSerialize2;
			parseElementCollection.AddSementicPropertyTag(this._propInfo);
		}

		// Token: 0x0400061A RID: 1562
		private CfgGrammar.CfgProperty _propInfo = new CfgGrammar.CfgProperty();
	}
}
