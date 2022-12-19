using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Speech.Internal.SrgsParser;
using System.Text;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x0200009A RID: 154
	internal sealed class Backend
	{
		// Token: 0x06000316 RID: 790 RVA: 0x0000AC0C File Offset: 0x00009C0C
		internal Backend()
		{
			this._words = new StringBlob();
			this._symbols = new StringBlob();
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000AC88 File Offset: 0x00009C88
		internal Backend(StreamMarshaler streamHelper)
		{
			this.InitFromBinaryGrammar(streamHelper);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000ACF4 File Offset: 0x00009CF4
		internal void Optimize()
		{
			this._states.Optimize();
			this._fNeedWeightTable = true;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000AD08 File Offset: 0x00009D08
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
			if (this._il != null && this._il.Length > 0)
			{
				streamBuffer.Stream.Write(this._il, 0, this._il.Length);
			}
			if (this._pdb != null && this._pdb.Length > 0)
			{
				streamBuffer.Stream.Write(this._pdb, 0, this._pdb.Length);
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000B124 File Offset: 0x0000A124
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

		// Token: 0x0600031B RID: 795 RVA: 0x0000B1C4 File Offset: 0x0000A1C4
		internal State CreateNewState(Rule rule)
		{
			return this._states.CreateNewState(rule);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000B1D2 File Offset: 0x0000A1D2
		internal void DeleteState(State state)
		{
			this._states.DeleteState(state);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000B1E0 File Offset: 0x0000A1E0
		internal void MoveInputTransitionsAndDeleteState(State from, State to)
		{
			this._states.MoveInputTransitionsAndDeleteState(from, to);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000B1EF File Offset: 0x0000A1EF
		internal void MoveOutputTransitionsAndDeleteState(State from, State to)
		{
			this._states.MoveOutputTransitionsAndDeleteState(from, to);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000B200 File Offset: 0x0000A200
		internal Rule CreateRule(string name, SPCFGRULEATTRIBUTES attributes)
		{
			SPCFGRULEATTRIBUTES spcfgruleattributes = SPCFGRULEATTRIBUTES.SPRAF_TopLevel | SPCFGRULEATTRIBUTES.SPRAF_Active | SPCFGRULEATTRIBUTES.SPRAF_Export | SPCFGRULEATTRIBUTES.SPRAF_Import | SPCFGRULEATTRIBUTES.SPRAF_Interpreter | SPCFGRULEATTRIBUTES.SPRAF_Dynamic | SPCFGRULEATTRIBUTES.SPRAF_Root;
			if (attributes != (SPCFGRULEATTRIBUTES)0 && ((attributes & ~(spcfgruleattributes != (SPCFGRULEATTRIBUTES)0)) != (SPCFGRULEATTRIBUTES)0 || ((attributes & SPCFGRULEATTRIBUTES.SPRAF_Import) != (SPCFGRULEATTRIBUTES)0 && (attributes & SPCFGRULEATTRIBUTES.SPRAF_Export) != (SPCFGRULEATTRIBUTES)0)))
			{
				throw new ArgumentException("attributes");
			}
			if ((attributes & SPCFGRULEATTRIBUTES.SPRAF_Import) != (SPCFGRULEATTRIBUTES)0 && ((attributes & SPCFGRULEATTRIBUTES.SPRAF_TopLevel) != (SPCFGRULEATTRIBUTES)0 || (attributes & SPCFGRULEATTRIBUTES.SPRAF_Active) != (SPCFGRULEATTRIBUTES)0 || (attributes & SPCFGRULEATTRIBUTES.SPRAF_Root) != (SPCFGRULEATTRIBUTES)0))
			{
				attributes &= ~(SPCFGRULEATTRIBUTES.SPRAF_TopLevel | SPCFGRULEATTRIBUTES.SPRAF_Active | SPCFGRULEATTRIBUTES.SPRAF_Root);
			}
			if ((attributes & SPCFGRULEATTRIBUTES.SPRAF_Import) != (SPCFGRULEATTRIBUTES)0 && name.get_Chars(0) == '\0')
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
			rule2._iSerialize2 = this._ruleIndex++;
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

		// Token: 0x06000320 RID: 800 RVA: 0x0000B398 File Offset: 0x0000A398
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

		// Token: 0x06000321 RID: 801 RVA: 0x0000B434 File Offset: 0x0000A434
		internal Arc WordTransition(string sWord, float flWeight, int requiredConfidence)
		{
			return this.CreateTransition(sWord, flWeight, requiredConfidence);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000B440 File Offset: 0x0000A440
		internal Arc SubsetTransition(string text, MatchMode matchMode)
		{
			text = Backend.NormalizeTokenWhiteSpace(text);
			return new Arc(text, null, this._words, 1f, 0, null, matchMode, ref this._fNeedWeightTable);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000B470 File Offset: 0x0000A470
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

		// Token: 0x06000324 RID: 804 RVA: 0x0000B4E4 File Offset: 0x0000A4E4
		internal Arc EpsilonTransition(float flWeight)
		{
			return this.CreateTransition(null, flWeight, 0);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000B4F0 File Offset: 0x0000A4F0
		internal void AddSemanticInterpretationTag(Arc arc, CfgGrammar.CfgProperty propertyInfo)
		{
			Tag tag = new Tag(this, propertyInfo);
			this._tags.Add(tag);
			arc.AddStartTag(tag);
			arc.AddEndTag(tag);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000B520 File Offset: 0x0000A520
		internal void AddPropertyTag(Arc start, Arc end, CfgGrammar.CfgProperty propertyInfo)
		{
			Tag tag = new Tag(this, propertyInfo);
			this._tags.Add(tag);
			start.AddStartTag(tag);
			end.AddEndTag(tag);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000B550 File Offset: 0x0000A550
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

		// Token: 0x06000328 RID: 808 RVA: 0x0000B668 File Offset: 0x0000A668
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
						if (arc.RuleRef.Name.IndexOf("URL:DYNAMIC#", 4) == 0)
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
						else if (arc.RuleRef.Name.IndexOf("URL:STATIC#", 4) == 0)
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
							org.FindInRules(text);
							if (!fromOrg)
							{
								this.CloneSubGraph(arc.RuleRef, org, extra, srcToDestHash, true);
							}
						}
						Rule rule4 = this.FindInRules(text);
						if (rule4 == null)
						{
							rule4 = this.CloneState(arc.RuleRef._firstState, list, srcToDestHash);
						}
						arc2.RuleRef = rule4;
					}
					arc2.ConnectStates();
				}
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000B904 File Offset: 0x0000A904
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

		// Token: 0x0600032A RID: 810 RVA: 0x0000BA54 File Offset: 0x0000AA54
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
			rule._cfgRule.TopLevel = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_TopLevel) != (SPCFGRULEATTRIBUTES)0;
			rule._cfgRule.DefaultActive = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Active) != (SPCFGRULEATTRIBUTES)0;
			rule._cfgRule.PropRule = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Interpreter) != (SPCFGRULEATTRIBUTES)0;
			rule._cfgRule.Export = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Export) != (SPCFGRULEATTRIBUTES)0;
			rule._cfgRule.Dynamic = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Dynamic) != (SPCFGRULEATTRIBUTES)0;
			rule._cfgRule.Import = (dwAttributes & SPCFGRULEATTRIBUTES.SPRAF_Import) != (SPCFGRULEATTRIBUTES)0;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000BB00 File Offset: 0x0000AB00
		internal void SetBasePath(string sBasePath)
		{
			if (!string.IsNullOrEmpty(sBasePath))
			{
				Uri uri = new Uri(sBasePath, 0);
				this._basePath = uri.ToString();
				return;
			}
			this._basePath = null;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000BB34 File Offset: 0x0000AB34
		internal static string NormalizeTokenWhiteSpace(string sToken)
		{
			sToken = sToken.Trim(Helpers._achTrimChars);
			if (sToken.IndexOf("  ", 4) == -1)
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000BBA5 File Offset: 0x0000ABA5
		internal StringBlob Words
		{
			get
			{
				return this._words;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000BBAD File Offset: 0x0000ABAD
		internal StringBlob Symbols
		{
			get
			{
				return this._symbols;
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000BBB8 File Offset: 0x0000ABB8
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
						array2[(int)((UIntPtr)tag._cfgTag.StartArcIndex)].AddStartTag(tag);
						array2[(int)((UIntPtr)tag._cfgTag.EndArcIndex)].AddEndTag(tag);
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

		// Token: 0x06000330 RID: 816 RVA: 0x0000BFD1 File Offset: 0x0000AFD1
		private Arc CreateTransition(string sWord, float flWeight, int requiredConfidence)
		{
			return this.AddSingleWordTransition((!string.IsNullOrEmpty(sWord)) ? sWord : null, flWeight, requiredConfidence);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000BFE8 File Offset: 0x0000AFE8
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

		// Token: 0x06000332 RID: 818 RVA: 0x0000C390 File Offset: 0x0000B390
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

		// Token: 0x06000333 RID: 819 RVA: 0x0000C4FC File Offset: 0x0000B4FC
		private Rule CloneState(State srcToState, List<State> CloneStack, Dictionary<State, State> srcToDestHash)
		{
			bool flag = false;
			string text = ((srcToState.Rule.Name.IndexOf("URL:DYNAMIC#", 4) != 0) ? srcToState.Rule.Name : srcToState.Rule.Name.Substring(12));
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

		// Token: 0x06000334 RID: 820 RVA: 0x0000C598 File Offset: 0x0000B598
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

		// Token: 0x06000335 RID: 821 RVA: 0x0000C5FC File Offset: 0x0000B5FC
		private static void LogError(string rule, SRID srid, params object[] args)
		{
			string text = SR.Get(srid, args);
			throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Rule=\"{0}\" - ", new object[] { rule }) + text);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000C637 File Offset: 0x0000B637
		private static void AddArc(Arc arc)
		{
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000C63C File Offset: 0x0000B63C
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

		// Token: 0x06000338 RID: 824 RVA: 0x0000C75C File Offset: 0x0000B75C
		private void CheckLeftRecursion(List<State> states)
		{
			foreach (State state in states)
			{
				bool flag;
				state.CheckLeftRecursion(out flag);
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000C7AC File Offset: 0x0000B7AC
		private Arc AddSingleWordTransition(string s, float flWeight, int requiredConfidence)
		{
			Arc arc = new Arc(s, null, this._words, flWeight, requiredConfidence, null, MatchMode.AllWords, ref this._fNeedWeightTable);
			Backend.AddArc(arc);
			return arc;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000C7D8 File Offset: 0x0000B7D8
		internal void AddState(State state)
		{
			this._states.Add(state);
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000C7E6 File Offset: 0x0000B7E6
		// (set) Token: 0x0600033C RID: 828 RVA: 0x0000C7EE File Offset: 0x0000B7EE
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000C7F7 File Offset: 0x0000B7F7
		// (set) Token: 0x0600033E RID: 830 RVA: 0x0000C7FF File Offset: 0x0000B7FF
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

		// Token: 0x1700004B RID: 75
		// (set) Token: 0x0600033F RID: 831 RVA: 0x0000C808 File Offset: 0x0000B808
		internal GrammarType GrammarMode
		{
			set
			{
				this._grammarMode = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000C81A File Offset: 0x0000B81A
		// (set) Token: 0x06000340 RID: 832 RVA: 0x0000C811 File Offset: 0x0000B811
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000C822 File Offset: 0x0000B822
		// (set) Token: 0x06000343 RID: 835 RVA: 0x0000C82A File Offset: 0x0000B82A
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

		// Token: 0x1700004E RID: 78
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0000C833 File Offset: 0x0000B833
		internal Collection<ScriptRef> ScriptRefs
		{
			set
			{
				this._scriptRefs = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0000C83C File Offset: 0x0000B83C
		internal byte[] IL
		{
			set
			{
				this._il = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (set) Token: 0x06000346 RID: 838 RVA: 0x0000C845 File Offset: 0x0000B845
		internal byte[] PDB
		{
			set
			{
				this._pdb = value;
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000C850 File Offset: 0x0000B850
		// Note: this type is marked as 'beforefieldinit'.
		static Backend()
		{
			byte[] array = new byte[3];
			Backend._abZero3 = array;
			char[] array2 = new char[1];
			Backend._achZero = array2;
		}

		// Token: 0x040002DA RID: 730
		private const uint SPGF_RESET_DIRTY_FLAG = 2147483648U;

		// Token: 0x040002DB RID: 731
		private int _langId = CultureInfo.CurrentUICulture.LCID;

		// Token: 0x040002DC RID: 732
		private StringBlob _words;

		// Token: 0x040002DD RID: 733
		private StringBlob _symbols;

		// Token: 0x040002DE RID: 734
		private Guid _guid;

		// Token: 0x040002DF RID: 735
		private bool _fNeedWeightTable;

		// Token: 0x040002E0 RID: 736
		private Graph _states = new Graph();

		// Token: 0x040002E1 RID: 737
		private List<Rule> _rules = new List<Rule>();

		// Token: 0x040002E2 RID: 738
		private int _ruleIndex;

		// Token: 0x040002E3 RID: 739
		private Dictionary<int, Rule> _nameOffsetRules = new Dictionary<int, Rule>();

		// Token: 0x040002E4 RID: 740
		private Rule _rootRule;

		// Token: 0x040002E5 RID: 741
		private GrammarOptions _grammarOptions;

		// Token: 0x040002E6 RID: 742
		private int _ulRecursiveDepth;

		// Token: 0x040002E7 RID: 743
		private string _basePath;

		// Token: 0x040002E8 RID: 744
		private List<Tag> _tags = new List<Tag>();

		// Token: 0x040002E9 RID: 745
		private GrammarType _grammarMode;

		// Token: 0x040002EA RID: 746
		private AlphabetType _alphabet;

		// Token: 0x040002EB RID: 747
		private Collection<string> _globalTags = new Collection<string>();

		// Token: 0x040002EC RID: 748
		private static byte[] _abZero3;

		// Token: 0x040002ED RID: 749
		private static char[] _achZero;

		// Token: 0x040002EE RID: 750
		private int _cImportedRules;

		// Token: 0x040002EF RID: 751
		private Collection<ScriptRef> _scriptRefs = new Collection<ScriptRef>();

		// Token: 0x040002F0 RID: 752
		private byte[] _il;

		// Token: 0x040002F1 RID: 753
		private byte[] _pdb;

		// Token: 0x040002F2 RID: 754
		private bool _fLoadedFromBinary;
	}
}
