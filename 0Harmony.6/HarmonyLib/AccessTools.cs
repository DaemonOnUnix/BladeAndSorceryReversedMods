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
	// Token: 0x02000087 RID: 135
	public static class AccessTools
	{
		// Token: 0x06000298 RID: 664 RVA: 0x0000D614 File Offset: 0x0000B814
		public static Type TypeByName(string name)
		{
			Type type = Type.GetType(name, false);
			IEnumerable<Assembly> enumerable = from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.FullName.StartsWith("Microsoft.VisualStudio")
				select a;
			if (type == null)
			{
				type = enumerable.SelectMany((Assembly a) => AccessTools.GetTypesFromAssembly(a)).FirstOrDefault((Type t) => t.FullName == name);
			}
			if (type == null)
			{
				type = enumerable.SelectMany((Assembly a) => AccessTools.GetTypesFromAssembly(a)).FirstOrDefault((Type t) => t.Name == name);
			}
			if (type == null && Harmony.DEBUG)
			{
				FileLog.Log("AccessTools.TypeByName: Could not find type named " + name);
			}
			return type;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000D700 File Offset: 0x0000B900
		public static Type[] GetTypesFromAssembly(Assembly assembly)
		{
			Type[] array;
			try
			{
				array = assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log(string.Format("AccessTools.GetTypesFromAssembly: assembly {0} => {1}", assembly, ex));
				}
				array = ex.Types.Where((Type type) => type != null).ToArray<Type>();
			}
			return array;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000D774 File Offset: 0x0000B974
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

		// Token: 0x0600029B RID: 667 RVA: 0x0000D7A8 File Offset: 0x0000B9A8
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

		// Token: 0x0600029C RID: 668 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		public static FieldInfo DeclaredField(Type type, string name)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredField: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredField: name is null");
				}
				return null;
			}
			FieldInfo field = type.GetField(name, AccessTools.allDeclared);
			if (field == null && Harmony.DEBUG)
			{
				FileLog.Log(string.Format("AccessTools.DeclaredField: Could not find field for type {0} and name {1}", type, name));
			}
			return field;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000D854 File Offset: 0x0000BA54
		public static FieldInfo Field(Type type, string name)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Field: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Field: name is null");
				}
				return null;
			}
			FieldInfo fieldInfo = AccessTools.FindIncludingBaseTypes<FieldInfo>(type, (Type t) => t.GetField(name, AccessTools.all));
			if (fieldInfo == null && Harmony.DEBUG)
			{
				FileLog.Log(string.Format("AccessTools.Field: Could not find field for type {0} and name {1}", type, name));
			}
			return fieldInfo;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000D8D4 File Offset: 0x0000BAD4
		public static FieldInfo DeclaredField(Type type, int idx)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredField: type is null");
				}
				return null;
			}
			FieldInfo fieldInfo = AccessTools.GetDeclaredFields(type).ElementAtOrDefault(idx);
			if (fieldInfo == null && Harmony.DEBUG)
			{
				FileLog.Log(string.Format("AccessTools.DeclaredField: Could not find field for type {0} and idx {1}", type, idx));
			}
			return fieldInfo;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000D924 File Offset: 0x0000BB24
		public static PropertyInfo DeclaredProperty(Type type, string name)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredProperty: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredProperty: name is null");
				}
				return null;
			}
			PropertyInfo property = type.GetProperty(name, AccessTools.allDeclared);
			if (property == null && Harmony.DEBUG)
			{
				FileLog.Log(string.Format("AccessTools.DeclaredProperty: Could not find property for type {0} and name {1}", type, name));
			}
			return property;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000D984 File Offset: 0x0000BB84
		public static MethodInfo DeclaredPropertyGetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000D999 File Offset: 0x0000BB99
		public static MethodInfo DeclaredPropertySetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.DeclaredProperty(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000D9B0 File Offset: 0x0000BBB0
		public static PropertyInfo Property(Type type, string name)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Property: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Property: name is null");
				}
				return null;
			}
			PropertyInfo propertyInfo = AccessTools.FindIncludingBaseTypes<PropertyInfo>(type, (Type t) => t.GetProperty(name, AccessTools.all));
			if (propertyInfo == null && Harmony.DEBUG)
			{
				FileLog.Log(string.Format("AccessTools.Property: Could not find property for type {0} and name {1}", type, name));
			}
			return propertyInfo;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000DA2D File Offset: 0x0000BC2D
		public static MethodInfo PropertyGetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.Property(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000DA42 File Offset: 0x0000BC42
		public static MethodInfo PropertySetter(Type type, string name)
		{
			PropertyInfo propertyInfo = AccessTools.Property(type, name);
			if (propertyInfo == null)
			{
				return null;
			}
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000DA58 File Offset: 0x0000BC58
		public static MethodInfo DeclaredMethod(Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredMethod: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredMethod: name is null");
				}
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
				if (Harmony.DEBUG)
				{
					FileLog.Log(string.Format("AccessTools.DeclaredMethod: Could not find method for type {0} and name {1} and parameters {2}", type, name, (parameters != null) ? parameters.Description() : null));
				}
				return null;
			}
			if (generics != null)
			{
				methodInfo = methodInfo.MakeGenericMethod(generics);
			}
			return methodInfo;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000DAF0 File Offset: 0x0000BCF0
		public static MethodInfo Method(Type type, string name, Type[] parameters = null, Type[] generics = null)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Method: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Method: name is null");
				}
				return null;
			}
			ParameterModifier[] modifiers = new ParameterModifier[0];
			MethodInfo methodInfo;
			if (parameters == null)
			{
				try
				{
					methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all));
					goto IL_B2;
				}
				catch (AmbiguousMatchException ex)
				{
					methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all, null, new Type[0], modifiers));
					if (methodInfo == null)
					{
						throw new AmbiguousMatchException(string.Format("Ambiguous match in Harmony patch for {0}:{1}", type, name), ex);
					}
					goto IL_B2;
				}
			}
			methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Type t) => t.GetMethod(name, AccessTools.all, null, parameters, modifiers));
			IL_B2:
			if (methodInfo == null)
			{
				if (Harmony.DEBUG)
				{
					string text = "AccessTools.Method: Could not find method for type {0} and name {1} and parameters {2}";
					object name2 = name;
					Type[] parameters2 = parameters;
					FileLog.Log(string.Format(text, type, name2, (parameters2 != null) ? parameters2.Description() : null));
				}
				return null;
			}
			if (generics != null)
			{
				methodInfo = methodInfo.MakeGenericMethod(generics);
			}
			return methodInfo;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000DC00 File Offset: 0x0000BE00
		public static MethodInfo Method(string typeColonMethodname, Type[] parameters = null, Type[] generics = null)
		{
			if (typeColonMethodname == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Method: typeColonMethodname is null");
				}
				return null;
			}
			string[] array = typeColonMethodname.Split(new char[] { ':' });
			if (array.Length != 2)
			{
				throw new ArgumentException("Method must be specified as 'Namespace.Type1.Type2:MethodName", "typeColonMethodname");
			}
			return AccessTools.DeclaredMethod(AccessTools.TypeByName(array[0]), array[1], parameters, generics);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000DC60 File Offset: 0x0000BE60
		public static List<string> GetMethodNames(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetMethodNames: type is null");
				}
				return new List<string>();
			}
			return (from m in AccessTools.GetDeclaredMethods(type)
				select m.Name).ToList<string>();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000DCB6 File Offset: 0x0000BEB6
		public static List<string> GetMethodNames(object instance)
		{
			if (instance == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetMethodNames: instance is null");
				}
				return new List<string>();
			}
			return AccessTools.GetMethodNames(instance.GetType());
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000DCE0 File Offset: 0x0000BEE0
		public static List<string> GetFieldNames(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetFieldNames: type is null");
				}
				return new List<string>();
			}
			return (from f in AccessTools.GetDeclaredFields(type)
				select f.Name).ToList<string>();
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000DD36 File Offset: 0x0000BF36
		public static List<string> GetFieldNames(object instance)
		{
			if (instance == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetFieldNames: instance is null");
				}
				return new List<string>();
			}
			return AccessTools.GetFieldNames(instance.GetType());
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000DD60 File Offset: 0x0000BF60
		public static List<string> GetPropertyNames(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetPropertyNames: type is null");
				}
				return new List<string>();
			}
			return (from f in AccessTools.GetDeclaredProperties(type)
				select f.Name).ToList<string>();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000DDB6 File Offset: 0x0000BFB6
		public static List<string> GetPropertyNames(object instance)
		{
			if (instance == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetPropertyNames: instance is null");
				}
				return new List<string>();
			}
			return AccessTools.GetPropertyNames(instance.GetType());
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000DDE0 File Offset: 0x0000BFE0
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

		// Token: 0x060002AF RID: 687 RVA: 0x0000DE51 File Offset: 0x0000C051
		public static bool IsDeclaredMember<T>(this T member) where T : MemberInfo
		{
			return member.DeclaringType == member.ReflectedType;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000DE70 File Offset: 0x0000C070
		public static T GetDeclaredMember<T>(this T member) where T : MemberInfo
		{
			if (member.DeclaringType == null || member.IsDeclaredMember<T>())
			{
				return member;
			}
			int metadataToken = member.MetadataToken;
			foreach (MemberInfo memberInfo in member.DeclaringType.GetMembers(AccessTools.all))
			{
				if (memberInfo.MetadataToken == metadataToken)
				{
					return (T)((object)memberInfo);
				}
			}
			return member;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000DEDC File Offset: 0x0000C0DC
		public static ConstructorInfo DeclaredConstructor(Type type, Type[] parameters = null, bool searchForStatic = false)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.DeclaredConstructor: type is null");
				}
				return null;
			}
			if (parameters == null)
			{
				parameters = new Type[0];
			}
			BindingFlags bindingFlags = (searchForStatic ? (AccessTools.allDeclared & ~BindingFlags.Instance) : (AccessTools.allDeclared & ~BindingFlags.Static));
			return type.GetConstructor(bindingFlags, null, parameters, new ParameterModifier[0]);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000DF30 File Offset: 0x0000C130
		public static ConstructorInfo Constructor(Type type, Type[] parameters = null, bool searchForStatic = false)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.ConstructorInfo: type is null");
				}
				return null;
			}
			if (parameters == null)
			{
				parameters = new Type[0];
			}
			BindingFlags flags = (searchForStatic ? (AccessTools.all & ~BindingFlags.Instance) : (AccessTools.all & ~BindingFlags.Static));
			return AccessTools.FindIncludingBaseTypes<ConstructorInfo>(type, (Type t) => t.GetConstructor(flags, null, parameters, new ParameterModifier[0]));
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000DFA4 File Offset: 0x0000C1A4
		public static List<ConstructorInfo> GetDeclaredConstructors(Type type, bool? searchForStatic = null)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetDeclaredConstructors: type is null");
				}
				return null;
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

		// Token: 0x060002B4 RID: 692 RVA: 0x0000E01E File Offset: 0x0000C21E
		public static List<MethodInfo> GetDeclaredMethods(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetDeclaredMethods: type is null");
				}
				return null;
			}
			return type.GetMethods(AccessTools.allDeclared).ToList<MethodInfo>();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000E046 File Offset: 0x0000C246
		public static List<PropertyInfo> GetDeclaredProperties(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetDeclaredProperties: type is null");
				}
				return null;
			}
			return type.GetProperties(AccessTools.allDeclared).ToList<PropertyInfo>();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000E06E File Offset: 0x0000C26E
		public static List<FieldInfo> GetDeclaredFields(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetDeclaredFields: type is null");
				}
				return null;
			}
			return type.GetFields(AccessTools.allDeclared).ToList<FieldInfo>();
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000E096 File Offset: 0x0000C296
		public static Type GetReturnedType(MethodBase methodOrConstructor)
		{
			if (methodOrConstructor == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetReturnedType: methodOrConstructor is null");
				}
				return null;
			}
			if (methodOrConstructor is ConstructorInfo)
			{
				return typeof(void);
			}
			return ((MethodInfo)methodOrConstructor).ReturnType;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000E0CC File Offset: 0x0000C2CC
		public static Type Inner(Type type, string name)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Inner: type is null");
				}
				return null;
			}
			if (name == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.Inner: name is null");
				}
				return null;
			}
			return AccessTools.FindIncludingBaseTypes<Type>(type, (Type t) => t.GetNestedType(name, AccessTools.all));
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000E12C File Offset: 0x0000C32C
		public static Type FirstInner(Type type, Func<Type, bool> predicate)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstInner: type is null");
				}
				return null;
			}
			if (predicate == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstInner: predicate is null");
				}
				return null;
			}
			return type.GetNestedTypes(AccessTools.all).FirstOrDefault((Type subType) => predicate(subType));
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000E194 File Offset: 0x0000C394
		public static MethodInfo FirstMethod(Type type, Func<MethodInfo, bool> predicate)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstMethod: type is null");
				}
				return null;
			}
			if (predicate == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstMethod: predicate is null");
				}
				return null;
			}
			return type.GetMethods(AccessTools.allDeclared).FirstOrDefault((MethodInfo method) => predicate(method));
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000E1FC File Offset: 0x0000C3FC
		public static ConstructorInfo FirstConstructor(Type type, Func<ConstructorInfo, bool> predicate)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstConstructor: type is null");
				}
				return null;
			}
			if (predicate == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstConstructor: predicate is null");
				}
				return null;
			}
			return type.GetConstructors(AccessTools.allDeclared).FirstOrDefault((ConstructorInfo constructor) => predicate(constructor));
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000E264 File Offset: 0x0000C464
		public static PropertyInfo FirstProperty(Type type, Func<PropertyInfo, bool> predicate)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstProperty: type is null");
				}
				return null;
			}
			if (predicate == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.FirstProperty: predicate is null");
				}
				return null;
			}
			return type.GetProperties(AccessTools.allDeclared).FirstOrDefault((PropertyInfo property) => predicate(property));
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000E2CB File Offset: 0x0000C4CB
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

		// Token: 0x060002BE RID: 702 RVA: 0x0000E304 File Offset: 0x0000C504
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

		// Token: 0x060002BF RID: 703 RVA: 0x0000E394 File Offset: 0x0000C594
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
				fieldRef = AccessTools.FieldRefAccessInternal<T, F>(AccessTools.GetInstanceField(typeFromHandle, fieldName), false);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("FieldRefAccess<{0}, {1}> for {2} caused an exception", typeof(T), typeof(F), fieldName), ex);
			}
			return fieldRef;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000E414 File Offset: 0x0000C614
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
				ptr = AccessTools.FieldRefAccessInternal<T, F>(AccessTools.GetInstanceField(typeFromHandle, fieldName), false)(instance);
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

		// Token: 0x060002C1 RID: 705 RVA: 0x0000E4C8 File Offset: 0x0000C6C8
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
				fieldRef = AccessTools.FieldRefAccessInternal<object, F>(fieldInfo, true);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("FieldRefAccess<{0}> for {1}, {2} caused an exception", typeof(F), type, fieldName), ex);
			}
			return fieldRef;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000E568 File Offset: 0x0000C768
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
						flag = AccessTools.FieldRefNeedsClasscast(typeFromHandle, declaringType);
					}
				}
				fieldRef = AccessTools.FieldRefAccessInternal<T, F>(fieldInfo, flag);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("FieldRefAccess<{0}, {1}> for {2} caused an exception", typeof(T), typeof(F), fieldInfo), ex);
			}
			return fieldRef;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000E618 File Offset: 0x0000C818
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
					flag = AccessTools.FieldRefNeedsClasscast(typeFromHandle, declaringType);
				}
				ptr = AccessTools.FieldRefAccessInternal<T, F>(fieldInfo, flag)(instance);
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

		// Token: 0x060002C4 RID: 708 RVA: 0x0000E704 File Offset: 0x0000C904
		private static AccessTools.FieldRef<T, F> FieldRefAccessInternal<T, F>(FieldInfo fieldInfo, bool needCastclass)
		{
			AccessTools.ValidateFieldType<F>(fieldInfo);
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

		// Token: 0x060002C5 RID: 709 RVA: 0x0000E7CC File Offset: 0x0000C9CC
		public static AccessTools.StructFieldRef<T, F> StructFieldRefAccess<T, F>(string fieldName) where T : struct
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			AccessTools.StructFieldRef<T, F> structFieldRef;
			try
			{
				structFieldRef = AccessTools.StructFieldRefAccessInternal<T, F>(AccessTools.GetInstanceField(typeof(T), fieldName));
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StructFieldRefAccess<{0}, {1}> for {2} caused an exception", typeof(T), typeof(F), fieldName), ex);
			}
			return structFieldRef;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000E838 File Offset: 0x0000CA38
		public static ref F StructFieldRefAccess<T, F>(ref T instance, string fieldName) where T : struct
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			F ptr;
			try
			{
				ptr = AccessTools.StructFieldRefAccessInternal<T, F>(AccessTools.GetInstanceField(typeof(T), fieldName))(ref instance);
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

		// Token: 0x060002C7 RID: 711 RVA: 0x0000E8C8 File Offset: 0x0000CAC8
		public static AccessTools.StructFieldRef<T, F> StructFieldRefAccess<T, F>(FieldInfo fieldInfo) where T : struct
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			AccessTools.StructFieldRef<T, F> structFieldRef;
			try
			{
				AccessTools.ValidateStructField<T, F>(fieldInfo);
				structFieldRef = AccessTools.StructFieldRefAccessInternal<T, F>(fieldInfo);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StructFieldRefAccess<{0}, {1}> for {2} caused an exception", typeof(T), typeof(F), fieldInfo), ex);
			}
			return structFieldRef;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000E92C File Offset: 0x0000CB2C
		public static ref F StructFieldRefAccess<T, F>(ref T instance, FieldInfo fieldInfo) where T : struct
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			F ptr;
			try
			{
				AccessTools.ValidateStructField<T, F>(fieldInfo);
				ptr = AccessTools.StructFieldRefAccessInternal<T, F>(fieldInfo)(ref instance);
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

		// Token: 0x060002C9 RID: 713 RVA: 0x0000E9B4 File Offset: 0x0000CBB4
		private static AccessTools.StructFieldRef<T, F> StructFieldRefAccessInternal<T, F>(FieldInfo fieldInfo) where T : struct
		{
			AccessTools.ValidateFieldType<F>(fieldInfo);
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("__refget_" + typeof(T).Name + "_struct_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[] { typeof(T).MakeByRefType() });
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldflda, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (AccessTools.StructFieldRef<T, F>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(AccessTools.StructFieldRef<T, F>));
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000EA57 File Offset: 0x0000CC57
		public static ref F StaticFieldRefAccess<T, F>(string fieldName)
		{
			return AccessTools.StaticFieldRefAccess<F>(typeof(T), fieldName);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000EA6C File Offset: 0x0000CC6C
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
				ptr = AccessTools.StaticFieldRefAccessInternal<F>(fieldInfo)();
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StaticFieldRefAccess<{0}> for {1}, {2} caused an exception", typeof(F), type, fieldName), ex);
			}
			return ref ptr;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000EACC File Offset: 0x0000CCCC
		public static ref F StaticFieldRefAccess<T, F>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			F ptr;
			try
			{
				ptr = AccessTools.StaticFieldRefAccessInternal<F>(fieldInfo)();
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StaticFieldRefAccess<{0}, {1}> for {2} caused an exception", typeof(T), typeof(F), fieldInfo), ex);
			}
			return ref ptr;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000EB30 File Offset: 0x0000CD30
		public static AccessTools.FieldRef<F> StaticFieldRefAccess<F>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			AccessTools.FieldRef<F> fieldRef;
			try
			{
				fieldRef = AccessTools.StaticFieldRefAccessInternal<F>(fieldInfo);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("StaticFieldRefAccess<{0}> for {1} caused an exception", typeof(F), fieldInfo), ex);
			}
			return fieldRef;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000EB84 File Offset: 0x0000CD84
		private static AccessTools.FieldRef<F> StaticFieldRefAccessInternal<F>(FieldInfo fieldInfo)
		{
			if (!fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must be static");
			}
			AccessTools.ValidateFieldType<F>(fieldInfo);
			string text = "__refget_";
			Type declaringType = fieldInfo.DeclaringType;
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(text + (((declaringType != null) ? declaringType.Name : null) ?? "null") + "_static_fi_" + fieldInfo.Name, typeof(F).MakeByRefType(), new Type[0]);
			ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldsflda, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (AccessTools.FieldRef<F>)dynamicMethodDefinition.Generate().CreateDelegate(typeof(AccessTools.FieldRef<F>));
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000EC29 File Offset: 0x0000CE29
		private static FieldInfo GetInstanceField(Type type, string fieldName)
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

		// Token: 0x060002D0 RID: 720 RVA: 0x0000EC58 File Offset: 0x0000CE58
		private static bool FieldRefNeedsClasscast(Type delegateInstanceType, Type declaringType)
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

		// Token: 0x060002D1 RID: 721 RVA: 0x0000EC90 File Offset: 0x0000CE90
		private static void ValidateStructField<T, F>(FieldInfo fieldInfo) where T : struct
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

		// Token: 0x060002D2 RID: 722 RVA: 0x0000ECC8 File Offset: 0x0000CEC8
		private static void ValidateFieldType<F>(FieldInfo fieldInfo)
		{
			Type fieldType = fieldInfo.FieldType;
			if (fieldType.IsValueType)
			{
				if (typeof(F) != fieldType)
				{
					throw new ArgumentException("FieldRefAccess return type must be the same as FieldType for value types");
				}
			}
			else if (!typeof(F).IsAssignableFrom(fieldType))
			{
				throw new ArgumentException("FieldRefAccess return type must be assignable from FieldType for reference types");
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000ED20 File Offset: 0x0000CF20
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
			if (type.IsInterface && !virtualCall)
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
				if (type.IsInterface && parameterType.IsValueType)
				{
					InterfaceMapping interfaceMap = parameterType.GetInterfaceMap(type);
					method = interfaceMap.TargetMethods[Array.IndexOf<MethodInfo>(interfaceMap.InterfaceMethods, method)];
					type = parameterType;
				}
				if (virtualCall)
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
				if (type.IsValueType)
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
				if (!type.IsInstanceOfType(instance))
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

		// Token: 0x060002D4 RID: 724 RVA: 0x0000F050 File Offset: 0x0000D250
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

		// Token: 0x060002D5 RID: 725 RVA: 0x0000F0C0 File Offset: 0x0000D2C0
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

		// Token: 0x060002D6 RID: 726 RVA: 0x0000F124 File Offset: 0x0000D324
		public static void RethrowException(Exception exception)
		{
			ExceptionDispatchInfo.Capture(exception).Throw();
			throw exception;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000F132 File Offset: 0x0000D332
		public static bool IsMonoRuntime { get; } = Type.GetType("Mono.Runtime") != null;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000F139 File Offset: 0x0000D339
		public static bool IsNetFrameworkRuntime { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000F140 File Offset: 0x0000D340
		public static bool IsNetCoreRuntime { get; }

		// Token: 0x060002DA RID: 730 RVA: 0x0000F148 File Offset: 0x0000D348
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

		// Token: 0x060002DB RID: 731 RVA: 0x0000F1B7 File Offset: 0x0000D3B7
		public static object GetDefaultValue(Type type)
		{
			if (type == null)
			{
				if (Harmony.DEBUG)
				{
					FileLog.Log("AccessTools.GetDefaultValue: type is null");
				}
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

		// Token: 0x060002DC RID: 732 RVA: 0x0000F1F3 File Offset: 0x0000D3F3
		public static object CreateInstance(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.Any, new Type[0], null) != null)
			{
				return Activator.CreateInstance(type);
			}
			return FormatterServices.GetUninitializedObject(type);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000F223 File Offset: 0x0000D423
		public static T MakeDeepCopy<T>(object source) where T : class
		{
			return AccessTools.MakeDeepCopy(source, typeof(T), null, "") as T;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F245 File Offset: 0x0000D445
		public static void MakeDeepCopy<T>(object source, out T result, Func<string, Traverse, Traverse, object> processor = null, string pathRoot = "")
		{
			result = (T)((object)AccessTools.MakeDeepCopy(source, typeof(T), processor, pathRoot));
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000F264 File Offset: 0x0000D464
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

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F560 File Offset: 0x0000D760
		public static bool IsStruct(Type type)
		{
			return type.IsValueType && !AccessTools.IsValue(type) && !AccessTools.IsVoid(type);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F57D File Offset: 0x0000D77D
		public static bool IsClass(Type type)
		{
			return !type.IsValueType;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F588 File Offset: 0x0000D788
		public static bool IsValue(Type type)
		{
			return type.IsPrimitive || type.IsEnum;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F59C File Offset: 0x0000D79C
		public static bool IsInteger(Type type)
		{
			TypeCode typeCode = Type.GetTypeCode(type);
			return typeCode - TypeCode.SByte <= 7;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F5BC File Offset: 0x0000D7BC
		public static bool IsFloatingPoint(Type type)
		{
			TypeCode typeCode = Type.GetTypeCode(type);
			return typeCode - TypeCode.Single <= 2;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F5DA File Offset: 0x0000D7DA
		public static bool IsNumber(Type type)
		{
			return AccessTools.IsInteger(type) || AccessTools.IsFloatingPoint(type);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000F5EC File Offset: 0x0000D7EC
		public static bool IsVoid(Type type)
		{
			return type == typeof(void);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F5FE File Offset: 0x0000D7FE
		public static bool IsOfNullableType<T>(T instance)
		{
			return Nullable.GetUnderlyingType(typeof(T)) != null;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000F614 File Offset: 0x0000D814
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

		// Token: 0x060002E9 RID: 745 RVA: 0x0000F6C2 File Offset: 0x0000D8C2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsStatic(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.IsAbstract && type.IsSealed;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000F6E2 File Offset: 0x0000D8E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsStatic(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}
			return propertyInfo.GetAccessors(true)[0].IsStatic;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000F700 File Offset: 0x0000D900
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool IsStatic(EventInfo eventInfo)
		{
			if (eventInfo == null)
			{
				throw new ArgumentNullException("eventInfo");
			}
			return eventInfo.GetAddMethod(true).IsStatic;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000F71C File Offset: 0x0000D91C
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

		// Token: 0x060002ED RID: 749 RVA: 0x0000F7A4 File Offset: 0x0000D9A4
		// Note: this type is marked as 'beforefieldinit'.
		static AccessTools()
		{
			Type type = AccessTools.TypeByName("System.Runtime.InteropServices.RuntimeInformation");
			AccessTools.IsNetFrameworkRuntime = ((type != null) ? type.GetProperty("FrameworkDescription").GetValue(null, null).ToString()
				.StartsWith(".NET Framework") : (!AccessTools.IsMonoRuntime));
			Type type2 = AccessTools.TypeByName("System.Runtime.InteropServices.RuntimeInformation");
			AccessTools.IsNetCoreRuntime = type2 != null && type2.GetProperty("FrameworkDescription").GetValue(null, null).ToString()
				.StartsWith(".NET Core");
			AccessTools.addHandlerCache = new Dictionary<Type, FastInvokeHandler>();
			AccessTools.addHandlerCacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		}

		// Token: 0x040001A7 RID: 423
		public static readonly BindingFlags all = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

		// Token: 0x040001A8 RID: 424
		public static readonly BindingFlags allDeclared = AccessTools.all | BindingFlags.DeclaredOnly;

		// Token: 0x040001AC RID: 428
		private static readonly Dictionary<Type, FastInvokeHandler> addHandlerCache;

		// Token: 0x040001AD RID: 429
		private static readonly ReaderWriterLockSlim addHandlerCacheLock;

		// Token: 0x02000088 RID: 136
		// (Invoke) Token: 0x060002EF RID: 751
		public unsafe delegate F* FieldRef<T, F>(T instance = default(T));

		// Token: 0x02000089 RID: 137
		// (Invoke) Token: 0x060002F3 RID: 755
		public unsafe delegate F* StructFieldRef<T, F>(ref T instance) where T : struct;

		// Token: 0x0200008A RID: 138
		// (Invoke) Token: 0x060002F7 RID: 759
		public unsafe delegate F* FieldRef<F>();
	}
}
