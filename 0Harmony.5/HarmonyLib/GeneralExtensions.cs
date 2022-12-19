﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HarmonyLib
{
	// Token: 0x0200018A RID: 394
	public static class GeneralExtensions
	{
		// Token: 0x06000651 RID: 1617 RVA: 0x0001557C File Offset: 0x0001377C
		public static string Join<T>(this IEnumerable<T> enumeration, Func<T, string> converter = null, string delimiter = ", ")
		{
			if (converter == null)
			{
				converter = (T t) => t.ToString();
			}
			return enumeration.Aggregate("", (string prev, T curr) => prev + ((prev.Length > 0) ? delimiter : "") + converter(curr));
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x000155E4 File Offset: 0x000137E4
		public static string Description(this Type[] parameters)
		{
			if (parameters == null)
			{
				return "NULL";
			}
			return "(" + parameters.Join((Type p) => p.FullDescription(), ", ") + ")";
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00015634 File Offset: 0x00013834
		public static string FullDescription(this Type type)
		{
			if (type == null)
			{
				return "null";
			}
			string text = type.Namespace;
			if (!string.IsNullOrEmpty(text))
			{
				text += ".";
			}
			string text2 = text + type.Name;
			if (type.IsGenericType)
			{
				text2 += "<";
				Type[] genericArguments = type.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (!text2.EndsWith("<", StringComparison.Ordinal))
					{
						text2 += ", ";
					}
					text2 += genericArguments[i].FullDescription();
				}
				text2 += ">";
			}
			return text2;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000156D4 File Offset: 0x000138D4
		public static string FullDescription(this MethodBase member)
		{
			if (member == null)
			{
				return "null";
			}
			Type returnedType = AccessTools.GetReturnedType(member);
			StringBuilder stringBuilder = new StringBuilder();
			if (member.IsStatic)
			{
				stringBuilder.Append("static ");
			}
			if (member.IsAbstract)
			{
				stringBuilder.Append("abstract ");
			}
			if (member.IsVirtual)
			{
				stringBuilder.Append("virtual ");
			}
			stringBuilder.Append(returnedType.FullDescription() + " ");
			if (member.DeclaringType != null)
			{
				stringBuilder.Append(member.DeclaringType.FullDescription() + "::");
			}
			string text = member.GetParameters().Join((ParameterInfo p) => p.ParameterType.FullDescription() + " " + p.Name, ", ");
			stringBuilder.Append(member.Name + "(" + text + ")");
			return stringBuilder.ToString();
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000157C1 File Offset: 0x000139C1
		public static Type[] Types(this ParameterInfo[] pinfo)
		{
			return pinfo.Select((ParameterInfo pi) => pi.ParameterType).ToArray<Type>();
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000157F0 File Offset: 0x000139F0
		public static T GetValueSafe<S, T>(this Dictionary<S, T> dictionary, S key)
		{
			T t;
			if (dictionary.TryGetValue(key, out t))
			{
				return t;
			}
			return default(T);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00015814 File Offset: 0x00013A14
		public static T GetTypedValue<T>(this Dictionary<string, object> dictionary, string key)
		{
			object obj;
			if (dictionary.TryGetValue(key, out obj) && obj is T)
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00015844 File Offset: 0x00013A44
		public static string ToLiteral(this string input, string quoteChar = "\"")
		{
			StringBuilder stringBuilder = new StringBuilder(input.Length + 2);
			stringBuilder.Append(quoteChar);
			int i = 0;
			while (i < input.Length)
			{
				char c = input[i];
				if (c <= '"')
				{
					switch (c)
					{
					case '\0':
						stringBuilder.Append("\\0");
						break;
					case '\u0001':
					case '\u0002':
					case '\u0003':
					case '\u0004':
					case '\u0005':
					case '\u0006':
						goto IL_12C;
					case '\a':
						stringBuilder.Append("\\a");
						break;
					case '\b':
						stringBuilder.Append("\\b");
						break;
					case '\t':
						stringBuilder.Append("\\t");
						break;
					case '\n':
						stringBuilder.Append("\\n");
						break;
					case '\v':
						stringBuilder.Append("\\v");
						break;
					case '\f':
						stringBuilder.Append("\\f");
						break;
					case '\r':
						stringBuilder.Append("\\r");
						break;
					default:
						if (c != '"')
						{
							goto IL_12C;
						}
						stringBuilder.Append("\\\"");
						break;
					}
				}
				else if (c != '\'')
				{
					if (c != '\\')
					{
						goto IL_12C;
					}
					stringBuilder.Append("\\\\");
				}
				else
				{
					stringBuilder.Append("\\'");
				}
				IL_162:
				i++;
				continue;
				IL_12C:
				if (c >= ' ' && c <= '~')
				{
					stringBuilder.Append(c);
					goto IL_162;
				}
				stringBuilder.Append("\\u");
				StringBuilder stringBuilder2 = stringBuilder;
				int num = (int)c;
				stringBuilder2.Append(num.ToString("x4"));
				goto IL_162;
			}
			stringBuilder.Append(quoteChar);
			return stringBuilder.ToString();
		}
	}
}
