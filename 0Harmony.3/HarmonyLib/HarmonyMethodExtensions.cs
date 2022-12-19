using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000069 RID: 105
	public static class HarmonyMethodExtensions
	{
		// Token: 0x060001DD RID: 477 RVA: 0x0000AD38 File Offset: 0x00008F38
		internal static void SetValue(Traverse trv, string name, object val)
		{
			if (val == null)
			{
				return;
			}
			Traverse traverse = trv.Field(name);
			if (name == "methodType" || name == "reversePatchType")
			{
				val = Enum.ToObject(Nullable.GetUnderlyingType(traverse.GetValueType()), (int)val);
			}
			traverse.SetValue(val);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000AD8C File Offset: 0x00008F8C
		public static void CopyTo(this HarmonyMethod from, HarmonyMethod to)
		{
			if (to == null)
			{
				return;
			}
			Traverse fromTrv = Traverse.Create(from);
			Traverse toTrv = Traverse.Create(to);
			HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
			{
				object value = fromTrv.Field(f).GetValue();
				if (value != null)
				{
					HarmonyMethodExtensions.SetValue(toTrv, f, value);
				}
			});
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000ADD4 File Offset: 0x00008FD4
		public static HarmonyMethod Clone(this HarmonyMethod original)
		{
			HarmonyMethod harmonyMethod = new HarmonyMethod();
			original.CopyTo(harmonyMethod);
			return harmonyMethod;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000ADF0 File Offset: 0x00008FF0
		public static HarmonyMethod Merge(this HarmonyMethod master, HarmonyMethod detail)
		{
			if (detail == null)
			{
				return master;
			}
			HarmonyMethod harmonyMethod = new HarmonyMethod();
			Traverse resultTrv = Traverse.Create(harmonyMethod);
			Traverse masterTrv = Traverse.Create(master);
			Traverse detailTrv = Traverse.Create(detail);
			HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
			{
				object value = masterTrv.Field(f).GetValue();
				object value2 = detailTrv.Field(f).GetValue();
				HarmonyMethodExtensions.SetValue(resultTrv, f, value2 ?? value);
			});
			return harmonyMethod;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000AE4C File Offset: 0x0000904C
		private static HarmonyMethod GetHarmonyMethodInfo(object attribute)
		{
			FieldInfo field = attribute.GetType().GetField("info", AccessTools.all);
			if (field == null)
			{
				return null;
			}
			if (field.FieldType.FullName != typeof(HarmonyMethod).FullName)
			{
				return null;
			}
			return AccessTools.MakeDeepCopy<HarmonyMethod>(field.GetValue(attribute));
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000AEA4 File Offset: 0x000090A4
		public static List<HarmonyMethod> GetFromType(Type type)
		{
			return (from attr in type.GetCustomAttributes(true)
				select HarmonyMethodExtensions.GetHarmonyMethodInfo(attr) into info
				where info != null
				select info).ToList<HarmonyMethod>();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000AF05 File Offset: 0x00009105
		public static HarmonyMethod GetMergedFromType(Type type)
		{
			return HarmonyMethod.Merge(HarmonyMethodExtensions.GetFromType(type));
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000AF14 File Offset: 0x00009114
		public static List<HarmonyMethod> GetFromMethod(MethodBase method)
		{
			return (from attr in method.GetCustomAttributes(true)
				select HarmonyMethodExtensions.GetHarmonyMethodInfo(attr) into info
				where info != null
				select info).ToList<HarmonyMethod>();
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000AF75 File Offset: 0x00009175
		public static HarmonyMethod GetMergedFromMethod(MethodBase method)
		{
			return HarmonyMethod.Merge(HarmonyMethodExtensions.GetFromMethod(method));
		}
	}
}
