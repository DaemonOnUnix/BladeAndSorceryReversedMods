using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mono.Cecil
{
	// Token: 0x020000FF RID: 255
	[Serializable]
	public sealed class AssemblyResolutionException : FileNotFoundException
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0001F6FD File Offset: 0x0001D8FD
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001F705 File Offset: 0x0001D905
		public AssemblyResolutionException(AssemblyNameReference reference)
			: this(reference, null)
		{
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001F70F File Offset: 0x0001D90F
		public AssemblyResolutionException(AssemblyNameReference reference, Exception innerException)
			: base(string.Format("Failed to resolve assembly: '{0}'", reference), innerException)
		{
			this.reference = reference;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001F72A File Offset: 0x0001D92A
		private AssemblyResolutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0400029F RID: 671
		private readonly AssemblyNameReference reference;
	}
}
