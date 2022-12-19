using System;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000424 RID: 1060
	internal static class TypeReferenceRocks
	{
		// Token: 0x0600165B RID: 5723 RVA: 0x000482AA File Offset: 0x000464AA
		public static ArrayType MakeArrayType(this TypeReference self)
		{
			return new ArrayType(self);
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x000482B4 File Offset: 0x000464B4
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

		// Token: 0x0600165D RID: 5725 RVA: 0x000482F7 File Offset: 0x000464F7
		public static PointerType MakePointerType(this TypeReference self)
		{
			return new PointerType(self);
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x000482FF File Offset: 0x000464FF
		public static ByReferenceType MakeByReferenceType(this TypeReference self)
		{
			return new ByReferenceType(self);
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00048307 File Offset: 0x00046507
		public static OptionalModifierType MakeOptionalModifierType(this TypeReference self, TypeReference modifierType)
		{
			return new OptionalModifierType(modifierType, self);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00048310 File Offset: 0x00046510
		public static RequiredModifierType MakeRequiredModifierType(this TypeReference self, TypeReference modifierType)
		{
			return new RequiredModifierType(modifierType, self);
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x0004831C File Offset: 0x0004651C
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

		// Token: 0x06001662 RID: 5730 RVA: 0x00048390 File Offset: 0x00046590
		public static PinnedType MakePinnedType(this TypeReference self)
		{
			return new PinnedType(self);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00048398 File Offset: 0x00046598
		public static SentinelType MakeSentinelType(this TypeReference self)
		{
			return new SentinelType(self);
		}
	}
}
