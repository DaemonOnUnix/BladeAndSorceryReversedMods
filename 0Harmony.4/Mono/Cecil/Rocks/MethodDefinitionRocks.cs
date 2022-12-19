using System;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200041C RID: 1052
	internal static class MethodDefinitionRocks
	{
		// Token: 0x0600163E RID: 5694 RVA: 0x00047C8C File Offset: 0x00045E8C
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

		// Token: 0x0600163F RID: 5695 RVA: 0x00047CE4 File Offset: 0x00045EE4
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

		// Token: 0x06001640 RID: 5696 RVA: 0x00047D10 File Offset: 0x00045F10
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

		// Token: 0x06001641 RID: 5697 RVA: 0x00047D34 File Offset: 0x00045F34
		private static MethodDefinition GetMatchingMethod(TypeDefinition type, MethodDefinition method)
		{
			return MetadataResolver.GetMethod(type.Methods, method);
		}
	}
}
