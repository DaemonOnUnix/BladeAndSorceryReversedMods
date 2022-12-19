using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x02000196 RID: 406
	public class Traverse
	{
		// Token: 0x060006A5 RID: 1701 RVA: 0x00016DCB File Offset: 0x00014FCB
		[MethodImpl(MethodImplOptions.Synchronized)]
		static Traverse()
		{
			if (Traverse.Cache == null)
			{
				Traverse.Cache = new AccessCache();
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00016DF3 File Offset: 0x00014FF3
		public static Traverse Create(Type type)
		{
			return new Traverse(type);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00016DFB File Offset: 0x00014FFB
		public static Traverse Create<T>()
		{
			return Traverse.Create(typeof(T));
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00016E0C File Offset: 0x0001500C
		public static Traverse Create(object root)
		{
			return new Traverse(root);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00016E14 File Offset: 0x00015014
		public static Traverse CreateWithType(string name)
		{
			return new Traverse(AccessTools.TypeByName(name));
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00002AED File Offset: 0x00000CED
		private Traverse()
		{
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00016E21 File Offset: 0x00015021
		public Traverse(Type type)
		{
			this._type = type;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00016E30 File Offset: 0x00015030
		public Traverse(object root)
		{
			this._root = root;
			this._type = ((root != null) ? root.GetType() : null);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00016E51 File Offset: 0x00015051
		private Traverse(object root, MemberInfo info, object[] index)
		{
			this._root = root;
			this._type = ((root != null) ? root.GetType() : null) ?? info.GetUnderlyingType();
			this._info = info;
			this._params = index;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00016E8A File Offset: 0x0001508A
		private Traverse(object root, MethodInfo method, object[] parameter)
		{
			this._root = root;
			this._type = method.ReturnType;
			this._method = method;
			this._params = parameter;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00016EB4 File Offset: 0x000150B4
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

		// Token: 0x060006B0 RID: 1712 RVA: 0x00016F58 File Offset: 0x00015158
		public T GetValue<T>()
		{
			object value = this.GetValue();
			if (value == null)
			{
				return default(T);
			}
			return (T)((object)value);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00016F7F File Offset: 0x0001517F
		public object GetValue(params object[] arguments)
		{
			if (this._method == null)
			{
				throw new Exception("cannot get method value without method");
			}
			return this._method.Invoke(this._root, arguments);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00016FA6 File Offset: 0x000151A6
		public T GetValue<T>(params object[] arguments)
		{
			if (this._method == null)
			{
				throw new Exception("cannot get method value without method");
			}
			return (T)((object)this._method.Invoke(this._root, arguments));
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00016FD4 File Offset: 0x000151D4
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

		// Token: 0x060006B4 RID: 1716 RVA: 0x00017069 File Offset: 0x00015269
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

		// Token: 0x060006B5 RID: 1717 RVA: 0x000170A8 File Offset: 0x000152A8
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

		// Token: 0x060006B6 RID: 1718 RVA: 0x00017140 File Offset: 0x00015340
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

		// Token: 0x060006B7 RID: 1719 RVA: 0x00017188 File Offset: 0x00015388
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

		// Token: 0x060006B8 RID: 1720 RVA: 0x000171F8 File Offset: 0x000153F8
		public Traverse<T> Field<T>(string name)
		{
			return new Traverse<T>(this.Field(name));
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00017206 File Offset: 0x00015406
		public List<string> Fields()
		{
			return AccessTools.GetFieldNames(this.Resolve()._type);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00017218 File Offset: 0x00015418
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

		// Token: 0x060006BB RID: 1723 RVA: 0x00017272 File Offset: 0x00015472
		public Traverse<T> Property<T>(string name, object[] index = null)
		{
			return new Traverse<T>(this.Property(name, index));
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00017281 File Offset: 0x00015481
		public List<string> Properties()
		{
			return AccessTools.GetPropertyNames(this.Resolve()._type);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00017294 File Offset: 0x00015494
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

		// Token: 0x060006BE RID: 1726 RVA: 0x000172FC File Offset: 0x000154FC
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

		// Token: 0x060006BF RID: 1727 RVA: 0x0001735C File Offset: 0x0001555C
		public List<string> Methods()
		{
			return AccessTools.GetMethodNames(this.Resolve()._type);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001736E File Offset: 0x0001556E
		public bool FieldExists()
		{
			return this._info != null && this._info is FieldInfo;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00017388 File Offset: 0x00015588
		public bool PropertyExists()
		{
			return this._info != null && this._info is PropertyInfo;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000173A2 File Offset: 0x000155A2
		public bool MethodExists()
		{
			return this._method != null;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000173AD File Offset: 0x000155AD
		public bool TypeExists()
		{
			return this._type != null;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x000173B8 File Offset: 0x000155B8
		public static void IterateFields(object source, Action<Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Field(f));
			});
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x000173F8 File Offset: 0x000155F8
		public static void IterateFields(object source, object target, Action<Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Field(f), targetTrv.Field(f));
			});
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00017444 File Offset: 0x00015644
		public static void IterateFields(object source, object target, Action<string, Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetFieldNames(source).ForEach(delegate(string f)
			{
				action(f, sourceTrv.Field(f), targetTrv.Field(f));
			});
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00017490 File Offset: 0x00015690
		public static void IterateProperties(object source, Action<Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Property(f, null));
			});
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000174D0 File Offset: 0x000156D0
		public static void IterateProperties(object source, object target, Action<Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(sourceTrv.Property(f, null), targetTrv.Property(f, null));
			});
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001751C File Offset: 0x0001571C
		public static void IterateProperties(object source, object target, Action<string, Traverse, Traverse> action)
		{
			Traverse sourceTrv = Traverse.Create(source);
			Traverse targetTrv = Traverse.Create(target);
			AccessTools.GetPropertyNames(source).ForEach(delegate(string f)
			{
				action(f, sourceTrv.Property(f, null), targetTrv.Property(f, null));
			});
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00017565 File Offset: 0x00015765
		public override string ToString()
		{
			MethodBase methodBase = this._method ?? this.GetValue();
			if (methodBase == null)
			{
				return null;
			}
			return methodBase.ToString();
		}

		// Token: 0x0400020D RID: 525
		private static readonly AccessCache Cache;

		// Token: 0x0400020E RID: 526
		private readonly Type _type;

		// Token: 0x0400020F RID: 527
		private readonly object _root;

		// Token: 0x04000210 RID: 528
		private readonly MemberInfo _info;

		// Token: 0x04000211 RID: 529
		private readonly MethodBase _method;

		// Token: 0x04000212 RID: 530
		private readonly object[] _params;

		// Token: 0x04000213 RID: 531
		public static Action<Traverse, Traverse> CopyFields = delegate(Traverse from, Traverse to)
		{
			to.SetValue(from.GetValue());
		};
	}
}
