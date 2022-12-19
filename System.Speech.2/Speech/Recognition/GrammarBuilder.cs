using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Speech.Internal;
using System.Speech.Internal.GrammarBuilding;
using System.Speech.Internal.SrgsCompiler;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition.SrgsGrammar;
using System.Text;

namespace System.Speech.Recognition
{
	// Token: 0x0200005E RID: 94
	[DebuggerDisplay("{DebugSummary}")]
	public class GrammarBuilder
	{
		// Token: 0x06000203 RID: 515 RVA: 0x00009594 File Offset: 0x00007794
		public GrammarBuilder()
		{
			this._grammarBuilder = new GrammarBuilder.InternalGrammarBuilder();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000095B2 File Offset: 0x000077B2
		public GrammarBuilder(string phrase)
			: this()
		{
			this.Append(phrase);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000095C1 File Offset: 0x000077C1
		public GrammarBuilder(string phrase, SubsetMatchingMode subsetMatchingCriteria)
			: this()
		{
			this.Append(phrase, subsetMatchingCriteria);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000095D1 File Offset: 0x000077D1
		public GrammarBuilder(string phrase, int minRepeat, int maxRepeat)
			: this()
		{
			this.Append(phrase, minRepeat, maxRepeat);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000095E2 File Offset: 0x000077E2
		public GrammarBuilder(GrammarBuilder builder, int minRepeat, int maxRepeat)
			: this()
		{
			this.Append(builder, minRepeat, maxRepeat);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000095F3 File Offset: 0x000077F3
		public GrammarBuilder(Choices alternateChoices)
			: this()
		{
			this.Append(alternateChoices);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00009602 File Offset: 0x00007802
		public GrammarBuilder(SemanticResultKey key)
			: this()
		{
			this.Append(key);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009611 File Offset: 0x00007811
		public GrammarBuilder(SemanticResultValue value)
			: this()
		{
			this.Append(value);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00009620 File Offset: 0x00007820
		public void Append(string phrase)
		{
			Helpers.ThrowIfEmptyOrNull(phrase, "phrase");
			this.AddItem(new GrammarBuilderPhrase(phrase));
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00009639 File Offset: 0x00007839
		public void Append(string phrase, SubsetMatchingMode subsetMatchingCriteria)
		{
			Helpers.ThrowIfEmptyOrNull(phrase, "phrase");
			GrammarBuilder.ValidateSubsetMatchingCriteriaArgument(subsetMatchingCriteria, "subsetMatchingCriteria");
			this.AddItem(new GrammarBuilderPhrase(phrase, subsetMatchingCriteria));
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009660 File Offset: 0x00007860
		public void Append(string phrase, int minRepeat, int maxRepeat)
		{
			Helpers.ThrowIfEmptyOrNull(phrase, "phrase");
			GrammarBuilder.ValidateRepeatArguments(minRepeat, maxRepeat, "minRepeat", "maxRepeat");
			GrammarBuilderPhrase grammarBuilderPhrase = new GrammarBuilderPhrase(phrase);
			if (minRepeat != 1 || maxRepeat != 1)
			{
				this.AddItem(new ItemElement(grammarBuilderPhrase, minRepeat, maxRepeat));
				return;
			}
			this.AddItem(grammarBuilderPhrase);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x000096B0 File Offset: 0x000078B0
		public void Append(GrammarBuilder builder)
		{
			Helpers.ThrowIfNull(builder, "builder");
			Helpers.ThrowIfNull(builder.InternalBuilder, "builder.InternalBuilder");
			Helpers.ThrowIfNull(builder.InternalBuilder.Items, "builder.InternalBuilder.Items");
			using (List<GrammarBuilderBase>.Enumerator enumerator = builder.InternalBuilder.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == null)
					{
						throw new ArgumentException(SR.Get(SRID.ArrayOfNullIllegal, new object[0]), "builder");
					}
				}
			}
			List<GrammarBuilderBase> list = ((builder == this) ? builder.Clone().InternalBuilder.Items : builder.InternalBuilder.Items);
			foreach (GrammarBuilderBase grammarBuilderBase in list)
			{
				this.AddItem(grammarBuilderBase);
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000097AC File Offset: 0x000079AC
		public void Append(Choices alternateChoices)
		{
			Helpers.ThrowIfNull(alternateChoices, "alternateChoices");
			this.AddItem(alternateChoices.OneOf);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000097C5 File Offset: 0x000079C5
		public void Append(SemanticResultKey key)
		{
			Helpers.ThrowIfNull(key, "builder");
			this.AddItem(key.SemanticKeyElement);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000097DE File Offset: 0x000079DE
		public void Append(SemanticResultValue value)
		{
			Helpers.ThrowIfNull(value, "builder");
			this.AddItem(value.Tag);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000097F8 File Offset: 0x000079F8
		public void Append(GrammarBuilder builder, int minRepeat, int maxRepeat)
		{
			Helpers.ThrowIfNull(builder, "builder");
			GrammarBuilder.ValidateRepeatArguments(minRepeat, maxRepeat, "minRepeat", "maxRepeat");
			Helpers.ThrowIfNull(builder.InternalBuilder, "builder.InternalBuilder");
			if (minRepeat != 1 || maxRepeat != 1)
			{
				this.AddItem(new ItemElement(builder.InternalBuilder.Items, minRepeat, maxRepeat));
				return;
			}
			this.Append(builder);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00009859 File Offset: 0x00007A59
		public void AppendDictation()
		{
			this.AddItem(new GrammarBuilderDictation());
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009866 File Offset: 0x00007A66
		public void AppendDictation(string category)
		{
			Helpers.ThrowIfEmptyOrNull(category, "category");
			this.AddItem(new GrammarBuilderDictation(category));
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000987F File Offset: 0x00007A7F
		public void AppendWildcard()
		{
			this.AddItem(new GrammarBuilderWildcard());
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000988C File Offset: 0x00007A8C
		public void AppendRuleReference(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			Uri uri;
			try
			{
				uri = new Uri(path, UriKind.RelativeOrAbsolute);
			}
			catch (UriFormatException ex)
			{
				throw new ArgumentException(ex.Message, path, ex);
			}
			this.AddItem(new GrammarBuilderRuleRef(uri, null));
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000098DC File Offset: 0x00007ADC
		public void AppendRuleReference(string path, string rule)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			Helpers.ThrowIfEmptyOrNull(rule, "rule");
			Uri uri;
			try
			{
				uri = new Uri(path, UriKind.RelativeOrAbsolute);
			}
			catch (UriFormatException ex)
			{
				throw new ArgumentException(ex.Message, path, ex);
			}
			this.AddItem(new GrammarBuilderRuleRef(uri, rule));
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00009938 File Offset: 0x00007B38
		public string DebugShowPhrases
		{
			get
			{
				return this.DebugSummary;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00009957 File Offset: 0x00007B57
		// (set) Token: 0x06000219 RID: 537 RVA: 0x00009940 File Offset: 0x00007B40
		public CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._culture = value;
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000995F File Offset: 0x00007B5F
		public static GrammarBuilder operator +(string phrase, GrammarBuilder builder)
		{
			return GrammarBuilder.Add(phrase, builder);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00009968 File Offset: 0x00007B68
		public static GrammarBuilder Add(string phrase, GrammarBuilder builder)
		{
			Helpers.ThrowIfNull(builder, "builder");
			GrammarBuilder grammarBuilder = new GrammarBuilder(phrase);
			grammarBuilder.Append(builder);
			return grammarBuilder;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000998F File Offset: 0x00007B8F
		public static GrammarBuilder operator +(GrammarBuilder builder, string phrase)
		{
			return GrammarBuilder.Add(builder, phrase);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00009998 File Offset: 0x00007B98
		public static GrammarBuilder Add(GrammarBuilder builder, string phrase)
		{
			Helpers.ThrowIfNull(builder, "builder");
			GrammarBuilder grammarBuilder = builder.Clone();
			grammarBuilder.Append(phrase);
			return grammarBuilder;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000099BF File Offset: 0x00007BBF
		public static GrammarBuilder operator +(Choices choices, GrammarBuilder builder)
		{
			return GrammarBuilder.Add(choices, builder);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000099C8 File Offset: 0x00007BC8
		public static GrammarBuilder Add(Choices choices, GrammarBuilder builder)
		{
			Helpers.ThrowIfNull(choices, "choices");
			Helpers.ThrowIfNull(builder, "builder");
			GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
			grammarBuilder.Append(builder);
			return grammarBuilder;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000099FA File Offset: 0x00007BFA
		public static GrammarBuilder operator +(GrammarBuilder builder, Choices choices)
		{
			return GrammarBuilder.Add(builder, choices);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00009A04 File Offset: 0x00007C04
		public static GrammarBuilder Add(GrammarBuilder builder, Choices choices)
		{
			Helpers.ThrowIfNull(builder, "builder");
			Helpers.ThrowIfNull(choices, "choices");
			GrammarBuilder grammarBuilder = builder.Clone();
			grammarBuilder.Append(choices);
			return grammarBuilder;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00009A36 File Offset: 0x00007C36
		public static GrammarBuilder operator +(GrammarBuilder builder1, GrammarBuilder builder2)
		{
			return GrammarBuilder.Add(builder1, builder2);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00009A40 File Offset: 0x00007C40
		public static GrammarBuilder Add(GrammarBuilder builder1, GrammarBuilder builder2)
		{
			Helpers.ThrowIfNull(builder1, "builder1");
			Helpers.ThrowIfNull(builder2, "builder2");
			GrammarBuilder grammarBuilder = builder1.Clone();
			grammarBuilder.Append(builder2);
			return grammarBuilder;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00009A72 File Offset: 0x00007C72
		public static implicit operator GrammarBuilder(string phrase)
		{
			return new GrammarBuilder(phrase);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00009509 File Offset: 0x00007709
		public static implicit operator GrammarBuilder(Choices choices)
		{
			return new GrammarBuilder(choices);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009A7A File Offset: 0x00007C7A
		public static implicit operator GrammarBuilder(SemanticResultKey semanticKey)
		{
			return new GrammarBuilder(semanticKey);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00009A82 File Offset: 0x00007C82
		public static implicit operator GrammarBuilder(SemanticResultValue semanticValue)
		{
			return new GrammarBuilder(semanticValue);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00009A8A File Offset: 0x00007C8A
		internal static void ValidateRepeatArguments(int minRepeat, int maxRepeat, string minParamName, string maxParamName)
		{
			if (minRepeat < 0)
			{
				throw new ArgumentOutOfRangeException(minParamName, SR.Get(SRID.InvalidMinRepeat, new object[] { minRepeat }));
			}
			if (minRepeat > maxRepeat)
			{
				throw new ArgumentException(SR.Get(SRID.MinGreaterThanMax, new object[0]), maxParamName);
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00009AC5 File Offset: 0x00007CC5
		internal static void ValidateSubsetMatchingCriteriaArgument(SubsetMatchingMode subsetMatchingCriteria, string paramName)
		{
			if (subsetMatchingCriteria > SubsetMatchingMode.OrderedSubsetContentRequired)
			{
				throw new ArgumentException(SR.Get(SRID.EnumInvalid, new object[] { paramName }), paramName);
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00009AE4 File Offset: 0x00007CE4
		internal void CreateGrammar(IElementFactory elementFactory)
		{
			IdentifierCollection identifierCollection = new IdentifierCollection();
			elementFactory.Grammar.Culture = this.Culture;
			this._grammarBuilder.CreateElement(elementFactory, null, null, identifierCollection);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00009B18 File Offset: 0x00007D18
		internal void Compile(Stream stream)
		{
			Backend backend = new Backend();
			CustomGrammar customGrammar = new CustomGrammar();
			SrgsElementCompilerFactory srgsElementCompilerFactory = new SrgsElementCompilerFactory(backend, customGrammar);
			this.CreateGrammar(srgsElementCompilerFactory);
			backend.Optimize();
			using (StreamMarshaler streamMarshaler = new StreamMarshaler(stream))
			{
				backend.Commit(streamMarshaler);
			}
			stream.Position = 0L;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00009B78 File Offset: 0x00007D78
		internal GrammarBuilder Clone()
		{
			return new GrammarBuilder
			{
				_grammarBuilder = (GrammarBuilder.InternalGrammarBuilder)this._grammarBuilder.Clone()
			};
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00009BA4 File Offset: 0x00007DA4
		internal virtual string DebugSummary
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (GrammarBuilderBase grammarBuilderBase in this.InternalBuilder.Items)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(' ');
					}
					stringBuilder.Append(grammarBuilderBase.DebugSummary);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00009C20 File Offset: 0x00007E20
		internal BuilderElements InternalBuilder
		{
			get
			{
				return this._grammarBuilder;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009C28 File Offset: 0x00007E28
		private void AddItem(GrammarBuilderBase item)
		{
			this.InternalBuilder.Items.Add(item.Clone());
		}

		// Token: 0x04000343 RID: 835
		private GrammarBuilder.InternalGrammarBuilder _grammarBuilder;

		// Token: 0x04000344 RID: 836
		private CultureInfo _culture = CultureInfo.CurrentUICulture;

		// Token: 0x0200017A RID: 378
		private class InternalGrammarBuilder : BuilderElements
		{
			// Token: 0x06000B48 RID: 2888 RVA: 0x0002D3A8 File Offset: 0x0002B5A8
			internal override GrammarBuilderBase Clone()
			{
				GrammarBuilder.InternalGrammarBuilder internalGrammarBuilder = new GrammarBuilder.InternalGrammarBuilder();
				foreach (GrammarBuilderBase grammarBuilderBase in base.Items)
				{
					internalGrammarBuilder.Items.Add(grammarBuilderBase.Clone());
				}
				return internalGrammarBuilder;
			}

			// Token: 0x06000B49 RID: 2889 RVA: 0x0002D40C File Offset: 0x0002B60C
			internal override IElement CreateElement(IElementFactory elementFactory, IElement parent, IRule rule, IdentifierCollection ruleIds)
			{
				Collection<RuleElement> collection = new Collection<RuleElement>();
				this.CalcCount(null);
				base.Optimize(collection);
				foreach (GrammarBuilderBase grammarBuilderBase in collection)
				{
					base.Items.Add(grammarBuilderBase);
				}
				string text = ruleIds.CreateNewIdentifier("root");
				elementFactory.Grammar.Root = text;
				elementFactory.Grammar.TagFormat = SrgsTagFormat.KeyValuePairs;
				IRule rule2 = elementFactory.Grammar.CreateRule(text, RulePublic.False, RuleDynamic.NotSet, false);
				foreach (GrammarBuilderBase grammarBuilderBase2 in base.Items)
				{
					if (grammarBuilderBase2 is RuleElement)
					{
						grammarBuilderBase2.CreateElement(elementFactory, rule2, rule2, ruleIds);
					}
				}
				foreach (GrammarBuilderBase grammarBuilderBase3 in base.Items)
				{
					if (!(grammarBuilderBase3 is RuleElement))
					{
						IElement element = grammarBuilderBase3.CreateElement(elementFactory, rule2, rule2, ruleIds);
						if (element != null)
						{
							element.PostParse(rule2);
							elementFactory.AddElement(rule2, element);
						}
					}
				}
				rule2.PostParse(elementFactory.Grammar);
				elementFactory.Grammar.PostParse(null);
				return null;
			}
		}
	}
}
