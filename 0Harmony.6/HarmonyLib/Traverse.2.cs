using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x020000A4 RID: 164
	public class Traverse
	{
		// Token: 0x0600036F RID: 879 RVA: 0x00010F40 File Offset: 0x0000F140
		[MethodImpl(MethodImplOptions.Synchronized)]
		static Traverse()
		{
			if (Traverse.Cache == null)
			{
				Traverse.Cache = new AccessCache();
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00010F68 File Offset: 0x0000F168
		public static Traverse Create(Type type)
		{
			return new Traverse(type);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00010F70 File Offset: 0x0000F170
		public static Traverse Create<T>()
		{
			return Traverse.Create(typeof(T));
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00010F81 File Offset: 0x0000F181
		public static Traverse Create(object root)
		{
			return new Traverse(root);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00010F89 File Offset: 0x0000F189
		public static Traverse CreateWithType(string name)
		{
			return new Traverse(AccessTools.TypeByName(name));
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00002AED File Offset: 0x00000CED
		private Traverse()
		{
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00010F96 File Offset: 0x0000F196
		public Traverse(Type type)
		{
			this._type = type;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00010FA5 File Offset: 0x0000F1A5
		public Traverse(object root)
		{
			this._root = root;
			this._type = ((root != null) ? root.GetType() : null);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00010FC6 File Offset: 0x0000F1C6
		private Traverse(object root, MemberInfo info, object[] index)
		{
			this._root = root;
			this._type = ((root != null) ? root.GetType() : null) ?? info.GetUnderlyingType();
			this._info = info;
			this._params = index;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00010FFF File Offset: 0x0000F1FF
		private Traverse(object root, MethodInfo method, object[] parameter)
		{
			this._root = root;
			this._type = method.ReturnType;
			this._method = method;
			this._params = parameter;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00011028 File Offset: 0x0000F228
		public object GetValue()
		{
			if (this._info is FieldInfo)
			{
				return ((FieldInfo)this._info).GetValue(this._root);
			}
			if (this._info is PropertyInfo)
			{
				return ((PropertyInfo)this._info).GetValue(this._root, AccessTools.all, null, this._params, CultureInfo.CurrentCulture);
			}
			if (this._method != null)
			{
				return this._method.Invoke(this._root, this._params);
			}
			if (this._root == null && this._type != null)
			{
				return this._type;
			}
			return this._root;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000110CC File Offset: 0x0000F2CC
		public T GetValue<T>()
		{
			object value = this.GetValue();
			if (value == null)
			{
				return default(T);
			}
			return (T)((object)value);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000110F3 File Offset: 0x0000F2F3
		public object GetValue(params object[] arguments)
		{
			if (this._method == null)
			{
				throw new Exception("cannot get method value without method");
			}
			return this._method.Invoke(this._root, arguments);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0001111A File Offset: 0x0000F31A
		public T GetValue<T>(params object[] arguments)
		{
			if (this._method == null)
			{
				throw new Exception("cannot get method value without method");
			}
			return (T)((object)this._method.Invoke(this._root, arguments));
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00011148 File Offset: 0x0000F348
		public Traverse SetValue(object value)
		{
			if (this._info is FieldInfo)
			{
				((FieldInfo)this._info).SetValue(this._root, value, AccessTools.all, null, CultureInfo.CurrentCulture);
			}
			if (this._info is PropertyInfo)
			{
				((PropertyInfo)this._info).SetValue(this._root, value, AccessTools.all, null, this._params, CultureInfo.CurrentCulture);
			}
			if (this._method != null)
			{
				throw new Exception("cannot set value of method " + this._method.FullDescription());
			}
			return this;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000111DD File Offset: 0x0000F3DD
		public Type GetValueType()
		{
			if (this._info is FieldInfo)
			{
				return ((FieldInfo)this._info).FieldType;
			}
			if (this._info is PropertyInfo)
			{
				return ((PropertyInfo)this._info).PropertyType;
			}
			return null;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0001121C File Offset: 0x0000F41C
		private Traverse Resolve()
		{
			if (this._root == null)
			{
				FieldInfo fieldInfo = this._info as FieldInfo;
				if (fieldInfo != null && fieldInfo.IsStatic)
				{
					return new Traverse(this.GetValue());
				}
				PropertyInfo propertyInfo = this._info as PropertyInfo;
				if (propertyInfo != null && propertyInfo.GetGetMethod().IsStatic)
				{
					return new Traverse(this.GetValue());
				}
				if (this._method != null && this._method.IsStatic)
				{
					return new Traverse(this.GetValue());
				}
				if (this._type != null)
				{
					return this;
				}
			}
			return new Traverse(this.GetValue());
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000112B4 File Offset: 0x0000F4B4
		public Traverse Type(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this._type == null)
			{
				return new Traverse();
			}
			Type type = AccessTools.Inner(this._type, name);
			if (type == null)
			{
				return new Traverse();
			}
			return new Traverse(type);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000112FC File Offset: 0x0000F4FC
		public Traverse Field(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			FieldInfo fieldInfo = Traverse.Cache.GetFieldInfo(traverse._type, name, AccessCache.MemberType.Any, false);
			if (fieldInfo == null)
			{
				return new Traverse();
			}
			if (!fieldInfo.IsStatic && traverse._root == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, fieldInfo, null);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0001136C File Offset: 0x0000F56C
		public Traverse<T> Field<T>(string name)
		{
			return new Traverse<T>(this.Field(name));
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001137A File Offset: 0x0000F57A
		public List<string> Fields()
		{
			return AccessTools.GetFieldNames(this.Resolve()._type);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001138C File Offset: 0x0000F58C
		public Traverse Property(string name, object[] index = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			PropertyInfo propertyInfo = Traverse.Cache.GetPropertyInfo(traverse._type, name, AccessCache.MemberType.Any, false);
			if (propertyInfo == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, propertyInfo, index);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000113E6 File Offset: 0x0000F5E6
		public Traverse<T> Property<T>(string name, object[] index = null)
		{
			return new Traverse<T>(this.Property(name, index));
		}

		// Token: 0x06000386 RID: 902 RVA: 0x000113F5 File Offset: 0x0000F5F5
		public List<string> Properties()
		{
			return AccessTools.GetPropertyNames(this.Resolve()._type);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00011408 File Offset: 0x0000F608
		public Traverse Method(string name, params object[] arguments)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			Type[] types = AccessTools.GetTypes(arguments);
			MethodBase methodInfo = Traverse.Cache.GetMethodInfo(traverse._type, name, types, AccessCache.MemberType.Any, false);
			if (methodInfo == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, (MethodInfo)methodInfo, arguments);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00011470 File Offset: 0x0000F670
		public Traverse Method(string name, Type[] paramTypes, object[] arguments = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Traverse traverse = this.Resolve();
			if (traverse._type == null)
			{
				return new Traverse();
			}
			MethodBase methodInfo = Traverse.Cache.GetMethodInfo(traverse._type, name, paramTypes, AccessCache.MemberType.Any, false);
			if (methodInfo == null)
			{
				return new Traverse();
			}
			return new Traverse(traverse._root, (MethodInfo)methodInfo, arguments);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000114D0 File Offset: 0x0000F6D0
		public List<string> Methods()
		{
			return AccessTools.GetMethodNames(this.Resolve()._type);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000114E2 File Offset: 0x0000F6E2
		public bool FieldExists()
		{
			return this._info != null && this._info is FieldInfo;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000114FC File Offset: 0x0000F6FC
		public bool PropertyExists()
		{
			return this._info != null && this._info is PropertyInfo;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00011516 File Offset: 0x0000F716
		public bool MethodExists()
		{
			return this._method != null;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00011521 File Offset: 0x0000F721
		public bool TypeExists()
		{
			return this._type != null;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0001152C File Offset: 0x0000F72C
		public static void IterateFields(object source, Action<Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Field(f));
			});
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0001156C File Offset: 0x0000F76C
		public static void IterateFields(object source, object target, Action<Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Field(f), targetTrv.Field(f));
			});
		}

		// Token: 0x06000390 RID: 912 RVA: 0x000115B8 File Offset: 0x0000F7B8
		public static void IterateFields(object source, object target, Action<string, Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(f, sourceTrv.Field(f), targetTrv.Field(f));
			});
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00011604 File Offset: 0x0000F804
		public static void IterateProperties(object source, Action<Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Property(f, null));
			});
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00011644 File Offset: 0x0000F844
		public static void IterateProperties(object source, object target, Action<Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Property(f, null), targetTrv.Property(f, null));
			});
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00011690 File Offset: 0x0000F890
		public static void IterateProperties(object source, object target, Action<string, Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(f, sourceTrv.Property(f, null), targetTrv.Property(f, null));
			});
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000116D9 File Offset: 0x0000F8D9
		public override string ToString()
		{
			MethodBase methodBase = this._method ?? this.GetValue();
			if (methodBase == null)
			{
				return null;
			}
			return methodBase.ToString();
		}

		// Token: 0x040001DF RID: 479
		private static readonly AccessCache Cache;

		// Token: 0x040001E0 RID: 480
		private readonly Type _type;

		// Token: 0x040001E1 RID: 481
		private readonly object _root;

		// Token: 0x040001E2 RID: 482
		private readonly MemberInfo _info;

		// Token: 0x040001E3 RID: 483
		private readonly MethodBase _method;

		// Token: 0x040001E4 RID: 484
		private readonly object[] _params;

		// Token: 0x040001E5 RID: 485
		public static Action<Traverse, Traverse> CopyFields = delegate(Traverse from, Traverse to)
		{
			to.SetValue(from.GetValue());
		};
	}
}
