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
	// Token: 0x0200012E RID: 302
	[DebuggerDisplay("{Text}")]
	[Serializable]
	public class RecognizedPhrase
	{
		// Token: 0x060007EA RID: 2026 RVA: 0x00022B66 File Offset: 0x00021B66
		internal RecognizedPhrase()
		{
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00022B78 File Offset: 0x00021B78
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

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x00022CCC File Offset: 0x00021CCC
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
							while (stringBuilder.Length > 0 && stringBuilder.get_Chars(stringBuilder.Length - 1) == ' ')
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

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00022E0F File Offset: 0x00021E0F
		public float Confidence
		{
			get
			{
				return this._confidence;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x00022E18 File Offset: 0x00021E18
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
					GCHandle gchandle = GCHandle.Alloc(this._phraseBuffer, 3);
					try
					{
						IntPtr intPtr = gchandle.AddrOfPinnedObject();
						for (int i = 0; i < ulCountOfElements; i++)
						{
							IntPtr intPtr2;
							intPtr2..ctor((long)intPtr + (long)elementsOffset + (long)(i * num));
							SPSERIALIZEDPHRASEELEMENT spserializedphraseelement = (SPSERIALIZEDPHRASEELEMENT)Marshal.PtrToStructure(intPtr2, typeof(SPSERIALIZEDPHRASEELEMENT));
							string text = null;
							string text2 = null;
							string text3 = null;
							if (spserializedphraseelement.pszDisplayTextOffset != 0U)
							{
								IntPtr intPtr3;
								intPtr3..ctor((long)intPtr + (long)spserializedphraseelement.pszDisplayTextOffset);
								text = Marshal.PtrToStringUni(intPtr3);
							}
							if (spserializedphraseelement.pszLexicalFormOffset != 0U)
							{
								IntPtr intPtr4;
								intPtr4..ctor((long)intPtr + (long)spserializedphraseelement.pszLexicalFormOffset);
								text2 = Marshal.PtrToStringUni(intPtr4);
							}
							if (spserializedphraseelement.pszPronunciationOffset != 0U)
							{
								IntPtr intPtr5;
								intPtr5..ctor((long)intPtr + (long)spserializedphraseelement.pszPronunciationOffset);
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

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x00023010 File Offset: 0x00022010
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

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x00023050 File Offset: 0x00022050
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

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00023114 File Offset: 0x00022114
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

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00023180 File Offset: 0x00022180
		public Collection<ReplacementText> ReplacementWordUnits
		{
			get
			{
				if (this._replacementText == null)
				{
					this._replacementText = new Collection<ReplacementText>();
					GCHandle gchandle = GCHandle.Alloc(this._phraseBuffer, 3);
					try
					{
						IntPtr intPtr = gchandle.AddrOfPinnedObject();
						IntPtr intPtr2;
						intPtr2..ctor((long)intPtr + (long)((ulong)this._serializedPhrase.ReplacementsOffset));
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

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x00023290 File Offset: 0x00022290
		public int HomophoneGroupId
		{
			get
			{
				return this._homophoneGroupId;
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00023298 File Offset: 0x00022298
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

		// Token: 0x060007F5 RID: 2037 RVA: 0x00023300 File Offset: 0x00022300
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

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x000233A7 File Offset: 0x000223A7
		internal ulong GrammarId
		{
			get
			{
				return this._grammarId;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x000233AF File Offset: 0x000223AF
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

		// Token: 0x060007F8 RID: 2040 RVA: 0x000233C8 File Offset: 0x000223C8
		private void CalcSemantics(Grammar grammar)
		{
			if (this._semantics == null && this._serializedPhrase.SemanticErrorInfoOffset == 0U)
			{
				GCHandle gchandle = GCHandle.Alloc(this._phraseBuffer, 3);
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

		// Token: 0x060007F9 RID: 2041 RVA: 0x00023494 File Offset: 0x00022494
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

		// Token: 0x060007FA RID: 2042 RVA: 0x0002350C File Offset: 0x0002250C
		private static List<RecognizedPhrase.ResultPropertiesRef> BuildRecoPropertyTree(SPSERIALIZEDPHRASE serializedPhrase, IntPtr phraseBuffer, RecognizedPhrase.RuleNode ruleTree, IList<RecognizedWordUnit> words, bool isSapi53Header)
		{
			List<RecognizedPhrase.ResultPropertiesRef> list = new List<RecognizedPhrase.ResultPropertiesRef>();
			if (serializedPhrase.PropertiesOffset > 0U)
			{
				RecognizedPhrase.RecursivelyExtractSemanticProperties(list, (int)serializedPhrase.PropertiesOffset, phraseBuffer, ruleTree, words, isSapi53Header);
			}
			return list;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0002353C File Offset: 0x0002253C
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

		// Token: 0x060007FC RID: 2044 RVA: 0x00023764 File Offset: 0x00022764
		private static void RecursivelyExtractSemanticProperties(List<RecognizedPhrase.ResultPropertiesRef> propertyList, int semanticsOffset, IntPtr phraseBuffer, RecognizedPhrase.RuleNode ruleTree, IList<RecognizedWordUnit> words, bool isSapi53Header)
		{
			IntPtr intPtr;
			intPtr..ctor((long)phraseBuffer + (long)semanticsOffset);
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

		// Token: 0x060007FD RID: 2045 RVA: 0x00023828 File Offset: 0x00022828
		private void RecursivelyExtractSemanticValue(IntPtr phraseBuffer, int semanticsOffset, SemanticValue semanticValue, IList<RecognizedWordUnit> words, bool isSapi53Header, GrammarOptions semanticTag)
		{
			IntPtr intPtr;
			intPtr..ctor((long)phraseBuffer + (long)semanticsOffset);
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

		// Token: 0x060007FE RID: 2046 RVA: 0x000238DC File Offset: 0x000228DC
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

		// Token: 0x060007FF RID: 2047 RVA: 0x00023A48 File Offset: 0x00022A48
		private static SemanticValue ExtractSemanticValueInformation(int semanticsOffset, SPSERIALIZEDPHRASEPROPERTY property, IntPtr phraseBuffer, bool isSapi53Header, out string propertyName)
		{
			bool flag = false;
			if (property.pszNameOffset > 0U)
			{
				IntPtr intPtr;
				intPtr..ctor((long)phraseBuffer + (long)property.pszNameOffset);
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
				IntPtr intPtr2;
				intPtr2..ctor((long)phraseBuffer + (long)property.pszValueOffset);
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
					IntPtr intPtr3;
					intPtr3..ctor((long)phraseBuffer + (long)semanticsOffset + 16L);
					VarEnum vValue = property.vValue;
					switch (vValue)
					{
					case 0:
						obj = null;
						goto IL_1AD;
					case 1:
					case 2:
						break;
					case 3:
						obj = Marshal.ReadInt32(intPtr3);
						goto IL_1AD;
					case 4:
						obj = Marshal.PtrToStructure(intPtr3, typeof(float));
						goto IL_1AD;
					case 5:
						obj = Marshal.PtrToStructure(intPtr3, typeof(double));
						goto IL_1AD;
					default:
						if (vValue == 11)
						{
							obj = Marshal.ReadByte(intPtr3) != 0;
							goto IL_1AD;
						}
						switch (vValue)
						{
						case 19:
							obj = Marshal.PtrToStructure(intPtr3, typeof(uint));
							goto IL_1AD;
						case 20:
							obj = Marshal.ReadInt64(intPtr3);
							goto IL_1AD;
						case 21:
							obj = Marshal.PtrToStructure(intPtr3, typeof(ulong));
							goto IL_1AD;
						}
						break;
					}
					throw new NotSupportedException(SR.Get(SRID.UnhandledVariant, new object[0]));
				}
				obj = string.Empty;
			}
			IL_1AD:
			return new SemanticValue(propertyName, obj, property.SREngineConfidence);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00023C14 File Offset: 0x00022C14
		private static RecognizedPhrase.RuleNode ExtractRules(Grammar grammar, SPSERIALIZEDPHRASERULE rule, IntPtr phraseBuffer)
		{
			IntPtr intPtr;
			intPtr..ctor((long)phraseBuffer + (long)rule.pszNameOffset);
			string text = Marshal.PtrToStringUni(intPtr);
			Grammar grammar2 = ((grammar != null) ? grammar.Find(text) : null);
			if (grammar2 != null)
			{
				grammar = grammar2;
			}
			RecognizedPhrase.RuleNode ruleNode = new RecognizedPhrase.RuleNode(grammar, text, rule.SREngineConfidence, rule.ulFirstElement, rule.ulCountOfElements);
			if (rule.NextSiblingOffset > 0U)
			{
				IntPtr intPtr2;
				intPtr2..ctor((long)phraseBuffer + (long)((ulong)rule.NextSiblingOffset));
				SPSERIALIZEDPHRASERULE spserializedphraserule = (SPSERIALIZEDPHRASERULE)Marshal.PtrToStructure(intPtr2, typeof(SPSERIALIZEDPHRASERULE));
				ruleNode._next = RecognizedPhrase.ExtractRules(grammar, spserializedphraserule, phraseBuffer);
			}
			if (rule.FirstChildOffset > 0U)
			{
				IntPtr intPtr3;
				intPtr3..ctor((long)phraseBuffer + (long)((ulong)rule.FirstChildOffset));
				SPSERIALIZEDPHRASERULE spserializedphraserule2 = (SPSERIALIZEDPHRASERULE)Marshal.PtrToStructure(intPtr3, typeof(SPSERIALIZEDPHRASERULE));
				ruleNode._child = RecognizedPhrase.ExtractRules(grammar, spserializedphraserule2, phraseBuffer);
			}
			return ruleNode;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00023CF8 File Offset: 0x00022CF8
		private void ThrowInvalidSemanticInterpretationError()
		{
			if (!this._isSapi53Header)
			{
				throw new NotSupportedException(SR.Get(SRID.NotSupportedWithThisVersionOfSAPI, new object[0]));
			}
			GCHandle gchandle = GCHandle.Alloc(this._phraseBuffer, 3);
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

		// Token: 0x06000802 RID: 2050 RVA: 0x00023E18 File Offset: 0x00022E18
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

		// Token: 0x06000803 RID: 2051 RVA: 0x00023EA4 File Offset: 0x00022EA4
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

		// Token: 0x06000804 RID: 2052 RVA: 0x00023F98 File Offset: 0x00022F98
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

		// Token: 0x06000805 RID: 2053 RVA: 0x00024044 File Offset: 0x00023044
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
							MethodInfo method = type.GetMethod(scriptRef._sMethod, 52);
							obj = method.Invoke(grammar, array);
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x000240FC File Offset: 0x000230FC
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

		// Token: 0x06000807 RID: 2055 RVA: 0x00024184 File Offset: 0x00023184
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

		// Token: 0x06000808 RID: 2056 RVA: 0x000241D8 File Offset: 0x000231D8
		private static int NextReplacementWord(Collection<ReplacementText> replacements, out ReplacementText replacement, ref int posInCollection)
		{
			if (posInCollection < replacements.Count)
			{
				replacement = replacements[posInCollection++];
				return replacement.FirstWordIndex;
			}
			replacement = null;
			return -1;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002420C File Offset: 0x0002320C
		private void AppendSml(XmlDocument document, int i, NumberFormatInfo nfo)
		{
			XmlNode documentElement = document.DocumentElement;
			XmlElement xmlElement = document.CreateElement("alternate");
			documentElement.AppendChild(xmlElement);
			xmlElement.SetAttribute("Rank", i.ToString(CultureInfo.CurrentUICulture));
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

		// Token: 0x0600080A RID: 2058 RVA: 0x000242D8 File Offset: 0x000232D8
		private void AppendPropertiesSML(XmlDocument document, XmlElement alternateNode, SemanticValue semanticsNode, NumberFormatInfo nfo)
		{
			if (semanticsNode != null)
			{
				foreach (KeyValuePair<string, SemanticValue> keyValuePair in semanticsNode)
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

		// Token: 0x0600080B RID: 2059 RVA: 0x00024484 File Offset: 0x00023484
		private string RemoveTrailingNumber(string name)
		{
			return name.Substring(0, name.LastIndexOf('_'));
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00024498 File Offset: 0x00023498
		private void AppendAttributes(XmlElement propertyNode, SemanticValue semanticValue)
		{
			foreach (KeyValuePair<string, SemanticValue> keyValuePair in semanticValue)
			{
				if (propertyNode.Attributes.get_ItemOf(keyValuePair.Key) == null)
				{
					propertyNode.SetAttribute(keyValuePair.Key, keyValuePair.Value.Value.ToString());
				}
			}
		}

		// Token: 0x0400059F RID: 1439
		private const int SpVariantSubsetOffset = 16;

		// Token: 0x040005A0 RID: 1440
		internal SPSERIALIZEDPHRASE _serializedPhrase;

		// Token: 0x040005A1 RID: 1441
		internal byte[] _phraseBuffer;

		// Token: 0x040005A2 RID: 1442
		internal bool _isSapi53Header;

		// Token: 0x040005A3 RID: 1443
		internal bool _hasIPAPronunciation;

		// Token: 0x040005A4 RID: 1444
		private RecognitionResult _recoResult;

		// Token: 0x040005A5 RID: 1445
		private GrammarOptions _grammarOptions;

		// Token: 0x040005A6 RID: 1446
		private string _text;

		// Token: 0x040005A7 RID: 1447
		private float _confidence;

		// Token: 0x040005A8 RID: 1448
		private SemanticValue _semantics;

		// Token: 0x040005A9 RID: 1449
		private ReadOnlyCollection<RecognizedWordUnit> _words;

		// Token: 0x040005AA RID: 1450
		private Collection<ReplacementText> _replacementText;

		// Token: 0x040005AB RID: 1451
		[NonSerialized]
		private ulong _grammarId = ulong.MaxValue;

		// Token: 0x040005AC RID: 1452
		[NonSerialized]
		private Grammar _grammar;

		// Token: 0x040005AD RID: 1453
		private int _homophoneGroupId;

		// Token: 0x040005AE RID: 1454
		private ReadOnlyCollection<RecognizedPhrase> _homophones;

		// Token: 0x040005AF RID: 1455
		private Collection<SemanticValue> _dupItems;

		// Token: 0x040005B0 RID: 1456
		private string _smlContent;

		// Token: 0x0200012F RID: 303
		private class RuleNode
		{
			// Token: 0x0600080D RID: 2061 RVA: 0x0002450C File Offset: 0x0002350C
			internal RuleNode(Grammar grammar, string rule, float confidence, uint first, uint count)
			{
				this._name = rule;
				this._rule = rule;
				this._firstElement = first;
				this._count = count;
				this._confidence = confidence;
				this._grammar = grammar;
			}

			// Token: 0x0600080E RID: 2062 RVA: 0x00024550 File Offset: 0x00023550
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

			// Token: 0x040005B1 RID: 1457
			internal Grammar _grammar;

			// Token: 0x040005B2 RID: 1458
			internal string _rule;

			// Token: 0x040005B3 RID: 1459
			internal string _name;

			// Token: 0x040005B4 RID: 1460
			internal uint _firstElement;

			// Token: 0x040005B5 RID: 1461
			internal uint _count;

			// Token: 0x040005B6 RID: 1462
			internal float _confidence;

			// Token: 0x040005B7 RID: 1463
			internal bool _hasName;

			// Token: 0x040005B8 RID: 1464
			internal RecognizedPhrase.RuleNode _next;

			// Token: 0x040005B9 RID: 1465
			internal RecognizedPhrase.RuleNode _child;
		}

		// Token: 0x02000130 RID: 304
		private struct ResultPropertiesRef
		{
			// Token: 0x0600080F RID: 2063 RVA: 0x000245D5 File Offset: 0x000235D5
			internal ResultPropertiesRef(string name, SemanticValue value, RecognizedPhrase.RuleNode ruleNode)
			{
				this._name = name;
				this._value = value;
				this._ruleNode = ruleNode;
			}

			// Token: 0x040005BA RID: 1466
			internal string _name;

			// Token: 0x040005BB RID: 1467
			internal SemanticValue _value;

			// Token: 0x040005BC RID: 1468
			internal RecognizedPhrase.RuleNode _ruleNode;
		}
	}
}
