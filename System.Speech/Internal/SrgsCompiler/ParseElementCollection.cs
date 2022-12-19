using System;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000A7 RID: 167
	internal abstract class ParseElementCollection : ParseElement
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x0000E799 File Offset: 0x0000D799
		protected ParseElementCollection(Backend backend, Rule rule)
			: base(rule)
		{
			this._backend = backend;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000E7AC File Offset: 0x0000D7AC
		internal void AddSemanticInterpretationTag(CfgGrammar.CfgProperty propertyInfo)
		{
			if (this._endArc != null && this._endArc.RuleRef != null)
			{
				Arc arc = this._backend.EpsilonTransition(1f);
				this._backend.AddSemanticInterpretationTag(arc, propertyInfo);
				State state = this._backend.CreateNewState(this._rule);
				arc.Start = state;
				this._endArc.End = state;
				this._endArc = arc;
				return;
			}
			if (this._startArc == null)
			{
				this._startArc = (this._endArc = this._backend.EpsilonTransition(1f));
			}
			this._backend.AddSemanticInterpretationTag(this._endArc, propertyInfo);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000E854 File Offset: 0x0000D854
		internal void AddSementicPropertyTag(CfgGrammar.CfgProperty propertyInfo)
		{
			if (this._startArc == null)
			{
				this._startArc = (this._endArc = this._backend.EpsilonTransition(1f));
			}
			this._backend.AddPropertyTag(this._startArc, this._endArc, propertyInfo);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000E8A0 File Offset: 0x0000D8A0
		protected Arc InsertState(Arc arc, float weight, ParseElementCollection.Position position)
		{
			if (arc.IsEpsilonTransition)
			{
				if (position == ParseElementCollection.Position.Before && arc.End != null && arc.End.InArcs.CountIsOne && Graph.MoveSemanticTagRight(arc))
				{
					return arc;
				}
				if (position == ParseElementCollection.Position.After && arc.Start != null && arc.Start.OutArcs.CountIsOne && Graph.MoveSemanticTagLeft(arc))
				{
					return arc;
				}
			}
			Arc arc2 = this._backend.EpsilonTransition(weight);
			State state = this._backend.CreateNewState(this._rule);
			if (position == ParseElementCollection.Position.Before)
			{
				arc2.End = state;
				arc.Start = state;
			}
			else
			{
				arc.End = state;
				arc2.Start = state;
			}
			return arc2;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000E948 File Offset: 0x0000D948
		protected static Arc TrimStart(Arc start, Backend backend)
		{
			Arc arc = start;
			if (start.End != null)
			{
				State state = arc.End;
				while (arc.IsEpsilonTransition && state != null && Graph.MoveSemanticTagRight(arc) && state.InArcs.CountIsOne && state.OutArcs.CountIsOne)
				{
					arc.End = null;
					arc = state.OutArcs.First;
					arc.Start = null;
					backend.DeleteState(state);
					state = arc.End;
				}
			}
			return arc;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000E9C0 File Offset: 0x0000D9C0
		protected static Arc TrimEnd(Arc end, Backend backend)
		{
			Arc arc = end;
			if (arc != null)
			{
				State state = arc.Start;
				while (arc.IsEpsilonTransition && state != null && Graph.MoveSemanticTagLeft(arc) && state.InArcs.CountIsOne && state.OutArcs.CountIsOne)
				{
					arc.Start = null;
					arc = state.InArcs.First;
					arc.End = null;
					backend.DeleteState(state);
					state = arc.Start;
				}
			}
			return arc;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000EA31 File Offset: 0x0000DA31
		protected void PostParse(ParseElementCollection parent)
		{
			if (this._startArc != null)
			{
				parent.AddArc(this._startArc, this._endArc);
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000EA4D File Offset: 0x0000DA4D
		internal void AddArc(Arc arc)
		{
			this.AddArc(arc, arc);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000EA58 File Offset: 0x0000DA58
		internal virtual void AddArc(Arc start, Arc end)
		{
			State state = null;
			if (this._startArc == null)
			{
				this._startArc = start;
				this._endArc = end;
				return;
			}
			bool flag = false;
			if (this._endArc.IsEpsilonTransition && start.IsEpsilonTransition)
			{
				start = ParseElementCollection.TrimStart(start, this._backend);
				if (start.IsEpsilonTransition)
				{
					this._endArc = ParseElementCollection.TrimEnd(this._endArc, this._backend);
					if (this._endArc.IsEpsilonTransition)
					{
						State start2 = this._endArc.Start;
						State end2 = start.End;
						flag = true;
						if (start2 == null)
						{
							Arc.CopyTags(this._endArc, start, Direction.Right);
							this._startArc = start;
						}
						else if (end2 == null)
						{
							Arc.CopyTags(start, this._endArc, Direction.Left);
							end = this._endArc;
						}
						else if (this._endArc.IsPropertylessTransition && start.IsPropertylessTransition)
						{
							start.End = null;
							this._endArc.Start = null;
							this._backend.MoveInputTransitionsAndDeleteState(start2, end2);
						}
						else
						{
							Arc.CopyTags(start, this._endArc, Direction.Left);
							start.End = null;
							this._endArc.End = end2;
						}
					}
				}
			}
			if (!flag)
			{
				if (this._endArc.IsEpsilonTransition && Graph.CanTagsBeMoved(this._endArc, start))
				{
					Arc.CopyTags(this._endArc, start, Direction.Right);
					if (this._endArc.Start != null)
					{
						state = this._endArc.Start;
						this._endArc.Start = null;
					}
					if (this._endArc == this._startArc)
					{
						this._startArc = start;
					}
				}
				else if (start.IsEpsilonTransition && Graph.CanTagsBeMoved(start, this._endArc))
				{
					Arc.CopyTags(start, this._endArc, Direction.Left);
					if (start.End != null)
					{
						state = start.End;
						start.End = null;
						this._endArc.End = state;
						state = null;
					}
					if (start == end)
					{
						end = this._endArc;
					}
				}
				else
				{
					state = this._backend.CreateNewState(this._rule);
					this._endArc.End = state;
				}
				if (state != null)
				{
					start.Start = state;
				}
			}
			this._endArc = end;
		}

		// Token: 0x0400035A RID: 858
		protected Backend _backend;

		// Token: 0x0400035B RID: 859
		protected Arc _startArc;

		// Token: 0x0400035C RID: 860
		protected Arc _endArc;

		// Token: 0x020000A8 RID: 168
		internal enum Position
		{
			// Token: 0x0400035E RID: 862
			Before,
			// Token: 0x0400035F RID: 863
			After
		}
	}
}
