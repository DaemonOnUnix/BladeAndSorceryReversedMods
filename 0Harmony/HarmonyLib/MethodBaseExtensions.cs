using System;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000190 RID: 400
	public static class MethodBaseExtensions
	{
		// Token: 0x06000687 RID: 1671 RVA: 0x000164F0 File Offset: 0x000146F0
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
