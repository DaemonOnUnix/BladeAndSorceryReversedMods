using System;
using System.Collections.Generic;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F0 RID: 496
	public sealed class MethodDebugInformation : DebugInformation
	{
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x00034A09 File Offset: 0x00032C09
		public MethodDefinition Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000F6D RID: 3949 RVA: 0x00034A11 File Offset: 0x00032C11
		public bool HasSequencePoints
		{
			get
			{
				return !this.sequence_points.IsNullOrEmpty<SequencePoint>();
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000F6E RID: 3950 RVA: 0x00034A21 File Offset: 0x00032C21
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

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x00034A43 File Offset: 0x00032C43
		// (set) Token: 0x06000F70 RID: 3952 RVA: 0x00034A4B File Offset: 0x00032C4B
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

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x00034A54 File Offset: 0x00032C54
		// (set) Token: 0x06000F72 RID: 3954 RVA: 0x00034A5C File Offset: 0x00032C5C
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

		// Token: 0x06000F73 RID: 3955 RVA: 0x00034A68 File Offset: 0x00032C68
		internal MethodDebugInformation(MethodDefinition method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.method = method;
			this.token = new MetadataToken(TokenType.MethodDebugInformation, method.MetadataToken.RID);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00034AB0 File Offset: 0x00032CB0
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

		// Token: 0x06000F75 RID: 3957 RVA: 0x00034B04 File Offset: 0x00032D04
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

		// Token: 0x06000F76 RID: 3958 RVA: 0x00034BDF File Offset: 0x00032DDF
		public IEnumerable<ScopeDebugInformation> GetScopes()
		{
			if (this.scope == null)
			{
				return Empty<ScopeDebugInformation>.Array;
			}
			return MethodDebugInformation.GetScopes(new ScopeDebugInformation[] { this.scope });
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00034C03 File Offset: 0x00032E03
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

		// Token: 0x06000F78 RID: 3960 RVA: 0x00034C14 File Offset: 0x00032E14
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

		// Token: 0x04000952 RID: 2386
		internal MethodDefinition method;

		// Token: 0x04000953 RID: 2387
		internal Collection<SequencePoint> sequence_points;

		// Token: 0x04000954 RID: 2388
		internal ScopeDebugInformation scope;

		// Token: 0x04000955 RID: 2389
		internal MethodDefinition kickoff_method;

		// Token: 0x04000956 RID: 2390
		internal int code_size;

		// Token: 0x04000957 RID: 2391
		internal MetadataToken local_var_token;
	}
}
