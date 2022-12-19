using System;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000422 RID: 1058
	public static class TypeDefinitionRocks
	{
		// Token: 0x06001652 RID: 5714 RVA: 0x00048168 File Offset: 0x00046368
		public static IEnumerable<MethodDefinition> GetConstructors(this TypeDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (!self.HasMethods)
			{
				return Empty<MethodDefinition>.Array;
			}
			return self.Methods.Where((MethodDefinition method) => method.IsConstructor);
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x000481BC File Offset: 0x000463BC
		public static MethodDefinition GetStaticConstructor(this TypeDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (!self.HasMethods)
			{
				return null;
			}
			return self.GetConstructors().FirstOrDefault((MethodDefinition ctor) => ctor.IsStatic);
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x0004820C File Offset: 0x0004640C
		public static IEnumerable<MethodDefinition> GetMethods(this TypeDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (!self.HasMethods)
			{
				return Empty<MethodDefinition>.Array;
			}
			return self.Methods.Where((MethodDefinition method) => !method.IsConstructor);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0004825F File Offset: 0x0004645F
		public static TypeReference GetEnumUnderlyingType(this TypeDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (!self.IsEnum)
			{
				throw new ArgumentException();
			}
			return self.GetEnumUnderlyingType();
		}
	}
}
