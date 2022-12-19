using System;
using System.Collections.Generic;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000413 RID: 1043
	internal static class Functional
	{
		// Token: 0x06001601 RID: 5633 RVA: 0x00046968 File Offset: 0x00044B68
		public static Func<A, R> Y<A, R>(Func<Func<A, R>, Func<A, R>> f)
		{
			Func<A, R> g = null;
			g = f((A a) => g(a));
			return g;
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x000469A0 File Offset: 0x00044BA0
		public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource element)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Functional.PrependIterator<TSource>(source, element);
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x000469B7 File Offset: 0x00044BB7
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
