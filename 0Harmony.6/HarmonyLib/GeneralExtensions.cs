using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HarmonyLib
{
	// Token: 0x0200009A RID: 154
	public static class GeneralExtensions
	{
		// Token: 0x06000326 RID: 806 RVA: 0x0000FAA8 File Offset: 0x0000DCA8
		public static string Join<T>(this IEnumerable<T> enumeration, Func<T, string> converter = null, string delimiter = ", ")
		{
			if (converter == null)
			{
				converter = (T t) => t.ToString();
			}
			return enumeration.Aggregate("", (string prev, T curr) => prev + ((prev.Length > 0) ? delimiter : "") + converter(curr));
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000FB10 File Offset: 0x0000DD10
		public static string Description(this Type[] parameters)
		{
			if (parameters == null)
			{
				return "NULL";
			}
			return "(" + parameters.Join((Type p) => p.FullDescription(), ", ") + ")";
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000FB60 File Offset: 0x0000DD60
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

		// Token: 0x06000329 RID: 809 RVA: 0x0000FC00 File Offset: 0x0000DE00
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

		// Token: 0x0600032A RID: 810 RVA: 0x0000FCED File Offset: 0x0000DEED
		public static Type[] Types(this ParameterInfo[] pinfo)
		{
			return pinfo.Select((ParameterInfo pi) => pi.ParameterType).ToArray<Type>();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000FD1C File Offset: 0x0000DF1C
		public static T GetValueSafe<S, T>(this Dictionary<S, T> dictionary, S key)
		{
			T t;
			if (dictionary.TryGetValue(key, out t))
			{
				return t;
			}
			return default(T);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000FD40 File Offset: 0x0000DF40
		public static T GetTypedValue<T>(this Dictionary<string, object> dictionary, string key)
		{
			object obj;
			if (dictionary.TryGetValue(key, out obj) && obj is T)
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000FD70 File Offset: 0x0000DF70
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
