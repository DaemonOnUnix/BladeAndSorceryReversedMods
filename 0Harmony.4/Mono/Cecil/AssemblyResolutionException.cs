using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mono.Cecil
{
	// Token: 0x020001F1 RID: 497
	[Serializable]
	public sealed class AssemblyResolutionException : FileNotFoundException
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x0002559D File Offset: 0x0002379D
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x000255A5 File Offset: 0x000237A5
		public AssemblyResolutionException(AssemblyNameReference reference)
			: this(reference, null)
		{
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x000255AF File Offset: 0x000237AF
		public AssemblyResolutionException(AssemblyNameReference reference, Exception innerException)
			: base(string.Format("Failed to resolve assembly: '{0}'", reference), innerException)
		{
			this.reference = reference;
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x000255CA File Offset: 0x000237CA
		private AssemblyResolutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x040002D1 RID: 721
		private readonly AssemblyNameReference reference;
	}
}
