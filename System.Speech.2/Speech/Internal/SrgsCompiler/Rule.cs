using System;
using System.Collections.Generic;
using System.Speech.Internal.SrgsParser;
using System.Text;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000F6 RID: 246
	internal sealed class Rule : ParseElementCollection, IRule, IElement, IComparable<Rule>
	{
		// Token: 0x060008A5 RID: 2213 RVA: 0x0002730A File Offset: 0x0002550A
		internal Rule(int iSerialize)
			: base(null, null)
		{
			this._iSerialize = iSerialize;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0002733C File Offset: 0x0002553C
		internal Rule(Backend backend, string name, CfgRule cfgRule, int iSerialize, GrammarOptions SemanticFormat, ref int cImportedRules)
			: base(backend, null)
		{
			this._rule = this;
			this.Init(name, cfgRule, iSerialize, SemanticFormat, ref cImportedRules);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002737C File Offset: 0x0002557C
		internal Rule(Backend backend, string name, int offsetName, SPCFGRULEATTRIBUTES attributes, int id, int iSerialize, GrammarOptions SemanticFormat, ref int cImportedRules)
			: base(backend, null)
		{
			this._rule = this;
			this.Init(name, new CfgRule(id, offsetName, attributes), iSerialize, SemanticFormat, ref cImportedRules);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x000273D0 File Offset: 0x000255D0
		int IComparable<Rule>.CompareTo(Rule rule2)
		{
			if (this._cfgRule.Import)
			{
				if (!rule2._cfgRule.Import)
				{
					return -1;
				}
				return this._cfgRule._nameOffset - rule2._cfgRule._nameOffset;
			}
			else if (this._cfgRule.Dynamic)
			{
				if (!rule2._cfgRule.Dynamic)
				{
					return 1;
				}
				return this._cfgRule._nameOffset - rule2._cfgRule._nameOffset;
			}
			else
			{
				if (rule2._cfgRule.Import)
				{
					return 1;
				}
				if (!rule2._cfgRule.Dynamic)
				{
					return this._cfgRule._nameOffset - rule2._cfgRule._nameOffset;
				}
				return -1;
			}
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0002747C File Offset: 0x0002567C
		internal void Validate()
		{
			if (!this._cfgRule.Dynamic && !this._cfgRule.Import && this._id != "VOID" && this._firstState.NumArcs == 0)
			{
				XmlParser.ThrowSrgsException(SRID.EmptyRule, new object[0]);
				return;
			}
			this._fHasDynamicRef = this._cfgRule.Dynamic;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x000274E4 File Offset: 0x000256E4
		internal void PopulateDynamicRef(ref int iRecursiveDepth)
		{
			if (iRecursiveDepth > 256)
			{
				XmlParser.ThrowSrgsException(SRID.MaxTransitionsCount, new object[0]);
			}
			foreach (Rule rule in this._listRules)
			{
				if (!rule._fHasDynamicRef)
				{
					rule._fHasDynamicRef = true;
					rule.PopulateDynamicRef(ref iRecursiveDepth);
				}
			}
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0002755C File Offset: 0x0002575C
		internal Rule Clone(StringBlob symbol, string ruleName)
		{
			Rule rule = new Rule(this._iSerialize);
			int num2;
			int num = symbol.Add(ruleName, out num2);
			rule._id = ruleName;
			rule._cfgRule = new CfgRule(num2, num, this._cfgRule._flag);
			rule._cfgRule.DirtyRule = true;
			rule._cfgRule.FirstArcIndex = 0U;
			return rule;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x000275B8 File Offset: 0x000257B8
		internal void Serialize(StreamMarshaler streamBuffer)
		{
			this._cfgRule.FirstArcIndex = (uint)((this._firstState != null && !this._firstState.OutArcs.IsEmpty) ? this._firstState.SerializeId : 0);
			this._cfgRule.DirtyRule = true;
			streamBuffer.WriteStream(this._cfgRule);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00027618 File Offset: 0x00025818
		void IElement.PostParse(IElement grammar)
		{
			if (this._endArc == null)
			{
				this._firstState = this._backend.CreateNewState(this);
				return;
			}
			Rule.TrimEndEpsilons(this._endArc, this._backend);
			if (this._startArc.IsEpsilonTransition && this._startArc.End != null && Graph.MoveSemanticTagRight(this._startArc))
			{
				this._firstState = this._startArc.End;
				this._startArc.End = null;
				return;
			}
			this._firstState = this._backend.CreateNewState(this);
			this._startArc.Start = this._firstState;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000276B9 File Offset: 0x000258B9
		void IRule.CreateScript(IGrammar grammar, string rule, string method, RuleMethodScript type)
		{
			((GrammarElement)grammar).CustomGrammar._scriptRefs.Add(new ScriptRef(rule, method, type));
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x000276D9 File Offset: 0x000258D9
		internal string Name
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x000276E1 File Offset: 0x000258E1
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x000276E9 File Offset: 0x000258E9
		string IRule.BaseClass
		{
			get
			{
				return this._baseclass;
			}
			set
			{
				this._baseclass = value;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x000276E1 File Offset: 0x000258E1
		internal string BaseClass
		{
			get
			{
				return this._baseclass;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x000276F2 File Offset: 0x000258F2
		internal StringBuilder Script
		{
			get
			{
				return this._script;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x000276FA File Offset: 0x000258FA
		internal StringBuilder Constructors
		{
			get
			{
				return this._constructors;
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00027704 File Offset: 0x00025904
		private void Init(string id, CfgRule cfgRule, int iSerialize, GrammarOptions SemanticFormat, ref int cImportedRules)
		{
			this._id = id;
			this._cfgRule = cfgRule;
			this._firstState = null;
			this._cfgRule.DirtyRule = true;
			this._iSerialize = iSerialize;
			this._fHasExitPath = false;
			this._fHasDynamicRef = false;
			this._fIsEpsilonRule = false;
			this._fStaticRule = false;
			if (this._cfgRule.Import)
			{
				cImportedRules++;
			}
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0002776C File Offset: 0x0002596C
		private static void TrimEndEpsilons(Arc end, Backend backend)
		{
			State start = end.Start;
			if (start != null && end.IsEpsilonTransition && start.OutArcs.CountIsOne && Graph.MoveSemanticTagLeft(end))
			{
				end.Start = null;
				foreach (Arc arc in start.InArcs.ToList())
				{
					arc.End = null;
					Rule.TrimEndEpsilons(arc, backend);
				}
				backend.DeleteState(start);
			}
		}

		// Token: 0x0400061B RID: 1563
		internal CfgRule _cfgRule;

		// Token: 0x0400061C RID: 1564
		internal State _firstState;

		// Token: 0x0400061D RID: 1565
		internal bool _fHasExitPath;

		// Token: 0x0400061E RID: 1566
		internal bool _fHasDynamicRef;

		// Token: 0x0400061F RID: 1567
		internal bool _fIsEpsilonRule;

		// Token: 0x04000620 RID: 1568
		internal int _iSerialize;

		// Token: 0x04000621 RID: 1569
		internal int _iSerialize2;

		// Token: 0x04000622 RID: 1570
		internal List<Rule> _listRules = new List<Rule>();

		// Token: 0x04000623 RID: 1571
		internal bool _fStaticRule;

		// Token: 0x04000624 RID: 1572
		private string _id;

		// Token: 0x04000625 RID: 1573
		private string _baseclass;

		// Token: 0x04000626 RID: 1574
		private StringBuilder _script = new StringBuilder();

		// Token: 0x04000627 RID: 1575
		private StringBuilder _constructors = new StringBuilder();
	}
}
