using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000128 RID: 296
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsText : SrgsElement, IElementText, IElement
	{
		// Token: 0x060007D5 RID: 2005 RVA: 0x00022824 File Offset: 0x00021824
		public SrgsText()
		{
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00022837 File Offset: 0x00021837
		public SrgsText(string text)
		{
			Helpers.ThrowIfNull(text, "text");
			this.Text = text;
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0002285C File Offset: 0x0002185C
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x00022864 File Offset: 0x00021864
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				Helpers.ThrowIfNull(value, "value");
				XmlParser.ParseText(null, value, null, null, -1f, null);
				this._text = value;
			}
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00022887 File Offset: 0x00021887
		internal override void WriteSrgs(XmlWriter writer)
		{
			if (this._text != null && this._text.Length > 0)
			{
				writer.WriteString(this._text);
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000228AB File Offset: 0x000218AB
		internal override string DebuggerDisplayString()
		{
			return "'" + this._text + "'";
		}

		// Token: 0x0400059A RID: 1434
		private string _text = string.Empty;
	}
}
