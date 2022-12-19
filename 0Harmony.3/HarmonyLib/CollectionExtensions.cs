using System;
using System.Collections.Generic;
using System.Linq;

namespace HarmonyLib
{
	// Token: 0x0200009F RID: 159
	public static class CollectionExtensions
	{
		// Token: 0x06000354 RID: 852 RVA: 0x00010860 File Offset: 0x0000EA60
		public static void Do<T>(this IEnumerable<T> sequence, Action<T> action)
		{
			if (sequence == null)
			{
				return;
			}
			foreach (T t in sequence)
			{
				action(t);
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0001088E File Offset: 0x0000EA8E
		public static void DoIf<T>(this IEnumerable<T> sequence, Func<T, bool> condition, Action<T> action)
		{
			sequence.Where(condition).Do(action);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0001089D File Offset: 0x0000EA9D
		public static IEnumerable<T> AddItem<T>(this IEnumerable<T> sequence, T item)
		{
			return (sequence ?? Enumerable.Empty<T>()).Concat(new T[] { item });
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000108BC File Offset: 0x0000EABC
		public static T[] AddToArray<T>(this T[] sequence, T item)
		{
			return sequence.AddItem(item).ToArray<T>();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000108CC File Offset: 0x0000EACC
		public static T[] AddRangeToArray<T>(this T[] sequence, T[] items)
		{
			return (sequence ?? Enumerable.Empty<T>()).Concat(items).ToArray<T>();
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000108F0 File Offset: 0x0000EAF0
		internal static Dictionary<K, V> Merge<K, V>(this IEnumerable<KeyValuePair<K, V>> firstDict, params IEnumerable<KeyValuePair<K, V>>[] otherDicts)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			foreach (KeyValuePair<K, V> keyValuePair in firstDict)
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value;
			}
			for (int i = 0; i < otherDicts.Length; i++)
			{
				foreach (KeyValuePair<K, V> keyValuePair2 in otherDicts[i])
				{
					dictionary[keyValuePair2.Key] = keyValuePair2.Value;
				}
			}
			return dictionary;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000109A8 File Offset: 0x0000EBA8
		internal static Dictionary<K, V> TransformKeys<K, V>(this Dictionary<K, V> origDict, Func<K, K> transform)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			foreach (KeyValuePair<K, V> keyValuePair in origDict)
			{
				dictionary.Add(transform(keyValuePair.Key), keyValuePair.Value);
			}
			return dictionary;
		}
	}
}
