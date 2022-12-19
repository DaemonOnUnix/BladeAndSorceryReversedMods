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
	// Token: 0x0200018E RID: 398
	[DebuggerDisplay("{DebugSummary}")]
	public class GrammarBuilder
	{
		// Token: 0x060009D3 RID: 2515 RVA: 0x0002AEC4 File Offset: 0x00029EC4
		public GrammarBuilder()
		{
			this._grammarBuilder = new GrammarBuilder.InternalGrammarBuilder();
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0002AEE2 File Offset: 0x00029EE2
		public GrammarBuilder(string phrase)
			: this()
		{
			this.Append(phrase);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0002AEF1 File Offset: 0x00029EF1
		public GrammarBuilder(string phrase, SubsetMatchingMode subsetMatchingCriteria)
			: this()
		{
			this.Append(phrase, subsetMatchingCriteria);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0002AF01 File Offset: 0x00029F01
		public GrammarBuilder(string phrase, int minRepeat, int maxRepeat)
			: this()
		{
			this.Append(phrase, minRepeat, maxRepeat);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0002AF12 File Offset: 0x00029F12
		public GrammarBuilder(GrammarBuilder builder, int minRepeat, int maxRepeat)
			: this()
		{
			this.Append(builder, minRepeat, maxRepeat);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0002AF23 File Offset: 0x00029F23
		public GrammarBuilder(Choices alternateChoices)
			: this()
		{
			this.Append(alternateChoices);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0002AF32 File Offset: 0x00029F32
		public GrammarBuilder(SemanticResultKey key)
			: this()
		{
			this.Append(key);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0002AF41 File Offset: 0x00029F41
		public GrammarBuilder(SemanticResultValue value)
			: this()
		{
			this.Append(value);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0002AF50 File Offset: 0x00029F50
		public void Append(string phrase)
		{
			Helpers.ThrowIfEmptyOrNull(phrase, "phrase");
			this.AddItem(new GrammarBuilderPhrase(phrase));
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0002AF69 File Offset: 0x00029F69
		public void Append(string phrase, SubsetMatchingMode subsetMatchingCriteria)
		{
			Helpers.ThrowIfEmptyOrNull(phrase, "phrase");
			GrammarBuilder.ValidateSubsetMatchingCriteriaArgument(subsetMatchingCriteria, "subsetMatchingCriteria");
			this.AddItem(new GrammarBuilderPhrase(phrase, subsetMatchingCriteria));
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0002AF90 File Offset: 0x00029F90
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

		// Token: 0x060009DE RID: 2526 RVA: 0x0002AFE0 File Offset: 0x00029FE0
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

		// Token: 0x060009DF RID: 2527 RVA: 0x0002B0DC File Offset: 0x0002A0DC
		public void Append(Choices alternateChoices)
		{
			Helpers.ThrowIfNull(alternateChoices, "alternateChoices");
			this.AddItem(alternateChoices.OneOf);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0002B0F5 File Offset: 0x0002A0F5
		public void Append(SemanticResultKey key)
		{
			Helpers.ThrowIfNull(key, "builder");
			this.AddItem(key.SemanticKeyElement);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0002B10E File Offset: 0x0002A10E
		public void Append(SemanticResultValue value)
		{
			Helpers.ThrowIfNull(value, "builder");
			this.AddItem(value.Tag);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0002B128 File Offset: 0x0002A128
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

		// Token: 0x060009E3 RID: 2531 RVA: 0x0002B189 File Offset: 0x0002A189
		public void AppendDictation()
		{
			this.AddItem(new GrammarBuilderDictation());
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0002B196 File Offset: 0x0002A196
		public void AppendDictation(string category)
		{
			Helpers.ThrowIfEmptyOrNull(category, "category");
			this.AddItem(new GrammarBuilderDictation(category));
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0002B1AF File Offset: 0x0002A1AF
		public void AppendWildcard()
		{
			this.AddItem(new GrammarBuilderWildcard());
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0002B1BC File Offset: 0x0002A1BC
		public void AppendRuleReference(string path)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			Uri uri;
			try
			{
				uri = new Uri(path, 0);
			}
			catch (UriFormatException ex)
			{
				throw new ArgumentException(ex.Message, path, ex);
			}
			this.AddItem(new GrammarBuilderRuleRef(uri, null));
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0002B20C File Offset: 0x0002A20C
		public void AppendRuleReference(string path, string rule)
		{
			Helpers.ThrowIfEmptyOrNull(path, "path");
			Helpers.ThrowIfEmptyOrNull(rule, "rule");
			Uri uri;
			try
			{
				uri = new Uri(path, 0);
			}
			catch (UriFormatException ex)
			{
				throw new ArgumentException(ex.Message, path, ex);
			}
			this.AddItem(new GrammarBuilderRuleRef(uri, rule));
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0002B268 File Offset: 0x0002A268
		public string DebugShowPhrases
		{
			get
			{
				return this.DebugSummary;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0002B287 File Offset: 0x0002A287
		// (set) Token: 0x060009E9 RID: 2537 RVA: 0x0002B270 File Offset: 0x0002A270
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

		// Token: 0x060009EB RID: 2539 RVA: 0x0002B28F File Offset: 0x0002A28F
		public static GrammarBuilder operator +(string phrase, GrammarBuilder builder)
		{
			return GrammarBuilder.Add(phrase, builder);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0002B298 File Offset: 0x0002A298
		public static GrammarBuilder Add(string phrase, GrammarBuilder builder)
		{
			Helpers.ThrowIfNull(builder, "builder");
			GrammarBuilder grammarBuilder = new GrammarBuilder(phrase);
			grammarBuilder.Append(builder);
			return grammarBuilder;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0002B2BF File Offset: 0x0002A2BF
		public static GrammarBuilder operator +(GrammarBuilder builder, string phrase)
		{
			return GrammarBuilder.Add(builder, phrase);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0002B2C8 File Offset: 0x0002A2C8
		public static GrammarBuilder Add(GrammarBuilder builder, string phrase)
		{
			Helpers.ThrowIfNull(builder, "builder");
			GrammarBuilder grammarBuilder = builder.Clone();
			grammarBuilder.Append(phrase);
			return grammarBuilder;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0002B2EF File Offset: 0x0002A2EF
		public static GrammarBuilder operator +(Choices choices, GrammarBuilder builder)
		{
			return GrammarBuilder.Add(choices, builder);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0002B2F8 File Offset: 0x0002A2F8
		public static GrammarBuilder Add(Choices choices, GrammarBuilder builder)
		{
			Helpers.ThrowIfNull(choices, "choices");
			Helpers.ThrowIfNull(builder, "builder");
			GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
			grammarBuilder.Append(builder);
			return grammarBuilder;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0002B32A File Offset: 0x0002A32A
		public static GrammarBuilder operator +(GrammarBuilder builder, Choices choices)
		{
			return GrammarBuilder.Add(builder, choices);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0002B334 File Offset: 0x0002A334
		public static GrammarBuilder Add(GrammarBuilder builder, Choices choices)
		{
			Helpers.ThrowIfNull(builder, "builder");
			Helpers.ThrowIfNull(choices, "choices");
			GrammarBuilder grammarBuilder = builder.Clone();
			grammarBuilder.Append(choices);
			return grammarBuilder;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0002B366 File Offset: 0x0002A366
		public static GrammarBuilder operator +(GrammarBuilder builder1, GrammarBuilder builder2)
		{
			return GrammarBuilder.Add(builder1, builder2);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0002B370 File Offset: 0x0002A370
		public static GrammarBuilder Add(GrammarBuilder builder1, GrammarBuilder builder2)
		{
			Helpers.ThrowIfNull(builder1, "builder1");
			Helpers.ThrowIfNull(builder2, "builder2");
			GrammarBuilder grammarBuilder = builder1.Clone();
			grammarBuilder.Append(builder2);
			return grammarBuilder;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0002B3A2 File Offset: 0x0002A3A2
		public static implicit operator GrammarBuilder(string phrase)
		{
			return new GrammarBuilder(phrase);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0002B3AA File Offset: 0x0002A3AA
		public static implicit operator GrammarBuilder(Choices choices)
		{
			return new GrammarBuilder(choices);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0002B3B2 File Offset: 0x0002A3B2
		public static implicit operator GrammarBuilder(SemanticResultKey semanticKey)
		{
			return new GrammarBuilder(semanticKey);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0002B3BA File Offset: 0x0002A3BA
		public static implicit operator GrammarBuilder(SemanticResultValue semanticValue)
		{
			return new GrammarBuilder(semanticValue);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0002B3C4 File Offset: 0x0002A3C4
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

		// Token: 0x060009FA RID: 2554 RVA: 0x0002B40C File Offset: 0x0002A40C
		internal static void ValidateSubsetMatchingCriteriaArgument(SubsetMatchingMode subsetMatchingCriteria, string paramName)
		{
			switch (subsetMatchingCriteria)
			{
			case SubsetMatchingMode.Subsequence:
			case SubsetMatchingMode.OrderedSubset:
			case SubsetMatchingMode.SubsequenceContentRequired:
			case SubsetMatchingMode.OrderedSubsetContentRequired:
				return;
			default:
				throw new ArgumentException(SR.Get(SRID.EnumInvalid, new object[] { paramName }), paramName);
			}
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0002B44C File Offset: 0x0002A44C
		internal void CreateGrammar(IElementFactory elementFactory)
		{
			IdentifierCollection identifierCollection = new IdentifierCollection();
			elementFactory.Grammar.Culture = this.Culture;
			this._grammarBuilder.CreateElement(elementFactory, null, null, identifierCollection);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0002B480 File Offset: 0x0002A480
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

		// Token: 0x060009FD RID: 2557 RVA: 0x0002B4E0 File Offset: 0x0002A4E0
		internal GrammarBuilder Clone()
		{
			return new GrammarBuilder
			{
				_grammarBuilder = (GrammarBuilder.InternalGrammarBuilder)this._grammarBuilder.Clone()
			};
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x0002B50C File Offset: 0x0002A50C
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

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x0002B588 File Offset: 0x0002A588
		internal BuilderElements InternalBuilder
		{
			get
			{
				return this._grammarBuilder;
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0002B590 File Offset: 0x0002A590
		private void AddItem(GrammarBuilderBase item)
		{
			this.InternalBuilder.Items.Add(item.Clone());
		}

		// Token: 0x04000902 RID: 2306
		private GrammarBuilder.InternalGrammarBuilder _grammarBuilder;

		// Token: 0x04000903 RID: 2307
		private CultureInfo _culture = CultureInfo.CurrentUICulture;

		// Token: 0x02000191 RID: 401
		private class InternalGrammarBuilder : BuilderElements
		{
			// Token: 0x06000A1B RID: 2587 RVA: 0x0002BC24 File Offset: 0x0002AC24
			internal override GrammarBuilderBase Clone()
			{
				GrammarBuilder.InternalGrammarBuilder internalGrammarBuilder = new GrammarBuilder.InternalGrammarBuilder();
				foreach (GrammarBuilderBase grammarBuilderBase in base.Items)
				{
					internalGrammarBuilder.Items.Add(grammarBuilderBase.Clone());
				}
				return internalGrammarBuilder;
			}

			// Token: 0x06000A1C RID: 2588 RVA: 0x0002BC88 File Offset: 0x0002AC88
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
