using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000EF RID: 239
	internal class Graph : IEnumerable<State>, IEnumerable
	{
		// Token: 0x0600085D RID: 2141 RVA: 0x00025814 File Offset: 0x00023A14
		internal void Add(State state)
		{
			state.Init();
			if (this._startState == null)
			{
				this._startState = state;
				this._curState = state;
				return;
			}
			this._curState = this._curState.Add(state);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00025852 File Offset: 0x00023A52
		internal void Remove(State state)
		{
			if (state == this._startState)
			{
				this._startState = state.Next;
			}
			if (state == this._curState)
			{
				this._curState = state.Prev;
			}
			state.Remove();
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00025884 File Offset: 0x00023A84
		IEnumerator IEnumerable.GetEnumerator()
		{
			State item;
			for (item = this._startState; item != null; item = item.Next)
			{
				yield return item;
			}
			item = null;
			yield break;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00025893 File Offset: 0x00023A93
		IEnumerator<State> IEnumerable<State>.GetEnumerator()
		{
			State item;
			for (item = this._startState; item != null; item = item.Next)
			{
				yield return item;
			}
			item = null;
			yield break;
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x000258A4 File Offset: 0x00023AA4
		internal State CreateNewState(Rule rule)
		{
			uint nextHandle = CfgGrammar.NextHandle;
			State state = new State(rule, nextHandle);
			this.Add(state);
			return state;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x000258C7 File Offset: 0x00023AC7
		internal void DeleteState(State state)
		{
			this.Remove(state);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x000258D0 File Offset: 0x00023AD0
		internal void Optimize()
		{
			foreach (State state in ((IEnumerable<State>)this))
			{
				Graph.NormalizeTransitionWeights(state);
			}
			this.MergeDuplicateTransitions();
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00025920 File Offset: 0x00023B20
		internal void MoveInputTransitionsAndDeleteState(State srcState, State destState)
		{
			List<Arc> list = srcState.InArcs.ToList();
			foreach (Arc arc in list)
			{
				arc.End = destState;
			}
			if (srcState.Rule._firstState == srcState)
			{
				srcState.Rule._firstState = destState;
			}
			this.DeleteState(srcState);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0002599C File Offset: 0x00023B9C
		internal void MoveOutputTransitionsAndDeleteState(State srcState, State destState)
		{
			List<Arc> list = srcState.OutArcs.ToList();
			foreach (Arc arc in list)
			{
				arc.Start = destState;
			}
			this.DeleteState(srcState);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00025A00 File Offset: 0x00023C00
		private void MergeDuplicateTransitions()
		{
			List<Arc> list = new List<Arc>();
			foreach (State state in ((IEnumerable<State>)this))
			{
				if (state.OutArcs.ContainsMoreThanOneItem)
				{
					Graph.MergeIdenticalTransitions(state.OutArcs, list);
				}
			}
			Stack<State> stack = new Stack<State>();
			this.RecursiveMergeDuplicatedOutputTransition(stack);
			this.RecursiveMergeDuplicatedInputTransition(stack);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00025A74 File Offset: 0x00023C74
		private void RecursiveMergeDuplicatedInputTransition(Stack<State> mergeStates)
		{
			foreach (State state in ((IEnumerable<State>)this))
			{
				if (state.InArcs.ContainsMoreThanOneItem)
				{
					this.MergeDuplicateInputTransitions(state.InArcs, mergeStates);
				}
			}
			List<Arc> list = new List<Arc>();
			while (mergeStates.Count > 0)
			{
				State state2 = mergeStates.Pop();
				if (state2.InArcs.ContainsMoreThanOneItem)
				{
					Graph.MergeIdenticalTransitions(state2.InArcs, list);
					this.MergeDuplicateInputTransitions(state2.InArcs, mergeStates);
				}
			}
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00025B10 File Offset: 0x00023D10
		private void RecursiveMergeDuplicatedOutputTransition(Stack<State> mergeStates)
		{
			foreach (State state in ((IEnumerable<State>)this))
			{
				if (state.OutArcs.ContainsMoreThanOneItem)
				{
					this.MergeDuplicateOutputTransitions(state.OutArcs, mergeStates);
				}
			}
			List<Arc> list = new List<Arc>();
			while (mergeStates.Count > 0)
			{
				State state2 = mergeStates.Pop();
				if (state2.OutArcs.ContainsMoreThanOneItem)
				{
					Graph.MergeIdenticalTransitions(state2.OutArcs, list);
					this.MergeDuplicateOutputTransitions(state2.OutArcs, mergeStates);
				}
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00025BAC File Offset: 0x00023DAC
		private void MergeDuplicateInputTransitions(ArcList arcs, Stack<State> mergeStates)
		{
			List<Arc> list = null;
			Arc arc = null;
			bool flag = false;
			foreach (object obj in arcs)
			{
				Arc arc2 = (Arc)obj;
				bool flag2 = arc2.Start == null || !arc2.Start.OutArcs.CountIsOne;
				if (arc != null && Arc.CompareContent(arc2, arc) == 0)
				{
					if (!flag2)
					{
						if (list == null)
						{
							list = new List<Arc>();
						}
						if (!flag)
						{
							list.Add(arc);
							flag = true;
						}
						list.Add(arc2);
					}
				}
				else
				{
					arc = (flag2 ? null : arc2);
					flag = false;
				}
			}
			if (list != null)
			{
				list.Sort(new Comparison<Arc>(Arc.CompareForDuplicateInputTransitions));
				arc = null;
				Arc arc3 = null;
				State state = null;
				bool flag3 = false;
				foreach (Arc arc4 in list)
				{
					if (arc == null || Arc.CompareContent(arc4, arc) != 0)
					{
						arc = arc4;
						if (flag3)
						{
							Graph.AddToMergeStateList(mergeStates, state);
						}
						arc3 = null;
						state = null;
						flag3 = false;
					}
					Arc arc5 = arc4;
					State start = arc5.Start;
					if (Graph.MoveSemanticTagLeft(arc5))
					{
						if (arc3 != null)
						{
							if (!flag3)
							{
								foreach (object obj2 in state.OutArcs)
								{
									Arc arc6 = (Arc)obj2;
									arc6.Weight *= arc3.Weight;
								}
								flag3 = true;
							}
							foreach (object obj3 in start.OutArcs)
							{
								Arc arc7 = (Arc)obj3;
								arc7.Weight *= arc5.Weight;
							}
							arc5.Weight += arc3.Weight;
							Arc.CopyTags(arc3, arc5, Direction.Left);
							Graph.DeleteTransition(arc3);
							this.MoveInputTransitionsAndDeleteState(state, start);
						}
						arc3 = arc5;
						state = start;
					}
				}
				if (flag3)
				{
					Graph.AddToMergeStateList(mergeStates, state);
				}
			}
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00025E40 File Offset: 0x00024040
		private void MergeDuplicateOutputTransitions(ArcList arcs, Stack<State> mergeStates)
		{
			List<Arc> list = null;
			Arc arc = null;
			bool flag = false;
			foreach (object obj in arcs)
			{
				Arc arc2 = (Arc)obj;
				bool flag2 = arc2.End == null || !arc2.End.InArcs.CountIsOne;
				if (arc != null && Arc.CompareContent(arc2, arc) == 0)
				{
					if (!flag2)
					{
						if (list == null)
						{
							list = new List<Arc>();
						}
						if (!flag)
						{
							list.Add(arc);
							flag = true;
						}
						list.Add(arc2);
					}
				}
				else
				{
					arc = (flag2 ? null : arc2);
					flag = false;
				}
			}
			if (list != null)
			{
				list.Sort(new Comparison<Arc>(Arc.CompareForDuplicateOutputTransitions));
				arc = null;
				Arc arc3 = null;
				State state = null;
				bool flag3 = false;
				foreach (Arc arc4 in list)
				{
					if (arc == null || Arc.CompareContent(arc4, arc) != 0)
					{
						arc = arc4;
						if (flag3)
						{
							Graph.AddToMergeStateList(mergeStates, state);
						}
						arc3 = null;
						state = null;
						flag3 = false;
					}
					Arc arc5 = arc4;
					State end = arc5.End;
					if (end != end.Rule._firstState && Graph.MoveSemanticTagRight(arc5))
					{
						if (arc3 != null)
						{
							if (!flag3)
							{
								foreach (object obj2 in state.OutArcs)
								{
									Arc arc6 = (Arc)obj2;
									arc6.Weight *= arc3.Weight;
								}
								flag3 = true;
							}
							foreach (object obj3 in end.OutArcs)
							{
								Arc arc7 = (Arc)obj3;
								arc7.Weight *= arc5.Weight;
							}
							arc5.Weight += arc3.Weight;
							Arc.CopyTags(arc3, arc5, Direction.Right);
							Graph.DeleteTransition(arc3);
							this.MoveOutputTransitionsAndDeleteState(state, end);
						}
						arc3 = arc5;
						state = end;
					}
				}
				if (flag3)
				{
					Graph.AddToMergeStateList(mergeStates, state);
				}
			}
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000260E8 File Offset: 0x000242E8
		private static void AddToMergeStateList(Stack<State> mergeStates, State commonEndState)
		{
			Graph.NormalizeTransitionWeights(commonEndState);
			if (!mergeStates.Contains(commonEndState))
			{
				mergeStates.Push(commonEndState);
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00026100 File Offset: 0x00024300
		internal static bool MoveSemanticTagLeft(Arc arc)
		{
			State start = arc.Start;
			Arc first = start.InArcs.First;
			if (start.InArcs.CountIsOne && start.OutArcs.CountIsOne && Graph.CanTagsBeMoved(first, arc))
			{
				Arc.CopyTags(arc, first, Direction.Left);
				return true;
			}
			return arc.IsPropertylessTransition;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00026154 File Offset: 0x00024354
		internal static bool MoveSemanticTagRight(Arc arc)
		{
			State end = arc.End;
			Arc first = end.OutArcs.First;
			if (end.InArcs.CountIsOne && end.OutArcs.CountIsOne && Graph.CanTagsBeMoved(arc, first))
			{
				Arc.CopyTags(arc, first, Direction.Right);
				return true;
			}
			return arc.IsPropertylessTransition;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x000261A7 File Offset: 0x000243A7
		internal static bool CanTagsBeMoved(Arc start, Arc end)
		{
			return start.RuleRef == null && end.RuleRef == null && end.SpecialTransitionIndex == 0;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x000261C4 File Offset: 0x000243C4
		private static void DeleteTransition(Arc arc)
		{
			arc.Start = (arc.End = null);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000261E4 File Offset: 0x000243E4
		private static void MergeIdenticalTransitions(ArcList arcs, List<Arc> identicalWords)
		{
			List<List<Arc>> list = null;
			Arc arc = arcs.First;
			foreach (object obj in arcs)
			{
				Arc arc2 = (Arc)obj;
				if (Arc.CompareContent(arc, arc2) != 0)
				{
					if (identicalWords.Count >= 2)
					{
						identicalWords.Sort(new Comparison<Arc>(Arc.CompareIdenticalTransitions));
						if (list == null)
						{
							list = new List<List<Arc>>();
						}
						list.Add(new List<Arc>(identicalWords));
					}
					identicalWords.Clear();
				}
				arc = arc2;
				identicalWords.Add(arc2);
			}
			if (identicalWords.Count >= 2)
			{
				Graph.MergeIdenticalTransitions(identicalWords);
			}
			identicalWords.Clear();
			if (list != null)
			{
				foreach (List<Arc> list2 in list)
				{
					Graph.MergeIdenticalTransitions(list2);
				}
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x000262DC File Offset: 0x000244DC
		private static void MergeIdenticalTransitions(List<Arc> identicalWords)
		{
			Collection<Arc> collection = null;
			Arc arc = null;
			foreach (Arc arc2 in identicalWords)
			{
				if (arc != null && Arc.CompareIdenticalTransitions(arc, arc2) == 0)
				{
					arc2.Weight += arc.Weight;
					arc.ClearTags();
					if (collection == null)
					{
						collection = new Collection<Arc>();
					}
					collection.Add(arc);
				}
				arc = arc2;
			}
			if (collection != null)
			{
				foreach (Arc arc3 in collection)
				{
					Graph.DeleteTransition(arc3);
				}
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x000263A0 File Offset: 0x000245A0
		private static void NormalizeTransitionWeights(State state)
		{
			float num = 0f;
			foreach (object obj in state.OutArcs)
			{
				Arc arc = (Arc)obj;
				num += arc.Weight;
			}
			if (!num.Equals(0f) && !num.Equals(1f))
			{
				float num2 = 1f / num;
				foreach (object obj2 in state.OutArcs)
				{
					Arc arc2 = (Arc)obj2;
					arc2.Weight *= num2;
				}
			}
		}

		// Token: 0x04000607 RID: 1543
		private State _startState;

		// Token: 0x04000608 RID: 1544
		private State _curState;
	}
}
