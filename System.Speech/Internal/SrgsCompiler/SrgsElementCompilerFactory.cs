using System;
using System.Globalization;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000BB RID: 187
	internal class SrgsElementCompilerFactory : IElementFactory
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x00010159 File Offset: 0x0000F159
		internal SrgsElementCompilerFactory(Backend backend, CustomGrammar cg)
		{
			this._backend = backend;
			this._cg = cg;
			this._grammar = new GrammarElement(backend, cg);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001017C File Offset: 0x0000F17C
		void IElementFactory.RemoveAllRules()
		{
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001017E File Offset: 0x0000F17E
		IPropertyTag IElementFactory.CreatePropertyTag(IElement parent)
		{
			return new PropertyTag((ParseElementCollection)parent, this._backend);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00010191 File Offset: 0x0000F191
		ISemanticTag IElementFactory.CreateSemanticTag(IElement parent)
		{
			return new SemanticTag((ParseElementCollection)parent, this._backend);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x000101A4 File Offset: 0x0000F1A4
		IElementText IElementFactory.CreateText(IElement parent, string value)
		{
			return null;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x000101A7 File Offset: 0x0000F1A7
		IToken IElementFactory.CreateToken(IElement parent, string content, string pronunciation, string display, float reqConfidence)
		{
			this.ParseToken((ParseElementCollection)parent, content, pronunciation, display, reqConfidence);
			return null;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000101BC File Offset: 0x0000F1BC
		IItem IElementFactory.CreateItem(IElement parent, IRule rule, int minRepeat, int maxRepeat, float repeatProbability, float weight)
		{
			return new Item(this._backend, (Rule)rule, minRepeat, maxRepeat, repeatProbability, weight);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x000101D6 File Offset: 0x0000F1D6
		IRuleRef IElementFactory.CreateRuleRef(IElement parent, Uri srgsUri)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x000101DD File Offset: 0x0000F1DD
		IRuleRef IElementFactory.CreateRuleRef(IElement parent, Uri srgsUri, string semanticKey, string parameters)
		{
			return new RuleRef((ParseElementCollection)parent, this._backend, srgsUri, this._grammar.UndefRules, semanticKey, parameters);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x000101FF File Offset: 0x0000F1FF
		void IElementFactory.InitSpecialRuleRef(IElement parent, IRuleRef specialRule)
		{
			((RuleRef)specialRule).InitSpecialRuleRef(this._backend, (ParseElementCollection)parent);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00010218 File Offset: 0x0000F218
		IOneOf IElementFactory.CreateOneOf(IElement parent, IRule rule)
		{
			return new OneOf((Rule)rule, this._backend);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001022B File Offset: 0x0000F22B
		ISubset IElementFactory.CreateSubset(IElement parent, string text, MatchMode mode)
		{
			return new Subset((ParseElementCollection)parent, this._backend, text, mode);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00010240 File Offset: 0x0000F240
		void IElementFactory.AddScript(IGrammar grammar, string rule, string code)
		{
			((GrammarElement)grammar).AddScript(rule, code);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00010250 File Offset: 0x0000F250
		string IElementFactory.AddScript(IGrammar grammar, string rule, string code, string filename, int line)
		{
			if (line < 0)
			{
				return code;
			}
			if (this._cg._language == "C#")
			{
				return string.Format(CultureInfo.InvariantCulture, "#line {0} \"{1}\"\n{2}", new object[]
				{
					line.ToString(CultureInfo.InvariantCulture),
					filename,
					code
				});
			}
			return string.Format(CultureInfo.InvariantCulture, "#ExternalSource (\"{1}\",{0}) \n{2}\n#End ExternalSource\n", new object[]
			{
				line.ToString(CultureInfo.InvariantCulture),
				filename,
				code
			});
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000102DC File Offset: 0x0000F2DC
		void IElementFactory.AddScript(IGrammar grammar, string script, string filename, int line)
		{
			if (line < 0)
			{
				this._cg._script.Append(script);
				return;
			}
			if (this._cg._language == "C#")
			{
				this._cg._script.Append("#line ");
				this._cg._script.Append(line.ToString(CultureInfo.InvariantCulture));
				this._cg._script.Append(" \"");
				this._cg._script.Append(filename);
				this._cg._script.Append("\"\n");
				this._cg._script.Append(script);
				return;
			}
			this._cg._script.Append("#ExternalSource (");
			this._cg._script.Append(" \"");
			this._cg._script.Append(filename);
			this._cg._script.Append("\",");
			this._cg._script.Append(line.ToString(CultureInfo.InvariantCulture));
			this._cg._script.Append(")\n");
			this._cg._script.Append(script);
			this._cg._script.Append("#End #ExternalSource\n");
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00010451 File Offset: 0x0000F451
		void IElementFactory.AddItem(IOneOf oneOf, IItem item)
		{
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00010453 File Offset: 0x0000F453
		void IElementFactory.AddElement(IRule rule, IElement value)
		{
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00010455 File Offset: 0x0000F455
		void IElementFactory.AddElement(IItem item, IElement value)
		{
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x00010457 File Offset: 0x0000F457
		IGrammar IElementFactory.Grammar
		{
			get
			{
				return this._grammar;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0001045F File Offset: 0x0000F45F
		IRuleRef IElementFactory.Null
		{
			get
			{
				return RuleRef.Null;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x00010466 File Offset: 0x0000F466
		IRuleRef IElementFactory.Void
		{
			get
			{
				return RuleRef.Void;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0001046D File Offset: 0x0000F46D
		IRuleRef IElementFactory.Garbage
		{
			get
			{
				return RuleRef.Garbage;
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00010474 File Offset: 0x0000F474
		private void ParseToken(ParseElementCollection parent, string sToken, string pronunciation, string display, float reqConfidence)
		{
			int num = ((parent != null) ? parent._confidence : 0);
			sToken = Backend.NormalizeTokenWhiteSpace(sToken);
			if (string.IsNullOrEmpty(sToken))
			{
				return;
			}
			parent._confidence = 0;
			if (reqConfidence < 0f || reqConfidence.Equals(0.5f))
			{
				parent._confidence = 0;
			}
			else if ((double)reqConfidence < 0.5)
			{
				parent._confidence = -1;
			}
			else
			{
				parent._confidence = 1;
			}
			if (pronunciation != null || display != null)
			{
				string text = SrgsElementCompilerFactory.EscapeToken(sToken);
				string text2 = ((display == null) ? text : SrgsElementCompilerFactory.EscapeToken(display));
				if (pronunciation == null)
				{
					string text3 = string.Format(CultureInfo.InvariantCulture, "/{0}/{1};", new object[] { text2, text });
					parent.AddArc(this._backend.WordTransition(text3, 1f, num));
					return;
				}
				OneOf oneOf = ((pronunciation.IndexOf(';') >= 0) ? new OneOf(parent._rule, this._backend) : null);
				int num2;
				for (int i = 0; i < pronunciation.Length; i = num2 + 1)
				{
					num2 = pronunciation.IndexOf(';', i);
					if (num2 == -1)
					{
						num2 = pronunciation.Length;
					}
					string text4 = pronunciation.Substring(i, num2 - i);
					string text5 = null;
					switch (this._backend.Alphabet)
					{
					case AlphabetType.Sapi:
						text5 = PhonemeConverter.ConvertPronToId(text4, this._grammar.Backend.LangId);
						break;
					case AlphabetType.Ipa:
						text5 = text4;
						PhonemeConverter.ValidateUpsIds(text5);
						break;
					case AlphabetType.Ups:
						text5 = PhonemeConverter.UpsConverter.ConvertPronToId(text4);
						break;
					}
					string text6 = string.Format(CultureInfo.InvariantCulture, "/{0}/{1}/{2};", new object[] { text2, text, text5 });
					if (oneOf != null)
					{
						oneOf.AddArc(this._backend.WordTransition(text6, 1f, num));
					}
					else
					{
						parent.AddArc(this._backend.WordTransition(text6, 1f, num));
					}
				}
				if (oneOf != null)
				{
					((IElement)oneOf).PostParse(parent);
					return;
				}
			}
			else
			{
				parent.AddArc(this._backend.WordTransition(sToken, 1f, num));
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00010690 File Offset: 0x0000F690
		private static string EscapeToken(string sToken)
		{
			if (sToken.IndexOf("\\/", 4) == -1)
			{
				return sToken;
			}
			char[] array = sToken.ToCharArray();
			char[] array2 = new char[array.Length * 2];
			int num = 0;
			int i = 0;
			while (i < array.Length)
			{
				if (array[i] == '\\' || array[i] == '/')
				{
					array2[num++] = '\\';
				}
				array2[num++] = array[i++];
			}
			return new string(array2, 0, num);
		}

		// Token: 0x04000384 RID: 900
		private Backend _backend;

		// Token: 0x04000385 RID: 901
		private GrammarElement _grammar;

		// Token: 0x04000386 RID: 902
		private CustomGrammar _cg;
	}
}
