using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000F2 RID: 242
	internal class OneOf : ParseElementCollection, IOneOf, IElement
	{
		// Token: 0x06000894 RID: 2196 RVA: 0x00026B60 File Offset: 0x00024D60
		public OneOf(Rule rule, Backend backend)
			: base(backend, rule)
		{
			this._startState = this._backend.CreateNewState(rule);
			this._endState = this._backend.CreateNewState(rule);
			this._startArc = this._backend.EpsilonTransition(1f);
			this._startArc.End = this._startState;
			this._endArc = this._backend.EpsilonTransition(1f);
			this._endArc.Start = this._endState;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00026BE8 File Offset: 0x00024DE8
		void IElement.PostParse(IElement parentElement)
		{
			if (this._startArc.End.OutArcs.IsEmpty)
			{
				XmlParser.ThrowSrgsException(SRID.EmptyOneOf, new object[0]);
			}
			this._startArc = ParseElementCollection.TrimStart(this._startArc, this._backend);
			this._endArc = ParseElementCollection.TrimEnd(this._endArc, this._backend);
			base.PostParse((ParseElementCollection)parentElement);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00026C54 File Offset: 0x00024E54
		internal override void AddArc(Arc start, Arc end)
		{
			start = ParseElementCollection.TrimStart(start, this._backend);
			end = ParseElementCollection.TrimEnd(end, this._backend);
			State start2 = end.Start;
			State end2 = start.End;
			if ((start.IsEpsilonTransition & start.IsPropertylessTransition) && end2 != null && end2.InArcs.IsEmpty)
			{
				start.End = null;
				this._backend.MoveOutputTransitionsAndDeleteState(end2, this._startState);
			}
			else
			{
				start.Start = this._startState;
			}
			if ((end.IsEpsilonTransition & end.IsPropertylessTransition) && start2 != null && start2.OutArcs.IsEmpty)
			{
				end.Start = null;
				this._backend.MoveInputTransitionsAndDeleteState(start2, this._endState);
				return;
			}
			end.End = this._endState;
		}

		// Token: 0x04000613 RID: 1555
		private State _startState;

		// Token: 0x04000614 RID: 1556
		private State _endState;
	}
}
