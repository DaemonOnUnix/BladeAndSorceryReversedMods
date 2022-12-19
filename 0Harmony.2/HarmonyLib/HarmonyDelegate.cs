using System;

namespace HarmonyLib
{
	// Token: 0x0200004B RID: 75
	[AttributeUsage(AttributeTargets.Delegate, AllowMultiple = true)]
	public class HarmonyDelegate : HarmonyPatch
	{
		// Token: 0x06000173 RID: 371 RVA: 0x0000ABA9 File Offset: 0x00008DA9
		public HarmonyDelegate(Type declaringType)
			: base(declaringType)
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000ABB2 File Offset: 0x00008DB2
		public HarmonyDelegate(Type declaringType, Type[] argumentTypes)
			: base(declaringType, argumentTypes)
		{
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000ABBC File Offset: 0x00008DBC
		public HarmonyDelegate(Type declaringType, string methodName)
			: base(declaringType, methodName)
		{
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000ABC6 File Offset: 0x00008DC6
		public HarmonyDelegate(Type declaringType, string methodName, params Type[] argumentTypes)
			: base(declaringType, methodName, argumentTypes)
		{
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000ABD1 File Offset: 0x00008DD1
		public HarmonyDelegate(Type declaringType, string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(declaringType, methodName, argumentTypes, argumentVariations)
		{
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000ABDE File Offset: 0x00008DDE
		public HarmonyDelegate(Type declaringType, MethodDispatchType methodDispatchType)
			: base(declaringType, MethodType.Normal)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000ABF7 File Offset: 0x00008DF7
		public HarmonyDelegate(Type declaringType, MethodDispatchType methodDispatchType, params Type[] argumentTypes)
			: base(declaringType, MethodType.Normal, argumentTypes)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000AC11 File Offset: 0x00008E11
		public HarmonyDelegate(Type declaringType, MethodDispatchType methodDispatchType, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(declaringType, MethodType.Normal, argumentTypes, argumentVariations)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000AC2D File Offset: 0x00008E2D
		public HarmonyDelegate(Type declaringType, string methodName, MethodDispatchType methodDispatchType)
			: base(declaringType, methodName, MethodType.Normal)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000AC47 File Offset: 0x00008E47
		public HarmonyDelegate(string methodName)
			: base(methodName)
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000AC50 File Offset: 0x00008E50
		public HarmonyDelegate(string methodName, params Type[] argumentTypes)
			: base(methodName, argumentTypes)
		{
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000AC5A File Offset: 0x00008E5A
		public HarmonyDelegate(string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(methodName, argumentTypes, argumentVariations)
		{
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000AC65 File Offset: 0x00008E65
		public HarmonyDelegate(string methodName, MethodDispatchType methodDispatchType)
			: base(methodName, MethodType.Normal)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000AC7E File Offset: 0x00008E7E
		public HarmonyDelegate(MethodDispatchType methodDispatchType)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000AC95 File Offset: 0x00008E95
		public HarmonyDelegate(MethodDispatchType methodDispatchType, params Type[] argumentTypes)
			: base(MethodType.Normal, argumentTypes)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000ACAE File Offset: 0x00008EAE
		public HarmonyDelegate(MethodDispatchType methodDispatchType, Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(MethodType.Normal, argumentTypes, argumentVariations)
		{
			this.info.nonVirtualDelegate = methodDispatchType == MethodDispatchType.Call;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000ACC8 File Offset: 0x00008EC8
		public HarmonyDelegate(Type[] argumentTypes)
			: base(argumentTypes)
		{
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000ACD1 File Offset: 0x00008ED1
		public HarmonyDelegate(Type[] argumentTypes, ArgumentType[] argumentVariations)
			: base(argumentTypes, argumentVariations)
		{
		}
	}
}
