using System;
using System.Globalization;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000FB RID: 251
	internal class SrgsElementCompilerFactory : IElementFactory
	{
		// Token: 0x060008C5 RID: 2245 RVA: 0x00027E4D File Offset: 0x0002604D
		internal SrgsElementCompilerFactory(Backend backend, CustomGrammar cg)
		{
			this._backend = backend;
			this._cg = cg;
			this._grammar = new GrammarElement(backend, cg);
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0000BB6D File Offset: 0x00009D6D
		void IElementFactory.RemoveAllRules()
		{
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00027E70 File Offset: 0x00026070
		IPropertyTag IElementFactory.CreatePropertyTag(IElement parent)
		{
			return new PropertyTag((ParseElementCollection)parent, this._backend);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00027E83 File Offset: 0x00026083
		ISemanticTag IElementFactory.CreateSemanticTag(IElement parent)
		{
			return new SemanticTag((ParseElementCollection)parent, this._backend);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00016269 File Offset: 0x00014469
		IElementText IElementFactory.CreateText(IElement parent, string value)
		{
			return null;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00027E96 File Offset: 0x00026096
		IToken IElementFactory.CreateToken(IElement parent, string content, string pronunciation, string display, float reqConfidence)
		{
			this.ParseToken((ParseElementCollection)parent, content, pronunciation, display, reqConfidence);
			return null;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00027EAB File Offset: 0x000260AB
		IItem IElementFactory.CreateItem(IElement parent, IRule rule, int minRepeat, int maxRepeat, float repeatProbability, float weight)
		{
			return new Item(this._backend, (Rule)rule, minRepeat, maxRepeat, repeatProbability, weight);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00027EC5 File Offset: 0x000260C5
		IRuleRef IElementFactory.CreateRuleRef(IElement parent, Uri srgsUri)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00027ECC File Offset: 0x000260CC
		IRuleRef IElementFactory.CreateRuleRef(IElement parent, Uri srgsUri, string semanticKey, string parameters)
		{
			return new RuleRef((ParseElementCollection)parent, this._backend, srgsUri, this._grammar.UndefRules, semanticKey, parameters);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00027EEE File Offset: 0x000260EE
		void IElementFactory.InitSpecialRuleRef(IElement parent, IRuleRef specialRule)
		{
			((RuleRef)specialRule).InitSpecialRuleRef(this._backend, (ParseElementCollection)parent);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00027F07 File Offset: 0x00026107
		IOneOf IElementFactory.CreateOneOf(IElement parent, IRule rule)
		{
			return new OneOf((Rule)rule, this._backend);
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00027F1A File Offset: 0x0002611A
		ISubset IElementFactory.CreateSubset(IElement parent, string text, MatchMode mode)
		{
			return new Subset((ParseElementCollection)parent, this._backend, text, mode);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00027F2F File Offset: 0x0002612F
		void IElementFactory.AddScript(IGrammar grammar, string rule, string code)
		{
			((GrammarElement)grammar).AddScript(rule, code);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00027F40 File Offset: 0x00026140
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

		// Token: 0x060008D3 RID: 2259 RVA: 0x00027FC8 File Offset: 0x000261C8
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

		// Token: 0x060008D4 RID: 2260 RVA: 0x0000BB6D File Offset: 0x00009D6D
		void IElementFactory.AddItem(IOneOf oneOf, IItem item)
		{
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0000BB6D File Offset: 0x00009D6D
		void IElementFactory.AddElement(IRule rule, IElement value)
		{
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0000BB6D File Offset: 0x00009D6D
		void IElementFactory.AddElement(IItem item, IElement value)
		{
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0002813D File Offset: 0x0002633D
		IGrammar IElementFactory.Grammar
		{
			get
			{
				return this._grammar;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00028145 File Offset: 0x00026345
		IRuleRef IElementFactory.Null
		{
			get
			{
				return RuleRef.Null;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0002814C File Offset: 0x0002634C
		IRuleRef IElementFactory.Void
		{
			get
			{
				return RuleRef.Void;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x00028153 File Offset: 0x00026353
		IRuleRef IElementFactory.Garbage
		{
			get
			{
				return RuleRef.Garbage;
			}
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0002815C File Offset: 0x0002635C
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

		// Token: 0x060008DC RID: 2268 RVA: 0x0002836C File Offset: 0x0002656C
		private static string EscapeToken(string sToken)
		{
			if (sToken.IndexOf("\\/", StringComparison.Ordinal) == -1)
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

		// Token: 0x0400062E RID: 1582
		private Backend _backend;

		// Token: 0x0400062F RID: 1583
		private GrammarElement _grammar;

		// Token: 0x04000630 RID: 1584
		private CustomGrammar _cg;
	}
}
