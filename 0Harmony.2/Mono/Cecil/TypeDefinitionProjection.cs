using System;
using System.Collections.Generic;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200027C RID: 636
	internal sealed class TypeDefinitionProjection
	{
		// Token: 0x06000FEE RID: 4078 RVA: 0x00030D3D File Offset: 0x0002EF3D
		public TypeDefinitionProjection(TypeDefinition type, TypeDefinitionTreatment treatment, Collection<MethodDefinition> redirectedMethods, Collection<KeyValuePair<InterfaceImplementation, InterfaceImplementation>> redirectedInterfaces)
		{
			this.Attributes = type.Attributes;
			this.Name = type.Name;
			this.Treatment = treatment;
			this.RedirectedMethods = redirectedMethods;
			this.RedirectedInterfaces = redirectedInterfaces;
		}

		// Token: 0x040005A3 RID: 1443
		public readonly TypeAttributes Attributes;

		// Token: 0x040005A4 RID: 1444
		public readonly string Name;

		// Token: 0x040005A5 RID: 1445
		public readonly TypeDefinitionTreatment Treatment;

		// Token: 0x040005A6 RID: 1446
		public readonly Collection<MethodDefinition> RedirectedMethods;

		// Token: 0x040005A7 RID: 1447
		public readonly Collection<KeyValuePair<InterfaceImplementation, InterfaceImplementation>> RedirectedInterfaces;
	}
}
