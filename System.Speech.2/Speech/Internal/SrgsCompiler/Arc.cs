using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000E5 RID: 229
	internal class Arc : IComparer<Arc>, IComparable<Arc>
	{
		// Token: 0x060007C2 RID: 1986 RVA: 0x00003BF5 File Offset: 0x00001DF5
		internal Arc()
		{
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002210C File Offset: 0x0002030C
		internal Arc(Arc arc)
			: this()
		{
			this._start = arc._start;
			this._end = arc._end;
			this._iWord = arc._iWord;
			this._flWeight = arc._flWeight;
			this._confidence = arc._confidence;
			this._ruleRef = arc._ruleRef;
			this._specialTransitionIndex = arc._specialTransitionIndex;
			this._iSerialize = arc._iSerialize;
			this._matchMode = arc._matchMode;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0002218B File Offset: 0x0002038B
		internal Arc(Arc arc, State start, State end)
			: this(arc)
		{
			this._start = start;
			this._end = end;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x000221A2 File Offset: 0x000203A2
		internal Arc(Arc arc, State start, State end, int wordId)
			: this(arc, start, end)
		{
			this._iWord = wordId;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x000221B8 File Offset: 0x000203B8
		internal Arc(string sWord, Rule ruleRef, StringBlob words, float flWeight, int confidence, Rule specialRule, MatchMode matchMode, ref bool fNeedWeightTable)
			: this(sWord, ruleRef, words, flWeight, confidence, specialRule, Arc._serializeToken++, matchMode, ref fNeedWeightTable)
		{
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x000221E8 File Offset: 0x000203E8
		private Arc(string sWord, Rule ruleRef, StringBlob words, float flWeight, int confidence, Rule specialRule, uint iSerialize, MatchMode matchMode, ref bool fNeedWeightTable)
			: this(0, flWeight, confidence, 0, matchMode, ref fNeedWeightTable)
		{
			this._ruleRef = ruleRef;
			this._iSerialize = iSerialize;
			if (ruleRef == null)
			{
				if (specialRule != null)
				{
					this._specialTransitionIndex = ((specialRule == CfgGrammar.SPRULETRANS_WILDCARD) ? 4194302 : ((specialRule == CfgGrammar.SPRULETRANS_DICTATION) ? 4194301 : 4194303));
					return;
				}
				words.Add(sWord, out this._iWord);
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00022258 File Offset: 0x00020458
		internal Arc(int iWord, float flWeight, int confidence, int ulSpecialTransitionIndex, MatchMode matchMode, ref bool fNeedWeightTable)
			: this()
		{
			this._confidence = confidence;
			this._iWord = iWord;
			this._flWeight = flWeight;
			this._matchMode = matchMode;
			this._iSerialize = Arc._serializeToken++;
			if (!flWeight.Equals(1f))
			{
				fNeedWeightTable |= true;
			}
			this._specialTransitionIndex = ulSpecialTransitionIndex;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x000222B9 File Offset: 0x000204B9
		public int CompareTo(Arc obj1)
		{
			return this.Compare(this, obj1);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000222C3 File Offset: 0x000204C3
		int IComparer<Arc>.Compare(Arc obj1, Arc obj2)
		{
			return this.Compare(obj1, obj2);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000222D0 File Offset: 0x000204D0
		private int Compare(Arc obj1, Arc obj2)
		{
			if (obj1 == obj2)
			{
				return 0;
			}
			if (obj1 == null)
			{
				return -1;
			}
			if (obj2 == null)
			{
				return 1;
			}
			int num = obj1.SortRank() - obj2.SortRank();
			return (num != 0) ? num : ((int)(obj1._iSerialize - obj2._iSerialize));
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00022314 File Offset: 0x00020514
		internal static int CompareContent(Arc arc1, Arc arc2)
		{
			if (arc1._iWord != arc2._iWord)
			{
				return arc1._iWord - arc2._iWord;
			}
			if (arc1._ruleRef != null || arc2._ruleRef != null || arc1._specialTransitionIndex - arc2._specialTransitionIndex + (arc1._confidence - arc2._confidence) != 0)
			{
				int num = 0;
				if (arc1._ruleRef != null || arc2._ruleRef != null)
				{
					if (arc1._ruleRef != null && arc2._ruleRef == null)
					{
						num = -1;
					}
					else if (arc1._ruleRef == null && arc2._ruleRef != null)
					{
						num = 1;
					}
					else
					{
						num = string.Compare(arc1._ruleRef.Name, arc2._ruleRef.Name, StringComparison.CurrentCulture);
					}
				}
				if (num != 0)
				{
					return num;
				}
				if (arc1._specialTransitionIndex != arc2._specialTransitionIndex)
				{
					return arc1._specialTransitionIndex - arc2._specialTransitionIndex;
				}
				if (arc1._confidence != arc2._confidence)
				{
					return arc1._confidence - arc2._confidence;
				}
			}
			return 0;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00022404 File Offset: 0x00020604
		internal static int CompareContentForKey(Arc arc1, Arc arc2)
		{
			int num = Arc.CompareContent(arc1, arc2);
			if (num == 0)
			{
				return (int)(arc1._iSerialize - arc2._iSerialize);
			}
			return num;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002242C File Offset: 0x0002062C
		internal float Serialize(StreamMarshaler streamBuffer, bool isLast, uint arcIndex)
		{
			CfgArc cfgArc = default(CfgArc);
			cfgArc.LastArc = isLast;
			cfgArc.HasSemanticTag = this.SemanticTagCount > 0;
			cfgArc.NextStartArcIndex = (uint)((this._end != null) ? this._end.SerializeId : 0);
			if (this._ruleRef != null)
			{
				cfgArc.RuleRef = true;
				cfgArc.TransitionIndex = (uint)this._ruleRef._iSerialize;
			}
			else
			{
				cfgArc.RuleRef = false;
				if (this._specialTransitionIndex != 0)
				{
					cfgArc.TransitionIndex = (uint)this._specialTransitionIndex;
				}
				else
				{
					cfgArc.TransitionIndex = (uint)this._iWord;
				}
			}
			cfgArc.LowConfRequired = this._confidence < 0;
			cfgArc.HighConfRequired = this._confidence > 0;
			cfgArc.MatchMode = (uint)this._matchMode;
			this._iSerialize = arcIndex;
			streamBuffer.WriteStream(cfgArc);
			return this._flWeight;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002250C File Offset: 0x0002070C
		internal static float SerializeExtraEpsilonWithTag(StreamMarshaler streamBuffer, Arc arc, bool isLast, uint arcIndex)
		{
			streamBuffer.WriteStream(new CfgArc
			{
				LastArc = isLast,
				HasSemanticTag = true,
				NextStartArcIndex = arcIndex,
				TransitionIndex = 0U,
				LowConfRequired = false,
				HighConfRequired = false,
				MatchMode = (uint)arc._matchMode
			});
			return arc._flWeight;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00022570 File Offset: 0x00020770
		internal void SetArcIndexForTag(int iArc, uint iArcOffset, bool tagsCannotSpanOverMultipleArcs)
		{
			this._startTags[iArc]._cfgTag.StartArcIndex = iArcOffset;
			this._startTags[iArc]._cfgTag.ArcIndex = iArcOffset;
			if (tagsCannotSpanOverMultipleArcs)
			{
				this._startTags[iArc]._cfgTag.EndArcIndex = iArcOffset;
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x000225C8 File Offset: 0x000207C8
		internal void SetEndArcIndexForTags()
		{
			if (this._endTags != null)
			{
				foreach (Tag tag in this._endTags)
				{
					tag._cfgTag.EndArcIndex = this._iSerialize;
				}
			}
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00022628 File Offset: 0x00020828
		internal static int CompareForDuplicateInputTransitions(Arc arc1, Arc arc2)
		{
			int num = Arc.CompareContent(arc1, arc2);
			if (num != 0)
			{
				return num;
			}
			return (int)(((arc1._start != null) ? arc1._start.Id : 0U) - ((arc2._start != null) ? arc2._start.Id : 0U));
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00022670 File Offset: 0x00020870
		internal static int CompareForDuplicateOutputTransitions(Arc arc1, Arc arc2)
		{
			int num = Arc.CompareContent(arc1, arc2);
			if (num != 0)
			{
				return num;
			}
			return (int)(((arc1._end != null) ? arc1._end.Id : 0U) - ((arc2._end != null) ? arc2._end.Id : 0U));
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000226B8 File Offset: 0x000208B8
		internal static int CompareIdenticalTransitions(Arc arc1, Arc arc2)
		{
			int num = (int)(((arc1._start != null) ? arc1._start.Id : 0U) - ((arc2._start != null) ? arc2._start.Id : 0U));
			if (num == 0 && (num = (int)(((arc1._end != null) ? arc1._end.Id : 0U) - ((arc2._end != null) ? arc2._end.Id : 0U))) == 0)
			{
				num = (arc1.SameTags(arc2) ? 0 : 1);
			}
			return num;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00022736 File Offset: 0x00020936
		internal void AddStartTag(Tag tag)
		{
			if (this._startTags == null)
			{
				this._startTags = new Collection<Tag>();
			}
			this._startTags.Add(tag);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00022757 File Offset: 0x00020957
		internal void AddEndTag(Tag tag)
		{
			if (this._endTags == null)
			{
				this._endTags = new Collection<Tag>();
			}
			this._endTags.Add(tag);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00022778 File Offset: 0x00020978
		internal void ClearTags()
		{
			this._startTags = null;
			this._endTags = null;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00022788 File Offset: 0x00020988
		internal static void CopyTags(Arc src, Arc dest, Direction move)
		{
			if (src._startTags != null)
			{
				if (dest._startTags == null)
				{
					dest._startTags = src._startTags;
				}
				else if (move == Direction.Right)
				{
					for (int i = 0; i < src._startTags.Count; i++)
					{
						dest._startTags.Insert(i, src._startTags[i]);
					}
				}
				else
				{
					foreach (Tag tag in src._startTags)
					{
						dest._startTags.Add(tag);
					}
				}
			}
			if (src._endTags != null)
			{
				if (dest._endTags == null)
				{
					dest._endTags = src._endTags;
				}
				else if (move == Direction.Right)
				{
					for (int j = 0; j < src._endTags.Count; j++)
					{
						dest._endTags.Insert(j, src._endTags[j]);
					}
				}
				else
				{
					foreach (Tag tag2 in src._endTags)
					{
						dest._endTags.Add(tag2);
					}
				}
			}
			src._startTags = (src._endTags = null);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000228DC File Offset: 0x00020ADC
		internal void CloneTags(Arc arc, List<Tag> _tags, Dictionary<Tag, Tag> endArcs, Backend be)
		{
			if (arc._startTags != null)
			{
				if (this._startTags == null)
				{
					this._startTags = new Collection<Tag>();
				}
				foreach (Tag tag in arc._startTags)
				{
					Tag tag2 = new Tag(tag);
					_tags.Add(tag2);
					this._startTags.Add(tag2);
					endArcs.Add(tag, tag2);
					if (be != null)
					{
						int num;
						tag2._cfgTag._nameOffset = be.Symbols.Add(tag._be.Symbols.FromOffset(tag._cfgTag._nameOffset), out num);
						if (tag._cfgTag._valueOffset != 0 && tag._cfgTag.PropVariantType == VarEnum.VT_EMPTY)
						{
							tag2._cfgTag._valueOffset = be.Symbols.Add(tag._be.Symbols.FromOffset(tag._cfgTag._valueOffset), out num);
						}
					}
				}
			}
			if (arc._endTags != null)
			{
				if (this._endTags == null)
				{
					this._endTags = new Collection<Tag>();
				}
				foreach (Tag tag3 in arc._endTags)
				{
					Tag tag4 = endArcs[tag3];
					this._endTags.Add(tag4);
					endArcs.Remove(tag3);
				}
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00022A68 File Offset: 0x00020C68
		internal bool SameTags(Arc arc)
		{
			bool flag = this._startTags == null && arc._startTags == null;
			if (!flag && this._startTags != null && arc._startTags != null && this._startTags.Count == arc._startTags.Count)
			{
				flag = true;
				for (int i = 0; i < this._startTags.Count; i++)
				{
					flag &= this._startTags[i] == arc._startTags[i];
				}
			}
			if (flag)
			{
				flag = this._endTags == null && arc._endTags == null;
				if (!flag && this._endTags != null && arc._endTags != null && this._endTags.Count == arc._endTags.Count)
				{
					flag = true;
					for (int j = 0; j < this._endTags.Count; j++)
					{
						flag &= this._endTags[j] == arc._endTags[j];
					}
				}
			}
			return flag;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00022B63 File Offset: 0x00020D63
		internal void ConnectStates()
		{
			if (this._end != null)
			{
				this._end.InArcs.Add(this);
			}
			if (this._start != null)
			{
				this._start.OutArcs.Add(this);
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00022B97 File Offset: 0x00020D97
		internal bool IsEpsilonTransition
		{
			get
			{
				return this._ruleRef == null && this._specialTransitionIndex == 0 && this._iWord == 0;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00022BB4 File Offset: 0x00020DB4
		internal bool IsPropertylessTransition
		{
			get
			{
				return this._startTags == null && this._endTags == null;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00022BC9 File Offset: 0x00020DC9
		internal int SemanticTagCount
		{
			get
			{
				if (this._startTags != null)
				{
					return this._startTags.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x00022C2F File Offset: 0x00020E2F
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x00022BE0 File Offset: 0x00020DE0
		internal State Start
		{
			get
			{
				return this._start;
			}
			set
			{
				if (value != this._start)
				{
					if (this._start != null)
					{
						this._start.OutArcs.Remove(this);
					}
					this._start = value;
					if (this._start != null)
					{
						this._start.OutArcs.Add(this);
					}
				}
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00022C87 File Offset: 0x00020E87
		// (set) Token: 0x060007E1 RID: 2017 RVA: 0x00022C38 File Offset: 0x00020E38
		internal State End
		{
			get
			{
				return this._end;
			}
			set
			{
				if (value != this._end)
				{
					if (this._end != null)
					{
						this._end.InArcs.Remove(this);
					}
					this._end = value;
					if (this._end != null)
					{
						this._end.InArcs.Add(this);
					}
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00022C8F File Offset: 0x00020E8F
		internal int WordId
		{
			get
			{
				return this._iWord;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00022CE6 File Offset: 0x00020EE6
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x00022C98 File Offset: 0x00020E98
		internal Rule RuleRef
		{
			get
			{
				return this._ruleRef;
			}
			set
			{
				if ((this._start != null && !this._start.OutArcs.IsEmpty) || (this._end != null && !this._end.InArcs.IsEmpty))
				{
					throw new InvalidOperationException();
				}
				this._ruleRef = value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00022CF7 File Offset: 0x00020EF7
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x00022CEE File Offset: 0x00020EEE
		internal float Weight
		{
			get
			{
				return this._flWeight;
			}
			set
			{
				this._flWeight = value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00022CFF File Offset: 0x00020EFF
		internal int SpecialTransitionIndex
		{
			get
			{
				return this._specialTransitionIndex;
			}
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00022D08 File Offset: 0x00020F08
		private int SortRank()
		{
			int num = 0;
			if (this._ruleRef != null)
			{
				num = 16777216 + this._ruleRef._cfgRule._nameOffset;
			}
			if (this._iWord != 0)
			{
				num += 33554432 + this._iWord;
			}
			if (this._specialTransitionIndex != 0)
			{
				num += 50331648;
			}
			return num;
		}

		// Token: 0x040005B0 RID: 1456
		private State _start;

		// Token: 0x040005B1 RID: 1457
		private State _end;

		// Token: 0x040005B2 RID: 1458
		private int _iWord;

		// Token: 0x040005B3 RID: 1459
		private Rule _ruleRef;

		// Token: 0x040005B4 RID: 1460
		private int _specialTransitionIndex;

		// Token: 0x040005B5 RID: 1461
		private float _flWeight;

		// Token: 0x040005B6 RID: 1462
		private MatchMode _matchMode;

		// Token: 0x040005B7 RID: 1463
		private int _confidence;

		// Token: 0x040005B8 RID: 1464
		private uint _iSerialize;

		// Token: 0x040005B9 RID: 1465
		private Collection<Tag> _startTags;

		// Token: 0x040005BA RID: 1466
		private Collection<Tag> _endTags;

		// Token: 0x040005BB RID: 1467
		private static uint _serializeToken = 1U;
	}
}
