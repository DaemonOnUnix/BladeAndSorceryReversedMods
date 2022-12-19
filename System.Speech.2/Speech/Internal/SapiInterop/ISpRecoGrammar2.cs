using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000153 RID: 339
	[Guid("4B37BC9E-9ED6-44a3-93D3-18F022B79EC3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpRecoGrammar2
	{
		// Token: 0x06000A1E RID: 2590
		void GetRules(out IntPtr ppCoMemRules, out uint puNumRules);

		// Token: 0x06000A1F RID: 2591
		void LoadCmdFromFile2([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, SPLOADOPTIONS Options, [MarshalAs(UnmanagedType.LPWStr)] string pszSharingUri, [MarshalAs(UnmanagedType.LPWStr)] string pszBaseUri);

		// Token: 0x06000A20 RID: 2592
		void LoadCmdFromMemory2(IntPtr pGrammar, SPLOADOPTIONS Options, [MarshalAs(UnmanagedType.LPWStr)] string pszSharingUri, [MarshalAs(UnmanagedType.LPWStr)] string pszBaseUri);

		// Token: 0x06000A21 RID: 2593
		void SetRulePriority([MarshalAs(UnmanagedType.LPWStr)] string pszRuleName, uint ulRuleId, int nRulePriority);

		// Token: 0x06000A22 RID: 2594
		void SetRuleWeight([MarshalAs(UnmanagedType.LPWStr)] string pszRuleName, uint ulRuleId, float flWeight);

		// Token: 0x06000A23 RID: 2595
		void SetDictationWeight(float flWeight);

		// Token: 0x06000A24 RID: 2596
		void SetGrammarLoader(ISpGrammarResourceLoader pLoader);

		// Token: 0x06000A25 RID: 2597
		void Slot2();
	}
}
