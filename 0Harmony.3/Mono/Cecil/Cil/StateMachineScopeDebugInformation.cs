using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001ED RID: 493
	internal sealed class StateMachineScopeDebugInformation : CustomDebugInformation
	{
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0003493C File Offset: 0x00032B3C
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

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x00012561 File Offset: 0x00010761
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.StateMachineScope;
			}
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00034961 File Offset: 0x00032B61
		public StateMachineScopeDebugInformation()
			: base(StateMachineScopeDebugInformation.KindIdentifier)
		{
		}

		// Token: 0x0400094B RID: 2379
		internal Collection<StateMachineScope> scopes;

		// Token: 0x0400094C RID: 2380
		public static Guid KindIdentifier = new Guid("{6DA9A61E-F8C7-4874-BE62-68BC5630DF71}");
	}
}
