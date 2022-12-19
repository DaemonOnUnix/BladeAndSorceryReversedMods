using System;
using System.Collections.Generic;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000BC RID: 188
	internal sealed class State : IComparable<State>
	{
		// Token: 0x06000439 RID: 1081 RVA: 0x000106F9 File Offset: 0x0000F6F9
		internal State(Rule rule, uint hState, int iSerialize)
		{
			this._rule = rule;
			this._iSerialize = iSerialize;
			this._id = hState;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0001072C File Offset: 0x0000F72C
		internal State(Rule rule, uint hState)
			: this(rule, hState, (int)hState)
		{
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00010737 File Offset: 0x0000F737
		int IComparable<State>.CompareTo(State state2)
		{
			return State.Compare(this, state2);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00010740 File Offset: 0x0000F740
		internal void SerializeStateEntries(StreamMarshaler streamBuffer, bool tagsCannotSpanOverMultipleArcs, float[] pWeights, ref uint iArcOffset, ref int iOffset)
		{
			List<Arc> list = this._outArcs.ToList();
			list.Sort();
			Arc arc = ((list.Count > 0) ? list[list.Count - 1] : null);
			IEnumerator<Arc> enumerator = list.GetEnumerator();
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
					int num3 = iOffset++;
					Arc arc3 = arc2;
					bool flag = arc == arc2;
					uint num4;
					iArcOffset = (num4 = iArcOffset) + 1U;
					pWeights[num3] = arc3.Serialize(streamBuffer, flag, num4);
				}
				else
				{
					iArcOffset += 1U;
					pWeights[iOffset++] = Arc.SerializeExtraEpsilonWithTag(streamBuffer, arc2, arc == arc2, num);
					num += (uint)(semanticTagCount - 1);
				}
			}
			enumerator = list.GetEnumerator();
			enumerator.MoveNext();
			num = num2;
			foreach (Arc arc4 in list)
			{
				int semanticTagCount2 = arc4.SemanticTagCount;
				if (semanticTagCount2 > 1)
				{
					for (int i = 1; i < semanticTagCount2 - 1; i++)
					{
						arc4.SetArcIndexForTag(i, iArcOffset, tagsCannotSpanOverMultipleArcs);
						num += 1U;
						pWeights[iOffset++] = Arc.SerializeExtraEpsilonWithTag(streamBuffer, arc4, true, num);
						iArcOffset += 1U;
					}
					arc4.SetArcIndexForTag(semanticTagCount2 - 1, iArcOffset, tagsCannotSpanOverMultipleArcs);
					int num5 = iOffset++;
					Arc arc5 = arc4;
					bool flag2 = true;
					uint num6;
					iArcOffset = (num6 = iArcOffset) + 1U;
					pWeights[num5] = arc5.Serialize(streamBuffer, flag2, num6);
					num += 1U;
				}
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00010920 File Offset: 0x0000F920
		internal void SetEndArcIndexForTags()
		{
			foreach (object obj in this._outArcs)
			{
				Arc arc = (Arc)obj;
				arc.SetEndArcIndexForTags();
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00010978 File Offset: 0x0000F978
		internal void Init()
		{
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001097A File Offset: 0x0000F97A
		internal State Add(State state)
		{
			this._next = state;
			state._prev = this;
			return state;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001098C File Offset: 0x0000F98C
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

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x000109DB File Offset: 0x0000F9DB
		internal State Next
		{
			get
			{
				return this._next;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x000109E3 File Offset: 0x0000F9E3
		internal State Prev
		{
			get
			{
				return this._prev;
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x000109EC File Offset: 0x0000F9EC
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
				if (this._rule._firstState == this && fReachedEndState)
				{
					this._rule._fIsEpsilonRule = true;
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00010B54 File Offset: 0x0000FB54
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00010C10 File Offset: 0x0000FC10
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00010C70 File Offset: 0x0000FC70
		internal Rule Rule
		{
			get
			{
				return this._rule;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00010C78 File Offset: 0x0000FC78
		internal uint Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00010C80 File Offset: 0x0000FC80
		internal ArcList OutArcs
		{
			get
			{
				return this._outArcs;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00010C88 File Offset: 0x0000FC88
		internal ArcList InArcs
		{
			get
			{
				return this._inArcs;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x00010C99 File Offset: 0x0000FC99
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x00010C90 File Offset: 0x0000FC90
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

		// Token: 0x0600044C RID: 1100 RVA: 0x00010CA4 File Offset: 0x0000FCA4
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

		// Token: 0x04000387 RID: 903
		private ArcList _outArcs = new ArcList();

		// Token: 0x04000388 RID: 904
		private ArcList _inArcs = new ArcList();

		// Token: 0x04000389 RID: 905
		private int _iSerialize;

		// Token: 0x0400038A RID: 906
		private uint _id;

		// Token: 0x0400038B RID: 907
		private Rule _rule;

		// Token: 0x0400038C RID: 908
		private State _next;

		// Token: 0x0400038D RID: 909
		private State _prev;

		// Token: 0x0400038E RID: 910
		private State.RecurFlag _recurseFlag;

		// Token: 0x020000BD RID: 189
		internal enum RecurFlag : uint
		{
			// Token: 0x04000390 RID: 912
			RF_CHECKED_EPSILON = 1U,
			// Token: 0x04000391 RID: 913
			RF_CHECKED_EXIT_PATH,
			// Token: 0x04000392 RID: 914
			RF_CHECKED_LEFT_RECURSION = 4U,
			// Token: 0x04000393 RID: 915
			RF_IN_LEFT_RECUR_CHECK = 8U
		}
	}
}
