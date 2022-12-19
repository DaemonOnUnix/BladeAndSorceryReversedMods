using System;

namespace MonoMod.Utils
{
	// Token: 0x02000346 RID: 838
	internal sealed class DynDllMapping
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x0600136C RID: 4972 RVA: 0x00044568 File Offset: 0x00042768
		// (set) Token: 0x0600136D RID: 4973 RVA: 0x00044570 File Offset: 0x00042770
		public string LibraryName { get; set; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x0600136E RID: 4974 RVA: 0x00044579 File Offset: 0x00042779
		// (set) Token: 0x0600136F RID: 4975 RVA: 0x00044581 File Offset: 0x00042781
		public int? Flags { get; set; }

		// Token: 0x06001370 RID: 4976 RVA: 0x0004458A File Offset: 0x0004278A
		public DynDllMapping(string libraryName, int? flags = null)
		{
			if (libraryName == null)
			{
				throw new ArgumentNullException("libraryName");
			}
			this.LibraryName = libraryName;
			this.Flags = flags;
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x000445B0 File Offset: 0x000427B0
		public static implicit operator DynDllMapping(string libraryName)
		{
			return new DynDllMapping(libraryName, null);
		}
	}
}
