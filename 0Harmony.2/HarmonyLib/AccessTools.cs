using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Threading;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x0200008A RID: 138
	public static class AccessTools
	{
		// Token: 0x060002B5 RID: 693 RVA: 0x0000E8B3 File Offset: 0x0000CAB3
		public static IEnumerable<Assembly> AllAssemblies()
		{
			return from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.FullName.StartsWith("Microsoft.VisualStudio")
				select a;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000E8E4 File Offset: 0x0000CAE4
		public static Type TypeByName(string name)
		{
			Type type = Type.GetType(name, false);
			if (type == null)
			{
				type = AccessTools.AllTypes().FirstOrDefault((Type t) => t.FullName == name);
			}
			if (type == null)
			{
				type = AccessTools.AllTypes().FirstOrDefault((Type t) => t.Name == name);
			}
			if (type == null)
			{
				FileLog.Debug("AccessTools.TypeByName: Could not find type named " + name);
			}
			return type;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000E958 File Offset: 0x0000CB58
		public static Type[] GetTypesFromAssembly(Assembly assembly)
		{
			Type[] array;
			try
			{
				array = assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				FileLog.Debug(string.Format("AccessTools.GetTypesFromAssembly: assembly {0} => {1}", assembly, ex));
				array = ex.Types.Where((Type type) => type != null).ToArray<Type>();
			}
			return array;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000E9C4 File Offset: 0x0000CBC4
		public static IEnumerable<Type> AllTypes()
		{
			return AccessTools.AllAssemblies().SelectMany((Assembly a) => AccessTools.GetTypesFromAssembly(a));
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000E9F0 File Offset: 0x0000CBF0
		public static T FindIncludingBaseTypes<T>(Type type, Func<Type, T> func) where T : class
		{
			T t;
			for (;;)
			{
				t = func(type);
				if (t != null)
				{
					break;
				}
				type = type.BaseType;
				if (type == null)
				{
					goto Block_1;
				}
			}
			return t;
			Block_1:
			return default(T);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000EA24 File Offset: 0x0000CC24
		public static T FindIncludingInnerTypes<T>(Type type, Func<Type, T> func) where T : class
		{
			T t = func(type);
			if (t != null)
			{
				return t;
			}
			Type[] nestedTypes = type.GetNestedTypes(AccessTools.all);
			for (int i = 0; i < nestedTypes.Length; i++)
			{
				t = AccessTools.FindIncludingInnerTypes<T>(nestedTypes[i], func);
				if (t != null)
				{
					break;
				}
			}
			return t;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000EA70 File Offset: 0x0000CC70
		public static FieldInfo DeclaredField(Type type, string name)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.DeclaredField: type is null");
				return null;
			}
			if (name == null)
			{
				FileLog.Debug("AccessTools.DeclaredField: name is null");
				return null;
			}
			FieldInfo field = type.GetField(name, AccessTools.allDeclared);
			if (field == null)
			{
				FileLog.Debug(string.Format("AccessTools.DeclaredField: Could not find field for type {0} and name {1}", type, name));
			}
			return field;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000EAB0 File Offset: 0x0000CCB0
		public static FieldInfo DeclaredField(string typeColonName)
		{
			Tools.TypeAndName typeAndName = Tools.TypColonName(typeColonName);
			FieldInfo field = typeAndName.type.GetField(typeAndName.name, AccessTools.allDeclared);
			if (field == null)
			{
				FileLog.Debug(string.Format("AccessTools.DeclaredField: Could not find field for type {0} and name {1}", typeAndName.type, typeAndName.name));
			}
			return field;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000EAF8 File Offset: 0x0000CCF8
		public static FieldInfo Field(Type type, string name)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.Field: type is null");
				return null;
			}
			if (name == null)
			{
				FileLog.Debug("AccessTools.Field: name is null");
				return null;
			}
			FieldInfo fieldInfo = AccessTools.FindIncludingBaseTypes<FieldInfo>(type, (Type t) => t.GetField(name, AccessTools.all));
			if (fieldInfo == null)
			{
				FileLog.Debug(string.Format("AccessTools.Field: Could not find field for type {0} and name {1}", type, name));
			}
			return fieldInfo;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000EB60 File Offset: 0x0000CD60
		public static FieldInfo Field(string typeColonName)
		{
			Tools.TypeAndName info = Tools.TypColonName(typeColonName);
			FieldInfo fieldInfo = AccessTools.FindIncludingBaseTypes<FieldInfo>(info.type, (Type t) => t.GetField(info.name, AccessTools.all));
			if (fieldInfo == null)
			{
				FileLog.Debug(string.Format("AccessTools.Field: Could not find field for type {0} and name {1}", info.type, info.name));
			}
			return fieldInfo;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000EBC3 File Offset: 0x0000CDC3
		public static FieldInfo DeclaredField(Type type, int idx)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.DeclaredField: type is null");
				return null;
			}
			FieldInfo fieldInfo = AccessTools.GetDeclaredFields(type).ElementAtOrDefault(idx);
			if (fieldInfo == null)
			{
				FileLog.Debug(string.Format("AccessTools.DeclaredField: Could not find field for type {0} and idx {1}", type, idx));
			}
			return fieldInfo;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000EBF9 File Offset: 0x0000CDF9
		public static PropertyInfo DeclaredProperty(Type type, string name)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.DeclaredProperty: type is null");
				return null;
			}
			if (name == null)
			{
				FileLog.Debug("AccessTools.DeclaredProperty: name is null");
				return null;
			}
			PropertyInfo property = type.GetProperty(name, AccessTools.allDeclared);
			if (property == null)
			{
				FileLog.Debug(string.Format("AccessTools.DeclaredProperty: Could not find property for type {0} and name {1}", type, name));
			}
			return property;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000EC3C File Offset: 0x0000CE3C
		public static PropertyInfo DeclaredProperty(string typeColonName)
		{
			Tools.TypeAndName typeAndName = Tools.TypColonName(typeColonName);
			PropertyInfo property = typeAndName.type.GetProperty(typeAndName.name, AccessTools.allDeclared);
			if (property == null)
			{
				FileLog.Debug(string.Format("AccessTools.DeclaredProperty: Could not find property for type {0} and name {1}", typeAndName.type, typeAndName.name));
			}
			return property;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000EC84 File Offset: 0x0000CE84
		public static MethodInfo DeclaredPropertyGetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000EC99 File Offset: 0x0000CE99
		public static MethodInfo DeclaredPropertyGetter(string typeColonName)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(typeColonName);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000ECAD File Offset: 0x0000CEAD
		public static MethodInfo DeclaredPropertySetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000ECC2 File Offset: 0x0000CEC2
		public static MethodInfo DeclaredPropertySetter(string typeColonName)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(typeColonName);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000ECD8 File Offset: 0x0000CED8
		public static PropertyInfo Property(Type type, string name)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.Property: type is null");
				return null;
			}
			if (name == null)
			{
				FileLog.Debug("AccessTools.Property: name is null");
				return null;
			}
			PropertyInfo propertyInfo = AccessTools.FindIncludingBaseTypes<PropertyInfo>(type, (Type t) => t.GetProperty(name, AccessTools.all));
			if (propertyInfo == null)
			{
				FileLog.Debug(string.Format("AccessTools.Property: Could not find property for type {0} and name {1}", type, name));
			}
			return propertyInfo;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000ED40 File Offset: 0x0000CF40
		public static PropertyInfo Property(string typeColonName)
		{
			Tools.TypeAndName info = Tools.TypColonName(typeColonName);
			PropertyInfo propertyInfo = AccessTools.FindIncludingBaseTypes<PropertyInfo>(info.type, (Type t) => t.GetProperty(info.name, AccessTools.all));
			if (propertyInfo == null)
			{
				FileLog.Debug(string.Format("AccessTools.Property: Could not find property for type {0} and name {1}", info.type, info.name));
			}
			return propertyInfo;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000EDA3 File Offset: 0x0000CFA3
		public static MethodInfo PropertyGetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.Property(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		public static MethodInfo PropertyGetter(string typeColonName)
		{
			PropertyInfo propertyInfo = AccessTools.Property(typeColonName);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000EDCC File Offset: 0x0000CFCC
		public static MethodInfo PropertySetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.Property(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000EDE1 File Offset: 0x0000CFE1
		public static MethodInfo PropertySetter(string typeColonName)
		{
			PropertyInfo propertyInfo = AccessTools.Property(typeColonName);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000EDF8 File Offset: 0x0000CFF8
		public static MethodInfo DeclaredMethod(Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.DeclaredMethod: type is null");
				return null;
			}
			if (name == null)
			{
				FileLog.Debug("AccessTools.DeclaredMethod: name is null");
				return null;
			}
			ParameterModifier[] array = new ParameterModifier[0];
			MethodInfo methodInfo;
			if (parameters == null)
			{
				methodInfo = type.GetMethod(name, AccessTools.allDeclared);
			}
			else
			{
				methodInfo = type.GetMethod(name, AccessTools.allDeclared, null, parameters, array);
			}
			if (methodInfo == null)
			{
				FileLog.Debug(string.Format("AccessTools.DeclaredMethod: Could not find method for type {0} and name {1} and parameters {2}", type, name, (parameters != null) ? parameters.Description() : null));
				return null;
			}
			if (generics != null)
			{
				methodInfo = methodInfo.MakeGenericMethod(generics);
			}
			return methodInfo;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000EE7C File Offset: 0x0000D07C
		public static MethodInfo DeclaredMethod(string typeColonName, Type[] parameters = null, Type[] generics = null)
		{
			Tools.TypeAndName typeAndName = Tools.TypColonName(typeColonName);
			return AccessTools.DeclaredMethod(typeAndName.type, typeAndName.name, parameters, generics);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000EEA4 File Offset: 0x0000D0A4
		public static MethodInfo Method(Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.Method: type is null");
				return null;
			}
			if (name == null)
			{
				FileLog.Debug("AccessTools.Method: name is null");
				return null;
			}
			ParameterModifier[] modifiers = new ParameterModifier[0];
			MethodInfo methodInfo;
			if (parameters == null)
			{
				try
				{
					methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all));
					goto IL_A4;
				}
				catch (AmbiguousMatchException ex)
				{
					methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all, null, new Type[0], modifiers));
					if (methodInfo == null)
					{
						throw new AmbiguousMatchException(string.Format("Ambiguous match in Harmony patch for {0}:{1}", type, name), ex);
					}
					goto IL_A4;
				}
			}
			methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all, null, parameters, modifiers));
			IL_A4:
			if (methodInfo == null)
			{
				string text = "AccessTools.Method: Could not find method for type {0} and name {1} and parameters {2}";
				object name2 = name;
				Type[] parameters2 = parameters;
				FileLog.Debug(string.Format(text, type, name2, (parameters2 != null) ? parameters2.Description() : null));
				return null;
			}
			if (generics != null)
			{
				methodInfo = methodInfo.MakeGenericMethod(generics);
			}
			return methodInfo;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000EFA0 File Offset: 0x0000D1A0
		public static MethodInfo Method(string typeColonName, Type[] parameters = null, Type[] generics = null)
		{
			Tools.TypeAndName typeAndName = Tools.TypColonName(typeColonName);
			return AccessTools.Method(typeAndName.type, typeAndName.name, parameters, generics);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000EFC8 File Offset: 0x0000D1C8
		public static MethodInfo EnumeratorMoveNext(MethodBase method)
		{
			if (method == null)
			{
				FileLog.Debug("AccessTools.EnumeratorMoveNext: method is null");
				return null;
			}
			IEnumerable<KeyValuePair<OpCode, object>> enumerable = from pair in PatchProcessor.ReadMethodBody(method)
				where pair.Key == OpCodes.Newobj
				select pair;
			if (enumerable.Count<KeyValuePair<OpCode, object>>() != 1)
			{
				FileLog.Debug("AccessTools.EnumeratorMoveNext: " + method.FullDescription() + " contains no Newobj opcode");
				return null;
			}
			ConstructorInfo constructorInfo = enumerable.First<KeyValuePair<OpCode, object>>().Value as ConstructorInfo;
			if (constructorInfo == null)
			{
				FileLog.Debug("AccessTools.EnumeratorMoveNext: " + method.FullDescription() + " contains no constructor");
				return null;
			}
			Type declaringType = constructorInfo.DeclaringType;
			if (declaringType == null)
			{
				FileLog.Debug("AccessTools.EnumeratorMoveNext: " + method.FullDescription() + " refers to a global type");
				return null;
			}
			return AccessTools.Method(declaringType, "MoveNext", null, null);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
		public static List<string> GetMethodNames(Type type)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.GetMethodNames: type is null");
				return new List<string>();
			}
			return (from m in AccessTools.GetDeclaredMethods(type)
				select m.Name).ToList<string>();
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000F0F7 File Offset: 0x0000D2F7
		public static List<string> GetMethodNames(object instance)
		{
			if (instance == null)
			{
				FileLog.Debug("AccessTools.GetMethodNames: instance is null");
				return new List<string>();
			}
			return AccessTools.GetMethodNames(instance.GetType());
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000F118 File Offset: 0x0000D318
		public static List<string> GetFieldNames(Type type)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.GetFieldNames: type is null");
				return new List<string>();
			}
			return (from f in AccessTools.GetDeclaredFields(type)
				select f.Name).ToList<string>();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000F167 File Offset: 0x0000D367
		public static List<string> GetFieldNames(object instance)
		{
			if (instance == null)
			{
				FileLog.Debug("AccessTools.GetFieldNames: instance is null");
				return new List<string>();
			}
			return AccessTools.GetFieldNames(instance.GetType());
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000F188 File Offset: 0x0000D388
		public static List<string> GetPropertyNames(Type type)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.GetPropertyNames: type is null");
				return new List<string>();
			}
			return (from f in AccessTools.GetDeclaredProperties(type)
				select f.Name).ToList<string>();
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000F1D7 File Offset: 0x0000D3D7
		public static List<string> GetPropertyNames(object instance)
		{
			if (instance == null)
			{
				FileLog.Debug("AccessTools.GetPropertyNames: instance is null");
				return new List<string>();
			}
			return AccessTools.GetPropertyNames(instance.GetType());
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000F1F8 File Offset: 0x0000D3F8
		public static Type GetUnderlyingType(this MemberInfo member)
		{
			MemberTypes memberType = member.MemberType;
			if (memberType <= MemberTypes.Field)
			{
				if (memberType == MemberTypes.Event)
				{
					return ((EventInfo)member).EventHandlerType;
				}
				if (memberType == MemberTypes.Field)
				{
					return ((FieldInfo)member).FieldType;
				}
			}
			else
			{
				if (memberType == MemberTypes.Method)
				{
					return ((MethodInfo)member).ReturnType;
				}
				if (memberType == MemberTypes.Property)
				{
					return ((PropertyInfo)member).PropertyType;
				}
			}
			throw new ArgumentException("Member must be of type EventInfo, FieldInfo, MethodInfo, or PropertyInfo");
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000F269 File Offset: 0x0000D469
		public static bool IsDeclaredMember<T>(this T member) where T : MemberInfo
		{
			return member.DeclaringType == member.ReflectedType;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000F288 File Offset: 0x0000D488
		public static T GetDeclaredMember<T>(this T member) where T : MemberInfo
		{
			if (member.DeclaringType == null || member.IsDeclaredMember<T>())
			{
				return member;
			}
			int metadataToken = member.MetadataToken;
			Type declaringType = member.DeclaringType;
			foreach (MemberInfo memberInfo in ((declaringType != null) ? declaringType.GetMembers(AccessTools.all) : null) ?? new MemberInfo[0])
			{
				if (memberInfo.MetadataToken == metadataToken)
				{
					return (T)((object)memberInfo);
				}
			}
			return member;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000F304 File Offset: 0x0000D504
		public static ConstructorInfo DeclaredConstructor(Type type, Type[] parameters = null, bool searchForStatic = false)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.DeclaredConstructor: type is null");
				return null;
			}
			if (parameters == null)
			{
				parameters = new Type[0];
			}
			BindingFlags bindingFlags = (searchForStatic ? (AccessTools.allDeclared & ~BindingFlags.Instance) : (AccessTools.allDeclared & ~BindingFlags.Static));
			return type.GetConstructor(bindingFlags, null, parameters, new ParameterModifier[0]);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000F350 File Offset: 0x0000D550
		public static ConstructorInfo Constructor(Type type, Type[] parameters = null, bool searchForStatic = false)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.ConstructorInfo: type is null");
				return null;
			}
			if (parameters == null)
			{
				parameters = new Type[0];
			}
			BindingFlags flags = (searchForStatic ? (AccessTools.all & ~BindingFlags.Instance) : (AccessTools.all & ~BindingFlags.Static));
			return AccessTools.FindIncludingBaseTypes<ConstructorInfo>(type, (Type t) => t.GetConstructor(flags, null, parameters, new ParameterModifier[0]));
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000F3BC File Offset: 0x0000D5BC
		public static List<ConstructorInfo> GetDeclaredConstructors(Type type, bool? searchForStatic = null)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.GetDeclaredConstructors: type is null");
				return new List<ConstructorInfo>();
			}
			BindingFlags bindingFlags = AccessTools.allDeclared;
			if (searchForStatic != null)
			{
				bindingFlags = (searchForStatic.Value ? (bindingFlags & ~BindingFlags.Instance) : (bindingFlags & ~BindingFlags.Static));
			}
			return (from method in type.GetConstructors(bindingFlags)
				where method.DeclaringType == type
				select method).ToList<ConstructorInfo>();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000F433 File Offset: 0x0000D633
		public static List<MethodInfo> GetDeclaredMethods(Type type)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.GetDeclaredMethods: type is null");
				return new List<MethodInfo>();
			}
			return type.GetMethods(AccessTools.allDeclared).ToList<MethodInfo>();
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F458 File Offset: 0x0000D658
		public static List<PropertyInfo> GetDeclaredProperties(Type type)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.GetDeclaredProperties: type is null");
				return new List<PropertyInfo>();
			}
			return type.GetProperties(AccessTools.allDeclared).ToList<PropertyInfo>();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000F47D File Offset: 0x0000D67D
		public static List<FieldInfo> GetDeclaredFields(Type type)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.GetDeclaredFields: type is null");
				return new List<FieldInfo>();
			}
			return type.GetFields(AccessTools.allDeclared).ToList<FieldInfo>();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F4A2 File Offset: 0x0000D6A2
		public static Type GetReturnedType(MethodBase methodOrConstructor)
		{
			if (methodOrConstructor == null)
			{
				FileLog.Debug("AccessTools.GetReturnedType: methodOrConstructor is null");
				return null;
			}
			if (methodOrConstructor is ConstructorInfo)
			{
				return typeof(void);
			}
			return ((MethodInfo)methodOrConstructor).ReturnType;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F4D4 File Offset: 0x0000D6D4
		public static Type Inner(Type type, string name)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.Inner: type is null");
				return null;
			}
			if (name == null)
			{
				FileLog.Debug("AccessTools.Inner: name is null");
				return null;
			}
			return AccessTools.FindIncludingBaseTypes<Type>(type, (Type t) => t.GetNestedType(name, AccessTools.all));
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F524 File Offset: 0x0000D724
		public static Type FirstInner(Type type, Func<Type, bool> predicate)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.FirstInner: type is null");
				return null;
			}
			if (predicate == null)
			{
				FileLog.Debug("AccessTools.FirstInner: predicate is null");
				return null;
			}
			return type.GetNestedTypes(AccessTools.all).FirstOrDefault((Type subType) => predicate(subType));
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F580 File Offset: 0x0000D780
		public static MethodInfo FirstMethod(Type type, Func<MethodInfo, bool> predicate)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.FirstMethod: type is null");
				return null;
			}
			if (predicate == null)
			{
				FileLog.Debug("AccessTools.FirstMethod: predicate is null");
				return null;
			}
			return type.GetMethods(AccessTools.allDeclared).FirstOrDefault((MethodInfo method) => predicate(method));
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F5DC File Offset: 0x0000D7DC
		public static ConstructorInfo FirstConstructor(Type type, Func<ConstructorInfo, bool> predicate)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.FirstConstructor: type is null");
				return null;
			}
			if (predicate == null)
			{
				FileLog.Debug("AccessTools.FirstConstructor: predicate is null");
				return null;
			}
			return type.GetConstructors(AccessTools.allDeclared).FirstOrDefault((ConstructorInfo constructor) => predicate(constructor));
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F638 File Offset: 0x0000D838
		public static PropertyInfo FirstProperty(Type type, Func<PropertyInfo, bool> predicate)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.FirstProperty: type is null");
				return null;
			}
			if (predicate == null)
			{
				FileLog.Debug("AccessTools.FirstProperty: predicate is null");
				return null;
			}
			return type.GetProperties(AccessTools.allDeclared).FirstOrDefault((PropertyInfo property) => predicate(property));
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000F691 File Offset: 0x0000D891
		public static Type[] GetTypes(object[] parameters)
		{
			if (parameters == null)
			{
				return new Type[0];
			}
			return parameters.Select(delegate(object p)
			{
				if (p != null)
				{
					return p.GetType();
				}
				return typeof(object);
			}).ToArray<Type>();
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F6C8 File Offset: 0x0000D8C8
		public static object[] ActualParameters(MethodBase method, object[] inputs)
		{
			List<Type> inputTypes = inputs.Select(delegate(object obj)
			{
				if (obj == null)
				{
					return null;
				}
				return obj.GetType();
			}).ToList<Type>();
			return (from p in method.GetParameters()
				select p.ParameterType).Select(delegate(Type pType)
			{
				int num = inputTypes.FindIndex((Type inType) => inType != null && pType.IsAssignableFrom(inType));
				if (num >= 0)
				{
					return inputs[num];
				}
				return AccessTools.GetDefaultValue(pType);
			}).ToArray<object>();
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000F758 File Offset: 0x0000D958
		public static AccessTools.FieldRef<T, F> FieldRefAccess<T, F>(string fieldName)
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			AccessTools.FieldRef<T, F> fieldRef;
			try
			{
				Type typeFromHandle = typeof(T);
				if (typeFromHandle.IsValueType)
				{
					throw new ArgumentException("T (FieldRefAccess instance type) must not be a value type");
				}
				fieldRef = Tools.FieldRefAccess<T, F>(Tools.GetInstanceField(typeFromHandle, fieldName), false);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("FieldRefAccess<{0}, {1}> for {2} caused an exception", typeof(T), typeof(F), fieldName), ex);
			}
			return fieldRef;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000F7D8 File Offset: 0x0000D9D8
		public static ref F FieldRefAccess<T, F>(T instance, string fieldName)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			F ptr;
			try
			{
				Type typeFromHandle = typeof(T);
				if (typeFromHandle.IsValueType)
				{
					throw new ArgumentException("T (FieldRefAccess instance type) must not be a value type");
				}
				ptr = Tools.FieldRefAccess<T, F>(Tools.GetInstanceField(typeFromHandle, fieldName), false)(instance);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("FieldRefAccess<{0}, {1}> for {2}, {3} caused an exception", new object[]
				{
					typeof(T),
					typeof(F),
					instance,
					fieldName
				}), ex);
			}
			return ref ptr;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000F88C File Offset: 0x0000DA8C
		public static AccessTools.FieldRef<object, F> FieldRefAccess<F>(Type type, string fieldName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			AccessTools.FieldRef<object, F> fieldRef;
			try
			{
				FieldInfo fieldInfo = AccessTools.Field(type, fieldName);
				if (fieldInfo == null)
				{
					throw new MissingFieldException(type.Name, fieldName);
				}
				if (!fieldInfo.IsStatic)
				{
					Type declaringType = fieldInfo.DeclaringType;
					if (declaringType != null && declaringType.IsValueType)
					{
						throw new ArgumentException("Either FieldDeclaringType must be a class or field must be static");
					}
				}
				fieldRef = Tools.FieldRefAccess<object, F>(fieldInfo, true);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("FieldRefAccess<{0}> for {1}, {2} caused an exception", typeof(F), type, fieldName), ex);
			}
			return fieldRef;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000F92C File Offset: 0x0000DB2C
		public static AccessTools.FieldRef<object, F> FieldRefAccess<F>(string typeColonName)
		{
			Tools.TypeAndName typeAndName = Tools.TypColonName(typeColonName);
			return AccessTools.FieldRefAccess<F>(typeAndName.type, typeAndName.name);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000F954 File Offset: 0x0000DB54
		public static AccessTools.FieldRef<T, F> FieldRefAccess<T, F>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			AccessTools.FieldRef<T, F> fieldRef;
			try
			{
				Type typeFromHandle = typeof(T);
				if (typeFromHandle.IsValueType)
				{
					throw new ArgumentException("T (FieldRefAccess instance type) must not be a value type");
				}
				bool flag = false;
				if (!fieldInfo.IsStatic)
				{
					Type declaringType = fieldInfo.DeclaringType;
					if (declaringType != null)
					{
						if (declaringType.IsValueType)
						{
							throw new ArgumentException("Either FieldDeclaringType must be a class or field must be static");
						}
						flag = Tools.FieldRefNeedsClasscast(typeFromHandle, declaringType);
					}
				}
				fieldRef = Tools.FieldRefAccess<T, F>(fieldInfo, flag);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("FieldRefAccess<{0}, {1}> for {2} caused an exception", typeof(T), typeof(F), fieldInfo), ex);
			}
			return fieldRef;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000FA04 File Offset: 0x0000DC04
		public static ref F FieldRefAccess<T, F>(T instance, FieldInfo fieldInfo)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			F ptr;
			try
			{
				Type typeFromHandle = typeof(T);
				if (typeFromHandle.IsValueType)
				{
					throw new ArgumentException("T (FieldRefAccess instance type) must not be a value type");
				}
				if (fieldInfo.IsStatic)
				{
					throw new ArgumentException("Field must not be static");
				}
				bool flag = false;
				Type declaringType = fieldInfo.DeclaringType;
				if (declaringType != null)
				{
					if (declaringType.IsValueType)
					{
						throw new ArgumentException("FieldDeclaringType must be a class");
					}
					flag = Tools.FieldRefNeedsClasscast(typeFromHandle, declaringType);
				}
				ptr = Tools.FieldRefAccess<T, F>(fieldInfo, flag)(instance);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("FieldRefAccess<{0}, {1}> for {2}, {3} caused an exception", new object[]
				{
					typeof(T),
					typeof(F),
					instance,
					fieldInfo
				}), ex);
			}
			return ref ptr;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
		public static AccessTools.StructFieldRef<T, F> StructFieldRefAccess<T, F>(string fieldName) where T : struct
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			AccessTools.StructFieldRef<T, F> structFieldRef;
			try
			{
				structFieldRef = Tools.StructFieldRefAccess<T, F>(Tools.GetInstanceField(typeof(T), fieldName));
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StructFieldRefAccess<{0}, {1}> for {2} caused an exception", typeof(T), typeof(F), fieldName), ex);
			}
			return structFieldRef;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000FB5C File Offset: 0x0000DD5C
		public static ref F StructFieldRefAccess<T, F>(ref T instance, string fieldName) where T : struct
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			F ptr;
			try
			{
				ptr = Tools.StructFieldRefAccess<T, F>(Tools.GetInstanceField(typeof(T), fieldName))(ref instance);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StructFieldRefAccess<{0}, {1}> for {2}, {3} caused an exception", new object[]
				{
					typeof(T),
					typeof(F),
					instance,
					fieldName
				}), ex);
			}
			return ref ptr;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000FBEC File Offset: 0x0000DDEC
		public static AccessTools.StructFieldRef<T, F> StructFieldRefAccess<T, F>(FieldInfo fieldInfo) where T : struct
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			AccessTools.StructFieldRef<T, F> structFieldRef;
			try
			{
				Tools.ValidateStructField<T, F>(fieldInfo);
				structFieldRef = Tools.StructFieldRefAccess<T, F>(fieldInfo);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StructFieldRefAccess<{0}, {1}> for {2} caused an exception", typeof(T), typeof(F), fieldInfo), ex);
			}
			return structFieldRef;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000FC50 File Offset: 0x0000DE50
		public static ref F StructFieldRefAccess<T, F>(ref T instance, FieldInfo fieldInfo) where T : struct
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			F ptr;
			try
			{
				Tools.ValidateStructField<T, F>(fieldInfo);
				ptr = Tools.StructFieldRefAccess<T, F>(fieldInfo)(ref instance);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StructFieldRefAccess<{0}, {1}> for {2}, {3} caused an exception", new object[]
				{
					typeof(T),
					typeof(F),
					instance,
					fieldInfo
				}), ex);
			}
			return ref ptr;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000FCD8 File Offset: 0x0000DED8
		public static ref F StaticFieldRefAccess<T, F>(string fieldName)
		{
			return AccessTools.StaticFieldRefAccess<F>(typeof(T), fieldName);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000FCEC File Offset: 0x0000DEEC
		public static ref F StaticFieldRefAccess<F>(Type type, string fieldName)
		{
			F ptr;
			try
			{
				FieldInfo fieldInfo = AccessTools.Field(type, fieldName);
				if (fieldInfo == null)
				{
					throw new MissingFieldException(type.Name, fieldName);
				}
				ptr = Tools.StaticFieldRefAccess<F>(fieldInfo)();
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StaticFieldRefAccess<{0}> for {1}, {2} caused an exception", typeof(F), type, fieldName), ex);
			}
			return ref ptr;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000FD4C File Offset: 0x0000DF4C
		public static ref F StaticFieldRefAccess<F>(string typeColonName)
		{
			Tools.TypeAndName typeAndName = Tools.TypColonName(typeColonName);
			return AccessTools.StaticFieldRefAccess<F>(typeAndName.type, typeAndName.name);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000FD74 File Offset: 0x0000DF74
		public static ref F StaticFieldRefAccess<T, F>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			F ptr;
			try
			{
				ptr = Tools.StaticFieldRefAccess<F>(fieldInfo)();
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StaticFieldRefAccess<{0}, {1}> for {2} caused an exception", typeof(T), typeof(F), fieldInfo), ex);
			}
			return ref ptr;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
		public static AccessTools.FieldRef<F> StaticFieldRefAccess<F>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			AccessTools.FieldRef<F> fieldRef;
			try
			{
				fieldRef = Tools.StaticFieldRefAccess<F>(fieldInfo);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StaticFieldRefAccess<{0}> for {1} caused an exception", typeof(F), fieldInfo), ex);
			}
			return fieldRef;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000FE2C File Offset: 0x0000E02C
		public static DelegateType MethodDelegate<DelegateType>(MethodInfo method, object instance = null, bool virtualCall = true) where DelegateType : Delegate
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			Type typeFromHandle = typeof(DelegateType);
			if (method.IsStatic)
			{
				return (DelegateType)((object)Delegate.CreateDelegate(typeFromHandle, method));
			}
			Type type = method.DeclaringType;
			if (type != null && type.IsInterface && !virtualCall)
			{
				throw new ArgumentException("Interface methods must be called virtually");
			}
			if (instance == null)
			{
				ParameterInfo[] parameters = typeFromHandle.GetMethod("Invoke").GetParameters();
				if (parameters.Length == 0)
				{
					Delegate.CreateDelegate(typeof(DelegateType), method);
					throw new ArgumentException("Invalid delegate type");
				}
				Type parameterType = parameters[0].ParameterType;
				if (type != null && type.IsInterface && parameterType.IsValueType)
				{
					InterfaceMapping interfaceMap = parameterType.GetInterfaceMap(type);
					method = interfaceMap.TargetMethods[Array.IndexOf<MethodInfo>(interfaceMap.InterfaceMethods, method)];
					type = parameterType;
				}
				if (type != null && virtualCall)
				{
					if (type.IsInterface)
					{
						return (DelegateType)((object)Delegate.CreateDelegate(typeFromHandle, method));
					}
					if (parameterType.IsInterface)
					{
						InterfaceMapping interfaceMap2 = type.GetInterfaceMap(parameterType);
						MethodInfo methodInfo = interfaceMap2.InterfaceMethods[Array.IndexOf<MethodInfo>(interfaceMap2.TargetMethods, method)];
						return (DelegateType)((object)Delegate.CreateDelegate(typeFromHandle, methodInfo));
					}
					if (!type.IsValueType)
					{
						return (DelegateType)((object)Delegate.CreateDelegate(typeFromHandle, method.GetBaseDefinition()));
					}
				}
				ParameterInfo[] parameters2 = method.GetParameters();
				int num = parameters2.Length;
				Type[] array = new Type[num + 1];
				array[0] = type;
				for (int i = 0; i < num; i++)
				{
					array[i + 1] = parameters2[i].ParameterType;
				}
				DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("OpenInstanceDelegate_" + method.Name, method.ReturnType, array)
				{
					OwnerType = type
				};
				ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
				if (type != null && type.IsValueType)
				{
					ilgenerator.Emit(OpCodes.Ldarga_S, 0);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Ldarg_0);
				}
				for (int j = 1; j < array.Length; j++)
				{
					ilgenerator.Emit(OpCodes.Ldarg, j);
				}
				ilgenerator.Emit(OpCodes.Call, method);
				ilgenerator.Emit(OpCodes.Ret);
				return (DelegateType)((object)dynamicMethodDefinition.Generate().CreateDelegate(typeFromHandle));
			}
			else
			{
				if (virtualCall)
				{
					return (DelegateType)((object)Delegate.CreateDelegate(typeFromHandle, instance, method.GetBaseDefinition()));
				}
				if (type != null && !type.IsInstanceOfType(instance))
				{
					Delegate.CreateDelegate(typeof(DelegateType), instance, method);
					throw new ArgumentException("Invalid delegate type");
				}
				if (AccessTools.IsMonoRuntime)
				{
					DynamicMethodDefinition dynamicMethodDefinition2 = new DynamicMethodDefinition("LdftnDelegate_" + method.Name, typeFromHandle, new Type[] { typeof(object) })
					{
						OwnerType = typeFromHandle
					};
					ILGenerator ilgenerator2 = dynamicMethodDefinition2.GetILGenerator();
					ilgenerator2.Emit(OpCodes.Ldarg_0);
					ilgenerator2.Emit(OpCodes.Ldftn, method);
					ilgenerator2.Emit(OpCodes.Newobj, typeFromHandle.GetConstructor(new Type[]
					{
						typeof(object),
						typeof(IntPtr)
					}));
					ilgenerator2.Emit(OpCodes.Ret);
					return (DelegateType)((object)dynamicMethodDefinition2.Generate().Invoke(null, new object[] { instance }));
				}
				return (DelegateType)((object)Activator.CreateInstance(typeFromHandle, new object[]
				{
					instance,
					method.MethodHandle.GetFunctionPointer()
				}));
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00010185 File Offset: 0x0000E385
		public static DelegateType MethodDelegate<DelegateType>(string typeColonName, object instance = null, bool virtualCall = true) where DelegateType : Delegate
		{
			return AccessTools.MethodDelegate<DelegateType>(AccessTools.DeclaredMethod(typeColonName, null, null), instance, virtualCall);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00010198 File Offset: 0x0000E398
		public static DelegateType HarmonyDelegate<DelegateType>(object instance = null) where DelegateType : Delegate
		{
			HarmonyMethod mergedFromType = HarmonyMethodExtensions.GetMergedFromType(typeof(DelegateType));
			MethodType? methodType = mergedFromType.methodType;
			if (methodType == null)
			{
				mergedFromType.methodType = new MethodType?(MethodType.Normal);
			}
			MethodInfo methodInfo = mergedFromType.GetOriginalMethod() as MethodInfo;
			if (methodInfo == null)
			{
				throw new NullReferenceException(string.Format("Delegate {0} has no defined original method", typeof(DelegateType)));
			}
			return AccessTools.MethodDelegate<DelegateType>(methodInfo, instance, !mergedFromType.nonVirtualDelegate);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00010208 File Offset: 0x0000E408
		public static MethodBase GetOutsideCaller()
		{
			StackFrame[] frames = new StackTrace(true).GetFrames();
			for (int i = 0; i < frames.Length; i++)
			{
				MethodBase method = frames[i].GetMethod();
				Type declaringType = method.DeclaringType;
				if (((declaringType != null) ? declaringType.Namespace : null) != typeof(Harmony).Namespace)
				{
					return method;
				}
			}
			throw new Exception("Unexpected end of stack trace");
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0001026C File Offset: 0x0000E46C
		public static void RethrowException(Exception exception)
		{
			ExceptionDispatchInfo.Capture(exception).Throw();
			throw exception;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0001027A File Offset: 0x0000E47A
		public static bool IsMonoRuntime { get; } = Type.GetType("Mono.Runtime") != null;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00010281 File Offset: 0x0000E481
		public static bool IsNetFrameworkRuntime { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00010288 File Offset: 0x0000E488
		public static bool IsNetCoreRuntime { get; }

		// Token: 0x060002FF RID: 767 RVA: 0x00010290 File Offset: 0x0000E490
		public static void ThrowMissingMemberException(Type type, params string[] names)
		{
			string text = string.Join(",", AccessTools.GetFieldNames(type).ToArray());
			string text2 = string.Join(",", AccessTools.GetPropertyNames(type).ToArray());
			throw new MissingMemberException(string.Concat(new string[]
			{
				string.Join(",", names),
				"; available fields: ",
				text,
				"; available properties: ",
				text2
			}));
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000102FF File Offset: 0x0000E4FF
		public static object GetDefaultValue(Type type)
		{
			if (type == null)
			{
				FileLog.Debug("AccessTools.GetDefaultValue: type is null");
				return null;
			}
			if (type == typeof(void))
			{
				return null;
			}
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00010334 File Offset: 0x0000E534
		public static object CreateInstance(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, new Type[0], null);
			if (constructor != null)
			{
				return constructor.Invoke(null);
			}
			return FormatterServices.GetUninitializedObject(type);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00010374 File Offset: 0x0000E574
		public static T CreateInstance<T>()
		{
			object obj = AccessTools.CreateInstance(typeof(T));
			if (obj is T)
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000103AB File Offset: 0x0000E5AB
		public static T MakeDeepCopy<T>(object source) where T : class
		{
			return AccessTools.MakeDeepCopy(source, typeof(T), null, "") as T;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000103CD File Offset: 0x0000E5CD
		public static void MakeDeepCopy<T>(object source, out T result, Func<string, Traverse, Traverse, object> processor = null, string pathRoot = "")
		{
			result = (T)((object)AccessTools.MakeDeepCopy(source, typeof(T), processor, pathRoot));
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000103EC File Offset: 0x0000E5EC
		public static object MakeDeepCopy(object source, Type resultType, Func<string, Traverse, Traverse, object> processor = null, string pathRoot = "")
		{
			if (source == null || resultType == null)
			{
				return null;
			}
			resultType = Nullable.GetUnderlyingType(resultType) ?? resultType;
			Type type = source.GetType();
			if (type.IsPrimitive)
			{
				return source;
			}
			if (type.IsEnum)
			{
				return Enum.ToObject(resultType, (int)source);
			}
			if (type.IsGenericType && resultType.IsGenericType)
			{
				AccessTools.addHandlerCacheLock.EnterUpgradeableReadLock();
				try
				{
					FastInvokeHandler handler;
					if (!AccessTools.addHandlerCache.TryGetValue(resultType, out handler))
					{
						MethodInfo methodInfo = AccessTools.FirstMethod(resultType, (MethodInfo m) => m.Name == "Add" && m.GetParameters().Length == 1);
						if (methodInfo != null)
						{
							handler = MethodInvoker.GetHandler(methodInfo, false);
						}
						AccessTools.addHandlerCacheLock.EnterWriteLock();
						try
						{
							AccessTools.addHandlerCache[resultType] = handler;
						}
						finally
						{
							AccessTools.addHandlerCacheLock.ExitWriteLock();
						}
					}
					if (handler != null)
					{
						object obj = Activator.CreateInstance(resultType);
						Type type2 = resultType.GetGenericArguments()[0];
						int num = 0;
						foreach (object obj2 in (source as IEnumerable))
						{
							string text = num++.ToString();
							string text2 = ((pathRoot.Length > 0) ? (pathRoot + "." + text) : text);
							object obj3 = AccessTools.MakeDeepCopy(obj2, type2, processor, text2);
							handler(obj, new object[] { obj3 });
						}
						return obj;
					}
				}
				finally
				{
					AccessTools.addHandlerCacheLock.ExitUpgradeableReadLock();
				}
			}
			if (type.IsArray && resultType.IsArray)
			{
				Type elementType = resultType.GetElementType();
				int length = ((Array)source).Length;
				object[] array = Activator.CreateInstance(resultType, new object[] { length }) as object[];
				object[] array2 = source as object[];
				for (int i = 0; i < length; i++)
				{
					string text3 = i.ToString();
					string text4 = ((pathRoot.Length > 0) ? (pathRoot + "." + text3) : text3);
					array[i] = AccessTools.MakeDeepCopy(array2[i], elementType, processor, text4);
				}
				return array;
			}
			string @namespace = type.Namespace;
			if (@namespace == "System" || (@namespace != null && @namespace.StartsWith("System.")))
			{
				return source;
			}
			object obj4 = AccessTools.CreateInstance((resultType == typeof(object)) ? type : resultType);
			Traverse.IterateFields(source, obj4, delegate(string name, Traverse src, Traverse dst)
			{
				string text5 = ((pathRoot.Length > 0) ? (pathRoot + "." + name) : name);
				object obj5 = ((processor != null) ? processor(text5, src, dst) : src.GetValue());
				dst.SetValue(AccessTools.MakeDeepCopy(obj5, dst.GetValueType(), processor, text5));
			});
			return obj4;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000106E8 File Offset: 0x0000E8E8
		public static bool IsStruct(Type type)
		{
			return !(type == null) && (type.IsValueType && !AccessTools.IsValue(type)) && !AccessTools.IsVoid(type);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00010710 File Offset: 0x0000E910
		public static bool IsClass(Type type)
		{
			return !(type == null) && !type.IsValueType;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00010726 File Offset: 0x0000E926
		public static bool IsValue(Type type)
		{
			return !(type == null) && (type.IsPrimitive || type.IsEnum);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00010744 File Offset: 0x0000E944
		public static bool IsInteger(Type type)
		{
			if (type == null)
			{
				return false;
			}
			TypeCode typeCode = Type.GetTypeCode(type);
			return typeCode - TypeCode.SByte <= 7;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0001076C File Offset: 0x0000E96C
		public static bool IsFloatingPoint(Type type)
		{
			if (type == null)
			{
				return false;
			}
			TypeCode typeCode = Type.GetTypeCode(type);
			return typeCode - TypeCode.Single <= 2;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00010795 File Offset: 0x0000E995
		public static bool IsNumber(Type type)
		{
			return AccessTools.IsInteger(type) || AccessTools.IsFloatingPoint(type);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x000107A7 File Offset: 0x0000E9A7
		public static bool IsVoid(Type type)
		{
			return type == typeof(void);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000107B9 File Offset: 0x0000E9B9
		public static bool IsOfNullableType<T>(T instance)
		{
			return Nullable.GetUnderlyingType(typeof(T)) != null;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x000107D0 File Offset: 0x0000E9D0
		public static bool IsStatic(MemberInfo member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			MemberTypes memberType = member.MemberType;
			if (memberType <= MemberTypes.Method)
			{
				switch (memberType)
				{
				case MemberTypes.Constructor:
					break;
				case MemberTypes.Event:
					return AccessTools.IsStatic((EventInfo)member);
				case MemberTypes.Constructor | MemberTypes.Event:
					goto IL_87;
				case MemberTypes.Field:
					return ((FieldInfo)member).IsStatic;
				default:
					if (memberType != MemberTypes.Method)
					{
						goto IL_87;
					}
					break;
				}
				return ((MethodBase)member).IsStatic;
			}
			if (memberType == MemberTypes.Property)
			{
				return AccessTools.IsStatic((PropertyInfo)member);
			}
			if (memberType == MemberTypes.TypeInfo || memberType == MemberTypes.NestedType)
			{
				return AccessTools.IsStatic((Type)member);
			}
			IL_87:
			throw new ArgumentException(string.Format("Unknown member type: {0}", member.MemberType));
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0001087E File Offset: 0x0000EA7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsStatic(Type type)
		{
			return type != null && type.IsAbstract && type.IsSealed;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00010895 File Offset: 0x0000EA95
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsStatic(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}
			return propertyInfo.GetAccessors(true)[0].IsStatic;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000108B3 File Offset: 0x0000EAB3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsStatic(EventInfo eventInfo)
		{
			if (eventInfo == null)
			{
				throw new ArgumentNullException("eventInfo");
			}
			return eventInfo.GetAddMethod(true).IsStatic;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000108D0 File Offset: 0x0000EAD0
		public static int CombinedHashCode(IEnumerable<object> objects)
		{
			int num = 352654597;
			int num2 = num;
			int num3 = 0;
			foreach (object obj in objects)
			{
				if (num3 % 2 == 0)
				{
					num = ((num << 5) + num + (num >> 27)) ^ obj.GetHashCode();
				}
				else
				{
					num2 = ((num2 << 5) + num2 + (num2 >> 27)) ^ obj.GetHashCode();
				}
				num3++;
			}
			return num + num2 * 1566083941;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00010958 File Offset: 0x0000EB58
		// Note: this type is marked as 'beforefieldinit'.
		static AccessTools()
		{
			Type type = Type.GetType("System.Runtime.InteropServices.RuntimeInformation", false);
			AccessTools.IsNetFrameworkRuntime = ((type != null) ? type.GetProperty("FrameworkDescription").GetValue(null, null).ToString()
				.StartsWith(".NET Framework") : (!AccessTools.IsMonoRuntime));
			Type type2 = Type.GetType("System.Runtime.InteropServices.RuntimeInformation", false);
			AccessTools.IsNetCoreRuntime = type2 != null && type2.GetProperty("FrameworkDescription").GetValue(null, null).ToString()
				.StartsWith(".NET Core");
			AccessTools.addHandlerCache = new Dictionary<Type, FastInvokeHandler>();
			AccessTools.addHandlerCacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		}

		// Token: 0x040001B9 RID: 441
		public static readonly BindingFlags all = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

		// Token: 0x040001BA RID: 442
		public static readonly BindingFlags allDeclared = AccessTools.all | BindingFlags.DeclaredOnly;

		// Token: 0x040001BE RID: 446
		private static readonly Dictionary<Type, FastInvokeHandler> addHandlerCache;

		// Token: 0x040001BF RID: 447
		private static readonly ReaderWriterLockSlim addHandlerCacheLock;

		// Token: 0x0200008B RID: 139
		// (Invoke) Token: 0x06000315 RID: 789
		public unsafe delegate F* FieldRef<in T, F>(T instance = default(T));

		// Token: 0x0200008C RID: 140
		// (Invoke) Token: 0x06000319 RID: 793
		public unsafe delegate F* StructFieldRef<T, F>(ref T instance) where T : struct;

		// Token: 0x0200008D RID: 141
		// (Invoke) Token: 0x0600031D RID: 797
		public unsafe delegate F* FieldRef<F>();
	}
}
