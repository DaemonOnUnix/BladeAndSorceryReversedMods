using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001DD RID: 477
	public sealed class ScopeDebugInformation : DebugInformation
	{
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x000342EE File Offset: 0x000324EE
		// (set) Token: 0x06000F09 RID: 3849 RVA: 0x000342F6 File Offset: 0x000324F6
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

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x000342FF File Offset: 0x000324FF
		// (set) Token: 0x06000F0B RID: 3851 RVA: 0x00034307 File Offset: 0x00032507
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

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x00034310 File Offset: 0x00032510
		// (set) Token: 0x06000F0D RID: 3853 RVA: 0x00034318 File Offset: 0x00032518
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

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x00034321 File Offset: 0x00032521
		public bool HasScopes
		{
			get
			{
				return !this.scopes.IsNullOrEmpty<ScopeDebugInformation>();
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x00034331 File Offset: 0x00032531
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

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x00034353 File Offset: 0x00032553
		public bool HasVariables
		{
			get
			{
				return !this.variables.IsNullOrEmpty<VariableDebugInformation>();
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x00034363 File Offset: 0x00032563
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

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x00034385 File Offset: 0x00032585
		public bool HasConstants
		{
			get
			{
				return !this.constants.IsNullOrEmpty<ConstantDebugInformation>();
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x00034395 File Offset: 0x00032595
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

		// Token: 0x06000F14 RID: 3860 RVA: 0x000343B7 File Offset: 0x000325B7
		internal ScopeDebugInformation()
		{
			this.token = new MetadataToken(TokenType.LocalScope);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x000343CF File Offset: 0x000325CF
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

		// Token: 0x06000F16 RID: 3862 RVA: 0x00034400 File Offset: 0x00032600
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

		// Token: 0x04000914 RID: 2324
		internal InstructionOffset start;

		// Token: 0x04000915 RID: 2325
		internal InstructionOffset end;

		// Token: 0x04000916 RID: 2326
		internal ImportDebugInformation import;

		// Token: 0x04000917 RID: 2327
		internal Collection<ScopeDebugInformation> scopes;

		// Token: 0x04000918 RID: 2328
		internal Collection<VariableDebugInformation> variables;

		// Token: 0x04000919 RID: 2329
		internal Collection<ConstantDebugInformation> constants;
	}
}
