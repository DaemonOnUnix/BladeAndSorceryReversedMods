using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000A4 RID: 164
	internal class Graph : IEnumerable<State>, IEnumerable
	{
		// Token: 0x06000388 RID: 904 RVA: 0x0000D9BC File Offset: 0x0000C9BC
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

		// Token: 0x06000389 RID: 905 RVA: 0x0000D9FA File Offset: 0x0000C9FA
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

		// Token: 0x0600038A RID: 906 RVA: 0x0000DAC8 File Offset: 0x0000CAC8
		IEnumerator IEnumerable.GetEnumerator()
		{
			for (State item = this._startState; item != null; item = item.Next)
			{
				yield return item;
			}
			yield break;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000DB80 File Offset: 0x0000CB80
		IEnumerator<State> IEnumerable<State>.GetEnumerator()
		{
			for (State item = this._startState; item != null; item = item.Next)
			{
				yield return item;
			}
			yield break;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000DB9C File Offset: 0x0000CB9C
		internal State CreateNewState(Rule rule)
		{
			uint nextHandle = CfgGrammar.NextHandle;
			State state = new State(rule, nextHandle);
			this.Add(state);
			return state;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000DBBF File Offset: 0x0000CBBF
		internal void DeleteState(State state)
		{
			this.Remove(state);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000DBC8 File Offset: 0x0000CBC8
		internal void Optimize()
		{
			foreach (State state in this)
			{
				Graph.NormalizeTransitionWeights(state);
			}
			this.MergeDuplicateTransitions();
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000DC18 File Offset: 0x0000CC18
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

		// Token: 0x06000390 RID: 912 RVA: 0x0000DC94 File Offset: 0x0000CC94
		internal void MoveOutputTransitionsAndDeleteState(State srcState, State destState)
		{
			List<Arc> list = srcState.OutArcs.ToList();
			foreach (Arc arc in list)
			{
				arc.Start = destState;
			}
			this.DeleteState(srcState);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000DCF8 File Offset: 0x0000CCF8
		private void MergeDuplicateTransitions()
		{
			List<Arc> list = new List<Arc>();
			foreach (State state in this)
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

		// Token: 0x06000392 RID: 914 RVA: 0x0000DD6C File Offset: 0x0000CD6C
		private void RecursiveMergeDuplicatedInputTransition(Stack<State> mergeStates)
		{
			foreach (State state in this)
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

		// Token: 0x06000393 RID: 915 RVA: 0x0000DE08 File Offset: 0x0000CE08
		private void RecursiveMergeDuplicatedOutputTransition(Stack<State> mergeStates)
		{
			foreach (State state in this)
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

		// Token: 0x06000394 RID: 916 RVA: 0x0000DEA4 File Offset: 0x0000CEA4
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

		// Token: 0x06000395 RID: 917 RVA: 0x0000E138 File Offset: 0x0000D138
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

		// Token: 0x06000396 RID: 918 RVA: 0x0000E3E0 File Offset: 0x0000D3E0
		private static void AddToMergeStateList(Stack<State> mergeStates, State commonEndState)
		{
			Graph.NormalizeTransitionWeights(commonEndState);
			if (!mergeStates.Contains(commonEndState))
			{
				mergeStates.Push(commonEndState);
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000E3F8 File Offset: 0x0000D3F8
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

		// Token: 0x06000398 RID: 920 RVA: 0x0000E44C File Offset: 0x0000D44C
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

		// Token: 0x06000399 RID: 921 RVA: 0x0000E49F File Offset: 0x0000D49F
		internal static bool CanTagsBeMoved(Arc start, Arc end)
		{
			return start.RuleRef == null && end.RuleRef == null && end.SpecialTransitionIndex == 0;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000E4BC File Offset: 0x0000D4BC
		private static void DeleteTransition(Arc arc)
		{
			arc.Start = (arc.End = null);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000E4DC File Offset: 0x0000D4DC
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

		// Token: 0x0600039C RID: 924 RVA: 0x0000E5D8 File Offset: 0x0000D5D8
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

		// Token: 0x0600039D RID: 925 RVA: 0x0000E69C File Offset: 0x0000D69C
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

		// Token: 0x04000356 RID: 854
		private State _startState;

		// Token: 0x04000357 RID: 855
		private State _curState;
	}
}
