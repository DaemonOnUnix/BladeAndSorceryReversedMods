using System;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000006 RID: 6
	public static class FastAccess
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002174 File Offset: 0x00000374
		public static InstantiationHandler<T> CreateInstantiationHandler<T>()
		{
			ConstructorInfo constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
			if (constructor == null)
			{
				throw new ApplicationException(string.Format("The type {0} must declare an empty constructor (the constructor may be private, internal, protected, protected internal, or public).", typeof(T)));
			}
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("InstantiateObject_" + typeof(T).Name, typeof(T), null);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Newobj, constructor);
			ilgenerator.Emit(OpCodes.Ret);
			return (InstantiationHandler<T>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(InstantiationHandler<T>));
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002218 File Offset: 0x00000418
		[Obsolete("Use AccessTools.MethodDelegate<Func<T, S>>(PropertyInfo.GetGetMethod(true))")]
		public static GetterHandler<T, S> CreateGetterHandler<T, S>(PropertyInfo propertyInfo)
		{
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateGetDynamicMethod<T, S>(propertyInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Call, getMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (GetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(GetterHandler<T, S>));
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002278 File Offset: 0x00000478
		[Obsolete("Use AccessTools.FieldRefAccess<T, S>(fieldInfo)")]
		public static GetterHandler<T, S> CreateGetterHandler<T, S>(FieldInfo fieldInfo)
		{
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateGetDynamicMethod<T, S>(fieldInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (GetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(GetterHandler<T, S>));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022D0 File Offset: 0x000004D0
		[Obsolete("Use AccessTools.FieldRefAccess<T, S>(name) for fields and AccessTools.MethodDelegate<Func<T, S>>(AccessTools.PropertyGetter(typeof(T), name)) for properties")]
		public static GetterHandler<T, S> CreateFieldGetter<T, S>(params string[] names)
		{
			foreach (string text in names)
			{
				FieldInfo field = typeof(T).GetField(text, AccessTools.all);
				if (field != null)
				{
					return FastAccess.CreateGetterHandler<T, S>(field);
				}
				PropertyInfo property = typeof(T).GetProperty(text, AccessTools.all);
				if (property != null)
				{
					return FastAccess.CreateGetterHandler<T, S>(property);
				}
			}
			return null;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002338 File Offset: 0x00000538
		[Obsolete("Use AccessTools.MethodDelegate<Action<T, S>>(PropertyInfo.GetSetMethod(true))")]
		public static SetterHandler<T, S> CreateSetterHandler<T, S>(PropertyInfo propertyInfo)
		{
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateSetDynamicMethod<T, S>(propertyInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Call, setMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (SetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(SetterHandler<T, S>));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023A4 File Offset: 0x000005A4
		[Obsolete("Use AccessTools.FieldRefAccess<T, S>(fieldInfo)")]
		public static SetterHandler<T, S> CreateSetterHandler<T, S>(FieldInfo fieldInfo)
		{
			DynamicMethodDefinition dynamicMethodDefinition = FastAccess.CreateSetDynamicMethod<T, S>(fieldInfo.DeclaringType);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (SetterHandler<T, S>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(SetterHandler<T, S>));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002407 File Offset: 0x00000607
		private static DynamicMethodDefinition CreateGetDynamicMethod<T, S>(Type type)
		{
			return new DynamicMethodDefinition("DynamicGet_" + type.Name, typeof(S), new Type[] { typeof(T) });
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000243C File Offset: 0x0000063C
		private static DynamicMethodDefinition CreateSetDynamicMethod<T, S>(Type type)
		{
			return new DynamicMethodDefinition("DynamicSet_" + type.Name, typeof(void), new Type[]
			{
				typeof(T),
				typeof(S)
			});
		}
	}
}
