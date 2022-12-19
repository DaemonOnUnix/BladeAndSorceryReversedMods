using System;
using System.Collections.Generic;
using System.Speech.Internal.SrgsParser;
using System.Text;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000B2 RID: 178
	internal sealed class Rule : ParseElementCollection, IRule, IElement, IComparable<Rule>
	{
		// Token: 0x060003EB RID: 1003 RVA: 0x0000F612 File Offset: 0x0000E612
		internal Rule(int iSerialize)
			: base(null, null)
		{
			this._iSerialize = iSerialize;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000F644 File Offset: 0x0000E644
		internal Rule(Backend backend, string name, CfgRule cfgRule, int iSerialize, GrammarOptions SemanticFormat, ref int cImportedRules)
			: base(backend, null)
		{
			this._rule = this;
			this.Init(name, cfgRule, iSerialize, SemanticFormat, ref cImportedRules);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000F684 File Offset: 0x0000E684
		internal Rule(Backend backend, string name, int offsetName, SPCFGRULEATTRIBUTES attributes, int id, int iSerialize, GrammarOptions SemanticFormat, ref int cImportedRules)
			: base(backend, null)
		{
			this._rule = this;
			this.Init(name, new CfgRule(id, offsetName, attributes), iSerialize, SemanticFormat, ref cImportedRules);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000F6D8 File Offset: 0x0000E6D8
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

		// Token: 0x060003EF RID: 1007 RVA: 0x0000F784 File Offset: 0x0000E784
		internal void Validate()
		{
			if (!this._cfgRule.Dynamic && !this._cfgRule.Import && this._id != "VOID" && this._firstState.NumArcs == 0)
			{
				XmlParser.ThrowSrgsException(SRID.EmptyRule, new object[0]);
				return;
			}
			this._fHasDynamicRef = this._cfgRule.Dynamic;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000F7EC File Offset: 0x0000E7EC
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

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000F864 File Offset: 0x0000E864
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

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000F8C0 File Offset: 0x0000E8C0
		internal void Serialize(StreamMarshaler streamBuffer)
		{
			this._cfgRule.FirstArcIndex = (uint)((this._firstState != null && !this._firstState.OutArcs.IsEmpty) ? this._firstState.SerializeId : 0);
			this._cfgRule.DirtyRule = true;
			streamBuffer.WriteStream(this._cfgRule);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000F920 File Offset: 0x0000E920
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

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000F9C1 File Offset: 0x0000E9C1
		void IRule.CreateScript(IGrammar grammar, string rule, string method, RuleMethodScript type)
		{
			((GrammarElement)grammar).CustomGrammar._scriptRefs.Add(new ScriptRef(rule, method, type));
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0000F9E1 File Offset: 0x0000E9E1
		internal string Name
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000F9E9 File Offset: 0x0000E9E9
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x0000F9F1 File Offset: 0x0000E9F1
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

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0000F9FA File Offset: 0x0000E9FA
		internal string BaseClass
		{
			get
			{
				return this._baseclass;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0000FA02 File Offset: 0x0000EA02
		internal StringBuilder Script
		{
			get
			{
				return this._script;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0000FA0A File Offset: 0x0000EA0A
		internal StringBuilder Constructors
		{
			get
			{
				return this._constructors;
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000FA14 File Offset: 0x0000EA14
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

		// Token: 0x060003FC RID: 1020 RVA: 0x0000FA7C File Offset: 0x0000EA7C
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

		// Token: 0x0400036D RID: 877
		internal CfgRule _cfgRule;

		// Token: 0x0400036E RID: 878
		internal State _firstState;

		// Token: 0x0400036F RID: 879
		internal bool _fHasExitPath;

		// Token: 0x04000370 RID: 880
		internal bool _fHasDynamicRef;

		// Token: 0x04000371 RID: 881
		internal bool _fIsEpsilonRule;

		// Token: 0x04000372 RID: 882
		internal int _iSerialize;

		// Token: 0x04000373 RID: 883
		internal int _iSerialize2;

		// Token: 0x04000374 RID: 884
		internal List<Rule> _listRules = new List<Rule>();

		// Token: 0x04000375 RID: 885
		internal bool _fStaticRule;

		// Token: 0x04000376 RID: 886
		private string _id;

		// Token: 0x04000377 RID: 887
		private string _baseclass;

		// Token: 0x04000378 RID: 888
		private StringBuilder _script = new StringBuilder();

		// Token: 0x04000379 RID: 889
		private StringBuilder _constructors = new StringBuilder();
	}
}
