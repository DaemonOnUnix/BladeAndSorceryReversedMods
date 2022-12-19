using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000330 RID: 816
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	internal class IgnoresAccessChecksToAttribute : Attribute
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x00040471 File Offset: 0x0003E671
		public string AssemblyName { get; }

		// Token: 0x060012F8 RID: 4856 RVA: 0x00040479 File Offset: 0x0003E679
		public IgnoresAccessChecksToAttribute(string assemblyName)
		{
			this.AssemblyName = assemblyName;
		}
	}
}
