using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;
using System.Speech.Internal.SrgsCompiler;
using System.Speech.Internal.SrgsParser;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace System.Speech.Recognition
{
	// Token: 0x02000052 RID: 82
	[DebuggerDisplay("{Text}")]
	[Serializable]
	public class RecognizedPhrase
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x00007760 File Offset: 0x00005960
		internal RecognizedPhrase()
		{
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007770 File Offset: 0x00005970
		public IXPathNavigable ConstructSmlFromSemantics()
		{
			if (!string.IsNullOrEmpty(this._smlContent))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(this._smlContent);
				return xmlDocument;
			}
			if (this._serializedPhrase.SemanticErrorInfoOffset != 0U)
			{
				this.ThrowInvalidSemanticInterpretationError();
			}
			XmlDocument xmlDocument2 = new XmlDocument();
			XmlElement xmlElement = xmlDocument2.CreateElement("SML");
			NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
			numberFormatInfo.NumberDecimalDigits = 3;
			xmlDocument2.AppendChild(xmlElement);
			xmlElement.SetAttribute("text", this.Text);
			xmlElement.SetAttribute("utteranceConfidence", this.Confidence.ToString("f", numberFormatInfo));
			xmlElement.SetAttribute("confidence", this.Confidence.ToString("f", numberFormatInfo));
			if (this.Semantics.Count > 0)
			{
				this.AppendPropertiesSML(xmlDocument2, xmlElement, this.Semantics, numberFormatInfo);
			}
			else if (this.Semantics.Value != null)
			{
				XmlText xmlText = xmlDocument2.CreateTextNode(this.Semantics.Value.ToString());
				xmlElement.AppendChild(xmlText);
			}
			for (int i = 0; i < this._recoResult.Alternates.Count; i++)
			{
				RecognizedPhrase recognizedPhrase = this._recoResult.Alternates[i];
				recognizedPhrase.AppendSml(xmlDocument2, i + 1, numberFormatInfo);
			}
			this._smlContent = xmlDocument2.OuterXml;
			return xmlDocument2;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000078C4 File Offset: 0x00005AC4
		public string Text
		{
			get
			{
				if (this._text == null)
				{
					Collection<ReplacementText> replacementWordUnits = this.ReplacementWordUnits;
					int num = 0;
					ReplacementText replacementText;
					int num2 = RecognizedPhrase.NextReplacementWord(replacementWordUnits, out replacementText, ref num);
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < this.Words.Count; i++)
					{
						DisplayAttributes displayAttributes;
						string text;
						if (i == num2)
						{
							displayAttributes = replacementText.DisplayAttributes;
							text = replacementText.Text;
							i += replacementText.CountOfWords - 1;
							num2 = RecognizedPhrase.NextReplacementWord(replacementWordUnits, out replacementText, ref num);
						}
						else
						{
							displayAttributes = this.Words[i].DisplayAttributes;
							text = this.Words[i].Text;
						}
						if ((displayAttributes & DisplayAttributes.ConsumeLeadingSpaces) != DisplayAttributes.None)
						{
							while (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == ' ')
							{
								stringBuilder.Remove(stringBuilder.Length - 1, 1);
							}
						}
						stringBuilder.Append(text);
						if ((displayAttributes & DisplayAttributes.ZeroTrailingSpaces) == DisplayAttributes.None)
						{
							if ((displayAttributes & DisplayAttributes.OneTrailingSpace) != DisplayAttributes.None)
							{
								stringBuilder.Append(" ");
							}
							else if ((displayAttributes & DisplayAttributes.TwoTrailingSpaces) != DisplayAttributes.None)
							{
								stringBuilder.Append("  ");
							}
						}
					}
					this._text = stringBuilder.ToString().Trim(new char[] { ' ' });
				}
				return this._text;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00007A02 File Offset: 0x00005C02
		public float Confidence
		{
			get
			{
				return this._confidence;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00007A0C File Offset: 0x00005C0C
		public ReadOnlyCollection<RecognizedWordUnit> Words
		{
			get
			{
				if (this._words == null)
				{
					int ulCountOfElements = (int)this._serializedPhrase.Rule.ulCountOfElements;
					int elementsOffset = (int)this._serializedPhrase.ElementsOffset;
					List<RecognizedWordUnit> list = new List<RecognizedWordUnit>(ulCountOfElements);
					int num = Marshal.SizeOf(typeof(SPSERIALIZEDPHRASEELEMENT));
					GCHandle gchandle = GCHandle.Alloc(this._phraseBuffer, GCHandleType.Pinned);
					try
					{
						IntPtr intPtr = gchandle.AddrOfPinnedObject();
						for (int i = 0; i < ulCountOfElements; i++)
						{
							IntPtr intPtr2 = new IntPtr((long)intPtr + (long)elementsOffset + (long)(i * num));
							SPSERIALIZEDPHRASEELEMENT spserializedphraseelement = (SPSERIALIZEDPHRASEELEMENT)Marshal.PtrToStructure(intPtr2, typeof(SPSERIALIZEDPHRASEELEMENT));
							string text = null;
							string text2 = null;
							string text3 = null;
							if (spserializedphraseelement.pszDisplayTextOffset != 0U)
							{
								IntPtr intPtr3 = new IntPtr((long)intPtr + (long)spserializedphraseelement.pszDisplayTextOffset);
								text = Marshal.PtrToStringUni(intPtr3);
							}
							if (spserializedphraseelement.pszLexicalFormOffset != 0U)
							{
								IntPtr intPtr4 = new IntPtr((long)intPtr + (long)spserializedphraseelement.pszLexicalFormOffset);
								text2 = Marshal.PtrToStringUni(intPtr4);
							}
							if (spserializedphraseelement.pszPronunciationOffset != 0U)
							{
								IntPtr intPtr5 = new IntPtr((long)intPtr + (long)spserializedphraseelement.pszPronunciationOffset);
								text3 = Marshal.PtrToStringUni(intPtr5);
								if (!this._hasIPAPronunciation)
								{
									text3 = this._recoResult.ConvertPronunciation(text3, (int)this._serializedPhrase.LangID);
								}
							}
							DisplayAttributes displayAttributes = RecognizedWordUnit.SapiAttributesToDisplayAttributes(spserializedphraseelement.bDisplayAttributes);
							if (!this._isSapi53Header)
							{
								spserializedphraseelement.SREngineConfidence = 1f;
							}
							list.Add(new RecognizedWordUnit(text, spserializedphraseelement.SREngineConfidence, text3, text2, displayAttributes, new TimeSpan((long)((ulong)spserializedphraseelement.ulAudioTimeOffset * 10000UL / 10000UL)), new TimeSpan((long)((ulong)spserializedphraseelement.ulAudioSizeTime * 10000UL / 10000UL))));
						}
						this._words = new ReadOnlyCollection<RecognizedWordUnit>(list);
					}
					finally
					{
						gchandle.Free();
					}
				}
				return this._words;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00007C04 File Offset: 0x00005E04
		public SemanticValue Semantics
		{
			get
			{
				if (this._serializedPhrase.SemanticErrorInfoOffset != 0U)
				{
					this.ThrowInvalidSemanticInterpretationError();
				}
				if (this._phraseBuffer == null)
				{
					throw new NotSupportedException();
				}
				if (this._semantics == null)
				{
					this.CalcSemantics(this.Grammar);
				}
				return this._semantics;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00007C44 File Offset: 0x00005E44
		public ReadOnlyCollection<RecognizedPhrase> Homophones
		{
			get
			{
				if (this._phraseBuffer == null)
				{
					throw new NotSupportedException();
				}
				if (this._homophones == null)
				{
					List<RecognizedPhrase> list = new List<RecognizedPhrase>(this._recoResult.Alternates.Count);
					for (int i = 0; i < this._recoResult.Alternates.Count; i++)
					{
						if (this._recoResult.Alternates[i]._homophoneGroupId == this._homophoneGroupId && this._recoResult.Alternates[i].Text != this.Text)
						{
							list.Add(this._recoResult.Alternates[i]);
						}
					}
					this._homophones = new ReadOnlyCollection<RecognizedPhrase>(list);
				}
				return this._homophones;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00007D08 File Offset: 0x00005F08
		public Grammar Grammar
		{
			get
			{
				if (this._grammarId == 18446744073709551615UL)
				{
					throw new NotSupportedException(SR.Get(SRID.CantGetPropertyFromSerializedInfo, new object[] { "Grammar" }));
				}
				if (this._grammar == null && this._recoResult.Recognizer != null)
				{
					this._grammar = this._recoResult.Recognizer.GetGrammarFromId(this._grammarId);
				}
				return this._grammar;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00007D74 File Offset: 0x00005F74
		public Collection<ReplacementText> ReplacementWordUnits
		{
			get
			{
				if (this._replacementText == null)
				{
					this._replacementText = new Collection<ReplacementText>();
					GCHandle gchandle = GCHandle.Alloc(this._phraseBuffer, GCHandleType.Pinned);
					try
					{
						IntPtr intPtr = gchandle.AddrOfPinnedObject();
						IntPtr intPtr2 = new IntPtr((long)intPtr + (long)((ulong)this._serializedPhrase.ReplacementsOffset));
						int num = 0;
						while ((long)num < (long)((ulong)this._serializedPhrase.cReplacements))
						{
							SPPHRASEREPLACEMENT spphrasereplacement = (SPPHRASEREPLACEMENT)Marshal.PtrToStructure(intPtr2, typeof(SPPHRASEREPLACEMENT));
							string text = Marshal.PtrToStringUni(new IntPtr((long)intPtr + (long)((ulong)spphrasereplacement.pszReplacementText)));
							DisplayAttributes displayAttributes = RecognizedWordUnit.SapiAttributesToDisplayAttributes(spphrasereplacement.bDisplayAttributes);
							this._replacementText.Add(new ReplacementText(displayAttributes, text, (int)spphrasereplacement.ulFirstElement, (int)spphrasereplacement.ulCountOfElements));
							num++;
							intPtr2 = (IntPtr)((long)intPtr2 + (long)Marshal.SizeOf(typeof(SPPHRASEREPLACEMENT)));
						}
					}
					finally
					{
						gchandle.Free();
					}
				}
				return this._replacementText;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00007E84 File Offset: 0x00006084
		public int HomophoneGroupId
		{
			get
			{
				return this._homophoneGroupId;
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007E8C File Offset: 0x0000608C
		internal static SPSERIALIZEDPHRASE GetPhraseHeader(IntPtr phraseBuffer, uint expectedPhraseSize, bool isSapi53Header)
		{
			SPSERIALIZEDPHRASE spserializedphrase;
			if (isSapi53Header)
			{
				spserializedphrase = (SPSERIALIZEDPHRASE)Marshal.PtrToStructure(phraseBuffer, typeof(SPSERIALIZEDPHRASE));
			}
			else
			{
				SPSERIALIZEDPHRASE_Sapi51 spserializedphrase_Sapi = (SPSERIALIZEDPHRASE_Sapi51)Marshal.PtrToStructure(phraseBuffer, typeof(SPSERIALIZEDPHRASE_Sapi51));
				spserializedphrase = new SPSERIALIZEDPHRASE(spserializedphrase_Sapi);
			}
			if (spserializedphrase.ulSerializedSize > expectedPhraseSize)
			{
				throw new FormatException(SR.Get(SRID.ResultInvalidFormat, new object[0]));
			}
			return spserializedphrase;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00007EF4 File Offset: 0x000060F4
		internal void InitializeFromSerializedBuffer(RecognitionResult recoResult, SPSERIALIZEDPHRASE serializedPhrase, IntPtr phraseBuffer, int phraseLength, bool isSapi53Header, bool hasIPAPronunciation)
		{
			this._recoResult = recoResult;
			this._isSapi53Header = isSapi53Header;
			this._serializedPhrase = serializedPhrase;
			this._confidence = this._serializedPhrase.Rule.SREngineConfidence;
			this._grammarId = this._serializedPhrase.ullGrammarID;
			this._homophoneGroupId = (int)this._serializedPhrase.wHomophoneGroupId;
			this._hasIPAPronunciation = hasIPAPronunciation;
			this._phraseBuffer = new byte[phraseLength];
			Marshal.Copy(phraseBuffer, this._phraseBuffer, 0, phraseLength);
			this._grammarOptions = ((recoResult.Grammar != null) ? recoResult.Grammar._semanticTag : GrammarOptions.KeyValuePairSrgs);
			this.CalcSemantics(recoResult.Grammar);
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00007F9B File Offset: 0x0000619B
		internal ulong GrammarId
		{
			get
			{
				return this._grammarId;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00007FA3 File Offset: 0x000061A3
		internal string SmlContent
		{
			get
			{
				if (this._smlContent == null)
				{
					this.ConstructSmlFromSemantics();
				}
				return this._smlContent;
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007FBC File Offset: 0x000061BC
		private void CalcSemantics(Grammar grammar)
		{
			if (this._semantics == null && this._serializedPhrase.SemanticErrorInfoOffset == 0U)
			{
				GCHandle gchandle = GCHandle.Alloc(this._phraseBuffer, GCHandleType.Pinned);
				try
				{
					IntPtr intPtr = gchandle.AddrOfPinnedObject();
					if (!this.CalcILSemantics(intPtr))
					{
						IList<RecognizedWordUnit> words = this.Words;
						RecognizedPhrase.RuleNode ruleNode = RecognizedPhrase.ExtractRules(grammar, this._serializedPhrase.Rule, intPtr);
						List<RecognizedPhrase.ResultPropertiesRef> list = RecognizedPhrase.BuildRecoPropertyTree(this._serializedPhrase, intPtr, ruleNode, words, this._isSapi53Header);
						this._semantics = RecognizedPhrase.RecursiveBuildSemanticProperties(words, list, ruleNode, this._grammarOptions & GrammarOptions.TagFormat, ref this._dupItems);
						this._semantics.Value = RecognizedPhrase.TryExecuteOnRecognition(grammar, this._recoResult, ruleNode._rule);
					}
				}
				finally
				{
					gchandle.Free();
				}
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00008088 File Offset: 0x00006288
		private bool CalcILSemantics(IntPtr phraseBuffer)
		{
			if ((this._grammarOptions & GrammarOptions.SemanticInterpretation) != GrammarOptions.KeyValuePairs || this._grammarOptions == GrammarOptions.KeyValuePairs)
			{
				IList<RecognizedWordUnit> words = this.Words;
				this._semantics = new SemanticValue("<ROOT>", null, this._confidence);
				if (this._serializedPhrase.PropertiesOffset != 0U)
				{
					this.RecursivelyExtractSemanticValue(phraseBuffer, (int)this._serializedPhrase.PropertiesOffset, this._semantics, words, this._isSapi53Header, this._grammarOptions & GrammarOptions.TagFormat);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008100 File Offset: 0x00006300
		private static List<RecognizedPhrase.ResultPropertiesRef> BuildRecoPropertyTree(SPSERIALIZEDPHRASE serializedPhrase, IntPtr phraseBuffer, RecognizedPhrase.RuleNode ruleTree, IList<RecognizedWordUnit> words, bool isSapi53Header)
		{
			List<RecognizedPhrase.ResultPropertiesRef> list = new List<RecognizedPhrase.ResultPropertiesRef>();
			if (serializedPhrase.PropertiesOffset > 0U)
			{
				RecognizedPhrase.RecursivelyExtractSemanticProperties(list, (int)serializedPhrase.PropertiesOffset, phraseBuffer, ruleTree, words, isSapi53Header);
			}
			return list;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00008130 File Offset: 0x00006330
		private static SemanticValue RecursiveBuildSemanticProperties(IList<RecognizedWordUnit> words, List<RecognizedPhrase.ResultPropertiesRef> properties, RecognizedPhrase.RuleNode ruleTree, GrammarOptions semanticTag, ref Collection<SemanticValue> dupItems)
		{
			SemanticValue semanticValue = new SemanticValue(ruleTree._name, null, ruleTree._confidence);
			for (RecognizedPhrase.RuleNode ruleNode = ruleTree._child; ruleNode != null; ruleNode = ruleNode._next)
			{
				SemanticValue semanticValue2 = RecognizedPhrase.RecursiveBuildSemanticProperties(words, properties, ruleNode, semanticTag, ref dupItems);
				if (!ruleNode._hasName)
				{
					foreach (KeyValuePair<string, SemanticValue> keyValuePair in semanticValue2._dictionary)
					{
						RecognizedPhrase.InsertSemanticValueToDictionary(semanticValue, keyValuePair.Key, keyValuePair.Value, semanticTag, ref dupItems);
					}
					if (semanticValue2.Value != null)
					{
						if ((semanticTag & GrammarOptions.SemanticInterpretation) == GrammarOptions.KeyValuePairs && semanticValue._valueFieldSet && !semanticValue.Value.Equals(semanticValue2.Value))
						{
							throw new InvalidOperationException(SR.Get(SRID.DupSemanticValue, new object[] { ruleTree._name }));
						}
						semanticValue.Value = semanticValue2.Value;
						semanticValue._valueFieldSet = true;
					}
				}
				else
				{
					if (!semanticValue2._valueFieldSet && semanticValue2.Count == 0)
					{
						StringBuilder stringBuilder = new StringBuilder();
						int num = 0;
						while ((long)num < (long)((ulong)ruleNode._count))
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.Append(" ");
							}
							stringBuilder.Append(words[(int)(ruleNode._firstElement + (uint)num)].Text);
							num++;
						}
						semanticValue2._valueFieldSet = true;
						semanticValue2.Value = stringBuilder.ToString();
					}
					semanticValue._dictionary.Add(ruleNode._name, semanticValue2);
				}
			}
			foreach (RecognizedPhrase.ResultPropertiesRef resultPropertiesRef in properties)
			{
				if (resultPropertiesRef._ruleNode == ruleTree)
				{
					RecognizedPhrase.InsertSemanticValueToDictionary(semanticValue, resultPropertiesRef._name, resultPropertiesRef._value, semanticTag, ref dupItems);
				}
			}
			Exception ex = null;
			object obj;
			bool flag = RecognizedPhrase.TryExecuteOnParse(ruleTree, semanticValue, words, out obj, ref ex);
			if (ex != null)
			{
				throw ex;
			}
			if (flag)
			{
				semanticValue._dictionary.Clear();
				semanticValue.Value = obj;
				semanticValue._valueFieldSet = true;
			}
			return semanticValue;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00008360 File Offset: 0x00006560
		private static void RecursivelyExtractSemanticProperties(List<RecognizedPhrase.ResultPropertiesRef> propertyList, int semanticsOffset, IntPtr phraseBuffer, RecognizedPhrase.RuleNode ruleTree, IList<RecognizedWordUnit> words, bool isSapi53Header)
		{
			IntPtr intPtr = new IntPtr((long)phraseBuffer + (long)semanticsOffset);
			SPSERIALIZEDPHRASEPROPERTY spserializedphraseproperty = (SPSERIALIZEDPHRASEPROPERTY)Marshal.PtrToStructure(intPtr, typeof(SPSERIALIZEDPHRASEPROPERTY));
			string text;
			SemanticValue semanticValue = RecognizedPhrase.ExtractSemanticValueInformation(semanticsOffset, spserializedphraseproperty, phraseBuffer, isSapi53Header, out text);
			RecognizedPhrase.RuleNode ruleNode = ruleTree.Find(spserializedphraseproperty.ulFirstElement, spserializedphraseproperty.ulCountOfElements);
			if (text == "SemanticKey")
			{
				ruleNode._name = (string)semanticValue.Value;
				ruleNode._hasName = true;
			}
			else
			{
				propertyList.Add(new RecognizedPhrase.ResultPropertiesRef(text, semanticValue, ruleNode));
			}
			if (spserializedphraseproperty.pFirstChildOffset > 0U)
			{
				RecognizedPhrase.RecursivelyExtractSemanticProperties(propertyList, (int)spserializedphraseproperty.pFirstChildOffset, phraseBuffer, ruleTree, words, isSapi53Header);
			}
			if (spserializedphraseproperty.pNextSiblingOffset > 0U)
			{
				RecognizedPhrase.RecursivelyExtractSemanticProperties(propertyList, (int)spserializedphraseproperty.pNextSiblingOffset, phraseBuffer, ruleTree, words, isSapi53Header);
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00008424 File Offset: 0x00006624
		private void RecursivelyExtractSemanticValue(IntPtr phraseBuffer, int semanticsOffset, SemanticValue semanticValue, IList<RecognizedWordUnit> words, bool isSapi53Header, GrammarOptions semanticTag)
		{
			IntPtr intPtr = new IntPtr((long)phraseBuffer + (long)semanticsOffset);
			SPSERIALIZEDPHRASEPROPERTY spserializedphraseproperty = (SPSERIALIZEDPHRASEPROPERTY)Marshal.PtrToStructure(intPtr, typeof(SPSERIALIZEDPHRASEPROPERTY));
			string text;
			SemanticValue semanticValue2 = RecognizedPhrase.ExtractSemanticValueInformation(semanticsOffset, spserializedphraseproperty, phraseBuffer, isSapi53Header, out text);
			if (text == "_value" && semanticValue != null)
			{
				semanticValue.Value = semanticValue2.Value;
				if (spserializedphraseproperty.pFirstChildOffset > 0U)
				{
					semanticValue2 = semanticValue;
				}
			}
			else
			{
				RecognizedPhrase.InsertSemanticValueToDictionary(semanticValue, text, semanticValue2, semanticTag, ref this._dupItems);
			}
			if (spserializedphraseproperty.pFirstChildOffset > 0U)
			{
				this.RecursivelyExtractSemanticValue(phraseBuffer, (int)spserializedphraseproperty.pFirstChildOffset, semanticValue2, words, isSapi53Header, semanticTag);
			}
			if (spserializedphraseproperty.pNextSiblingOffset > 0U)
			{
				this.RecursivelyExtractSemanticValue(phraseBuffer, (int)spserializedphraseproperty.pNextSiblingOffset, semanticValue, words, isSapi53Header, semanticTag);
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000084D8 File Offset: 0x000066D8
		private static void InsertSemanticValueToDictionary(SemanticValue semanticValue, string propertyName, SemanticValue thisSemanticValue, GrammarOptions semanticTag, ref Collection<SemanticValue> dupItems)
		{
			if ((propertyName == "$" && semanticTag == GrammarOptions.MssV1) || (propertyName == "=" && (semanticTag == GrammarOptions.KeyValuePairSrgs || semanticTag == GrammarOptions.KeyValuePairs)) || (thisSemanticValue.Count == -1 && semanticTag == GrammarOptions.W3cV1))
			{
				if ((semanticTag & GrammarOptions.SemanticInterpretation) == GrammarOptions.KeyValuePairs && semanticValue._valueFieldSet && !semanticValue.Value.Equals(thisSemanticValue.Value))
				{
					throw new InvalidOperationException(SR.Get(SRID.DupSemanticValue, new object[] { semanticValue.KeyName }));
				}
				semanticValue.Value = thisSemanticValue.Value;
				semanticValue._valueFieldSet = true;
				return;
			}
			else
			{
				if (!semanticValue._dictionary.ContainsKey(propertyName))
				{
					semanticValue._dictionary.Add(propertyName, thisSemanticValue);
					return;
				}
				if (!semanticValue._dictionary[propertyName].Equals(thisSemanticValue))
				{
					if (semanticTag == GrammarOptions.KeyValuePairSrgs)
					{
						throw new InvalidOperationException(SR.Get(SRID.DupSemanticKey, new object[] { propertyName, semanticValue.KeyName }));
					}
					int num = 0;
					string text;
					do
					{
						text = propertyName + string.Format(CultureInfo.InvariantCulture, "_{0}", new object[] { num++ });
					}
					while (semanticValue._dictionary.ContainsKey(text));
					semanticValue._dictionary.Add(text, thisSemanticValue);
					if (dupItems == null)
					{
						dupItems = new Collection<SemanticValue>();
					}
					SemanticValue semanticValue2 = semanticValue._dictionary[text];
					dupItems.Add(semanticValue2);
				}
				return;
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00008634 File Offset: 0x00006834
		private static SemanticValue ExtractSemanticValueInformation(int semanticsOffset, SPSERIALIZEDPHRASEPROPERTY property, IntPtr phraseBuffer, bool isSapi53Header, out string propertyName)
		{
			bool flag = false;
			if (property.pszNameOffset > 0U)
			{
				IntPtr intPtr = new IntPtr((long)phraseBuffer + (long)property.pszNameOffset);
				propertyName = Marshal.PtrToStringUni(intPtr);
			}
			else
			{
				propertyName = property.ulId.ToString(CultureInfo.InvariantCulture);
				flag = true;
			}
			object obj;
			if (property.pszValueOffset > 0U)
			{
				IntPtr intPtr2 = new IntPtr((long)phraseBuffer + (long)property.pszValueOffset);
				obj = Marshal.PtrToStringUni(intPtr2);
				if (!isSapi53Header && flag && ((string)obj).Contains("$"))
				{
					throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI, new object[0]));
				}
			}
			else
			{
				if (property.SpVariantSubset >= 0UL)
				{
					IntPtr intPtr3 = new IntPtr((long)phraseBuffer + (long)semanticsOffset + 16L);
					VarEnum vValue = (VarEnum)property.vValue;
					switch (vValue)
					{
					case VarEnum.VT_EMPTY:
						obj = null;
						goto IL_1A6;
					case VarEnum.VT_NULL:
					case VarEnum.VT_I2:
						break;
					case VarEnum.VT_I4:
						obj = Marshal.ReadInt32(intPtr3);
						goto IL_1A6;
					case VarEnum.VT_R4:
						obj = Marshal.PtrToStructure(intPtr3, typeof(float));
						goto IL_1A6;
					case VarEnum.VT_R8:
						obj = Marshal.PtrToStructure(intPtr3, typeof(double));
						goto IL_1A6;
					default:
						if (vValue == VarEnum.VT_BOOL)
						{
							obj = Marshal.ReadByte(intPtr3) > 0;
							goto IL_1A6;
						}
						switch (vValue)
						{
						case VarEnum.VT_UI4:
							obj = Marshal.PtrToStructure(intPtr3, typeof(uint));
							goto IL_1A6;
						case VarEnum.VT_I8:
							obj = Marshal.ReadInt64(intPtr3);
							goto IL_1A6;
						case VarEnum.VT_UI8:
							obj = Marshal.PtrToStructure(intPtr3, typeof(ulong));
							goto IL_1A6;
						}
						break;
					}
					throw new NotSupportedException(SR.Get(SRID.UnhandledVariant, new object[0]));
				}
				obj = string.Empty;
			}
			IL_1A6:
			return new SemanticValue(propertyName, obj, property.SREngineConfidence);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000087F8 File Offset: 0x000069F8
		private static RecognizedPhrase.RuleNode ExtractRules(Grammar grammar, SPSERIALIZEDPHRASERULE rule, IntPtr phraseBuffer)
		{
			IntPtr intPtr = new IntPtr((long)phraseBuffer + (long)rule.pszNameOffset);
			string text = Marshal.PtrToStringUni(intPtr);
			Grammar grammar2 = ((grammar != null) ? grammar.Find(text) : null);
			if (grammar2 != null)
			{
				grammar = grammar2;
			}
			RecognizedPhrase.RuleNode ruleNode = new RecognizedPhrase.RuleNode(grammar, text, rule.SREngineConfidence, rule.ulFirstElement, rule.ulCountOfElements);
			if (rule.NextSiblingOffset > 0U)
			{
				IntPtr intPtr2 = new IntPtr((long)phraseBuffer + (long)((ulong)rule.NextSiblingOffset));
				SPSERIALIZEDPHRASERULE spserializedphraserule = (SPSERIALIZEDPHRASERULE)Marshal.PtrToStructure(intPtr2, typeof(SPSERIALIZEDPHRASERULE));
				ruleNode._next = RecognizedPhrase.ExtractRules(grammar, spserializedphraserule, phraseBuffer);
			}
			if (rule.FirstChildOffset > 0U)
			{
				IntPtr intPtr3 = new IntPtr((long)phraseBuffer + (long)((ulong)rule.FirstChildOffset));
				SPSERIALIZEDPHRASERULE spserializedphraserule2 = (SPSERIALIZEDPHRASERULE)Marshal.PtrToStructure(intPtr3, typeof(SPSERIALIZEDPHRASERULE));
				ruleNode._child = RecognizedPhrase.ExtractRules(grammar, spserializedphraserule2, phraseBuffer);
			}
			return ruleNode;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000088DC File Offset: 0x00006ADC
		private void ThrowInvalidSemanticInterpretationError()
		{
			if (!this._isSapi53Header)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI, new object[0]));
			}
			GCHandle gchandle = GCHandle.Alloc(this._phraseBuffer, GCHandleType.Pinned);
			try
			{
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				SPSEMANTICERRORINFO spsemanticerrorinfo = (SPSEMANTICERRORINFO)Marshal.PtrToStructure((IntPtr)((long)intPtr + (long)this._serializedPhrase.SemanticErrorInfoOffset), typeof(SPSEMANTICERRORINFO));
				string text = Marshal.PtrToStringUni(new IntPtr((long)intPtr + (long)((ulong)spsemanticerrorinfo.pszSourceOffset)));
				string text2 = Marshal.PtrToStringUni(new IntPtr((long)intPtr + (long)((ulong)spsemanticerrorinfo.pszDescriptionOffset)));
				string text3 = Marshal.PtrToStringUni(new IntPtr((long)intPtr + (long)((ulong)spsemanticerrorinfo.pszScriptLineOffset)));
				string text4 = string.Format(CultureInfo.InvariantCulture, "Error while evaluating semantic interpretation:\n  HRESULT:     {0:x}\n  Line:        {1}\n  Source:      {2}\n  Description: {3}\n  Script:      {4}\n", new object[] { spsemanticerrorinfo.hrResultCode, spsemanticerrorinfo.ulLineNumber, text, text2, text3 });
				throw new InvalidOperationException(text4);
			}
			finally
			{
				gchandle.Free();
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000089F4 File Offset: 0x00006BF4
		private static bool TryExecuteOnParse(RecognizedPhrase.RuleNode ruleRef, SemanticValue value, IList<RecognizedWordUnit> words, out object newValue, ref Exception exceptionThrown)
		{
			newValue = null;
			bool flag = false;
			Grammar grammar = ruleRef._grammar;
			if (grammar != null && grammar._scripts != null)
			{
				try
				{
					if (exceptionThrown == null)
					{
						flag = RecognizedPhrase.ExecuteOnParse(grammar, ruleRef, value, words, out newValue);
					}
					else if (RecognizedPhrase.ExecuteOnError(grammar, ruleRef, exceptionThrown))
					{
						exceptionThrown = null;
					}
				}
				catch (Exception ex)
				{
					if (exceptionThrown == null)
					{
						exceptionThrown = ex;
						try
						{
							if (RecognizedPhrase.ExecuteOnError(grammar, ruleRef, exceptionThrown))
							{
								exceptionThrown = null;
							}
						}
						catch (Exception ex2)
						{
							exceptionThrown = ex2;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008A80 File Offset: 0x00006C80
		private static bool ExecuteOnParse(Grammar grammar, RecognizedPhrase.RuleNode ruleRef, SemanticValue value, IList<RecognizedWordUnit> words, out object newValue)
		{
			ScriptRef[] scripts = grammar._scripts;
			bool flag = false;
			newValue = null;
			foreach (ScriptRef scriptRef in scripts)
			{
				if (ruleRef._rule == scriptRef._rule && scriptRef._method == RuleMethodScript.onParse)
				{
					RecognizedWordUnit[] array = new RecognizedWordUnit[ruleRef._count];
					int num = 0;
					while ((long)num < (long)((ulong)ruleRef._count))
					{
						array[num] = words[num];
						num++;
					}
					object[] array2 = new object[] { value, array };
					if (grammar._proxy != null)
					{
						Exception ex;
						newValue = grammar._proxy.OnParse(scriptRef._rule, scriptRef._sMethod, array2, out ex);
						if (ex != null)
						{
							throw ex;
						}
					}
					else
					{
						MethodInfo methodInfo;
						Grammar grammar2;
						RecognizedPhrase.GetRuleInstance(grammar, scriptRef._rule, scriptRef._sMethod, out methodInfo, out grammar2);
						newValue = methodInfo.Invoke(grammar2, array2);
					}
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008B6C File Offset: 0x00006D6C
		private static bool ExecuteOnError(Grammar grammar, RecognizedPhrase.RuleNode ruleRef, Exception e)
		{
			ScriptRef[] scripts = grammar._scripts;
			bool flag = false;
			foreach (ScriptRef scriptRef in scripts)
			{
				if (ruleRef._rule == scriptRef._rule && scriptRef._method == RuleMethodScript.onError)
				{
					object[] array = new object[] { e };
					if (grammar._proxy != null)
					{
						Exception ex;
						grammar._proxy.OnError(scriptRef._rule, scriptRef._sMethod, array, out ex);
						if (ex != null)
						{
							throw ex;
						}
					}
					else
					{
						MethodInfo methodInfo;
						Grammar grammar2;
						RecognizedPhrase.GetRuleInstance(grammar, scriptRef._rule, scriptRef._sMethod, out methodInfo, out grammar2);
						methodInfo.Invoke(grammar2, array);
					}
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008C10 File Offset: 0x00006E10
		private static object TryExecuteOnRecognition(Grammar grammar, RecognitionResult result, string rootRule)
		{
			object obj = result.Semantics.Value;
			if (grammar != null && grammar._scripts != null)
			{
				foreach (ScriptRef scriptRef in grammar._scripts)
				{
					if (rootRule == scriptRef._rule && scriptRef._method == RuleMethodScript.onRecognition)
					{
						object[] array = new object[] { result };
						if (grammar._proxy != null)
						{
							Exception ex;
							obj = grammar._proxy.OnRecognition(scriptRef._sMethod, array, out ex);
							if (ex != null)
							{
								throw ex;
							}
						}
						else
						{
							Type type = grammar.GetType();
							MethodInfo method = type.GetMethod(scriptRef._sMethod, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
							obj = method.Invoke(grammar, array);
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008CC0 File Offset: 0x00006EC0
		private static void GetRuleInstance(Grammar grammar, string rule, string method, out MethodInfo onParse, out Grammar ruleInstance)
		{
			Type type = grammar.GetType();
			Assembly assembly = type.Assembly;
			Type type2 = ((rule == type.Name) ? type : RecognizedPhrase.GetTypeForRule(assembly, rule));
			if (type2 == null || !type2.IsSubclassOf(typeof(Grammar)))
			{
				throw new FormatException(SR.Get(SRID.RecognizerInvalidBinaryGrammar, new object[0]));
			}
			ruleInstance = ((type2 == type) ? grammar : ((Grammar)assembly.CreateInstance(type2.FullName)));
			onParse = ruleInstance.MethodInfo(method);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008D50 File Offset: 0x00006F50
		private static Type GetTypeForRule(Assembly assembly, string rule)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.Name == rule && type.IsPublic && type.IsSubclassOf(typeof(Grammar)))
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00008DA4 File Offset: 0x00006FA4
		private static int NextReplacementWord(Collection<ReplacementText> replacements, out ReplacementText replacement, ref int posInCollection)
		{
			if (posInCollection < replacements.Count)
			{
				int num = posInCollection;
				posInCollection = num + 1;
				replacement = replacements[num];
				return replacement.FirstWordIndex;
			}
			replacement = null;
			return -1;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00008DD8 File Offset: 0x00006FD8
		private void AppendSml(XmlDocument document, int i, NumberFormatInfo nfo)
		{
			XmlNode documentElement = document.DocumentElement;
			XmlElement xmlElement = document.CreateElement("alternate");
			documentElement.AppendChild(xmlElement);
			xmlElement.SetAttribute("Rank", i.ToString(CultureInfo.CurrentCulture));
			xmlElement.SetAttribute("text", this.Text);
			xmlElement.SetAttribute("utteranceConfidence", this.Confidence.ToString("f", nfo));
			xmlElement.SetAttribute("confidence", this.Confidence.ToString("f", nfo));
			if (this._semantics.Value != null)
			{
				XmlText xmlText = document.CreateTextNode(this._semantics.Value.ToString());
				xmlElement.AppendChild(xmlText);
			}
			this.AppendPropertiesSML(document, xmlElement, this._semantics, nfo);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008EA4 File Offset: 0x000070A4
		private void AppendPropertiesSML(XmlDocument document, XmlElement alternateNode, SemanticValue semanticsNode, NumberFormatInfo nfo)
		{
			if (semanticsNode != null)
			{
				foreach (KeyValuePair<string, SemanticValue> keyValuePair in ((IEnumerable<KeyValuePair<string, SemanticValue>>)semanticsNode))
				{
					if (keyValuePair.Key == "_attributes")
					{
						this.AppendAttributes(alternateNode, keyValuePair.Value);
						if (string.IsNullOrEmpty(alternateNode.InnerText) && semanticsNode.Value != null)
						{
							XmlText xmlText = document.CreateTextNode(semanticsNode.Value.ToString());
							alternateNode.AppendChild(xmlText);
						}
					}
					else
					{
						string text = keyValuePair.Key;
						if (this._dupItems != null && this._dupItems.Contains(keyValuePair.Value))
						{
							text = this.RemoveTrailingNumber(keyValuePair.Key);
						}
						XmlElement xmlElement = document.CreateElement(text);
						xmlElement.SetAttribute("confidence", semanticsNode[keyValuePair.Key].Confidence.ToString("f", nfo));
						alternateNode.AppendChild(xmlElement);
						if (keyValuePair.Value.Count > 0)
						{
							if (keyValuePair.Value.Value != null)
							{
								XmlText xmlText2 = document.CreateTextNode(keyValuePair.Value.Value.ToString());
								xmlElement.AppendChild(xmlText2);
							}
							this.AppendPropertiesSML(document, xmlElement, keyValuePair.Value, nfo);
						}
						else if (keyValuePair.Value.Value != null)
						{
							XmlText xmlText3 = document.CreateTextNode(keyValuePair.Value.Value.ToString());
							xmlElement.AppendChild(xmlText3);
						}
					}
				}
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00009050 File Offset: 0x00007250
		private string RemoveTrailingNumber(string name)
		{
			return name.Substring(0, name.LastIndexOf('_'));
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00009064 File Offset: 0x00007264
		private void AppendAttributes(XmlElement propertyNode, SemanticValue semanticValue)
		{
			foreach (KeyValuePair<string, SemanticValue> keyValuePair in ((IEnumerable<KeyValuePair<string, SemanticValue>>)semanticValue))
			{
				if (propertyNode.Attributes[keyValuePair.Key] == null)
				{
					propertyNode.SetAttribute(keyValuePair.Key, keyValuePair.Value.Value.ToString());
				}
			}
		}

		// Token: 0x0400030A RID: 778
		internal SPSERIALIZEDPHRASE _serializedPhrase;

		// Token: 0x0400030B RID: 779
		internal byte[] _phraseBuffer;

		// Token: 0x0400030C RID: 780
		internal bool _isSapi53Header;

		// Token: 0x0400030D RID: 781
		internal bool _hasIPAPronunciation;

		// Token: 0x0400030E RID: 782
		private RecognitionResult _recoResult;

		// Token: 0x0400030F RID: 783
		private GrammarOptions _grammarOptions;

		// Token: 0x04000310 RID: 784
		private string _text;

		// Token: 0x04000311 RID: 785
		private float _confidence;

		// Token: 0x04000312 RID: 786
		private SemanticValue _semantics;

		// Token: 0x04000313 RID: 787
		private ReadOnlyCollection<RecognizedWordUnit> _words;

		// Token: 0x04000314 RID: 788
		private Collection<ReplacementText> _replacementText;

		// Token: 0x04000315 RID: 789
		[NonSerialized]
		private ulong _grammarId = ulong.MaxValue;

		// Token: 0x04000316 RID: 790
		[NonSerialized]
		private Grammar _grammar;

		// Token: 0x04000317 RID: 791
		private int _homophoneGroupId;

		// Token: 0x04000318 RID: 792
		private ReadOnlyCollection<RecognizedPhrase> _homophones;

		// Token: 0x04000319 RID: 793
		private Collection<SemanticValue> _dupItems;

		// Token: 0x0400031A RID: 794
		private string _smlContent;

		// Token: 0x0400031B RID: 795
		private const int SpVariantSubsetOffset = 16;

		// Token: 0x02000177 RID: 375
		private class RuleNode
		{
			// Token: 0x06000B3F RID: 2879 RVA: 0x0002D1F8 File Offset: 0x0002B3F8
			internal RuleNode(Grammar grammar, string rule, float confidence, uint first, uint count)
			{
				this._name = rule;
				this._rule = rule;
				this._firstElement = first;
				this._count = count;
				this._confidence = confidence;
				this._grammar = grammar;
			}

			// Token: 0x06000B40 RID: 2880 RVA: 0x0002D23C File Offset: 0x0002B43C
			internal RecognizedPhrase.RuleNode Find(uint firstElement, uint count)
			{
				float num;
				if (count == 0U)
				{
					num = firstElement - 0.5f;
				}
				else
				{
					float num2 = firstElement;
					num = num2 + (count - 1U);
				}
				for (RecognizedPhrase.RuleNode ruleNode = this._child; ruleNode != null; ruleNode = ruleNode._next)
				{
					float num4;
					float num3;
					if (ruleNode._count == 0U)
					{
						num3 = (num4 = ruleNode._firstElement - 0.5f);
					}
					else
					{
						num4 = ruleNode._firstElement;
						num3 = num4 + (ruleNode._count - 1U);
					}
					if (num4 <= firstElement && num3 >= num)
					{
						return ruleNode.Find(firstElement, count);
					}
				}
				return this;
			}

			// Token: 0x0400089C RID: 2204
			internal Grammar _grammar;

			// Token: 0x0400089D RID: 2205
			internal string _rule;

			// Token: 0x0400089E RID: 2206
			internal string _name;

			// Token: 0x0400089F RID: 2207
			internal uint _firstElement;

			// Token: 0x040008A0 RID: 2208
			internal uint _count;

			// Token: 0x040008A1 RID: 2209
			internal float _confidence;

			// Token: 0x040008A2 RID: 2210
			internal bool _hasName;

			// Token: 0x040008A3 RID: 2211
			internal RecognizedPhrase.RuleNode _next;

			// Token: 0x040008A4 RID: 2212
			internal RecognizedPhrase.RuleNode _child;
		}

		// Token: 0x02000178 RID: 376
		private struct ResultPropertiesRef
		{
			// Token: 0x06000B41 RID: 2881 RVA: 0x0002D2C1 File Offset: 0x0002B4C1
			internal ResultPropertiesRef(string name, SemanticValue value, RecognizedPhrase.RuleNode ruleNode)
			{
				this._name = name;
				this._value = value;
				this._ruleNode = ruleNode;
			}

			// Token: 0x040008A5 RID: 2213
			internal string _name;

			// Token: 0x040008A6 RID: 2214
			internal SemanticValue _value;

			// Token: 0x040008A7 RID: 2215
			internal RecognizedPhrase.RuleNode _ruleNode;
		}
	}
}
