using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000067 RID: 103
	public class HarmonyMethod
	{
		// Token: 0x060001E6 RID: 486 RVA: 0x0000BB5E File Offset: 0x00009D5E
		public HarmonyMethod()
		{
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000BB70 File Offset: 0x00009D70
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

		// Token: 0x060001E8 RID: 488 RVA: 0x0000BBA7 File Offset: 0x00009DA7
		public HarmonyMethod(MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.ImportMethod(method);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000BBCC File Offset: 0x00009DCC
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

		// Token: 0x060001EA RID: 490 RVA: 0x0000BC1C File Offset: 0x00009E1C
		public HarmonyMethod(Type methodType, string methodName, Type[] argumentTypes = null)
		{
			MethodInfo methodInfo = AccessTools.Method(methodType, methodName, argumentTypes, null);
			if (methodInfo == null)
			{
				throw new ArgumentException(string.Format("Cannot not find method for type {0} and name {1} and parameters {2}", methodType, methodName, (argumentTypes != null) ? argumentTypes.Description() : null));
			}
			this.ImportMethod(methodInfo);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000BC68 File Offset: 0x00009E68
		public static List<string> HarmonyFields()
		{
			return (from s in AccessTools.GetFieldNames(typeof(HarmonyMethod))
				where s != "method"
				select s).ToList<string>();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000BCA4 File Offset: 0x00009EA4
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
					if (value != null && (f != "priority" || (int)value != -1))
					{
						HarmonyMethodExtensions.SetValue(resultTrv, f, value);
					}
				});
			});
			return harmonyMethod;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000BCE4 File Offset: 0x00009EE4
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

		// Token: 0x060001EE RID: 494 RVA: 0x0000BD3C File Offset: 0x00009F3C
		internal string Description()
		{
			string text = ((this.declaringType != null) ? this.declaringType.FullName : "undefined");
			string text2 = this.methodName ?? "undefined";
			string text3 = ((this.methodType != null) ? this.methodType.Value.ToString() : "undefined");
			string text4 = ((this.argumentTypes != null) ? this.argumentTypes.Description() : "undefined");
			return string.Concat(new string[] { "(class=", text, ", methodname=", text2, ", type=", text3, ", args=", text4, ")" });
		}

		// Token: 0x0400012C RID: 300
		public MethodInfo method;

		// Token: 0x0400012D RID: 301
		public Type declaringType;

		// Token: 0x0400012E RID: 302
		public string methodName;

		// Token: 0x0400012F RID: 303
		public MethodType? methodType;

		// Token: 0x04000130 RID: 304
		public Type[] argumentTypes;

		// Token: 0x04000131 RID: 305
		public int priority = -1;

		// Token: 0x04000132 RID: 306
		public string[] before;

		// Token: 0x04000133 RID: 307
		public string[] after;

		// Token: 0x04000134 RID: 308
		public HarmonyReversePatchType? reversePatchType;

		// Token: 0x04000135 RID: 309
		public bool? debug;

		// Token: 0x04000136 RID: 310
		public bool nonVirtualDelegate;
	}
}
