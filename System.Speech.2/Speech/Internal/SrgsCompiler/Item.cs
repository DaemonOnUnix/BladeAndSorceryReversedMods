using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000F0 RID: 240
	internal sealed class Item : ParseElementCollection, IItem, IElement
	{
		// Token: 0x06000874 RID: 2164 RVA: 0x0002647C File Offset: 0x0002467C
		internal Item(Backend backend, Rule rule, int minRepeat, int maxRepeat, float repeatProbability, float weigth)
			: base(backend, rule)
		{
			this._minRepeat = minRepeat;
			this._maxRepeat = maxRepeat;
			this._repeatProbability = repeatProbability;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000264B8 File Offset: 0x000246B8
		void IElement.PostParse(IElement parentElement)
		{
			if (this._maxRepeat != this._minRepeat && this._startArc != null && this._startArc == this._endArc && this._endArc.IsEpsilonTransition && !this._endArc.IsPropertylessTransition)
			{
				XmlParser.ThrowSrgsException(SRID.InvalidTagInAnEmptyItem, new object[0]);
			}
			if (this._startArc == null || this._maxRepeat == 0)
			{
				if (this._maxRepeat == 0 && this._startArc != null && this._startArc.End != null)
				{
					State end = this._startArc.End;
					this._startArc.End = null;
					this._backend.DeleteSubGraph(end);
				}
				this._startArc = (this._endArc = this._backend.EpsilonTransition(this._repeatProbability));
			}
			else if (this._minRepeat != 1 || this._maxRepeat != 1)
			{
				this._startArc = base.InsertState(this._startArc, this._repeatProbability, ParseElementCollection.Position.Before);
				State end2 = this._startArc.End;
				if (this._maxRepeat == 2147483647 && this._minRepeat == 1)
				{
					this._endArc = base.InsertState(this._endArc, 1f, ParseElementCollection.Position.After);
					this.AddEpsilonTransition(this._endArc.Start, end2, 1f - this._repeatProbability);
				}
				else
				{
					State state = end2;
					uint num = 1U;
					while ((ulong)num < (ulong)((long)this._maxRepeat) && num < 255U)
					{
						State state2 = this._backend.CreateNewState(this._endArc.Start.Rule);
						State state3 = this._backend.CloneSubGraph(state, this._endArc.Start, state2);
						this._endArc.End = state2;
						this._endArc = state3.OutArcs.First;
						if (this._maxRepeat == 2147483647)
						{
							if ((ulong)num == (ulong)((long)(this._minRepeat - 1)))
							{
								this._endArc = base.InsertState(this._endArc, 1f, ParseElementCollection.Position.After);
								this.AddEpsilonTransition(this._endArc.Start, state2, 1f - this._repeatProbability);
								break;
							}
						}
						else if ((ulong)num <= (ulong)((long)(this._maxRepeat - this._minRepeat)))
						{
							this.AddEpsilonTransition(end2, state2, 1f - this._repeatProbability);
						}
						state = state2;
						num += 1U;
					}
				}
				if (this._minRepeat == 0 && (this._startArc != this._endArc || !this._startArc.IsEpsilonTransition))
				{
					if (!this._endArc.IsEpsilonTransition || this._endArc.SemanticTagCount > 0)
					{
						this._endArc = base.InsertState(this._endArc, 1f, ParseElementCollection.Position.After);
					}
					this.AddEpsilonTransition(end2, this._endArc.Start, 1f - this._repeatProbability);
				}
				this._startArc = ParseElementCollection.TrimStart(this._startArc, this._backend);
			}
			base.PostParse((ParseElementCollection)parentElement);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000267AC File Offset: 0x000249AC
		private void AddEpsilonTransition(State start, State end, float weigth)
		{
			Arc arc = this._backend.EpsilonTransition(weigth);
			arc.Start = start;
			arc.End = end;
		}

		// Token: 0x04000609 RID: 1545
		private float _repeatProbability = 0.5f;

		// Token: 0x0400060A RID: 1546
		private int _minRepeat = -1;

		// Token: 0x0400060B RID: 1547
		private int _maxRepeat = -1;

		// Token: 0x0400060C RID: 1548
		private const int NotSet = -1;
	}
}
