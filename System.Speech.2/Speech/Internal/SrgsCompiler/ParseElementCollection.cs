using System;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000F4 RID: 244
	internal abstract class ParseElementCollection : ParseElement
	{
		// Token: 0x06000899 RID: 2201 RVA: 0x00026D25 File Offset: 0x00024F25
		protected ParseElementCollection(Backend backend, Rule rule)
			: base(rule)
		{
			this._backend = backend;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00026D38 File Offset: 0x00024F38
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

		// Token: 0x0600089B RID: 2203 RVA: 0x00026DE0 File Offset: 0x00024FE0
		internal void AddSementicPropertyTag(CfgGrammar.CfgProperty propertyInfo)
		{
			if (this._startArc == null)
			{
				this._startArc = (this._endArc = this._backend.EpsilonTransition(1f));
			}
			this._backend.AddPropertyTag(this._startArc, this._endArc, propertyInfo);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00026E2C File Offset: 0x0002502C
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

		// Token: 0x0600089D RID: 2205 RVA: 0x00026ED4 File Offset: 0x000250D4
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

		// Token: 0x0600089E RID: 2206 RVA: 0x00026F4C File Offset: 0x0002514C
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

		// Token: 0x0600089F RID: 2207 RVA: 0x00026FBD File Offset: 0x000251BD
		protected void PostParse(ParseElementCollection parent)
		{
			if (this._startArc != null)
			{
				parent.AddArc(this._startArc, this._endArc);
			}
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00026FD9 File Offset: 0x000251D9
		internal void AddArc(Arc arc)
		{
			this.AddArc(arc, arc);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00026FE4 File Offset: 0x000251E4
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

		// Token: 0x04000617 RID: 1559
		protected Backend _backend;

		// Token: 0x04000618 RID: 1560
		protected Arc _startArc;

		// Token: 0x04000619 RID: 1561
		protected Arc _endArc;

		// Token: 0x020001A7 RID: 423
		internal enum Position
		{
			// Token: 0x0400099D RID: 2461
			Before,
			// Token: 0x0400099E RID: 2462
			After
		}
	}
}
