using System;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200032E RID: 814
	internal static class TypeReferenceRocks
	{
		// Token: 0x060012EC RID: 4844 RVA: 0x00040366 File Offset: 0x0003E566
		public static ArrayType MakeArrayType(this TypeReference self)
		{
			return new ArrayType(self);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00040370 File Offset: 0x0003E570
		public static ArrayType MakeArrayType(this TypeReference self, int rank)
		{
			if (rank == 0)
			{
				throw new ArgumentOutOfRangeException("rank");
			}
			ArrayType arrayType = new ArrayType(self);
			for (int i = 1; i < rank; i++)
			{
				arrayType.Dimensions.Add(default(ArrayDimension));
			}
			return arrayType;
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x000403B3 File Offset: 0x0003E5B3
		public static PointerType MakePointerType(this TypeReference self)
		{
			return new PointerType(self);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x000403BB File Offset: 0x0003E5BB
		public static ByReferenceType MakeByReferenceType(this TypeReference self)
		{
			return new ByReferenceType(self);
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x000403C3 File Offset: 0x0003E5C3
		public static OptionalModifierType MakeOptionalModifierType(this TypeReference self, TypeReference modifierType)
		{
			return new OptionalModifierType(modifierType, self);
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x000403CC File Offset: 0x0003E5CC
		public static RequiredModifierType MakeRequiredModifierType(this TypeReference self, TypeReference modifierType)
		{
			return new RequiredModifierType(modifierType, self);
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x000403D8 File Offset: 0x0003E5D8
		public static GenericInstanceType MakeGenericInstanceType(this TypeReference self, params TypeReference[] arguments)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
			if (arguments.Length == 0)
			{
				throw new ArgumentException();
			}
			if (self.GenericParameters.Count != arguments.Length)
			{
				throw new ArgumentException();
			}
			GenericInstanceType genericInstanceType = new GenericInstanceType(self, arguments.Length);
			foreach (TypeReference typeReference in arguments)
			{
				genericInstanceType.GenericArguments.Add(typeReference);
			}
			return genericInstanceType;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0004044C File Offset: 0x0003E64C
		public static PinnedType MakePinnedType(this TypeReference self)
		{
			return new PinnedType(self);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00040454 File Offset: 0x0003E654
		public static SentinelType MakeSentinelType(this TypeReference self)
		{
			return new SentinelType(self);
		}
	}
}
