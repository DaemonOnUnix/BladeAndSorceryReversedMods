using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Speech.Internal.SrgsParser;
using System.Text;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000E8 RID: 232
	internal sealed class Backend
	{
		// Token: 0x060007EF RID: 2031 RVA: 0x00022DEC File Offset: 0x00020FEC
		internal Backend()
		{
			this._words = new StringBlob();
			this._symbols = new StringBlob();
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00022E68 File Offset: 0x00021068
		internal Backend(StreamMarshaler streamHelper)
		{
			this.InitFromBinaryGrammar(streamHelper);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00022ED4 File Offset: 0x000210D4
		internal void Optimize()
		{
			this._states.Optimize();
			this._fNeedWeightTable = true;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00022EE8 File Offset: 0x000210E8
		internal void Commit(StreamMarshaler streamBuffer)
		{
			long position = streamBuffer.Stream.Position;
			List<State> list = new List<State>(this._states);
			this._states = null;
			list.Sort();
			this.ValidateAndTagRules();
			this.CheckLeftRecursion(list);
			int num = ((this._basePath != null) ? (this._basePath.Length + 1) : 0);
			int num2 = 0;
			if (this._globalTags.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text in this._globalTags)
				{
					stringBuilder.Append(text);
				}
				this._symbols.Add(stringBuilder.ToString(), out num2);
				num2 = this._symbols.OffsetFromId(num2);
				if (num2 > 65535)
				{
					throw new OverflowException(SR.Get(SRID.TooManyRulesWithSemanticsGlobals, new object[0]));
				}
			}
			foreach (ScriptRef scriptRef in this._scriptRefs)
			{
				this._symbols.Add(scriptRef._sMethod, out scriptRef._idSymbol);
			}
			int num3;
			float[] array;
			CfgGrammar.CfgSerializedHeader cfgSerializedHeader = this.BuildHeader(list, num, (ushort)num2, out num3, out array);
			streamBuffer.WriteStream(cfgSerializedHeader);
			streamBuffer.WriteArrayChar(this._words.SerializeData(), this._words.SerializeSize());
			streamBuffer.WriteArrayChar(this._symbols.SerializeData(), this._symbols.SerializeSize());
			foreach (Rule rule in this._rules)
			{
				rule.Serialize(streamBuffer);
			}
			if (num > 0)
			{
				streamBuffer.WriteArrayChar(this._basePath.ToCharArray(), this._basePath.Length);
				streamBuffer.WriteArrayChar(Backend._achZero, 1);
				streamBuffer.WriteArray<byte>(Backend._abZero3, (num * 2) & 3);
			}
			streamBuffer.WriteStream(default(CfgArc));
			int num4 = 1;
			uint num5 = 1U;
			bool flag = (this.GrammarOptions & GrammarOptions.MssV1) == GrammarOptions.MssV1;
			foreach (State state in list)
			{
				state.SerializeStateEntries(streamBuffer, flag, array, ref num5, ref num4);
			}
			if (this._fNeedWeightTable)
			{
				streamBuffer.WriteArray<float>(array, num3);
			}
			if (!flag)
			{
				foreach (State state2 in list)
				{
					state2.SetEndArcIndexForTags();
				}
			}
			for (int i = this._tags.Count - 1; i >= 0; i--)
			{
				if (this._tags[i]._cfgTag.ArcIndex == 0U)
				{
					this._tags.RemoveAt(i);
				}
			}
			this._tags.Sort();
			foreach (Tag tag in this._tags)
			{
				tag.Serialize(streamBuffer);
			}
			foreach (ScriptRef scriptRef2 in this._scriptRefs)
			{
				scriptRef2.Serialize(this._symbols, streamBuffer);
			}
			if (this._il != null && this._il.Length != 0)
			{
				streamBuffer.Stream.Write(this._il, 0, this._il.Length);
			}
			if (this._pdb != null && this._pdb.Length != 0)
			{
				streamBuffer.Stream.Write(this._pdb, 0, this._pdb.Length);
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00023300 File Offset: 0x00021500
		internal static Backend CombineGrammar(string ruleName, Backend org, Backend extra)
		{
			Backend backend = new Backend();
			backend._fLoadedFromBinary = true;
			backend._fNeedWeightTable = org._fNeedWeightTable;
			backend._grammarMode = org._grammarMode;
			backend._grammarOptions = org._grammarOptions;
			Dictionary<State, State> dictionary = new Dictionary<State, State>();
			foreach (Rule rule in org._rules)
			{
				if (rule.Name == ruleName)
				{
					backend.CloneSubGraph(rule, org, extra, dictionary, true);
				}
			}
			return backend;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000233A0 File Offset: 0x000215A0
		internal State CreateNewState(Rule rule)
		{
			return this._states.CreateNewState(rule);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000233AE File Offset: 0x000215AE
		internal void DeleteState(State state)
		{
			this._states.DeleteState(state);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000233BC File Offset: 0x000215BC
		internal void MoveInputTransitionsAndDeleteState(State from, State to)
		{
			this._states.MoveInputTransitionsAndDeleteState(from, to);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000233CB File Offset: 0x000215CB
		internal void MoveOutputTransitionsAndDeleteState(State from, State to)
		{
			this._states.MoveOutputTransitionsAndDeleteState(from, to);
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000233DC File Offset: 0x000215DC
		internal Rule CreateRule(string name, SPCFGRULEATTRIBUTES attributes)
		{
			SPCFGRULEATTRIBUTES spcfgruleattributes = SPCFGRULEATTRIBUTES.SPRAF_TopLevel | SPCFGRULEATTRIBUTES.SPRAF_Active | SPCFGRULEATTRIBUTES.SPRAF_Export | SPCFGRULEATTRIBUTES.SPRAF_Import | SPCFGRULEATTRIBUTES.SPRAF_Interpreter | SPCFGRULEATTRIBUTES.SPRAF_Dynamic | SPCFGRULEATTRIBUTES.SPRAF_Root;
			if (attributes != (SPCFGRULEATTRIBUTES)0 && ((attributes & ~(spcfgruleattributes != (SPCFGRULEATTRIBUTES)0)) != (SPCFGRULEATTRIBUTES)0 || ((attributes & SPCFGRULEATTRIBUTES.SPRAF_Import) != (SPCFGRULEATTRIBUTES)0 && (attributes & SPCFGRULEATTRIBUTES.SPRAF_Export) != (SPCFGRULEATTRIBUTES)0)))
			{
				throw new ArgumentException(SR.Get(SRID.InvalidFlagsSet, new object[0]), "attributes");
			}
			if ((attributes & SPCFGRULEATTRIBUTES.SPRAF_Import) != (SPCFGRULEATTRIBUTES)0 && ((attributes & SPCFGRULEATTRIBUTES.SPRAF_TopLevel) != (SPCFGRULEATTRIBUTES)0 || (attributes & SPCFGRULEATTRIBUTES.SPRAF_Active) != (SPCFGRULEATTRIBUTES)0 || (attributes & SPCFGRULEATTRIBUTES.SPRAF_Root) != (SPCFGRULEATTRIBUTES)0))
			{
				attributes &= ~(SPCFGRULEATTRIBUTES.SPRAF_TopLevel | SPCFGRULEATTRIBUTES.SPRAF_Active | SPCFGRULEATTRIBUTES.SPRAF_Root);
			}
			if ((attributes & SPCFGRULEATTRIBUTES.SPRAF_Import) != (SPCFGRULEATTRIBUTES)0 && name[0] == '\0')
			{
				Backend.LogError(name, SRID.InvalidImport, new object[0]);
			}
			if (this._fLoadedFromBinary)
			{
				foreach (Rule rule in this._rules)
				{
					string text = this._symbols[rule._cfgRule._nameOffset];
					if (!rule._cfgRule.Dynamic && name == text)
					{
						Backend.LogError(name, SRID.DuplicatedRuleName, new object[0]);
					}
				}
			}
			int num = 0;
			int num2;
			Rule rule2 = new Rule(this, name, this._symbols.Add(name, out num2), attributes, this._ruleIndex, 0, this._grammarOptions & GrammarOptions.TagFormat, ref num);
			Rule rule3 = rule2;
			int ruleIndex = this._ruleIndex;
			this._ruleIndex = ruleIndex + 1;
			rule3._iSerialize2 = ruleIndex;
			if ((attributes & SPCFGRULEATTRIBUTES.SPRAF_Root) != (SPCFGRULEATTRIBUTES)0)
			{
				if (this._rootRule != null)
				{
					Backend.LogError(name, SRID.RootRuleAlreadyDefined, new object[0]);
				}
				else
				{
					this._rootRule = rule2;
				}
			}
			if (rule2._cfgRule._nameOffset != 0)
			{
				this._nameOffsetRules.Add(rule2._cfgRule._nameOffset, rule2);
			}
			this._rules.Add(rule2);
			this._rules.Sort();
			return rule2;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0002357C File Offset: 0x0002177C
		internal Rule FindRule(string sRule)
		{
			Rule rule = null;
			if (this._nameOffsetRules.Count > 0)
			{
				int num = this._symbols.Find(sRule);
				if (num > 0)
				{
					int num2 = this._symbols.OffsetFromId(num);
					rule = ((num2 > 0 && this._nameOffsetRules.ContainsKey(num2)) ? this._nameOffsetRules[num2] : null);
				}
			}
			if (rule != null)
			{
				string name = rule.Name;
				if (!string.IsNullOrEmpty(sRule) && (string.IsNullOrEmpty(sRule) || string.IsNullOrEmpty(name) || !(name == sRule)))
				{
					Backend.LogError(sRule, SRID.RuleNameIdConflict, new object[0]);
				}
			}
			if (rule == null)
			{
				return null;
			}
			return rule;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00023618 File Offset: 0x00021818
		internal Arc WordTransition(string sWord, float flWeight, int requiredConfidence)
		{
			return this.CreateTransition(sWord, flWeight, requiredConfidence);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00023623 File Offset: 0x00021823
		internal Arc SubsetTransition(string text, MatchMode matchMode)
		{
			text = Backend.NormalizeTokenWhiteSpace(text);
			return new Arc(text, null, this._words, 1f, 0, null, matchMode, ref this._fNeedWeightTable);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00023648 File Offset: 0x00021848
		internal Arc RuleTransition(Rule rule, Rule parentRule, float flWeight)
		{
			Rule rule2 = null;
			if (flWeight < 0f)
			{
				XmlParser.ThrowSrgsException(SRID.UnsupportedFormat, new object[0]);
			}
			Rule rule3 = null;
			if (rule == CfgGrammar.SPRULETRANS_WILDCARD || rule == CfgGrammar.SPRULETRANS_DICTATION || rule == CfgGrammar.SPRULETRANS_TEXTBUFFER)
			{
				rule3 = rule;
			}
			else
			{
				rule2 = rule;
			}
			bool flag = false;
			Arc arc = new Arc(null, rule2, this._words, flWeight, 0, rule3, MatchMode.AllWords, ref flag);
			Backend.AddArc(arc);
			if (rule2 != null && parentRule != null)
			{
				rule2._listRules.Insert(0, parentRule);
			}
			return arc;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x000236BC File Offset: 0x000218BC
		internal Arc EpsilonTransition(float flWeight)
		{
			return this.CreateTransition(null, flWeight, 0);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x000236C8 File Offset: 0x000218C8
		internal void AddSemanticInterpretationTag(Arc arc, CfgGrammar.CfgProperty propertyInfo)
		{
			Tag tag = new Tag(this, propertyInfo);
			this._tags.Add(tag);
			arc.AddStartTag(tag);
			arc.AddEndTag(tag);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000236F8 File Offset: 0x000218F8
		internal void AddPropertyTag(Arc start, Arc end, CfgGrammar.CfgProperty propertyInfo)
		{
			Tag tag = new Tag(this, propertyInfo);
			this._tags.Add(tag);
			start.AddStartTag(tag);
			end.AddEndTag(tag);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00023728 File Offset: 0x00021928
		internal State CloneSubGraph(State srcFromState, State srcEndState, State destFromState)
		{
			Dictionary<State, State> dictionary = new Dictionary<State, State>();
			Stack<State> stack = new Stack<State>();
			Dictionary<Tag, Tag> dictionary2 = new Dictionary<Tag, Tag>();
			dictionary.Add(srcFromState, destFromState);
			stack.Push(srcFromState);
			while (stack.Count > 0)
			{
				srcFromState = stack.Pop();
				destFromState = dictionary[srcFromState];
				foreach (object obj in srcFromState.OutArcs)
				{
					Arc arc = (Arc)obj;
					State end = arc.End;
					State state = null;
					if (end != null)
					{
						if (!dictionary.ContainsKey(end))
						{
							state = this.CreateNewState(end.Rule);
							dictionary.Add(end, state);
							stack.Push(end);
						}
						else
						{
							state = dictionary[end];
						}
					}
					Arc arc2 = new Arc(arc, destFromState, state);
					Backend.AddArc(arc2);
					arc2.CloneTags(arc, this._tags, dictionary2, null);
					arc2.ConnectStates();
				}
			}
			return dictionary[srcEndState];
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00023840 File Offset: 0x00021A40
		internal void CloneSubGraph(Rule rule, Backend org, Backend extra, Dictionary<State, State> srcToDestHash, bool fromOrg)
		{
			Backend backend = (fromOrg ? org : extra);
			List<State> list = new List<State>();
			Dictionary<Tag, Tag> dictionary = new Dictionary<Tag, Tag>();
			this.CloneState(rule._firstState, list, srcToDestHash);
			while (list.Count > 0)
			{
				State state = list[0];
				list.RemoveAt(0);
				State state2 = srcToDestHash[state];
				foreach (object obj in state.OutArcs)
				{
					Arc arc = (Arc)obj;
					State end = arc.End;
					State state3 = null;
					if (end != null)
					{
						if (!srcToDestHash.ContainsKey(end))
						{
							this.CloneState(end, list, srcToDestHash);
						}
						state3 = srcToDestHash[end];
					}
					int wordId = arc.WordId;
					if (backend != null && arc.WordId > 0)
					{
						this._words.Add(backend.Words[arc.WordId], out wordId);
					}
					Arc arc2 = new Arc(arc, state2, state3, wordId);
					arc2.CloneTags(arc, this._tags, dictionary, this);
					if (arc.RuleRef != null)
					{
						string text;
						if (arc.RuleRef.Name.IndexOf("URL:DYNAMIC#", StringComparison.Ordinal) == 0)
						{
							text = arc.RuleRef.Name.Substring(12);
							if (fromOrg && this.FindInRules(text) == null)
							{
								Rule rule2 = extra.FindInRules(text);
								if (rule2 == null)
								{
									XmlParser.ThrowSrgsException(SRID.DynamicRuleNotFound, new object[] { text });
								}
								this.CloneSubGraph(rule2, org, extra, srcToDestHash, false);
							}
						}
						else if (arc.RuleRef.Name.IndexOf("URL:STATIC#", StringComparison.Ordinal) == 0)
						{
							text = arc.RuleRef.Name.Substring(11);
							if (!fromOrg && this.FindInRules(text) == null)
							{
								Rule rule3 = org.FindInRules(text);
								if (rule3 == null)
								{
									XmlParser.ThrowSrgsException(SRID.DynamicRuleNotFound, new object[] { text });
								}
								this.CloneSubGraph(rule3, org, extra, srcToDestHash, true);
							}
						}
						else
						{
							text = arc.RuleRef.Name;
							Rule rule4 = org.FindInRules(text);
							if (!fromOrg)
							{
								this.CloneSubGraph(arc.RuleRef, org, extra, srcToDestHash, true);
							}
						}
						Rule rule5 = this.FindInRules(text);
						if (rule5 == null)
						{
							rule5 = this.CloneState(arc.RuleRef._firstState, list, srcToDestHash);
						}
						arc2.RuleRef = rule5;
					}
					arc2.ConnectStates();
				}
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00023AD4 File Offset: 0x00021CD4
		internal void DeleteSubGraph(State state)
		{
			Stack<State> stack = new Stack<State>();
			Collection<Arc> collection = new Collection<Arc>();
			Collection<State> collection2 = new Collection<State>();
			stack.Push(state);
			while (stack.Count > 0)
			{
				state = stack.Pop();
				collection2.Add(state);
				collection.Clear();
				foreach (object obj in state.OutArcs)
				{
					Arc arc = (Arc)obj;
					State end = arc.End;
					if (end != null && !stack.Contains(end) && !collection2.Contains(end))
					{
						stack.Push(end);
					}
					collection.Add(arc);
				}
				foreach (Arc arc2 in collection)
				{
					arc2.Start = (arc2.End = null);
				}
			}
			foreach (State state2 in collection2)
			{
				this.DeleteState(state2);
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00023C24 File Offset: 0x00021E24
		internal void SetRuleAttributes(Rule rule, SPCFGRULEATTRIBUTES dwAttributes)
		{
			if ((dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Root) != (SPCFGRULEATTRIBUTES)0)
			{
				if (this._rootRule != null)
				{
					XmlParser.ThrowSrgsException(SRID.RootRuleAlreadyDefined, new object[0]);
				}
				else
				{
					this._rootRule = rule;
				}
			}
			rule._cfgRule.TopLevel = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_TopLevel) > (SPCFGRULEATTRIBUTES)0;
			rule._cfgRule.DefaultActive = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Active) > (SPCFGRULEATTRIBUTES)0;
			rule._cfgRule.PropRule = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Interpreter) > (SPCFGRULEATTRIBUTES)0;
			rule._cfgRule.Export = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Export) > (SPCFGRULEATTRIBUTES)0;
			rule._cfgRule.Dynamic = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Dynamic) > (SPCFGRULEATTRIBUTES)0;
			rule._cfgRule.Import = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Import) > (SPCFGRULEATTRIBUTES)0;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00023CC0 File Offset: 0x00021EC0
		internal void SetBasePath(string sBasePath)
		{
			if (!string.IsNullOrEmpty(sBasePath))
			{
				Uri uri = new Uri(sBasePath, UriKind.RelativeOrAbsolute);
				this._basePath = uri.ToString();
				return;
			}
			this._basePath = null;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00023CF4 File Offset: 0x00021EF4
		internal static string NormalizeTokenWhiteSpace(string sToken)
		{
			sToken = sToken.Trim(Helpers._achTrimChars);
			if (sToken.IndexOf("  ", StringComparison.Ordinal) == -1)
			{
				return sToken;
			}
			char[] array = sToken.ToCharArray();
			int num = 0;
			int i = 0;
			while (i < array.Length)
			{
				if (array[i] == ' ')
				{
					do
					{
						i++;
					}
					while (array[i] == ' ');
					array[num++] = ' ';
				}
				else
				{
					array[num++] = array[i++];
				}
			}
			return new string(array, 0, num);
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x00023D65 File Offset: 0x00021F65
		internal StringBlob Words
		{
			get
			{
				return this._words;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x00023D6D File Offset: 0x00021F6D
		internal StringBlob Symbols
		{
			get
			{
				return this._symbols;
			}
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00023D78 File Offset: 0x00021F78
		internal void InitFromBinaryGrammar(StreamMarshaler streamHelper)
		{
			CfgGrammar.CfgHeader cfgHeader = CfgGrammar.ConvertCfgHeader(streamHelper);
			this._words = cfgHeader.pszWords;
			this._symbols = cfgHeader.pszSymbols;
			this._grammarOptions = cfgHeader.GrammarOptions;
			State[] array = new State[cfgHeader.arcs.Length];
			SortedDictionary<int, Rule> sortedDictionary = new SortedDictionary<int, Rule>();
			int count = this._rules.Count;
			this.BuildRulesFromBinaryGrammar(cfgHeader, array, sortedDictionary, count);
			Arc[] array2 = new Arc[cfgHeader.arcs.Length];
			bool flag = true;
			CfgArc cfgArc = default(CfgArc);
			State state = null;
			IEnumerator<KeyValuePair<int, Rule>> enumerator = sortedDictionary.GetEnumerator();
			if (enumerator.MoveNext())
			{
				KeyValuePair<int, Rule> keyValuePair = enumerator.Current;
				Rule rule = keyValuePair.Value;
				for (int i = 1; i < cfgHeader.arcs.Length; i++)
				{
					CfgArc cfgArc2 = cfgHeader.arcs[i];
					if (cfgArc2.RuleRef)
					{
						rule._listRules.Add(this._rules[(int)cfgArc2.TransitionIndex]);
					}
					if (keyValuePair.Key == i)
					{
						rule = keyValuePair.Value;
						if (enumerator.MoveNext())
						{
							keyValuePair = enumerator.Current;
						}
					}
					if (flag || cfgArc.LastArc)
					{
						if (array[i] == null)
						{
							uint nextHandle = CfgGrammar.NextHandle;
							array[i] = new State(rule, nextHandle, i);
							this.AddState(array[i]);
						}
						state = array[i];
					}
					int nextStartArcIndex = (int)cfgArc2.NextStartArcIndex;
					State state2 = null;
					if (state != null && nextStartArcIndex != 0)
					{
						if (array[nextStartArcIndex] == null)
						{
							uint nextHandle2 = CfgGrammar.NextHandle;
							array[nextStartArcIndex] = new State(rule, nextHandle2, nextStartArcIndex);
							this.AddState(array[nextStartArcIndex]);
						}
						state2 = array[nextStartArcIndex];
					}
					float num = ((cfgHeader.weights != null) ? cfgHeader.weights[i] : 1f);
					Arc arc;
					if (cfgArc2.RuleRef)
					{
						Rule rule2 = this._rules[(int)cfgArc2.TransitionIndex];
						arc = new Arc(null, rule2, this._words, num, 0, null, MatchMode.AllWords, ref this._fNeedWeightTable);
					}
					else
					{
						int transitionIndex = (int)cfgArc2.TransitionIndex;
						int num2 = ((transitionIndex == 4194302 || transitionIndex == 4194301 || transitionIndex == 4194303) ? transitionIndex : 0);
						arc = new Arc((int)((num2 != 0) ? 0U : cfgArc2.TransitionIndex), num, cfgArc2.LowConfRequired ? (-1) : (cfgArc2.HighConfRequired ? 1 : 0), num2, MatchMode.AllWords, ref this._fNeedWeightTable);
					}
					arc.Start = state;
					arc.End = state2;
					Backend.AddArc(arc);
					array2[i] = arc;
					flag = false;
					cfgArc = cfgArc2;
				}
			}
			int j = 1;
			int num3 = 0;
			while (j < cfgHeader.arcs.Length)
			{
				CfgArc cfgArc3 = cfgHeader.arcs[j];
				if (cfgArc3.HasSemanticTag)
				{
					while (num3 < cfgHeader.tags.Length && (ulong)cfgHeader.tags[num3].StartArcIndex == (ulong)((long)j))
					{
						CfgSemanticTag cfgSemanticTag = cfgHeader.tags[num3];
						Tag tag = new Tag(this, cfgSemanticTag);
						this._tags.Add(tag);
						array2[(int)tag._cfgTag.StartArcIndex].AddStartTag(tag);
						array2[(int)tag._cfgTag.EndArcIndex].AddEndTag(tag);
						if (cfgSemanticTag._nameOffset > 0)
						{
							tag._cfgTag._nameOffset = this._symbols.OffsetFromId(this._symbols.Find(this._symbols.FromOffset(cfgSemanticTag._nameOffset)));
						}
						else
						{
							tag._cfgTag._valueOffset = this._symbols.OffsetFromId(this._symbols.Find(this._symbols.FromOffset(cfgSemanticTag._valueOffset)));
						}
						num3++;
					}
				}
				j++;
			}
			this._fNeedWeightTable = true;
			if (cfgHeader.BasePath != null)
			{
				this.SetBasePath(cfgHeader.BasePath);
			}
			this._guid = cfgHeader.GrammarGUID;
			this._langId = (int)cfgHeader.langId;
			this._grammarMode = cfgHeader.GrammarMode;
			this._fLoadedFromBinary = true;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002416D File Offset: 0x0002236D
		private Arc CreateTransition(string sWord, float flWeight, int requiredConfidence)
		{
			return this.AddSingleWordTransition((!string.IsNullOrEmpty(sWord)) ? sWord : null, flWeight, requiredConfidence);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00024184 File Offset: 0x00022384
		private CfgGrammar.CfgSerializedHeader BuildHeader(List<State> sortedStates, int cBasePath, ushort iSemanticGlobals, out int cArcs, out float[] pWeights)
		{
			cArcs = 1;
			pWeights = null;
			int num = 0;
			int num2 = 0;
			foreach (State state in sortedStates)
			{
				state.SerializeId = cArcs;
				int numArcs = state.NumArcs;
				cArcs += numArcs;
				if (num2 < numArcs)
				{
					num2 = numArcs;
				}
				num += state.NumSemanticTags;
			}
			CfgGrammar.CfgSerializedHeader cfgSerializedHeader = new CfgGrammar.CfgSerializedHeader();
			uint num3 = (uint)Marshal.SizeOf(typeof(CfgGrammar.CfgSerializedHeader));
			cfgSerializedHeader.FormatId = CfgGrammar._SPGDF_ContextFree;
			this._guid = Guid.NewGuid();
			cfgSerializedHeader.GrammarGUID = this._guid;
			cfgSerializedHeader.LangID = (ushort)this._langId;
			cfgSerializedHeader.pszSemanticInterpretationGlobals = iSemanticGlobals;
			cfgSerializedHeader.cArcsInLargestState = num2;
			cfgSerializedHeader.cchWords = this._words.StringSize();
			cfgSerializedHeader.cWords = this._words.Count;
			if (cfgSerializedHeader.cWords > 0)
			{
				cfgSerializedHeader.cWords++;
			}
			cfgSerializedHeader.pszWords = num3;
			num3 += (uint)(this._words.SerializeSize() * 2);
			cfgSerializedHeader.cchSymbols = this._symbols.StringSize();
			cfgSerializedHeader.pszSymbols = num3;
			num3 += (uint)(this._symbols.SerializeSize() * 2);
			cfgSerializedHeader.cRules = this._rules.Count;
			cfgSerializedHeader.pRules = num3;
			num3 += (uint)(this._rules.Count * Marshal.SizeOf(typeof(CfgRule)));
			cfgSerializedHeader.cBasePath = ((cBasePath > 0) ? num3 : 0U);
			num3 += (uint)((cBasePath * 2 + 3) & -4);
			cfgSerializedHeader.cArcs = cArcs;
			cfgSerializedHeader.pArcs = num3;
			num3 += (uint)(cArcs * Marshal.SizeOf(typeof(CfgArc)));
			if (this._fNeedWeightTable)
			{
				cfgSerializedHeader.pWeights = num3;
				num3 += (uint)(cArcs * Marshal.SizeOf(typeof(float)));
				pWeights = new float[cArcs];
				pWeights[0] = 0f;
			}
			else
			{
				cfgSerializedHeader.pWeights = 0U;
				num3 = num3;
			}
			if (this._rootRule != null)
			{
				cfgSerializedHeader.ulRootRuleIndex = (uint)this._rootRule._iSerialize;
			}
			else
			{
				cfgSerializedHeader.ulRootRuleIndex = uint.MaxValue;
			}
			cfgSerializedHeader.GrammarOptions = this._grammarOptions | ((this._alphabet == AlphabetType.Sapi) ? GrammarOptions.KeyValuePairs : GrammarOptions.IpaPhoneme);
			cfgSerializedHeader.GrammarOptions |= ((this._scriptRefs.Count > 0) ? (GrammarOptions.KeyValuePairSrgs | GrammarOptions.STG) : GrammarOptions.KeyValuePairs);
			cfgSerializedHeader.GrammarMode = (uint)this._grammarMode;
			cfgSerializedHeader.cTags = num;
			cfgSerializedHeader.tags = num3;
			num3 += (uint)(num * Marshal.SizeOf(typeof(CfgSemanticTag)));
			cfgSerializedHeader.cScripts = this._scriptRefs.Count;
			cfgSerializedHeader.pScripts = ((cfgSerializedHeader.cScripts > 0) ? num3 : 0U);
			num3 += (uint)(this._scriptRefs.Count * Marshal.SizeOf(typeof(CfgScriptRef)));
			cfgSerializedHeader.cIL = ((this._il != null) ? this._il.Length : 0);
			cfgSerializedHeader.pIL = ((cfgSerializedHeader.cIL > 0) ? num3 : 0U);
			num3 += (uint)(cfgSerializedHeader.cIL * Marshal.SizeOf(typeof(byte)));
			cfgSerializedHeader.cPDB = ((this._pdb != null) ? this._pdb.Length : 0);
			cfgSerializedHeader.pPDB = ((cfgSerializedHeader.cPDB > 0) ? num3 : 0U);
			num3 += (uint)(cfgSerializedHeader.cPDB * Marshal.SizeOf(typeof(byte)));
			cfgSerializedHeader.ulTotalSerializedSize = num3;
			return cfgSerializedHeader;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000244EC File Offset: 0x000226EC
		private CfgGrammar.CfgHeader BuildRulesFromBinaryGrammar(CfgGrammar.CfgHeader header, State[] apStateTable, SortedDictionary<int, Rule> ruleFirstArcs, int previousCfgLastRules)
		{
			for (int i = 0; i < header.rules.Length; i++)
			{
				CfgRule cfgRule = header.rules[i];
				int firstArcIndex = (int)cfgRule.FirstArcIndex;
				cfgRule._nameOffset = this._symbols.OffsetFromId(this._symbols.Find(header.pszSymbols.FromOffset(cfgRule._nameOffset)));
				Rule rule = new Rule(this, this._symbols.FromOffset(cfgRule._nameOffset), cfgRule, i + previousCfgLastRules, this._grammarOptions & GrammarOptions.TagFormat, ref this._cImportedRules);
				rule._firstState = this._states.CreateNewState(rule);
				this._rules.Add(rule);
				if (firstArcIndex > 0)
				{
					ruleFirstArcs.Add((int)cfgRule.FirstArcIndex, rule);
				}
				rule._fStaticRule = !cfgRule.Dynamic;
				rule._cfgRule.DirtyRule = false;
				rule._fHasExitPath = rule._fStaticRule;
				if (firstArcIndex != 0)
				{
					rule._firstState.SerializeId = (int)cfgRule.FirstArcIndex;
					apStateTable[firstArcIndex] = rule._firstState;
				}
				if (rule._cfgRule.HasResources)
				{
					throw new NotImplementedException();
				}
				if ((ulong)header.ulRootRuleIndex == (ulong)((long)i))
				{
					this._rootRule = rule;
				}
				if (rule._cfgRule._nameOffset != 0)
				{
					this._nameOffsetRules.Add(rule._cfgRule._nameOffset, rule);
				}
			}
			return header;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0002464C File Offset: 0x0002284C
		private Rule CloneState(State srcToState, List<State> CloneStack, Dictionary<State, State> srcToDestHash)
		{
			bool flag = false;
			string text = ((srcToState.Rule.Name.IndexOf("URL:DYNAMIC#", StringComparison.Ordinal) != 0) ? srcToState.Rule.Name : srcToState.Rule.Name.Substring(12));
			Rule rule = this.FindInRules(text);
			if (rule == null)
			{
				rule = srcToState.Rule.Clone(this._symbols, text);
				this._rules.Add(rule);
				flag = true;
			}
			State state = this.CreateNewState(rule);
			srcToDestHash.Add(srcToState, state);
			CloneStack.Add(srcToState);
			if (flag)
			{
				rule._firstState = state;
			}
			return rule;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000246E8 File Offset: 0x000228E8
		private Rule FindInRules(string ruleName)
		{
			foreach (Rule rule in this._rules)
			{
				if (rule.Name == ruleName)
				{
					return rule;
				}
			}
			return null;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0002474C File Offset: 0x0002294C
		private static void LogError(string rule, SRID srid, params object[] args)
		{
			string text = SR.Get(srid, args);
			throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Rule=\"{0}\" - ", new object[] { rule }) + text);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0000BB6D File Offset: 0x00009D6D
		private static void AddArc(Arc arc)
		{
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00024788 File Offset: 0x00022988
		private void ValidateAndTagRules()
		{
			bool flag = false;
			int num = 0;
			foreach (Rule rule in this._rules)
			{
				rule._fHasExitPath |= rule._cfgRule.Dynamic | rule._cfgRule.Import;
				rule._iSerialize = num++;
				flag |= rule._cfgRule.Dynamic || rule._cfgRule.TopLevel || rule._cfgRule.Export;
				rule.Validate();
			}
			foreach (Rule rule2 in this._rules)
			{
				if (rule2._cfgRule.Dynamic)
				{
					rule2._cfgRule.HasDynamicRef = true;
					this._ulRecursiveDepth = 0;
					rule2.PopulateDynamicRef(ref this._ulRecursiveDepth);
				}
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x000248AC File Offset: 0x00022AAC
		private void CheckLeftRecursion(List<State> states)
		{
			foreach (State state in states)
			{
				bool flag;
				state.CheckLeftRecursion(out flag);
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x000248FC File Offset: 0x00022AFC
		private Arc AddSingleWordTransition(string s, float flWeight, int requiredConfidence)
		{
			Arc arc = new Arc(s, null, this._words, flWeight, requiredConfidence, null, MatchMode.AllWords, ref this._fNeedWeightTable);
			Backend.AddArc(arc);
			return arc;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00024928 File Offset: 0x00022B28
		internal void AddState(State state)
		{
			this._states.Add(state);
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00024936 File Offset: 0x00022B36
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x0002493E File Offset: 0x00022B3E
		internal int LangId
		{
			get
			{
				return this._langId;
			}
			set
			{
				this._langId = value;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x00024947 File Offset: 0x00022B47
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x0002494F File Offset: 0x00022B4F
		internal GrammarOptions GrammarOptions
		{
			get
			{
				return this._grammarOptions;
			}
			set
			{
				this._grammarOptions = value;
			}
		}

		// Token: 0x1700019E RID: 414
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x00024958 File Offset: 0x00022B58
		internal GrammarType GrammarMode
		{
			set
			{
				this._grammarMode = value;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0002496A File Offset: 0x00022B6A
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x00024961 File Offset: 0x00022B61
		internal AlphabetType Alphabet
		{
			get
			{
				return this._alphabet;
			}
			set
			{
				this._alphabet = value;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x00024972 File Offset: 0x00022B72
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x0002497A File Offset: 0x00022B7A
		internal Collection<string> GlobalTags
		{
			get
			{
				return this._globalTags;
			}
			set
			{
				this._globalTags = value;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x00024983 File Offset: 0x00022B83
		internal Collection<ScriptRef> ScriptRefs
		{
			set
			{
				this._scriptRefs = value;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0002498C File Offset: 0x00022B8C
		internal byte[] IL
		{
			set
			{
				this._il = value;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00024995 File Offset: 0x00022B95
		internal byte[] PDB
		{
			set
			{
				this._pdb = value;
			}
		}

		// Token: 0x040005BF RID: 1471
		private int _langId = CultureInfo.CurrentUICulture.LCID;

		// Token: 0x040005C0 RID: 1472
		private StringBlob _words;

		// Token: 0x040005C1 RID: 1473
		private StringBlob _symbols;

		// Token: 0x040005C2 RID: 1474
		private Guid _guid;

		// Token: 0x040005C3 RID: 1475
		private bool _fNeedWeightTable;

		// Token: 0x040005C4 RID: 1476
		private Graph _states = new Graph();

		// Token: 0x040005C5 RID: 1477
		private List<Rule> _rules = new List<Rule>();

		// Token: 0x040005C6 RID: 1478
		private int _ruleIndex;

		// Token: 0x040005C7 RID: 1479
		private Dictionary<int, Rule> _nameOffsetRules = new Dictionary<int, Rule>();

		// Token: 0x040005C8 RID: 1480
		private Rule _rootRule;

		// Token: 0x040005C9 RID: 1481
		private GrammarOptions _grammarOptions;

		// Token: 0x040005CA RID: 1482
		private int _ulRecursiveDepth;

		// Token: 0x040005CB RID: 1483
		private string _basePath;

		// Token: 0x040005CC RID: 1484
		private List<Tag> _tags = new List<Tag>();

		// Token: 0x040005CD RID: 1485
		private GrammarType _grammarMode;

		// Token: 0x040005CE RID: 1486
		private AlphabetType _alphabet;

		// Token: 0x040005CF RID: 1487
		private Collection<string> _globalTags = new Collection<string>();

		// Token: 0x040005D0 RID: 1488
		private static byte[] _abZero3 = new byte[3];

		// Token: 0x040005D1 RID: 1489
		private static char[] _achZero = new char[1];

		// Token: 0x040005D2 RID: 1490
		private const uint SPGF_RESET_DIRTY_FLAG = 2147483648U;

		// Token: 0x040005D3 RID: 1491
		private int _cImportedRules;

		// Token: 0x040005D4 RID: 1492
		private Collection<ScriptRef> _scriptRefs = new Collection<ScriptRef>();

		// Token: 0x040005D5 RID: 1493
		private byte[] _il;

		// Token: 0x040005D6 RID: 1494
		private byte[] _pdb;

		// Token: 0x040005D7 RID: 1495
		private bool _fLoadedFromBinary;
	}
}
