using System;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000326 RID: 806
	internal static class MethodDefinitionRocks
	{
		// Token: 0x060012CF RID: 4815 RVA: 0x0003FD44 File Offset: 0x0003DF44
		public static MethodDefinition GetBaseMethod(this MethodDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (!self.IsVirtual)
			{
				return self;
			}
			if (self.IsNewSlot)
			{
				return self;
			}
			for (TypeDefinition typeDefinition = MethodDefinitionRocks.ResolveBaseType(self.DeclaringType); typeDefinition != null; typeDefinition = MethodDefinitionRocks.ResolveBaseType(typeDefinition))
			{
				MethodDefinition matchingMethod = MethodDefinitionRocks.GetMatchingMethod(typeDefinition, self);
				if (matchingMethod != null)
				{
					return matchingMethod;
				}
			}
			return self;
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0003FD9C File Offset: 0x0003DF9C
		public static MethodDefinition GetOriginalBaseMethod(this MethodDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			for (;;)
			{
				MethodDefinition baseMethod = self.GetBaseMethod();
				if (baseMethod == self)
				{
					break;
				}
				self = baseMethod;
			}
			return self;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0003FDC8 File Offset: 0x0003DFC8
		private static TypeDefinition ResolveBaseType(TypeDefinition type)
		{
			if (type == null)
			{
				return null;
			}
			TypeReference baseType = type.BaseType;
			if (baseType == null)
			{
				return null;
			}
			return baseType.Resolve();
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0003FDEC File Offset: 0x0003DFEC
		private static MethodDefinition GetMatchingMethod(TypeDefinition type, MethodDefinition method)
		{
			return MetadataResolver.GetMethod(type.Methods, method);
		}
	}
}
