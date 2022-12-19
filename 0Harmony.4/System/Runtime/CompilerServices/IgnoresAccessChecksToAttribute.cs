using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000428 RID: 1064
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	internal class IgnoresAccessChecksToAttribute : Attribute
	{
		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x0600166D RID: 5741 RVA: 0x000483E2 File Offset: 0x000465E2
		public string AssemblyName { get; }

		// Token: 0x0600166E RID: 5742 RVA: 0x000483EA File Offset: 0x000465EA
		public IgnoresAccessChecksToAttribute(string assemblyName)
		{
			this.AssemblyName = assemblyName;
		}
	}
}
