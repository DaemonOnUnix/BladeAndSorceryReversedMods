using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000126 RID: 294
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsSemanticInterpretationTag : SrgsElement, ISemanticTag, IElement
	{
		// Token: 0x060007CD RID: 1997 RVA: 0x0002271E File Offset: 0x0002171E
		public SrgsSemanticInterpretationTag()
		{
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00022731 File Offset: 0x00021731
		public SrgsSemanticInterpretationTag(string script)
		{
			Helpers.ThrowIfNull(script, "script");
			this._script = script;
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00022756 File Offset: 0x00021756
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0002275E File Offset: 0x0002175E
		public string Script
		{
			get
			{
				return this._script;
			}
			set
			{
				Helpers.ThrowIfNull(value, "value");
				this._script = value;
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00022772 File Offset: 0x00021772
		internal override void Validate(SrgsGrammar grammar)
		{
			if (grammar.TagFormat == SrgsTagFormat.Default)
			{
				grammar.TagFormat |= SrgsTagFormat.W3cV1;
				return;
			}
			if (grammar.TagFormat == SrgsTagFormat.KeyValuePairs)
			{
				XmlParser.ThrowSrgsException(SRID.SapiPropertiesAndSemantics, new object[0]);
			}
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x000227A4 File Offset: 0x000217A4
		internal override void WriteSrgs(XmlWriter writer)
		{
			string text = this.Script.Trim(Helpers._achTrimChars);
			writer.WriteStartElement("tag");
			if (!string.IsNullOrEmpty(text))
			{
				writer.WriteString(text);
			}
			writer.WriteEndElement();
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x000227E4 File Offset: 0x000217E4
		internal override string DebuggerDisplayString()
		{
			StringBuilder stringBuilder = new StringBuilder("SrgsSemanticInterpretationTag '");
			stringBuilder.Append(this._script);
			stringBuilder.Append("'");
			return stringBuilder.ToString();
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0002281B File Offset: 0x0002181B
		void ISemanticTag.Content(IElement parent, string value, int line)
		{
			this.Script = value;
		}

		// Token: 0x04000594 RID: 1428
		private string _script = string.Empty;
	}
}
