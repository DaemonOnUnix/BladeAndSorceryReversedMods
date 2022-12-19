using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002D3 RID: 723
	public sealed class ScopeDebugInformation : DebugInformation
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x0003C142 File Offset: 0x0003A342
		// (set) Token: 0x06001272 RID: 4722 RVA: 0x0003C14A File Offset: 0x0003A34A
		public InstructionOffset Start
		{
			get
			{
				return this.start;
			}
			set
			{
				this.start = value;
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x0003C153 File Offset: 0x0003A353
		// (set) Token: 0x06001274 RID: 4724 RVA: 0x0003C15B File Offset: 0x0003A35B
		public InstructionOffset End
		{
			get
			{
				return this.end;
			}
			set
			{
				this.end = value;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x0003C164 File Offset: 0x0003A364
		// (set) Token: 0x06001276 RID: 4726 RVA: 0x0003C16C File Offset: 0x0003A36C
		public ImportDebugInformation Import
		{
			get
			{
				return this.import;
			}
			set
			{
				this.import = value;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x0003C175 File Offset: 0x0003A375
		public bool HasScopes
		{
			get
			{
				return !this.scopes.IsNullOrEmpty<ScopeDebugInformation>();
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x0003C185 File Offset: 0x0003A385
		public Collection<ScopeDebugInformation> Scopes
		{
			get
			{
				if (this.scopes == null)
				{
					Interlocked.CompareExchange<Collection<ScopeDebugInformation>>(ref this.scopes, new Collection<ScopeDebugInformation>(), null);
				}
				return this.scopes;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001279 RID: 4729 RVA: 0x0003C1A7 File Offset: 0x0003A3A7
		public bool HasVariables
		{
			get
			{
				return !this.variables.IsNullOrEmpty<VariableDebugInformation>();
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x0003C1B7 File Offset: 0x0003A3B7
		public Collection<VariableDebugInformation> Variables
		{
			get
			{
				if (this.variables == null)
				{
					Interlocked.CompareExchange<Collection<VariableDebugInformation>>(ref this.variables, new Collection<VariableDebugInformation>(), null);
				}
				return this.variables;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x0003C1D9 File Offset: 0x0003A3D9
		public bool HasConstants
		{
			get
			{
				return !this.constants.IsNullOrEmpty<ConstantDebugInformation>();
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x0003C1E9 File Offset: 0x0003A3E9
		public Collection<ConstantDebugInformation> Constants
		{
			get
			{
				if (this.constants == null)
				{
					Interlocked.CompareExchange<Collection<ConstantDebugInformation>>(ref this.constants, new Collection<ConstantDebugInformation>(), null);
				}
				return this.constants;
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0003C20B File Offset: 0x0003A40B
		internal ScopeDebugInformation()
		{
			this.token = new MetadataToken(TokenType.LocalScope);
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0003C223 File Offset: 0x0003A423
		public ScopeDebugInformation(Instruction start, Instruction end)
			: this()
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			this.start = new InstructionOffset(start);
			if (end != null)
			{
				this.end = new InstructionOffset(end);
			}
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0003C254 File Offset: 0x0003A454
		public bool TryGetName(VariableDefinition variable, out string name)
		{
			name = null;
			if (this.variables == null || this.variables.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.variables.Count; i++)
			{
				if (this.variables[i].Index == variable.Index)
				{
					name = this.variables[i].Name;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000950 RID: 2384
		internal InstructionOffset start;

		// Token: 0x04000951 RID: 2385
		internal InstructionOffset end;

		// Token: 0x04000952 RID: 2386
		internal ImportDebugInformation import;

		// Token: 0x04000953 RID: 2387
		internal Collection<ScopeDebugInformation> scopes;

		// Token: 0x04000954 RID: 2388
		internal Collection<VariableDebugInformation> variables;

		// Token: 0x04000955 RID: 2389
		internal Collection<ConstantDebugInformation> constants;
	}
}
