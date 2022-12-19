﻿using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000303 RID: 771
	internal class IntHashTable
	{
		// Token: 0x06001236 RID: 4662 RVA: 0x0003BD00 File Offset: 0x00039F00
		private static int GetPrime(int minSize)
		{
			if (minSize < 0)
			{
				throw new ArgumentException("Arg_HTCapacityOverflow");
			}
			for (int i = 0; i < IntHashTable.primes.Length; i++)
			{
				int num = IntHashTable.primes[i];
				if (num >= minSize)
				{
					return num;
				}
			}
			throw new ArgumentException("Arg_HTCapacityOverflow");
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0003BD46 File Offset: 0x00039F46
		internal IntHashTable()
			: this(0, 100)
		{
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0003BD54 File Offset: 0x00039F54
		internal IntHashTable(int capacity, int loadFactorPerc)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", "ArgumentOutOfRange_NeedNonNegNum");
			}
			if (loadFactorPerc < 10 || loadFactorPerc > 100)
			{
				throw new ArgumentOutOfRangeException("loadFactorPerc", string.Format("ArgumentOutOfRange_IntHashTableLoadFactor", 10, 100));
			}
			this.loadFactorPerc = loadFactorPerc * 72 / 100;
			int prime = IntHashTable.GetPrime(capacity / this.loadFactorPerc);
			this.buckets = new IntHashTable.bucket[prime];
			this.loadsize = this.loadFactorPerc * prime / 100;
			if (this.loadsize >= prime)
			{
				this.loadsize = prime - 1;
			}
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0003BDF4 File Offset: 0x00039FF4
		private static uint InitHash(int key, int hashsize, out uint seed, out uint incr)
		{
			uint num = (uint)(key & int.MaxValue);
			seed = num;
			incr = 1U + ((seed >> 5) + 1U) % (uint)(hashsize - 1);
			return num;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0003BE1B File Offset: 0x0003A01B
		internal void Add(int key, object value)
		{
			this.Insert(key, value, true);
		}

		// Token: 0x17000389 RID: 905
		internal object this[int key]
		{
			get
			{
				if (key < 0)
				{
					throw new ArgumentException("Argument_KeyLessThanZero");
				}
				IntHashTable.bucket[] array = this.buckets;
				uint num2;
				uint num3;
				uint num = IntHashTable.InitHash(key, array.Length, out num2, out num3);
				int num4 = 0;
				IntHashTable.bucket bucket;
				for (;;)
				{
					int num5 = (int)(num2 % (uint)array.Length);
					bucket = array[num5];
					if (bucket.val == null)
					{
						break;
					}
					if ((long)(bucket.hash_coll & 2147483647) == (long)((ulong)num) && key == bucket.key)
					{
						goto Block_4;
					}
					num2 += num3;
					if (bucket.hash_coll >= 0 || ++num4 >= array.Length)
					{
						goto IL_81;
					}
				}
				return null;
				Block_4:
				return bucket.val;
				IL_81:
				return null;
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0003BEB7 File Offset: 0x0003A0B7
		private void expand()
		{
			this.rehash(IntHashTable.GetPrime(1 + this.buckets.Length * 2));
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0003BED0 File Offset: 0x0003A0D0
		private void rehash()
		{
			this.rehash(this.buckets.Length);
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0003BEE0 File Offset: 0x0003A0E0
		private void rehash(int newsize)
		{
			this.occupancy = 0;
			IntHashTable.bucket[] array = new IntHashTable.bucket[newsize];
			for (int i = 0; i < this.buckets.Length; i++)
			{
				IntHashTable.bucket bucket = this.buckets[i];
				if (bucket.val != null)
				{
					this.putEntry(array, bucket.key, bucket.val, bucket.hash_coll & int.MaxValue);
				}
			}
			this.version++;
			this.buckets = array;
			this.loadsize = this.loadFactorPerc * newsize / 100;
			if (this.loadsize >= newsize)
			{
				this.loadsize = newsize - 1;
			}
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0003BF7C File Offset: 0x0003A17C
		private void Insert(int key, object nvalue, bool add)
		{
			if (key < 0)
			{
				throw new ArgumentException("Argument_KeyLessThanZero");
			}
			if (nvalue == null)
			{
				throw new ArgumentNullException("nvalue", "ArgumentNull_Value");
			}
			if (this.count >= this.loadsize)
			{
				this.expand();
			}
			else if (this.occupancy > this.loadsize && this.count > 100)
			{
				this.rehash();
			}
			uint num2;
			uint num3;
			uint num = IntHashTable.InitHash(key, this.buckets.Length, out num2, out num3);
			int num4 = 0;
			int num5 = -1;
			int num6;
			for (;;)
			{
				num6 = (int)(num2 % (uint)this.buckets.Length);
				if (this.buckets[num6].val == null)
				{
					break;
				}
				if ((long)(this.buckets[num6].hash_coll & 2147483647) == (long)((ulong)num) && key == this.buckets[num6].key)
				{
					goto Block_9;
				}
				if (num5 == -1 && this.buckets[num6].hash_coll >= 0)
				{
					IntHashTable.bucket[] array = this.buckets;
					int num7 = num6;
					array[num7].hash_coll = array[num7].hash_coll | int.MinValue;
					this.occupancy++;
				}
				num2 += num3;
				if (++num4 >= this.buckets.Length)
				{
					goto Block_13;
				}
			}
			if (num5 != -1)
			{
				num6 = num5;
			}
			this.buckets[num6].val = nvalue;
			this.buckets[num6].key = key;
			IntHashTable.bucket[] array2 = this.buckets;
			int num8 = num6;
			array2[num8].hash_coll = array2[num8].hash_coll | (int)num;
			this.count++;
			this.version++;
			return;
			Block_9:
			if (add)
			{
				throw new ArgumentException("Argument_AddingDuplicate__" + this.buckets[num6].key.ToString());
			}
			this.buckets[num6].val = nvalue;
			this.version++;
			return;
			Block_13:
			if (num5 != -1)
			{
				this.buckets[num5].val = nvalue;
				this.buckets[num5].key = key;
				IntHashTable.bucket[] array3 = this.buckets;
				int num9 = num5;
				array3[num9].hash_coll = array3[num9].hash_coll | (int)num;
				this.count++;
				this.version++;
				return;
			}
			throw new InvalidOperationException("InvalidOperation_HashInsertFailed");
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0003C1BC File Offset: 0x0003A3BC
		private void putEntry(IntHashTable.bucket[] newBuckets, int key, object nvalue, int hashcode)
		{
			uint num = (uint)hashcode;
			uint num2 = 1U + ((num >> 5) + 1U) % (uint)(newBuckets.Length - 1);
			int num3;
			for (;;)
			{
				num3 = (int)(num % (uint)newBuckets.Length);
				if (newBuckets[num3].val == null)
				{
					break;
				}
				if (newBuckets[num3].hash_coll >= 0)
				{
					int num4 = num3;
					newBuckets[num4].hash_coll = newBuckets[num4].hash_coll | int.MinValue;
					this.occupancy++;
				}
				num += num2;
			}
			newBuckets[num3].val = nvalue;
			newBuckets[num3].key = key;
			int num5 = num3;
			newBuckets[num5].hash_coll = newBuckets[num5].hash_coll | hashcode;
		}

		// Token: 0x04000EEA RID: 3818
		private static readonly int[] primes = new int[]
		{
			3, 7, 11, 17, 23, 29, 37, 47, 59, 71,
			89, 107, 131, 163, 197, 239, 293, 353, 431, 521,
			631, 761, 919, 1103, 1327, 1597, 1931, 2333, 2801, 3371,
			4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591, 17519, 21023,
			25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363,
			156437, 187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403,
			968897, 1162687, 1395263, 1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559,
			5999471, 7199369
		};

		// Token: 0x04000EEB RID: 3819
		private IntHashTable.bucket[] buckets;

		// Token: 0x04000EEC RID: 3820
		private int count;

		// Token: 0x04000EED RID: 3821
		private int occupancy;

		// Token: 0x04000EEE RID: 3822
		private int loadsize;

		// Token: 0x04000EEF RID: 3823
		private int loadFactorPerc;

		// Token: 0x04000EF0 RID: 3824
		private int version;

		// Token: 0x02000304 RID: 772
		private struct bucket
		{
			// Token: 0x04000EF1 RID: 3825
			internal int key;

			// Token: 0x04000EF2 RID: 3826
			internal int hash_coll;

			// Token: 0x04000EF3 RID: 3827
			internal object val;
		}
	}
}
