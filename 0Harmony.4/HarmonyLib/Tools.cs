using System;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000193 RID: 403
	internal class Tools
	{
		// Token: 0x06000698 RID: 1688 RVA: 0x00016A00 File Offset: 0x00014C00
		internal static Tools.TypeAndName TypColonName(string typeColonName)
		{
			if (typeColonName == null)
			{
				throw new ArgumentNullException("typeColonName");
			}
			string[] array = typeColonName.Split(new char[] { ':' });
			if (array.Length != 2)
			{
				throw new ArgumentException(" must be specified as 'Namespace.Type1.Type2:MemberName", "typeColonName");
			}
			return new Tools.TypeAndName
			{
				type = AccessTools.TypeByName(array[0]),
				name = array[1]
			};
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00016A68 File Offset: 0x00014C68
		internal static void ValidateFieldType<F>(FieldInfo fieldInfo)
		{
			Type typeFromHandle = typeof(F);
			Type fieldType = fieldInfo.FieldType;
			if (typeFromHandle == fieldType)
			{
				return;
			}
			if (fieldType.IsEnum)
			{
				Type underlyingType = Enum.GetUnderlyingType(fieldType);
				if (typeFromHandle != underlyingType)
				{
					throw new ArgumentException("FieldRefAccess return type must be the same as FieldType or " + string.Format("FieldType's underlying integral type ({0}) for enum types", underlyingType));
				}
			}
			else
			{
				if (fieldType.IsValueType)
				{
					throw new ArgumentException("FieldRefAccess return type must be the same as FieldType for value types");
				}
				if (!typeFromHandle.IsAssignableFrom(fieldType))
				{
					throw new ArgumentException("FieldRefAccess return type must be assignable from FieldType for reference types");
				}
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00016AEC File Offset: 0x00014CEC
		internal static AccessTools.FieldRef<T, F> FieldRefAccess<T, F>(FieldInfo fieldInfo, bool needCastclass)
		{
			Tools.ValidateFieldType<F>(fieldInfo);
			Type typeFromHandle = typeof(T);
			Type declaringType = fieldInfo.DeclaringType;
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("__refget_" + typeFromHandle.Name + "_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[] { typeFromHandle });
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			if (fieldInfo.IsStatic)
			{
				ilgenerator.Emit(OpCodes.Ldsflda, fieldInfo);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				if (needCastclass)
				{
					ilgenerator.Emit(OpCodes.Castclass, declaringType);
				}
				ilgenerator.Emit(OpCodes.Ldflda, fieldInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (AccessTools.FieldRef<T, F>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(AccessTools.FieldRef<T, F>));
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00016BB4 File Offset: 0x00014DB4
		internal static AccessTools.StructFieldRef<T, F> StructFieldRefAccess<T, F>(FieldInfo fieldInfo) where T : struct
		{
			Tools.ValidateFieldType<F>(fieldInfo);
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("__refget_" + typeof(T).Name + "_struct_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[] { typeof(T).MakeByRefType() });
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldflda, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (AccessTools.StructFieldRef<T, F>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(AccessTools.StructFieldRef<T, F>));
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00016C58 File Offset: 0x00014E58
		internal static AccessTools.FieldRef<F> StaticFieldRefAccess<F>(FieldInfo fieldInfo)
		{
			if (!fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must be static");
			}
			Tools.ValidateFieldType<F>(fieldInfo);
			string text = "__refget_";
			Type declaringType = fieldInfo.DeclaringType;
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(text + (((declaringType != null) ? declaringType.Name : null) ?? "null") + "_static_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[0]);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldsflda, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (AccessTools.FieldRef<F>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(AccessTools.FieldRef<F>));
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00016CFD File Offset: 0x00014EFD
		internal static FieldInfo GetInstanceField(Type type, string fieldName)
		{
			FieldInfo fieldInfo = AccessTools.Field(type, fieldName);
			if (fieldInfo == null)
			{
				throw new MissingFieldException(type.Name, fieldName);
			}
			if (fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must not be static");
			}
			return fieldInfo;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00016D2C File Offset: 0x00014F2C
		internal static bool FieldRefNeedsClasscast(Type delegateInstanceType, Type declaringType)
		{
			bool flag = false;
			if (delegateInstanceType != declaringType)
			{
				flag = delegateInstanceType.IsAssignableFrom(declaringType);
				if (!flag && !declaringType.IsAssignableFrom(delegateInstanceType))
				{
					throw new ArgumentException("FieldDeclaringType must be assignable from or to T (FieldRefAccess instance type) - \"instanceOfT is FieldDeclaringType\" must be possible");
				}
			}
			return flag;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00016D64 File Offset: 0x00014F64
		internal static void ValidateStructField<T, F>(FieldInfo fieldInfo) where T : struct
		{
			if (fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must not be static");
			}
			if (fieldInfo.DeclaringType != typeof(T))
			{
				throw new ArgumentException("FieldDeclaringType must be T (StructFieldRefAccess instance type)");
			}
		}

		// Token: 0x02000194 RID: 404
		internal struct TypeAndName
		{
			// Token: 0x0400020A RID: 522
			internal Type type;

			// Token: 0x0400020B RID: 523
			internal string name;
		}
	}
}
