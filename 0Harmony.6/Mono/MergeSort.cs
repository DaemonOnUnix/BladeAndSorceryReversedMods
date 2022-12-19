using System;
using System.Collections.Generic;

namespace Mono
{
	// Token: 0x020000B3 RID: 179
	internal class MergeSort<T>
	{
		// Token: 0x060003AA RID: 938 RVA: 0x00011842 File Offset: 0x0000FA42
		private MergeSort(T[] elements, IComparer<T> comparer)
		{
			this.elements = elements;
			this.buffer = new T[elements.Length];
			Array.Copy(this.elements, this.buffer, elements.Length);
			this.comparer = comparer;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001187A File Offset: 0x0000FA7A
		public static void Sort(T[] source, IComparer<T> comparer)
		{
			MergeSort<T>.Sort(source, 0, source.Length, comparer);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00011887 File Offset: 0x0000FA87
		public static void Sort(T[] source, int start, int length, IComparer<T> comparer)
		{
			new MergeSort<T>(source, comparer).Sort(start, length);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00011897 File Offset: 0x0000FA97
		private void Sort(int start, int length)
		{
			this.TopDownSplitMerge(this.buffer, this.elements, start, length);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x000118B0 File Offset: 0x0000FAB0
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

		// Token: 0x060003AF RID: 943 RVA: 0x000118F0 File Offset: 0x0000FAF0
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

		// Token: 0x040001FD RID: 509
		private readonly T[] elements;

		// Token: 0x040001FE RID: 510
		private readonly T[] buffer;

		// Token: 0x040001FF RID: 511
		private readonly IComparer<T> comparer;
	}
}
