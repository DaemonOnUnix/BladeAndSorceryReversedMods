using System;

namespace MonoMod.Utils
{
	// Token: 0x0200043E RID: 1086
	internal sealed class DynDllMapping
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060016EB RID: 5867 RVA: 0x0004C7B8 File Offset: 0x0004A9B8
		// (set) Token: 0x060016EC RID: 5868 RVA: 0x0004C7C0 File Offset: 0x0004A9C0
		public string LibraryName { get; set; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x0004C7C9 File Offset: 0x0004A9C9
		// (set) Token: 0x060016EE RID: 5870 RVA: 0x0004C7D1 File Offset: 0x0004A9D1
		public int? Flags { get; set; }

		// Token: 0x060016EF RID: 5871 RVA: 0x0004C7DA File Offset: 0x0004A9DA
		public DynDllMapping(string libraryName, int? flags = null)
		{
			if (libraryName == null)
			{
				throw new ArgumentNullException("libraryName");
			}
			this.LibraryName = libraryName;
			this.Flags = flags;
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x0004C800 File Offset: 0x0004AA00
		public static implicit operator DynDllMapping(string libraryName)
		{
			return new DynDllMapping(libraryName, null);
		}
	}
}
