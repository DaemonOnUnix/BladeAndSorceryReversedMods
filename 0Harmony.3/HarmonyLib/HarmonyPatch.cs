using System;
using System.Collections.Generic;

namespace HarmonyLib
{
	// Token: 0x02000049 RID: 73
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true)]
	public class HarmonyPatch : HarmonyAttribute
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00009A38 File Offset: 0x00007C38
		public HarmonyPatch()
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00009A40 File Offset: 0x00007C40
		public HarmonyPatch(Type declaringType)
		{
			this.info.declaringType = declaringType;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00009A54 File Offset: 0x00007C54
		public HarmonyPatch(Type declaringType, Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00009A74 File Offset: 0x00007C74
		public HarmonyPatch(Type declaringType, string methodName)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00009A94 File Offset: 0x00007C94
		public HarmonyPatch(Type declaringType, string methodName, params Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00009AC0 File Offset: 0x00007CC0
		public HarmonyPatch(Type declaringType, string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00009AE9 File Offset: 0x00007CE9
		public HarmonyPatch(Type declaringType, MethodType methodType)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00009B0E File Offset: 0x00007D0E
		public HarmonyPatch(Type declaringType, MethodType methodType, params Type[] argumentTypes)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00009B3F File Offset: 0x00007D3F
		public HarmonyPatch(Type declaringType, MethodType methodType, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.declaringType = declaringType;
			this.info.methodType = new MethodType?(methodType);
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00009B6D File Offset: 0x00007D6D
		public HarmonyPatch(Type declaringType, string methodName, MethodType methodType)
		{
			this.info.declaringType = declaringType;
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00009B9E File Offset: 0x00007D9E
		public HarmonyPatch(string methodName)
		{
			this.info.methodName = methodName;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00009BB2 File Offset: 0x00007DB2
		public HarmonyPatch(string methodName, params Type[] argumentTypes)
		{
			this.info.methodName = methodName;
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00009BD2 File Offset: 0x00007DD2
		public HarmonyPatch(string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.methodName = methodName;
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00009BEE File Offset: 0x00007DEE
		public HarmonyPatch(string methodName, MethodType methodType)
		{
			this.info.methodName = methodName;
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00009C13 File Offset: 0x00007E13
		public HarmonyPatch(MethodType methodType)
		{
			this.info.methodType = new MethodType?(methodType);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00009C2C File Offset: 0x00007E2C
		public HarmonyPatch(MethodType methodType, params Type[] argumentTypes)
		{
			this.info.methodType = new MethodType?(methodType);
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00009C51 File Offset: 0x00007E51
		public HarmonyPatch(MethodType methodType, Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.info.methodType = new MethodType?(methodType);
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00009C72 File Offset: 0x00007E72
		public HarmonyPatch(Type[] argumentTypes)
		{
			this.info.argumentTypes = argumentTypes;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00009C86 File Offset: 0x00007E86
		public HarmonyPatch(Type[] argumentTypes, ArgumentType[] argumentVariations)
		{
			this.ParseSpecialArguments(argumentTypes, argumentVariations);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00009C98 File Offset: 0x00007E98
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
