using System;
using System.Reflection;
using ThunderRoad;

namespace SheathFramework
{
	// Token: 0x02000002 RID: 2
	public static class HolderExtension
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		internal static void RaiseSnapped(this object source, Item item)
		{
			MulticastDelegate multicastDelegate = (MulticastDelegate)typeof(Holder).GetField("Snapped", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(source);
			if (multicastDelegate != null)
			{
				multicastDelegate.DynamicInvoke(new object[] { item });
			}
		}
	}
}
