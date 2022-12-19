using System;
using System.Diagnostics;
using System.Globalization;
using System.Speech.Internal;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x0200011C RID: 284
	[DebuggerDisplay("{DebuggerDisplayString ()}")]
	[Serializable]
	public class SrgsNameValueTag : SrgsElement, IPropertyTag, IElement
	{
		// Token: 0x06000770 RID: 1904 RVA: 0x00021501 File Offset: 0x00020501
		public SrgsNameValueTag()
		{
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00021509 File Offset: 0x00020509
		public SrgsNameValueTag(object value)
		{
			Helpers.ThrowIfNull(value, "value");
			this.Value = value;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00021523 File Offset: 0x00020523
		public SrgsNameValueTag(string name, object value)
			: this(value)
		{
			this._name = SrgsNameValueTag.GetTrimmedName(name, "name");
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0002153D File Offset: 0x0002053D
		// (set) Token: 0x06000774 RID: 1908 RVA: 0x00021545 File Offset: 0x00020545
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x00021558 File Offset: 0x00020558
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x00021560 File Offset: 0x00020560
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

		// Token: 0x06000777 RID: 1911 RVA: 0x000215B8 File Offset: 0x000205B8
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

		// Token: 0x06000778 RID: 1912 RVA: 0x0002166C File Offset: 0x0002066C
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

		// Token: 0x06000779 RID: 1913 RVA: 0x000216A3 File Offset: 0x000206A3
		void IPropertyTag.NameValue(IElement parent, string name, object value)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x000216B4 File Offset: 0x000206B4
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

		// Token: 0x0600077B RID: 1915 RVA: 0x00021765 File Offset: 0x00020765
		private static string GetTrimmedName(string name, string parameterName)
		{
			Helpers.ThrowIfEmptyOrNull(name, parameterName);
			name = name.Trim(Helpers._achTrimChars);
			Helpers.ThrowIfEmptyOrNull(name, parameterName);
			return name;
		}

		// Token: 0x04000571 RID: 1393
		private string _name;

		// Token: 0x04000572 RID: 1394
		private object _value;
	}
}
