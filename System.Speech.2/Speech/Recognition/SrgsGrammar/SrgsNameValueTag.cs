using System;
using System.Diagnostics;
using System.Globalization;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x0200007C RID: 124
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsNameValueTag : SrgsElement, IPropertyTag, IElement
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x00010C15 File Offset: 0x0000EE15
		public SrgsNameValueTag()
		{
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00010C1D File Offset: 0x0000EE1D
		public SrgsNameValueTag(object value)
		{
			Helpers.ThrowIfNull(value, "value");
			this.Value = value;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00010C37 File Offset: 0x0000EE37
		public SrgsNameValueTag(string name, object value)
			: this(value)
		{
			this._name = SrgsNameValueTag.GetTrimmedName(name, "name");
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00010C51 File Offset: 0x0000EE51
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x00010C59 File Offset: 0x0000EE59
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = SrgsNameValueTag.GetTrimmedName(value, "value");
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00010C6C File Offset: 0x0000EE6C
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x00010C74 File Offset: 0x0000EE74
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				Helpers.ThrowIfNull(value, "value");
				if (value is string || value is bool || value is int || value is double)
				{
					this._value = value;
					return;
				}
				throw new ArgumentException(SR.Get(SRID.InvalidValueType, new object[0]), "value");
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00010CCC File Offset: 0x0000EECC
		internal override void WriteSrgs(XmlWriter writer)
		{
			bool flag = this.Value != null;
			bool flag2 = !string.IsNullOrEmpty(this._name);
			writer.WriteStartElement("tag");
			StringBuilder stringBuilder = new StringBuilder();
			if (flag2)
			{
				stringBuilder.Append(this._name);
				stringBuilder.Append("=");
			}
			if (flag)
			{
				if (this.Value is string)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\"{0}\"", new object[] { this.Value.ToString() });
				}
				else
				{
					stringBuilder.Append(this.Value.ToString());
				}
			}
			writer.WriteString(stringBuilder.ToString());
			writer.WriteEndElement();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00010D7C File Offset: 0x0000EF7C
		internal override void Validate(SrgsGrammar grammar)
		{
			SrgsTagFormat tagFormat = grammar.TagFormat;
			if (tagFormat == SrgsTagFormat.Default)
			{
				grammar.TagFormat |= SrgsTagFormat.KeyValuePairs;
				return;
			}
			if (tagFormat != SrgsTagFormat.KeyValuePairs)
			{
				XmlParser.ThrowSrgsException(SRID.SapiPropertiesAndSemantics, new object[0]);
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00010DB3 File Offset: 0x0000EFB3
		void IPropertyTag.NameValue(IElement parent, string name, object value)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00010DC4 File Offset: 0x0000EFC4
		internal override string DebuggerDisplayString()
		{
			StringBuilder stringBuilder = new StringBuilder("SrgsNameValue ");
			if (this._name != null)
			{
				stringBuilder.Append(this._name);
				stringBuilder.Append(" (");
			}
			if (this._value != null)
			{
				if (this._value is string)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\"{0}\"", new object[] { this._value.ToString() });
				}
				else
				{
					stringBuilder.Append(this._value.ToString());
				}
			}
			else
			{
				stringBuilder.Append("null");
			}
			if (this._name != null)
			{
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00010E73 File Offset: 0x0000F073
		private static string GetTrimmedName(string name, string parameterName)
		{
			Helpers.ThrowIfEmptyOrNull(name, parameterName);
			name = name.Trim(Helpers._achTrimChars);
			Helpers.ThrowIfEmptyOrNull(name, parameterName);
			return name;
		}

		// Token: 0x040003E8 RID: 1000
		private string _name;

		// Token: 0x040003E9 RID: 1001
		private object _value;
	}
}
