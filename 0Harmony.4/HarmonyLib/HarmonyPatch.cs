using System;
using System.Collections.Generic;

namespace HarmonyLib
{
	// Token: 0x0200004A RID: 74
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true)]
	public class HarmonyPatch : HarmonyAttribute
	{
		// Token: 0x0600015E RID: 350 RVA: 0x0000A880 File Offset: 0x00008A80
		public HarmonyPatch()
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000A888 File Offset: 0x00008A88
		public HarmonyPatch(Type declaringType)
		{
			this.info.declaringType = declaringType;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000A89C File Offset: 0x00008A9C
		public HarmonyPatch(Type declaringType, Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000A8BC File Offset: 0x00008ABC
		public HarmonyPatch(Type declaringType, string methodName)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000A8DC File Offset: 0x00008ADC
		public HarmonyPatch(Type declaringType, string methodName, params Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000A908 File Offset: 0x00008B08
		public HarmonyPatch(Type declaringType, string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000A931 File Offset: 0x00008B31
		public HarmonyPatch(Type declaringType, MethodType methodType)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000A956 File Offset: 0x00008B56
		public HarmonyPatch(Type declaringType, MethodType methodType, params Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000A987 File Offset: 0x00008B87
		public HarmonyPatch(Type declaringType, MethodType methodType, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000A9B5 File Offset: 0x00008BB5
		public HarmonyPatch(Type declaringType, string methodName, MethodType methodType)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000A9E6 File Offset: 0x00008BE6
		public HarmonyPatch(string methodName)
		{
			this.info.methodName = methodName;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000A9FA File Offset: 0x00008BFA
		public HarmonyPatch(string methodName, params Type[] argumentTypes)
		{
			this.info.methodName = methodName;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000AA1A File Offset: 0x00008C1A
		public HarmonyPatch(string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.methodName = methodName;
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000AA36 File Offset: 0x00008C36
		public HarmonyPatch(string methodName, MethodType methodType)
		{
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000AA5B File Offset: 0x00008C5B
		public HarmonyPatch(MethodType methodType)
		{
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000AA74 File Offset: 0x00008C74
		public HarmonyPatch(MethodType methodType, params Type[] argumentTypes)
		{
			this.info.methodType = new MethodType?(methodType);
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000AA99 File Offset: 0x00008C99
		public HarmonyPatch(MethodType methodType, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.methodType = new MethodType?(methodType);
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000AABA File Offset: 0x00008CBA
		public HarmonyPatch(Type[] argumentTypes)
		{
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000AACE File Offset: 0x00008CCE
		public HarmonyPatch(Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000AADE File Offset: 0x00008CDE
		public HarmonyPatch(string typeName, string methodName, MethodType methodType = MethodType.Normal)
		{
			this.info.declaringType = AccessTools.TypeByName(typeName);
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000AB14 File Offset: 0x00008D14
		private void ParseSpecialArguments(Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			if (argumentVariations == null || argumentVariations.Length == 0)
			{
				this.info.argumentTypes = argumentTypes;
				return;
			}
			if (argumentTypes.Length < argumentVariations.Length)
			{
				throw new ArgumentException("argumentVariations contains more elements than argumentTypes", "argumentVariations");
			}
			List<Type> list = new List<Type>();
			for (int i = 0; i < argumentTypes.Length; i++)
			{
				Type type = argumentTypes[i];
				switch (argumentVariations[i])
				{
				case ArgumentType.Ref:
				case ArgumentType.Out:
					type = type.MakeByRefType();
					break;
				case ArgumentType.Pointer:
					type = type.MakePointerType();
					break;
				}
				list.Add(type);
			}
			this.info.argumentTypes = list.ToArray();
		}
	}
}
