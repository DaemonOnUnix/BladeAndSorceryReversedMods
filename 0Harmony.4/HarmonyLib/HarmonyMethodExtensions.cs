using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x0200006C RID: 108
	public static class HarmonyMethodExtensions
	{
		// Token: 0x060001F8 RID: 504 RVA: 0x0000BF04 File Offset: 0x0000A104
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

		// Token: 0x060001F9 RID: 505 RVA: 0x0000BF58 File Offset: 0x0000A158
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

		// Token: 0x060001FA RID: 506 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		public static HarmonyMethod Clone(this HarmonyMethod original)
		{
			HarmonyMethod harmonyMethod = new HarmonyMethod();
			original.CopyTo(harmonyMethod);
			return harmonyMethod;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000BFBC File Offset: 0x0000A1BC
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
				if (f != "priority" || (int)value2 != -1)
				{
					HarmonyMethodExtensions.SetValue(resultTrv, f, value2 ?? value);
				}
			});
			return harmonyMethod;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000C018 File Offset: 0x0000A218
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

		// Token: 0x060001FD RID: 509 RVA: 0x0000C070 File Offset: 0x0000A270
		public static List<HarmonyMethod> GetFromType(Type type)
		{
			return (from attr in type.GetCustomAttributes(true)
				select HarmonyMethodExtensions.GetHarmonyMethodInfo(attr) into info
				where info != null
				select info).ToList<HarmonyMethod>();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000C0D1 File Offset: 0x0000A2D1
		public static HarmonyMethod GetMergedFromType(Type type)
		{
			return HarmonyMethod.Merge(HarmonyMethodExtensions.GetFromType(type));
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000C0E0 File Offset: 0x0000A2E0
		public static List<HarmonyMethod> GetFromMethod(MethodBase method)
		{
			return (from attr in method.GetCustomAttributes(true)
				select HarmonyMethodExtensions.GetHarmonyMethodInfo(attr) into info
				where info != null
				select info).ToList<HarmonyMethod>();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000C141 File Offset: 0x0000A341
		public static HarmonyMethod GetMergedFromMethod(MethodBase method)
		{
			return HarmonyMethod.Merge(HarmonyMethodExtensions.GetFromMethod(method));
		}
	}
}
