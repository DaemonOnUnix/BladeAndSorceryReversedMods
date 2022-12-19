using System;
using System.Collections.Generic;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002E6 RID: 742
	public sealed class MethodDebugInformation : DebugInformation
	{
		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x0003C957 File Offset: 0x0003AB57
		public MethodDefinition Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x0003C95F File Offset: 0x0003AB5F
		public bool HasSequencePoints
		{
			get
			{
				return !this.sequence_points.IsNullOrEmpty<SequencePoint>();
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x0003C96F File Offset: 0x0003AB6F
		public Collection<SequencePoint> SequencePoints
		{
			get
			{
				if (this.sequence_points == null)
				{
					Interlocked.CompareExchange<Collection<SequencePoint>>(ref this.sequence_points, new Collection<SequencePoint>(), null);
				}
				return this.sequence_points;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x0003C991 File Offset: 0x0003AB91
		// (set) Token: 0x060012E0 RID: 4832 RVA: 0x0003C999 File Offset: 0x0003AB99
		public ScopeDebugInformation Scope
		{
			get
			{
				return this.scope;
			}
			set
			{
				this.scope = value;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x0003C9A2 File Offset: 0x0003ABA2
		// (set) Token: 0x060012E2 RID: 4834 RVA: 0x0003C9AA File Offset: 0x0003ABAA
		public MethodDefinition StateMachineKickOffMethod
		{
			get
			{
				return this.kickoff_method;
			}
			set
			{
				this.kickoff_method = value;
			}
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0003C9B4 File Offset: 0x0003ABB4
		internal MethodDebugInformation(MethodDefinition method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.method = method;
			this.token = new MetadataToken(TokenType.MethodDebugInformation, method.MetadataToken.RID);
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0003C9FC File Offset: 0x0003ABFC
		public SequencePoint GetSequencePoint(Instruction instruction)
		{
			if (!this.HasSequencePoints)
			{
				return null;
			}
			for (int i = 0; i < this.sequence_points.Count; i++)
			{
				if (this.sequence_points[i].Offset == instruction.Offset)
				{
					return this.sequence_points[i];
				}
			}
			return null;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0003CA50 File Offset: 0x0003AC50
		public IDictionary<Instruction, SequencePoint> GetSequencePointMapping()
		{
			Dictionary<Instruction, SequencePoint> dictionary = new Dictionary<Instruction, SequencePoint>();
			if (!this.HasSequencePoints || !this.method.HasBody)
			{
				return dictionary;
			}
			Dictionary<int, SequencePoint> dictionary2 = new Dictionary<int, SequencePoint>(this.sequence_points.Count);
			for (int i = 0; i < this.sequence_points.Count; i++)
			{
				if (!dictionary2.ContainsKey(this.sequence_points[i].Offset))
				{
					dictionary2.Add(this.sequence_points[i].Offset, this.sequence_points[i]);
				}
			}
			Collection<Instruction> instructions = this.method.Body.Instructions;
			for (int j = 0; j < instructions.Count; j++)
			{
				SequencePoint sequencePoint;
				if (dictionary2.TryGetValue(instructions[j].Offset, out sequencePoint))
				{
					dictionary.Add(instructions[j], sequencePoint);
				}
			}
			return dictionary;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0003CB2B File Offset: 0x0003AD2B
		public IEnumerable<ScopeDebugInformation> GetScopes()
		{
			if (this.scope == null)
			{
				return Empty<ScopeDebugInformation>.Array;
			}
			return MethodDebugInformation.GetScopes(new ScopeDebugInformation[] { this.scope });
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0003CB4F File Offset: 0x0003AD4F
		private static IEnumerable<ScopeDebugInformation> GetScopes(IList<ScopeDebugInformation> scopes)
		{
			int num;
			for (int i = 0; i < scopes.Count; i = num + 1)
			{
				ScopeDebugInformation scope = scopes[i];
				yield return scope;
				if (scope.HasScopes)
				{
					foreach (ScopeDebugInformation scopeDebugInformation in MethodDebugInformation.GetScopes(scope.Scopes))
					{
						yield return scopeDebugInformation;
					}
					IEnumerator<ScopeDebugInformation> enumerator = null;
					scope = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0003CB60 File Offset: 0x0003AD60
		public bool TryGetName(VariableDefinition variable, out string name)
		{
			name = null;
			bool flag = false;
			string text = "";
			using (IEnumerator<ScopeDebugInformation> enumerator = this.GetScopes().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text2;
					if (enumerator.Current.TryGetName(variable, out text2))
					{
						if (!flag)
						{
							flag = true;
							text = text2;
						}
						else if (text != text2)
						{
							return false;
						}
					}
				}
			}
			name = text;
			return flag;
		}

		// Token: 0x04000991 RID: 2449
		internal MethodDefinition method;

		// Token: 0x04000992 RID: 2450
		internal Collection<SequencePoint> sequence_points;

		// Token: 0x04000993 RID: 2451
		internal ScopeDebugInformation scope;

		// Token: 0x04000994 RID: 2452
		internal MethodDefinition kickoff_method;

		// Token: 0x04000995 RID: 2453
		internal int code_size;

		// Token: 0x04000996 RID: 2454
		internal MetadataToken local_var_token;
	}
}
