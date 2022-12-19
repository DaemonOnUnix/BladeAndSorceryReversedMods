using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000085 RID: 133
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsText : SrgsElement, IElementText, IElement
	{
		// Token: 0x06000479 RID: 1145 RVA: 0x00011D80 File Offset: 0x0000FF80
		public SrgsText()
		{
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00011D93 File Offset: 0x0000FF93
		public SrgsText(string text)
		{
			Helpers.ThrowIfNull(text, "text");
			this.Text = text;
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00011DB8 File Offset: 0x0000FFB8
		// (set) Token: 0x0600047C RID: 1148 RVA: 0x00011DC0 File Offset: 0x0000FFC0
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

		// Token: 0x0600047D RID: 1149 RVA: 0x00011DE3 File Offset: 0x0000FFE3
		internal override void WriteSrgs(XmlWriter writer)
		{
			if (this._text != null && this._text.Length > 0)
			{
				writer.WriteString(this._text);
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00011E07 File Offset: 0x00010007
		internal override string DebuggerDisplayString()
		{
			return "'" + this._text + "'";
		}

		// Token: 0x0400040B RID: 1035
		private string _text = string.Empty;
	}
}
