using System;
using System.Collections.Generic;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200031D RID: 797
	internal static class Functional
	{
		// Token: 0x06001292 RID: 4754 RVA: 0x0003EA20 File Offset: 0x0003CC20
		public static Func<A, R> Y<A, R>(Func<Func<A, R>, Func<A, R>> f)
		{
			Func<A, R> g = null;
			g = f((A a) => g(a));
			return g;
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x0003EA58 File Offset: 0x0003CC58
		public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource element)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Functional.PrependIterator<TSource>(source, element);
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0003EA6F File Offset: 0x0003CC6F
		private static IEnumerable<TSource> PrependIterator<TSource>(IEnumerable<TSource> source, TSource element)
		{
			yield return element;
			foreach (TSource tsource in source)
			{
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}
	}
}
