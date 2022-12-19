using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000064 RID: 100
	public class HarmonyMethod
	{
		// Token: 0x060001CB RID: 459 RVA: 0x0000A9AA File Offset: 0x00008BAA
		public HarmonyMethod()
		{
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000A9BC File Offset: 0x00008BBC
		private void ImportMethod(MethodInfo theMethod)
		{
			this.method = theMethod;
			if (this.method != null)
			{
				List<HarmonyMethod> fromMethod = HarmonyMethodExtensions.GetFromMethod(this.method);
				if (fromMethod != null)
				{
					HarmonyMethod.Merge(fromMethod).CopyTo(this);
				}
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000A9F3 File Offset: 0x00008BF3
		public HarmonyMethod(MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.ImportMethod(method);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000AA18 File Offset: 0x00008C18
		public HarmonyMethod(MethodInfo method, int priority = -1, string[] before = null, string[] after = null, bool? debug = null)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.ImportMethod(method);
			this.priority = priority;
			this.before = before;
			this.after = after;
			this.debug = debug;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000AA68 File Offset: 0x00008C68
		public HarmonyMethod(Type methodType, string methodName, Type[] argumentTypes = null)
		{
			MethodInfo methodInfo = AccessTools.Method(methodType, methodName, argumentTypes, null);
			if (methodInfo == null)
			{
				throw new ArgumentException(string.Format("Cannot not find method for type {0} and name {1} and parameters {2}", methodType, methodName, (argumentTypes != null) ? argumentTypes.Description() : null));
			}
			this.ImportMethod(methodInfo);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000AAB4 File Offset: 0x00008CB4
		public static List<string> HarmonyFields()
		{
			return (from s in AccessTools.GetFieldNames(typeof(HarmonyMethod))
				where s != "method"
				select s).ToList<string>();
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000AAF0 File Offset: 0x00008CF0
		public static HarmonyMethod Merge(List<HarmonyMethod> attributes)
		{
			HarmonyMethod harmonyMethod = new HarmonyMethod();
			if (attributes == null)
			{
				return harmonyMethod;
			}
			Traverse resultTrv = Traverse.Create(harmonyMethod);
			attributes.ForEach(delegate(HarmonyMethod attribute)
			{
				Traverse trv = Traverse.Create(attribute);
				HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
				{
					object value = trv.Field(f).GetValue();
					if (value != null)
					{
						HarmonyMethodExtensions.SetValue(resultTrv, f, value);
					}
				});
			});
			return harmonyMethod;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000AB30 File Offset: 0x00008D30
		public override string ToString()
		{
			string result = "";
			Traverse trv = Traverse.Create(this);
			HarmonyMethod.HarmonyFields().ForEach(delegate(string f)
			{
				if (result.Length > 0)
				{
					result += ", ";
				}
				result += string.Format("{0}={1}", f, trv.Field(f).GetValue());
			});
			return "HarmonyMethod[" + result + "]";
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000AB88 File Offset: 0x00008D88
		internal string Description()
		{
			string text = ((this.declaringType != null) ? this.declaringType.FullName : "undefined");
			string text2 = this.methodName ?? "undefined";
			string text3 = ((this.methodType != null) ? this.methodType.Value.ToString() : "undefined");
			string text4 = ((this.argumentTypes != null) ? this.argumentTypes.Description() : "undefined");
			return string.Concat(new string[] { "(class=", text, ", methodname=", text2, ", type=", text3, ", args=", text4, ")" });
		}

		// Token: 0x0400011B RID: 283
		public MethodInfo method;

		// Token: 0x0400011C RID: 284
		public Type declaringType;

		// Token: 0x0400011D RID: 285
		public string methodName;

		// Token: 0x0400011E RID: 286
		public MethodType? methodType;

		// Token: 0x0400011F RID: 287
		public Type[] argumentTypes;

		// Token: 0x04000120 RID: 288
		public int priority = -1;

		// Token: 0x04000121 RID: 289
		public string[] before;

		// Token: 0x04000122 RID: 290
		public string[] after;

		// Token: 0x04000123 RID: 291
		public HarmonyReversePatchType? reversePatchType;

		// Token: 0x04000124 RID: 292
		public bool? debug;

		// Token: 0x04000125 RID: 293
		public bool nonVirtualDelegate;
	}
}
