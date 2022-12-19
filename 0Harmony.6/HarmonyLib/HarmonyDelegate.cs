using System;

namespace HarmonyLib
{
	// Token: 0x0200004A RID: 74
	[AttributeUsage(AttributeTargets.Delegate, AllowMultiple = true)]
	public class HarmonyDelegate : HarmonyPatch
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00009D2D File Offset: 0x00007F2D
		public HarmonyDelegate(Type declaringType)
			: base(declaringType)
		{
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009D36 File Offset: 0x00007F36
		public HarmonyDelegate(Type declaringType, Type[] argumentTypes)
			: base(declaringType, argumentTypes)
		{
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009D40 File Offset: 0x00007F40
		public HarmonyDelegate(Type declaringType, string methodName)
			: base(declaringType, methodName)
		{
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00009D4A File Offset: 0x00007F4A
		public HarmonyDelegate(Type declaringType, string methodName, params Type[] argumentTypes)
			: base(declaringType, methodName, argumentTypes)
		{
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009D55 File Offset: 0x00007F55
		public HarmonyDelegate(Type declaringType, string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(declaringType, methodName, argumentTypes, argumentVariations)
		{
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00009D62 File Offset: 0x00007F62
		public HarmonyDelegate(Type declaringType, MethodDispatchType methodDispatchType)
			: base(declaringType, MethodType.Normal)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00009D7B File Offset: 0x00007F7B
		public HarmonyDelegate(Type declaringType, MethodDispatchType methodDispatchType, params Type[] argumentTypes)
			: base(declaringType, MethodType.Normal, argumentTypes)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00009D95 File Offset: 0x00007F95
		public HarmonyDelegate(Type declaringType, MethodDispatchType methodDispatchType, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(declaringType, MethodType.Normal, argumentTypes, argumentVariations)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00009DB1 File Offset: 0x00007FB1
		public HarmonyDelegate(Type declaringType, string methodName, MethodDispatchType methodDispatchType)
			: base(declaringType, methodName, MethodType.Normal)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00009DCB File Offset: 0x00007FCB
		public HarmonyDelegate(string methodName)
			: base(methodName)
		{
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00009DD4 File Offset: 0x00007FD4
		public HarmonyDelegate(string methodName, params Type[] argumentTypes)
			: base(methodName, argumentTypes)
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00009DDE File Offset: 0x00007FDE
		public HarmonyDelegate(string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(methodName, argumentTypes, argumentVariations)
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00009DE9 File Offset: 0x00007FE9
		public HarmonyDelegate(string methodName, MethodDispatchType methodDispatchType)
			: base(methodName, MethodType.Normal)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00009E02 File Offset: 0x00008002
		public HarmonyDelegate(MethodDispatchType methodDispatchType)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00009E19 File Offset: 0x00008019
		public HarmonyDelegate(MethodDispatchType methodDispatchType, params Type[] argumentTypes)
			: base(MethodType.Normal, argumentTypes)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00009E32 File Offset: 0x00008032
		public HarmonyDelegate(MethodDispatchType methodDispatchType, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(MethodType.Normal, argumentTypes, argumentVariations)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00009E4C File Offset: 0x0000804C
		public HarmonyDelegate(Type[] argumentTypes)
			: base(argumentTypes)
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00009E55 File Offset: 0x00008055
		public HarmonyDelegate(Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(argumentTypes, argumentVariations)
		{
		}
	}
}
