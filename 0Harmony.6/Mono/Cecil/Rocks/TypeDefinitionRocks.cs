using System;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200032C RID: 812
	public static class TypeDefinitionRocks
	{
		// Token: 0x060012E3 RID: 4835 RVA: 0x00040224 File Offset: 0x0003E424
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

		// Token: 0x060012E4 RID: 4836 RVA: 0x00040278 File Offset: 0x0003E478
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

		// Token: 0x060012E5 RID: 4837 RVA: 0x000402C8 File Offset: 0x0003E4C8
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

		// Token: 0x060012E6 RID: 4838 RVA: 0x0004031B File Offset: 0x0003E51B
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
