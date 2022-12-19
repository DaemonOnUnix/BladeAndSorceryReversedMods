using System;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200041D RID: 1053
	public static class ModuleDefinitionRocks
	{
		// Token: 0x06001642 RID: 5698 RVA: 0x00047D42 File Offset: 0x00045F42
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
