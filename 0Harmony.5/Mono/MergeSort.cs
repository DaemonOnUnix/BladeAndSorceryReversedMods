using System;
using System.Collections.Generic;

namespace Mono
{
	// Token: 0x020001A5 RID: 421
	internal class MergeSort<T>
	{
		// Token: 0x060006E0 RID: 1760 RVA: 0x000176CE File Offset: 0x000158CE
		private MergeSort(T[] elements, IComparer<T> comparer)
		{
			this.elements = elements;
			this.buffer = new T[elements.Length];
			Array.Copy(this.elements, this.buffer, elements.Length);
			this.comparer = comparer;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00017706 File Offset: 0x00015906
		public static void Sort(T[] source, IComparer<T> comparer)
		{
			MergeSort<T>.Sort(source, 0, source.Length, comparer);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00017713 File Offset: 0x00015913
		public static void Sort(T[] source, int start, int length, IComparer<T> comparer)
		{
			new MergeSort<T>(source, comparer).Sort(start, length);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00017723 File Offset: 0x00015923
		private void Sort(int start, int length)
		{
			this.TopDownSplitMerge(this.buffer, this.elements, start, length);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001773C File Offset: 0x0001593C
		private void TopDownSplitMerge(T[] a, T[] b, int start, int end)
		{
			if (end - start < 2)
			{
				return;
			}
			int num = (end + start) / 2;
			this.TopDownSplitMerge(b, a, start, num);
			this.TopDownSplitMerge(b, a, num, end);
			this.TopDownMerge(a, b, start, num, end);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001777C File Offset: 0x0001597C
		private void TopDownMerge(T[] a, T[] b, int start, int middle, int end)
		{
			int num = start;
			int num2 = middle;
			for (int i = start; i < end; i++)
			{
				if (num < middle && (num2 >= end || this.comparer.Compare(a[num], a[num2]) <= 0))
				{
					b[i] = a[num++];
				}
				else
				{
					b[i] = a[num2++];
				}
			}
		}

		// Token: 0x0400022B RID: 555
		private readonly T[] elements;

		// Token: 0x0400022C RID: 556
		private readonly T[] buffer;

		// Token: 0x0400022D RID: 557
		private readonly IComparer<T> comparer;
	}
}
