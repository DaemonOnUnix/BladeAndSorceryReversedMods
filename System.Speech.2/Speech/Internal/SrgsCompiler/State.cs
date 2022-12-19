using System;
using System.Collections.Generic;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000FC RID: 252
	internal sealed class State : IComparable<State>
	{
		// Token: 0x060008DD RID: 2269 RVA: 0x000283D5 File Offset: 0x000265D5
		internal State(Rule rule, uint hState, int iSerialize)
		{
			this._rule = rule;
			this._iSerialize = iSerialize;
			this._id = hState;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00028408 File Offset: 0x00026608
		internal State(Rule rule, uint hState)
			: this(rule, hState, (int)hState)
		{
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00028413 File Offset: 0x00026613
		int IComparable<State>.CompareTo(State state2)
		{
			return State.Compare(this, state2);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002841C File Offset: 0x0002661C
		internal void SerializeStateEntries(StreamMarshaler streamBuffer, bool tagsCannotSpanOverMultipleArcs, float[] pWeights, ref uint iArcOffset, ref int iOffset)
		{
			List<Arc> list = this._outArcs.ToList();
			list.Sort();
			Arc arc = ((list.Count > 0) ? list[list.Count - 1] : null);
			IEnumerator<Arc> enumerator = ((IEnumerable<Arc>)list).GetEnumerator();
			enumerator.MoveNext();
			uint num = (uint)(list.Count + (int)iArcOffset);
			uint num2 = num;
			foreach (Arc arc2 in list)
			{
				int semanticTagCount = arc2.SemanticTagCount;
				if (semanticTagCount > 0)
				{
					arc2.SetArcIndexForTag(0, iArcOffset, tagsCannotSpanOverMultipleArcs);
				}
				if (semanticTagCount <= 1)
				{
					int num3 = iOffset;
					iOffset = num3 + 1;
					int num4 = num3;
					Arc arc3 = arc2;
					bool flag = arc == arc2;
					uint num5 = iArcOffset;
					iArcOffset = num5 + 1U;
					pWeights[num4] = arc3.Serialize(streamBuffer, flag, num5);
				}
				else
				{
					iArcOffset += 1U;
					int num3 = iOffset;
					iOffset = num3 + 1;
					pWeights[num3] = Arc.SerializeExtraEpsilonWithTag(streamBuffer, arc2, arc == arc2, num);
					num += (uint)(semanticTagCount - 1);
				}
			}
			enumerator = ((IEnumerable<Arc>)list).GetEnumerator();
			enumerator.MoveNext();
			num = num2;
			foreach (Arc arc4 in list)
			{
				int semanticTagCount2 = arc4.SemanticTagCount;
				if (semanticTagCount2 > 1)
				{
					int num3;
					for (int i = 1; i < semanticTagCount2 - 1; i++)
					{
						arc4.SetArcIndexForTag(i, iArcOffset, tagsCannotSpanOverMultipleArcs);
						num += 1U;
						num3 = iOffset;
						iOffset = num3 + 1;
						pWeights[num3] = Arc.SerializeExtraEpsilonWithTag(streamBuffer, arc4, true, num);
						iArcOffset += 1U;
					}
					arc4.SetArcIndexForTag(semanticTagCount2 - 1, iArcOffset, tagsCannotSpanOverMultipleArcs);
					num3 = iOffset;
					iOffset = num3 + 1;
					int num6 = num3;
					Arc arc5 = arc4;
					bool flag2 = true;
					uint num5 = iArcOffset;
					iArcOffset = num5 + 1U;
					pWeights[num6] = arc5.Serialize(streamBuffer, flag2, num5);
					num += 1U;
				}
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00028610 File Offset: 0x00026810
		internal void SetEndArcIndexForTags()
		{
			foreach (object obj in this._outArcs)
			{
				Arc arc = (Arc)obj;
				arc.SetEndArcIndexForTags();
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0000BB6D File Offset: 0x00009D6D
		internal void Init()
		{
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00028668 File Offset: 0x00026868
		internal State Add(State state)
		{
			this._next = state;
			state._prev = this;
			return state;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0002867C File Offset: 0x0002687C
		internal void Remove()
		{
			if (this._prev != null)
			{
				this._prev._next = this._next;
			}
			if (this._next != null)
			{
				this._next._prev = this._prev;
			}
			this._next = (this._prev = null);
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x000286CB File Offset: 0x000268CB
		internal State Next
		{
			get
			{
				return this._next;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x000286D3 File Offset: 0x000268D3
		internal State Prev
		{
			get
			{
				return this._prev;
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x000286DC File Offset: 0x000268DC
		internal void CheckLeftRecursion(out bool fReachedEndState)
		{
			fReachedEndState = false;
			if ((this._recurseFlag & State.RecurFlag.RF_IN_LEFT_RECUR_CHECK) != (State.RecurFlag)0U)
			{
				XmlParser.ThrowSrgsException(SRID.CircularRuleRef, new object[] { (this._rule != null) ? this._rule._rule.Name : string.Empty });
				return;
			}
			if ((this._recurseFlag & State.RecurFlag.RF_CHECKED_LEFT_RECURSION) == (State.RecurFlag)0U)
			{
				this._recurseFlag |= (State.RecurFlag)12U;
				foreach (object obj in this._outArcs)
				{
					Arc arc = (Arc)obj;
					bool flag = false;
					if (arc.RuleRef != null && arc.RuleRef._firstState != null)
					{
						State firstState = arc.RuleRef._firstState;
						if ((firstState._recurseFlag & State.RecurFlag.RF_IN_LEFT_RECUR_CHECK) != (State.RecurFlag)0U || (firstState._recurseFlag & State.RecurFlag.RF_CHECKED_LEFT_RECURSION) == (State.RecurFlag)0U)
						{
							firstState.CheckLeftRecursion(out flag);
						}
						else
						{
							flag = arc.RuleRef._fIsEpsilonRule;
						}
					}
					if (flag || (arc.RuleRef == null && arc.WordId == 0 && arc.WordId == 0))
					{
						if (arc.End != null)
						{
							arc.End.CheckLeftRecursion(out fReachedEndState);
						}
						else
						{
							fReachedEndState = true;
						}
					}
				}
				this._recurseFlag &= (State.RecurFlag)4294967287U;
				if ((this._rule._firstState == this) & fReachedEndState)
				{
					this._rule._fIsEpsilonRule = true;
				}
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00028840 File Offset: 0x00026A40
		internal int NumArcs
		{
			get
			{
				int num = 0;
				foreach (object obj in this._outArcs)
				{
					Arc arc = (Arc)obj;
					if (arc.SemanticTagCount > 0)
					{
						num += arc.SemanticTagCount - 1;
					}
				}
				int num2 = 0;
				foreach (object obj2 in this._outArcs)
				{
					Arc arc2 = (Arc)obj2;
					num2++;
				}
				return num2 + num;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x00028900 File Offset: 0x00026B00
		internal int NumSemanticTags
		{
			get
			{
				int num = 0;
				foreach (object obj in this._outArcs)
				{
					Arc arc = (Arc)obj;
					num += arc.SemanticTagCount;
				}
				return num;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x00028960 File Offset: 0x00026B60
		internal Rule Rule
		{
			get
			{
				return this._rule;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x00028968 File Offset: 0x00026B68
		internal uint Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x00028970 File Offset: 0x00026B70
		internal ArcList OutArcs
		{
			get
			{
				return this._outArcs;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x00028978 File Offset: 0x00026B78
		internal ArcList InArcs
		{
			get
			{
				return this._inArcs;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x00028989 File Offset: 0x00026B89
		// (set) Token: 0x060008EE RID: 2286 RVA: 0x00028980 File Offset: 0x00026B80
		internal int SerializeId
		{
			get
			{
				return this._iSerialize;
			}
			set
			{
				this._iSerialize = value;
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00028994 File Offset: 0x00026B94
		private static int Compare(State state1, State state2)
		{
			if (state1._rule._cfgRule._nameOffset != state2._rule._cfgRule._nameOffset)
			{
				return state1._rule._cfgRule._nameOffset - state2._rule._cfgRule._nameOffset;
			}
			int num = ((state1._rule._firstState == state1) ? (-1) : 0);
			int num2 = ((state2._rule._firstState == state2) ? (-1) : 0);
			if (num != num2)
			{
				return num - num2;
			}
			Arc arc = ((state1._outArcs != null && !state1._outArcs.IsEmpty) ? state1._outArcs.First : null);
			Arc arc2 = ((state2._outArcs != null && !state2._outArcs.IsEmpty) ? state2._outArcs.First : null);
			int num3 = ((arc != null) ? (((arc.RuleRef != null) ? 16777216 : 0) + arc.WordId) : state1._iSerialize) - ((arc2 != null) ? (((arc2.RuleRef != null) ? 16777216 : 0) + arc2.WordId) : state2._iSerialize);
			return (num3 != 0) ? num3 : (state1._iSerialize - state2._iSerialize);
		}

		// Token: 0x04000631 RID: 1585
		private ArcList _outArcs = new ArcList();

		// Token: 0x04000632 RID: 1586
		private ArcList _inArcs = new ArcList();

		// Token: 0x04000633 RID: 1587
		private int _iSerialize;

		// Token: 0x04000634 RID: 1588
		private uint _id;

		// Token: 0x04000635 RID: 1589
		private Rule _rule;

		// Token: 0x04000636 RID: 1590
		private State _next;

		// Token: 0x04000637 RID: 1591
		private State _prev;

		// Token: 0x04000638 RID: 1592
		private State.RecurFlag _recurseFlag;

		// Token: 0x020001A9 RID: 425
		internal enum RecurFlag : uint
		{
			// Token: 0x040009A4 RID: 2468
			RF_CHECKED_EPSILON = 1U,
			// Token: 0x040009A5 RID: 2469
			RF_CHECKED_EXIT_PATH,
			// Token: 0x040009A6 RID: 2470
			RF_CHECKED_LEFT_RECURSION = 4U,
			// Token: 0x040009A7 RID: 2471
			RF_IN_LEFT_RECUR_CHECK = 8U
		}
	}
}
