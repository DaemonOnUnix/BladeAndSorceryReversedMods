using System;
using System.Collections.Generic;
using System.Linq;

namespace HarmonyLib
{
	// Token: 0x0200018F RID: 399
	public static class CollectionExtensions
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x00016340 File Offset: 0x00014540
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

		// Token: 0x06000681 RID: 1665 RVA: 0x0001636E File Offset: 0x0001456E
		public static void DoIf<T>(this IEnumerable<T> sequence, Func<T, bool> condition, Action<T> action)
		{
			sequence.Where(condition).Do(action);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001637D File Offset: 0x0001457D
		public static IEnumerable<T> AddItem<T>(this IEnumerable<T> sequence, T item)
		{
			return (sequence ?? Enumerable.Empty<T>()).Concat(new T[] { item });
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001639C File Offset: 0x0001459C
		public static T[] AddToArray<T>(this T[] sequence, T item)
		{
			return sequence.AddItem(item).ToArray<T>();
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x000163AC File Offset: 0x000145AC
		public static T[] AddRangeToArray<T>(this T[] sequence, T[] items)
		{
			return (sequence ?? Enumerable.Empty<T>()).Concat(items).ToArray<T>();
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000163D0 File Offset: 0x000145D0
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

		// Token: 0x06000686 RID: 1670 RVA: 0x00016488 File Offset: 0x00014688
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
