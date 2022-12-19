using System;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x020000A0 RID: 160
	public static class MethodBaseExtensions
	{
		// Token: 0x0600035B RID: 859 RVA: 0x00010A10 File Offset: 0x0000EC10
		public static bool HasMethodBody(this MethodBase member)
		{
			MethodBody methodBody = member.GetMethodBody();
			int? num;
			if (methodBody == null)
			{
				num = null;
			}
			else
			{
				byte[] ilasByteArray = methodBody.GetILAsByteArray();
				num = ((ilasByteArray != null) ? new int?(ilasByteArray.Length) : null);
			}
			int? num2 = num;
			return num2.GetValueOrDefault() > 0;
		}
	}
}
