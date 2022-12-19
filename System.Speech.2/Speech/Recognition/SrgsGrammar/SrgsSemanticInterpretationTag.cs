using System;
using System.Diagnostics;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000083 RID: 131
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsSemanticInterpretationTag : SrgsElement, ISemanticTag, IElement
	{
		// Token: 0x06000471 RID: 1137 RVA: 0x00011C7B File Offset: 0x0000FE7B
		public SrgsSemanticInterpretationTag()
		{
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00011C8E File Offset: 0x0000FE8E
		public SrgsSemanticInterpretationTag(string script)
		{
			Helpers.ThrowIfNull(script, "script");
			this._script = script;
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x00011CB3 File Offset: 0x0000FEB3
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x00011CBB File Offset: 0x0000FEBB
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

		// Token: 0x06000475 RID: 1141 RVA: 0x00011CCF File Offset: 0x0000FECF
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

		// Token: 0x06000476 RID: 1142 RVA: 0x00011D00 File Offset: 0x0000FF00
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

		// Token: 0x06000477 RID: 1143 RVA: 0x00011D40 File Offset: 0x0000FF40
		internal override string DebuggerDisplayString()
		{
			StringBuilder stringBuilder = new StringBuilder("SrgsSemanticInterpretationTag '");
			stringBuilder.Append(this._script);
			stringBuilder.Append("'");
			return stringBuilder.ToString();
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00011D77 File Offset: 0x0000FF77
		void ISemanticTag.Content(IElement parent, string value, int line)
		{
			this.Script = value;
		}

		// Token: 0x04000405 RID: 1029
		private string _script = string.Empty;
	}
}
