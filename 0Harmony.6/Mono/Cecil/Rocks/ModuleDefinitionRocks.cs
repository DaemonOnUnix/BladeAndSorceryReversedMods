using System;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000327 RID: 807
	public static class ModuleDefinitionRocks
	{
		// Token: 0x060012D3 RID: 4819 RVA: 0x0003FDFA File Offset: 0x0003DFFA
		public static IEnumerable<TypeDefinition> GetAllTypes(this ModuleDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			return self.Types.SelectMany(Functional.Y<TypeDefinition, IEnumerable<TypeDefinition>>((Func<TypeDefinition, IEnumerable<TypeDefinition>> f) => (TypeDefinition type) => type.NestedTypes.SelectMany(f).Prepend(type)));
		}
	}
}
