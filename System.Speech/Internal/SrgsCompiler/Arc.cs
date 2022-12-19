using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x02000097 RID: 151
	internal class Arc : IComparer<Arc>, IComparable<Arc>
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x00009F1F File Offset: 0x00008F1F
		internal Arc()
		{
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00009F28 File Offset: 0x00008F28
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

		// Token: 0x060002EB RID: 747 RVA: 0x00009FA7 File Offset: 0x00008FA7
		internal Arc(Arc arc, State start, State end)
			: this(arc)
		{
			this._start = start;
			this._end = end;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00009FBE File Offset: 0x00008FBE
		internal Arc(Arc arc, State start, State end, int wordId)
			: this(arc, start, end)
		{
			this._iWord = wordId;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00009FD4 File Offset: 0x00008FD4
		internal Arc(string sWord, Rule ruleRef, StringBlob words, float flWeight, int confidence, Rule specialRule, MatchMode matchMode, ref bool fNeedWeightTable)
			: this(sWord, ruleRef, words, flWeight, confidence, specialRule, Arc._serializeToken++, matchMode, ref fNeedWeightTable)
		{
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000A004 File Offset: 0x00009004
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

		// Token: 0x060002EF RID: 751 RVA: 0x0000A074 File Offset: 0x00009074
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
				bool flag = fNeedWeightTable;
				fNeedWeightTable = true;
			}
			this._specialTransitionIndex = ulSpecialTransitionIndex;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000A0D4 File Offset: 0x000090D4
		public int CompareTo(Arc obj1)
		{
			return this.Compare(this, obj1);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000A0DE File Offset: 0x000090DE
		int IComparer<Arc>.Compare(Arc obj1, Arc obj2)
		{
			return this.Compare(obj1, obj2);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000A0E8 File Offset: 0x000090E8
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

		// Token: 0x060002F3 RID: 755 RVA: 0x0000A12C File Offset: 0x0000912C
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
						num = arc1._ruleRef.Name.CompareTo(arc2._ruleRef.Name);
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

		// Token: 0x060002F4 RID: 756 RVA: 0x0000A21C File Offset: 0x0000921C
		internal static int CompareContentForKey(Arc arc1, Arc arc2)
		{
			int num = Arc.CompareContent(arc1, arc2);
			if (num == 0)
			{
				return (int)(arc1._iSerialize - arc2._iSerialize);
			}
			return num;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000A244 File Offset: 0x00009244
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

		// Token: 0x060002F6 RID: 758 RVA: 0x0000A324 File Offset: 0x00009324
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

		// Token: 0x060002F7 RID: 759 RVA: 0x0000A388 File Offset: 0x00009388
		internal void SetArcIndexForTag(int iArc, uint iArcOffset, bool tagsCannotSpanOverMultipleArcs)
		{
			this._startTags[iArc]._cfgTag.StartArcIndex = iArcOffset;
			this._startTags[iArc]._cfgTag.ArcIndex = iArcOffset;
			if (tagsCannotSpanOverMultipleArcs)
			{
				this._startTags[iArc]._cfgTag.EndArcIndex = iArcOffset;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000A3E0 File Offset: 0x000093E0
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

		// Token: 0x060002F9 RID: 761 RVA: 0x0000A440 File Offset: 0x00009440
		internal static int CompareForDuplicateInputTransitions(Arc arc1, Arc arc2)
		{
			int num = Arc.CompareContent(arc1, arc2);
			if (num != 0)
			{
				return num;
			}
			return (int)(((arc1._start != null) ? arc1._start.Id : 0U) - ((arc2._start != null) ? arc2._start.Id : 0U));
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000A488 File Offset: 0x00009488
		internal static int CompareForDuplicateOutputTransitions(Arc arc1, Arc arc2)
		{
			int num = Arc.CompareContent(arc1, arc2);
			if (num != 0)
			{
				return num;
			}
			return (int)(((arc1._end != null) ? arc1._end.Id : 0U) - ((arc2._end != null) ? arc2._end.Id : 0U));
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000A4D0 File Offset: 0x000094D0
		internal static int CompareIdenticalTransitions(Arc arc1, Arc arc2)
		{
			int num = (int)(((arc1._start != null) ? arc1._start.Id : 0U) - ((arc2._start != null) ? arc2._start.Id : 0U));
			if (num == 0 && (num = (int)(((arc1._end != null) ? arc1._end.Id : 0U) - ((arc2._end != null) ? arc2._end.Id : 0U))) == 0)
			{
				num = (arc1.SameTags(arc2) ? 0 : 1);
			}
			return num;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000A54E File Offset: 0x0000954E
		internal void AddStartTag(Tag tag)
		{
			if (this._startTags == null)
			{
				this._startTags = new Collection<Tag>();
			}
			this._startTags.Add(tag);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000A56F File Offset: 0x0000956F
		internal void AddEndTag(Tag tag)
		{
			if (this._endTags == null)
			{
				this._endTags = new Collection<Tag>();
			}
			this._endTags.Add(tag);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000A590 File Offset: 0x00009590
		internal void ClearTags()
		{
			this._startTags = null;
			this._endTags = null;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000A5A0 File Offset: 0x000095A0
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

		// Token: 0x06000300 RID: 768 RVA: 0x0000A6F8 File Offset: 0x000096F8
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
						if (tag._cfgTag._valueOffset != 0 && tag._cfgTag.PropVariantType == null)
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

		// Token: 0x06000301 RID: 769 RVA: 0x0000A888 File Offset: 0x00009888
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

		// Token: 0x06000302 RID: 770 RVA: 0x0000A983 File Offset: 0x00009983
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000A9B7 File Offset: 0x000099B7
		internal bool IsEpsilonTransition
		{
			get
			{
				return this._ruleRef == null && this._specialTransitionIndex == 0 && this._iWord == 0;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000A9D4 File Offset: 0x000099D4
		internal bool IsPropertylessTransition
		{
			get
			{
				return this._startTags == null && this._endTags == null;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000A9E9 File Offset: 0x000099E9
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000AA4F File Offset: 0x00009A4F
		// (set) Token: 0x06000306 RID: 774 RVA: 0x0000AA00 File Offset: 0x00009A00
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000AAA7 File Offset: 0x00009AA7
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0000AA58 File Offset: 0x00009A58
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0000AAAF File Offset: 0x00009AAF
		internal int WordId
		{
			get
			{
				return this._iWord;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0000AB06 File Offset: 0x00009B06
		// (set) Token: 0x0600030B RID: 779 RVA: 0x0000AAB8 File Offset: 0x00009AB8
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000AB17 File Offset: 0x00009B17
		// (set) Token: 0x0600030D RID: 781 RVA: 0x0000AB0E File Offset: 0x00009B0E
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000AB1F File Offset: 0x00009B1F
		internal int SpecialTransitionIndex
		{
			get
			{
				return this._specialTransitionIndex;
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000AB28 File Offset: 0x00009B28
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

		// Token: 0x040002CB RID: 715
		private State _start;

		// Token: 0x040002CC RID: 716
		private State _end;

		// Token: 0x040002CD RID: 717
		private int _iWord;

		// Token: 0x040002CE RID: 718
		private Rule _ruleRef;

		// Token: 0x040002CF RID: 719
		private int _specialTransitionIndex;

		// Token: 0x040002D0 RID: 720
		private float _flWeight;

		// Token: 0x040002D1 RID: 721
		private MatchMode _matchMode;

		// Token: 0x040002D2 RID: 722
		private int _confidence;

		// Token: 0x040002D3 RID: 723
		private uint _iSerialize;

		// Token: 0x040002D4 RID: 724
		private Collection<Tag> _startTags;

		// Token: 0x040002D5 RID: 725
		private Collection<Tag> _endTags;

		// Token: 0x040002D6 RID: 726
		private static uint _serializeToken = 1U;
	}
}
