using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002E3 RID: 739
	internal sealed class StateMachineScopeDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0003C7C8 File Offset: 0x0003A9C8
		public Collection<StateMachineScope> Scopes
		{
			get
			{
				Collection<StateMachineScope> collection;
				if ((collection = this.scopes) == null)
				{
					collection = (this.scopes = new Collection<StateMachineScope>());
				}
				return collection;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x000183ED File Offset: 0x000165ED
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.StateMachineScope;
			}
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0003C7ED File Offset: 0x0003A9ED
		public StateMachineScopeDebugInformation()
			: base(StateMachineScopeDebugInformation.KindIdentifier)
		{
		}

		// Token: 0x04000987 RID: 2439
		internal Collection<StateMachineScope> scopes;

		// Token: 0x04000988 RID: 2440
		public static Guid KindIdentifier = new Guid("{6DA9A61E-F8C7-4874-BE62-68BC5630DF71}");
	}
}
